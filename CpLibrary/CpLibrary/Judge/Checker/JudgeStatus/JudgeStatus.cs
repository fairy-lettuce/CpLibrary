using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{
	/// <summary>
	/// Indicates the final result of the judge.
	/// </summary>
	public enum JudgeStatus
	{
		AC = 1,
		WA = 2,
		TLE = 3,
		RE = 4,
		PE = 5,
		IE = 6
	}
}
