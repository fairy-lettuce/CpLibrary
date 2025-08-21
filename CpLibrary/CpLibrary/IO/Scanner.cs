using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace CpLibrary
{
	public class Scanner
	{
		public StreamReader sr { get; private set; }

		char[] buffer;
		int len;
		int index;

		char[] separators;

		char[]? pooledToken;
		int pooledTokenLen;
		bool hasPooledToken;

		public Scanner(StreamReader sr, char[] separators, int size = 1 << 12)
		{
			this.sr = sr;
			this.separators = separators;
			buffer = GC.AllocateUninitializedArray<char>(size);
			len = sr.Read(buffer, 0, buffer.Length);
			index = 0;
		}

		public Scanner(StreamReader sr, int size = 1 << 12) : this(sr, new char[] { ' ' }, size) { }

		public Scanner(int size = 1 << 12) : this(new StreamReader(Console.OpenStandardInput()), new char[] { ' ' }, size) { }

		public void Dispose()
		{
			if (hasPooledToken && pooledToken != null)
			{
				ArrayPool<char>.Shared.Return(pooledToken, clearArray: false);
				pooledToken = null;
				hasPooledToken = false;
				pooledTokenLen = 0;
			}
		}

		static bool IsSep(char c, ReadOnlySpan<char> extra)
		{
			if (c <= ' ') return true;
			for (int i = 0; i < extra.Length; i++) if (c == extra[i]) return true;
			return false;
		}

		protected void Refill()
		{
			len = sr.Read(buffer, 0, buffer.Length);
			index = 0;
		}

		protected ReadOnlySpan<char> ReadToken()
		{
			if (hasPooledToken)
			{
				ArrayPool<char>.Shared.Return(pooledToken!, clearArray: false);
				pooledToken = null;
				hasPooledToken = false;
				pooledTokenLen = 0;
			}

			while (true)
			{
				while (index < len && IsSep(buffer[index], separators)) index++;
				if (index < len) break;
				Refill();
				if (len == 0) return ReadOnlySpan<char>.Empty;
			}

			int start = index;
			while (index < len && !IsSep(buffer[index], separators)) index++;

			if (index < len)
			{
				return new ReadOnlySpan<char>(buffer, start, index - start);
			}

			int firstChunk = len - start;
			pooledToken = ArrayPool<char>.Shared.Rent(Math.Max(64, firstChunk * 2));
			hasPooledToken = true;
			if (firstChunk > 0)
			{
				buffer.AsSpan(start, firstChunk).CopyTo(pooledToken);
				pooledTokenLen = firstChunk;
			}

			while (true)
			{
				Refill();
				if (len == 0) break;

				int i = index;
				while (i < len && !IsSep(buffer[i], separators)) i++;

				int chunkLen = i - index;
				int need = pooledTokenLen + chunkLen;
				if (pooledToken!.Length < need)
				{
					var bigger = ArrayPool<char>.Shared.Rent(need * 2);
					pooledToken.AsSpan(0, pooledTokenLen).CopyTo(bigger);
					ArrayPool<char>.Shared.Return(pooledToken, clearArray: false);
					pooledToken = bigger;
				}

				if (chunkLen > 0)
				{
					buffer.AsSpan(index, chunkLen).CopyTo(pooledToken.AsSpan(pooledTokenLen));
					pooledTokenLen += chunkLen;
				}

				index = i;
				if (index < len) break;
			}

			return new ReadOnlySpan<char>(pooledToken!, 0, pooledTokenLen);
		}

		public string Read() => ReadToken().ToString();

		public string ReadString() => Read();

		public string[] ReadStringArray(int n) => ReadValueArray<string>(n);

		public int ReadInt() => ReadValue<int>();

		public int[] ReadIntArray(int n) => ReadValueArray<int>(n);

		public long ReadLong() => ReadValue<long>();

		public long[] ReadLongArray(int n) => ReadValueArray<long>(n);

		public double ReadDouble() => ReadValue<double>();

		public double[] ReadDoubleArray(int n) => ReadValueArray<double>(n);

		public BigInteger ReadBigInteger() => BigInteger.Parse(ReadToken(), CultureInfo.InvariantCulture);

		public T1 ReadValue<T1>()
			where T1 : ISpanParsable<T1>
			=> T1.Parse(ReadToken(), CultureInfo.InvariantCulture);

		public (T1, T2) ReadValue<T1, T2>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			=> (ReadValue<T1>(), ReadValue<T2>());
		public (T1, T2, T3) ReadValue<T1, T2, T3>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>());
		public (T1, T2, T3, T4) ReadValue<T1, T2, T3, T4>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>());
		public (T1, T2, T3, T4, T5) ReadValue<T1, T2, T3, T4, T5>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>());
		public (T1, T2, T3, T4, T5, T6) ReadValue<T1, T2, T3, T4, T5, T6>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>());
		public (T1, T2, T3, T4, T5, T6, T7) ReadValue<T1, T2, T3, T4, T5, T6, T7>()
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
			where T7 : ISpanParsable<T7>
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>(), ReadValue<T7>());

		public T1[] ReadValueArray<T1>(int n)
			where T1 : ISpanParsable<T1>
		{
			var arr = GC.AllocateUninitializedArray<T1>(n);
			for (int i = 0; i < n; i++)
			{
				arr[i] = ReadValue<T1>();
			}
			return arr;
		}
		public (T1[], T2[]) ReadValueArray<T1, T2>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i]) = ReadValue<T1, T2>();
			}
			return (a1, a2);
		}
		public (T1[], T2[], T3[]) ReadValueArray<T1, T2, T3>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			var a3 = GC.AllocateUninitializedArray<T3>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i], a3[i]) = ReadValue<T1, T2, T3>();
			}
			return (a1, a2, a3);
		}
		public (T1[], T2[], T3[], T4[]) ReadValueArray<T1, T2, T3, T4>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			var a3 = GC.AllocateUninitializedArray<T3>(n);
			var a4 = GC.AllocateUninitializedArray<T4>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i], a3[i], a4[i]) = ReadValue<T1, T2, T3, T4>();
			}
			return (a1, a2, a3, a4);
		}
		public (T1[], T2[], T3[], T4[], T5[]) ReadValueArray<T1, T2, T3, T4, T5>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			var a3 = GC.AllocateUninitializedArray<T3>(n);
			var a4 = GC.AllocateUninitializedArray<T4>(n);
			var a5 = GC.AllocateUninitializedArray<T5>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i], a3[i], a4[i], a5[i]) = ReadValue<T1, T2, T3, T4, T5>();
			}
			return (a1, a2, a3, a4, a5);
		}
		public (T1[], T2[], T3[], T4[], T5[], T6[]) ReadValueArray<T1, T2, T3, T4, T5, T6>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			var a3 = GC.AllocateUninitializedArray<T3>(n);
			var a4 = GC.AllocateUninitializedArray<T4>(n);
			var a5 = GC.AllocateUninitializedArray<T5>(n);
			var a6 = GC.AllocateUninitializedArray<T6>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i], a3[i], a4[i], a5[i], a6[i]) = ReadValue<T1, T2, T3, T4, T5, T6>();
			}
			return (a1, a2, a3, a4, a5, a6);
		}
		public (T1[], T2[], T3[], T4[], T5[], T6[], T7[]) ReadValueArray<T1, T2, T3, T4, T5, T6, T7>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
			where T7 : ISpanParsable<T7>
		{
			var a1 = GC.AllocateUninitializedArray<T1>(n);
			var a2 = GC.AllocateUninitializedArray<T2>(n);
			var a3 = GC.AllocateUninitializedArray<T3>(n);
			var a4 = GC.AllocateUninitializedArray<T4>(n);
			var a5 = GC.AllocateUninitializedArray<T5>(n);
			var a6 = GC.AllocateUninitializedArray<T6>(n);
			var a7 = GC.AllocateUninitializedArray<T7>(n);
			for (int i = 0; i < n; i++)
			{
				(a1[i], a2[i], a3[i], a4[i], a5[i], a6[i], a7[i]) = ReadValue<T1, T2, T3, T4, T5, T6, T7>();
			}
			return (a1, a2, a3, a4, a5, a6, a7);
		}

		public (T1, T2)[] ReadTupleArray<T1, T2>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2>();
			}
			return ret;
		}
		public (T1, T2, T3)[] ReadTupleArray<T1, T2, T3>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3>();
			}
			return ret;
		}
		public (T1, T2, T3, T4)[] ReadTupleArray<T1, T2, T3, T4>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5)[] ReadTupleArray<T1, T2, T3, T4, T5>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4, T5)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4, T5>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5, T6)[] ReadTupleArray<T1, T2, T3, T4, T5, T6>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4, T5, T6)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4, T5, T6>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5, T6, T7)[] ReadTupleArray<T1, T2, T3, T4, T5, T6, T7>(int n)
			where T1 : ISpanParsable<T1>
			where T2 : ISpanParsable<T2>
			where T3 : ISpanParsable<T3>
			where T4 : ISpanParsable<T4>
			where T5 : ISpanParsable<T5>
			where T6 : ISpanParsable<T6>
			where T7 : ISpanParsable<T7>
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4, T5, T6, T7)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4, T5, T6, T7>();
			}
			return ret;
		}
	}
}
