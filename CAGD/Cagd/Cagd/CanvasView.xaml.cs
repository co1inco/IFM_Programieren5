using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Cagd;


public record CanvasPoint(float X, float Y);
public record CanvasLine((float X, float Y) A, (float X, float Y) B);

public partial class CanvasView : UserControl
{
    public CanvasView()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(
        nameof(Points), 
        typeof(IEnumerable<CanvasPoint>), 
        typeof(CanvasView), 
        new PropertyMetadata(default(IEnumerable<CanvasPoint>)));

    public IEnumerable<CanvasPoint> Points
    {
        get => (IEnumerable<CanvasPoint>)GetValue(PointsProperty);
        set => SetValue(PointsProperty, value);
    }


    public static readonly DependencyProperty PointRadiusProperty = DependencyProperty.Register(
        nameof(PointRadius), typeof(float), typeof(CanvasView), new PropertyMetadata(2));

    public float PointRadius
    {
        get => (float)GetValue(PointRadiusProperty);
        set => SetValue(PointRadiusProperty, value);
    }


    public static readonly DependencyProperty LinesProperty = DependencyProperty.Register(
        nameof(Lines), typeof(IEnumerable<CanvasLine>), typeof(CanvasView), new PropertyMetadata(default(IEnumerable<CanvasLine>)));

    public IEnumerable<CanvasLine> Lines
    {
        get => (IEnumerable<CanvasLine>)GetValue(LinesProperty);
        set => SetValue(LinesProperty, value);
    }

    public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
        nameof(LineThickness), typeof(float), typeof(CanvasView), new PropertyMetadata(1));

    public float LineThickness
    {
        get => (float)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    
    public void Redraw()
    {
        Canvas.Children.Clear();

        Canvas.Children.Add(new Ellipse ()
        {
            Width = 20,
            Height = 20,
            
        });
    }
    
}