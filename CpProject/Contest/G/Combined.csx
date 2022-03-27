using System;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Threading;
namespace CpLibrary.Contest
{
	public class SolverG : SolverBase
	{
		Scanner sr;
		bool hasMultipleTestcases = false;

		bool IsLocal { get; set; }

		public override void Solve()
		{
			/*
			 * Write your code here!
			 */
		}

		public void Init()
		{
			/*
			 * Write your init code here if you need!
			 */
		}

		public SolverG(Scanner sr, bool isLocal = false) { this.sr = sr; this.IsLocal = isLocal; }

		public override void Run()
		{
			Init();
			var _t = 1;
			if (hasMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramG
	{
		private static bool StartsOnThread = true;

		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverG(sr);
			if (StartsOnThread)
			{
				var thread = new Thread(new ThreadStart(() => solver.Run()), 1 << 27);
				thread.Start();
				thread.Join();
			}
			else solver.Run();
			Console.Out.Flush();
		}

		public static void Expand() => SourceExpander.Expander.Expand();
	}
}
#region Expanded by https://github.com/naminodarie/SourceExpander
namespace CpLibrary { public class Scanner { public StreamReader sr { get; private set; }  string[] str; int index; char[] separators; public Scanner(StreamReader sr, char[] separators) { this.sr = sr; this.separators = separators; str = new string[0]; index = 0; }  public Scanner(StreamReader sr) : this(sr, new char[]{' '}) { }  public Scanner() : this(new StreamReader(Console.OpenStandardInput()), new char[]{' '}) { }  public string Read() { if (index < str.Length) return str[index++]; string s; do s = sr.ReadLine(); while (s == ""); str = s.Split(separators, StringSplitOptions.RemoveEmptyEntries); index = 0; return str[index++]; }  public string ReadString() => Read(); public string[] ReadStringArray(int n) { var arr = new string[n]; for (int i = 0; i < n; i++) { arr[i] = ReadString(); }  return arr; }  public int ReadInt() => int.Parse(ReadString()); public int[] ReadIntArray(int n) => ReadValueArray<int>(n); public long ReadLong() => long.Parse(ReadString()); public long[] ReadLongArray(int n) => ReadValueArray<long>(n); public double ReadDouble() => double.Parse(ReadString()); public double[] ReadDoubleArray(int n) => ReadValueArray<double>(n); public BigInteger ReadBigInteger() => BigInteger.Parse(ReadString()); public T1 ReadValue<T1>() => (T1)Convert.ChangeType(ReadString(), typeof(T1)); public (T1, T2) ReadValue<T1, T2>() => (ReadValue<T1>(), ReadValue<T2>()); public (T1, T2, T3) ReadValue<T1, T2, T3>() => (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>()); public (T1, T2, T3, T4) ReadValue<T1, T2, T3, T4>() => (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>()); public (T1, T2, T3, T4, T5) ReadValue<T1, T2, T3, T4, T5>() => (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>()); public (T1, T2, T3, T4, T5, T6) ReadValue<T1, T2, T3, T4, T5, T6>() => (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>()); public (T1, T2, T3, T4, T5, T6, T7) ReadValue<T1, T2, T3, T4, T5, T6, T7>() => (ReadValue<T1>(), ReadValue<T2>(), ReadValue<T3>(), ReadValue<T4>(), ReadValue<T5>(), ReadValue<T6>(), ReadValue<T7>()); public T1[] ReadValueArray<T1>(int n) { var arr = new T1[n]; for (int i = 0; i < n; i++) { arr[i] = ReadValue<T1>(); }  return arr; }  public (T1[], T2[]) ReadValueArray<T1, T2>(int n) { var(v1, v2) = (new T1[n], new T2[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i]) = ReadValue<T1, T2>(); }  return (v1, v2); }  public (T1[], T2[], T3[]) ReadValueArray<T1, T2, T3>(int n) { var(v1, v2, v3) = (new T1[n], new T2[n], new T3[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i], v3[i]) = ReadValue<T1, T2, T3>(); }  return (v1, v2, v3); }  public (T1[], T2[], T3[], T4[]) ReadValueArray<T1, T2, T3, T4>(int n) { var(v1, v2, v3, v4) = (new T1[n], new T2[n], new T3[n], new T4[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i], v3[i], v4[i]) = ReadValue<T1, T2, T3, T4>(); }  return (v1, v2, v3, v4); }  public (T1[], T2[], T3[], T4[], T5[]) ReadValueArray<T1, T2, T3, T4, T5>(int n) { var(v1, v2, v3, v4, v5) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i], v3[i], v4[i], v5[i]) = ReadValue<T1, T2, T3, T4, T5>(); }  return (v1, v2, v3, v4, v5); }  public (T1[], T2[], T3[], T4[], T5[], T6[]) ReadValueArray<T1, T2, T3, T4, T5, T6>(int n) { var(v1, v2, v3, v4, v5, v6) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n], new T6[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i], v3[i], v4[i], v5[i], v6[i]) = ReadValue<T1, T2, T3, T4, T5, T6>(); }  return (v1, v2, v3, v4, v5, v6); }  public (T1[], T2[], T3[], T4[], T5[], T6[], T7[]) ReadValueArray<T1, T2, T3, T4, T5, T6, T7>(int n) { var(v1, v2, v3, v4, v5, v6, v7) = (new T1[n], new T2[n], new T3[n], new T4[n], new T5[n], new T6[n], new T7[n]); for (int i = 0; i < n; i++) { (v1[i], v2[i], v3[i], v4[i], v5[i], v6[i], v7[i]) = ReadValue<T1, T2, T3, T4, T5, T6, T7>(); }  return (v1, v2, v3, v4, v5, v6, v7); }  public (T1, T2)[] ReadTupleArray<T1, T2>(int n) { var ret = new (T1, T2)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2>(); }  return ret; }  public (T1, T2, T3)[] ReadTupleArray<T1, T2, T3>(int n) { var ret = new (T1, T2, T3)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2, T3>(); }  return ret; }  public (T1, T2, T3, T4)[] ReadTupleArray<T1, T2, T3, T4>(int n) { var ret = new (T1, T2, T3, T4)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2, T3, T4>(); }  return ret; }  public (T1, T2, T3, T4, T5)[] ReadTupleArray<T1, T2, T3, T4, T5>(int n) { var ret = new (T1, T2, T3, T4, T5)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2, T3, T4, T5>(); }  return ret; }  public (T1, T2, T3, T4, T5, T6)[] ReadTupleArray<T1, T2, T3, T4, T5, T6>(int n) { var ret = new (T1, T2, T3, T4, T5, T6)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2, T3, T4, T5, T6>(); }  return ret; }  public (T1, T2, T3, T4, T5, T6, T7)[] ReadTupleArray<T1, T2, T3, T4, T5, T6, T7>(int n) { var ret = new (T1, T2, T3, T4, T5, T6, T7)[n]; for (int i = 0; i < n; i++) { ret[i] = ReadValue<T1, T2, T3, T4, T5, T6, T7>(); }  return ret; } } }
namespace CpLibrary { public interface ISolver { public void Solve(); public void Run(); }  public abstract class SolverBase : ISolver { public abstract void Solve(); public abstract void Run(); } }
namespace SourceExpander{public class Expander{[Conditional("EXP")]public static void Expand(string inputFilePath=null,string outputFilePath=null,bool ignoreAnyError=true){}public static string ExpandString(string inputFilePath=null,bool ignoreAnyError=true){return "";}}}
#endregion Expanded by https://github.com/naminodarie/SourceExpander
