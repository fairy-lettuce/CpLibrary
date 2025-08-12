using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{
	public class JudgeResult
	{
		public JudgeStatus Status { get; set; }
		public TimeSpan Time { get; set; }
		public long Memory { get; set; }
		public string JudgeOutput { get; set; }
	}

	public class TotalJudgeResult
	{
		public JudgeStatus Status { get; set; }
		public TimeSpan Time { get; set; }
		public long Memory { get; set; }
		public Dictionary<JudgeStatus, int> StatusCount { get; set; }
		public string JudgeOutput { get; set; }

		public TotalJudgeResult()
		{
			this.StatusCount = new Dictionary<JudgeStatus, int>();
		}

		public TotalJudgeResult(JudgeStatus status) : this()
		{
			this.StatusCount[status] = 1;
		}

		public void AddResult(JudgeResult result)
		{
			if (this.Status < result.Status)
			{
				this.Status = result.Status;
			}
			if (this.Time < result.Time)
			{
				this.Time = result.Time;
			}
			if (this.Memory < result.Memory)
			{
				this.Memory = result.Memory;
			}
			if (this.StatusCount.ContainsKey(result.Status))
			{
				this.StatusCount[result.Status]++;
			}
			else
			{
				this.StatusCount.Add(result.Status, 1);
			}
			this.JudgeOutput += result.JudgeOutput;
		}
	}
}