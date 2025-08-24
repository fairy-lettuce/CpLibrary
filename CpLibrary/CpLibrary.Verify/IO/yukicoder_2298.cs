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

namespace CpLibrary.Verify.IO;

// competitive-verifier: document_title Scaner.ReadString() (yukicoder: No. 2298 yukicounter)
internal class ReadStringTest : VerifySolver
{
	public override string Url => "https://yukicoder.me/problems/no/2298";
	public override double? Tle => 2.0;
	public override void Run()
	{
		var s = sr.ReadString();
		var ans = 0;
		var ren = 0;
		var tmp = 0;
		var yukicoder = "yukicoder";
		for (int i = 0; i < s.Length; i++)
		{
			if (s[i] == yukicoder[tmp])
			{
				if (++tmp == yukicoder.Length) { tmp = 0; ++ren; ans.Chmax(ren); }
			}
			else
			{
				tmp = 0;
				ren = 0;
			}
		}
		sw.WriteLine(ans);
	}
}
