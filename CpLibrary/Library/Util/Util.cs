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
		public static int[] dx4 => new int[] { 1, 0, -1, 0 };
		public static int[] dy4 => new int[] { 0, 1, 0, -1 };
		public static int[] dx8 => new int[] { 1, 1, 0, -1, -1, -1, 0, 1 };
		public static int[] dy8 => new int[] { 0, 1, 1, 1, 0, -1, -1, -1 };
	}
}
