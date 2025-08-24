using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Graph;

public class UnionFind
{
	public int Count { get; private set; }
	int[] parent;
	int[] rank;
	int[] size;

	public UnionFind(int n)
	{
		parent = Enumerable.Range(0, n).ToArray();
		rank = Enumerable.Repeat(0, n).ToArray();
		size = Enumerable.Repeat(1, n).ToArray();
	}

	public void Unite(int x, int y)
	{
		x = Find(x);
		y = Find(y);
		if (x == y) return;
		if (rank[x] > rank[y])
		{
			parent[y] = x;
			size[x] += size[y];
		}
		else
		{
			parent[x] = y;
			if (rank[x] == rank[y]) rank[x]++;
			size[y] += size[x];
		}
	}

	public int Find(int x)
	{
		if (parent[x] == x) return x;
		else return parent[x] = Find(parent[x]);
	}

	public bool IsSame(int x, int y) => Find(x) == Find(y);

	public int Size(int x) => size[Find(x)];
}
