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
		public new JudgeResult Run(MemoryStream inputStream, MemoryStream expectedStream)
		{
			throw new NotImplementedException();
		}

		public override JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			throw new NotImplementedException();
		}
	}
}
