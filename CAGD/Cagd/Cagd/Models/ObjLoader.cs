using System.Globalization;
using System.IO;

namespace Cagd;

public static class ObjLoader
{
    public static Obj Load(string path)
    {
        if (!File.Exists(path))
        {
            var appDir = AppDomain.CurrentDomain.BaseDirectory;
            var fallback = Path.Combine(appDir, path);
            if (File.Exists(fallback))
            {
                path = fallback;
            }
        }

        using var reader = new StreamReader(path);
        return Load(reader);
    }

    public static Obj Load(TextReader reader)
    {
        List<Point2D> points = [];
        List<int[]> faces = [];

        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            var trimmed = line.Trim();
            if (string.IsNullOrWhiteSpace(trimmed) || trimmed.StartsWith('#'))
                continue;

            var tokens = trimmed.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length < 2)
                continue;

            var tag = tokens[0];

            if (tag == "v" && tokens.Length >= 3)
            {
                if (float.TryParse(tokens[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var x) &&
                    float.TryParse(tokens[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
                {
                    points.Add(new Point2D(x, y));
                }
            }
            else if (tag == "f")
            {
                var faceIndices = new List<int>();
                for (int i = 1; i < tokens.Length; i++)
                {
                    var token = tokens[i];
                    var slashIndex = token.IndexOf('/');
                    var vertexPart = slashIndex >= 0 ? token[..slashIndex] : token;

                    if (int.TryParse(vertexPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out var vIndex))
                    {
                        // OBJ format uses 1-based indices
                        faceIndices.Add(vIndex > 0 ? vIndex - 1 : points.Count + vIndex);
                    }
                }

                if (faceIndices.Count > 0)
                {
                    faces.Add(faceIndices.ToArray());
                }
            }
            else if (tag == "curv" && tokens.Length >= 4)
            {
                // Format: curv u_min u_max p1 p2 p3 ...
                var curveIndices = new List<int>();
                for (int i = 3; i < tokens.Length; i++)
                {
                    if (int.TryParse(tokens[i], NumberStyles.Integer, CultureInfo.InvariantCulture, out var vIndex))
                    {
                        curveIndices.Add(vIndex > 0 ? vIndex - 1 : points.Count + vIndex);
                    }
                }

                if (curveIndices.Count > 0)
                {
                    faces.Add(curveIndices.ToArray());
                }
            }
        }

        return new Obj
        {
            Points = points.ToArray(),
            Faces = faces.ToArray()
        };
    }
}
