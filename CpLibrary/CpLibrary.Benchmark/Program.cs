using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CpLibrary;
using CpLibrary.Collections;
using CpLibrary.Collections.Internal;
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
		Generate(sw);
		sw.Flush();
	}

	public static void Generate(StreamWriter sw)
	{
		var rand = new Xoshiro256StarStar();
		var n = 500000;
		var q = 500000;
		var a = Enumerable.Repeat(0, n)
			.Select(p => rand.Next(0, 10000000))
			.Distinct()
			.OrderBy(p => p)
			.ToArray();
		n = a.Length;
		sw.WriteLine($"{n} {q}");
		sw.WriteLine(a.Join(" "));
		for (int i = 0; i < q; i++)
		{
			var query = rand.Next(0, 6);
			var x = rand.Next(query == 2 ? 1 : 0, 10000000);
			sw.WriteLine($"{query} {x}");
		}
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
	public void SetNew()
	{
		using var r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
		var sr = new Scanner(r);
		var sw = new StreamWriter(new MemoryStream(), new UTF8Encoding(false)) { AutoFlush = false };
		var (n, q) = sr.ReadValue<int, int>();
		var a = sr.ReadIntArray(n);
		var s = new SetIndexed<int>(a);
		for (int i = 0; i < q; i++)
		{
			var (query, x) = sr.ReadValue<int, int>();
			if (query == 0)
			{
				s.Add(x);
			}
			if (query == 1)
			{
				s.Remove(x);
			}
			if (query == 2)
			{
				if (s.Count < x) sw.WriteLine(-1);
				else sw.WriteLine(s[x - 1]);
			}
			if (query == 3)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(0);
					continue;
				}
				var idx = s.FindFirstGreaterThan(x).Index;
				sw.WriteLine(idx);
			}
			if (query == 4)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(-1);
					continue;
				}
				var (idx, val) = s.FindLastLessOrEqual(x);
				if (idx <= 0) sw.WriteLine(-1);
				else
				{
					sw.WriteLine(val);
				}
			}
			if (query == 5)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(-1);
					continue;
				}
				var (idx, val) = s.FindFirstGreaterOrEqual(x);
				if (idx >= s.Count) sw.WriteLine(-1);
				else
				{
					sw.WriteLine(val);
				}
			}
		}
		sw.Flush();
	}
	[Benchmark]
	public void SetOld()
	{
		using var r = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read));
		var sr = new Scanner(r);
		var sw = new StreamWriter(new MemoryStream(), new UTF8Encoding(false)) { AutoFlush = false };
		var (n, q) = sr.ReadValue<int, int>();
		var a = sr.ReadIntArray(n);
		var s = new Set<int>(a);
		for (int i = 0; i < q; i++)
		{
			var (query, x) = sr.ReadValue<int, int>();
			if (query == 0)
			{
				s.Add(x);
			}
			if (query == 1)
			{
				s.Remove(x);
			}
			if (query == 2)
			{
				if (s.Count < x) sw.WriteLine(-1);
				else sw.WriteLine(s[x - 1]);
			}
			if (query == 3)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(0);
					continue;
				}
				var idx = s.FindFirstGreaterThan(x).Index;
				sw.WriteLine(idx);
			}
			if (query == 4)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(-1);
					continue;
				}
				var (idx, val) = s.FindLastLessOrEqual(x);
				if (idx <= 0) sw.WriteLine(-1);
				else
				{
					sw.WriteLine(val);
				}
			}
			if (query == 5)
			{
				if (s.Count == 0)
				{
					sw.WriteLine(-1);
					continue;
				}
				var (idx, val) = s.FindFirstGreaterOrEqual(x);
				if (idx >= s.Count) sw.WriteLine(-1);
				else
				{
					sw.WriteLine(val);
				}
			}
		}
		sw.Flush();
	}
}
