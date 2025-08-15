using CpLibrary.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify.Collections
{
	// competitive-verifier: document_title Set<T> (Library Checker: OrderedSet)
	internal class SetTestOrderedSet : VerifySolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/ordered_set";
		public override double? Tle => 10.0;
		public override void Run()
		{
			var (n, q) = sr.ReadValue<int, int>();
			var a = sr.ReadIntArray(n);
			var s = new Set<int>(a);
			for (int i = 0; i < q; i++)
			{
				var (query, x) = sr.ReadValue<int, int>();
				if (query == 0)
				{
					if (s.Contains(x)) { }
					else s.Add(x);
				}
				if (query == 1)
				{
					if (s.Contains(x)) s.Remove(x);
				}
				if (query == 2)
				{
					if (s.Count < x) sw.WriteLine(-1);
					else sw.WriteLine(s[x - 1]);
				}
				if (query == 3)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(0);
						continue;
					}
					var idx = s.UpperBound(x);
					sw.WriteLine(idx);
				}
				if (query == 4)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(-1);
						continue;
					}
					var idx = s.UpperBound(x);
					if (idx <= 0) sw.WriteLine(-1);
					else
					{
						sw.WriteLine(s[idx - 1]);
					}
				}
				if (query == 5)
				{
					if (s.Count == 0)
					{
						sw.WriteLine(-1);
						continue;
					}
					var idx = s.LowerBound(x);
					if (idx >= s.Count) sw.WriteLine(-1);
					else
					{
						sw.WriteLine(s[idx]);
					}
				}
			}
		}
	}
}
