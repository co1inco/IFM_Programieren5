using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cagd;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private Point? _lastMousePosition;
    private bool _isDragging;

    public MainWindow()
    {
        DataContext = new MainWindowViewModel();
        InitializeComponent();
    }

    private void CanvasViewport_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (sender is UIElement element)
        {
            element.CaptureMouse();
            _isDragging = true;
            _lastMousePosition = e.GetPosition(CanvasItemsControl);
        }
    }

    private void CanvasViewport_OnMouseMove(object sender, MouseEventArgs e)
    {
        if (!_isDragging || e.LeftButton != MouseButtonState.Pressed || _lastMousePosition == null)
            return;

        if (DataContext is not MainWindowViewModel vm)
            return;

        var currentPosition = e.GetPosition(CanvasItemsControl);
        var delta = currentPosition - _lastMousePosition.Value;
        _lastMousePosition = currentPosition;

        vm.MoveCanvas(new Vector2D((float)delta.X, (float)delta.Y));
    }

    private void CanvasViewport_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (_isDragging)
        {
            _isDragging = false;
            _lastMousePosition = null;
            if (sender is UIElement element)
            {
                element.ReleaseMouseCapture();
            }
        }
    }

    private void CanvasViewport_OnLostMouseCapture(object sender, MouseEventArgs e)
    {
        _isDragging = false;
        _lastMousePosition = null;
    }

    private void CanvasViewport_OnMouseWheel(object sender, MouseWheelEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm)
            return;

        var mousePos = e.GetPosition(CanvasItemsControl);
        float zoomFactor = e.Delta > 0 ? 1.1f : 1.0f / 1.1f;

        vm.Zoom(new Point2D((float)mousePos.X, (float)mousePos.Y), zoomFactor);
        e.Handled = true;
    }
}