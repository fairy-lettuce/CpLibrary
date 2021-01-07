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
	public interface ISolver
	{
		public void Solve();
		public void Run();
	}

	public abstract class SolverBase : ISolver
	{
		public abstract void Solve();
		public abstract void Run();

		public bool YesNo(bool condition)
		{
			Console.WriteLine(condition ? "Yes" : "No");
			return condition;
		}
		public bool YESNO(bool condition)
		{
			Console.WriteLine(condition ? "YES" : "NO");
			return condition;
		}
		public bool yesno(bool condition)
		{
			Console.WriteLine(condition ? "yes" : "no");
			return condition;
		}
	}

}
