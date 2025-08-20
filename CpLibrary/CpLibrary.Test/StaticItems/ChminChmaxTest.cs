using CpLibrary;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CpLibrary.Test;

public class ChminChmaxTest
{
	public static TheoryData<int, int, (int min, int max)> ChminChmaxData => new()
	{
		{ 10, 2, (2, 10) },
		{ 1, 1, (1, 1) },
		{ 5, -100, (-100, 5) },
		{ 20, 2025, (20, 2025) },
		{ int.MaxValue, int.MinValue, (int.MinValue, int.MaxValue) }
	};

	[Theory]
	[MemberData(nameof(ChminChmaxData))]
	public void Test(int target, int value, (int min, int max) expected)
	{
		var t1 = target;
		var t2 = target;
		t1.Chmin(value);
		t2.Chmax(value);
		(t1, t2).Should().Be(expected);
	}
}
