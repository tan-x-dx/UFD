namespace UFD.UfdElements.GaussianIntegers;

public readonly ref struct GaussianPrimeIdentifier : IPrimeIdentifier<GaussianInteger>
{
    public static int Norm(GaussianInteger x)
    {
        var modulus = Modulus(x);
        return (int)Math.Round(modulus * modulus);
    }

    public static double Modulus(GaussianInteger x)
    {
        var halfRe = (double)x.Re;
        halfRe /= 2;
        var halfIm = (double)x.Im;
        halfIm /= 2;

        var halfModulus = Math.Sqrt((halfRe * halfRe) + (halfIm * halfIm));
        return halfModulus * 2;
    }

    public static bool IsUnit(GaussianInteger x) => x.Norm() == 1;
    public static bool IsPrime(GaussianInteger x)
    {
        x = x.ToFirstQuadrant();

        if (x.Im == 0)
        {
            return (x.Re & 3) == 3 &&
                IntegerMethods.IsPrime(x.Re);
        }

        var norm = Norm(x);
        if ((norm & 3) == 1)
            return IntegerMethods.IsPrime(norm);

        return x.Re == 1 && x.Im == 1;
    }

    public static void DivRem(GaussianInteger left, GaussianInteger right, out GaussianInteger quotient, out GaussianInteger remainder)
    {
        left.DivRem(right, out quotient, out remainder);
    }

    public static IEnumerable<GaussianInteger> GetPrimeList() => GaussianIntegerMethods.GaussianPrimesEnumerable();

}
