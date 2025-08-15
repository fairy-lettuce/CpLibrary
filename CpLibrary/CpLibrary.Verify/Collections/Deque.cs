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
	// competitive-verifier: document_title Deque<T> (Library Checker: Deque)
	internal class DequeTest : VerifySolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/deque";
		public override double? Tle => 5.0;
		public override void Run()
		{
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
					sw.WriteLine(deque[k]);
				}
			}
		}
	}
}
