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
	private ulong[] s;

	public Xoshiro256StarStar() : this(new Random()) { }

	public Xoshiro256StarStar(int seed) : this(new Random(seed)) { }

	public Xoshiro256StarStar(ulong s0, ulong s1, ulong s2, ulong s3)
	{
		s = new[] { s0, s1, s2, s3 };
		if ((s[0] | s[1] | s[2] | s[3]) == 0)
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
			s = b.ToArray();
		} while ((s[0] | s[1] | s[2] | s[3]) == 0);
	}

	public ulong NextULong()
	{
		ulong res = BitOperations.RotateLeft(s[1] * 5, 7) * 9;
		ulong t = s[1] << 17;
		s[2] ^= s[0];
		s[3] ^= s[1];
		s[1] ^= s[2];
		s[0] ^= s[3];
		s[2] ^= t;
		s[3] = BitOperations.RotateLeft(s[3], 45);
		return res;
	}

	public uint NextUInt() => (uint)(NextULong() >> 32);

	public int Next() => (int)(NextUInt() >> 1);

	public int Next(int b)
	{
		if (b < 0) throw new ArgumentOutOfRangeException();
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (int)(r % (uint)b);
	}

	public int Next(int a, int b)
	{
		if (a > b) throw new ArgumentException();
		return Next(b - a) + a;
	}

	public void NextBytes(byte[] b) => NextBytes((Span<byte>)b);

	public void NextBytes(Span<byte> b)
	{
		int i = 0;
		while (i + 8 <= b.Length)
		{
			ulong r = NextULong();
			MemoryMarshal.Write(b.Slice(i), r);
			i += 8;
		}
		if (i < b.Length)
		{
			ulong r = NextULong();
			Span<byte> lastBytes = stackalloc byte[8];
			MemoryMarshal.Write(lastBytes, r);

			lastBytes.Slice(0, b.Length - i).CopyTo(b.Slice(i));
		}
	}

	public double NextDouble() => (NextULong() >> 11) * (1.0 / (1UL << 53));

	public long NextLong() => (long)(NextULong() >> 1);

	public long NextLong(long b)
	{
		if (b < 0) throw new ArgumentOutOfRangeException();
		if (b == 0) return 0;
		ulong threshold = ulong.MaxValue - ulong.MaxValue % (ulong)b;
		ulong r;
		while ((r = NextULong()) >= threshold) { }
		return (long)(r % (ulong)b);
	}

	public long NextLong(long a, long b)
	{
		if (a > b) throw new ArgumentException();
		return NextLong(b - a) + a;
	}
}
