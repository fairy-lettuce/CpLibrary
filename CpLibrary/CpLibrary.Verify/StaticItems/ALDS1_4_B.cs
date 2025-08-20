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
	// competitive-verifier: document_title Binary Search (Aizu Online Judge: ALDS1_4_B)
	internal class BinarySearchTest : VerifySolver
	{
		public override string Url => "https://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=ALDS1_4_B";
		public override double? Tle => 2.0;
		public override void Run()
		{
			var n = sr.ReadInt();
			var s = sr.ReadIntArray(n);
			var q = sr.ReadInt();
			var t = sr.ReadIntArray(q);
			var ans = 0;
			for (int i = 0; i < q; i++)
			{
				var idx = s.LowerBound(t[i]);
				if (idx < n && s[idx] == t[i]) ans++;
			}
			sw.WriteLine(ans);
		}
	}
}
