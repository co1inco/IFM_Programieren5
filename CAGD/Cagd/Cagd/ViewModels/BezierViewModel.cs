using System.Windows.Media;
using Cagd.Helper;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public partial class BezierViewModel : ObservableObject
{
    private PolygonTrain? _polygonTrain;
    private readonly List<DrawablePoint> _points = new();
    private readonly DrawableCollection _pointCollection = new();
    private readonly List<DrawableLine> _lines = new();
    private readonly DrawableCollection _lineCollection = new();

    public BezierViewModel()
    {
        // DrawableCollection.Drawables.Add(_pointCollection);
        // DrawableCollection.Drawables.Add(_lineCollection);
    }
    
    public bool ShowSegment
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
            {
                if (value)
                {
                    if (!DrawableCollection.Drawables.Contains(_lineCollection))
                        DrawableCollection.Drawables.Add(_lineCollection);
                    if (!DrawableCollection.Drawables.Contains(_pointCollection))
                        DrawableCollection.Drawables.Add(_pointCollection);
                }
                else
                {
                    DrawableCollection.Drawables.Remove(_pointCollection);
                    DrawableCollection.Drawables.Remove(_lineCollection);
                }
            }
        }
    }

    public float CurveParameter
    {
        get;
        set
        {
            if (value < 0) value = 0;
            if (value > 1) value = 1;
            if (SetProperty(ref field, value))
            {
                UpdatePoints();
            }
        }
    }

    public Brush Color
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
                RebuildDrawables();
        }
    } = Brushes.Green;


    public DrawableCollection DrawableCollection { get; } = new();


    public void LoadPolygonTrain(PolygonTrain polygonTrain)
    {
        _polygonTrain = polygonTrain;
        RebuildDrawables();
    }


    private void RebuildDrawables()
    {
        _points.Clear();
        _pointCollection.Drawables.Clear();
        _lines.Clear();
        _lineCollection.Drawables.Clear();
        
        if (_polygonTrain is null || _polygonTrain.Count < 2)
            return;
        
        CreateBezierPoints(_polygonTrain.Vertices);
        if (_points.LastOrDefault() is { } p)
        {
            p.Color = Brushes.Yellow;
            p.Radius = 0.6f;
        }
    }

    private void CreateBezierPoints(IEnumerable<Point2D> points)
    {

        List<Point2D> newPoints = [];
        foreach (var (p1, p2) in points.Offset())
        {
            // Add line between points
            // adding them here would draw over the polygons base lines
            // var line = new DrawableLine(p1, p2) { Color = Color, Thickness = 0.1f };
            // _lines.Add(line);
            // _pointCollection.Drawables.Add(line);
            
            // calculate point between points
            var point = (p1.ToVector() * CurveParameter + p2.ToVector() * (1-CurveParameter)).ToPoint();
            newPoints.Add(point);
            
            var drawable = DrawablePoint.FromCenter(point, 0.3f, Color);
            _points.Add(drawable);
            _pointCollection.Drawables.Add(drawable);
        }

        foreach (var (p1, p2) in newPoints.Offset())
        {
            var line = new DrawableLine(p1, p2) { Color = Color, Thickness = 0.07f };
            _lines.Add(line);
            _pointCollection.Drawables.Add(line);
        }
        
        if (newPoints.Count > 1)
            CreateBezierPoints(newPoints);
        
    }
    

    private void UpdatePoints()
    {
        // TODO: Update existing instances instead of redrawing 
        RebuildDrawables();
        // if (_polygonTrain is null || _polygonTrain.Count < 2)
        //     return;
        //
        // for (int i = 0; i < _polygonTrain.Count - 1; i++)
        // {
        //     if (_points.ElementAtOrDefault(i) is not { } d)
        //         continue;
        //     
        //     var point = (_polygonTrain[i].ToVector() * CurveParameter + _polygonTrain[i + 1].ToVector() * (1-CurveParameter)).ToPoint();
        //     d.Position = point;
        // }
    }
    
    
}