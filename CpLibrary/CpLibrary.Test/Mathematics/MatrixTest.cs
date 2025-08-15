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

public class MatrixTest
{
	[Fact]
	public void Constructor()
	{
		new Matrix<int>(new[]{
			new[] { 1, 2, 3 },
			new[] { 4, 5, 6 }
		}).ToRowArray().Should().BeEquivalentTo(new[]{
			new[] { 1, 2, 3 },
			new[] { 4, 5, 6 }
		});
		new Matrix<int>(new[,]
		{
			{ 1, 2, 3 },
			{ 4, 5, 6 }
		}).ToRowArray().Should().BeEquivalentTo(new[]{
			new[] { 1, 2, 3 },
			new[] { 4, 5, 6 }
		});
		new Matrix<int>(2, 3, new[] { 1, 2, 3, 4, 5, 6 }).ToRowArray().Should().BeEquivalentTo(new[]{
			new[] { 1, 2, 3 },
			new[] { 4, 5, 6 }
		});
	}

	public static TheoryData<Matrix<int>, Matrix<int>, Matrix<int>> AddData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ -3, 1, -4 },
				{ 1, -5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ -2, 3, -1 },
				{ 5, 0, 12 }
			})
		}
	};

	[Theory]
	[MemberData(nameof(AddData))]
	public void Add(Matrix<int> a, Matrix<int> b, Matrix<int> expected)
	{
		(a + b).Should().Be(expected);
		(b + a).Should().Be(expected);
	}

	public static TheoryData<Matrix<int>, Matrix<int>, Matrix<int>> SubtractData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ -3, 1, -4 },
				{ 1, -5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 4, 1, 7 },
				{ 3, 10, 0 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ -3, 1, -4 },
				{ 1, -5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ -4, -1, -7 },
				{ -3, -10, 0 }
			})
		},
	};

	[Theory]
	[MemberData(nameof(SubtractData))]
	public void Subtract(Matrix<int> a, Matrix<int> b, Matrix<int> c)
	{
		(a - b).Should().Be(c);
	}

	public static TheoryData<Matrix<int>, int, Matrix<int>> MultiplyScalarData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			4,
			new Matrix<int>(new[,]
			{
				{ 4, 8, 12 },
				{ 16, 20, 24 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			0,
			new Matrix<int>(new[,]
			{
				{ 0, 0, 0 },
				{ 0, 0, 0 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			-1,
			new Matrix<int>(new[,]
			{
				{ -1, -2, -3 },
				{ -4, -5, -6 }
			})
		}
	};

	[Theory]
	[MemberData(nameof(MultiplyScalarData))]
	public void MultiplyScalar(Matrix<int> a, int k, Matrix<int> expected)
	{
		(a * k).Should().Be(expected);
		(k * a).Should().Be(expected);
	}

	public static TheoryData<Matrix<int>, Matrix<int>, Matrix<int>> MultiplyData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2 },
				{ 3, 4 }
			}),
			new Matrix<int>(new[,]
			{
				{ 3, 1 },
				{ 2, 4 }
			}),
			new Matrix<int>(new[,]
			{
				{ 7, 9 },
				{ 17, 19 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 3, 1 },
				{ 2, 4 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2 },
				{ 3, 4 }
			}),
			new Matrix<int>(new[,]
			{
				{ 6, 10 },
				{ 14, 20 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 998244353, 1000000007 },
				{ 314159265, 271828182 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 0 },
				{ 0, 1 }
			}),
			new Matrix<int>(new[,]
			{
				{ 998244353, 1000000007 },
				{ 314159265, 271828182 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 0 },
				{ -2, 1 },
				{ 5, 2 },
			}),
			new Matrix<int>(new[,]
			{
				{ 12, 8 },
				{ 24, 17 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 0 },
				{ -2, 1 },
				{ 5, 2 },
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 2, 1, 0 },
				{ 13, 20, 27 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 9, -9, 8 },
				{ 2, 4, 4 },
				{ 35, 0, 3 }
			}),
			new Matrix<int>(new[,]
			{
				{ 5 },
				{ -9 },
				{ 2 }
			}),
			new Matrix<int>(new[,]
			{
				{ 142 },
				{ -18 },
				{ 181 }
			})
		},
	};

	[Theory]
	[MemberData(nameof(MultiplyData))]
	public void Multiply(Matrix<int> a, Matrix<int> b, Matrix<int> expected)
	{
		(a * b).Should().Be(expected);
	}

	public static TheoryData<Matrix<int>, Matrix<int>, bool> EqualData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			true
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 },
				{ 7, 8, 9 }
			}),
			false
		},
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 999 }
			}),
			false
		},
	};

	[Theory]
	[MemberData(nameof(EqualData))]
	public void Equal(Matrix<int> a, Matrix<int> b, bool expected)
	{
		(a == b).Should().Be(expected);
		(a != b).Should().Be(!expected);
	}

	public static TheoryData<Matrix<int>, Matrix<int>> TransposeData => new()
	{
		{
			new Matrix<int>(new[,]
			{
				{ 1, 2, 3 },
				{ 4, 5, 6 }
			}),
			new Matrix<int>(new[,]
			{
				{ 1, 4 },
				{ 2, 5 },
				{ 3, 6 }
			})
		},
		{
			new Matrix<int>(new[,]
			{
				{ 3, 1, 4 },
				{ 1, 5, 9 },
				{ 2, 6, 5 }
			}),
			new Matrix<int>(new[,]
			{
				{ 3, 1, 2 },
				{ 1, 5, 6 },
				{ 4, 9, 5 }
			})
		}
	};

	[Theory]
	[MemberData(nameof(TransposeData))]
	public void Transpose(Matrix<int> a, Matrix<int> b)
	{
		a.Transpose().Should().Be(b);
		b.Transpose().Should().Be(a);
	}
}
