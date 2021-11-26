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
using ModInt = AtCoder.StaticModInt<AtCoder.Mod1000000007>;
//using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using static System.Math;
using static CpLibrary.Util;

namespace CpLibrary.Contest
{
	public class SolverC : SolverBase
	{
		Scanner sr;
		bool HasMultipleTestcases { get; }

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public SolverC(Scanner sr) => this.sr = sr;

		public override void Run()
		{
			var _t = 1;
			if (HasMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramC
	{
		private static bool StartsOnThread = true;

		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverC(sr);
			if (StartsOnThread)
			{
				var thread = new Thread(new ThreadStart(() => solver.Run()), 1 << 27);
				thread.Start();
				thread.Join();
			}
			else solver.Run();
			Console.Out.Flush();
		}

		public static void Expand() => SourceExpander.Expander.Expand();
	}
}
