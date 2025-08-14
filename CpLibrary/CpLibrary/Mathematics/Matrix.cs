using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Mathematics;

public class Matrix<T> where T : INumberBase<T>
{
	int row;
	int column;
	public T[] value;

	public Matrix(int r, int c) : this(r, c, new T[r * c]) { }

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

	public T this[int r, int c]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => this.value[r * column + c];
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => this.value[r * column + c] = value;
	}

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
}
