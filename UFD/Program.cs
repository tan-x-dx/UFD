using UFD;

IntegerMethods.Initialise(10_000_000);

var firstHundredPrimes = new IntegerMethods.PrimeEnumerable().Take(100);

foreach (var prime in firstHundredPrimes)
{
    Console.WriteLine(prime);
}
