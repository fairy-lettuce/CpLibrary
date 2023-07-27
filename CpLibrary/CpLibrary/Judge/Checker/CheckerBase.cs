﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Judge
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

	public struct JudgeResult
	{
		public JudgeStatus Status;
		public TimeSpan Time;
		public long Memory;
	}

	/// <summary>
	/// Checks whether actual output is accepted.
	/// </summary>
	public abstract class CheckerBase
	{
		protected TimeSpan TimeLimit { get; set; } = TimeSpan.Zero;
		// Currently not supported because the judge calls C# methods in the same process.
		protected int MemoryLimitKB { get; set; } = 0;

		protected Action<StreamReader, StreamWriter> Solution;

		public JudgeResult Run(MemoryStream inputStream, MemoryStream expectedStream)
		{
			using var actualStream = new MemoryStream();

			inputStream.Seek(0, SeekOrigin.Begin);
			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var actualWriter = new StreamWriter(actualStream, leaveOpen: true))
			{
				Solution(inputReader, actualWriter);
			}

			var result = new JudgeStatus();

			inputStream.Seek(0, SeekOrigin.Begin);
			expectedStream.Seek(0, SeekOrigin.Begin);
			actualStream.Seek(0, SeekOrigin.Begin);
			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var expectedReader = new StreamReader(expectedStream, leaveOpen: true))
			using (var actualReader = new StreamReader(actualStream, leaveOpen: true))
			{
				result = Judge(inputReader, expectedReader, actualReader);
			}
			return new JudgeResult
			{
				Status = result
			};
		}

		public JudgeResult Run(MemoryStream inputStream, string expected) => Run(inputStream, ToMemoryStream(expected));

		public JudgeResult Run(string input, MemoryStream expectedStream) => Run(ToMemoryStream(input), expectedStream);

		public JudgeResult Run(string input, string expected) => Run(ToMemoryStream(input), ToMemoryStream(expected));

		public JudgeResult Run(MemoryStream inputStream, Action<StreamReader, StreamWriter> expectedSolution)
		{
			using var expectedStream = new MemoryStream();

			using (var inputReader = new StreamReader(inputStream, leaveOpen: true))
			using (var expectedWriter = new StreamWriter(expectedStream, leaveOpen: true))
			{
				expectedSolution(inputReader, expectedWriter);
			}

			inputStream.Seek(0, SeekOrigin.Begin);
			expectedStream.Seek(0, SeekOrigin.Begin);
			return Run(inputStream, expectedStream);
		}

		public JudgeResult Run(string input, Action<StreamReader, StreamWriter> expectedSolution) => Run(ToMemoryStream(input), expectedSolution);

		public abstract JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual);

		private protected static MemoryStream ToMemoryStream(string str)
			=> ToMemoryStream(str, Encoding.Default);

		private protected static MemoryStream ToMemoryStream(string str, Encoding encoding)
			=> new MemoryStream(encoding.GetBytes(str));
	}
}
