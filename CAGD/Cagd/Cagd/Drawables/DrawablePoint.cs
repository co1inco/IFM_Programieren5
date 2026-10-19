using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public class DrawablePoint : ObservableObject, IDrawableObject
{
    private Point2D _position;
    private float _radius;
    private Brush _color;

    public DrawablePoint(Point2D position, float radius = 0.5f, Brush? color = null)
    {
        _position = position;
        _radius = radius;
        _color = color ?? Brushes.Black;
    }

    public static DrawablePoint FromCenter(Point2D position, float radius, Brush? color = null) =>
        new(position, radius, color);

    public Point2D Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }

    public float Radius
    {
        get => _radius;
        set
        {
            if (SetProperty(ref _radius, value))
            {
                OnPropertyChanged(nameof(OffsetX));
                OnPropertyChanged(nameof(OffsetY));
            }
        }
    }

    public Brush Color
    {
        get => _color;
        set => SetProperty(ref _color, value);
    }

    public Vector2D Offset => new(-Radius / 2f, -Radius / 2f);

    public float OffsetX => -Radius / 2f;
    public float OffsetY => -Radius / 2f;
}
