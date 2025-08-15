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
	// competitive-verifier: document_title Counter-Clockwise (Aizu Online Judge: CGL_1_C)
	internal class CounterClockwiseTest : VerifySolver
	{
		public override string Url => "https://onlinejudge.u-aizu.ac.jp/courses/library/4/CGL/1/CGL_1_C";
		public override double? Tle => 2.0;
		public override void Run()
		{
			var (x1, y1, x2, y2) = sr.ReadValue<int, int, int, int>();
			var p1 = new Complex(x1, y1);
			var p2 = new Complex(x2, y2);
			var l = new Line2D(p1, p2);
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var (x, y) = sr.ReadValue<int, int>();
				var p = new Complex(x, y);
				var ans = Geometry2D.ISP(p2, p1, p);
				var msg = "";
				if (ans == +1) msg = "CLOCKWISE";
				if (ans == -1) msg = "COUNTER_CLOCKWISE";
				if (ans == +2) msg = "ONLINE_BACK";
				if (ans == 0) msg = "ON_SEGMENT";
				if (ans == -2) msg = "ONLINE_FRONT";
				sw.WriteLine(msg);
			}
		}
	}
}
