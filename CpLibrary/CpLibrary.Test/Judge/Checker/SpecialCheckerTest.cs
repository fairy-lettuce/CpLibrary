using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using CpLibrary.Judge;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CpLibrary.Test.Judge
{
	public class SpecialCheckerTest
	{
		// Problem: Given a positive integer x, output two positive integers a, b such that a + b = z
		// If there are no solutions, just output `-1`.

		public static JudgeStatus Judge(StreamReader input, StreamReader expected, StreamReader actual)
		{
			var srInput = new Scanner(input);
			var srActual = new Scanner(actual);
			var x = srInput.ReadInt();
			if (x == 1)
			{
				var actualStr = srActual.ReadString();
				if (actualStr != "-1")
				{
					return JudgeStatus.WA;
				}
				return JudgeStatus.AC;
			}
			var (a, b) = srActual.ReadValue<int, int>();
			if (a + b != x)
			{
				return JudgeStatus.WA;
			}
			return JudgeStatus.AC;
		}

		[Theory]
		[InlineData(10, 1000)]
		[InlineData(1, 1000)]
		public static void AcceptedTest(int x, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				var sr = new Scanner(reader);
				var x = sr.ReadInt();
				if (x == 1) writer.WriteLine(-1);
				else writer.WriteLine($"{x - 1} {1}");
			}

			var checker = new SpecialChecker(Actual, Judge);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{x}\n";
			var output = $"\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.AC);
		}

		[Theory]
		[InlineData(1, 1000)]
		public static void WrongAnswerTest(int x, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				var sr = new Scanner(reader);
				var x = sr.ReadInt();
				writer.WriteLine($"{x - 1} {1}");
			}

			var checker = new SpecialChecker(Actual, Judge);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{x}\n";
			var output = $"\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.WA);
		}

		[Theory]
		[InlineData(1, 500, 600)]
		public static void TimeLimitExceededTest(int x, int timeLimit, int waitTime)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				Thread.Sleep(waitTime);
				var sr = new Scanner(reader);
				var x = sr.ReadInt();
				if (x == 1) writer.WriteLine(-1);
				else writer.WriteLine($"{x - 1} {1}");
			}

			var checker = new SpecialChecker(Actual, Judge);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{x}\n";
			var output = $"\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.TLE);
		}

		[Theory]
		[InlineData(1, 1000)]
		public static void RuntimeErrorTest(int x, int timeLimit)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				throw new Exception("Test");
			}

			var checker = new SpecialChecker(Actual, Judge);
			checker.TimeLimit = TimeSpan.FromMilliseconds(timeLimit);
			var input = $"{x}\n";
			var output = $"\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.RE);
		}
	}
}
