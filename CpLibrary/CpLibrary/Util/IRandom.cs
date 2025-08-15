using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Util;

public interface IRandom
{
	public int Next();
	public int Next(int b);
	public int Next(int a, int b);
	public void NextBytes(byte[] b);
	public void NextBytes(Span<byte> b);
	public double NextDouble();
	public long NextLong();
	public long NextLong(long b);
	public long NextLong(long a, long b);
}
