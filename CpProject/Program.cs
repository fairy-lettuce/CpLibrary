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
using System.Text.RegularExpressions;
using CommandLine;
using CpLibrary.Contest;

namespace CpLibrary
{
	public static class Program
	{
		const string workspacePath = @"../../../Workspace/";

		private static StreamReader standardStreamReader;
		private static StreamWriter standardStreamWriter;

		static void Main(string[] args)
		{
			if (args.Length >= 1 && args[0] == "expand")
			{
				ProgramA.Expand();
				ProgramB.Expand();
				ProgramC.Expand();
				ProgramD.Expand();
				ProgramE.Expand();
				ProgramF.Expand();
				ProgramG.Expand();
				ProgramH.Expand();
				ProgramI.Expand();
				ProgramJ.Expand();
				return;
			}

			Directory.SetCurrentDirectory(workspacePath);

			standardStreamReader = new StreamReader(Console.OpenStandardInput());
			standardStreamWriter = new StreamWriter(Console.OpenStandardOutput());

			while (true)
			{
				Console.SetIn(standardStreamReader);
				Console.SetOut(standardStreamWriter);

				Console.Write(">> ");
				Console.Out.Flush();

				var regex = new Regex(@"\s\s+");
				var arg = regex.Replace(Console.ReadLine().Trim(), " ").Split(' ');

				Parser.Default.ParseArguments<RunCommand, DlCommand, ExitCommand>(arg)
					.WithParsed<RunCommand>(opt => Run(opt))
					.WithParsed<DlCommand>(opt => Download(opt))
					.WithParsed<ExitCommand>(opt => Exit(opt))
					.WithNotParsed(err => Console.WriteLine("Failed to parse arguments."));
			}
		}

		static void Run(RunCommand opt)
		{
			StreamReader sr;
			StreamWriter sw;
			var solvers = new Type[]
			{
				typeof(SolverA),
				typeof(SolverB),
				typeof(SolverC),
				typeof(SolverD),
				typeof(SolverE),
				typeof(SolverF),
				typeof(SolverG),
				typeof(SolverH),
				typeof(SolverI),
				typeof(SolverJ)
			};
			SolverBase solver;

			var program = char.ToUpper(opt.Program) - 'A';
			if (program < 0) return;
			if (program >= 10) return;

			if (opt.DumpResult) sw = new StreamWriter("output.txt");
			else sw = new StreamWriter(Console.OpenStandardOutput());

			if (opt.IsInputConsole) sr = new StreamReader(Console.OpenStandardInput());
			else sr = new StreamReader(((char)(program + 'A')).ToString() + "_input.txt");

			if (opt.IsSampleTestcases) throw new NotImplementedException();

			Console.SetIn(sr);
			Console.SetOut(sw);

			var scanner = new Scanner(sr);

			solver = (SolverBase)Activator.CreateInstance(solvers[program], scanner, true);

			var startTime = DateTime.Now;

			solver.Run();

			var endTime = DateTime.Now;

			sr.Close();
			sw.Close();

			sr = new StreamReader(Console.OpenStandardInput());
			sw = new StreamWriter(Console.OpenStandardOutput());
			Console.SetIn(sr);
			Console.SetOut(sw);

			Console.WriteLine("Program finished.");
			Console.WriteLine($"Time: {(endTime - startTime).ToString(@"ss\.ffffff")} sec.");
			Console.Out.Flush();
		}

		static void Download(DlCommand opt) => throw new NotImplementedException();

		static void Exit(ExitCommand opt) => Environment.Exit(0);
	}

	[Verb("run", HelpText = "Run the program.")]
	class RunCommand
	{
		[Value(0, Required = true)]
		public char Program { get; set; }

		[Option('d', "dump", Required = false, HelpText = "Outputs the result on X_output.txt if enabled.")]
		public bool DumpResult { get; set; }

		[Option('c', "console", Required = false, HelpText = "Uses the standard input.")]
		public bool IsInputConsole { get; set; }

		[Option('t', "test", Required = false, HelpText = "Run the program on sample testcases.")]
		public bool IsSampleTestcases { get; set; }
	}

	[Verb("dl", HelpText = "Downloads the sample testcases.")]
	class DlCommand
	{
		[Value(0, Required = true)]
		public char Program { get; set; }
	}

	[Verb("exit", HelpText = "Terminate this program.")]
	class ExitCommand { }
}
