using CpLibrary;

namespace CpLibrary.Verify
{
	// competitive-verifier: document_title Sample Program (Library Checker A+B)
	internal class APlusB : VerifySolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/aplusb";
		public override double? Tle => 2.0;
		public override void Run()
		{
			var a = sr.ReadInt();
			var b = sr.ReadInt();
			sw.WriteLine(a + b);
		}
	}
}
