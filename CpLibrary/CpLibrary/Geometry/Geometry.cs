using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using Point2D = System.Numerics.Complex;

namespace CpLibrary.Geometry;

public static partial class Geometry2D
{
	public const double EPS = 1e-10;

	/// <summary>
	/// Returns whether <paramref name="x"/> is plus, minus, or zero.
	/// </summary>
	/// <returns>1 if plus, 0 if zero, or -1 if minus.</returns>
	public static int Sign(this double x) => (x < -EPS ? -1 : (x > EPS ? 1 : 0));
	public static int Compare(this double x, double y) => Sign(x - y);

	public static int ComplexCompare(this Complex x, Complex y)
	{
		if (Compare(x.Real, y.Real) != 0) return Compare(x.Real, y.Real);
		else return Compare(x.Imaginary, y.Imaginary);
	}

	public static double Distance(this Complex x, Complex y) => (x - y).Magnitude;

	public static Complex Normalize(this Complex x) => x / x.Magnitude;

	public static Complex Rotate(this Complex z, double theta) => z * Complex.FromPolarCoordinates(1, theta);
	public static Complex RotateDegree(this Complex z, double deg) => Rotate(z, deg / 180 * Math.PI);

	public static double Dot(this Complex x, Complex y) => x.Real * y.Real + x.Imaginary * y.Imaginary;
	public static double Cross(this Complex x, Complex y) => x.Real * y.Imaginary - y.Real * x.Imaginary;

	/// <summary>
	/// Returns the relative position of points a, b, c.
	/// </summary>
	/// <returns>
	/// <para>+1 if BC turns left seen from AB.</para>
	/// <para>-1 if BC turns right seen from AB.</para>
	/// <para>+2 if they are on a line in the order ABC or CBA.</para>
	/// <para>0 if they are on a line in the order ACB or BCA.</para>
	/// <para>-2 if they are on a line in the order BAC or CAB.</para></returns>
	public static int ISP(Complex a, Complex b, Complex c)
	{
		var cross = Sign((b - a).Cross(c - b));
		if (cross > 0) return 1;
		else if (cross < 0) return -1;
		else
		{
			var dot = Sign((b - a).Dot(c - a));
			return dot * 2;
		}
	}

	/// <summary>
	/// Returns whether the angle ABC is acute, right, or obtuse.
	/// </summary>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="c"></param>
	/// <returns><para>+1 if ABC is an acute angle.</para>
	/// <para>0 if ABC is a right angle.</para>
	/// <para>-1 if ABC is a obtuse angle.</para></returns>
	public static int AngleType(Complex a, Complex b, Complex c) => Sign((a - b).Dot(c - b));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static double Pow2(this double x) => x * x;
}
