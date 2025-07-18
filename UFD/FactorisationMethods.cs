using System.Numerics;
using UFD.UfdElements;

namespace UFD;

public static class FactorisationMethods<TPrimeIdentifier, T>
    where TPrimeIdentifier : struct, IPrimeIdentifier<T>
    where T :
        struct,
        IAdditionOperators<T, T, T>,
        IAdditiveIdentity<T, T>,
        IDivisionOperators<T, T, T>,
        IEqualityOperators<T, T, bool>,
        IEquatable<T>,
        IModulusOperators<T, T, T>,
        IMultiplicativeIdentity<T, T>,
        IMultiplyOperators<T, T, T>,
        ISubtractionOperators<T, T, T>,
        IUnaryNegationOperators<T, T>,
        IUnaryPlusOperators<T, T>
{
    public static IEnumerable<T> GetPrimeFactors(T x)
    {
        if (x == T.AdditiveIdentity)
            throw new ArgumentException("Cannot factorise Zero!");

        var primeEnumerator = TPrimeIdentifier.GetPrimeList().GetEnumerator();

        primeEnumerator.MoveNext();
        T currentPrime = primeEnumerator.Current;

        while (!TPrimeIdentifier.IsUnit(x))
        {
            if (TPrimeIdentifier.Modulus(x) < TPrimeIdentifier.Modulus(currentPrime))
                yield break;

            TPrimeIdentifier.DivRem(x, currentPrime, out var quotient, out var remainder);

            if (remainder == T.AdditiveIdentity)
            {
                yield return currentPrime;

                x = quotient;
            }
            else if (primeEnumerator.MoveNext())
            {
                currentPrime = primeEnumerator.Current;
            }
            else
            {
                yield break;
            }
        }
    }
}
