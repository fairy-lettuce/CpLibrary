using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using CpLibrary.Judge.Checker;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CpLibrary.Test.Judge.Checker
{
	public class NormalCheckerTest
	{
		[Theory]
		[InlineData(3, 1, 4, 1000)]
		[InlineData(1, 5, 6, 1000)]
		public static void AcceptedTest(int a, int b, int sum, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				writer.WriteLine(a + b);
			}

			var checker = new NormalChecker(Actual);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.AC);
		}

		[Theory]
		[InlineData(3, 1, 4, 1000)]
		[InlineData(1, 5, 6, 1000)]
		public static void WrongAnswerTest(int a, int b, int sum, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b, but wrong answer
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				writer.WriteLine(a + b + 1);
			}

			var checker = new NormalChecker(Actual);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.WA);
		}

		[Theory]
		[InlineData(3, 1, 4, 500, 600)]
		public static void TimeLimitExceededTest(int a, int b, int sum, int timeLimit, int waitTime)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b, but TLE
				Thread.Sleep(waitTime);
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				writer.WriteLine(a + b);
			}

			var checker = new NormalChecker(Actual);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.TLE);
		}

		[Theory]
		[InlineData(3, 1, 4, 1000)]
		public static void RuntimeErrorTest(int a, int b, int sum, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b, but runtime error occurs
				throw new Exception("Test");
			}

			var checker = new NormalChecker(Actual);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.RE);
		}

		public static IEnumerable<object[]> GetData()
		{
			yield return new object[]
			{
				new List<(int a, int b, int sum, JudgeStatus status)>
				{
					(3, 1, 4, JudgeStatus.AC),
					(1, 4, 5, JudgeStatus.WA),
					(1, 1, 2, JudgeStatus.RE),
					(10, 10, 100, JudgeStatus.TLE)
				},
				new Dictionary<JudgeStatus, int>
				{
					{ JudgeStatus.AC, 1 },
					{ JudgeStatus.WA, 1 },
					{ JudgeStatus.RE, 1 },
					{ JudgeStatus.TLE, 1 }
				},
				500
			};
		}

		[Theory]
		[MemberData(nameof(GetData))]
		public static void MultipleTestcasesTest(
			IEnumerable<(int a, int b, int sum, JudgeStatus status)> testcases,
			Dictionary<JudgeStatus, int> expectedDictionary,
			int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b, but sometimes wrong...
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				if (a < b)
				{
					writer.WriteLine(a + b + 1);
					return;
				}
				if (a == 1 && b == 1)
				{
					throw new Exception("edge case runtime error");
				}
				if (a >= 10 && b >= 10)
				{
					Thread.Sleep(timeLimit * 2);
				}
				writer.WriteLine(a + b);
			}

			var checker = new NormalChecker(Actual);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var tc = testcases.Select(p => ($"{p.a} {p.b}\n", $"{p.sum}\n"));
			var result = checker.Run(tc);
			result.Status.Should().Be(testcases.Select(p => p.status).Max());
			result.StatusCount.Should().Equal(expectedDictionary);
		}
	}
}
