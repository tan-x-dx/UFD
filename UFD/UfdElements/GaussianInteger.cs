using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace UFD.UfdElements;

public readonly struct GaussianInteger(int re, int im) :
    IAdditionOperators<GaussianInteger, GaussianInteger, GaussianInteger>,
    IAdditiveIdentity<GaussianInteger, GaussianInteger>,
    IDivisionOperators<GaussianInteger, GaussianInteger, GaussianInteger>,
    IEqualityOperators<GaussianInteger, GaussianInteger, bool>,
    IEquatable<GaussianInteger>,
    IModulusOperators<GaussianInteger, GaussianInteger, GaussianInteger>,
    IMultiplicativeIdentity<GaussianInteger, GaussianInteger>,
    IMultiplyOperators<GaussianInteger, GaussianInteger, GaussianInteger>,
    ISubtractionOperators<GaussianInteger, GaussianInteger, GaussianInteger>,
    IUnaryNegationOperators<GaussianInteger, GaussianInteger>,
    IUnaryPlusOperators<GaussianInteger, GaussianInteger>
{
    public readonly int Re = re;
    public readonly int Im = im;

    public static readonly GaussianInteger Zero = new(0, 0);
    public static readonly GaussianInteger One = new(1, 0);
    public static readonly GaussianInteger NegativeOne = new(-1, 0);
    public static readonly GaussianInteger I = new(0, 1);
    public static readonly GaussianInteger NegativeI = new(0, -1);

    public static GaussianInteger AdditiveIdentity { get; } = Zero;
    public static GaussianInteger MultiplicativeIdentity { get; } = One;

    public static GaussianInteger operator +(GaussianInteger value) => value;
    public static GaussianInteger operator +(GaussianInteger left, GaussianInteger right) =>
        new(left.Re + right.Re, left.Im + right.Im);

    public static GaussianInteger operator -(GaussianInteger value) => new(-value.Re, -value.Im);
    public static GaussianInteger operator -(GaussianInteger left, GaussianInteger right) =>
        new(left.Re - right.Re, left.Im - right.Im);

    public static GaussianInteger operator *(GaussianInteger left, GaussianInteger right)
    {
        var newRe = (left.Re * right.Re) - (left.Im * right.Im);
        var newIm = (left.Re * right.Im) + (left.Im * right.Re);

        return new GaussianInteger(newRe, newIm);
    }

    public static GaussianInteger operator /(GaussianInteger left, GaussianInteger right)
    {
        if (right.IsZero)
            throw new DivideByZeroException();

        double bNorm = right.NormSquared();

        int newRe = (left.Re * right.Re) + (left.Im * right.Re);
        int newIm = (left.Im * right.Re) - (left.Re * right.Im);

        newRe = (int)Math.Round(newRe / bNorm);
        newIm = (int)Math.Round(newIm / bNorm);

        return new GaussianInteger(newRe, newIm);
    }

    public static GaussianInteger operator %(GaussianInteger left, GaussianInteger right)
    {
        var q = left / right;
        q = left - (q * right);

        Debug.Assert(q.NormSquared() < right.NormSquared(), "Error in division algorithm");

        return q;
    }

    /// <summary>
    /// Complex conjugate operator.
    /// </summary>
    /// <param name="value">The Gaussian Integer to conjugate</param>
    /// <returns>The complex conjugate of this Gaussian Integer</returns>
    public static GaussianInteger operator ~(GaussianInteger value) => new(value.Re, -value.Im);

    public static implicit operator GaussianInteger(int n) => new(n, 0);
    public static GaussianInteger operator +(GaussianInteger g, int n) => new(g.Re + n, g.Im);
    public static GaussianInteger operator +(int n, GaussianInteger g) => new(n + g.Re, g.Im);
    public static GaussianInteger operator -(GaussianInteger g, int n) => new(g.Re - n, g.Im);
    public static GaussianInteger operator -(int n, GaussianInteger g) => new(n - g.Re, g.Im);
    public static GaussianInteger operator *(GaussianInteger g, int n) => new(g.Re * n, g.Im * n);
    public static GaussianInteger operator *(int n, GaussianInteger g) => new(n * g.Re, n * g.Im);

    public bool IsZero => Re == 0 && Im == 0;

    public int NormSquared() => checked((Re * Re) + (Im * Im));

    public (GaussianInteger Quotient, GaussianInteger Remainder) DivRem(GaussianInteger other)
    {
        var q = this / other;
        var rem = this - (q * other);

        Debug.Assert(rem.NormSquared() < other.NormSquared(), "Error in division algorithm");

        return (q, rem);
    }

    public void DivRem(GaussianInteger other, out GaussianInteger quotient, out GaussianInteger remainder)
    {
        quotient = this / other;
        remainder = this - (quotient * other);

        Debug.Assert(remainder.NormSquared() < other.NormSquared(), "Error in division algorithm");
    }

    public bool Equals(GaussianInteger other) => Re == other.Re && Im == other.Im;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is GaussianInteger other && Equals(other);
    public override int GetHashCode() =>
        6373897 * Re +
        2408113 * Im;

    public static bool operator ==(GaussianInteger left, GaussianInteger right) => left.Equals(right);
    public static bool operator !=(GaussianInteger left, GaussianInteger right) => !left.Equals(right);
}

public readonly struct GaussianPrimeIdentifier : IPrimeIdentifier<GaussianInteger>
{
    public static bool IsUnit(GaussianInteger x) => Math.Abs(x.Re) == 1 && Math.Abs(x.Im) == 1;

    public static bool IsPrime(GaussianInteger x)
    {
        if (x.Im == 0)
        {
            return ((x.Re & 3) == 3) &&
                IntegerMethods.IsPrime(x.Re);
        }

        var normSquared = x.NormSquared();
        if ((normSquared & 3) == 1)
            return IntegerMethods.IsPrime(normSquared);

        return Math.Abs(x.Re) == 1 && Math.Abs(x.Im) == 1;
    }
}

public static class GaussianIntegerHelpers
{
#pragma warning disable IDE1006 // Naming Styles
    public static GaussianInteger i(this int k) => new(0, k);
#pragma warning restore IDE1006 // Naming Styles
}
