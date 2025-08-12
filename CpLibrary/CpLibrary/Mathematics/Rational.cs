using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Mathematics;

public struct Rational<T>
	: IComparable<Rational<T>>, IEquatable<Rational<T>>, ISignedNumber<Rational<T>>
	where T : IBinaryInteger<T>, ISignedNumber<T>
{
	public T Numerator { get; set; }
	public T Denominator { get; set; }

	public Rational(T numerator, T denominator)
	{
		if (denominator < T.Zero)
		{
			numerator *= T.NegativeOne;
			denominator *= T.NegativeOne;
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

	public static Rational<T> Parse(string s, IFormatProvider? provider = null)
	{
		var sarray = s.Split('/');
		if (sarray.Length == 1)
			return new Rational<T>(T.Parse(sarray[0], provider));
		if (sarray.Length == 2)
			return new Rational<T>(T.Parse(sarray[0], provider), T.Parse(sarray[1], provider));
		throw new ArgumentException("Invalid argument input.");
	}

	public static Rational<T> operator +(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator + l.Denominator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator -(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator - l.Denominator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator *(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Numerator, l.Denominator * r.Denominator);
	public static Rational<T> operator /(Rational<T> l, Rational<T> r) => new Rational<T>(l.Numerator * r.Denominator, l.Denominator * r.Numerator);

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

	public override string ToString() => $"{Numerator}/{Denominator}";

	public int CompareTo(Rational<T> x)
	{
		var diff = this - x;
		if (diff.Numerator > T.Zero) return 1;
		if (diff.Numerator < T.Zero) return -1;
		return 0;
	}

	public bool Equals(Rational<T> item) => Denominator == item.Denominator && Numerator == item.Numerator;

	public override bool Equals(object? obj) => obj is Rational<T> r && Numerator == r.Numerator && Denominator == r.Denominator;

	public override int GetHashCode()
	{
		return HashCode.Combine(Numerator, Denominator);
	}
}
