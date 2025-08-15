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
		sw = (StreamWriter)Console.Out;
		Run();
	}

	public abstract void Run();
}
