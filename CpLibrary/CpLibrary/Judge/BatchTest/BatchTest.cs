using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CpLibrary.Judge.Checker;

namespace CpLibrary.Judge
{
	public static class BatchTester
	{
		public static TotalJudgeResult BatchTest(CheckerBase checker, string path)
		{
			var path_in = "in";
			var path_out = "out";
			var ext_in = ".in";
			var ext_out = ".out";
			var testcases = new List<(Stream, Stream)>();
			var input_text = Directory.GetFiles(Path.Combine(path, path_in))
				.Select(p => Path.GetFileNameWithoutExtension(p));
			var output_text = Directory.GetFiles(Path.Combine(path, path_out))
				.Select(p => Path.GetFileNameWithoutExtension(p));

			foreach (var testcase in input_text.Intersect(output_text))
			{
				var input_path = Path.ChangeExtension(Path.Combine(path, path_in, testcase), ext_in);
				var output_path = Path.ChangeExtension(Path.Combine(path, path_out, testcase), ext_out);
				var f_in = File.Open(input_path, FileMode.Open, FileAccess.Read);
				var f_out = File.Open(output_path, FileMode.Open, FileAccess.Read);
				testcases.Add((f_in, f_out));
			}

			var result = checker.Run(testcases);

			foreach (var (inStream, outStream) in testcases)
			{
				inStream.Close();
				outStream.Close();
			}

			return result;
		}
	}
}
