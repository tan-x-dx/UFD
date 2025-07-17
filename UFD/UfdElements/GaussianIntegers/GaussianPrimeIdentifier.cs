namespace UFD.UfdElements.GaussianIntegers;

public readonly struct GaussianPrimeIdentifier : IPrimeIdentifier<GaussianInteger>
{
    public static int Norm(GaussianInteger x) => x.Norm();
    public static bool IsUnit(GaussianInteger x) => x.Norm() == 1;
    public static bool IsPrime(GaussianInteger x)
    {
        x = x.ToFirstQuadrant();

        if (x.Im == 0)
        {
            return (x.Re & 3) == 3 &&
                IntegerMethods.IsPrime(x.Re);
        }

        var normSquared = x.Norm();
        if ((normSquared & 3) == 1)
            return IntegerMethods.IsPrime(normSquared);

        return x.Re == 1 && x.Im == 1;
    }

    public static void DivRem(GaussianInteger left, GaussianInteger right, out GaussianInteger quotient, out GaussianInteger remainder)
    {
        left.DivRem(right, out quotient, out remainder);
    }

    public static IEnumerable<GaussianInteger> GetPrimeList() => GaussianIntegerMethods.GaussianPrimesEnumerable();
}
