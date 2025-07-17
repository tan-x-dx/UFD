namespace UFD.UfdElements.EisensteinIntegers;

public readonly struct EisensteinPrimeIdentifier : IPrimeIdentifier<EisensteinInteger>
{
    public static bool IsUnit(EisensteinInteger x) => x.NormSquared() == 1;
    public static bool IsPrime(EisensteinInteger x)
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<EisensteinInteger> GetPrimeList()
    {
        throw new NotImplementedException();
    }
}
