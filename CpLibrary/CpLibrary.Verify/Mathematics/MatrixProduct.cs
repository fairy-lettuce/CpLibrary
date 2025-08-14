using CpLibrary.Collections;
using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Mathematics;

// competitive-verifier: document_title Matrix<T> (Library Checker: Matrix Product)
internal class MatrixProductTest : CompetitiveVerifier.ProblemSolver
{
	public override string Url => "https://judge.yosupo.jp/problem/matrix_product";
	public override void Solve()
	{
		var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

		var (n, m, k) = sr.ReadValue<int, int, int>();
		var _a = StaticItems.CreateArray(n, i => sr.ReadIntArray(m).Select(x => new ModInt(x)).ToArray());
		var _b = StaticItems.CreateArray(m, i => sr.ReadIntArray(k).Select(x => new ModInt(x)).ToArray());
		var a = new Matrix<ModInt>(_a);
		var b = new Matrix<ModInt>(_b);
		var c = a * b;
		for (int i = 0; i < n; i++)
		{
			Console.WriteLine(Enumerable.Range(0, k).Select(j => c[i, j]).Join(" "));
		}
	}
}
