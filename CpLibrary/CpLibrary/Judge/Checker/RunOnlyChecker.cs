using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge.Checker
{
	public class RunOnlyChecker : CheckerBase
	{
		// Runs the solution, but does not executes the judge.
		// Intended for checking if the solution RE's or TLE's without checking the validity of the answer on Hacker class uses.

		public RunOnlyChecker(Action<StreamReader, StreamWriter> solution): base(solution) { }

		protected override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual) => JudgeStatus.AC;
	}
}
