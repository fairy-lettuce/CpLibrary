using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

namespace CpLibrary.Mathematics;

[DebuggerDisplay("{ToString(), nq}")]
public class Matrix<T> : IEquatable<Matrix<T>>, IEnumerable<T>, IEnumerable<IEnumerable<T>> where T : INumberBase<T>
{
	int row;
	int column;
	public T[] value;

	public Matrix(int r, int c) : this(r, c, new T[r * c]) { }

	public Matrix(int r, int c, IEnumerable<T> a)
	{
		value = a.ToArray();
		row = r;
		column = c;
	}

	public Matrix(int r, int c, T[] a)
	{
		value = a;
		row = r;
		column = c;
	}

	public Matrix(T[][] a)
	{
		if (a is null || a.Length == 0) throw new ArgumentException("Input array cannot be null or empty.", nameof(a));
		row = a.Length;
		column = a[0].Length;
		if (!a.Select(b => b.Length).All(x => x == column)) throw new ArgumentException("All rows must have the same length.", nameof(a));
		value = a.SelectMany(x => x).ToArray();
	}

	public Matrix(T[,] a)
	{
		if (a is null || a.Length == 0) throw new ArgumentException("Input array cannot be null or empty.", nameof(a));
		row = a.GetLength(0);
		column = a.GetLength(1);
		value = a.Cast<T>().ToArray();
	}

	public Span<T> this[int r]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => value.AsSpan(r * column, column);
	}

	public T this[int r, int c]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => value[r * column + c];
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => this.value[r * column + c] = value;
	}

	public T[][] Rows => ToRowArray();

	public static Matrix<T> operator +(Matrix<T> lhs, Matrix<T> rhs)
	{
		if (lhs.row != rhs.row) throw new ArgumentException();
		if (lhs.column != rhs.column) throw new ArgumentException();
		var ret = new T[lhs.row * lhs.column];
		for (int i = 0; i < lhs.row * lhs.column; i++)
		{
			ret[i] = lhs.value[i] + rhs.value[i];
		}
		return new Matrix<T>(lhs.row, lhs.column, ret);
	}

	public static Matrix<T> operator -(Matrix<T> lhs, Matrix<T> rhs)
	{
		if (lhs.row != rhs.row) throw new ArgumentException();
		if (lhs.column != rhs.column) throw new ArgumentException();
		var ret = new T[lhs.row * lhs.column];
		for (int i = 0; i < lhs.row * lhs.column; i++)
		{
			ret[i] = lhs.value[i] - rhs.value[i];
		}
		return new Matrix<T>(lhs.row, lhs.column, ret);
	}

	public static Matrix<T> operator *(T k, Matrix<T> x)
	{
		var ret = new T[x.row * x.column];
		for (int i = 0; i < x.row * x.column; i++)
		{
			ret[i] = k * x.value[i];
		}
		return new Matrix<T>(x.row, x.column, ret);
	}

	public static Matrix<T> operator *(Matrix<T> x, T k) => k * x;

	public static Matrix<T> operator *(Matrix<T> l, Matrix<T> r)
	{
		if (l.column != r.row) throw new ArgumentException();
		var rt = r.Transpose();
		var lv = l.value;
		var rtv = rt.value;
		var ret = new T[l.row * r.column];
		// ijk loop is somehow faster (why???)
		for (int i = 0; i < l.row; i++)
		{
			for (int j = 0; j < r.column; j++)
			{
				var sum = T.Zero;
				for (int k = 0; k < l.column; k++)
				{
					sum += lv[i * l.column + k] * rtv[j * rt.column + k];
				}
				ret[i * r.column + j] = sum;
			}
		}
		return new Matrix<T>(l.row, r.column, ret);
	}

	public static bool operator ==(Matrix<T> lhs, Matrix<T> rhs) => lhs.Equals(rhs);
	public static bool operator !=(Matrix<T> lhs, Matrix<T> rhs) => !(lhs == rhs);

	public Matrix<T> Pow(long n) => Pow(this, n);

	public static Matrix<T> Pow(Matrix<T> x, long n)
	{
		if (x.column != x.row) throw new ArgumentException();
		var ret = new Matrix<T>(x.column, x.column);
		for (int i = 0; i < x.column; i++)
		{
			ret[i, i] = T.One;
		}

		for (long i = 1; i <= n; i *= 2, x *= x)
		{
			if (n / i % 2 == 1) ret *= x;
		}

		return ret;
	}

	public Matrix<T> Transpose()
	{
		var res = new Matrix<T>(this.column, this.row);
		for (int i = 0; i < this.row; i++)
		{
			for (int j = 0; j < this.column; j++)
			{
				res[j, i] = this[i, j];
			}
		}
		return res;
	}

	public T Determinant()
	{
		if (column != row) throw new ArgumentException();
		var a = new T[row * column];
		value.AsSpan().CopyTo(a);
		var swap = false;
		var p = Enumerable.Range(0, row).ToArray();
		for (int i = 0; i < row; i++)
		{
			var ok = false;
			for (int j = i; j < row; j++)
			{
				if (a[p[j] * row + i] != T.Zero)
				{
					ok = true;
					if (i != j)
					{
						(p[i], p[j]) = (p[j], p[i]);
						swap = !swap;
					}
					break;
				}
			}
			if (!ok) return T.AdditiveIdentity;
			var t = a[p[i] * row + i];
			for (int j = i + 1; j < row; j++)
			{
				var c = a[p[j] * row + i] / t;
				for (int k = 0; k < row; k++)
				{
					a[p[j] * row + k] -= c * a[p[i] * row + k];
				}
			}
		}
		var det = T.MultiplicativeIdentity;
		if (swap) det = -det;
		for (int i = 0; i < row; i++)
		{
			det *= a[p[i] * row + i];
		}
		return det;
	}

	public T[][] ToRowArray()
	{
		var ret = new T[row][];
		for (int i = 0; i < row; i++)
		{
			ret[i] = this[i].ToArray();
		}
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override bool Equals(object? obj) => obj is Matrix<T> x && Equals(x);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(Matrix<T> other)
	{
		if (row != other.row) return false;
		if (column != other.column) return false;
		return value.AsSpan().SequenceEqual(other.value);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override int GetHashCode() => HashCode.Combine(row, value);

	public override string ToString()
	{
		var sb = new StringBuilder();
		for (int i = 0; i < row; i++)
		{
			sb.Append("{ ");
			for (int j = 0; j < column; j++)
			{
				sb.Append(this[i, j]);
				if (j < column - 1)
				{
					sb.Append(", ");
				}
			}
			sb.Append(" }");
			if (i < row - 1)
			{
				sb.AppendLine();
			}
		}
		return sb.ToString();
	}

	public IEnumerator<T> GetEnumerator() => ((IEnumerable<T>)value).GetEnumerator();

	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

	IEnumerator<IEnumerable<T>> IEnumerable<IEnumerable<T>>.GetEnumerator()
	{
		for (int i = 0; i < row; i++)
		{
			yield return GetRow(i);
		}
	}

	IEnumerable<T> GetRow(int idx)
	{
		if (idx < 0 || row <= idx) throw new ArgumentException();
		for (int i = 0; i < column; i++)
		{
			yield return value[idx * column + i];
		}
	}
}
