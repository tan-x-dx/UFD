using UFD;
using UFD.UfdElements;
using UFD.UfdElements.GaussianIntegers;

IntegerMethods.Initialise(10_000_000);
GaussianIntegerMethods.Initialise(10_000);

for (var i = 1; i <= 300; i++)
{
    var primeFactors = FactorisationMethods<IntegerPrimeIdentifier, int>.GetPrimeFactors(i);

    Console.WriteLine(i + " = " + string.Join('*', primeFactors));
}
