using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CpLibrary.Collections.Internal;

public class SetIndexed<T> : IEnumerable<T> where T : unmanaged, IComparable<T>
{
	int root;
	readonly IComparer<T> comparer;
	readonly bool isMultiSet;
	NodePool<Node> nodes;

	public SetIndexed(bool isMultiSet = false) : this(Comparer<T>.Default, isMultiSet) { }
	public SetIndexed(IComparer<T> comparer, bool isMultiSet = false)
	{
		this.comparer = comparer;
		this.isMultiSet = isMultiSet;
		nodes = new();
		ref var node = ref nodes.AllocSlot(out _);
		node = new(default(T), 0, 0, 0, 0);
		root = 0;
	}
	public SetIndexed(IList<T> list, bool isMultiSet = false) : this(list, Comparer<T>.Default, isMultiSet) { }
	public SetIndexed(IList<T> list, IComparer<T> comparer, bool isMultiSet = false) : this(comparer, isMultiSet)
	{
		nodes = new NodePool<Node>(list.Count + 1);
		ref var node = ref nodes.AllocSlot(out _);
		node = new(default(T), 0, 0, 0, 0);
		root = Build(list, 0, list.Count);
	}
	public SetIndexed(Comparison<T> comparison, bool isMultiSet = false) : this(Comparer<T>.Create(comparison), isMultiSet) { }
	public SetIndexed(IList<T> list, Comparison<T> comparison, bool isMultiSet = false) : this(list, Comparer<T>.Create(comparison), isMultiSet) { }

	int Build(IList<T> a, int l, int r)
	{
		if (l >= r) return 0;
		var m = (l + r) >> 1;
		ref var n = ref nodes.AllocSlot(out var p);
		n.Value = a[m];
		n.Count = 1;
		n.Height = 1;
		n.Left = Build(a, l, m);
		n.Right = Build(a, m + 1, r);
		nodes[p] = n;
		Update(p);
		return p;
	}

	public T this[int index]
	{
		get
		{
			Debug.Assert((uint)Count > (uint)index);
			return Find(root, index);
		}
	}

	public bool Add(T x) => Insert(ref root, x);
	public bool Remove(T x) => Erase(ref root, x);
	public void RemoveAt(int index)
	{
		Debug.Assert((uint)Count > (uint)index);
		EraseAt(ref root, index);
	}

	public bool Contains(T x)
	{
		if (this.Count == 0) return false;
		var (idx, val) = LowerBound(x);
		if (idx >= this.Count) return false;
		return val.CompareTo(x) == 0;
	}

	public (int Index, T Value) LowerBound(T x) => FindFirstGreaterOrEqual(x);
	public (int Index, T Value) UpperBound(T x) => FindFirstGreaterThan(x);
	public (int Index, T Value) FindFirstGreaterOrEqual(T x) => BinarySearch(root, x, false, true);
	public (int Index, T Value) FindFirstGreaterThan(T x) => BinarySearch(root, x, true, true);
	public (int Index, T Value) FindLastLessOrEqual(T x) => BinarySearch(root, x, true, false);
	public (int Index, T Value) FindLastLessThan(T x) => BinarySearch(root, x, false, false);

	public int EqualRange(T x) => FindFirstGreaterOrEqual(x).Index - FindFirstGreaterThan(x).Index;

	public void Clear()
	{
		root = 0;
	}

	public T Min() => nodes[MinIndex(root)].Value;
	public T Max() => nodes[MaxIndex(root)].Value;

	public T[] Items
	{
		get
		{
			var a = new T[Count];
			var k = 0;
			Walk(root, a, ref k);
			return a;
		}
	}
	public int Count => nodes[root].Count;
	public int Height => nodes[root].Height;

	public IEnumerator<T> GetEnumerator() => Items.AsEnumerable().GetEnumerator();
	System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

	protected void Update(int p)
	{
		if (p == 0) return;
		var n = nodes[p];
		n.Count = nodes[n.Left].Count + nodes[n.Right].Count + 1;
		n.Height = Math.Max(nodes[n.Left].Height, nodes[n.Right].Height) + 1;
		nodes[p] = n;
	}

	protected bool Insert(ref int p, T x)
	{
		if (p == 0)
		{
			ref var node = ref nodes.AllocSlot(out var k);
			node.Value = x;
			node.Left = 0;
			node.Right = 0;
			node.Count = 1;
			node.Height = 1;
			p = k;
			return true;
		}
		ref var n = ref nodes[p];
		bool ret;
		int t = comparer.Compare(n.Value, x);
		if (t > 0)
		{
			ret = Insert(ref n.Left, x);
		}
		else if (t < 0)
		{
			ret = Insert(ref n.Right, x);
		}
		else
		{
			if (isMultiSet) ret = Insert(ref n.Left, x);
			else return false;
		}
		nodes[p] = n;
		Balance(ref p);
		return ret;
	}

