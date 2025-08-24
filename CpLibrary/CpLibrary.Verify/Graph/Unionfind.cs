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

// competitive-verifier: document_title UnionFind (Library Checker: Unionfind)
internal class UnionFindTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/unionfind";
	public override double? Tle => 2.0;
	public override void Run()
	{
		var (n, q) = sr.ReadValue<int, int>();
		var uf = new CpLibrary.Graph.UnionFind(n);
		for (int i = 0; i < q; i++)
		{
			var (t, u, v) = sr.ReadValue<int, int, int>();
			if (t == 0)
			{
				uf.Unite(u, v);
			}
			if (t == 1)
			{
				sw.WriteLine(uf.IsSame(u, v) ? 1 : 0);
			}
		}
	}
}
