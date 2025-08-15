using CpLibrary.Util;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CpLibrary.Test;

public class Xoshiro256StarStarTest
{
	[Fact]
	public static void SameSeed()
	{
		int N = 100;
		var a = new Xoshiro256StarStar(998244353);
		var b = new Xoshiro256StarStar(998244353);
		var ares = new ulong[N];
		var bres = new ulong[N];
		for (int i = 0; i < N; i++)
		{
			ares[i] = a.NextULong();
			bres[i] = b.NextULong();
		}
		ares.Should().BeEquivalentTo(bres);
	}

	[Fact]
	public static void DifferentSeed()
	{
		int N = 100;
		var a = new Xoshiro256StarStar();
		var b = new Xoshiro256StarStar(42);
		var ares = new ulong[N];
		var bres = new ulong[N];
		for (int i = 0; i < N; i++)
		{
			ares[i] = a.NextULong();
			bres[i] = b.NextULong();
		}
		ares.Should().NotBeEquivalentTo(bres);
	}

	[Fact]
	public static void Range()
	{
		int N = 100;
		const int MIN = -100, MAX = 101;
		var a = new Xoshiro256StarStar(314159265);
		var ares = new int[N];
		for (int i = 0; i < N; i++)
		{
			ares[i] = a.Next(MIN, MAX);
		}
		ares.Should().AllSatisfy(x => x.Should().BeInRange(MIN, MAX - 1));
	}
}
