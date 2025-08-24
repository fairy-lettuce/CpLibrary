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

namespace CpLibrary.Verify.IO;

// competitive-verifier: document_title Scaner.ReadStringArray() / Union Find (yukicoder: No. 2946 Puyo)
internal class ProjectionTest : VerifySolver
{
	public override string Url => "https://yukicoder.me/problems/no/2946";
	public override double? Tle => 2.0;
	public override void Run()
	{
		var (h, w) = sr.ReadValue<int, int>();
		var g = sr.ReadStringArray(h);
		var uf = new CpLibrary.Graph.UnionFind(h * w);
		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				if (i > 0 && g[i][j] == g[i - 1][j]) uf.Unite(i * w + j, (i - 1) * w + j);
				if (j > 0 && g[i][j] == g[i][j - 1]) uf.Unite(i * w + j, i * w + j - 1);
			}
		}
		var ans = StaticItems.CreateArray(h, w, (i, j) => g[i][j]);
		for (int i = 0; i < h; i++)
		{
			for (int j = 0; j < w; j++)
			{
				if (uf.Size(i * w + j) >= 4) ans[i][j] = '.';
			}
		}
		for (int i = 0; i < h; i++)
		{
			sw.WriteLine(ans[i].Join());
		}
	}
}
