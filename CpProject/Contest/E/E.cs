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
using CpLibrary.Collections;

namespace CpLibrary.Contest
{
	public class SolverE : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverE());

		public override void Solve()
		{
			var (n, q) = sr.ReadValue<int, int>();
			var a = sr.ReadIntArray(n);
			var s = new Set<int>(a);
			for (int i = 0; i < q; i++)
			{
				var (query, x) = sr.ReadValue<int, int>();
				if (query == 0)
				{
					if (s.Contains(x)) { }
					else s.Add(x);
				}
				if (query == 1)
				{
					if (s.Contains(x)) s.Remove(x);
				}
				if (query == 2)
				{
					if (s.Count < x) sw.WriteLine(-1);
					else sw.WriteLine(s[x - 1]);
				}
				if (query == 3)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(0);
						continue;
					}
					var idx = s.UpperBound(x);
					sw.WriteLine(idx);
				}
				if (query == 4)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(-1);
						continue;
					}
					var idx = s.UpperBound(x);
					if (idx <= 0) sw.WriteLine(-1);
					else
					{
						sw.WriteLine(s[idx - 1]);
					}
				}
				if (query == 5)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(-1);
						continue;
					}
					var idx = s.LowerBound(x);
					if (idx >= s.Count) sw.WriteLine(-1);
					else
					{
						sw.WriteLine(s[idx]);
					}
				}
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
