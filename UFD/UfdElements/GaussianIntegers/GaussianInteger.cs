using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace UFD.UfdElements.GaussianIntegers;

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
        var newRe = left.Re * right.Re - left.Im * right.Im;
        var newIm = left.Re * right.Im + left.Im * right.Re;

        return new GaussianInteger(newRe, newIm);
    }

    public static GaussianInteger operator /(GaussianInteger left, GaussianInteger right)
    {
        if (right.IsZero)
            throw new DivideByZeroException();

        double bNorm = right.Norm();

        int newRe = left.Re * right.Re + left.Im * right.Im;
        int newIm = left.Im * right.Re - left.Re * right.Im;

        newRe = (int)Math.Round(newRe / bNorm);
        newIm = (int)Math.Round(newIm / bNorm);

        return new GaussianInteger(newRe, newIm);
    }

    public static GaussianInteger operator %(GaussianInteger left, GaussianInteger right)
    {
        var q = left / right;
        q = left - q * right;

        Debug.Assert(q.Norm() < right.Norm(), "Error in division algorithm");

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

    public GaussianInteger ToFirstQuadrant()
    {
        const double PiBy2 = Math.PI / 2;

        var arg = Arg();

        if (0 <= arg && arg < PiBy2)
            return this;

        if (PiBy2 <= arg && arg < Math.PI)
            return this * NegativeI;

        if (-PiBy2 <= arg && arg < 0)
            return this * I;

        return this * NegativeOne;
    }

    public bool IsZero => Re == 0 && Im == 0;

    public double Arg() => Math.Atan2(Im, Re);
    public int Norm() => checked(Re * Re + Im * Im);

    public (GaussianInteger Quotient, GaussianInteger Remainder) DivRem(GaussianInteger other)
    {
        var q = this / other;
        var rem = this - q * other;

        Debug.Assert(rem.Norm() < other.Norm(), "Error in division algorithm");

        return (q, rem);
    }

    public void DivRem(GaussianInteger other, out GaussianInteger quotient, out GaussianInteger remainder)
    {
        quotient = this / other;
        remainder = this - (quotient * other);

        Debug.Assert(remainder.Norm() < other.Norm(), "Error in division algorithm");
    }

    public bool Equals(GaussianInteger other) => Re == other.Re && Im == other.Im;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is GaussianInteger other && Equals(other);
    public override int GetHashCode() =>
        6373897 * Re +
        2408113 * Im;

    public static bool operator ==(GaussianInteger left, GaussianInteger right) => left.Equals(right);
    public static bool operator !=(GaussianInteger left, GaussianInteger right) => !left.Equals(right);

    public override string ToString()
    {
        var source = MemoryMarshal.CreateReadOnlySpan(in Re, 2);
        return MiscHelpers.FormatString(source, 'i');
    }
}
