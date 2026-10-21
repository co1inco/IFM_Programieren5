namespace Cagd.Tests;

[TestClass]
public class PolygonTrainViewModelTests
{
    [TestMethod]
    public void PolygonTrainViewModel_GeneratesDrawablesAndUpdatesOnPointMove()
    {
        var points = new[]
        {
            new Point2D(0, 0),
            new Point2D(5, 5),
            new Point2D(10, 0)
        };

        var vm = new PolygonTrainViewModel(points);

        Assert.HasCount(3, vm.Points);
        // 3 points + 2 lines = 5 drawables
        Assert.HasCount(5, vm.DrawableCollection.Drawables);

        var pointDrawables = vm.DrawableCollection.Drawables.OfType<DrawablePoint>().ToList();
        var lineDrawables = vm.DrawableCollection.Drawables.OfType<DrawableLine>().ToList();

        Assert.HasCount(3, pointDrawables);
        Assert.HasCount(2, lineDrawables);

        // Move middle point
        vm.Points[1].X = 6f;
        vm.Points[1].Y = 8f;

        // Verify point drawable updated
        Assert.AreEqual(6f, pointDrawables[1].Position.X);
        Assert.AreEqual(8f, pointDrawables[1].Position.Y);

        // Verify connected lines updated
        Assert.AreEqual(6f, lineDrawables[0].P2.X);
        Assert.AreEqual(8f, lineDrawables[0].P2.Y);
        Assert.AreEqual(6f, lineDrawables[1].P1.X);
        Assert.AreEqual(8f, lineDrawables[1].P1.Y);

        // Add point
        vm.AddPoint(new Point2D(15, 5));
        Assert.HasCount(4, vm.Points);
        Assert.HasCount(7, vm.DrawableCollection.Drawables); // 4 points + 3 lines

        // Remove point
        vm.RemovePointAt(0);
        Assert.HasCount(3, vm.Points);
        Assert.HasCount(5, vm.DrawableCollection.Drawables); // 3 points + 2 lines
    }
}
