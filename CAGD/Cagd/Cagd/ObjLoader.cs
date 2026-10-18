using System.IO;

namespace Cagd;


public class ObjLoader
{

    public static Obj Load(string path)
    {
        var lines = File.ReadAllLines(path);

        List<Point2D> points = [];
        List<int[]> faces = [];

        foreach (var line in lines.Select(x => x.Trim()))
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith('#'))
                continue;
            
            if (line.StartsWith("v "))
            {
                var parts = line.Split(' ');
                points.Add(new Point2D { X = float.Parse(parts[1]), Y = float.Parse(parts[2]) });
            }
            else if (line.StartsWith("f "))
            {
                var parts = line.Split(' ');
                faces.Add(parts.Skip(1).Select(x => int.Parse(x) - 1).ToArray());
            }
        }
        
        return new Obj
        {
            Points = points.ToArray(), 
            Faces = faces.ToArray()
        };
    }
    
}