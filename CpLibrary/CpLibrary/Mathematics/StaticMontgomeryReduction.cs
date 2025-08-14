using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Runtime.CompilerServices;
using AtCoder;

namespace CpLibrary.Mathematics;

public readonly struct StaticMontgomeryReduction<T> where T: IStaticMod
{
	static readonly uint mod = default(T).Mod;
	static readonly uint r2 = (uint)((1UL << 32) % mod * ((1UL << 32) % mod) % mod);
	static readonly uint modinv = CalculateRneginv();

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	static uint CalculateRneginv()
	{
		var mod = default(T).Mod;
		uint rinv = mod;
		for (int i = 0; i < 5; i++)
		{
			rinv *= 2 - mod * rinv;
		}
		return ~rinv + 1;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Reduce(ulong t)
	{
		return (uint)(((ulong)((uint)t * modinv) * mod + t) >> 32);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ToMontgomery(ulong x)
	{
		return Reduce(x % mod * r2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ToMontgomery(long x)
	{
		long v = x % mod;
		if (v < 0)
		{
			v += mod;
		}
		return Reduce((ulong)v * r2);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint ToInteger(uint x)
	{
		var ret = Reduce(x);
		return ret >= mod ? ret - mod : ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Mult(uint x, uint y)
	{
		return Reduce((ulong)x * y);
	}
}
