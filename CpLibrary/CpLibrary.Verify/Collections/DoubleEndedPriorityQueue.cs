using CpLibrary.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify.Collections
{
	// competitive-verifier: document_title Set<T> (Library Checker: Double-Ended Priority Queue)
	internal class DoubleEndedPriorityQueue : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/double_ended_priority_queue";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));

			var (n, q) = sr.ReadValue<int, int>();
			var s = sr.ReadLongArray(n);
			var set = new MultiSet<long>(s);
			for (int i = 0; i < q; i++)
			{
				var query = sr.ReadInt();
				if (query == 0)
				{
					var x = sr.ReadInt();
					set.Add(x);
				}
				else if (query == 1)
				{
					Console.WriteLine(set.Min());
					set.RemoveAt(0);
				}
				else
				{
					Console.WriteLine(set.Max());
					set.RemoveAt(set.Count - 1);
				}
			}
		}
	}
}
