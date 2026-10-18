using Shouldly;

namespace Ads1.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        Span<char> c = "RGBORGBO".ToCharArray().AsSpan();
        
        Sorting.FlipSort(ref c);
        
        new string(c).ShouldBe("RRGGBBOO");
    }
}