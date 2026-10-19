using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;

/// <summary>
/// ViewModel managing a Polygon Train (control polygon) and synchronizing its visual drawables.
/// Replaces the legacy ObjectManager with a clean, reactive, and extensible MVVM design.
/// </summary>
public partial class PolygonTrainViewModel : ObservableObject
{
    private Brush _pointBrush = Brushes.Red;
    private Brush _lineBrush = Brushes.LightBlue;
    private float _pointRadius = 0.5f;
    private float _lineThickness = 0.1f;
    private bool _isClosed;

    // Track corresponding drawable items
    private readonly List<DrawablePoint> _drawablePoints = [];
    private readonly List<DrawableLine> _drawableLines = [];

    public PolygonTrainViewModel()
    {
        Points.CollectionChanged += OnPointsCollectionChanged;
    }

    public PolygonTrainViewModel(IEnumerable<Point2D> points) : this()
    {
        SetPoints(points);
    }

    public ObservableCollection<ControlPointViewModel> Points { get; } = [];

    public DrawableCollection DrawableCollection { get; } = new();

    public Brush PointBrush
    {
        get => _pointBrush;
        set
        {
            if (SetProperty(ref _pointBrush, value))
            {
                foreach (var p in _drawablePoints) p.Color = value;
            }
        }
    }

    public Brush LineBrush
    {
        get => _lineBrush;
        set
        {
            if (SetProperty(ref _lineBrush, value))
            {
                foreach (var l in _drawableLines) l.Color = value;
            }
        }
    }

    public float PointRadius
    {
        get => _pointRadius;
        set
        {
            if (SetProperty(ref _pointRadius, value))
            {
                foreach (var p in _drawablePoints) p.Radius = value;
            }
        }
    }

    public float LineThickness
    {
        get => _lineThickness;
        set
        {
            if (SetProperty(ref _lineThickness, value))
            {
                foreach (var l in _drawableLines) l.Thickness = value;
            }
        }
    }

    public bool IsClosed
    {
        get => _isClosed;
        set
        {
            if (SetProperty(ref _isClosed, value))
            {
                RebuildDrawables();
            }
        }
    }

    public int Count => Points.Count;

    [RelayCommand]
    public void AddPoint(Point2D? point = null)
    {
        var newPt = point ?? (Points.Count > 0 ? Points[^1].Position + new Vector2D(1, 0) : Point2D.Zero);
        var vm = new ControlPointViewModel(newPt, Points.Count);
        Points.Add(vm);
    }

    [RelayCommand]
    public void RemovePoint(ControlPointViewModel? point)
    {
        if (point != null)
        {
            Points.Remove(point);
        }
        else if (Points.Count > 0)
        {
            Points.RemoveAt(Points.Count - 1);
        }
    }

    public void RemovePointAt(int index)
    {
        if (index >= 0 && index < Points.Count)
        {
            Points.RemoveAt(index);
        }
    }

    public void Clear()
    {
        Points.Clear();
    }

    public void SetPoints(IEnumerable<Point2D> points)
    {
        Points.Clear();
        int idx = 0;
        foreach (var p in points)
        {
            Points.Add(new ControlPointViewModel(p, idx++));
        }
    }

    public PolygonTrain ToPolygonTrain()
    {
        return new PolygonTrain(Points.Select(p => p.ToPoint()));
    }

    public void LoadFrom(PolygonTrain polygonTrain)
    {
        SetPoints(polygonTrain.Vertices);
    }

    private void OnPointsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Update indices and event subscriptions
        if (e.OldItems != null)
        {
            foreach (ControlPointViewModel item in e.OldItems)
            {
                item.PositionChanged -= OnPointPositionChanged;
            }
        }

        if (e.NewItems != null)
        {
            foreach (ControlPointViewModel item in e.NewItems)
            {
                item.PositionChanged += OnPointPositionChanged;
            }
        }

        for (int i = 0; i < Points.Count; i++)
        {
            Points[i].Index = i;
        }

        RebuildDrawables();
    }

    private void OnPointPositionChanged(object? sender, Point2D newPos)
    {
        if (sender is not ControlPointViewModel pointVm) return;
        var index = Points.IndexOf(pointVm);
        if (index < 0 || index >= _drawablePoints.Count) return;

        // Update point drawable
        _drawablePoints[index].Position = newPos;

        // Update connected lines
        // Previous line connecting index-1 to index
        if (index > 0 && index - 1 < _drawableLines.Count)
        {
            _drawableLines[index - 1].P2 = newPos;
        }
        else if (_isClosed && index == 0 && _drawableLines.Count >= Points.Count)
        {
            _drawableLines[^1].P2 = newPos;
        }

        // Next line connecting index to index+1
        if (index < _drawableLines.Count && index < Points.Count - 1)
        {
            _drawableLines[index].P1 = newPos;
        }
        else if (_isClosed && index == Points.Count - 1 && _drawableLines.Count >= Points.Count)
        {
            _drawableLines[^1].P1 = newPos;
        }
    }

    private void RebuildDrawables()
    {
        DrawableCollection.Drawables.Clear();
        _drawablePoints.Clear();
        _drawableLines.Clear();

        if (Points.Count == 0) return;

        // Create point drawables
        for (int i = 0; i < Points.Count; i++)
        {
            var dp = DrawablePoint.FromCenter(Points[i].Position, _pointRadius, _pointBrush);
            _drawablePoints.Add(dp);
            DrawableCollection.Drawables.Add(dp);
        }

        // Create line drawables connecting consecutive points
        for (int i = 0; i < Points.Count - 1; i++)
        {
            var line = new DrawableLine(Points[i].Position, Points[i + 1].Position, _lineThickness, _lineBrush);
            _drawableLines.Add(line);
            DrawableCollection.Drawables.Add(line);
        }

        // If closed, add closing line
        if (_isClosed && Points.Count > 2)
        {
            var closingLine = new DrawableLine(Points[^1].Position, Points[0].Position, _lineThickness, _lineBrush);
            _drawableLines.Add(closingLine);
            DrawableCollection.Drawables.Add(closingLine);
        }
    }
}
