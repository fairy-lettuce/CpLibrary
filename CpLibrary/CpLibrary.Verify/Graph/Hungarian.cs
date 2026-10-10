using AtCoder;
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

namespace CpLibrary.Verify.Graph;

// competitive-verifier: document_title Assignment problem (Hungarian algorithm) (Library Checker: assignment)
internal class HungarianTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/assignment";
	public override double? Tle => 5.0;
	public override void Run()
	{
		var n = sr.ReadInt();
		var a = CpLibrary.Util.CreateArray(n, i => sr.ReadLongArray(n));
		var (x, p) = CpLibrary.Graph.Hungarian.Solve(a);
		sw.WriteLine(x);
		sw.WriteLine(p.Join(" "));
	}
}
