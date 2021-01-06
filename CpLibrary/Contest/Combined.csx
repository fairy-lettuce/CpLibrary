using CpLibrary.Library.Collections;
using System;
using System.Diagnostics;
using System.IO;
namespace CpLibrary.Contest
{
	public class SolverA : SolverBase
	{
		Scanner sr;
		StreamWriter sw;
		bool isMultipleTestcases = false;
		bool willExpandSource = true;

		public override void Solve()
		{
			var deque = new Deque<int>();
			deque.PushBack(1);
			deque.PushBack(3);
			deque.PopBack();
		}

		public SolverA(Scanner cin, StreamWriter cout)
		{
			this.sr = cin;
			this.sw = cout;
		}

		public override void Run()
		{
			if (willExpandSource) SourceExpander.Expander.Expand();
			var _t = 1;
			if (isMultipleTestcases) _t = sr.ReadInt();
			while (_t-- > 0) Solve();
		}
	}

	public static class ProgramA
	{
		public static void Main(string[] args)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			Console.SetOut(sw);
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var solver = new SolverA(sr, sw);
			solver.Run();
			Console.Out.Flush();
		}
	}
}
#region Expanded by https://github.com/naminodarie/SourceExpander
namespace SourceExpander{public class Expander{[Conditional("EXP")]public static void Expand(string inputFilePath=null,string outputFilePath=null,bool ignoreAnyError=true){}public static string ExpandString(string inputFilePath=null,bool ignoreAnyError=true){return "";}}}
#endregion Expanded by https://github.com/naminodarie/SourceExpander
