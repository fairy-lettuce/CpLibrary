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

	public ImplicitTreap(int size = 1 << 10)
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

	public ImplicitTreap(IList<T> a) : this(a.Count + 1)
	{
		root = Build(a, 0, a.Count);
	}

	public T this[int index]
	{
		get => nodes[Find(root, index)].value;
		set => Set(root, index, value);
	}

	public int Build(IList<T> a, int l, int r)
	{
		if (l >= r) return 0;
		var m = (l + r) >> 1;
		ref var n = ref nodes.AllocSlot(out var p);
		n.value = a[m];
		n.prod = a[m];
		n.lazy = op.FIdentity;
		n.c = 1;
		n.priority = NextPriority();
		n.l = Build(a, l, m);
		n.r = Build(a, m + 1, r);
		// heapify
		if (n.priority < nodes[n.l].priority) (n.priority, nodes[n.l].priority) = (nodes[n.l].priority, n.priority);
		if (n.priority < nodes[n.r].priority) (n.priority, nodes[n.r].priority) = (nodes[n.r].priority, n.priority);
		nodes[p] = n;
		Update(p);
		return p;
	}

	public int Count => nodes[root].c;

	public void Add(int index, T value) => root = Insert(root, index, value, NextPriority());

	public void Remove(int index) => root = Erase(root, index);

	// note: operation requires commutativity
	public T Prod(int l, int r)
	{
		var (lx, right) = Split(root, r);
		var (left, mid) = Split(lx, l);
		var prod = nodes[mid].prod;
		var t = Join(left, mid);
		root = Join(t, right);
		return prod;
	}

	public void Apply(int l, int r, F f)
	{
		var (lx, right) = Split(root, r);
		var (left, mid) = Split(lx, l);
		if (mid != 0) Apply(mid, f);
		var t = Join(left, mid);
		root = Join(t, right);
	}

	public void Reverse(int l, int r)
	{
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

	int Find(int t, int k)
	{
		if (t == 0) return 0;
		Propagate(t);
		ref var n = ref nodes[t];
		if (nodes[n.l].c > k)
		{
			var ret = Find(n.l, k);
			Update(t);
			return ret;
		}
		else if (nodes[n.l].c > k)
		{
			var ret = Find(n.r, k - nodes[n.l].c - 1);
			Update(t);
			return ret;
		}
		return t;
	}

	void Set(int t, int k, T value)
	{
		if (t == 0) return;
		Propagate(t);
		ref var n = ref nodes[t];
		if (nodes[n.l].c > k)
		{
			var ret = Find(n.l, k);
			Update(t);
			return;
		}
		else if (nodes[n.l].c > k)
		{
			var ret = Find(n.r, k - nodes[n.l].c - 1);
			Update(t);
			return;
		}
		n.value = value;
		Update(t);
	}

	int Join(int l, int r)
	{
		if (l == 0) return r;
		if (r == 0) return l;
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
		return (l, r);
	}

	int Insert(int t, int k, T value, ulong priority)
	{
		ref var node = ref nodes.AllocSlot(out var idx);
		node.value = value;
		node.prod = value;
		node.lazy = op.FIdentity;
		node.l = 0;
		node.r = 0;
		node.c = 1;
		node.priority = priority;
		node.rev = false;
		var (l, r) = Split(root, k);
		return Join(Join(l, idx), r);
	}

	int Insert2(int t, int k, T value, ulong priority)
	{
		if (t == 0)
		{
			ref var node = ref nodes.AllocSlot(out var idx);
			node.value = value;
			node.prod = value;
			node.lazy = op.FIdentity;
			node.l = 0;
			node.r = 0;
			node.c = 1;
			node.priority = priority;
			node.rev = false;
			return idx;
		}
		Propagate(t);
		ref var n = ref nodes[t];
		var lc = nodes[n.l].c;
		if (n.priority >= priority)
		{
			if (k <= lc)
			{
				var ret = Insert(n.l, k, value, priority);
				nodes[t].l = ret;
				Update(t);
				return t;
			}
			else
			{
				var ret = Insert(n.r, k - lc - 1, value, priority);
				nodes[t].r = ret;
				Update(t);
				return t;
			}
		}
		{
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
	}

	int Erase(int t, int k)
	{
		var (l, x) = Split(t, k);
		var (mid, r) = Split(x, 1);
		nodes.Free(mid);
		return Join(l, r);
	}

	int Erase2(int t, int k)
	{
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
		if (!EqualityComparer<F>.Default.Equals(n.lazy, op.FIdentity))
		{
			if (n.l != 0) Apply(n.l, n.lazy);
			if (n.r != 0) Apply(n.r, n.lazy);
			n.lazy = op.FIdentity;
		}
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
		n.prod = op.Operate(op.Operate(nodes[n.l].prod, n.value), nodes[n.r].prod);
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

		public Node(T value, ulong priority) : this(value, value, op.FIdentity, 0, 0, 1, priority, false) { }

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
