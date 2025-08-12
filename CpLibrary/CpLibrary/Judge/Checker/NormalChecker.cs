using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{

	public class NormalChecker : CheckerBase
	{
		public NormalChecker(Action<StreamReader, StreamWriter> solution) : base(solution) { }

		protected override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			var expectedString = expected
				.ReadToEnd()
				.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
			var actualString = actual
				.ReadToEnd()
				.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

			if (expectedString.Length != actualString.Length)
			{
				return JudgeStatus.WA;
			}
			foreach (var (expectedToken, actualToken) in expectedString.Zip(actualString))
			{
				if (expectedToken != actualToken)
				{
					return JudgeStatus.WA;
				}
			}
			return JudgeStatus.AC;
		}
	}
}
