using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public class ControlPointViewModel : ObservableObject
{
    private float _x;
    private float _y;
    private int _index;

    public ControlPointViewModel(float x, float y, int index = 0)
    {
        _x = x;
        _y = y;
        _index = index;
    }

    public ControlPointViewModel(Point2D point, int index = 0)
        : this(point.X, point.Y, index)
    {
    }

    public int Index
    {
        get => _index;
        set
        {
            if (SetProperty(ref _index, value))
            {
                OnPropertyChanged(nameof(DisplayName));
            }
        }
    }

    public string DisplayName => $"P{Index}";

    public float X
    {
        get => _x;
        set
        {
            if (SetProperty(ref _x, value))
            {
                OnPropertyChanged(nameof(Position));
                PositionChanged?.Invoke(this, Position);
            }
        }
    }

    public float Y
    {
        get => _y;
        set
        {
            if (SetProperty(ref _y, value))
            {
                OnPropertyChanged(nameof(Position));
                PositionChanged?.Invoke(this, Position);
            }
        }
    }

    public Point2D Position
    {
        get => new(_x, _y);
        set
        {
            _x = value.X;
            _y = value.Y;
            OnPropertyChanged(nameof(X));
            OnPropertyChanged(nameof(Y));
            OnPropertyChanged(nameof(Position));
            PositionChanged?.Invoke(this, Position);
        }
    }

    public event EventHandler<Point2D>? PositionChanged;

    public Point2D ToPoint() => new(_x, _y);
}
