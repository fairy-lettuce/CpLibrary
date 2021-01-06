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

using static System.Math;

namespace CpLibrary.Contest
{
	public class SolverA : SolverBase
	{
		Scanner sr;
		StreamWriter sw;
		bool isMultipleTestcases = false;
		bool willExpandSource = true;

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public SolverA(Scanner cin, StreamWriter cout)
		{
			this.sr = cin;
			this.sw = cout;
		}

		public override void Run()
		{
			if (willExpandSource) SourceExpander.Expander.Expand();
			var _t = 1;
			if (isMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramA
	{
		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverA(sr, sw);
			solver.Run();
			Console.Out.Flush();
		}
	}
}
