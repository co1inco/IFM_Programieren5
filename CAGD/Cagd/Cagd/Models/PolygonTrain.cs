using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Cagd;

/// <summary>
/// Represents an ordered sequence of control points (vertices) forming a polygon train / control polygon.
/// Serves as the fundamental domain model for polylines and control polygons in CAGD.
/// </summary>
public class PolygonTrain
{
    private readonly ObservableCollection<Point2D> _vertices = [];

    public PolygonTrain()
    {
        _vertices.CollectionChanged += OnVerticesCollectionChanged;
    }

    public PolygonTrain(IEnumerable<Point2D> points) : this()
    {
        foreach (var point in points)
        {
            _vertices.Add(point);
        }
    }

    public ObservableCollection<Point2D> Vertices => _vertices;

    public int Count => _vertices.Count;

    public Point2D this[int index]
    {
        get => _vertices[index];
        set => _vertices[index] = value;
    }

    public event EventHandler? Changed;

    public void Add(Point2D point)
    {
        _vertices.Add(point);
    }

    public void Insert(int index, Point2D point)
    {
        _vertices.Insert(index, point);
    }

    public bool Remove(Point2D point)
    {
        return _vertices.Remove(point);
    }

    public void RemoveAt(int index)
    {
        _vertices.RemoveAt(index);
    }

    public void Clear()
    {
        _vertices.Clear();
    }

    public void SetPoints(IEnumerable<Point2D> points)
    {
        _vertices.Clear();
        foreach (var point in points)
        {
            _vertices.Add(point);
        }
    }

    public Point2D GetCenter()
    {
        if (_vertices.Count == 0)
            return Point2D.Zero;

        var sum = Vector2D.Zero;
        foreach (var p in _vertices)
        {
            sum += p.ToVector();
        }

        return (sum / (float)_vertices.Count).ToPoint();
    }

    private void OnVerticesCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Changed?.Invoke(this, EventArgs.Empty);
    }
}
