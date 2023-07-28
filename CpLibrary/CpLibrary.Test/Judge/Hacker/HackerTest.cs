using System;
using System.IO;
using System.Linq;
using Xunit;
using CpLibrary.Judge.Checker;
using CpLibrary.Judge;

namespace CpLibrary.Judge.Checker.Test
{
	public class HackerTest
	{
		[Fact]
		public static void Test()
		{
			static void Actual(StreamReader reader, StreamWriter writer)
			{
				var sr = new Scanner(reader);
				var n = sr.ReadInt();
				writer.WriteLine("Yes");
			}

			static void Naive(StreamReader reader, StreamWriter writer)
			{
				var _sr = new Scanner(reader);
				var n = _sr.ReadInt();
				if (n == 2)
				{
					writer.WriteLine("No");
					return;
				}
				writer.WriteLine("Yes");
			}

			static void Generate(StreamWriter writer)
			{
				var rand = new System.Random();
				var n = rand.Next(1, 10);
				writer.WriteLine(n);
			}

			var hacker = new Hacker(new NormalChecker(Actual), Generate, Naive);
			var res = hacker.FindHackCase();
			var input = new StreamReader(res).ReadToEnd();

			Assert.Equal("2", input.Trim());
		}
	}
}
