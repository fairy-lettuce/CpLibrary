using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Geometry;

public readonly struct Line2D
{
	readonly Complex begin, end;

	/// <summary>
	/// Contructs Line2d that passes the both points.
	/// </summary>
	public Line2D(Complex begin, Complex end)
	{
		this.begin = begin;
		this.end = end;
	}

	/// <summary>
	/// Constructs Line2d using the formula ax+by+c=0.
	/// </summary>
	public Line2D(double a, double b, double c)
	{
		if (a.Sign() == 0 && b.Sign() == 0) throw new ArgumentException();
		if (a.Sign() == 0)
		{
			begin = new Complex(0.0, -c / b);
			end = new Complex(1.0, -c / b);
		}
		else if (b.Sign() == 0)
		{
			begin = new Complex(-c / a, 0.0);
			end = new Complex(-c / a, 1.0);
		}
		else
		{
			begin = new Complex(-c / a, 0.0);
			end = new Complex(0.0, -c / b);
		}
	}

	public Complex Vector { get => end - begin; }
	public Complex CounterVector { get => begin - end; }

	public Complex Intersection(Line2D other)
	{
		if (this.Vector.Cross(other.Vector).Sign() == 0) return Complex.NaN;
		return this.begin + this.Vector
			* ((other.end - this.begin).Cross(other.Vector) / this.Vector.Cross(other.Vector));
	}

	public double Distance(Complex other) => Math.Abs(this.Vector.Cross(other - this.begin) / this.Vector.Magnitude);

	public Complex Projection(Complex other)
		=> this.begin + this.Vector.Normalize() * this.Vector.Dot(other - this.begin) / this.Vector.Magnitude;

	public Complex Reflection(Complex other) => other + 2 * (this.Projection(other) - other);
}
