using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;

public partial class IndividualPointsViewModel
{
    private Brush _pointBrush = Brushes.Yellow;
    private float _pointRadius = 0.5f;
    private readonly List<DrawablePoint> _drawablePoints = [];

    public IndividualPointsViewModel()
    {
        Points.CollectionChanged += OnPointsCollectionChanged;
    }
    
    
    public ObservableCollection<ControlPointViewModel> Points { get; } = new();
    
    public DrawableCollection DrawableCollection { get; } = new();


    [RelayCommand]
    private void AddPoint(Point2D? point = null)
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

    [RelayCommand]
    public void ClearPoints()
    {
        Points.Clear();
    }
    
    
    private void OnPointsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        // Update indices and event subscriptions
        if (e.OldItems != null)
        {
            foreach (ControlPointViewModel item in e.OldItems)
            {
                item.PositionChanged -= OnPointPositionChanged;
                item.Removed -= OnPointRemoved;
            }
        }

        if (e.NewItems != null)
        {
            foreach (ControlPointViewModel item in e.NewItems)
            {
                item.PositionChanged += OnPointPositionChanged;
                item.Removed += OnPointRemoved;
            }
        }

        for (int i = 0; i < Points.Count; i++)
        {
            Points[i].Index = i;
        }

        RebuildDrawables();
    }

    private void OnPointRemoved(object? sender, EventArgs e)
    {
        if (sender is ControlPointViewModel pointVm)
            RemovePoint(pointVm);
    }

    private void OnPointPositionChanged(object? sender, Point2D newPos)
    {
        if (sender is not ControlPointViewModel pointVm) return;
        var index = Points.IndexOf(pointVm);
        if (index < 0 || index >= _drawablePoints.Count) return;
    
        // Update point drawable
        _drawablePoints[index].Position = newPos;
    }
    
    private void RebuildDrawables()
    {
        DrawableCollection.Drawables.Clear();
        _drawablePoints.Clear();

        if (Points.Count == 0) return;

        // Create point drawables
        for (int i = 0; i < Points.Count; i++)
        {
            var dp = DrawablePoint.FromCenter(Points[i].Position, _pointRadius, _pointBrush);
            _drawablePoints.Add(dp);
            DrawableCollection.Drawables.Add(dp);
        }
    }
}