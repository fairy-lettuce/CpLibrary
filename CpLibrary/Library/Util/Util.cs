using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace CpLibrary
{
	public static partial class Util
	{
		static int[] dx4 => new int[] { 1, 0, -1, 0 };
		static int[] dy4 => new int[] { 0, 1, 0, -1 };
		static int[] dx8 => new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
		static int[] dy8 => new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
	}
}
