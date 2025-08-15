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

// competitive-verifier: document_title Matrix<T> (Library Checker: Matrix Product)
internal class MatrixProductTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/matrix_product";
	public override void Solve()
	{
		var (n, m, k) = sr.ReadValue<int, int, int>();
		var a = new Matrix<ModInt>(sr.ReadMatrix<ModInt>(n, m));
		var b = new Matrix<ModInt>(sr.ReadMatrix<ModInt>(m, k));
		var c = a * b;
		sw.WriteMatrix(c);
	}
}
