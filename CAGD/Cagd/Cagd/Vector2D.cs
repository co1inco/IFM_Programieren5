namespace Cagd;

public record Vector2D(float X, float Y)
{
    public int E => 0;
    
    public static Vector2D Zero => new Vector2D(0,0);
    
    public Point2D ToPoint() => new Point2D(X, Y);
    
    public static Vector2D operator- (Vector2D a) => new Vector2D(-a.X, -a.Y );
    
    public static Vector2D operator+ (Vector2D a, Vector2D b) => new Vector2D(a.X + b.X, a.Y + b.Y );
    public static Vector2D operator- (Vector2D a, Vector2D b) => new Vector2D(a.X - b.X, a.Y - b.Y );
    public static Vector2D operator* (Vector2D a, Vector2D b) => new Vector2D(a.X * b.X, a.Y * b.Y );
    public static Vector2D operator/ (Vector2D a, Vector2D b) => new Vector2D(a.X / b.X, a.Y / b.Y );
    
    public static Vector2D operator* (Vector2D a, float b) => new Vector2D(a.X * b, a.Y * b );
    public static Vector2D operator/ (Vector2D a, float b) => new Vector2D(a.X / b, a.Y / b );
    
    public float Dor(Vector2D b) => X * b.X + Y * b.Y;
}