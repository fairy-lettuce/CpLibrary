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
	// competitive-verifier: document_title Cross Point (Aizu Online Judge: CGL_2_C)
	internal class CrossPointTest : VerifySolver
	{
		public override string Url => "https://onlinejudge.u-aizu.ac.jp/courses/library/4/CGL/2/CGL_2_C";
		public override double? Tle => 2.0;
		public override double? Error => 1e-5;
		public override void Run()
		{
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var p = new Complex[4];
				for (int j = 0; j < 4; j++)
				{
					var (x, y) = sr.ReadValue<int, int>();
					var pp = new Complex(x, y);
					p[j] = pp;
				}
				var line1 = new Line2D(p[0], p[1]);
				var line2 = new Line2D(p[2], p[3]);
				var ans = line1.Intersection(line2);
				sw.WriteLine($"{ans.Real} {ans.Imaginary}");
			}
		}
	}
}
