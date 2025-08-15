using CpLibrary.Collections;
using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = CpLibrary.Mathematics.MontgomeryModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Mathematics;

// competitive-verifier: document_title Matrix<T> (Library Checker: Determinant of Matrix)
internal class DeterminantOfMatrixTest : CompetitiveVerifier.ProblemSolver
{
	public override string Url => "https://judge.yosupo.jp/problem/matrix_det";
	public override void Solve()
	{
		var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

		var n = sr.ReadInt();
		var _a = StaticItems.CreateArray(n, i => sr.ReadIntArray(n).Select(p => new ModInt(p)).ToArray());
		var a = new Matrix<ModInt>(_a);
		var det = a.Determinant();
		Console.WriteLine(det);
	}
}
