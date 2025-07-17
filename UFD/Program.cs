using UFD.UfdElements;
using UFD.UfdElements.GaussianIntegers;

IntegerMethods.Initialise(10_000_000);
GaussianIntegerMethods.Initialise(10_000);

var firstHundredPrimes = new IntegerMethods.PrimeEnumerable().Take(100);

foreach (var prime in firstHundredPrimes)
{
    Console.WriteLine(prime);
}
