using CompetitiveVerifier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Verify;

internal abstract class VerifySolver : ProblemSolver
{
	internal Scanner sr;
	internal StreamWriter sw;

	public override sealed void Solve()
	{
		sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
		sw = new StreamWriter(Console.OpenStandardOutput());
		Run();
		sw.Flush();
		sw.Dispose();
	}

	public abstract void Run();
}
