using CpLibrary.Judge.Checker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{
	public class Hacker
	{
		// given actual/expected solutions and the input generator, this class attempts to find a "hack" case (a testcase with which the actual solution fails).
		// solutions are given through CheckerBase subclasses.

		private CheckerBase checker;
		private Action<StreamWriter> generator;
		private Action<StreamReader, StreamWriter> expectedSolution;

		public Hacker(CheckerBase checker, Action<StreamWriter> generator, Action<StreamReader, StreamWriter> expectedSolution)
		{
			this.checker = checker;
			this.generator = generator;
			this.expectedSolution = expectedSolution;
		}

		public MemoryStream FindHackCase()
		{
			while (true)
			{
				using var input = new MemoryStream();
				using (var inputWriter = new StreamWriter(input, leaveOpen: true))
				{
					generator(inputWriter);
				}
				input.Seek(0, SeekOrigin.Begin);

				using var output = new MemoryStream();
				var result = checker.Run(input, expectedSolution, output);
				if (result.Status != JudgeStatus.AC)
				{
					input.Seek(0, SeekOrigin.Begin);
					return input;
				}
			}
		}
	}
}
