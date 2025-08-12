using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Mathematics;

public struct Rational<T> : IComparable<Rational<T>>, IEquatable<Rational<T>> where T : IBinaryInteger<T>, ISignedNumber<T>
{
	public T numerator { get; set; }
	public T denominator { get; set; }

	public Rational(T numerator, T denominator)
	{
		if (T.IsNegative(denominator))
		{
			numerator *= T.NegativeOne;
			denominator *= T.NegativeOne;
		}

		static T GCD(T a, T b)
		{
			a = T.Abs(a);
			b = T.Abs(b);
			while (T.IsPositive(a))
			{
				b %= a;
				var x = a;
				a = b;
				b = x;
			}
			return b;
		}

		var gcd = GCD(numerator, denominator);
		numerator /= gcd;
		denominator /= gcd;

		this.numerator = numerator;
		this.denominator = denominator;
	}

	public Rational(T integer)
	{
		numerator = integer;
		denominator = T.One;
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

	public static Rational<T> operator +(Rational<T> l, Rational<T> r) => new Rational<T>(l.numerator * r.denominator + l.denominator * r.numerator, l.denominator * r.denominator);
	public static Rational<T> operator -(Rational<T> l, Rational<T> r) => new Rational<T>(l.numerator * r.denominator - l.denominator * r.numerator, l.denominator * r.denominator);
	public static Rational<T> operator *(Rational<T> l, Rational<T> r) => new Rational<T>(l.numerator * r.numerator, l.denominator * r.denominator);
	public static Rational<T> operator /(Rational<T> l, Rational<T> r) => new Rational<T>(l.numerator * r.denominator, l.denominator * r.numerator);

	public static Rational<T> operator ++(Rational<T> x)
	{
		var tmp = x + T.One;
		return tmp;
	}
	public static Rational<T> operator --(Rational<T> x)
	{
		var tmp = x - T.One;
		return tmp;
	}

	public static bool operator >(Rational<T> l, Rational<T> r) => l.CompareTo(r) > 0;
	public static bool operator <(Rational<T> l, Rational<T> r) => l.CompareTo(r) < 0;
	public static bool operator >=(Rational<T> l, Rational<T> r) => l.CompareTo(r) >= 0;
	public static bool operator <=(Rational<T> l, Rational<T> r) => l.CompareTo(r) <= 0;
	public static bool operator ==(Rational<T> l, Rational<T> r) => l.Equals(r);
	public static bool operator !=(Rational<T> l, Rational<T> r) => !l.Equals(r);

	public static implicit operator Rational<T>(T x) => new Rational<T>(x);

	public override string ToString() => $"{numerator}/{denominator}";

	public int CompareTo(Rational<T> x)
	{
		var diff = this - x;
		if (T.IsPositive(diff.numerator)) return 1;
		if (T.IsNegative(diff.numerator)) return -1;
		return 0;
	}

	public bool Equals(Rational<T> item) => denominator == item.denominator && numerator == item.numerator;

	public override bool Equals(object? obj) => obj is Rational<T> r && numerator == r.numerator && denominator == r.denominator;

	public override int GetHashCode()
	{
		return HashCode.Combine(numerator, denominator);
	}
}
