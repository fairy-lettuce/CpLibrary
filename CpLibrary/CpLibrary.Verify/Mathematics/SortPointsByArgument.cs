using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rational = CpLibrary.Mathematics.Rational<long>;

namespace CpLibrary.Verify.Mathematics;

// competitive-verifier: document_title Rational<T> [comparison] (Library Checker: Sort Points by Argument)
internal class SortPointsByArgumentRationalTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/sort_points_by_argument";
	public override double? Tle => 5.0;
	public override void Run()
	{
		int n = sr.ReadInt();
		var (x, y) = sr.ReadValueArray<long, long>(n);
		var p1 = new List<(Rational, (long, long))>();
		var p2 = new List<(Rational, (long, long))>();
		for (int i = 0; i < n; i++)
		{
			Rational rat;
			if (y[i] == 0)
			{
				rat = Rational.Zero;
			}
			else
			{
				if (x[i] == 0)
				{
					if (y[i] > 0) rat = new Rational(int.MaxValue);
					else rat = new Rational(int.MinValue);
				}
				else rat = new Rational(y[i], x[i]);
			}

			if (x[i] >= 0)
			{
				p1.Add(new(rat, (x[i], y[i])));
			}
			else
			{
				p2.Add(new(rat, (x[i], y[i])));
			}
		}

		p1.Sort();
		p2.Sort();

		var p2plus = p2.Where(x => x.Item1 > Rational.Zero);
		foreach (var e in p2plus)
		{
			sw.WriteLine($"{e.Item2.Item1} {e.Item2.Item2}");
		}
		foreach (var e in p1)
		{
			sw.WriteLine($"{e.Item2.Item1} {e.Item2.Item2}");
		}
		var p2minus = p2.Where(x => x.Item1 <= Rational.Zero);
		foreach (var e in p2minus)
		{
			sw.WriteLine($"{e.Item2.Item1} {e.Item2.Item2}");
		}
	}
}
