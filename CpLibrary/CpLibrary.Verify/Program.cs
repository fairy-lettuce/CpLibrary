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
using CpLibrary;

namespace CpLibrary.Verify
{
	internal class APlusB : CompetitiveVerifier.ProblemSolver
	{
		public override string Url => "https://judge.yosupo.jp/problem/aplusb";
		public override void Solve()
		{
			var sr = new Scanner(new StreamReader(Console.OpenStandardInput()));
			var a = sr.ReadInt();
			var b = sr.ReadInt();
            Console.WriteLine(a + b);
        }
	}
}
