using CpLibrary.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Collections;

public class ImplicitTreap<T> where T : unmanaged
{
	NodePool<Node> nodes;
	Xoshiro256StarStar rand;

	public ImplicitTreap(int size = 1 << 8)
	{
		rand = new();
		nodes = new(size);
		ref var node = ref nodes.AllocSlot(out _);
		node = new Node(default(T), 0, 0, 0, 0);
	}

	void Join(int l, int r)
	{

	}

	(int l, int r) Split(int t, int k)
	{
		if (nodes[t].l == 0 && nodes[t].r == 0) return (0, 0);

	}

	struct Node
	{
		public T value;
		public int l, r;
		public int lc;
		public ulong priority;

		public Node(T value)
		{
			this.value = value;
		}

		public Node(T value, int l, int r, int lc, ulong priority)
		{
			this.value = value;
			this.l = l;
			this.r = r;
			this.lc = lc;
			this.priority = priority;
		}
	}
}
