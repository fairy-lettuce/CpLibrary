using CpLibrary.Collections;
using CpLibrary.Geometry;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Geometry
{
	// competitive-verifier: document_title Cross Points of Circles (Aizu Online Judge: CGL_7_E)
	internal class CrossPointsOfCirclesTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://onlinejudge.u-aizu.ac.jp/courses/library/4/CGL/1/CGL_7_E";
		public override double? Error => 1e-5;
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

			var (c1x, c1y, c1r, c2x, c2y, c2r) = sr.ReadValue<int, int, int, int, int, int>();
			var c1 = new Circle2D(new Complex(c1x, c1y), c1r);
			var c2 = new Circle2D(new Complex(c2x, c2y), c2r);
			var ans = c1.Intersection(c2);
			if (ans.Length == 1) ans = ans.Append(ans[0]).ToArray();
			Console.WriteLine($"{ans[0].Real} {ans[0].Imaginary} {ans[1].Real} {ans[1].Imaginary}");
		}
	}
}
