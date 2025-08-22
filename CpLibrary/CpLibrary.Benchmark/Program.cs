using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CpLibrary;
using CpLibrary.Util;
using System.Buffers;
using System.Globalization;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;

public static class Program
{
	public static void Main()
	{
		string tempPath = Path.GetTempFileName();
		var summary = BenchmarkRunner.Run<ScannerBenchMark>();
	}

	public static void CreateFile(string path)
	{
		using var sw = new StreamWriter(new FileStream(path, FileMode.Create, FileAccess.Write)) { AutoFlush = false };
		var rand = new Xoshiro256StarStar();
		var q = 1000000;
		sw.WriteLine(q);
		var MIN_A = 0;
		var MAX_A = 1000_000_000;
		for (int i = 0; i < q; i++)
		{
			var (x, y) = (rand.Next(MIN_A, MAX_A + 1), rand.Next(MIN_A, MAX_A + 1));
			sw.WriteLine($"{x} {y}");
		}
		sw.Flush();
	}
}

public class ScannerBenchMark
{
	string path;

	[GlobalSetup]
	public void Setup()
	{
		path = System.IO.Path.GetTempFileName();
		Program.CreateFile(path);
	}

	[Benchmark]
	public void ScannerRead()
	{
		using var r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
		var sr = new Scanner(r);
		var q = sr.ReadInt();
		for (int i = 0; i < q; i++)
		{
			var (a, b) = sr.ReadValue<int, int>();
		}
	}
	[Benchmark]
	public void ScannerBufferRead()
	{
		using var r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
		var sr = new ScannerBuffer(r);
		var q = sr.ReadInt();
		for (int i = 0; i < q; i++)
		{
			var (a, b) = sr.ReadValue<int, int>();
		}
	}
	[Benchmark]
	public void ScannerStringSplitRead()
	{
		using var r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
		var sr = new ScannerStringSplit(r);
		var q = sr.ReadInt();
		for (int i = 0; i < q; i++)
		{
			var (a, b) = sr.ReadValue<int, int>();
		}
	}
}

public class ScannerBuffer : IDisposable
{
	public StreamReader sr { get; private set; }

	char[] buffer;
	int len;
	int index;

	char[] separators;

	char[]? pooledToken;
	int pooledTokenLen;
	bool hasPooledToken;

	public ScannerBuffer(StreamReader sr, char[] separators, int size = 1 << 12)
	{
		this.sr = sr;
		this.separators = separators;
		buffer = GC.AllocateUninitializedArray<char>(size);
		len = sr.Read(buffer, 0, buffer.Length);
		index = 0;
	}

	public ScannerBuffer(StreamReader sr, int size = 1 << 12) : this(sr, new char[] { ' ' }, size) { }

	public ScannerBuffer(int size = 1 << 12) : this(new StreamReader(Console.OpenStandardInput()), new char[] { ' ' }, size) { }

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

	public string[] ReadStringArray(int n)
	{
		var arr = GC.AllocateUninitializedArray<string>(n);
		for (int i = 0; i < n; i++)
		{
			arr[i] = ReadString();
		}
		return arr;
	}

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

public class ScannerStringSplit
{
	public StreamReader sr { get; private set; }

	string[] str;
	int index;

	char[] separators;

	public ScannerStringSplit(StreamReader sr, char[] separators)
	{
		this.sr = sr;
		this.separators = separators;
		str = new string[0];
		index = 0;
	}

	public ScannerStringSplit(StreamReader sr) : this(sr, new char[] { ' ' }) { }

	public ScannerStringSplit() : this(new StreamReader(Console.OpenStandardInput()), new char[] { ' ' }) { }

	public string Read()
	{
		if (index < str.Length) return str[index++];
		string s;
		do s = sr.ReadLine();
		while (s == "");
		str = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
		index = 0;
		return str[index++];
	}

	public string ReadString() => Read();

	public string[] ReadStringArray(int n)
	{
		var arr = new string[n];
		for (int i = 0; i < n; i++)
		{
			arr[i] = ReadString();
		}
		return arr;
	}

	public int ReadInt() => int.Parse(ReadString());

	public int[] ReadIntArray(int n) => ReadValueArray<int>(n);

	public long ReadLong() => long.Parse(ReadString());

	public long[] ReadLongArray(int n) => ReadValueArray<long>(n);

	public double ReadDouble() => double.Parse(ReadString());

	public double[] ReadDoubleArray(int n) => ReadValueArray<double>(n);

	public BigInteger ReadBigInteger() => BigInteger.Parse(ReadString());

	public T1 ReadValue<T1>()
		where T1 : IParsable<T1>
		=> T1.Parse(ReadString(), CultureInfo.InvariantCulture);

