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

namespace CpLibrary
{
	public interface ISolver
	{
		public void Solve();
		public void Run();
	}

	public abstract class SolverBase : ISolver
	{
		public abstract void Solve();
		public abstract void Run();

		public bool YesNo(bool condition)
		{
			Console.WriteLine(condition ? "Yes" : "No");
			return condition;
		}
		public bool YESNO(bool condition)
		{
			Console.WriteLine(condition ? "YES" : "NO");
			return condition;
		}
		public bool yesno(bool condition)
		{
			Console.WriteLine(condition ? "yes" : "no");
			return condition;
		}
		public static T SignOutput<T>(int x, T pos, T zero, T neg) => x == 0 ? zero : (x > 0 ? pos : neg);

		public static T[] CreateArray<T>(int n, Func<int, T> func) => Enumerable.Range(0, n).Select(p => func(p)).ToArray();
		public static T[][] CreateArray<T>(int h, int w, Func<int, int, T> func) => Enumerable.Range(0, h).Select(i => Enumerable.Range(0, w).Select(j => func(i, j)).ToArray()).ToArray();
	}

}
