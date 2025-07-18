using System.Collections;
using System.Diagnostics;
using BitArray = UFD.Util.BitArray;

namespace UFD.UfdElements;

public static class IntegerMethods
{
    private static BitArray _primeBits = null!;

    public static int NumberOfStoredPrimes => 1 + _primeBits.PopCount;

    public static void Initialise(int maxInt)
    {
        if (_primeBits is not null)
            throw new InvalidOperationException("Already initialised!");

        _primeBits = GeneratePrimes(maxInt);
    }

    private static BitArray GeneratePrimes(int maxInt)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxInt);

        var result = new BitArray(maxInt >>> 1, true);

        var enumerator = result.GetEnumerator();

        enumerator.MoveNext();
        var i = ConvertToPrime(enumerator.Current);

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

            enumerator.MoveNext();
            i = ConvertToPrime(enumerator.Current);
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

    public static int[] GetAllPrimes()
    {
        var result = new int[NumberOfStoredPrimes];

        var primeEnumerable = new PrimeEnumerable();
        var i = 0;
        foreach (var prime in primeEnumerable)
        {
            result[i++] = prime;
        }

        Debug.Assert(i == result.Length);

        return result;
    }

    public sealed class PrimeEnumerable : IEnumerable<int>, IEnumerator<int>
    {
        private BitArray.BitEnumerator _bitEnumerator = _primeBits.GetEnumerator();
        private int _current = -1;

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
