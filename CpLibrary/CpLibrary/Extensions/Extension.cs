using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary
{
	public static class Extension
	{
		public static string Join<T>(this IEnumerable<T> x, string separator = "") => string.Join(separator, x);
		public static string Join<T>(this IEnumerable<T> x, char separator) => string.Join(separator, x);

		public static int UpperBound<T>(this IList<T> list, T value) => list.BinarySearch(value, true, 0, list.Count, Comparer<T>.Default);
		public static int LowerBound<T>(this IList<T> list, T value) => list.BinarySearch(value, false, 0, list.Count, Comparer<T>.Default);
		public static int BinarySearch<T>(this IList<T> list, T value, bool isUpperBound, int index, int length, Comparer<T> comparer)
		{
			var ng = index - 1;
			var ok = index + length;
			while (ok - ng > 1)
			{
				var mid = ng + (ok - ng) / 2;
				var res = comparer.Compare(list[mid], value);
				if (res < 0 || (isUpperBound && res == 0)) ng = mid;
				else ok = mid;
			}
			return ok;
		}

		public static bool Chmax<T>(ref this T a, T b) where T : struct, IComparable<T>
		{
			if (a.CompareTo(b) >= 0) return false;
			a = b;
			return true;
		}
		public static bool Chmin<T>(ref this T a, T b) where T : struct, IComparable<T>
		{
			if (a.CompareTo(b) <= 0) return false;
			a = b;
			return true;
		}
	}
}
