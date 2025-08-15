using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary;

public static partial class StaticItems
{
	public static void WriteMatrix<T>(this StreamWriter sw, IEnumerable<IEnumerable<T>> arr) => sw.WriteLine(arr.Select(p => p.Join(" ")).Join("\n"));

	public static void WriteMatrix<T>(this StreamWriter sw, Matrix<T> arr) where T : INumberBase<T> => sw.WriteMatrix((IEnumerable<IEnumerable<T>>)arr);
}
