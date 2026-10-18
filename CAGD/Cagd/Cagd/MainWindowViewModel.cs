using System.Collections.ObjectModel;
using System.Drawing;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Cagd;


public interface IDrawableObject
{
    public float X { get; }
    public float Y { get; }
}

public record DrawableRect(float X, float Y, float Width, float Height, Brush Color) : IDrawableObject;



public partial class MainWindowViewModel : ObservableObject
{


    public ObservableCollection<IDrawableObject> Drawables { get; } = [
        new DrawableRect(5, 5, 100, 100, Brushes.Red)
    ];

}