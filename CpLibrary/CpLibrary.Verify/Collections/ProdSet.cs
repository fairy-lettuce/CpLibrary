using CpLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Collections
{
	// competitive-verifier: document_title Set<T> (Library Checker: Double-Ended Priority Queue)
	internal class ProdSet : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/point_set_range_composite_large_array";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

			var (n, q) = sr.ReadValue<long, int>();

			var ps = new ProdSet<(long index, (ModInt a, ModInt b) f), IOp>(new TupleComparer<long, (ModInt a, ModInt b)>());
			ps.Add((0, (1, 0)));

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
					Console.WriteLine(ans);
				}
			}
		}

		public readonly struct IOp : IProdSetOperator<(long index, (ModInt a, ModInt b) f)>
		{
			public (long index, (ModInt a, ModInt b) f) Identity => (-1, (1, 0));
			public (long index, (ModInt a, ModInt b) f) Operate((long index, (ModInt a, ModInt b) f) f, (long index, (ModInt a, ModInt b) f) g)
			{
				return (f.index, (g.f.a * f.f.a, g.f.a * f.f.b + g.f.b));
			}
		}

		public class TupleComparer<T1, T2> : IComparer<(T1, T2)>
			where T1 : IComparable<T1>
		{
			public int Compare((T1, T2) x, (T1, T2) y)
			{
				return x.Item1.CompareTo(y.Item1);
			}
		}
	}
}
