using System.IO;

namespace Cagd.Tests;

[TestClass]
public class ObjLoaderTests
{
    [TestMethod]
    public void ObjLoader_ParsesVerticesAndFaces()
    {
        string objContent = """
            # Sample OBJ
            v 0.0  5.0
            v 5.0  5.0
            v 5.0  -5.0
            v 0.0  -5.0
            f 1 2 3 4
            """;

        using var reader = new StringReader(objContent);
        var obj = ObjLoader.Load(reader);

        Assert.HasCount(4, obj.Points);
        Assert.HasCount(1, obj.Faces);
        Assert.HasCount(4, obj.Faces[0]);

        var center = obj.GetCenter(0);
        Assert.AreEqual(2.5f, center.X, 0.001f);
        Assert.AreEqual(0.0f, center.Y, 0.001f);
    }

    [TestMethod]
    public void ObjLoader_ParsesCurveFormat()
    {
        string objContent = """
            v 0.0  5.0
            v 5.0  5.0
            v 5.0  -5.0
            v 0.0  -5.0
            cstype bezier
            deg 3
            curv 0.0 1.0 1 2 3 4
            parm u 0.0 1.0
            end
            """;

        using var reader = new StringReader(objContent);
        var obj = ObjLoader.Load(reader);

        Assert.HasCount(4, obj.Points);
        Assert.HasCount(1, obj.Faces);
        CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, obj.Faces[0]);
    }
}
