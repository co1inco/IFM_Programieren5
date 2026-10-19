namespace Cagd;

public record Point2D(float X, float Y)
{
    public int E => 1;
    
    public static Point2D Zero => new Point2D(0,  0);
    
    public Vector2D ToVector() => new Vector2D(X, Y);
    
    public static Vector2D operator- (Point2D a, Point2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
    
    public static Point2D operator+ (Point2D a, Vector2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
    public static Point2D operator- (Point2D a, Vector2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
    public static Point2D operator* (Point2D a, Vector2D b) => new Point2D(a.X * b.X, a.Y * b.Y);
    public static Point2D operator/ (Point2D a, Vector2D b) => new Point2D(a.X / b.X, a.Y / b.Y);
}