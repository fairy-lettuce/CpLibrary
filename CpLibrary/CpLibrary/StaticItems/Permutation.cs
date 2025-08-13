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
		public static bool NextPermutation<T>(IList<T> arr) where T : IComparable<T>
		{
			var p = -1;
			var q = -1;
			for (int i = 0; i < arr.Count - 1; i++)
			{
				if (arr[i].CompareTo(arr[i + 1]) < 0) p = i;
			}
			if (p == -1) return false;
			for (int i = arr.Count - 1; i >= 0; i--)
			{
				if (arr[p].CompareTo(arr[i]) < 0)
				{
					q = i;
					break;
				}
			}
			(arr[p], arr[q]) = (arr[q], arr[p]);
			for (int i = 0; p + 1 + i < arr.Count - 1 - i; i++)
			{
				(arr[p + 1 + i], arr[arr.Count - 1 - i]) = (arr[arr.Count - 1 - i], arr[p + 1 + i]);
			}
			return true;
		}
	}
}
