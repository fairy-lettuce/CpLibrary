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
}
