using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace CpLibrary.Mathematics;

public readonly struct Rational<T>
	: IComparable<Rational<T>>, IEquatable<Rational<T>>, INumber<Rational<T>>, ISignedNumber<Rational<T>>
	where T : IBinaryInteger<T>, ISignedNumber<T>
{
	public T Numerator { get; }
	public T Denominator { get; }

	public static int Radix => 2;
	public static Rational<T> One => new Rational<T>(T.One);
	public static Rational<T> Zero => new Rational<T>(T.Zero);
	public static Rational<T> NegativeOne => new Rational<T>(T.NegativeOne);
	public static Rational<T> AdditiveIdentity => Zero;
	public static Rational<T> MultiplicativeIdentity => One;

	public Rational(T numerator, T denominator)
	{
		if (denominator < T.Zero)
		{
			numerator *= T.NegativeOne;
			denominator *= T.NegativeOne;
		}

		var gcd = GCD(numerator, denominator);
		numerator /= gcd;
		denominator /= gcd;

		this.Numerator = numerator;
		this.Denominator = denominator;
	}

	public Rational(T integer)
	{
		Numerator = integer;
		Denominator = T.One;
	}

	static T GCD(T a, T b)
	{
		a = T.Abs(a);
		b = T.Abs(b);
		while (a > T.Zero)
		{
			b %= a;
			var x = a;
			a = b;
			b = x;
		}
		return b;
	}

	public static Rational<T> operator +(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator + l.Denominator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator -(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator - l.Denominator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator *(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator /(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator, l.Denominator * r.Numerator);

	public static Rational<T> operator ++(Rational<T> x) => x + T.One;
	public static Rational<T> operator --(Rational<T> x) => x - T.One;

	public static Rational<T> operator +(Rational<T> value) => value;
	public static Rational<T> operator -(Rational<T> value) => value * T.NegativeOne;

	public static bool operator >(Rational<T> l, Rational<T> r) => l.CompareTo(r) > 0;
	public static bool operator <(Rational<T> l, Rational<T> r) => l.CompareTo(r) < 0;
	public static bool operator >=(Rational<T> l, Rational<T> r) => l.CompareTo(r) >= 0;
	public static bool operator <=(Rational<T> l, Rational<T> r) => l.CompareTo(r) <= 0;
	public static bool operator ==(Rational<T> l, Rational<T> r) => l.Equals(r);
	public static bool operator !=(Rational<T> l, Rational<T> r) => !l.Equals(r);

	public static implicit operator Rational<T>(T x) => new Rational<T>(x);

	public static Rational<T> operator %(Rational<T> left, Rational<T> right)
	{
		throw new NotImplementedException();
	}

	public override string ToString() => $"{Numerator}/{Denominator}";

	public int CompareTo(Rational<T> x)
	{
		var diff = this - x;
		if (diff.Numerator > T.Zero) return 1;
		if (diff.Numerator < T.Zero) return -1;
		return 0;
	}

	public int CompareTo(object? obj)
	{
		if (obj is null) return 1;
		if (obj is Rational<T> other) return CompareTo(other);
		throw new ArgumentException($"Object must be of type {nameof(Rational<T>)}.");
	}

	public bool Equals(Rational<T> item) => Denominator == item.Denominator && Numerator == item.Numerator;

	public override bool Equals(object? obj) => obj is Rational<T> r && Numerator == r.Numerator && Denominator == r.Denominator;

	public override int GetHashCode()
	{
		return HashCode.Combine(Numerator, Denominator);
	}

	public static Rational<T> Abs(Rational<T> value) => IsPositive(value) ? value : -value;
	public static bool IsCanonical(Rational<T> value) => value.Denominator >= T.One && GCD(value.Numerator, value.Denominator) == T.One;
	public static bool IsComplexNumber(Rational<T> value) => false;
	public static bool IsEvenInteger(Rational<T> value) => IsInteger(value) && T.IsEvenInteger(value.Numerator);
	public static bool IsFinite(Rational<T> value) => !IsInfinity(value);
	public static bool IsImaginaryNumber(Rational<T> value) => false;
	public static bool IsInfinity(Rational<T> value) => IsPositiveInfinity(value) || IsNegativeInfinity(value);
	public static bool IsInteger(Rational<T> value) => value.Denominator == T.One;
	public static bool IsNaN(Rational<T> value) => T.IsNaN(value.Numerator) || T.IsNaN(value.Denominator);
	public static bool IsNegative(Rational<T> value) => T.IsNegative(value.Numerator);
	public static bool IsNegativeInfinity(Rational<T> value) => T.IsNegativeInfinity(value.Numerator);
	public static bool IsNormal(Rational<T> value) => true;
	public static bool IsOddInteger(Rational<T> value) => IsInteger(value) && T.IsOddInteger(value.Numerator);
	public static bool IsPositive(Rational<T> value) => T.IsPositive(value.Numerator);
	public static bool IsPositiveInfinity(Rational<T> value) => T.IsPositiveInfinity(value.Numerator);
	public static bool IsRealNumber(Rational<T> value) => true;
	public static bool IsSubnormal(Rational<T> value) => false;
	public static bool IsZero(Rational<T> value) => T.IsZero(value.Numerator);
	public static Rational<T> MaxMagnitude(Rational<T> x, Rational<T> y) => x > y ? x : y;
	public static Rational<T> MaxMagnitudeNumber(Rational<T> x, Rational<T> y)
	{
		if (IsNaN(x)) return y;
		if (IsNaN(y)) return x;
		return MaxMagnitude(x, y);
	}
	public static Rational<T> MinMagnitude(Rational<T> x, Rational<T> y) => x < y ? x : y;
	public static Rational<T> MinMagnitudeNumber(Rational<T> x, Rational<T> y)
	{
		if (IsNaN(x)) return y;
		if (IsNaN(y)) return x;
		return MinMagnitude(x, y);
	}
	public static Rational<T> Parse(string s, IFormatProvider? provider = null)
	{
		var sarray = s.Split('/');
		if (sarray.Length == 1)
			return new Rational<T>(T.Parse(sarray[0], provider));
		if (sarray.Length == 2)
			return new Rational<T>(T.Parse(sarray[0], provider), T.Parse(sarray[1], provider));
		throw new ArgumentException("Invalid argument input.");
	}

	public static Rational<T> Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static Rational<T> Parse(string s, NumberStyles style, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static Rational<T> Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertFromChecked<TOther>(TOther value, [MaybeNullWhen(false)] out Rational<T> result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertFromSaturating<TOther>(TOther value, [MaybeNullWhen(false)] out Rational<T> result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertFromTruncating<TOther>(TOther value, [MaybeNullWhen(false)] out Rational<T> result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertToChecked<TOther>(Rational<T> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertToSaturating<TOther>(Rational<T> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryConvertToTruncating<TOther>(Rational<T> value, [MaybeNullWhen(false)] out TOther result) where TOther : INumberBase<TOther>
	{
		throw new NotImplementedException();
	}

	public static bool TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Rational<T> result)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse([NotNullWhen(true)] string? s, NumberStyles style, IFormatProvider? provider, [MaybeNullWhen(false)] out Rational<T> result)
	{
		throw new NotImplementedException();
	}

	public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
	{
		throw new NotImplementedException();
	}

	public string ToString(string? format, IFormatProvider? formatProvider) => ToString();

	public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rational<T> result)
	{
		throw new NotImplementedException();
	}

	public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Rational<T> result)
	{
		throw new NotImplementedException();
	}
}
