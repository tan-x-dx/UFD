using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.InteropServices;

namespace UFD.UfdElements.EisensteinIntegers;

public readonly struct EisensteinInteger(int re, int om) :
    IAdditionOperators<EisensteinInteger, EisensteinInteger, EisensteinInteger>,
    IAdditiveIdentity<EisensteinInteger, EisensteinInteger>,
    IDivisionOperators<EisensteinInteger, EisensteinInteger, EisensteinInteger>,
    IEqualityOperators<EisensteinInteger, EisensteinInteger, bool>,
    IEquatable<EisensteinInteger>,
    IModulusOperators<EisensteinInteger, EisensteinInteger, EisensteinInteger>,
    IMultiplicativeIdentity<EisensteinInteger, EisensteinInteger>,
    IMultiplyOperators<EisensteinInteger, EisensteinInteger, EisensteinInteger>,
    ISubtractionOperators<EisensteinInteger, EisensteinInteger, EisensteinInteger>,
    IUnaryNegationOperators<EisensteinInteger, EisensteinInteger>,
    IUnaryPlusOperators<EisensteinInteger, EisensteinInteger>
{
    // Math.Sqrt(3) / 2
    public const double Sqrt3By2 = 0.86602540378443864676372317d;

    public readonly int Re = re;
    public readonly int Om = om;

    public static readonly EisensteinInteger Zero = new(0, 0);
    public static readonly EisensteinInteger One = new(1, 0);
    public static readonly EisensteinInteger NegativeOne = new(-1, 0);
    public static readonly EisensteinInteger Omega = new(0, 1);
    public static readonly EisensteinInteger OmegaSquared = new(-1, -1);
    public static readonly EisensteinInteger NegativeOmega = new(0, -1);

    public static EisensteinInteger AdditiveIdentity { get; } = Zero;
    public static EisensteinInteger MultiplicativeIdentity { get; } = One;

    public static EisensteinInteger operator +(EisensteinInteger value) => value;
    public static EisensteinInteger operator +(EisensteinInteger left, EisensteinInteger right) =>
        new(left.Re + right.Re, left.Om + right.Om);

    public static EisensteinInteger operator -(EisensteinInteger value) => new(-value.Re, -value.Om);
    public static EisensteinInteger operator -(EisensteinInteger left, EisensteinInteger right) =>
        new(left.Re - right.Re, left.Om - right.Om);

    public static EisensteinInteger operator *(EisensteinInteger left, EisensteinInteger right)
    {
        int newRe = left.Re * right.Re - left.Om * right.Om;
        int newOm = left.Om * right.Re + right.Om * (left.Re - left.Om);

        return new EisensteinInteger(newRe, newOm);
    }

    public static EisensteinInteger operator /(EisensteinInteger left, EisensteinInteger right)
    {
        if (right.IsZero)
            throw new DivideByZeroException();

        double newRe = left.Re * right.Re + right.Om * (left.Om - left.Re);
        double newOm = left.Om * right.Re - left.Re * right.Om;

        double bNorm = right.NormSquared();

        int newRe2 = (int)Math.Round(newRe / bNorm);
        int newOm2 = (int)Math.Round(newOm / bNorm);

        return new EisensteinInteger(newRe2, newOm2);
    }

    public static EisensteinInteger operator %(EisensteinInteger left, EisensteinInteger right)
    {
        var q = left / right;
        q = left - q * right;

        Debug.Assert(q.NormSquared() < right.NormSquared(), "Error in division algorithm");

        return q;
    }

    /// <summary>
    /// Complex conjugate operator.
    /// </summary>
    /// <param name="value">The Eisenstein Integer to conjugate</param>
    /// <returns>The complex conjugate of this Eisenstein Integer</returns>
    public static EisensteinInteger operator ~(EisensteinInteger value) => new(value.Re, -value.Om);

    public static implicit operator EisensteinInteger(int n) => new(n, 0);
    public static EisensteinInteger operator +(EisensteinInteger g, int n) => new(g.Re + n, g.Om);
    public static EisensteinInteger operator +(int n, EisensteinInteger g) => new(n + g.Re, g.Om);
    public static EisensteinInteger operator -(EisensteinInteger g, int n) => new(g.Re - n, g.Om);
    public static EisensteinInteger operator -(int n, EisensteinInteger g) => new(n - g.Re, g.Om);
    public static EisensteinInteger operator *(EisensteinInteger g, int n) => new(g.Re * n, g.Om * n);
    public static EisensteinInteger operator *(int n, EisensteinInteger g) => new(n * g.Re, n * g.Om);

    public bool IsZero => Re == 0 && Om == 0;

    public double RealPart() => Re - 0.5d * Om;
    public double ImaginaryPart() => Om * Sqrt3By2;

    public int NormSquared() => checked(Re * Re + Om * Om - Re * Om);

    public (EisensteinInteger Quotient, EisensteinInteger Remainder) DivRem(EisensteinInteger other)
    {
        var q = this / other;
        var rem = this - q * other;

        Debug.Assert(rem.NormSquared() < other.NormSquared(), "Error in division algorithm");

        return (q, rem);
    }

    public void DivRem(EisensteinInteger other, out EisensteinInteger quotient, out EisensteinInteger remainder)
    {
        quotient = this / other;
        remainder = this - quotient * other;

        Debug.Assert(remainder.NormSquared() < other.NormSquared(), "Error in division algorithm");
    }

    public bool Equals(EisensteinInteger other) => Re == other.Re && Om == other.Om;
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is EisensteinInteger other && Equals(other);
    public override int GetHashCode() =>
        7511969 * Re +
        4088521 * Om;

    public static bool operator ==(EisensteinInteger left, EisensteinInteger right) => left.Equals(right);
    public static bool operator !=(EisensteinInteger left, EisensteinInteger right) => !left.Equals(right);

    public override string ToString()
    {
        var source = MemoryMarshal.CreateReadOnlySpan(in Re, 2);
        return MiscHelpers.FormatString(source, 'ω');
    }
}
