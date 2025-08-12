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
	// competitive-verifier: document_title Cross Points of a Circe and a Line (Aizu Online Judge: CGL_7_D)
	internal class CrossPointsOfCircleAndLineTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://onlinejudge.u-aizu.ac.jp/courses/library/4/CGL/7/CGL_7_D";
		public override double? Error => 1e-5;
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

			var (cx, cy, cr) = sr.ReadValue<int, int, int>();
			var c = new Circle2D(new Complex(cx, cy), cr);
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var (x1, y1, x2, y2) = sr.ReadValue<int, int, int, int>();
				var p1 = new Complex(x1, y1);
				var p2 = new Complex(x2, y2);
				var line = new Line2D(p1, p2);
				var ans = c.Intersection(line);
				if (ans.Length == 1) ans = ans.Append(ans[0]).ToArray();
				Console.WriteLine($"{ans[0].Real} {ans[0].Imaginary} {ans[1].Real} {ans[1].Imaginary}");
			}
		}
	}
}
