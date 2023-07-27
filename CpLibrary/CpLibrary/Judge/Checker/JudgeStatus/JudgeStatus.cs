using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge.Checker
{
	/// <summary>
	/// Indicates the final result of the judge.
	/// </summary>
	[Flags]
	public enum JudgeStatus
	{
		AC = 1 << 0,
		WA = 1 << 1,
		TLE = 1 << 2,
		RE = 1 << 3,
		PE = 1 << 4,
		IE = 1 << 5
	}
}
