namespace Ads1;

public class Sorting
{
    public readonly char[] _colors;
    
    public Sorting()
    {
        _colors = GenerateCharSequence(1000, 42);
    }
    
    public static char[] GenerateCharSequence(int length, int seed)
    {
        var r = new Random(seed);
        return Enumerable.Range(0, length)
            .Select(i => 
                r.Next(0, 4) switch
                {
                    0 => 'R',
                    1 => 'G',
                    2 => 'B',
                    3 => 'O',
                    _ => throw new InvalidOperationException("Invalid color"),
                })
            .ToArray();
    }
    

    public static void FlipSort(ref Span<char> chars)
    {
        if (chars.Length == 0)
            return;

        // Find the first char matching the last character(s) 
        var first = true;
        int matchIndex = -1;
        for (int i = chars.Length - 1; i >= 0; i--)
        {
            if (chars[i] == chars[^1])
            {
                if (first) continue;
                matchIndex = i;
                break;
            }
            else
            {
                first = false;
            }
        }
        
        // flip all characters between the matched and the end
        for (int i = 0; i < chars.Length - matchIndex; i++)
        {
            (chars[^i], chars[matchIndex + i + 1]) = (chars[matchIndex + i + 1], chars[^i]);
        }
    }
    
    
}