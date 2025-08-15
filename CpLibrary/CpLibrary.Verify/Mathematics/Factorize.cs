using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify.Mathematics
{   // competitive-verifier: document_title Factorize (Library Checker: Factorize)
	internal class Factorize : VerifySolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/factorize";
		public override double? Tle => 10.0;
		public override void Run()
		{
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var n = sr.ReadLong();
				var factors = Factorizer.Factorize(n);
				var ans = new List<long>();
				foreach (var (key, value) in factors)
				{
					for (int j = 0; j < value; j++)
					{
						ans.Add(key);
					}
				}
				sw.WriteLine($"{ans.Count} {ans.Join(" ")}");
			}
		}
	}
}
