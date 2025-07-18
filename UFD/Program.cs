using UFD.UfdElements;
using UFD.UfdElements.GaussianIntegers;

IntegerMethods.Initialise(100_000_000);
GaussianIntegerMethods.Initialise(10_000);

var primes = IntegerMethods.GetAllPrimes();

var n = 11638725;
var primeFactorsOfN = IntFactorisation.GetPrimeFactors(n);
Console.WriteLine($"{n} = {string.Join('*', primeFactorsOfN)}");

var g = 13 + 21.i();
var primeFactorsOfG = GaussianIntegerFactorisation.GetPrimeFactors(g);
Console.WriteLine($"{g} -> {string.Join(',', primeFactorsOfG)}");
