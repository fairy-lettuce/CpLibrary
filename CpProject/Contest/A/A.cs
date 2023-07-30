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
using CpLibrary.Judge;
using CpLibrary.Judge.Checker;
using CpLibrary.Judge.Downloader;

namespace CpLibrary.Contest
{
	public class SolverA : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverA());

		public override void Solve()
		{
			var dl = Downloader.DownloadTestcases("https://atcoder.jp/contests/abc312/tasks/abc312_a");
			dl.SaveTo("./abc312/a");

			var checker = new NormalChecker(new SolverA().Run);
			var path = "./abc312/a";
			var res = BatchTester.BatchTest(checker, path);

			sw.WriteLine(res.Status);
			foreach (var (k, v) in res.StatusCount)
			{
				sw.WriteLine($"{k}: {v}");
			}
		}

		public static void Actual(StreamReader reader, StreamWriter writer)
		{
			var sr = new Scanner(reader);
			var sw = writer;

			var s = sr.ReadString();
			var ans = new string[]
			{
				 "ACE", "BDF", "CEG", "DFA", "EGB", "FAC", "GBD"
			};
			if (ans.Contains(s))
			{
				sw.WriteLine("Yes");
			}
			else
			{
				sw.WriteLine("No");
			}
		}

		public override void Init()
		{
			/*
			 * Write your init code here!
			 */
		}
	}
}
