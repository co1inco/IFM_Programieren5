using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace Cagd;


public class ObjLoader
{

    private static readonly Regex VertexRe = new(@"v\s+(-?\d+\.\d+)\s+(-?\d+\.\d+)", RegexOptions.Compiled | RegexOptions.NonBacktracking);
    private static readonly Regex FaceRe = new(@"f(?:\s+(\d+))", RegexOptions.Compiled | RegexOptions.NonBacktracking);
    
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
                var result = VertexRe.Match(line);
                points.Add(new Point2D
                {
                    X = float.Parse(result.Groups[1].Value, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture),
                    Y = float.Parse(result.Groups[2].Value, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture)
                });
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