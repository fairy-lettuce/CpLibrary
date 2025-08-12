using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{
	public class SpecialChecker : CheckerBase
	{
		private Func<StreamReader, StreamReader, StreamReader, JudgeStatus> judge;

		public SpecialChecker(Action<StreamReader, StreamWriter> solution) : base(solution) { }

		public SpecialChecker(Action<StreamReader, StreamWriter> solution, Func<StreamReader, StreamReader, StreamReader, JudgeStatus> judge) : this(solution)
		{
			this.judge = judge;
		}

		protected override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			return this.judge(input, expected, actual);
		}
	}
}
