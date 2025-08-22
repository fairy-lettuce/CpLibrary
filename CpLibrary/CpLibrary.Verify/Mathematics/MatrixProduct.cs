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
	public override double? Tle => 10;
	public override void Run()
	{
		var (n, m, k) = sr.ReadValue<int, int, int>();
		var a = new Matrix<ModInt>(sr.ReadMatrix<uint>(n, m).Select(i => i.Select(j => new ModInt(j)).ToArray()).ToArray());
		var b = new Matrix<ModInt>(sr.ReadMatrix<uint>(m, k).Select(i => i.Select(j => new ModInt(j)).ToArray()).ToArray());
		var c = a * b;
		sw.WriteMatrix(c);
	}
}
