using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary
{
	public static class Util
	{
		public static T SignOutput<T>(int x, T pos, T zero, T neg) => x == 0 ? zero : (x > 0 ? pos : neg);

		public static T[] CreateArray<T>(int n, Func<int, T> func) => Enumerable.Range(0, n).Select(p => func(p)).ToArray();
		public static T[][] CreateArray<T>(int h, int w, Func<int, int, T> func) => Enumerable.Range(0, h).Select(i => Enumerable.Range(0, w).Select(j => func(i, j)).ToArray()).ToArray();
	}
}
