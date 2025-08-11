using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
{
	/// <summary>
	/// Checks whether actual output is accepted.
	/// </summary>
	public abstract class CheckerBase
	{
		private bool HasTimeLimit { get => TimeLimit != TimeSpan.Zero; }
		public TimeSpan TimeLimit { get; set; } = TimeSpan.Zero;
		// Currently not supported because the judge calls C# methods in the same process.
		public int MemoryLimitKB { get; set; } = 0;

		protected Action<StreamReader, StreamWriter> Solution;

		protected CheckerBase(Action<StreamReader, StreamWriter> Solution)
		{
			this.Solution = Solution;
		}

		public JudgeResult Run(Stream inputStream, Stream expectedStream, Stream actualStream)
		{
			var judgeResult = new JudgeResult();

			inputStream.Seek(0, SeekOrigin.Begin);
			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var actualWriter = new StreamWriter(actualStream, leaveOpen: true))
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					var runTask = Task.Run(() => Solution(inputReader, actualWriter));
					if (HasTimeLimit) runTask.Wait(TimeLimit * 2);
					else runTask.Wait();
				}
				catch (Exception)
				{
					judgeResult.Status = JudgeStatus.RE;
					return judgeResult;
				}
				stopwatch.Stop();
				judgeResult.Time = stopwatch.Elapsed;

				if (HasTimeLimit && stopwatch.Elapsed > TimeLimit)
				{
					judgeResult.Status = JudgeStatus.TLE;
					return judgeResult;
				}
			}

			inputStream.Seek(0, SeekOrigin.Begin);
			expectedStream.Seek(0, SeekOrigin.Begin);
			actualStream.Seek(0, SeekOrigin.Begin);
			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var expectedReader = new StreamReader(expectedStream, leaveOpen: true))
			using (var actualReader = new StreamReader(actualStream, leaveOpen: true))
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					var runTask = Task.Run(() => Judge(inputReader, expectedReader, actualReader));
					runTask.Wait();
					judgeResult.Status = runTask.Result;
				}
				catch (Exception)
				{
					judgeResult.Status = JudgeStatus.IE;
					return judgeResult;
				}
				stopwatch.Stop();

				if (HasTimeLimit && stopwatch.Elapsed > TimeLimit)
				{
					judgeResult.Status = JudgeStatus.IE;
					return judgeResult;
				}
			}
			return judgeResult;
		}

		public JudgeResult Run(Stream inputStream, Stream expectedStream)
		{
			using var actualStream = new MemoryStream();
			return Run(inputStream, expectedStream, actualStream);
		}

		public JudgeResult Run(Stream inputStream, string expected) => Run(inputStream, ToMemoryStream(expected));

		public JudgeResult Run(string input, Stream expectedStream) => Run(ToMemoryStream(input), expectedStream);

		public JudgeResult Run(string input, string expected) => Run(ToMemoryStream(input), ToMemoryStream(expected));

		public JudgeResult Run(Stream inputStream, Action<StreamReader, StreamWriter> expectedSolution)
		{
			using var actualStream = new MemoryStream();
			return Run(inputStream, expectedSolution, actualStream);
		}

		public JudgeResult Run(string input, Action<StreamReader, StreamWriter> expectedSolution) => Run(ToMemoryStream(input), expectedSolution);

		public JudgeResult Run(Stream inputStream, Action<StreamReader, StreamWriter> expectedSolution, Stream actualStream)
		{
			using var expectedStream = new MemoryStream();

			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var expectedWriter = new StreamWriter(expectedStream, leaveOpen: true))
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();
				try
				{
					var runTask = Task.Run(() => expectedSolution(inputReader, expectedWriter));
					runTask.Wait();
				}
				catch (Exception)
				{
					return new JudgeResult
					{
						Status = JudgeStatus.IE
					};
				}
				stopwatch.Stop();

				if (HasTimeLimit && stopwatch.Elapsed > TimeLimit)
				{
					return new JudgeResult
					{
						Status = JudgeStatus.IE
					};
				}
			}

			inputStream.Seek(0, SeekOrigin.Begin);
			expectedStream.Seek(0, SeekOrigin.Begin);
			return Run(inputStream, expectedStream, actualStream);
		}

		public TotalJudgeResult Run(IEnumerable<(Stream Input, Stream OutPut)> streams)
		{
			var resultTotal = new TotalJudgeResult();
			foreach (var (Input, Output) in streams)
			{
				var resultIter = Run(Input, Output);
				resultTotal.AddResult(resultIter);
			}
			return resultTotal;
		}

		public TotalJudgeResult Run(IEnumerable<(string Input, string Output)> testcases)
		{
			return Run(testcases.Select(p => (Input: ToMemoryStream(p.Input) as Stream, Output: ToMemoryStream(p.Output) as Stream)));
		}

		public TotalJudgeResult Run(IEnumerable<Stream> inputStreams, Action<StreamReader, StreamWriter> expectedSolution)
		{
			var resultTotal = new TotalJudgeResult();
			foreach (var input in inputStreams)
			{
				var resultIter = Run(input, expectedSolution);
				resultTotal.AddResult(resultIter);
			}
			return resultTotal;
		}

		public TotalJudgeResult Run(IEnumerable<string> inputs, Action<StreamReader, StreamWriter> expectedSolution)
			=> Run(inputs.Select(p => ToMemoryStream(p)), expectedSolution);

		protected abstract JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual);

		internal static MemoryStream ToMemoryStream(string str)
			=> ToMemoryStream(str, Encoding.Default);

		internal static MemoryStream ToMemoryStream(string str, Encoding encoding)
			=> new MemoryStream(encoding.GetBytes(str));
	}
}
