using CpLibrary.Geometry;
using CpLibrary.Judge;
using CpLibrary.Mathematics;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ModInt = CpLibrary.Mathematics.MontgomeryModInt<AtCoder.Mod998244353>;

namespace CpLibrary.Test;

public class MontgomeryModIntTest
{
	public static TheoryData<ModInt, int> ConstructorData => new()
	{
		{ new ModInt(998244353 * (-2) + 200), 200 },
		{ new ModInt(-1), 998244352 },
		{ new ModInt(0), 0 },
		{ new ModInt(1), 1 },
		{ new ModInt(314), 314 },
		{ new ModInt(998244352), 998244352 },
		{ new ModInt(998244353), 0 },
		{ new ModInt(998244354), 1 },
		{ new ModInt(998244353 + 500), 500 },
		{ new ModInt(998244353L * 2 + 30000), 30000 },
		{ new ModInt(998244353L * 4 + 30000), 30000 },
	};

	[Theory]
	[MemberData(nameof(ConstructorData))]
	public void ConstructorTest(ModInt a, int expected)
	{
		a.Value.Should().Be(expected);
	}

	public static TheoryData<ModInt, ModInt, int> AddData => new()
	{
		{ new ModInt(1), new ModInt(2), 3 },
		{ new ModInt(0), new ModInt(2), 2 },
		{ new ModInt(9), new ModInt(998244352), 8 },
		{ new ModInt(998244352), new ModInt(998244351), 998244350 }
	};

	[Theory]
	[MemberData(nameof(AddData))]
	public void AddTest(ModInt a, ModInt b, int expected)
	{
		(a + b).Value.Should().Be(expected);
		(b + a).Value.Should().Be(expected);
	}

	public static TheoryData<ModInt, ModInt, int> SubtractData => new()
	{
		{ new ModInt(6), new ModInt(2), 4 },
		{ new ModInt(1), new ModInt(2), 998244352 },
		{ new ModInt(0), new ModInt(2), 998244351 },
		{ new ModInt(9), new ModInt(998244352), 10 },
		{ new ModInt(998244352), new ModInt(998244351), 1 },
		{ new ModInt(998244351), new ModInt(998244352), 998244352 },
	};

	[Theory]
	[MemberData(nameof(SubtractData))]
	public void SubtractTest(ModInt a, ModInt b, int expected)
	{
		(a - b).Value.Should().Be(expected);
	}

	public static TheoryData<ModInt, int> IncrementTestData => new()
	{
		{ new ModInt(0), 1 },
		{ new ModInt(1), 2 },
		{ new ModInt(2), 3 },
		{ new ModInt(10), 11 },
		{ new ModInt(998244351), 998244352 },
		{ new ModInt(998244352), 0 }
	};

	[Theory]
	[MemberData(nameof(IncrementTestData))]
	public void IncrementTest(ModInt a, int expected)
	{
		(++a).Value.Should().Be(expected);
	}

	public static TheoryData<ModInt, int> DecrementTestData => new()
	{
		{ new ModInt(0), 998244352 },
		{ new ModInt(1), 0 },
		{ new ModInt(2), 1 },
		{ new ModInt(10), 9 },
		{ new ModInt(998244351), 998244350 },
		{ new ModInt(998244352), 998244351 }
	};

	[Theory]
	[MemberData(nameof(DecrementTestData))]
	public void DecrementTest(ModInt a, int expected)
	{
		(--a).Value.Should().Be(expected);
	}

	public static TheoryData<ModInt, ModInt> EqualsTestData => new()
	{
		{ new ModInt(0), new ModInt(0) },
		{ new ModInt(0), new ModInt(998244353) },
		{ new ModInt(0), new ModInt(998244353 * 2) },
		{ new ModInt(0), new ModInt(998244353L * 3) },
		{ new ModInt(0), new ModInt(998244353L * -1) },
		{ new ModInt(0), new ModInt(998244353L * -2) },
		{ new ModInt(1), new ModInt(1) },
		{ new ModInt(1), new ModInt(998244353 + 1) },
		{ new ModInt(2), new ModInt(998244353 * 2 + 2) },
		{ new ModInt(10), new ModInt(998244353 * (-2) + 10) },
		{ new ModInt(998244351), new ModInt(998244351) },
		{ new ModInt(998244351), new ModInt(-2) },
		{ new ModInt(998244352), new ModInt(998244352) },
		{ new ModInt(998244352), new ModInt(-1) },
		{ new ModInt(998244353 + 1000), new ModInt(998244353L * 2 + 1000) },
		{ new ModInt(998244353 * -1 + 1000), new ModInt(998244353L * 2 + 1000) },
		{ new ModInt(998244353 * -1 + 1000), new ModInt(998244353L * -2 + 1000) },
	};

	[Theory]
	[MemberData(nameof(EqualsTestData))]
	public void EqualsTest(ModInt a, ModInt expected)
	{
		a.Should().Be(expected);
		(a == expected).Should().BeTrue();
		(a != expected).Should().BeFalse();
	}

	public static TheoryData<ModInt, ModInt> NotEqualsTestData => new()
	{
		{ new ModInt(0), new ModInt(1) },
		{ new ModInt(0), new ModInt(998244353 + 1) },
		{ new ModInt(0), new ModInt(998244353 * 2 + 1) },
		{ new ModInt(0), new ModInt(998244353L * 3 + 1) },
		{ new ModInt(0), new ModInt(998244353L * -1 + 1) },
		{ new ModInt(0), new ModInt(998244353L * -2 + 1) },
		{ new ModInt(1), new ModInt(0) },
		{ new ModInt(1), new ModInt(998244353) },
		{ new ModInt(2), new ModInt(998244353 * 2) },
		{ new ModInt(10), new ModInt(998244353 * (-2) + 9) },
		{ new ModInt(998244351), new ModInt(998244352) },
		{ new ModInt(998244351), new ModInt(-1) },
		{ new ModInt(998244352), new ModInt(998244354) },
		{ new ModInt(998244352), new ModInt(-2) },
		{ new ModInt(998244353 + 1000), new ModInt(998244353L * 2 + 100) },
		{ new ModInt(998244353 * -1 + 1000), new ModInt(998244353L * 2 + 100) },
		{ new ModInt(998244353 * -1 + 1000), new ModInt(998244353L * -2 + 100) },
	};

	[Theory]
	[MemberData(nameof(NotEqualsTestData))]
	public void NotEqualsTest(ModInt a, ModInt expected)
	{
		a.Should().NotBe(expected);
		(a == expected).Should().BeFalse();
		(a != expected).Should().BeTrue();
	}

	public static TheoryData<string, int> ParseTestData => new()
	{

		{ "-1996486977", 1729 },
		{ "0", 0 },
		{ "1", 1 },
		{ "2025", 2025 },
		{ "998244352", 998244352 },
		{ "998244353", 0 },
		{ "998244354", 1 },
		{ "998244667", 314 },
		{ "1000000000000", 757402647 },
	};

	[Theory]
	[MemberData(nameof(ParseTestData))]
	public void ParseTest(string s, int expected)
	{
		ModInt.Parse(s).Value.Should().Be(expected);
	}
}
