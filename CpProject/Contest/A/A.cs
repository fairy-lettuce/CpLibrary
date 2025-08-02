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
using CpLibrary.Collections;

namespace CpLibrary.Contest
{
	public class SolverA : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverA());

		public readonly struct IOp:IProdSetOperator<(long index, (ModInt a, ModInt b) f)>
		{
			public (long index, (ModInt a, ModInt b) f) Identity => (-1, (1, 0));
			public (long index, (ModInt a, ModInt b) f) Operate((long index, (ModInt a, ModInt b) f) f, (long index, (ModInt a, ModInt b) f) g)
			{
				return (f.index, (g.f.a * f.f.a, g.f.a * f.f.b + g.f.b));
			}
		}

		public class TupleComparer<T1, T2> : IComparer<(T1, T2)>
			where T1: IComparable<T1>
		{
			public int Compare((T1, T2) x, (T1, T2) y)
			{
				return x.Item1.CompareTo(y.Item1);
			}
		}

		public override void Solve()
		{
			var ps = new ProdSet<(long index, (ModInt a, ModInt b) f), IOp>(new TupleComparer<long, (ModInt a, ModInt b)>());
			ps.Add((0, (1, 0)));
			var (n, q) = sr.ReadValue<long, int>();
			for (int i = 0; i < q; i++)
			{
				var query = sr.ReadInt();
				if (query == 0)
				{
					var (p, c, d) = sr.ReadValue<long, int, int>();
					var idx = ps.LowerBound((p, (0, 0)));
					if (idx >= ps.Count || ps[idx].index != p)
					{
						ps.Add((p, (c, d)));
					}
					else
					{
						ps.RemoveAt(idx);
						ps.Add((p, (c, d)));
					}
				}
				else
				{
					var (l, r, x) = sr.ReadValue<int, int, long>();
					var l2 = ps.LowerBound((l, (0, 0)));
					var r2 = ps.LowerBound((r, (0, 0)));
					var prod = ps.Prod(l2, r2);
					var ans = prod.f.a * x + prod.f.b;
					sw.WriteLine(ans);
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
