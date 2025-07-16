namespace UFD.Util;

public sealed class BitArray
{
    private readonly uint[] _bits;
    private int _popCount;

    public BitArray(int capacity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        _bits = BitArrayHelpers.CreateBitArray(capacity, false);
    }

    public bool GetBit(int bitIndex) => BitArrayHelpers.GetBit(_bits, bitIndex);
    public bool SetBit(int bitIndex) => BitArrayHelpers.SetBit(_bits, bitIndex, ref _popCount);
    public bool ClearBit(int bitIndex) => BitArrayHelpers.ClearBit(_bits, bitIndex, ref _popCount);
    public bool ToggleBit(int bitIndex) => BitArrayHelpers.ToggleBit(_bits, bitIndex, ref _popCount);

    public void Clear()
    {
        new Span<uint>(_bits).Clear();
        _popCount = 0;
    }
}
