using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public class DrawableRect : ObservableObject, IDrawableObject
{
    private Point2D _position;
    private float _width;
    private float _height;
    private Brush _color;

    public DrawableRect(Point2D position, float width, float height, Brush? color = null)
    {
        _position = position;
        _width = width;
        _height = height;
        _color = color ?? Brushes.Black;
    }

    public Point2D Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }

    public Vector2D Offset => Vector2D.Zero;

    public float Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    public float Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }

    public Brush Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }
}
