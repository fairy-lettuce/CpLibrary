using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AtCoder.Internal;
using System.Globalization;
using System.Numerics;
using AtCoder;

namespace CpLibrary.Mathematics;

// Modified version of ac-library-csharp StaticModInt<T> (licensed under CC0)
// https://github.com/kzrnm/ac-library-csharp/blob/main/Source/ac-library-csharp/Math/ModInt/StaticModInt.cs

/// <summary>
/// 四則演算時に自動で mod を取る整数型。mod の値はコンパイル時に決定している必要があります。
/// 積を Montgomery reduction により高速化します。そのため mod は奇数である必要があります。
/// </summary>
/// <typeparam name="T">定数 mod を表す構造体</typeparam>
/// <example>
/// <code>
/// using ModInt = AtCoder.StaticModInt&lt;AtCoder.Mod1000000007&gt;;
///
/// void SomeMethod()
/// {
///     var m = new ModInt(1);
///     m -= 2;
///     Console.WriteLine(m);   // 1000000006
/// }
/// </code>
/// </example>
[DebuggerDisplay("{Value,nq}")]
public readonly struct MontgomeryModInt<T> : IEquatable<MontgomeryModInt<T>>, IFormattable, IModInt<MontgomeryModInt<T>>
	where T : struct, IStaticMod
{
	internal readonly uint _v;
	private static readonly T op = default;
	internal static readonly StaticMontgomeryReduction<T> mr = default;

	/// <summary>
	/// 格納されている値を返します。
	/// </summary>
	public int Value => (int)mr.ToInteger(_v);

	/// <summary>
	/// mod を返します。
	/// </summary>
	public static int Mod => (int)op.Mod;
	public static MontgomeryModInt<T> Zero => default;
	public static MontgomeryModInt<T> One => new MontgomeryModInt<T>(1L);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static int Normalize(int v)
	{
		return v >= Mod ? v - Mod : v;
	}

	/// <summary>
	/// <paramref name="v"/> に対して mod を取らずに StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
	/// </summary>
	/// <remarks>
	/// <para>定数倍高速化のための関数です。 <paramref name="v"/> に 0 未満または mod 以上の値を入れた場合の挙動は未定義です。</para>
	/// <para>制約: 0≤|<paramref name="v"/>|&lt;mod</para>
	/// </remarks>
	[MethodImpl(256)]
	public static MontgomeryModInt<T> Raw(int v)
	{
		var u = unchecked((uint)v);
#if EMBEDDING
            Contract.Assert(u < Mod, $"{nameof(u)} must be less than {nameof(Mod)}.");
#endif
		return new MontgomeryModInt<T>(u);
	}

	/// <summary>
	/// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
	/// </summary>
	/// <remarks>
	/// <paramref name="v"/>が 0 未満、もしくは mod 以上の場合、自動で mod を取ります。
	/// </remarks>
	[MethodImpl(256)]
	public MontgomeryModInt(long v) : this((uint)mr.ToMontgomery(v)) { }

	/// <summary>
	/// StaticModInt&lt;<typeparamref name="T"/>&gt; 型のインスタンスを生成します。
	/// </summary>
	/// <remarks>
	/// <paramref name="v"/>が mod 以上の場合、自動で mod を取ります。
	/// </remarks>
	[MethodImpl(256)]
	public MontgomeryModInt(ulong v) : this((uint)mr.ToMontgomery(v)) { }

	[MethodImpl(256)]
	private MontgomeryModInt(uint v) => _v = v;

	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator ++(MontgomeryModInt<T> v)
	{
		var x = v._v + One._v;
		if (x == op.Mod)
		{
			x = 0;
		}
		return new MontgomeryModInt<T>(x);
	}

	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator --(MontgomeryModInt<T> v)
	{
		var x = v._v;
		if (x == 0)
		{
			x = op.Mod;
		}
		return new MontgomeryModInt<T>(x - One._v);
	}

	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator +(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs)
	{
		var v = lhs._v + rhs._v;
		if (v >= 2 * op.Mod)
		{
			v -= 2 * op.Mod;
		}
		return new MontgomeryModInt<T>(v);
	}

	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator -(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs)
	{
		unchecked
		{
			var v = lhs._v - rhs._v;
			if ((int)v < 0)
			{
				v += 2 * op.Mod;
			}
			return new MontgomeryModInt<T>(v);
		}
	}

	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator *(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs) => new MontgomeryModInt<T>((uint)mr.Mult(lhs._v, rhs._v));
	/// <summary>
	/// 除算を行います。
	/// </summary>
	/// <remarks>
	/// <para>- 制約: <paramref name="rhs"/> に乗法の逆元が存在する。（gcd(<paramref name="rhs"/>, mod) = 1）</para>
	/// <para>- 計算量: O(log(mod))</para>
	/// </remarks>
	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator /(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs) => lhs * rhs.Inv();
	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator +(MontgomeryModInt<T> v) => v;
	[MethodImpl(256)]
	public static MontgomeryModInt<T> operator -(MontgomeryModInt<T> v) => new MontgomeryModInt<T>(v._v == 0 ? 0 : op.Mod - v._v);
	[MethodImpl(256)]
	public static bool operator ==(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs) => Normalize((int)lhs._v) == Normalize((int)rhs._v);
	[MethodImpl(256)]
	public static bool operator !=(MontgomeryModInt<T> lhs, MontgomeryModInt<T> rhs) => Normalize((int)lhs._v) != Normalize((int)rhs._v);
	[MethodImpl(256)]
	public static implicit operator MontgomeryModInt<T>(int v) => new MontgomeryModInt<T>(v);
	[MethodImpl(256)]
	public static implicit operator MontgomeryModInt<T>(uint v) => new MontgomeryModInt<T>((ulong)v);
	[MethodImpl(256)]
	public static implicit operator MontgomeryModInt<T>(long v) => new MontgomeryModInt<T>(v);
	[MethodImpl(256)]
	public static implicit operator MontgomeryModInt<T>(ulong v) => new MontgomeryModInt<T>(v);

	/// <summary>
	/// 自身を x として、x^<paramref name="n"/> を返します。
	/// </summary>
	/// <remarks>
	/// <para>制約: 0≤|<paramref name="n"/>|</para>
	/// <para>計算量: O(log(<paramref name="n"/>))</para>
	/// </remarks>
	[MethodImpl(256)]
	public MontgomeryModInt<T> Pow(long n)
	{
		Contract.Assert(0 <= n, $"{nameof(n)} must be positive.");
		return Pow((ulong)n);
	}

	/// <summary>
	/// 自身を x として、x^<paramref name="n"/> を返します。
	/// </summary>
	/// <remarks>
	/// <para>制約: 0≤|<paramref name="n"/>|</para>
	/// <para>計算量: O(log(<paramref name="n"/>))</para>
	/// </remarks>
	[MethodImpl(256)]
	public MontgomeryModInt<T> Pow(ulong n)
	{
		var x = (uint)this._v;
		var r = mr.ToMontgomery(1UL);

		while (n > 0)
		{
			if ((n & 1) > 0)
			{
				r = mr.Mult(r, x);
			}
			x = mr.Mult(x, x);
			n >>= 1;
		}

		return new MontgomeryModInt<T>((uint)r);
	}

	/// <summary>
	/// 自身を x として、 xy≡1 なる y を返します。
	/// </summary>
	/// <remarks>
	/// <para>制約: gcd(x, mod) = 1</para>
	/// </remarks>
	[MethodImpl(256)]
	public MontgomeryModInt<T> Inv()
	{
		if (op.IsPrime)
		{
			Contract.Assert(_v > 0, reason: $"{nameof(Value)} must be positive.");
			return Pow(op.Mod - 2);
		}
		else
		{
			var (g, x) = ModCalc.InvGcd(_v, op.Mod);
			Contract.Assert(g == 1, reason: $"gcd({nameof(x)}, {nameof(Mod)}) must be 1.");
			return new MontgomeryModInt<T>(x);
		}
	}

	public override string ToString() => Value.ToString();
	public string ToString(string format, IFormatProvider formatProvider) => Value.ToString(format, formatProvider);
	public override bool Equals(object obj) => obj is MontgomeryModInt<T> m && Equals(m);
	[MethodImpl(256)] public bool Equals(MontgomeryModInt<T> other) => _v == other._v;
	public override int GetHashCode() => _v.GetHashCode();
	public static bool TryParse(ReadOnlySpan<char> s, out MontgomeryModInt<T> result)
	{
		result = Zero;
		MontgomeryModInt<T> ten = 10u;
		s = s.Trim();
		bool minus = false;
		if (s.Length > 0 && s[0] == '-')
		{
			minus = true;
			s = s.Slice(1);
		}
		for (int i = 0; i < s.Length; i++)
		{
			var d = (uint)(s[i] - '0');
			if (d >= 10) return false;
			result = result * ten + d;
		}
		if (minus)
			result = -result;
		return true;
	}
	public static MontgomeryModInt<T> Parse(ReadOnlySpan<char> s)
	{
		if (!TryParse(s, out var r))
			Throw();
		return r;
		void Throw() => throw new FormatException();
	}

	static int INumberBase<MontgomeryModInt<T>>.Radix => 2;
	static MontgomeryModInt<T> IAdditiveIdentity<MontgomeryModInt<T>, MontgomeryModInt<T>>.AdditiveIdentity => default;
	static MontgomeryModInt<T> IMultiplicativeIdentity<MontgomeryModInt<T>, MontgomeryModInt<T>>.MultiplicativeIdentity => One;
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Abs(MontgomeryModInt<T> v) => v;
	static bool INumberBase<MontgomeryModInt<T>>.IsCanonical(MontgomeryModInt<T> v) => true;
	static bool INumberBase<MontgomeryModInt<T>>.IsComplexNumber(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsRealNumber(MontgomeryModInt<T> v) => true;
	static bool INumberBase<MontgomeryModInt<T>>.IsImaginaryNumber(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsEvenInteger(MontgomeryModInt<T> v) => uint.IsEvenInteger(v._v);
	static bool INumberBase<MontgomeryModInt<T>>.IsOddInteger(MontgomeryModInt<T> v) => uint.IsOddInteger(v._v);
	static bool INumberBase<MontgomeryModInt<T>>.IsFinite(MontgomeryModInt<T> v) => true;
	static bool INumberBase<MontgomeryModInt<T>>.IsInfinity(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsInteger(MontgomeryModInt<T> v) => true;
	static bool INumberBase<MontgomeryModInt<T>>.IsPositive(MontgomeryModInt<T> v) => true;
	static bool INumberBase<MontgomeryModInt<T>>.IsNegative(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsPositiveInfinity(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsNegativeInfinity(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsNormal(MontgomeryModInt<T> v) => v._v != 0;
	static bool INumberBase<MontgomeryModInt<T>>.IsSubnormal(MontgomeryModInt<T> v) => false;
	static bool INumberBase<MontgomeryModInt<T>>.IsZero(MontgomeryModInt<T> v) => v._v == 0;
	static bool INumberBase<MontgomeryModInt<T>>.IsNaN(MontgomeryModInt<T> v) => false;
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Max(x._v, y._v));
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MaxMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Max(x._v, y._v));
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitude(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Min(x._v, y._v));
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.MinMagnitudeNumber(MontgomeryModInt<T> x, MontgomeryModInt<T> y) => new MontgomeryModInt<T>(uint.Min(x._v, y._v));

	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider) => Parse(s);
	static MontgomeryModInt<T> INumberBase<MontgomeryModInt<T>>.Parse(string s, NumberStyles style, IFormatProvider provider) => Parse(s);
	static MontgomeryModInt<T> ISpanParsable<MontgomeryModInt<T>>.Parse(ReadOnlySpan<char> s, IFormatProvider provider) => Parse(s);
	static MontgomeryModInt<T> IParsable<MontgomeryModInt<T>>.Parse(string s, IFormatProvider provider) => Parse(s);
	static bool ISpanParsable<MontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, IFormatProvider provider, out MontgomeryModInt<T> result) => TryParse(s, out result);
	static bool IParsable<MontgomeryModInt<T>>.TryParse(string s, IFormatProvider provider, out MontgomeryModInt<T> result) => TryParse(s, out result);
	static bool INumberBase<MontgomeryModInt<T>>.TryParse(ReadOnlySpan<char> s, NumberStyles style, IFormatProvider provider, out MontgomeryModInt<T> result) => TryParse(s, out result);
	static bool INumberBase<MontgomeryModInt<T>>.TryParse(string s, NumberStyles style, IFormatProvider provider, out MontgomeryModInt<T> result) => TryParse(s, out result);

	bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider provider) => _v.TryFormat(destination, out charsWritten, format, provider);


	static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromChecked<TOther>(TOther v, out MontgomeryModInt<T> r)
	{
		if (WrapChecked(v, out long l))
		{
			r = l;
			return true;
		}
		if (WrapChecked(v, out ulong u))
		{
			r = u;
			return true;
		}
		r = default;
		return false;
	}
	static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromSaturating<TOther>(TOther v, out MontgomeryModInt<T> r)
	{
		if (WrapSaturating(v, out long l))
		{
			r = l;
			return true;
		}
		if (WrapSaturating(v, out ulong u))
		{
			r = u;
			return true;
		}
		r = default;
		return false;
	}
	static bool INumberBase<MontgomeryModInt<T>>.TryConvertFromTruncating<TOther>(TOther v, out MontgomeryModInt<T> r)
	{
		if (WrapTruncating(v, out long l))
		{
			r = l;
			return true;
		}
		if (WrapTruncating(v, out ulong u))
		{
			r = u;
			return true;
		}
		r = default;
		return false;
	}
	static bool INumberBase<MontgomeryModInt<T>>.TryConvertToChecked<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapChecked(v._v, out r);
	static bool INumberBase<MontgomeryModInt<T>>.TryConvertToSaturating<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapSaturating(v._v, out r);
	static bool INumberBase<MontgomeryModInt<T>>.TryConvertToTruncating<TOther>(MontgomeryModInt<T> v, out TOther r) where TOther : default => WrapTruncating(v._v, out r);

	[MethodImpl(256)]
	static bool WrapChecked<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
		=> typeof(TFrom) == typeof(TTo)
		? (r = (TTo)(object)v) is { }
		: TTo.TryConvertFromChecked(v, out r) || TFrom.TryConvertToChecked(v, out r);
	[MethodImpl(256)]
	static bool WrapSaturating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
		=> typeof(TFrom) == typeof(TTo)
		? (r = (TTo)(object)v) is { }
		: TTo.TryConvertFromSaturating(v, out r) || TFrom.TryConvertToSaturating(v, out r);
	[MethodImpl(256)]
	static bool WrapTruncating<TFrom, TTo>(TFrom v, out TTo r) where TFrom : INumberBase<TFrom> where TTo : INumberBase<TTo>
		=> typeof(TFrom) == typeof(TTo)
		? (r = (TTo)(object)v) is { }
		: TTo.TryConvertFromTruncating(v, out r) || TFrom.TryConvertToTruncating(v, out r);
}