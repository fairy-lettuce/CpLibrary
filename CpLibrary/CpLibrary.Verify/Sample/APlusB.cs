using CpLibrary;

namespace CpLibrary.Verify
{
	// competitive-verifier: document_title Sample Program (Library Checker A+B)
	internal class APlusB : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/aplusb";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var a = sr.ReadInt();
			var b = sr.ReadInt();
			Console.WriteLine(a + b);
		}
	}
}
