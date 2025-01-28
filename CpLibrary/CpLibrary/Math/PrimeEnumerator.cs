using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Math
{
	public static class PrimeEnumerator
	{
		public static bool[] EnumeratePrime(int max, List<int> prime = null)
		{
			var ret = new bool[max + 1];
			for (int i = 2; i < max + 1; i++)
			{
				ret[i] = true;
			}

			for (int i = 2; i * i <= max; i++)
			{
				if (!ret[i]) continue;
				for (int j = i * i; j <= max; j += i)
				{
					ret[j] = false;
				}
			}

			if (prime != null)
			{
				for (int i = 2; i < max + 1; i++)
				{
					if (ret[i]) prime.Add(i);
				}
			}

			return ret;
		}
	}
}
