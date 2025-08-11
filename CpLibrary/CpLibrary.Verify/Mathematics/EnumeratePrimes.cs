using CpLibrary.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify.Mathematics
{
	// competitive-verifier: document_title EnumeratePrime (Library Checker: Enumerate Primes)
	internal class EnumeratePrimes : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/enumerate_primes";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var (n, a, b) = sr.ReadValue<int, int, int>();
			var prime = new List<int>();
			PrimeEnumerator.EnumeratePrime(n, prime);

			var ans = new List<int>();
			for (int i = 0; i < prime.Count; i++)
			{
				if (i >= b && (i - b) % a == 0) ans.Add(prime[i]);
			}

			Console.WriteLine($"{prime.Count} {ans.Count}");
			Console.WriteLine(ans.Join(" "));
		}
	}
}
