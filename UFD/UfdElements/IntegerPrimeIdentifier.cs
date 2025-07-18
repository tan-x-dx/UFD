namespace UFD.UfdElements;

public readonly ref struct IntegerPrimeIdentifier : IPrimeIdentifier<int>
{
    public static int Norm(int x) => x * x;
    public static double Modulus(int x) => Math.Abs(x);
    public static bool IsUnit(int x) => x == 1 || x == -1;
    public static bool IsPrime(int x) => IntegerMethods.IsPrime(x);

    public static void DivRem(int left, int right, out int quotient, out int remainder)
    {
        (quotient, remainder) = int.DivRem(left, right);
    }

    public static IEnumerable<int> GetPrimeList() => new IntegerMethods.PrimeEnumerable();

}
