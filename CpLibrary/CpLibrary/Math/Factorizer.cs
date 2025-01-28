using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Math
{
	public static class Factorizer
	{
		// http://miller-rabin.appspot.com/
		static readonly long[] baseSingle = { 126401071349994536 };
		static readonly long[] baseDouble = { 336781006125, 9639812373923155 };
		static readonly long[] baseQuad = { 2, 2570940, 211991001, 3749873356 };
		static readonly long[] baseBig = { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };

		static readonly long baseSingleMax = 291831;
		static readonly long baseDoubleMax = 1050535501;
		static readonly long baseQuadMax = 47636622961201;

		/// <summary>
		/// Factorizes n.
		/// </summary>
		/// <param name="n">The number to factorize.</param>
		/// <returns>Dictionary that has prime factors in Key, the number of each factor in Value.</returns>
		public static SortedDictionary<long, int> Factorize(long n)
		{
			var ret = new SortedDictionary<long, int>();
			var que = new Queue<long>();
			que.Enqueue(n);
			while (que.Count > 0)
			{
				var now = que.Dequeue();
				if (now == 1) continue;
				if (IsPrime(now))
				{
					if (ret.ContainsKey(now)) ret[now]++;
					else ret.Add(now, 1);
					continue;
				}

				long f = FindFactor(now);
				que.Enqueue(f);
				que.Enqueue(now / f);
			}

			return ret;
		}

		/// <summary>
		/// Tests if n is prime or not using Miller-Rabin Algorithm. Complexity: O(log^2 n)
		/// </summary>
		/// <param name="n">The number to test.</param>
		/// <returns>True if prime, False otherwise.</returns>
		public static bool IsPrime(long n)
		{
			if (n == 1) return false;
			if (n == 2) return true;
			if (n == 3) return true;
			if (n == 5) return true;
			if (n % 2 == 0) return false;
			if (n % 3 == 0) return false;
			if (n % 5 == 0) return false;

			if (n > int.MaxValue) return IsPrimeLong(n);

			long d = n - 1;
			int s = 0;
			while (d % 2 == 0)
			{
				d /= 2;
				s++;
			}

			long[] bases;
			if (n < baseSingleMax) bases = baseSingle;
			else if (n < baseDoubleMax) bases = baseDouble;
			else if (n < baseQuadMax) bases = baseQuad;
			else bases = baseBig;

			foreach (var e in bases)
			{
				if (!MillerRabinTest(e, d, n, s)) return false;
			}
			return true;
		}

		private static bool IsPrimeLong(long n)
		{
			long d = n - 1;
			int s = 0;
			while (d % 2 == 0)
			{
				d /= 2;
				s++;
			}

			long[] bases;
			if (n < baseSingleMax) bases = baseSingle;
			else if (n < baseDoubleMax) bases = baseDouble;
			else if (n < baseQuadMax) bases = baseQuad;
			else bases = baseBig;

			var montgomery = new MontgomeryReduction((ulong)n);

			foreach (var e in bases)
			{
				if (!MillerRabinTestMontgomery(e, d, n, s, montgomery)) return false;
			}
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool MillerRabinTest(long e, long d, long n, long s)
		{
			if (e % n == 0) return true;
			long pow = ModPow(e, d, n);
			if (pow == 1) return true;
			for (int i = 0; i < s; i++)
			{
				if (pow == n - 1) return true;
				pow = ModPow(pow, 2, n);
			}
			return false;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long ModPow(long a, long p, long mod)
		{
			a %= mod;
			long ret = 1;
			for (int i = 0; (1L << i) <= p; i++)
			{
				if ((p & (1L << i)) > 0)
				{
					ret *= a;
					ret %= mod;
				}

				a *= a;
				a %= mod;
			}
			return ret;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static bool MillerRabinTestMontgomery(long e, long d, long n, long s, MontgomeryReduction montgomery)
		{
			var pow = montgomery.Reduce((ulong)e);
			pow = montgomery.Pow(pow, d);
			if (montgomery.ToInteger(pow) == 1) return true;
			for (int i = 0; i < s; i++)
			{
				if (montgomery.ToInteger(pow) == (ulong)n - 1)
					return true;
				pow = montgomery.Mult(pow, pow);
			}
			return false;
		}

		/// <summary>
		/// Finds a factor of n by Pollard's ρ algorithm. Complexity: O(n^(1/4))
		/// </summary>
		/// <param name="n">The number to get a factor.</param>
		/// <returns>A prime factor of n.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static long FindFactor(long n)
		{
			if (n % 2 == 0) return 2;
			long i = 0;
			while (true)
			{
				i++;
				long x = i;
				long y = Next(i, i, n);
				while (true)
				{
					long g = GCD(x - y, n);
					if (g >= n) break;
					if (1 < g) return g;
					x = Next(x, i, n);
					y = Next(y, i, n);
					y = Next(y, i, n);
				}
			}
		}

		private static long Next(long x, long step, long mod)
		{
			Int128 tmp = x;
			tmp *= tmp;
			tmp += step;
			tmp %= mod;
			return (long)tmp;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static long GCD(long a, long b)
		{
			a = System.Math.Abs(a);
			b = System.Math.Abs(b);
			while (a > 0)
			{
				b %= a;
				var x = a;
				a = b;
				b = x;
			}
			return b;
		}
	}
}
