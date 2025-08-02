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

namespace CpLibrary.Contest
{
	public class SolverB : SolverBase
	{
		static void Main() => OnlineJudge.Run(new SolverB());

		public override void Solve()
		{
			var t = sr.ReadInt();
			while (t-- > 0)
			{
				var (n, k) = sr.ReadValue<int, int>();
				if (n % 2 == 1)
				{
					var gcd1 = GCD(n, k);
					var gcd2 = GCD(n, k + n / 2);
					if (gcd1 == 1 || gcd2 == 1)
					{
						sw.WriteLine("Yes");
					}
					else
					{
						sw.WriteLine("No");
					}
				}
				else
				{
					if (n / 2 == k)
					{
						sw.WriteLine("No");

					}
					else
					{
						var gcd1 = GCD(n, k);
						var gcd2 = GCD(n, k + n / 2);
						if (gcd1 <= 2 || gcd2 <= 2)
						{
							sw.WriteLine("Yes");
						}
						else
						{
							sw.WriteLine("No");
						}
					}
				}
			}
		}

		public int GCD(int a, int b)
		{
			if (a < b) return GCD(b, a);
			if (a % b == 0) return b;
			return GCD(b, a % b);
		}

		public override void Init()
		{
			/*
			 * Write your init code here!
			 */
		}
	}
}
