using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Mathematics
{
	public readonly struct MontgomeryReduction
	{
		readonly ulong mod;
		readonly ulong r;
		readonly ulong rinv;
		readonly ulong rneginv;
		readonly ulong one;

		public MontgomeryReduction(ulong mod)
		{
			this.mod = mod;
			this.r = (ulong)(-(UInt128)mod % mod);
			this.rinv = mod;
			for (int i = 0; i < 5; i++)
			{
				rinv *= 2 - mod * rinv;
			}
			this.rneginv = ~rinv + 1;
			this.one = ToMontgomery(1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong Reduce(UInt128 t)
		{
			var ret = (ulong)(((UInt128)((ulong)t * rneginv) * mod + t) >> 64);
			return ret >= mod ? ret - mod : ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ToMontgomery(ulong x)
		{
			return Reduce(x % mod * r);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong ToInteger(ulong x)
		{
			var ret = Reduce(x);
			return ret >= mod ? ret - mod : ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong Mult(ulong x, ulong y)
		{
			return Reduce((UInt128)x * y);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ulong Pow(ulong a, long p)
		{
			ulong ret = one;
			for (int i = 0; (1L << i) <= p; i++)
			{
				if ((p & (1L << i)) > 0)
				{
					ret = Mult(ret, a);
				}

				a = Mult(a, a);
			}
			return ret;
		}
	}
}
