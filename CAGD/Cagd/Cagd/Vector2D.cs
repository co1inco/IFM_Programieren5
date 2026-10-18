namespace Cagd;

public class Vector2D
{
    public float X { get; set; }
    public float Y { get; set; }
    public int E => 0;
    
    public static Vector2D operator+ (Vector2D a, Vector2D b) => new Vector2D { X = a.X + b.X, Y = a.Y + b.Y };
    public static Vector2D operator- (Vector2D a, Vector2D b) => new Vector2D { X = a.X - b.X, Y = a.Y - b.Y };
    public static Vector2D operator* (Vector2D a, Vector2D b) => new Vector2D { X = a.X * b.X, Y = a.Y * b.Y };
    public static Vector2D operator/ (Vector2D a, Vector2D b) => new Vector2D { X = a.X / b.X, Y = a.Y / b.Y };
    
    public static Vector2D operator* (Vector2D a, float b) => new Vector2D { X = a.X * b, Y = a.Y * b };
    public static Vector2D operator/ (Vector2D a, float b) => new Vector2D { X = a.X / b, Y = a.Y / b };
    
    public float Dor(Vector2D b) => X * b.X + Y * b.Y;
}