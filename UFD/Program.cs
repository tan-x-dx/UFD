using System.Numerics;
using UFD.UfdElements;
using UFD.UfdElements.GaussianIntegers;

IntegerMethods.Initialise(100_000_000);
GaussianIntegerMethods.Initialise(10_000);

for (var i = 1; i <= 1000; i++)
{
    try
    {
        Console.WriteLine($"f({i}) = {F(i)}");
    }
    catch
    {
    }
}

;

/*
var primes = IntegerMethods.GetAllPrimes();

var n = 11638725;
var primeFactorsOfN = IntFactorisation.GetPrimeFactors(n);
Console.WriteLine($"{n} = {string.Join('*', primeFactorsOfN)}");

var g = 13 + 21.i();
var primeFactorsOfG = GaussianIntegerFactorisation.GetPrimeFactors(g);
Console.WriteLine($"{g} -> {string.Join(',', primeFactorsOfG)}");
*/

BigInteger F(BigInteger n)
{
    if (n <= BigInteger.One)
        return BigInteger.One;

    int k = (int)n;
    var primeFactorsOfN = IntFactorisation.GetPrimeFactors(k);

    BigInteger total = BigInteger.One;

    foreach (var primeFactor in primeFactorsOfN)
    {
        BigInteger bigPrimePart = primeFactor - 1;

        var evaluatedPrimePart = F(bigPrimePart);

        total *= (evaluatedPrimePart * F(evaluatedPrimePart)) + 1;
    }

    return total;
}
