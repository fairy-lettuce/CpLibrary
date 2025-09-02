using CpLibrary.Util;
using System.Numerics;

namespace CpLibrary.Collections;

// TODO: REFACTOR THIS!

public class ImplicitTreap<T, F, TOp>
	where T : unmanaged
	where F : unmanaged
	where TOp : AtCoder.ILazySegtreeOperator<T, F>
{
	static readonly TOp op = default(TOp);
	NodePool<Node> nodes;
	int root;
	static readonly Xoshiro256StarStar rand = new();

	public ImplicitTreap(int size = 1 << 1)
	{
		nodes = new(size);
		ref var node = ref nodes.AllocSlot(out _);
		node.value = op.Identity;
		node.prod = op.Identity;
		node.lazy = op.FIdentity;
		node.l = 0;
		node.r = 0;
		node.c = 0;
		node.priority = 0;
		node.rev = false;
		root = 0;
	}

	public void Build(IList<T> list)
	{
		throw new NotImplementedException();
	}

	public int Count => nodes[root].c;

	public void Add(int index, T value) => root = Insert(root, index, value, NextPriority());

	public void Remove(int index) => root = Erase(root, index);


	public T Prod(int l, int r)
	{
		// split twice
		// get fold value
		// merge twice
		var (lx, right) = Split(root, r);
		var (left, mid) = Split(lx, l);
		var prod = nodes[mid].prod;
		var t = Join(left, mid);
		root = Join(t, right);
		return prod;
	}

	public void Apply(int l, int r, F f)
	{
		// split twice
		// apply
		// merge twice
		var (lx, right) = Split(root, r);
		var (left, mid) = Split(lx, l);
		if (mid != 0) Apply(mid, f);
		var t = Join(left, mid);
		root = Join(t, right);
	}

	public void Reverse(int l, int r)
	{
		// split twice
		// reverse subtree
		// merge twice
		var (lx, right) = Split(root, r);
		var (left, mid) = Split(lx, l);
		Toggle(mid);
		var t = Join(left, mid);
		root = Join(t, right);
	}

	void Toggle(int t)
	{
		// reverse the node in O(1)
		if (t == 0) return;
		nodes[t].rev ^= true;

	}

	T Find(int k)
	{
		throw new NotImplementedException();
	}

	int Join(int l, int r)
	{
		// TODO: impl Join
		// algorithm:
		// take the node with higher priority and unlink the edge for left/right child
		// recursively join the rest left/right
		// connect the resulting subtree as the new child
		if (l == 0)
		{
			return r;
		}
		if (r == 0)
		{
			return l;
		}
		Propagate(l);
		Propagate(r);
		ref var ln = ref nodes[l];
		ref var rn = ref nodes[r];
		if (ln.priority > rn.priority)
		{
			ln.r = Join(ln.r, r);
			Update(l);
			return l;
		}
		else
		{
			rn.l = Join(l, rn.l);
			Update(r);
			return r;
		}
	}

	(int l, int r) Split(int t, int k)
	{
		if (t == 0) return (0, 0);
		var l = 0;
		var r = 0;
		Propagate(t);
		ref var n = ref nodes[t];
		if (k > nodes[n.l].c)
		{
			(l, r) = Split(n.r, k - nodes[n.l].c - 1);
			n.r = l;
			l = t;
			Update(t);
		}
		else
		{
			(l, r) = Split(n.l, k);
			n.l = r;
			r = t;
			Update(t);
		}
		//Update(l);
		//Update(r);
		return (l, r);
	}

	int Insert(int t, int k, T value, ulong priority)
	{
		// algorithm:
		// go down the tree until priority
		// split subtree into L, R
		// make a new node with children L, R
		if (t == 0)
		{
			ref var n = ref nodes.AllocSlot(out var idx);
			n.value = value;
			n.prod = value;
			n.lazy = op.FIdentity;
			n.l = 0;
			n.r = 0;
			n.c = 1;
			n.priority = priority;
			n.rev = false;
			return idx;
		}
		Propagate(t);
		if (nodes[t].priority >= priority)
		{
			if (k <= nodes[nodes[t].l].c)
			{
				var ret = Insert(nodes[t].l, k, value, priority);
				nodes[t].l = ret;
				Update(t);
				return t;
			}
			else
			{
				var ret = Insert(nodes[t].r, k - nodes[nodes[t].l].c - 1, value, priority);
				nodes[t].r = ret;
				Update(t);
				return t;
			}
		}
		var (l, r) = Split(t, k);
		ref var node = ref nodes.AllocSlot(out var index);
		node.value = value;
		node.prod = value;
		node.lazy = op.FIdentity;
		node.l = l;
		node.r = r;
		node.priority = priority;
		node.rev = false;
		Update(index);
		return index;
	}

	int Erase(int t, int k)
	{
		// algorithm:
		// go down the tree until key
		// remove the node
		// merge the left/right children
		if (t == 0) return 0;
		Propagate(t);
		if (k < nodes[nodes[t].l].c)
		{
			var ret = Erase(nodes[t].l, k);
			nodes[t].l = ret;
			Update(t);
			return t;
		}
		else if (k > nodes[nodes[t].l].c)
		{
			var ret = Erase(nodes[t].r, k - nodes[nodes[t].l].c - 1);
			nodes[t].r = ret;
			Update(t);
			return t;
		}
		else
		{
			var res = Join(nodes[t].l, nodes[t].r);
			nodes.Free(t);
			t = res;
			// Update(t);
			return t;
		}
	}

	void Walk(int p, T[] a, ref int k)
	{
		if (p == 0) return;
		Propagate(p);
		Walk(nodes[p].l, a, ref k);
		a[k++] = nodes[p].value;
		Walk(nodes[p].r, a, ref k);
	}

	public T[] Enumerate()
	{
		var len = nodes[root].c;
		var a = new T[len];
		var idx = 0;
		Walk(root, a, ref idx);
		return a;
	}

	public override string ToString()
	{
		return Enumerate().Join(", ");
	}

	public void Propagate(int t)
	{
		if (t == 0) return;
		ref var n = ref nodes[t];
		if (n.rev)
		{
			(n.l, n.r) = (n.r, n.l);
			Toggle(n.l);
			Toggle(n.r);
			n.rev = false;
		}
		if (n.l != 0) Apply(n.l, n.lazy);
		if (n.r != 0) Apply(n.r, n.lazy);
		n.lazy = op.FIdentity;
	}

	void Apply(int t, F f)
	{
		nodes[t].lazy = op.Composition(f, nodes[t].lazy);
		nodes[t].value = op.Mapping(f, nodes[t].value);
		nodes[t].prod = op.Mapping(f, nodes[t].prod);
	}

	public void Update(int t)
	{
		if (t == 0) return;
		ref var n = ref nodes[t];
		n.c = nodes[n.l].c + nodes[n.r].c + 1;
		var prod = n.value;
		if (n.l != 0)
		{
			prod = op.Operate(nodes[n.l].prod, prod);
		}
		if (n.r != 0)
		{
			prod = op.Operate(prod, nodes[n.r].prod);
		}
		n.prod = prod;
	}

	public int MaxHeight() => MaxHeight(root);

	public int MaxHeight(int t)
	{
		if (t == 0) return 0;
		Propagate(t);
		return Math.Max(MaxHeight(nodes[t].l), MaxHeight(nodes[t].r)) + 1;
	}

	ulong NextPriority() => rand.NextULong();
	//uint NextPriority()
	//{
	//	var x = rand.NextULong();
	//	var x1 = BitOperations.LeadingZeroCount(x & 0xFFFFFFFFFF000000ul);
	//	var x2 = x & 0x0000000000FFFFFFul;
	//	return ((uint)x1 << 24) | (uint)x2;
	//}

	struct Node
	{
		public T value, prod;
		public F lazy;
		public int l, r;
		public int c;
		public ulong priority;
		public bool rev;

		public Node(T value, T prod, F lazy, int l, int r, int c, ulong priority, bool rev)
		{
			this.value = value;
			this.prod = prod;
			this.lazy = op.FIdentity;
			this.l = l;
			this.r = r;
			this.c = c;
			this.priority = priority;
			this.rev = false;
		}

		public override string ToString()
		{
			return $"Value: {value}, Count: {c}, Priority: {priority}";
		}
	}
}
