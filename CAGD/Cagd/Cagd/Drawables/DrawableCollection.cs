using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;

public class DrawableCollection : ObservableObject, IDrawableObject
{
    private Point2D _position = Point2D.Zero;
    private ObservableCollection<IDrawableObject> _drawables = [];

    public DrawableCollection()
    {
    }

    public DrawableCollection(IEnumerable<IDrawableObject> drawables)
    {
        _drawables = new ObservableCollection<IDrawableObject>(drawables);
    }

    public Point2D Position
    {
        get => _position;
        set => SetProperty(ref _position, value);
    }

    public Vector2D Offset => Vector2D.Zero;

    public ObservableCollection<IDrawableObject> Drawables
    {
        get => _drawables;
        private set => SetProperty(ref _drawables, value);
    }

    public void SetCollection(IEnumerable<IDrawableObject> drawables)
    {
        Drawables = new ObservableCollection<IDrawableObject>(drawables);
    }
}