	public (T1, T2) ReadValue<T1, T2>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		=> (ReadValue<T1>(), ReadValue<T2>());
	public (T1, T2, T3) ReadValue<T1, T2, T3>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>());
	public (T1, T2, T3, T4) ReadValue<T1, T2, T3, T4>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>());
	public (T1, T2, T3, T4, T5) ReadValue<T1, T2, T3, T4, T5>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>());
	public (T1, T2, T3, T4, T5, T6) ReadValue<T1, T2, T3, T4, T5, T6>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
		=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>());
	public (T1, T2, T3, T4, T5, T6, T7) ReadValue<T1, T2, T3, T4, T5, T6, T7>()
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
		where T7 : IParsable<T7>
		=> (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>(), ReadValue<T7>());

	public T1[] ReadValueArray<T1>(int n)
		where T1 : IParsable<T1>
	{
		var arr = new T1[n];
		for (int i = 0; i < n; i++)
		{
			arr[i] = ReadValue<T1>();
		}
		return arr;
	}
	public (T1[], T2[]) ReadValueArray<T1, T2>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
	{
		var (v1, v2) = (new T1[n], new T2[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i]) = ReadValue<T1, T2>();
		}
		return (v1, v2);
	}
	public (T1[], T2[], T3[]) ReadValueArray<T1, T2, T3>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
	{
		var (v1, v2, v3) = (new T1[n], new T2[n], new T3[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i], v3[i]) = ReadValue<T1, T2, T3>();
		}
		return (v1, v2, v3);
	}
	public (T1[], T2[], T3[], T4[]) ReadValueArray<T1, T2, T3, T4>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
	{
		var (v1, v2, v3, v4) = (new T1[n], new T2[n], new T3[n], new T4[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i], v3[i], v4[i]) = ReadValue<T1, T2, T3, T4>();
		}
		return (v1, v2, v3, v4);
	}
	public (T1[], T2[], T3[], T4[], T5[]) ReadValueArray<T1, T2, T3, T4, T5>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
	{
		var (v1, v2, v3, v4, v5) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i], v3[i], v4[i], v5[i]) = ReadValue<T1, T2, T3, T4, T5>();
		}
		return (v1, v2, v3, v4, v5);
	}
	public (T1[], T2[], T3[], T4[], T5[], T6[]) ReadValueArray<T1, T2, T3, T4, T5, T6>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
	{
		var (v1, v2, v3, v4, v5, v6) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n], new T6[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i], v3[i], v4[i], v5[i], v6[i]) = ReadValue<T1, T2, T3, T4, T5, T6>();
		}
		return (v1, v2, v3, v4, v5, v6);
	}
	public (T1[], T2[], T3[], T4[], T5[], T6[], T7[]) ReadValueArray<T1, T2, T3, T4, T5, T6, T7>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
		where T7 : IParsable<T7>
	{
		var (v1, v2, v3, v4, v5, v6, v7) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n], new T6[n], new T7[n]);
		for (int i = 0; i < n; i++)
		{
			(v1[i], v2[i], v3[i], v4[i], v5[i], v6[i], v7[i]) = ReadValue<T1, T2, T3, T4, T5, T6, T7>();
		}
		return (v1, v2, v3, v4, v5, v6, v7);
	}

	public (T1, T2)[] ReadTupleArray<T1, T2>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
	{
		var ret = new (T1, T2)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2>();
		}
		return ret;
	}
	public (T1, T2, T3)[] ReadTupleArray<T1, T2, T3>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
	{
		var ret = new (T1, T2, T3)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2, T3>();
		}
		return ret;
	}
	public (T1, T2, T3, T4)[] ReadTupleArray<T1, T2, T3, T4>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
	{
		var ret = new (T1, T2, T3, T4)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2, T3, T4>();
		}
		return ret;
	}
	public (T1, T2, T3, T4, T5)[] ReadTupleArray<T1, T2, T3, T4, T5>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
	{
		var ret = new (T1, T2, T3, T4, T5)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2, T3, T4, T5>();
		}
		return ret;
	}
	public (T1, T2, T3, T4, T5, T6)[] ReadTupleArray<T1, T2, T3, T4, T5, T6>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
	{
		var ret = new (T1, T2, T3, T4, T5, T6)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2, T3, T4, T5, T6>();
		}
		return ret;
	}
	public (T1, T2, T3, T4, T5, T6, T7)[] ReadTupleArray<T1, T2, T3, T4, T5, T6, T7>(int n)
		where T1 : IParsable<T1>
		where T2 : IParsable<T2>
		where T3 : IParsable<T3>
		where T4 : IParsable<T4>
		where T5 : IParsable<T5>
		where T6 : IParsable<T6>
		where T7 : IParsable<T7>
	{
		var ret = new (T1, T2, T3, T4, T5, T6, T7)[n];
		for (int i = 0; i < n; i++)
		{
			ret[i] = ReadValue<T1, T2, T3, T4, T5, T6, T7>();
		}
		return ret;
	}
}
