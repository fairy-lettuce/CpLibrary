using CpLibrary.Mathematics;

namespace CpLibrary.Verify.Mathematics;

// competitive-verifier: document_title Binomial Coefficient (Library Checker: Binomial Coefficient (Prime Mod))
internal class BinomialCoefficientTest : VerifySolver
{
	public override string Url => "https://judge.yosupo.jp/problem/binomial_coefficient_prime_mod";
	public override double? Tle => 10.0;
	public override void Run()
	{
		var (t, m) = sr.ReadValue<int, uint>();
		var f = new BinomialCoefficient(m, 10000000);
		for (int i = 0; i < t; i++)
		{
			var (n, k) = sr.ReadValue<int, int>();
			sw.WriteLine(f.Binom(n, k));
		}
	}
}
