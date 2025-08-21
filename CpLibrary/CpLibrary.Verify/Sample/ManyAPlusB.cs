using CpLibrary;

namespace CpLibrary.Verify
{
	// competitive-verifier: document_title IO Benchmark (Library Checker: Many A+B)
	internal class MnyAPlusBTest : VerifySolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/many_aplusb";
		public override double? Tle => 5.0;
		public override void Run()
		{
			var q = sr.ReadInt();
			for (int i = 0; i < q; i++)
			{
				var (x, y) = sr.ReadValue<long, long>();
				sw.WriteLine(x + y);
			}
		}
	}
}
