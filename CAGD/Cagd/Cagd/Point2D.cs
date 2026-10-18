namespace Cagd;

public class Point2D
{
    public float X { get; set; }
    public float Y { get; set; }
    public int E => 1;
    
    public static Vector2D operator- (Point2D a, Point2D b) => new Vector2D { X = a.X - b.X, Y = a.Y - b.Y };
    
    public static Point2D operator+ (Point2D a, Vector2D b) => new Point2D() { X = a.X + b.X, Y = a.Y + b.Y };
    public static Point2D operator- (Point2D a, Vector2D b) => new Point2D() { X = a.X - b.X, Y = a.Y - b.Y };
    public static Point2D operator* (Point2D a, Vector2D b) => new Point2D() { X = a.X * b.X, Y = a.Y * b.Y };
    public static Point2D operator/ (Point2D a, Vector2D b) => new Point2D() { X = a.X / b.X, Y = a.Y / b.Y };
}