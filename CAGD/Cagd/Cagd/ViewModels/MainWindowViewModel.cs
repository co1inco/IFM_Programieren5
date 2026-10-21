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
    private PolygonTrainViewModel _polygonTrain;

    public MainWindowViewModel()
    {
        _polygonTrain = new PolygonTrainViewModel();
        // _polygonTrain.Drawables.CollectionChanged += OnPolygonDrawablesChanged;

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

    public PolygonTrainViewModel PolygonTrain
    {
        get => _polygonTrain;
        set
        {
            if (_polygonTrain != value)
            {
                // if (_polygonTrain != null)
                // {
                //     _polygonTrain.Drawables.CollectionChanged -= OnPolygonDrawablesChanged;
                // }

                // if (SetProperty(ref _polygonTrain, value))
                // {
                //     if (_polygonTrain != null)
                //     {
                //         _polygonTrain.Drawables.CollectionChanged += OnPolygonDrawablesChanged;
                //     }
                //     RebuildAllDrawables();
                // }
            }
        }
    }

    public ObservableCollection<IDrawableObject> Drawables { get; } = [];
    
    public IndividualPointsViewModel IndividualPoints { get; } = new();

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
    private void ResetView()
    {
        CanvasResolution = new Point2D(20, 20);
        CanvasTransform = new Point2D(10, 10);
        CanvasScale = new Point2D(1, 1);
    }

    [RelayCommand]
    private void MoveCanvas(Vector2D v)
    {
        CanvasTransform += v;
    }
    
    // [RelayCommand]
    // private void MoveCanvas(MouseEventArgs args)
    // {
    //     if (args.LeftButton != MouseButtonState.Pressed)
    //         return;
    //     
    //     // CanvasTransform += new Vector2D(args.);
    // }

    [RelayCommand]
    private void CenterObject()
    {
        var objCenter = PolygonTrain.GetCenter();
        CanvasTransform = _canvasResolution / new Vector2D(2) - objCenter.ToVector();
    }
    
    
    private void RebuildAllDrawables()
    {
        Drawables.Clear();

        // Origin marker
        Drawables.Add(DrawablePoint.FromCenter(Point2D.Zero, 0.3f, Brushes.Black));

        Drawables.Add(new DrawableCollection([
            new DrawablePoint(new Point2D(-5, 5), 0.3f, Brushes.Blue),
            new DrawablePoint(new Point2D(-5, -5), 0.3f, Brushes.Blue),
            new DrawableLine(new Point2D(0, 0), new Point2D(2, 0), 0.05f, Brushes.DarkGray),
            new DrawableLine(new Point2D(0, 0), new Point2D(0, 2), 0.05f, Brushes.DarkGray),
        ]));
        
        // Active polygon drawables
        Drawables.Add(PolygonTrain.DrawableCollection);
        Drawables.Add(IndividualPoints.DrawableCollection);
        // foreach (var d in PolygonTrain.DrawableCollection)
        // {
        //     Drawables.Add(d);
        // }
    }
}
