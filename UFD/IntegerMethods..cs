using System.Collections;
using System.Diagnostics;
using BitArray = UFD.Util.BitArray;

namespace UFD;

public static class IntegerMethods
{
    private static BitArray _primeBits = null!;

    public static int NumberOfStoredPrimes => 1 + _primeBits.PopCount;

    public static void Initialise(int maxInt)
    {
        _primeBits = GeneratePrimes(maxInt);
    }

    private static BitArray GeneratePrimes(int maxInt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxInt);

        var result = new BitArray(maxInt >>> 1, true);

        var i = 3;

        var iLimit = 1 + (int)Math.Sqrt(result.Length);

        while (i < iLimit)
        {
            var j = i * i;
            var delta = i + i;
            var bitIndex = ConvertToBitIndex(j);

            while (bitIndex < result.Length)
            {
                result.ClearBit(bitIndex);
                j += delta;
                bitIndex = ConvertToBitIndex(j);
            }

            i += 2;
        }

        return result;
    }

    public static bool IsPrime(int n)
    {
        if (n < 0)
            n = -n;

        if (n < 2)
            return false;
        if ((n & 1) == 0)
            return n == 2;

        var bitIndex = ConvertToBitIndex(n);
        return _primeBits.GetBit(bitIndex);
    }

    private static int ConvertToBitIndex(int n) => (n >>> 1) - 1;
    private static int ConvertToPrime(int bitIndex) => (bitIndex << 1) + 3;

    public sealed class PrimeEnumerable : IEnumerable<int>, IEnumerator<int>
    {
        private int _current = -1;
        private BitArray.BitEnumerator _bitEnumerator = _primeBits.GetEnumerator();

        public int Current => _current;

        public bool MoveNext()
        {
            if (_current == -1)
            {
                _current = 2;
                return true;
            }

            var moveNext = _bitEnumerator.MoveNext();
            var bitIndex = _bitEnumerator.Current;

            _current = ConvertToPrime(bitIndex);
            return moveNext;
        }

        public void Reset()
        {
            _current = -1;
            _bitEnumerator.Reset();
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        object IEnumerator.Current => _current;
        void IDisposable.Dispose() { }

        IEnumerator<int> IEnumerable<int>.GetEnumerator() => this;
        IEnumerator IEnumerable.GetEnumerator() => this;
    }
}

public readonly struct IntegerPrimeIdentifier : IPrimeIdentifier<int>
{
    public static bool IsUnit(int x) => x == 1 || x == -1;
    public static bool IsPrime(int x) => IntegerMethods.IsPrime(x);
}
