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
using static CpLibrary.StaticItems;

namespace CpLibrary.Contest
{
	public class SolverI : SolverBase
	{
		Scanner sr;
		bool hasMultipleTestcases = false;

		bool IsLocal { get; set; }

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public void Init()
		{
			/*
			 * Write your init code here if you need!
			 */
		}

		public SolverI(Scanner sr, bool isLocal = false) { this.sr = sr; this.IsLocal = isLocal; }

		public override void Run()
		{
			Init();
			var _t = 1;
			if (hasMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramI
	{
		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverI(sr);
			var thread = new Thread(new ThreadStart(() => solver.Run()), 1 << 27);
			thread.Start();
			thread.Join();
			Console.Out.Flush();
		}

		public static void Expand() => SourceExpander.Expander.Expand();
	}
}
