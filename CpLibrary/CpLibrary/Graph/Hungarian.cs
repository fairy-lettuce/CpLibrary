using System;

namespace CpLibrary.Graph;

public static class Hungarian
{
	/// <summary>
	/// An implementation of the Hungarian algorithm for solving the assignment problem i.e. the minimum-weight bipartite matching problem.
	/// Reference: https://cp-algorithms.com/graph/hungarian-algorithm.html
	/// </summary>
	/// <param name="a">The cost matrix (n x m, n <= m).</param>
	/// <returns>The minimum total cost and the assignment array (i-th element is assigned to `match[i]`).</returns>
	public static (long cost, int[] match) Solve(long[][] a)
	{
		int n = a.Length;
		if (n == 0) return (0, Array.Empty<int>());
		int m = a[0].Length;
		if (n > m) throw new ArgumentException("rows must be at most columns");

		const long INF = long.MaxValue / 3;

		var u = new long[n + 1];
		var v = new long[m + 1];
		var p = new int[m + 1];
		var way = new int[m + 1];
		var minv = new long[m + 1];
		var used = new bool[m + 1];
		for (int i = 1; i <= n; i++)
		{
			p[0] = i;
			int j0 = 0;
			Array.Fill(minv, INF);
			Array.Fill(used, false);
			do
			{
				used[j0] = true;
				int i0 = p[j0], j1 = 0;
				long delta = INF;
				var row = a[i0 - 1];
				long ui = u[i0];
				for (int j = 1; j <= m; j++)
				{
					if (used[j]) continue;
					long cur = row[j - 1] - ui - v[j];
					if (cur < minv[j]) { minv[j] = cur; way[j] = j0; }
					if (minv[j] < delta) { delta = minv[j]; j1 = j; }
				}
				for (int j = 0; j <= m; j++)
				{
					if (used[j]) { u[p[j]] += delta; v[j] -= delta; }
					else minv[j] -= delta;
				}
				j0 = j1;
			} while (p[j0] != 0);
			do
			{
				int j1 = way[j0];
				p[j0] = p[j1];
				j0 = j1;
			} while (j0 != 0);
		}
		var match = new int[n];
		for (int j = 1; j <= m; j++)
		{
			if (p[j] != 0) match[p[j] - 1] = j - 1;
		}
		return (-v[0], match);
	}
}
