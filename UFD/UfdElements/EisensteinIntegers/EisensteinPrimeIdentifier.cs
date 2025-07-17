namespace UFD.UfdElements.EisensteinIntegers;

public readonly struct EisensteinPrimeIdentifier : IPrimeIdentifier<EisensteinInteger>
{
    public static int Norm(EisensteinInteger x) => x.Norm();
    public static bool IsUnit(EisensteinInteger x) => x.Norm() == 1;
    public static bool IsPrime(EisensteinInteger x)
    {
        throw new NotImplementedException();
    }

    public static void DivRem(EisensteinInteger left, EisensteinInteger right, out EisensteinInteger quotient, out EisensteinInteger remainder)
    {
        left.DivRem(right, out quotient, out remainder);
    }

    public static IEnumerable<EisensteinInteger> GetPrimeList()
    {
        throw new NotImplementedException();
    }
}
