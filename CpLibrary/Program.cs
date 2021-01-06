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
using CpLibrary.Contest;

using static System.Math;

namespace CpLibrary
{
	public static class Program
	{
		static void Main(string[] args)
		{
			SolverBase solver;
			var sr = new Scanner(new StreamReader("../../../Workspace/A_input.txt"));
			solver = new SolverA(sr, new StreamWriter(Console.OpenStandardOutput()));
			solver.Run();
		}
	}

	public interface ISolver
	{
		public void Solve();
		public void Run();
	}

	public abstract class SolverBase : ISolver
	{
		Scanner sr;
		StreamWriter sw;
		bool isMultipleTestcases = false;

		public abstract void Solve();
		public abstract void Run();

		public bool YesNo(bool condition)
		{
			sw.WriteLine(condition ? "Yes" : "No");
			return condition;
		}
		public bool YESNO(bool condition)
		{
			sw.WriteLine(condition ? "YES" : "NO");
			return condition;
		}
		public bool yesno(bool condition)
		{
			sw.WriteLine(condition ? "yes" : "no");
			return condition;
		}
	}
}
