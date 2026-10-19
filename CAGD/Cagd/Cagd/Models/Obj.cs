namespace Cagd;

public class Obj
{
    public required Point2D[] Points { get; init; }
    public required int[][] Faces { get; init; }

    public Point2D GetCenter(int faceIndex = 0)
    {
        if (faceIndex < 0 || faceIndex >= Faces.Length)
            return Point2D.Zero;

        var val = Vector2D.Zero;
        var count = 0;
        foreach (var index in Faces[faceIndex])
        {
            if (Points.ElementAtOrDefault(index) is not { } p)
                continue;
            
            val += p.ToVector();
            count++;
        }

        if (count == 0)
            return Point2D.Zero;

        return (val / (float)count).ToPoint();
    }

    public IEnumerable<Point2D> GetFacePoints(int faceIndex = 0)
    {
        if (faceIndex < 0 || faceIndex >= Faces.Length)
            return [];

        return Faces[faceIndex]
            .Where(idx => idx >= 0 && idx < Points.Length)
            .Select(idx => Points[idx]);
    }
}
