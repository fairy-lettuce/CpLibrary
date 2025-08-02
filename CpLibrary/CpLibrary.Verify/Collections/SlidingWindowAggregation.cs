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
	// competitive-verifier: document_title SlidingWindowAggregation<T> (Library Checker: Queue Operate All Composite)
	internal class SlidingWindowAggregationTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/queue_operate_all_composite";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

			var swag = new SlidingWindowAggregation<(ModInt a, ModInt b)>((x, y) => (x.a * y.a, y.a * x.b + y.b));

			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var query = sr.ReadInt();
				if (query == 0)
				{
					var (a, b) = sr.ReadValue<int, int>();
					swag.Push((a, b));
				}
				if (query == 1)
				{
					swag.Pop();
				}
				if (query == 2)
				{
					var x = sr.ReadInt();
					if (swag.Count == 0)
					{
						Console.WriteLine(x);
						continue;
					}
					var ret = swag.Prod();
					Console.WriteLine(ret.a * x + ret.b);
				}
			}
		}
	}
}
