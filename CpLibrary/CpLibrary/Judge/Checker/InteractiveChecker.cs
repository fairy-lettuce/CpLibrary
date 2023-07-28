using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge.Checker
{
	public class InteractiveChecker : CheckerBase
	{
		public InteractiveChecker(Action<StreamReader, StreamWriter> solution) : base(solution) { }

		public new JudgeResult Run(Stream inputStream, Stream expectedStream)
		{
			throw new NotImplementedException();
		}

		protected override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			throw new NotImplementedException();
		}
	}
}
