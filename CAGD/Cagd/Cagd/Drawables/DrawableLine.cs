using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public class DrawableLine : ObservableObject, IDrawableObject
{
    private Point2D _p1;
    private Point2D _p2;
    private float _thickness;
    private Brush _color;

    public DrawableLine(Point2D p1, Point2D p2, float thickness = 0.1f, Brush? color = null)
    {
        _p1 = p1;
        _p2 = p2;
        _thickness = thickness;
        _color = color ?? Brushes.Black;
    }

    public Point2D Position => Point2D.Zero;
    public Vector2D Offset => Vector2D.Zero;

    public Point2D P1
    {
        get => _p1;
        set => SetProperty(ref _p1, value);
    }

    public Point2D P2
    {
        get => _p2;
        set => SetProperty(ref _p2, value);
    }

    public float Thickness
    {
        get => _thickness;
        set => SetProperty(ref _thickness, value);
    }

    public Brush Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }
}
