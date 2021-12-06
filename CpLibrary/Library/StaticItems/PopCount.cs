using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CpLibrary
{
	public static partial class StaticItems
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PopCount(uint n)
		{
			n = (n & 0x55555555) + (n >> 1 & 0x55555555);
			n = (n & 0x33333333) + (n >> 2 & 0x33333333);
			n = (n & 0x0f0f0f0f) + (n >> 4 & 0x0f0f0f0f);
			n = (n & 0x00ff00ff) + (n >> 8 & 0x00ff00ff);
			n = (n & 0x0000ffff) + (n >> 16 & 0x0000ffff);
			return (int)n;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PopCount(int n) => PopCount((uint)n);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PopCount(ulong n)
		{
			n = (n & 0x5555555555555555) + (n >> 1 & 0x5555555555555555);
			n = (n & 0x3333333333333333) + (n >> 2 & 0x3333333333333333);
			n = (n & 0x0f0f0f0f0f0f0f0f) + (n >> 4 & 0x0f0f0f0f0f0f0f0f);
			n = (n & 0x00ff00ff00ff00ff) + (n >> 8 & 0x00ff00ff00ff00ff);
			n = (n & 0x0000ffff0000ffff) + (n >> 16 & 0x0000ffff0000ffff);
			n = (n & 0x00000000ffffffff) + (n >> 32 & 0x00000000ffffffff);
			return (int)n;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int PopCount(long n) => PopCount((ulong)n);
	}
}
