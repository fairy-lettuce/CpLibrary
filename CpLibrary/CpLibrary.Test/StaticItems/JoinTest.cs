using CpLibrary;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CpLibrary.Test;

public class JoinTest
{
	public static TheoryData<string[], char, string> JoinCharData => new()
	{
		{ new[] { "aaa", "bbb" }, ' ', "aaa bbb" },
		{ new string[] { }, ' ', "" },
		{ new[] { "sample", "test", "test" }, '\n', "sample\ntest\ntest" }
	};

	[Theory]
	[MemberData(nameof(JoinCharData))]
	public void JoinCharTest(string[] list, char separator, string expected)
	{
		list.Join(separator).Should().Be(expected);
	}

	public static TheoryData<string[], string, string> JoinStringData => new()
	{
		{ new[] { "aaa", "bbb" }, " ", "aaa bbb" },
		{ new string[] { }, " ", "" },
		{ new[] { "sample", "test", "test" }, "\n", "sample\ntest\ntest" },
		{ new[] { "a", "b", "c" }, "", "abc" },
		{ new[] { "ABC", "DEF", "GHI" }, "sep", "ABCsepDEFsepGHI" }
	};

	[Theory]
	[MemberData(nameof(JoinStringData))]
	public void JoinStringTest(string[] list, string separator, string expected)
	{
		list.Join(separator).Should().Be(expected);
	}
}
