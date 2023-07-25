using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace CpLibrary
{
	public abstract class SolverBase
	{
		public bool StartsOnThread { get; set; } = false;
		public int Testcases { get; set; } = 1;
		public Scanner scanner;
		public StreamWriter writer;

		public abstract void Init();
		public abstract void Solve();

		public void Run(StreamReader reader, StreamWriter writer)
		{
			this.writer = writer;
			scanner = new Scanner(reader);
			Console.SetOut(writer);
			if (StartsOnThread)
			{
				var thread = new Thread(new ThreadStart(RunInternal), 1 << 27);
				thread.Start();
				thread.Join();
			}
			else
			{
				RunInternal();
			}
		}

		void RunInternal()
		{
			Init();
			var testcases = Testcases;
			while (testcases-- > 0)
			{
				Solve();
			}
		}
	}

	public static partial class OnlineJudge
	{
		public static void Run(SolverBase solver)
		{
			var sw = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false };
			var sr = new StreamReader(Console.OpenStandardInput());
			solver.Run(sr, sw);
			Console.Out.Flush();
		}
	}
}
