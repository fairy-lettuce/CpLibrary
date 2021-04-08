using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace CpLibrary.Collections
{
	public class SparseTable<T>
	{
		int[] log;
		public int Count { private set; get; }
		T[][] table;
		Func<T, T, T> func; // func(T x, T y) = Min(x, y) or Max(x, y)
		T identity;

		public T this[int index] => table[0][index];

		public SparseTable(T[] a, Func<T, T, T> func, T identity)
		{
			this.func = func;
			this.identity = identity;
			Count = a.Length;
			log = new int[Count + 1];
			log[0] = -1;
			var t = 0;
			for (int i = 1; i <= Count; i++)
			{
				if ((1 << t) == i) t++;
				log[i] = t - 1;
			}
			table = new T[log[Count] + 1][];
			table[0] = a;
			for (int i = 1; i <= log[Count]; i++)
			{
				table[i] = new T[Count - (1 << i) + 1];
				for (int j = 0; j <= Count - (1 << i); j++)
				{
					table[i][j] = func(table[i - 1][j], table[i - 1][j + (1 << (i - 1))]);
				}
			}
		}

		public T Prod(int l, int r) => l == r ? identity : func(table[log[r - l]][l], table[log[r - l]][r - (1 << log[r - l])]);

		public T AllProd => Prod(0, Count);
	}
}
