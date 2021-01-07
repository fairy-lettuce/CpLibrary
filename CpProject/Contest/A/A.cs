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
		bool willExpandSource = true;
		bool isMultipleTestcases = false;

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public SolverA(Scanner sr) => this.sr = sr;

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
			var solver = new SolverA(sr);
			var thread = new Thread(new ThreadStart(() => solver.Run()), 1 << 27);
			thread.Start();
			thread.Join();
			Console.Out.Flush();
		}
	}
}
