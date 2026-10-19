namespace Cagd.Tests;

[TestClass]
public class PolygonTrainTests
{
    [TestMethod]
    public void PolygonTrain_AddAndRemove_WorksCorrectly()
    {
        var train = new PolygonTrain();
        bool changedFired = false;
        train.Changed += (_, _) => changedFired = true;

        train.Add(new Point2D(0, 0));
        train.Add(new Point2D(4, 0));
        train.Add(new Point2D(4, 4));

        Assert.IsTrue(changedFired);
        Assert.HasCount(3, train.Vertices);

        var center = train.GetCenter();
        Assert.AreEqual(8f / 3f, center.X, 0.001f);
        Assert.AreEqual(4f / 3f, center.Y, 0.001f);

        train.RemoveAt(1);
        Assert.HasCount(2, train.Vertices);
        Assert.AreEqual(4f, train[1].X);
        Assert.AreEqual(4f, train[1].Y);
    }
}
