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

namespace CpLibrary.Verify.Collections
{
	// competitive-verifier: document_title Projection (Aizu Online Judge: CGL_1_A)
	internal class ProjectionTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://onlinejudge.u-aizu.ac.jp/courses/library/4/CGL/1/CGL_1_A";
		public override double? Error => 1e-5;
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var (x1, y1, x2, y2) = sr.ReadValue<int, int, int, int>();
			var p1 = new Complex(x1, y1);
			var p2 = new Complex(x2, y2);
			var l = new Line2D(p1, p2);
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var (x, y) = sr.ReadValue<int, int>();
				var p = new Complex(x, y);
				var ans = l.Projection(p);
				Console.WriteLine($"{ans.Real} {ans.Imaginary}");
			}
		}
	}
}
