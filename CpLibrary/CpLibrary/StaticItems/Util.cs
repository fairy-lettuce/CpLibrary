using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CpLibrary
{
	public static partial class StaticItems
	{
		public static int[] dx4 => new int[] { 1, 0, -1, 0 };
		public static int[] dy4 => new int[] { 0, 1, 0, -1 };
		public static int[] dx8 => new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
		public static int[] dy8 => new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };

		public enum YesNoOption
		{
			Normal,
			Upper,
			Lower
		}

		public static string YesNo(bool condition, YesNoOption option = YesNoOption.Normal)
		{
			if (option == YesNoOption.Normal) return YesNoNormal(condition);
			else if (option == YesNoOption.Upper) return YesNoUpper(condition);
			else if (option == YesNoOption.Lower) return YesNoLower(condition);
			else throw new ArgumentException();
		}
		public static string YesNoNormal(bool condition) => condition ? "Yes" : "No";
		public static string YesNoUpper(bool condition) => condition ? "YES" : "NO";
		public static string YesNoLower(bool condition) => condition ? "yes" : "no";

		public static T SignOutput<T>(int x, T pos, T zero, T neg) => x == 0 ? zero : (x > 0 ? pos : neg);
		public static T SignOutput<T>(long x, T pos, T zero, T neg) => x == 0 ? zero : (x > 0 ? pos : neg);
		public static T SignOutput<T>(double x, T pos, T zero, T neg) => x == 0 ? zero : (x > 0 ? pos : neg);

		public static T[] CreateArray<T>(int n, Func<int, T> func) => Enumerable.Range(0, n).Select(p => func(p)).ToArray();
		public static T[][] CreateArray<T>(int x, int y, Func<int, int, T> func) => Enumerable.Range(0, x).Select(i => Enumerable.Range(0, y).Select(j => func(i, j)).ToArray()).ToArray();
		public static T[][][] CreateArray<T>(int x, int y, int z, Func<int, int, int, T> func) => Enumerable.Range(0, x).Select(i => Enumerable.Range(0, y).Select(j => Enumerable.Range(0, z).Select(k => func(i, j, k)).ToArray()).ToArray()).ToArray();
	}
}
