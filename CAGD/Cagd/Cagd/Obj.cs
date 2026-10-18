namespace Cagd;

public class Obj
{
    public required Point2D[] Points { get; init; }
    public required int[][] Faces { get; init; }

    public Point2D GetCenter(int face)
    {
        if (face < 0 || face >= Faces.Length)
            return Point2D.Zero;

        var val = Vector2D.Zero;
        var count = 0;
        foreach (var index in Faces[face])
        {
            if (Points.ElementAtOrDefault(index) is not { } p)
                continue;
            
            val += p.ToVector();
            count++;
        }

        return (val / (float)count).ToPoint();
    }
    
}