	protected bool Erase(ref int p, T x)
	{
		if (p == 0) return false;
		var n = nodes[p];
		int t = comparer.Compare(n.Value, x);
		bool ret;
		if (t < 0) ret = Erase(ref n.Right, x);
		else if (t > 0) ret = Erase(ref n.Left, x);
		else
		{
			ret = true;
			if (nodes[n.Right].Count == 0) { nodes.Free(p); p = n.Left; return true; }
			if (nodes[n.Left].Count == 0) { nodes.Free(p); p = n.Right; return true; }

			int mx = MaxIndex(n.Left);
			n.Value = nodes[mx].Value;
			nodes[p] = n;
			ret = Erase(ref n.Left, n.Value);
			nodes[p] = n;
		}
		nodes[p] = n;
		Balance(ref p);
		return ret;
	}

	protected void EraseAt(ref int p, int index)
	{
		var n = nodes[p];
		int leftCount = nodes[n.Left].Count;
		if (index < leftCount) { EraseAt(ref n.Left, index); nodes.Free(p); nodes[p] = n; Balance(ref p); return; }
		if (index > leftCount) { EraseAt(ref n.Right, index - leftCount - 1); nodes.Free(p); nodes[p] = n; Balance(ref p); return; }

		if (nodes[n.Left].Count == 0) { p = n.Right; return; }
		if (nodes[n.Right].Count == 0) { p = n.Left; return; }

		int mx = MaxIndex(n.Left);
		n.Value = nodes[mx].Value;
		nodes[p] = n;
		EraseAt(ref n.Left, leftCount - 1);
		nodes[p] = n;
		Balance(ref p);
	}

	protected void Balance(ref int p)
	{
		if (p == 0) return;
		var n = nodes[p];
		int balance = nodes[n.Left].Height - nodes[n.Right].Height;
		if (balance < -1)
		{
			var r = nodes[n.Right];
			if (nodes[r.Left].Height - nodes[r.Right].Height > 0) RotateR(ref n.Right);
			nodes[p] = n; RotateL(ref p);
		}
		else if (balance > 1)
		{
			var l = nodes[n.Left];
			if (nodes[l.Left].Height - nodes[l.Right].Height < 0) RotateL(ref n.Left);
			nodes[p] = n; RotateR(ref p);
		}
		else { Update(p); }
	}

	protected T Find(int p, int index)
	{
		var n = nodes[p];
		int lc = nodes[n.Left].Count;
		if (index < lc) return Find(n.Left, index);
		if (index > lc) return Find(n.Right, index - lc - 1);
		return n.Value;
	}

	protected void RotateL(ref int p)
	{
		int r = nodes[p].Right;
		var np = nodes[p];
		var nr = nodes[r];
		np.Right = nr.Left;
		nodes[p] = np;
		nr.Left = p;
		nodes[r] = nr;
		Update(p);
		Update(r);
		p = r;
	}

	protected void RotateR(ref int p)
	{
		int l = nodes[p].Left;
		var np = nodes[p];
		var nl = nodes[l];
		np.Left = nl.Right;
		nodes[p] = np;
		nl.Right = p;
		nodes[l] = nl;
		Update(p);
		Update(l);
		p = l;
	}

	protected (int Index, T Value) BinarySearch(int p, T x, bool isUpperBound, bool returnHigh)
	{
		int ret = 0, high = 0, low = 0;
		int cur = p;
		while (cur != 0)
		{
			ref var n = ref nodes[cur];
			int cmp = comparer.Compare(n.Value, x);
			if (cmp > 0 || (!isUpperBound && cmp == 0))
			{
				high = cur;
				cur = n.Left;
			}
			else
			{
				low = cur;
				ret += nodes[n.Left].Count + 1;
				cur = n.Right;
			}
		}
		return (ret, returnHigh ? nodes[high].Value : nodes[low].Value);
	}

	protected void Walk(int p, T[] a, ref int k)
	{
		if (p == 0) return;
		Walk(nodes[p].Left, a, ref k);
		a[k++] = nodes[p].Value;
		Walk(nodes[p].Right, a, ref k);
	}

	protected int MinIndex(int p)
	{
		while (nodes[p].Left != 0) p = nodes[p].Left;
		return p;
	}
	protected int MaxIndex(int p)
	{
		while (nodes[p].Right != 0) p = nodes[p].Right;
		return p;
	}

	public override string ToString() => string.Join(", ", Items);

	protected struct Node
	{
		public int Left, Right;
		public T Value;
		public int Count, Height;
		public Node()
		{
			Left = Right = Count = Height = 0;
			Value = default(T);
		}

		public Node(T value) => this.Value = value;

		public Node(T value, int l, int r, int c, int h) : this(value)
		{
			Left = l; Right = r; Count = c; Height = h;
		}

		public override string ToString() => $"Count = {Count}, Value = {Value}";
	}
}
