using BenchmarkDotNet.Attributes;

namespace Task2;

[MemoryDiagnoser]
public class BitVectorBenchmark
{
    private const int _size = 1_000_000;
    private const int _firstIndex = 0;
    private const int _lastIndex = _size*32 - 1;
    private BitVector _bitVector;
    
    private const int _sizeSmall = 32;
    private const int _firstIndexSmall = 0;
    private const int _lastIndexSmall = _size*32 - 1;
    private BitVector _bitVector_small;
    

    [GlobalSetup]
    public void Setup()
    {
        var random = new Random(420);
        _bitVector = new BitVector(
            Enumerable.Repeat(0, _size).Select(_ => (uint)random.Next())
        );
        
        _bitVector_small = new BitVector(
            Enumerable.Repeat(0, _sizeSmall).Select(_ => (uint)random.Next())
        );
    }

    [Benchmark]
    public bool AccessFirst_Large() => _bitVector.Access(_firstIndex);
    [Benchmark]
    public bool AccessLast_Large() => _bitVector.Access(_lastIndex);
    
    
    [Benchmark]
    public bool AccessFirst_Small() => _bitVector_small.Access(_firstIndexSmall);
    [Benchmark]
    public bool AccessLast_Small() => _bitVector_small.Access(_lastIndexSmall);
}