using System;
using System.IO;
using System.Linq;
using Xunit;
using CpLibrary.Judge;
using FluentAssertions;
using System.Text;

namespace CpLibrary.Test.IO;

public class ScannerTest
{
	public static TheoryData<string, int[]> IntData => new()
	{
		{
			"""
			5
			3 1 4 1 5
			""",
			new[]
			{
				3,1,4,1,5
			}
		},
		{
			"""
			0
			
			""",
			new int[]{ }
		},
		{
			"""
			10
			123456789 1000000000 2000000000 300000000 400000000 500000000 600000000 700000000 800000000 900000000
			""",
			new []
			{
				123456789, 1000000000, 2000000000, 300000000, 400000000, 500000000, 600000000, 700000000, 800000000, 900000000
			}
		},
	};

	[Theory]
	[MemberData(nameof(IntData))]
	public static void IntTest(string input, int[] expected)
	{
		var encoding = new UTF8Encoding(false);
		var inputStream = new MemoryStream();
		inputStream.Write(encoding.GetBytes(input));
		inputStream.Seek(0, SeekOrigin.Begin);
		var sr = new Scanner(inputStream, 64); // small buffer size for testing purpose
		var n = sr.ReadInt();
		var a = sr.ReadIntArray(n);
		n.Should().Be(expected.Length);
		a.Should().BeEquivalentTo(expected);
	}

	public static TheoryData<string, string[]> StringData => new()
	{
		{
			"""
			aaa bbb ccc ddd
			""",
			new[]
			{
				"aaa", "bbb", "ccc", "ddd"
			}
		},
		{
			"""
			Lorem ipsum dolor sit amet consectetur adipiscing elit sed do eiusmod tempor incididunt ut labore et dolore magna aliqua Ut enim ad minim veniam quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur Excepteur sint occaecat cupidatat non proident sunt in culpa qui officia deserunt mollit anim id est laborum
			""",
			new []
			{
				"Lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua", "Ut", "enim", "ad", "minim", "veniam", "quis", "nostrud", "exercitation", "ullamco", "laboris", "nisi", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "Duis", "aute", "irure", "dolor", "in", "reprehenderit", "in", "voluptate", "velit", "esse", "cillum", "dolore", "eu", "fugiat", "nulla", "pariatur", "Excepteur", "sint", "occaecat", "cupidatat", "non", "proident", "sunt", "in", "culpa", "qui", "officia", "deserunt", "mollit", "anim", "id", "est", "laborum"
			}
		}
	};

	[Theory]
	[MemberData(nameof(StringData))]
	public static void StringTest(string input, string[] expected)
	{
		var encoding = new UTF8Encoding(false);
		var inputStream = new MemoryStream();
		inputStream.Write(encoding.GetBytes(input));
		inputStream.Seek(0, SeekOrigin.Begin);
		var sr = new Scanner(inputStream, 64); // small buffer size for testing purpose
		var s = sr.ReadStringArray(expected.Length);
		s.Should().BeEquivalentTo(expected);
	}
}
