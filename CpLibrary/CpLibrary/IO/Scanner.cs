using System;
using System.Buffers;
using System.Buffers.Text;
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
		public Stream stream { get; private set; }

		byte[] buffer;
		int length;
		int index;

		readonly static byte[] DEFAULT_SEPARATORS = { (byte)'\n', (byte)'\r', (byte)'\t', (byte)' ' };
		byte[] separators;

		public Scanner(Stream stream, byte[] separators, int size = 1 << 12)
		{
			this.stream = stream;
			this.separators = separators;
			buffer = GC.AllocateUninitializedArray<byte>(size);
			length = index = 0;
		}

		public Scanner(Stream stream, int size = 1 << 12) : this(stream, DEFAULT_SEPARATORS, size) { }

		public Scanner(int size = 1 << 12) : this(Console.OpenStandardInput(), DEFAULT_SEPARATORS, size) { }

		public Scanner(StreamReader sr, byte[] separators, int size = 1 << 12) : this(sr.BaseStream, separators, size) { }

		public Scanner(StreamReader sr, int size = 1 << 12) : this(sr.BaseStream, DEFAULT_SEPARATORS, size) { }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsSeparator(byte c) => c <= ' ';

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static bool IsNewLine(byte c) => c >= '\n' && c <= '\r';

		void Fill()
		{
			if ((uint)index >= (uint)length)
			{
				length = stream.Read(buffer, 0, buffer.Length);
				index = 0;
				if (length < buffer.Length) buffer[length] = (byte)'\n';
			}

			while (IsSeparator(buffer[index]))
			{
				index++;
				if ((uint)index >= (uint)length)
				{
					length = stream.Read(buffer, 0, buffer.Length);
					index = 0;
					if (length < buffer.Length) buffer[length] = (byte)'\n';
				}
			}

			// Int128 can take up to 40 digits
			// We assume inputs are decimal, have no leading zeros, and are never "evil"
			if (index + 64u >= length && !IsSeparator(buffer[^1]))
			{
				buffer.AsSpan(index, length - index).CopyTo(buffer);
				length -= index;
				index = 0;
				length = stream.Read(buffer, length, buffer.Length - length) + length;
				if (length < buffer.Length) buffer[length] = (byte)'\n';
			}
		}

		public Span<byte> ReadToken()
		{
			if ((uint)index >= (uint)length)
			{
				length = stream.Read(buffer, 0, buffer.Length);
				index = 0;
			}

			while (IsSeparator(buffer[index]))
			{
				index++;
				if ((uint)index >= (uint)length)
				{
					length = stream.Read(buffer, 0, buffer.Length);
					index = 0;
					if (length < buffer.Length)
					{
						var p = index;
						index = length;
						return buffer.AsSpan(p, length - p);
					}
				}
			}

			var pool = ArrayPool<byte>.Shared.Rent(buffer.Length);
			var pindex = 0;

#if NET8_0_OR_GREATER
			var next = buffer.AsSpan(index, length - index).IndexOfAnyInRange((byte)0, (byte)' ');
#else
			var next = buffer.AsSpan(index, length - index).IndexOfAny(separators);
#endif

			while (length > 0 && (uint)next >= (uint)length)
			{
				if (pool.Length - pindex < length - index) Resize(ref pool, pool.Length);
				buffer.AsSpan(index).CopyTo(pool.AsSpan(pindex));
				pindex += length - index;
				length = stream.Read(buffer, 0, buffer.Length);
				index = 0;

#if NET8_0_OR_GREATER
				next = buffer.AsSpan(index, length - index).IndexOfAnyInRange((byte)0, (byte)' ');
#else
				next = buffer.AsSpan(index, length - index).IndexOfAny(separators);
#endif

			}

			if (next == -1) next = length;
			if (pool.Length - pindex < next) Resize(ref pool, pool.Length);
			buffer.AsSpan(index, next).CopyTo(pool.AsSpan(pindex));
			pindex += next;
			index = next + index;

			try
			{
				return pool.AsSpan(0, pindex);
			}
			finally
			{
				ArrayPool<byte>.Shared.Return(pool);
			}
		}

		void Resize<T>(ref T[] arr, int l)
		{
			var p = arr;
			arr = ArrayPool<T>.Shared.Rent(l << 1);
			p.AsSpan().CopyTo(arr);
			ArrayPool<T>.Shared.Return(p);
		}

		public string Read() => Encoding.UTF8.GetString(ReadToken());

		public string ReadString() => Read();

		public string[] ReadStringArray(int n)
		{
			var arr = GC.AllocateUninitializedArray<string>(n);
			for (int i = 0; i < n; i++)
			{
				arr[i] = ReadString();
			}
			return arr;
		}


		public uint ReadUInt()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out uint value, out int bc);
			index += bc;
			return value;
		}

		public int ReadInt()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out int value, out int bc);
			index += bc;
			return value;
		}
		public ulong ReadULong()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out ulong value, out int bc);
			index += bc;
			return value;
		}

		public long ReadLong()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out long value, out int bc);
			index += bc;
			return value;
		}

		public double ReadDouble()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out double value, out int bc);
			index += bc;
			return value;
		}

		public decimal ReadDecimal()
		{
			Fill();
			Utf8Parser.TryParse(buffer.AsSpan(index), out decimal value, out int bc);
			index += bc;
			return value;
		}

		public int[] ReadIntArray(int n) => ReadValueArray<int>(n);
		public uint[] ReadUIntArray(int n) => ReadValueArray<uint>(n);
		public ulong[] ReadULongArray(int n) => ReadValueArray<ulong>(n);
		public long[] ReadLongArray(int n) => ReadValueArray<long>(n);
		public double[] ReadDoubleArray(int n) => ReadValueArray<double>(n);
		public decimal[] ReadDecimalArray(int n) => ReadValueArray<decimal>(n);

		//public BigInteger ReadBigInteger() => BigInteger.Parse(ReadToken(), CultureInfo.InvariantCulture);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T ReadValue<T>()
		{
			if (typeof(T) == typeof(int)) return (T)(object)ReadInt();
			if (typeof(T) == typeof(uint)) return (T)(object)ReadUInt();
			if (typeof(T) == typeof(long)) return (T)(object)ReadLong();
			if (typeof(T) == typeof(ulong)) return (T)(object)ReadULong();
			if (typeof(T) == typeof(double)) return (T)(object)ReadDouble();
			if (typeof(T) == typeof(decimal)) return (T)(object)ReadDecimal();
			if (typeof(T) == typeof(string)) return (T)(object)ReadString();
			throw new NotSupportedException();
		}

		public (T1, T2) ReadValue<T1, T2>()
			=> (ReadValue<T1>(), ReadValue<T2>());
		public (T1, T2, T3) ReadValue<T1, T2, T3>()
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>());
		public (T1, T2, T3, T4) ReadValue<T1, T2, T3, T4>()
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>());
		public (T1, T2, T3, T4, T5) ReadValue<T1, T2, T3, T4, T5>()
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>());
		public (T1, T2, T3, T4, T5, T6) ReadValue<T1, T2, T3, T4, T5, T6>()
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>());
		public (T1, T2, T3, T4, T5, T6, T7) ReadValue<T1, T2, T3, T4, T5, T6, T7>()
			=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>(), ReadValue<T7>());

		public T1[] ReadValueArray<T1>(int n)
		{
			var arr = GC.AllocateUninitializedArray<T1>(n);
			for (int i = 0; i < n; i++)
			{
				arr[i] = ReadValue<T1>();
			}
			return arr;
		}
		public (T1[], T2[]) ReadValueArray<T1, T2>(int n)
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
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2>();
			}
			return ret;
		}
		public (T1, T2, T3)[] ReadTupleArray<T1, T2, T3>(int n)
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3>();
			}
			return ret;
		}
		public (T1, T2, T3, T4)[] ReadTupleArray<T1, T2, T3, T4>(int n)
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5)[] ReadTupleArray<T1, T2, T3, T4, T5>(int n)
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4, T5)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4, T5>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5, T6)[] ReadTupleArray<T1, T2, T3, T4, T5, T6>(int n)
		{
			var ret = GC.AllocateUninitializedArray<(T1, T2, T3, T4, T5, T6)>(n);
			for (int i = 0; i < n; i++)
			{
				ret[i] = ReadValue<T1, T2, T3, T4, T5, T6>();
			}
			return ret;
		}
		public (T1, T2, T3, T4, T5, T6, T7)[] ReadTupleArray<T1, T2, T3, T4, T5, T6, T7>(int n)
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
