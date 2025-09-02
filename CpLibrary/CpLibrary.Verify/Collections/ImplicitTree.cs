using AtCoder;
using CpLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Collections;

// competitive-verifier: document_title Implicit Treap (Library Checker: Dynamic Sequence Range Affine Range Sum)
internal class ImplicitTreapTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/dynamic_sequence_range_affine_range_sum";
	public override double? Tle => 10.0;
	public override void Run()
	{
		var (n, q) = sr.ReadValue<int, int>();
		var treap = new ImplicitTreap<(ModInt v, int len), (ModInt a, ModInt b), Op>();
		var a = sr.ReadIntArray(n);
		for (int i = 0; i < n; i++)
		{
			treap.Add(i, (a[i], 1));
		}
		for (int i = 0; i < q; i++)
		{
			var query = sr.ReadInt();
			if (query == 0)
			{
				var (idx, x) = sr.ReadValue<int, int>();
				treap.Add(idx, (x, 1));
			}
			else if (query == 1)
			{
				var idx = sr.ReadInt();
				treap.Remove(idx);
			}
			else if (query == 2)
			{
				var (l, r) = sr.ReadValue<int, int>();
				treap.Reverse(l, r);
			}
			else if (query == 3)
			{
				var (l, r, b, c) = sr.ReadValue<int, int, int, int>();
				treap.Apply(l, r, (b, c));
			}
			else
			{
				var (l, r) = sr.ReadValue<int, int>();
				sw.WriteLine(treap.Prod(l, r).v);
			}
		}
	}

	readonly struct Op : ILazySegtreeOperator<(ModInt v, int len), (ModInt a, ModInt b)>
	{
		public (ModInt a, ModInt b) FIdentity => (1, 0);

		public (ModInt v, int len) Identity => (0, 0);

		public (ModInt a, ModInt b) Composition((ModInt a, ModInt b) nf, (ModInt a, ModInt b) cf)
		{
			return (nf.a * cf.a, nf.a * cf.b + nf.b);
		}

		public (ModInt v, int len) Mapping((ModInt a, ModInt b) f, (ModInt v, int len) x)
		{
			return (f.a * x.v + f.b * x.len, x.len);
		}

		public (ModInt v, int len) Operate((ModInt v, int len) x, (ModInt v, int len) y)
		{
			return (x.v + y.v, x.len + y.len);
		}
	}
}
