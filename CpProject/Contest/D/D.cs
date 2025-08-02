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
	public class SolverD : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverD());

		public override void Solve()
		{
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
						sw.WriteLine(x);
						continue;
					}
					var ret = swag.Prod();
					sw.WriteLine(ret.a * x + ret.b);
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
