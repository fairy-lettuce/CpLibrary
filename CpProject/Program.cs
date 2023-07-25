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
		const string inputPath = "input.txt";
		const string outputPath = "output.txt";

		private static StreamReader standardStreamReader;
		private static StreamWriter standardStreamWriter;

		static void Main(string[] args)
		{
			if (args.Length >= 1 && args[0] == "expand")
			{
				for (char c = 'A'; c <= 'J'; c++)
				{
					SourceExpander.Expander.Expand(
						inputFilePath: $"Contest/{c}/{c}.cs",
						outputFilePath: $"Contest/{c}/{c}_combined.csx"
					);
				}
				return;
			}

			standardStreamReader = new StreamReader(Console.OpenStandardInput());
			standardStreamWriter = new StreamWriter(Console.OpenStandardOutput());

			while (true)
			{
				standardStreamWriter.Write(">> ");
				standardStreamWriter.Flush();

				var res = standardStreamReader.ReadLine().Trim();

				var regex = new Regex(@"\s\s+");
				var arg = regex.Replace(res, " ").Split(' ');

				Parser.Default.ParseArguments<RunCommand, DlCommand, ExitCommand>(arg)
					.WithParsed<RunCommand>(opt => Run(opt))
					.WithParsed<DlCommand>(opt => Download(opt))
					.WithParsed<ExitCommand>(opt => Exit(opt))
					.WithNotParsed(err => standardStreamWriter.WriteLine("Failed to parse arguments."));
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
			if (program < 0) throw new ArgumentOutOfRangeException();
			if (program >= 10) throw new ArgumentOutOfRangeException();

			if (opt.DumpResult) sw = new StreamWriter(outputPath);
			else sw = standardStreamWriter;

			if (opt.IsInputConsole) sr = standardStreamReader;
			else sr = new StreamReader(inputPath);

			if (opt.IsSampleTestcases) throw new NotImplementedException();

			solver = (SolverBase)Activator.CreateInstance(solvers[program]);

			var startTime = DateTime.Now;

			solver.Run(sr, sw);

			var endTime = DateTime.Now;

			sw.Flush();

			if (opt.DumpResult) sw.Close();
			if (!opt.IsInputConsole) sr.Close();

			standardStreamWriter.WriteLine("Program finished.");
			standardStreamWriter.WriteLine($"Time: {(endTime - startTime).ToString(@"ss\.ffffff")} sec.");
			standardStreamWriter.Flush();
		}

		static void Download(DlCommand opt) => throw new NotImplementedException();

		static void Exit(ExitCommand opt)
		{
			standardStreamReader.Close();
			standardStreamWriter.Close();
			Environment.Exit(0);
		}
	}

	[Verb("run", HelpText = "Run the program.")]
	class RunCommand
	{
		[Value(0, Required = true)]
		public char Program { get; set; }

		[Option('d', "dump", Required = false, HelpText = "Outputs the result on output.txt if enabled.")]
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
