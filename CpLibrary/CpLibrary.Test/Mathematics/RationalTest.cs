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

namespace CpLibrary.Test;

// Borrowed from https://github.com/kzrnm/Kzrnm.Competitive/blob/main/Library/Competitive.Library.Test/Number/FractionTests.cs
/*
Competitive.Library


MIT License

Copyright (c) 2020 naminodarie

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */


public class RationalTest
{
	static IEnumerable<Rational<long>> RandomRationals(Random rnd)
		=> Enumerable.Repeat(rnd, 1000).Select(rnd => new Rational<long>(rnd.Next(), rnd.Next()));
	public static TheoryData<long, long, long, long> Construct_Data => new()
{
	{ 16, 4, 4, 1 },
	{ 2, 845106, 1, 422553 },
	{ 230895518700, 230811434700, 9995477, 9991837 },
	{ 1, 2, 1, 2 },
	{ -1,  2, -1, 2 },
	{  1, -2, -1, 2 },
	{ -1, -2,  1, 2 },
	{  2,  2,  1, 1 },
	{ -2,  2, -1, 1 },
	{  2, -2, -1, 1 },
	{ -2, -2,  1, 1 },
};
	[Theory]
	[MemberData(nameof(Construct_Data))]
	[Trait("Category", "Normal")]
	public void ConstructTest(long 分子in, long 分母in, long 分子out, long 分母out)
	{
		var f = new Rational<long>(分子in, 分母in);
		f.Numerator.Should().Be(分子out);
		f.Denominator.Should().Be(分母out);
	}

	public static TheoryData<long, long, string> ToString_Data => new()
{
	{ 16, 4, "4/1" },
	{ 2, 845106, "1/422553" },
	{ 230895518700, 230811434700, "9995477/9991837" },
	{ 1, 2, "1/2" },
	{ -1, 2, "-1/2" },
	{ 1, -2, "-1/2" },
};
	[Theory]
	[MemberData(nameof(ToString_Data))]
	[Trait("Category", "Normal")]
	public void ParseAndToStringTest(long numerator, long denominator, string text)
	{
		var num = new Rational<long>(numerator, denominator);
		num.ToString().Should().Be(text);
		Rational<long>.Parse(text).Should().Be(num);
		Rational<long>.Parse($"{numerator}/{denominator}").Should().Be(num);
	}
	[Fact]
	[Trait("Category", "Normal")]
	public void LongImplicitTest()
	{
		foreach (var num in Enumerable.Repeat(new Random(150), 1000).Select(rnd => rnd.Next()))
		{
			Rational<long> f = num;
			f.Should().Be(new Rational<long>(num, 1));
		}
	}

	[Fact]
	[Trait("Category", "Normal")]
	public void EqualsTest()
	{
		foreach (var f in RandomRationals(new Random(3153)))
		{
			var f2 = f;
			(f == f2).Should().BeTrue();
			(f != f2).Should().BeFalse();
			(f >= f2).Should().BeTrue();
			(f <= f2).Should().BeTrue();
			(f > f2).Should().BeFalse();
			(f < f2).Should().BeFalse();
			f.Equals(f2).Should().BeTrue();
			f.Equals((object)f2).Should().BeTrue();
			f.CompareTo(f2).Should().Be(0);
		}
	}

	public static TheoryData<Rational<long>, Rational<long>> GreaterThan_Data => new()
{
	{ new Rational<long>(3, 1), new Rational<long>(2, 1) },
	{ new Rational<long>(2, 3), new Rational<long>(1, 2) },
	{ new Rational<long>(5, 6), new Rational<long>(4, 9) },
};
	[Theory]
	[MemberData(nameof(GreaterThan_Data))]
	[Trait("Category", "Normal")]
	public void GreaterThanTest(Rational<long> left, Rational<long> right)
	{
		(left == right).Should().BeFalse();
		(left != right).Should().BeTrue();
		(left >= right).Should().BeTrue();
		(left <= right).Should().BeFalse();
		(left > right).Should().BeTrue();
		(left < right).Should().BeFalse();
		left.Equals(right).Should().BeFalse();
		left.Equals((object)right).Should().BeFalse();
		left.CompareTo(right).Should().BeGreaterThan(0);
		right.CompareTo(left).Should().BeLessThan(0);
	}

