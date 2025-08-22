using System;
using System.Collections.Generic;
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
		if ((s0 | s1 | s2 | s3) == 0)
		{
			throw new ArgumentException();
		}
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
		if (b < 0) throw new ArgumentOutOfRangeException();
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (int)(r % (uint)b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int Next(int a, int b)
	{
		if (a > b) throw new ArgumentException();
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
			MemoryMarshal.Write(b.Slice(i), ref r);
			i += 8;
		}
		if (i < b.Length)
		{
			ulong r = NextULong();
			Span<byte> lastBytes = stackalloc byte[8];
			MemoryMarshal.Write(lastBytes, ref r);

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
		if (b < 0) throw new ArgumentOutOfRangeException();
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (long)(r % (ulong)b);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public long NextLong(long a, long b)
	{
		if (a > b) throw new ArgumentException();
		return NextLong(b - a) + a;
	}
}
