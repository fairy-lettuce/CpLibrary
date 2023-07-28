﻿using System;
using System.IO;
using System.Linq;
using Xunit;
using CpLibrary.Judge.Checker;
using FluentAssertions;

namespace CpLibrary.Test.Judge.Checker
{
	public class NormalCheckerTest
	{
		[Theory]
		[InlineData(3, 1, 4)]
		[InlineData(1, 5, 6)]
		public static void AcceptedTest(int a, int b, int sum)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				writer.WriteLine(a + b);
			}

			var checker = new NormalChecker(Actual);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.AC);
		}

		[Theory]
		[InlineData(3, 1, 0)]
		[InlineData(1, 5, 8)]
		public static void WrongAnswerTest(int a, int b, int sum)
		{
			void Actual(StreamReader reader, StreamWriter writer)
			{
				// returns a + b
				var sr = new Scanner(reader);
				var (a, b) = sr.ReadValue<int, int>();
				writer.WriteLine(a + b);
			}

			var checker = new NormalChecker(Actual);
			var input = $"{a} {b}\n";
			var output = $"{sum}\n";
			var result = checker.Run(input, output);
			result.Status.Should().Be(JudgeStatus.WA);
		}
	}
}