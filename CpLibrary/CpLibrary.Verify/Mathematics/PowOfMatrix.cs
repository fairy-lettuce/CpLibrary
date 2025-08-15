using CpLibrary.Collections;
using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = CpLibrary.Mathematics.MontgomeryModInt<AtCoder.Mod998244353>;
using static CpLibrary.StaticItems;

namespace CpLibrary.Verify.Mathematics;

// competitive-verifier: document_title Matrix<T> (Library Checker: Pow of Matrix)
internal class PowOfMatrixTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/pow_of_matrix";
	public override void Solve()
	{
		var (n, k) = sr.ReadValue<int, long>();
		var a = new Matrix<ModInt>(sr.ReadMatrix<ModInt>(n, n));
		var b = a.Pow(k);
		sw.WriteMatrix(b);
	}
}
