using System.Windows.Media;

namespace Cagd;

public record Point2D(float X, float Y)
{
    public int E => 1;
    
    public static Point2D Zero => new Point2D(0,  0);
    
    public Vector2D ToVector() => new Vector2D(X, Y);
    
    public static Point2DMath operator+ (Point2D a, Point2D b) => new Point2DMath(a.X + b.X, a.Y + b.Y, a.E + b.E);
    public static Vector2D operator- (Point2D a, Point2D b) => new Vector2D(a.X - b.X, a.Y - b.Y);
    public static Point2DMath operator* (Point2D a, Point2D b) => new Point2DMath(a.X * b.X, a.Y * b.Y, a.E * b.E);
    public static Point2DMath operator/ (Point2D a, Point2D b) => new Point2DMath(a.X / b.X, a.Y / b.Y, a.E / (float)b.E);
    
    
    public static Point2D operator+ (Point2D a, Vector2D b) => new Point2D(a.X + b.X, a.Y + b.Y);
    public static Point2D operator- (Point2D a, Vector2D b) => new Point2D(a.X - b.X, a.Y - b.Y);
    public static Point2D operator* (Point2D a, Vector2D b) => new Point2D(a.X * b.X, a.Y * b.Y);
    public static Point2D operator/ (Point2D a, Vector2D b) => new Point2D(a.X / b.X, a.Y / b.Y);
}

public record Point2DMath(float X, float Y, float E)
{
    private const float Tolerance = 0.01f;
    
    
    
    // Points
    public static Point2DMath operator+ (Point2DMath a, Point2D b) => new Point2DMath(a.X + b.X, a.Y + b.Y, a.E + b.E);
    public static Point2DMath operator- (Point2DMath a, Point2D b) => new Point2DMath(a.X - b.X, a.Y - b.Y, a.E - b.E);
    public static Point2DMath operator* (Point2DMath a, Point2D b) => new Point2DMath(a.X * b.X, a.Y * b.Y, a.E * b.E);
    public static Point2DMath operator/ (Point2DMath a, Point2D b) => new Point2DMath(a.X / b.X, a.Y / b.Y, a.E / b.E);
    
    public static Point2DMath operator+ (Point2D a, Point2DMath b) => new Point2DMath(a.X + b.X, a.Y + b.Y, a.E + b.E);
    public static Point2DMath operator- (Point2D a, Point2DMath b) => new Point2DMath(a.X - b.X, a.Y - b.Y, a.E - b.E);
    public static Point2DMath operator* (Point2D a, Point2DMath b) => new Point2DMath(a.X * b.X, a.Y * b.Y, a.E * b.E);
    public static Point2DMath operator/ (Point2D a, Point2DMath b) => new Point2DMath(a.X / b.X, a.Y / b.Y, a.E / b.E);
    
    public static implicit operator Point2DMath(Point2D p) => new Point2DMath(p.X, p.Y, p.E);
    
    
    // Vectors
    public static Point2DMath operator+ (Point2DMath a, Vector2D b) => new Point2DMath(a.X + b.X, a.Y + b.Y, a.E + b.E);
    public static Point2DMath operator- (Point2DMath a, Vector2D b) => new Point2DMath(a.X - b.X, a.Y - b.Y, a.E - b.E);
    public static Point2DMath operator* (Point2DMath a, Vector2D b) => new Point2DMath(a.X * b.X, a.Y * b.Y, a.E * b.E);
    public static Point2DMath operator/ (Point2DMath a, Vector2D b) => new Point2DMath(a.X / b.X, a.Y / b.Y, a.E / b.E);
    
    public static implicit operator Point2DMath(Vector2D p) => new Point2DMath(p.X, p.Y, p.E);
    
    // Point math
    public static Point2DMath operator+ (Point2DMath a, Point2DMath b) => new Point2DMath(a.X + b.X, a.Y + b.Y, a.E + b.E);
    public static Point2DMath operator- (Point2DMath a, Point2DMath b) => new Point2DMath(a.X - b.X, a.Y - b.Y, a.E - b.E);
    public static Point2DMath operator* (Point2DMath a, Point2DMath b) => new Point2DMath(a.X * b.X, a.Y * b.Y, a.E * b.E);
    public static Point2DMath operator/ (Point2DMath a, Point2DMath b) => new Point2DMath(a.X / b.X, a.Y / b.Y, a.E / b.E);
    
    // Scalar
    public static Point2DMath operator* (Point2DMath a, float b) => new Point2DMath(a.X * b, a.Y * b, a.E * b);
    public static Point2DMath operator/ (Point2DMath a, float b) => new Point2DMath(a.X / b, a.Y / b, a.E / b);

    public Point2D ToPoint()
    {
        if (Math.Abs(E - 1) > Tolerance)
            Console.WriteLine($"Error: Point2DMath cannot be converted to Point2D because E({E}) != 1");
        return new Point2D(X, Y);
    }
    
    public Vector2D ToVector()
    {
        if (Math.Abs(E) > Tolerance)
            Console.WriteLine($"Error: Point2DMath cannot be converted to Vector2D because E({E}) != 0");
        return new Vector2D(X, Y);
    }
    
    
}

