using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Util;

public class Xoshiro256StarStar : IRandom
{
	private ulong s0, s1, s2, s3;

	public Xoshiro256StarStar() : this(new Random()) { }

	public Xoshiro256StarStar(int seed) : this(new Random(seed)) { }

	public Xoshiro256StarStar(ulong s0, ulong s1, ulong s2, ulong s3)
	{
		this.s0 = s0;
		this.s1 = s1;
		this.s2 = s2;
		this.s3 = s3;
		Debug.Assert((s0 | s1 | s2 | s3) != 0);
	}

	public Xoshiro256StarStar(Random rand)
	{
		Span<ulong> b = stackalloc ulong[4];
		do
		{
			rand.NextBytes(MemoryMarshal.AsBytes(b));
			s0 = b[0];
			s1 = b[1];
			s2 = b[2];
			s3 = b[3];
		} while ((s0 | s1 | s2 | s3) == 0);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public ulong NextULong()
	{
		ulong res = BitOperations.RotateLeft(s1 * 5, 7) * 9;
		ulong t = s1 << 17;
		s2 ^= s0;
		s3 ^= s1;
		s1 ^= s2;
		s0 ^= s3;
		s2 ^= t;
		s3 = BitOperations.RotateLeft(s3, 45);
		return res;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint NextUInt() => (uint)(NextULong() >> 32);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next() => (int)(NextUInt() >> 1);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next(int b)
	{
		Debug.Assert(b >= 0);
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (int)(r % (uint)b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next(int a, int b)
	{
		Debug.Assert(a <= b);
		return Next(b - a) + a;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void NextBytes(byte[] b) => NextBytes((Span<byte>)b);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void NextBytes(Span<byte> b)
	{
		int i = 0;
		while (i + 8 <= b.Length)
		{
			ulong r = NextULong();
#if NET8_0_OR_GREATER
			MemoryMarshal.Write(b.Slice(i), in r);
#else
			MemoryMarshal.Write(b.Slice(i), ref r);
#endif
			i += 8;
		}
		if (i < b.Length)
		{
			ulong r = NextULong();
			Span<byte> lastBytes = stackalloc byte[8];
#if NET8_0_OR_GREATER
			MemoryMarshal.Write(lastBytes, in r);
#else
			MemoryMarshal.Write(lastBytes, ref r);
#endif

			lastBytes.Slice(0, b.Length - i).CopyTo(b.Slice(i));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long NextLong() => (long)(NextULong() >> 1);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long NextLong(long b)
	{
		Debug.Assert(b >= 0);
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (long)(r % (ulong)b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long NextLong(long a, long b)
	{
		Debug.Assert(a <= b);
		return NextLong(b - a) + a;
	}
}
