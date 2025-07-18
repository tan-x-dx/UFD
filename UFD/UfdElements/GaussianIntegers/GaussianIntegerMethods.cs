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
        var result = new HashSet<GaussianInteger>(maxInt, new AssociatedGaussianIntegerEqualityComparer())
        {
            new(1, 1)
        };

        var re = 2;
        while (result.Count < maxInt)
        {
            var im = (re & 1) ^ 1;

            while (im < re)
            {
                var g1 = new GaussianInteger(re, im);
                if (GaussianPrimeIdentifier.IsPrime(g1))
                {
                    result.Add(g1);
                    result.Add(new GaussianInteger(im, re));
                }

                im += 2;
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
