namespace Cagd.Helper;

public static class EnumerableHelper
{

    extension<T>(IEnumerable<T> enumerable)
    {

        public IEnumerable<(T, T)> Offset()
        {
            
            using var enumerator = enumerable.GetEnumerator();

            if (!enumerator.MoveNext())
                yield break;
            
            T last = enumerator.Current;
            while (enumerator.MoveNext())
            {
                yield return (last, enumerator.Current);
                last = enumerator.Current;
            }
        }
        
    }
    
}