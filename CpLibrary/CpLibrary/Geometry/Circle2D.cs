using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Geometry;

public static partial class Geometry2D
{
	public readonly struct Circle2D
	{
		readonly Complex center;
		readonly double radius;

		public Circle2D(Complex center, double radius)
		{
			this.center = center;
			this.radius = radius;
		}

		public Complex[] Intersection(Line2D other)
		{
			var comp = other.Distance(center).Compare(radius);
			if (comp < 0) return null;
			else if (comp == 0)
			{
				var ret = new Complex[1];
				ret[0] = other.Projection(center);
				return ret;
			}
			else
			{
				var ret = new Complex[2];
				var h = other.Projection(center);
				var ohLen = (h - center).Magnitude;
				var vec = (h - center).RotateDegree(90);
				var len = Math.Sqrt(Math.Max(0, radius * radius - ohLen * ohLen));
				ret[0] = h + vec.Normalize() * len;
				ret[1] = h - vec.Normalize() * len;
				return ret;
			}
		}

		public Complex[] Intersection(Circle2D other)
		{
			var dist = (this.center).Distance(other.center);
			var comp = dist.Compare(this.radius + other.radius);
			if (comp > 0) return null;
			else
			{
				var x = (Pow2(dist) + Pow2(this.radius) - Pow2(other.radius)) / (2 * dist);
				var h = (other.center - this.center).Normalize() * x;
				if (comp == 1)
				{
					var ret = new Complex[1];
					ret[0] = h;
					return ret;
				}
				var vec = (other.center - this.center).RotateDegree(90);
				var y = Math.Sqrt(Math.Max(0, Pow2(this.radius) - Pow2(x)));
				var ret2 = new Complex[2];
				ret2[0] = h + vec.Normalize() * y;
				ret2[1] = h - vec.Normalize() * y;
				return ret2;
			}
		}
	}
}
