using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify.Mathematics
{   // competitive-verifier: document_title IsPrime (Library Checker: Primality Test)
	internal class PrimalityTest : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/primality_test";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var n = sr.ReadLong();
				Console.WriteLine(StaticItems.YesNo(Factorizer.IsPrime(n)));
			}
		}
	}
}
