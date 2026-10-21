using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Cagd;

public partial class MainWindowViewModel : ObservableObject
{
    private Point2D _canvasResolution = new(20, 20);
    private Point2D _canvasTransform = new(10, 10);
    private Point2D _canvasScale = new(1, 1);

    public MainWindowViewModel()
    {
        PolygonTrain = new PolygonTrainViewModel();
        BezierViewModel = new();
        PolygonTrain.OnTrainChanged += (sender, train) => BezierViewModel.LoadPolygonTrain(train); 

        // Initialize origin marker and drawables
        RebuildAllDrawables();
    }

    public Point2D CanvasResolution
    {
        get => _canvasResolution;
        set => SetProperty(ref _canvasResolution, value);
    }

    public Point2D CanvasTransform
    {
        get => _canvasTransform;
        set => SetProperty(ref _canvasTransform, value);
    }

    public Point2D CanvasScale
    {
        get => _canvasScale;
        set => SetProperty(ref _canvasScale, value);
    }

    public PolygonTrainViewModel PolygonTrain { get; }

    public ObservableCollection<IDrawableObject> Drawables { get; } = [];
    
    public IndividualPointsViewModel IndividualPoints { get; } = new();

    public BezierViewModel BezierViewModel { get; }
    
    [RelayCommand]
    private void Load()
    {
        try
        {
            var path = Path.Combine("Resources", "Curve_1.obj");
            if (!File.Exists(path))
            {
                var appDir = AppDomain.CurrentDomain.BaseDirectory;
                path = Path.Combine(appDir, "Resources", "Curve_1.obj");
            }

            if (!File.Exists(path))
            {
                // Fallback default points if file not found
                PolygonTrain.SetPoints([
                    new Point2D(0.0f, 5.0f),
                    new Point2D(5.0f, 5.0f),
                    new Point2D(5.0f, -5.0f),
                    new Point2D(0.0f, -5.0f)
                ]);
                return;
            }

            var obj = ObjLoader.Load(path);
            if (obj.Faces.Length > 0)
            {
                var facePoints = obj.GetFacePoints(0).ToList();
                PolygonTrain.SetPoints(facePoints);
            }
            else if (obj.Points.Length > 0)
            {
                PolygonTrain.SetPoints(obj.Points);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Failed to load OBJ: {ex.Message}");
        }
    }

    [RelayCommand]
    private void AddPoint()
    {
        PolygonTrain.AddPoint();
    }

    [RelayCommand]
    private void RemovePoint()
    {
        PolygonTrain.RemovePoint(null);
    }

    [RelayCommand]
    private void Clear()
    {
        PolygonTrain.Clear();
    }

    [RelayCommand]
    public void ResetView()
    {
        CanvasResolution = new Point2D(20, 20);
        CanvasTransform = new Point2D(10, 10);
        CanvasScale = new Point2D(1, 1);
    }

    [RelayCommand]
    public void MoveCanvas(Vector2D v)
    {
        CanvasTransform += v;
    }

    public void Zoom(Point2D center, float factor)
    {
        var newScaleX = _canvasScale.X * factor;
        var newScaleY = _canvasScale.Y * factor;

        if (newScaleX < 0.01f || newScaleX > 100.0f)
            return;

        CanvasScale = new Point2D(newScaleX, newScaleY);

        var newTransX = center.X * (1 - factor) + _canvasTransform.X * factor;
        var newTransY = center.Y * (1 - factor) + _canvasTransform.Y * factor;

        CanvasTransform = new Point2D(newTransX, newTransY);
    }

    public void Zoom(float factor)
    {
        Zoom(_canvasResolution / new Vector2D(2), factor);
    }

    [RelayCommand]
    public void CenterObject()
    {
        var objCenter = PolygonTrain.ToPolygonTrain().GetCenter();
        CanvasTransform = _canvasResolution / new Vector2D(2) - (objCenter.ToVector() * _canvasScale.ToVector());
    }
    
    
    private void RebuildAllDrawables()
    {
        Drawables.Clear();

        // Origin marker
        Drawables.Add(DrawablePoint.FromCenter(Point2D.Zero, 0.3f, Brushes.Black));

        Drawables.Add(new DrawableCollection([
            // new DrawablePoint(new Point2D(-5, 5), 0.3f, Brushes.Blue),
            // new DrawablePoint(new Point2D(-5, -5), 0.3f, Brushes.Blue),
            new DrawableLine(new Point2D(0, 0), new Point2D(2, 0), 0.05f, Brushes.DarkGray),
            new DrawableLine(new Point2D(0, 0), new Point2D(0, 2), 0.05f, Brushes.DarkGray),
        ]));
        
        // Active polygon drawables
        Drawables.Add(PolygonTrain.DrawableCollection);
        Drawables.Add(IndividualPoints.DrawableCollection);
        Drawables.Add(BezierViewModel.DrawableCollection);
    }
}
