using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge.Checker
{
	public class SpecialChecker : CheckerBase
	{
		private Action<StreamReader, StreamWriter> judge;

		public SpecialChecker(Action<StreamReader, StreamWriter> solution) : base(solution) { }

		public SpecialChecker(Action<StreamReader, StreamWriter> solution, Action<StreamReader, StreamWriter> judge) : this(solution)
		{
			this.judge = judge;
		}

		protected override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			throw new NotImplementedException();
		}
	}
}
