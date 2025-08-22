using CpLibrary;

namespace CpLibrary.Verify
{
	// competitive-verifier: document_title IO Benchmark (Library Checker: Many A+B)
	internal class MnyAPlusBTest2 : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/many_aplusb";
		public override double? Tle => 5.0;
		public override void Solve()
		{
			var sr = new ScannerOld(new StreamReader(Console.OpenStandardInput()));
			var sw = new StreamWriter(Console.OpenStandardOutput());

			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var (x, y) = sr.ReadValue<long, long>();
				sw.WriteLine(x + y);
			}
		}
	}
}
