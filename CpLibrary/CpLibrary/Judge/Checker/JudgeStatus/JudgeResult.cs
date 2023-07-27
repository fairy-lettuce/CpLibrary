using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge.Checker
{
	public struct JudgeResult
	{
		public JudgeStatus Status;
		public TimeSpan Time;
		public long Memory;
	}
}
