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
//using ModInt = AtCoder.StaticModInt<AtCoder.Mod1000000007>;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;
using static System.Math;
using static CpLibrary.StaticItems;
using AtCoder;

namespace CpLibrary.Contest
{
	public class SolverC : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverC());

		public override void Solve()
		{
			var (n, m) = sr.ReadValue<int, int>();
			var (a, b, c) = sr.ReadValueArray<int, int, int>(m);
			var twosat = new TwoSat(2 * n);
			for (int i = 0; i < m; i++)
			{
				if (c[i] == 0)
				{
					twosat.AddClause(a[i] - 1, true, n + b[i] - 1, true);
					twosat.AddClause(a[i] - 1, false, n + b[i] - 1, false);
				}
				else
				{
					twosat.AddClause(a[i] - 1, true, n + b[i] - 1, false);
					twosat.AddClause(a[i] - 1, false, n + b[i] - 1, true);
				}
			}
			var ok = twosat.Satisfiable();
			if (!ok)
			{
				sw.WriteLine(-1);
				return;
			}
			var ans = twosat.Answer();
			sw.WriteLine(Enumerable.Range(0, n).Select(p => (ans[p] ^ ans[n + p]) ? 0 : 1).Join());
		}

		public override void Init()
		{
			/*
			 * Write your init code here!
			 */
		}
	}
}
