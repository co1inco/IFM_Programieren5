using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;

public partial class ControlPointViewModel : ObservableObject
{
    private float _x;
    private float _y;
    private int _index;
    
    // TODO: color is (more or less) passed down through the stages. I think this should be a UI / Xaml thing instead of a ViewModel property?
    private Brush _baseColor;
    private Brush _hoveredColor = Brushes.Black;

    public ControlPointViewModel(float x, float y, int index = 0, float radius = 0.5f, Brush? brush = null)
    {
        _x = x;
        _y = y;
        _index = index;

        _baseColor = brush ?? Brushes.Red;
        DrawablePoint = DrawablePoint.FromCenter(Position, radius, _baseColor);
    }

    public ControlPointViewModel(Point2D point, int index = 0, float radius = 0.5f, Brush? brush = null)
        : this(point.X, point.Y, index, radius, brush)
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
                OnPositionChanged();
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
                OnPositionChanged();
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
            OnPositionChanged();
        }
    }

    public DrawablePoint DrawablePoint { get; private set => SetProperty(ref field, value); }
    
    public event EventHandler<Point2D>? PositionChanged;

    public event EventHandler? Removed;
    
    public Point2D ToPoint() => new(_x, _y);
    
    public bool IsHovered
    {
        get;
        set
        {
            if (SetProperty(ref field, value))
                DrawablePoint.Color = value ? _hoveredColor : _baseColor;
        }
    }
    
    [RelayCommand]
    private void RemovePoint()
    {
        Removed?.Invoke(this, EventArgs.Empty);
    }

    private void OnPositionChanged()
    {
        DrawablePoint.Position = Position;
        PositionChanged?.Invoke(this, Position);
    }
    
}
