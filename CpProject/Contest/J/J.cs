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
using static CpLibrary.Util;

namespace CpLibrary.Contest
{
	public class SolverJ : SolverBase
	{
		Scanner sr;
		bool HasMultipleTestcases { get; }

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public SolverJ(Scanner sr) => this.sr = sr;

		public override void Run()
		{
			var _t = 1;
			if (HasMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramJ
	{
		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverJ(sr);
			var thread = new Thread(new ThreadStart(() => solver.Run()), 1 << 27);
			thread.Start();
			thread.Join();
			Console.Out.Flush();
		}

		public static void Expand() => SourceExpander.Expander.Expand();
	}
}