	[Fact]
	[Trait("Category", "Operator")]
	public void SingleMinusTest()
	{
		foreach (var f in RandomRationals(new Random(13)))
		{
			(-f).Should().Be(new Rational<long>(-f.Numerator, f.Denominator));
		}
	}

	public static TheoryData<Rational<long>, Rational<long>, Rational<long>> Add_Data => new()
{
	{ new Rational<long>(3, 1), new Rational<long>(2, 1), new Rational<long>(5, 1) },
	{ new Rational<long>(1, 2), new Rational<long>(1, 3), new Rational<long>(5, 6) },
	{ new Rational<long>(1, 6), new Rational<long>(1, 3), new Rational<long>(1, 2) },
	{ new Rational<long>(-1,6), new Rational<long>(1, 3), new Rational<long>(1, 6) },
};
	[Theory]
	[Trait("Category", "Operator")]
	[MemberData(nameof(Add_Data))]
	public void AddTest(Rational<long> num1, Rational<long> num2, Rational<long> expected)
	{
		(num1 + num2).Should().Be(expected);
	}

	public static TheoryData<Rational<long>, Rational<long>, Rational<long>> Subtract_Data => new()
{
	{ new Rational<long>(3, 1), new Rational<long>(2, 1), new Rational<long>(1, 1) },
	{ new Rational<long>(1, 2), new Rational<long>(1, 3), new Rational<long>(1, 6) },
	{ new Rational<long>(1, 6), new Rational<long>(1, 3), new Rational<long>(-1, 6) },
	{ new Rational<long>(-1, 6), new Rational<long>(1, 3), new Rational<long>(-1, 2) },
};
	[Theory]
	[Trait("Category", "Operator")]
	[MemberData(nameof(Subtract_Data))]
	public void SubtractTest(Rational<long> num1, Rational<long> num2, Rational<long> expected)
	{
		(num1 - num2).Should().Be(expected);
	}

	public static TheoryData<Rational<long>, Rational<long>, Rational<long>> Multiply_Data => new()
{
	{ new Rational<long>(3, 1), new Rational<long>(5, 1), new Rational<long>(15, 1) },
	{ new Rational<long>(1, 2), new Rational<long>(1, 7), new Rational<long>(1, 14) },
	{ new Rational<long>(-1, 6), new Rational<long>(2, 3), new Rational<long>(-1, 9) },
	{ new Rational<long>(-1, 16), new Rational<long>(-4, 3), new Rational<long>(1, 12) },
};
	[Theory]
	[Trait("Category", "Operator")]
	[MemberData(nameof(Multiply_Data))]
	public void MultiplyTest(Rational<long> num1, Rational<long> num2, Rational<long> expected)
	{
		(num1 * num2).Should().Be(expected);
	}

	public static TheoryData<Rational<long>, Rational<long>, Rational<long>> Divide_Data => new()
{
	{ new Rational<long>(3, 1), new Rational<long>(2, 1), new Rational<long>(3, 2) },
	{ new Rational<long>(1, 2), new Rational<long>(1, 7), new Rational<long>(7, 2) },
	{ new Rational<long>(-1, 6), new Rational<long>(2, 3), new Rational<long>(-1, 4) },
	{ new Rational<long>(-1, 12), new Rational<long>(-4, 3), new Rational<long>(1, 16) },
};
	[Theory]
	[Trait("Category", "Operator")]
	[MemberData(nameof(Divide_Data))]
	public void DivideTest(Rational<long> num1, Rational<long> num2, Rational<long> expected)
	{
		(num1 / num2).Should().Be(expected);
	}

	//[Fact]
	//public void InverseTest()
	//{
	//	foreach (var f in RandomRationals(new Random(48463)))
	//	{
	//		f.Inverse().Should().Be(new Rational<long>(f.Denominator, f.Numerator));
	//	}
	//}
}
