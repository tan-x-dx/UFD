namespace UFD.UfdElements.GaussianIntegers;

public static class GaussianIntegerMethods
{
    private static HashSet<GaussianInteger> _gaussianPrimes = null!;

    public static void Initialise(int maxInt)
    {
        if (_gaussianPrimes is not null)
            throw new InvalidOperationException("Already initialised!");

        _gaussianPrimes = GenerateGaussianPrimes(maxInt);
    }

    private static HashSet<GaussianInteger> GenerateGaussianPrimes(int maxInt)
    {
        var result = new HashSet<GaussianInteger>(maxInt, new AssociatedGaussianIntegerEqualityComparer());

        var re = 2;
        while (result.Count < maxInt)
        {
            var x = re;

            while (x > 0)
            {
                var y = re - x;
                var g = new GaussianInteger(x, y);
                if (GaussianPrimeIdentifier.IsPrime(g))
                {
                    result.Add(g);
                }

                x--;
            }

            re++;
        }

        return result;
    }

#pragma warning disable IDE1006 // Naming Styles
    public static GaussianInteger i(this int k) => new(0, k);
#pragma warning restore IDE1006 // Naming Styles

    public static IEnumerable<GaussianInteger> GaussianPrimesEnumerable() => _gaussianPrimes;

    private sealed class AssociatedGaussianIntegerEqualityComparer : IEqualityComparer<GaussianInteger>
    {
        public bool Equals(GaussianInteger x, GaussianInteger y)
        {
            return x.ToFirstQuadrant().Equals(y.ToFirstQuadrant());
        }

        public int GetHashCode(GaussianInteger obj)
        {
            return obj.ToFirstQuadrant().GetHashCode();
        }
    }
}
