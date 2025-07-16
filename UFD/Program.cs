using UFD;

var firstHundredPrimes = new IntegerMethods.PrimeEnumerable().Take(100);

foreach(var prime in firstHundredPrimes)
{
    Console.WriteLine(prime);
}
