using CpLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModInt = AtCoder.StaticModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Verify.Collections
{
	// competitive-verifier: document_title ProdSet<T> (Library Checker: Deque)
	internal class DequeTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/deque";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var q = sr.ReadInt();
			var deque = new CpLibrary.Collections.Deque<int>();
			for (int i = 0; i < q; i++)
			{
				var query = sr.ReadInt();
				if (query == 0)
				{
					var x = sr.ReadInt();
					deque.PushFront(x);
				}
				if (query == 1)
				{
					var x = sr.ReadInt();
					deque.PushBack(x);
				}
				if (query == 2)
				{
					deque.PopFront();
				}
				if (query == 3)
				{
					deque.PopBack();
				}
				if (query == 4)
				{
					var k = sr.ReadInt();
					Console.WriteLine(deque[k]);
				}
			}
		}
	}
}
