using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;


public interface IDrawableObject
{
    Point2D Position { get; }
    Vector2D Offset { get; }
}

public class DrawableRect : ObservableObject, IDrawableObject
{
    public DrawableRect(Point2D position, float width, float height, Brush? color = null)
    {
        Position = position;
        Width = width;
        Height = height;
        Color = color ?? Brushes.Black;
    }
    
    public Point2D Position { get; set => SetProperty(ref field, value); }
    public Vector2D Offset => Vector2D.Zero; 
    public float Width { get; set => SetProperty(ref field, value); }
    public float Height { get; set => SetProperty(ref field, value); }
    
    public Brush Color { get; set => SetProperty(ref field, value); }
}

public class DrawablePoint : ObservableObject, IDrawableObject
{
    public static DrawablePoint FromCenter(Point2D position, float radius, Brush color) => new(position, radius, color);
        // position - (new Vector2D(radius, radius) / 2), radius, color);

    public DrawablePoint(Point2D position, float radius, Brush? color = null)
    {
        Position = position;
        Radius = radius;
        Color = color ?? Brushes.Black;
    }
    
    public Point2D Position { get; set => SetProperty(ref field, value); }
    // public Vector2D Offset => new Vector2D(Radius, Radius) / 2;
    // public Vector2D Offset => new Vector2D(5,5);
    public Vector2D Offset => Vector2D.Zero;
    public float Radius { get; set => SetProperty(ref field, value); }
    public Brush Color { get; set => SetProperty(ref field, value); }
}

public class DrawableLine : ObservableObject, IDrawableObject
{
    public DrawableLine(Point2D p1, Point2D p2, float thickness, Brush? color = null)
    {
        P1 = p1;
        P2 = p2;
        Thickness = thickness;
        Color = color ?? Brushes.Black;
    }
    
    public Point2D Position => Point2D.Zero;
    public Vector2D Offset => Vector2D.Zero;
    public Point2D P1 { get; set => SetProperty(ref field, value); }
    public Point2D P2 { get; set => SetProperty(ref field, value); }
    public float Thickness { get; set => SetProperty(ref field, value); }
    public Brush Color { get; set => SetProperty(ref field, value); }
}

public class DrawableCollection : ObservableObject, IDrawableObject
{
    public DrawableCollection()
    {
        
    }

    public DrawableCollection(IEnumerable<IDrawableObject> drawables)
    {
        Drawables = [..drawables];
    }
    
    public Point2D Position { get; set => SetProperty(ref field, value); } = Point2D.Zero;
    public Vector2D Offset => Vector2D.Zero;

    public ObservableCollection<IDrawableObject> Drawables
    {
        get;
        private set => SetProperty(ref field, value);
    } = [];
    
    public void SetCollection(IEnumerable<IDrawableObject> drawables)
    {
        Drawables = [..drawables];
    }
}

public class ObjectManager
{
    private readonly ObservableCollection<IDrawableObject> _drawables;
    
    private PointData[] _modifyPoints = [];

    public ObjectManager(ObservableCollection<IDrawableObject> drawables, IEnumerable<Point2D> points)
    {
        _drawables = drawables;
        Points = [];
        GenerateDrawables(points);
    }
    
    public ObservableCollection<ModifyPoint> Points { get; }
    
    private void UpdatePoints(Point2D point, PointData data)
    {
        
    }

    private void GenerateDrawables(IEnumerable<Point2D> points)
    {
        var pointsArray = points.ToArray();
        
        List<PointData> pointData = [];

        foreach (var i in pointsArray)
        {
            var p = DrawablePoint.FromCenter(i, 0.5f, Brushes.Red);
            _drawables.Add(p);
            var data = new PointData { P = p };
            
            pointData.Add(data);
            Points.Add(new ModifyPoint(i, pNew => UpdatePoints(pNew, data)));
        }

        for (int i = 0; i < (pointData.Count - 1); i++)
        {
            var current = pointData[i];
            var next = pointData[i + 1];

            var line = new DrawableLine(current.P.Position, next.P.Position, 0.1f, Brushes.LightBlue);
            current.Next = line;
            next.Prev = line;
            
            _drawables.Add(line);
        }
        
        _modifyPoints = pointData.ToArray();
    }

    private record PointData
    {
        public required DrawablePoint P { get; set; } 
        public DrawableLine? Prev { get; set; }
        public DrawableLine? Next { get; set; }
    }

    public class ModifyPoint(Point2D point, Action<Point2D> onChange)
    {
        public float X
        {
            get;
            set
            {
                field = value;
                onChange(new Point2D(X, Y));
            }
        } = point.X;

        public float Y
        {
            get;
            set
            {
                field = value;
                onChange(new Point2D(X, Y));
            }
        } = point.Y;
    }
}



public partial class MainWindowViewModel : ObservableObject
{

    public Point CanvasResolution { get; set; } = new Point(20, 20);
    public Point CanvasTransform { get; set; } = new Point(10, 10);
    public Point CanvasScale { get; set; } = new Point(1, 1);
    

    public ObservableCollection<IDrawableObject> Drawables { get; } = [
        // new DrawableRect(Point2D.Zero, 10, 10, Brushes.Red),
        // DrawablePoint.FromCenter(Point2D.Zero, 5, Brushes.Black),
        // new DrawableLine(Point2D.Zero, new Point2D(10, 10),2, Brushes.Blue),
    ];


    public ObjectManager BezieObject
    {
        get;
        private set => SetProperty(ref field, value);
    } 


    [RelayCommand]
    private void Load()
    {
        var obj = ObjLoader.Load("Resources/Curve_1.obj");

        Drawables.Clear();
        Drawables.Add(DrawablePoint.FromCenter(Point2D.Zero, 0.3f, Brushes.Black));

        var center = obj.GetCenter(0);

        BezieObject = new ObjectManager(Drawables, obj.Faces[0].Select(x => obj.Points[x]));
        
        // var collection = DrawObj(obj, Brushes.Red, Brushes.LightBlue, -center.ToVector());
        // foreach (var d in collection.Drawables) Drawables.Add(d);
    }


    private DrawableCollection DrawObj(Obj obj, Brush pointBrush, Brush lineBrush, Vector2D? offset = null)
    {
        offset ??= Vector2D.Zero;
        
        if (obj.Faces is not [var face])
            return new DrawableCollection();

        List<IDrawableObject> drawables = [];
        
        foreach (var i in face)
        {
            var p = obj.Points[i] + offset;
            
            drawables.Add(DrawablePoint.FromCenter(p, 0.5f, pointBrush));

            var nextIndex = i+1;
            if (nextIndex < obj.Points.Length)
            {
                var np = obj.Points[nextIndex] + offset;
                drawables.Add(new DrawableLine(p, np, 0.1f, lineBrush));
            }
        }
        
        return new DrawableCollection(drawables);
    }
    
}