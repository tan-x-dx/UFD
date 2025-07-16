using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace UFD.Util;

public sealed class BitArray
{
    private readonly uint[] _bits;
    private int _popCount;

    public int Length => _bits.Length << BitArrayHelpers.Shift;
    public int PopCount => _popCount;

    public BitArray(int capacity, bool setAllBits)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(capacity);

        _bits = BitArrayHelpers.CreateBitArray(capacity, setAllBits);
        if (setAllBits)
            _popCount = BitArrayHelpers.GetPopCount(_bits);
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

    public BitEnumerator GetEnumerator() => new(_bits);

    public struct BitEnumerator : IEnumerator<int>
    {
        private readonly uint[] _bits;

        private int _remaining;
        private int _index;
        private int _current;
        private uint _v;

        public readonly int Current => _current;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BitEnumerator(uint[] bits)
        {
            _bits = bits;
            _remaining = BitArrayHelpers.GetPopCount(_bits);
            _index = 0;
            _current = 0;
            _v = _bits.Length == 0 ? 0U : _bits[0];
        }

        public bool MoveNext()
        {
            if (_v == 0U)
            {
                if (_remaining == 0)
                    return false;

                do
                {
                    _v = _bits[++_index];
                }
                while (_v == 0U);
            }

            _current = (_index << BitArrayHelpers.Shift) | BitOperations.TrailingZeroCount(_v);
            _v &= _v - 1;
            _remaining--;
            return true;
        }

        public void Reset()
        {
            _remaining = BitArrayHelpers.GetPopCount(_bits);
            _index = 0;
            _current = 0;
            _v = _bits.Length == 0 ? 0U : _bits[0];
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        readonly object IEnumerator.Current => _current;
        readonly void IDisposable.Dispose() { }
    }
}
