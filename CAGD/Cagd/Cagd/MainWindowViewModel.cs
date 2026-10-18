using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;


public interface IDrawableObject
{
    public float X { get; }
    public float Y { get; }
}

public record DrawableRect(float X, float Y, float Width, float Height, Brush Color) : IDrawableObject;

public record DrawablePoint(float X, float Y, float Radius, Brush Color) : IDrawableObject
{
    public static DrawablePoint FromCenter(float x, float y, float radius, Brush color) => 
        new(x - radius/2, y - radius/2, radius, color);
}

public record DrawableLine(float X1, float Y1, float X2, float Y2, float Thickness, Brush Color) : IDrawableObject
{
    public float X => 0;
    public float Y => 0;
}


public partial class MainWindowViewModel : ObservableObject
{

    public Point CanvasResolution { get; set; } = new Point(20, 20);
    public Point CanvasTransform { get; set; } = new Point(10, 10);
    public Point CanvasScale { get; set; } = new Point(1, 1);
    

    public ObservableCollection<IDrawableObject> Drawables { get; } = [
        new DrawableRect(0, 0, 10, 10, Brushes.Red),
        DrawablePoint.FromCenter(0, 0, 5, Brushes.Black),
        new DrawableLine(10, 10, 0, 0, 2, Brushes.Blue),
    ];


    [RelayCommand]
    private void Load()
    {
        var obj = ObjLoader.Load("Resources/Curve_1.obj");

        Drawables.Clear();
        Drawables.Add(DrawablePoint.FromCenter(0, 0, 0.3f, Brushes.Black));

        var center = obj.GetCenter(0);
        
        DrawObj(obj, Brushes.Red, Brushes.LightBlue, -center.ToVector());
        
    }


    private void DrawObj(Obj obj, Brush pointBrush, Brush lineBrush, Vector2D? offset = null)
    {
        offset ??= Vector2D.Zero;
        
        if (obj.Faces is not [var face])
            return;

        foreach (var i in face)
        {
            var p = obj.Points[i] + offset;
            
            Drawables.Add(DrawablePoint.FromCenter(p.X, p.Y, 0.5f, pointBrush));

            var nextIndex = i+1;
            if (nextIndex < obj.Points.Length)
            {
                var np = obj.Points[nextIndex] + offset;
                Drawables.Add(new DrawableLine(p.X, p.Y, np.X, np.Y, 0.1f, lineBrush));
            }
        }
    }
    
}