using System.Numerics;

namespace UFD.UfdElements;

public interface IPrimeIdentifier<T>
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
    static abstract int Norm(T x);
    static abstract double Modulus(T x);
    static abstract bool IsUnit(T x);
    static abstract bool IsPrime(T x);

    static abstract void DivRem(T left, T right, out T quotient, out T remainder);

    static abstract IEnumerable<T> GetPrimeList();
}
