namespace Cagd.Tests;

[TestClass]
public class GeometryTests
{
    [TestMethod]
    public void Point2D_Vector2D_Operations_WorkCorrectly()
    {
        var p1 = new Point2D(2f, 3f);
        var p2 = new Point2D(5f, 7f);
        
        // Point - Point = Vector
        var v = p2 - p1;
        Assert.AreEqual(3f, v.X);
        Assert.AreEqual(4f, v.Y);
        Assert.AreEqual(0, v.E); // Affine weight for vector
        Assert.AreEqual(1, p1.E); // Affine weight for point

        // Point + Vector = Point
        var p3 = p1 + v;
        Assert.AreEqual(5f, p3.X);
        Assert.AreEqual(7f, p3.Y);

        // Vector operations
        var vScaled = v * 2f;
        Assert.AreEqual(6f, vScaled.X);
        Assert.AreEqual(8f, vScaled.Y);

        Assert.AreEqual(25f, v.LengthSquared);
        Assert.AreEqual(5f, v.Length);
        Assert.AreEqual(14f, v.Dot(new Vector2D(2f, 2f)));
    }

    [TestMethod]
    public void DrawableCollection_ContainsAndManagesItems()
    {
        var p1 = new DrawablePoint(new Point2D(-5, 5), 0.3f);
        var p2 = new DrawablePoint(new Point2D(-5, -5), 0.3f);
        var collection = new DrawableCollection([p1, p2]);

        Assert.AreEqual(Point2D.Zero, collection.Position);
        Assert.AreEqual(Vector2D.Zero, collection.Offset);
        Assert.HasCount(2, collection.Drawables);

        collection.Position = new Point2D(10, 10);
        Assert.AreEqual(new Point2D(10, 10), collection.Position);

        collection.SetCollection([p1]);
        Assert.HasCount(1, collection.Drawables);
    }
}
