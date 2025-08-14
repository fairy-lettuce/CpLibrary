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

// competitive-verifier: document_title Matrix<T> (Library Checker: PowOfMatrix)
internal class PowOfMatrixTest : CompetitiveVerifier.ProblemSolver
{
	public override string Url => "https://judge.yosupo.jp/problem/pow_of_matrix";
	public override void Solve()
	{
		var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

		var (n, k) = sr.ReadValue<int, long>();
		var _a = StaticItems.CreateArray(n, i => sr.ReadIntArray(n).Select(x => new ModInt(x)).ToArray());
		var a = new Matrix<ModInt>(_a);
		var b = a.Pow(k);
		for (int i = 0; i < n; i++)
		{
			Console.WriteLine(Enumerable.Range(0, n).Select(j => b[i, j]).Join(" "));
		}
	}
}
