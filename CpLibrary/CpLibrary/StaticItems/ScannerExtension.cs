using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary;

public static partial class StaticItems
{
	public static T[][] ReadMatrix<T>(this Scanner sr, int n, int m)
		where T : ISpanParsable<T>
		=> Enumerable.Range(0, n).Select(p => sr.ReadValueArray<T>(m)).ToArray();
	public static T[][] ReadJaggedArray<T>(this Scanner sr, int n, int[] m)
		where T : ISpanParsable<T>
		=> Enumerable.Range(0, n).Select(p => sr.ReadValueArray<T>(m[p])).ToArray();
}
