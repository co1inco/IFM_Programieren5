using System.Numerics;

namespace Task2;

public class BitVector
{

    private List<(uint Bits, int CumulativeCount)> _bits = [];

    public BitVector()
    {
        
    }
    
    public BitVector(int size)
    {
        _bits = Enumerable.Repeat(((uint)0, 0), size / 32 + 1).ToList();
    }

    public BitVector(IEnumerable<uint> bitBlocks)
    {
            _bits = CalculateBlocks(bitBlocks).ToList();
    }

    public static BitVector CreateBlocks(int count) => new BitVector(count * 32);
    
    
    public bool Access(int index)
    {
        var (i, b) = GetAddress(index);
        return ((_bits[i].Bits) >> (b & 0x01)) != 0;
    }

    public void Set(int index, bool state = true)
    {
        if (Access(index) == state)
            return;

        var (i, b) = GetAddress(index);

        // The time constraints are only for reading operations. So the time complexity is hidden away in the Set method.
        if (state)
        {
            _bits[i] = (_bits[i].Bits | (0x01U << b), _bits[i].CumulativeCount + 1);
            for (int j = i+1 ; j < _bits.Count; j++)
                _bits[j] = (_bits[j].Bits, _bits[j].CumulativeCount + 1);
        }
        else
        {
            _bits[i] = (_bits[i].Bits & ~(0x01U << b), _bits[i].CumulativeCount - 1);
            for (int j = i+1 ; j < _bits.Count; j++)
                _bits[j] = (_bits[j].Bits, _bits[j].CumulativeCount - 1);
        }
    }
    
    public void Reset(int index) => Set(index, false);
    
    
    public int Rank1(int index)
    {
        var (i, b) = GetAddress(index);
        
        var prevCount = i > 0 ? _bits[i-1].CumulativeCount : 0;
        
        var mask = (0x01U << b) | (0x01U << b) - 1; // create bit mask (index bit + all bits before it)
        
        return BitOperations.PopCount(_bits[i].Bits & mask) + prevCount;
    }
    
    
    private (int Index, int Byte) GetAddress(int index) => (index >> 5, index & 0x1F);


    private IEnumerable<(uint, int)> CalculateBlocks(IEnumerable<uint> bitBlocks)
    {
        int cumulativeCount = 0;
        foreach (var bitBlock in bitBlocks)
        {
            cumulativeCount += BitOperations.PopCount(bitBlock);
            yield return (bitBlock, cumulativeCount);
        }
    }
    
}