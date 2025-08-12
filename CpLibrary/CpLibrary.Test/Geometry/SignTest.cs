using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xunit;
using CpLibrary.Judge;
using FluentAssertions;
using System.Threading.Tasks;
using System.Collections.Generic;
using CpLibrary.Geometry;

namespace CpLibrary.Test
{
	public class SignTest
	{
		[Theory]
		[InlineData(50, 1)]
		[InlineData(0.01, 1)]
		[InlineData(0, 0)]
		[InlineData(-0.01, -1)]
		[InlineData(-50, -1)]
		public static void SignPlusTest(double x, int ans)
		{
			var res = Geometry2D.Sign(x);
			res.Should().Be(ans);
		}
	}
}
