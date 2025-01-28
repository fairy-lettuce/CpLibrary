using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Collections
{
	public class Set<T> : IEnumerable<T> where T : IComparable<T>
	{
		Node root;
		readonly IComparer<T> comparer;
		readonly Node nil;
		public bool IsMultiSet { get; }

		public Set(bool isMultiSet = false) : this(Comparer<T>.Default, isMultiSet) { }
		public Set(IComparer<T> comparer, bool isMultiSet = false)
		{
			nil = new Node(default);
			root = nil;
			this.comparer = comparer;
			this.IsMultiSet = isMultiSet;
		}
		public Set(IEnumerable<T> list, bool isMultiSet = false) : this(list, Comparer<T>.Default, isMultiSet) { }
		public Set(IEnumerable<T> list, IComparer<T> comparer, bool isMultiSet = false) : this(comparer, isMultiSet)
		{
			foreach (var item in list)
			{
				Add(item);
			}
		}
		public Set(Comparison<T> comparison, bool isMultiSet = false) : this(Comparer<T>.Create(comparison), isMultiSet) { }
		public Set(IEnumerable<T> list, Comparison<T> comparison, bool isMultiSet = false) : this(list, Comparer<T>.Create(comparison), isMultiSet) { }

		public T this[int index]
		{
			get
			{
				if (index < 0 || root.Count < index) throw new ArgumentOutOfRangeException();
				return Find(root, index);
			}
		}

		public bool Add(T x) => Insert(ref root, x);
		public bool Remove(T x) => Erase(ref root, x);
		public void RemoveAt(int index)
		{
			if (index < 0 || Count < index) throw new ArgumentOutOfRangeException();
			EraseAt(ref root, index);
		}
		public bool Contains(T x) => this.Count == 0 ? false : EqualRange(x) > 0;
		public int LowerBound(T x) => BinarySearch(root, x, false);
		public int UpperBound(T x) => BinarySearch(root, x, true);
		public int EqualRange(T x) => UpperBound(x) - LowerBound(x);

		public void Clear() => root = nil;

		public T Min() => root.Min.Value;
		public T Max() => root.Max.Value;

		public T[] Items
		{
			get
			{
				var a = new T[root.Count];
				var k = 0;
				Walk(root, a, ref k);
				return a;
			}
		}
		public int Count => root.Count;
		public int Height => root.Height;

		public IEnumerator<T> GetEnumerator() { return Items.AsEnumerable().GetEnumerator(); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		bool Insert(ref Node p, T x)
		{
			if (p.Count == 0)
			{
				p = new Node(x);
				p.Left = p.Right = nil;
				p.Update();
				return true;
			}
			bool ret;
			var t = comparer.Compare(p.Value, x);
			if (t > 0)
			{
				ret = Insert(ref p.Left, x);
			}
			else if (t < 0)
			{
				ret = Insert(ref p.Right, x);
			}
			else
			{
				if (IsMultiSet == true) ret = Insert(ref p.Left, x);
				else return false;
			}
			Balance(ref p);
			return ret;
		}

		bool Erase(ref Node p, T x)
		{
			if (p.Count == 0) return false;
			var t = comparer.Compare(p.Value, x);
			bool ret;
			if (t < 0) ret = Erase(ref p.Right, x);
			else if (t > 0) ret = Erase(ref p.Left, x);
			else
			{
				ret = true;
				if (p.Right.Count == 0) { p = p.Left; return true; }
				if (p.Left.Count == 0) { p = p.Right; return true; }

				p.Value = p.Left.Max.Value;
				Erase(ref p.Left, p.Left.Max.Value);
			}
			Balance(ref p);

			return ret;
		}

		void EraseAt(ref Node p, int index)
		{
			var count = p.Left.Count;
			if (index < count) EraseAt(ref p.Left, index);
			else if (index > count) EraseAt(ref p.Right, index - count - 1);
			else
			{
				if (p.Left.Count == 0) { p = p.Right; return; }
				if (p.Right.Count == 0) { p = p.Left; return; }

				p.Value = p.Left.Max.Value;
				EraseAt(ref p.Left, index - 1);
			}

			Balance(ref p);
		}

		void Walk(Node t, T[] a, ref int k)
		{
			if (t.Count == 0) return;
			Walk(t.Left, a, ref k);
			a[k++] = t.Value;
			Walk(t.Right, a, ref k);
		}

		void Balance(ref Node p)
		{
			var balance = p.Left.Height - p.Right.Height;
			if (balance < -1)
			{
				if (p.Right.Left.Height - p.Right.Right.Height > 0) RotateR(ref p.Right);
				RotateL(ref p);
			}
			else if (balance > 1)
			{
				if (p.Left.Left.Height - p.Left.Right.Height < 0) RotateL(ref p.Left);
				RotateR(ref p);
			}
			else p.Update();
		}

		T Find(Node p, int index)
		{
			if (index < p.Left.Count) return Find(p.Left, index);
			if (index > p.Left.Count) return Find(p.Right, index - p.Left.Count - 1);
			return p.Value;
		}

		void RotateL(ref Node p)
		{
			var r = p.Right;
			p.Right = r.Left;
			r.Left = p;
			p.Update();
			r.Update();
			p = r;
		}

		void RotateR(ref Node p)
		{
			var l = p.Left;
			p.Left = l.Right;
			l.Right = p;
			p.Update();
			l.Update();
			p = l;
		}

		int BinarySearch(Node p, T x, bool isUpperBound)
		{
			if (p.Count == 0) throw new NullReferenceException();

			var node = p;

			var ret = 0;

			while (p.Count != 0)
			{
				var cmp = p.Value.CompareTo(x);
				if (cmp > 0 || (!isUpperBound && cmp == 0))
				{
					p = p.Left;
				}
				else
				{
					ret += p.Left.Count + 1;
					p = p.Right;
				}
			}

			return ret;
		}

		public override string ToString()
		{
			return string.Join(", ", Items);
		}

		class Node
		{
			public Node Left, Right;
			public T Value { get; set; }

			public int Count { get; private set; }
			public int Height { get; private set; }

			public Node Min
			{
				get
				{
					if (Left.Count == 0) return this;
					else return Left.Min;
				}
			}

			public Node Max
			{
				get
				{
					if (Right.Count == 0) return this;
					else return Right.Max;
				}
			}

			public Node(T value) => Value = value;

			public void Update()
			{
				Count = Left.Count + Right.Count + 1;
				Height = Math.Max(Left.Height, Right.Height) + 1;
			}

			public override string ToString() => $"Count = {Count}, Value = {Value}";
		}
	}

	public interface IProdSetOperator<T>
	{
		public T Identity { get; }
		public T Operate(T x, T y);
	}

	public class ProdSet<T, TOp> : IEnumerable<T> where T : IComparable<T> where TOp : struct, IProdSetOperator<T>
	{
		private static readonly TOp op = default;
		Node root;
		readonly IComparer<T> comparer;
		readonly Node nil;
		public bool IsMultiSet { get; set; }

		public ProdSet() : this(Comparer<T>.Default) { }
		public ProdSet(IComparer<T> comparer)
		{
			nil = new Node(default);
			root = nil;
			this.comparer = comparer;
		}
		public ProdSet(Comparison<T> comparison) : this(Comparer<T>.Create(comparison)) { }

		public T this[int index]
		{
			get
			{
				if (index < 0 || root.Count < index) throw new ArgumentOutOfRangeException();
				return Find(root, index);
			}
		}

		public bool Add(T x) => Insert(ref root, x);
		public bool Remove(T x) => Erase(ref root, x);
		public void RemoveAt(int index)
		{
			if (index < 0 || Count < index) throw new ArgumentOutOfRangeException();
			EraseAt(ref root, index);
		}
		public bool Contains(T x) => this.Count == 0 ? false : EqualRange(x) > 0;
		public int LowerBound(T x) => BinarySearch(root, x, false);
		public int UpperBound(T x) => BinarySearch(root, x, true);
		public int EqualRange(T x) => UpperBound(x) - LowerBound(x);

		public void Clear() => root = nil;

		public T Min() => root.Min.Value;
		public T Max() => root.Max.Value;

		public T Prod(int l, int r)
		{
			if (0 > l || r > Count) throw new ArgumentException($"The range [{l}, {r}) should be within the range [0, {Count}).");
			if (l >= r) return op.Identity;
			return Prod(root, root.Left.Count, 0, Count, l, r);
		}

		private T Prod(Node cur, int x, int a, int b, int l, int r)
		{
			if (cur.Count == 0) return op.Identity;
			if (r <= a || b <= l) return op.Identity;
			if (l <= a && b <= r) return cur.Prod;
			var ret = l <= x && x < r ? cur.Value : op.Identity;
			if (cur.Left.Count > 0) ret = op.Operate(Prod(cur.Left, x - cur.Left.Right.Count - 1, a, x, l, r), ret);
			if (cur.Right.Count > 0) ret = op.Operate(ret, Prod(cur.Right, x + cur.Right.Left.Count + 1, x + 1, b, l, r));
			return ret;
		}

		public T[] Items
		{
			get
			{
				var a = new T[root.Count];
				var k = 0;
				Walk(root, a, ref k);
				return a;
			}
		}
		public int Count => root.Count;
		public int Height => root.Height;

		public IEnumerator<T> GetEnumerator() { return Items.AsEnumerable().GetEnumerator(); }
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		bool Insert(ref Node p, T x)
		{
			if (p.Count == 0)
			{
				p = new Node(x);
				p.Left = p.Right = nil;
				p.Update();
				return true;
			}
			bool ret;
			var t = comparer.Compare(p.Value, x);
			if (t > 0)
			{
				ret = Insert(ref p.Left, x);
			}
			else if (t < 0)
			{
				ret = Insert(ref p.Right, x);
			}
			else
			{
				if (IsMultiSet == true) ret = Insert(ref p.Left, x);
				else return false;
			}
			Balance(ref p);
			return ret;
		}

		bool Erase(ref Node p, T x)
		{
			if (p.Count == 0) return false;
			var t = comparer.Compare(p.Value, x);
			bool ret;
			if (t < 0) ret = Erase(ref p.Right, x);
			else if (t > 0) ret = Erase(ref p.Left, x);
			else
			{
				ret = true;
				if (p.Right.Count == 0) { p = p.Left; return true; }
				if (p.Left.Count == 0) { p = p.Right; return true; }

				p.Value = p.Left.Max.Value;
				Erase(ref p.Left, p.Left.Max.Value);
			}
			Balance(ref p);

			return ret;
		}

		void EraseAt(ref Node p, int index)
		{
			var count = p.Left.Count;
			if (index < count) EraseAt(ref p.Left, index);
			else if (index > count) EraseAt(ref p.Right, index - count - 1);
			else
			{
				if (p.Left.Count == 0) { p = p.Right; return; }
				if (p.Right.Count == 0) { p = p.Left; return; }

				p.Value = p.Left.Max.Value;
				EraseAt(ref p.Left, index - 1);
			}

			Balance(ref p);
		}

		void Walk(Node t, T[] a, ref int k)
		{
			if (t.Count == 0) return;
			Walk(t.Left, a, ref k);
			a[k++] = t.Value;
			Walk(t.Right, a, ref k);
		}

		void Balance(ref Node p)
		{
			var balance = p.Left.Height - p.Right.Height;
			if (balance < -1)
			{
				if (p.Right.Left.Height - p.Right.Right.Height > 0) RotateR(ref p.Right);
				RotateL(ref p);
			}
			else if (balance > 1)
			{
				if (p.Left.Left.Height - p.Left.Right.Height < 0) RotateL(ref p.Left);
				RotateR(ref p);
			}
			else p.Update();
		}

		T Find(Node p, int index)
		{
			if (index < p.Left.Count) return Find(p.Left, index);
			if (index > p.Left.Count) return Find(p.Right, index - p.Left.Count - 1);
			return p.Value;
		}

		void RotateL(ref Node p)
		{
			var r = p.Right;
			p.Right = r.Left;
			r.Left = p;
			p.Update();
			r.Update();
			p = r;
		}

		void RotateR(ref Node p)
		{
			var l = p.Left;
			p.Left = l.Right;
			l.Right = p;
			p.Update();
			l.Update();
			p = l;
		}

		int BinarySearch(Node p, T x, bool isUpperBound)
		{
			if (p.Count == 0) throw new NullReferenceException();

			var node = p;

			var ret = 0;

			while (p.Count != 0)
			{
				var cmp = p.Value.CompareTo(x);
				if (cmp > 0 || (!isUpperBound && cmp == 0))
				{
					p = p.Left;
				}
				else
				{
					ret += p.Left.Count + 1;
					p = p.Right;
				}
			}

			return ret;
		}

		public override string ToString()
		{
			return string.Join(", ", Items);
		}

		class Node
		{
			public Node Left, Right;
			public T Value { get; set; }
			public T Prod { get; set; }

			public int Count { get; private set; }
			public int Height { get; private set; }

			public Node Min
			{
				get
				{
					if (Left.Count == 0) return this;
					else return Left.Min;
				}
			}

			public Node Max
			{
				get
				{
					if (Right.Count == 0) return this;
					else return Right.Max;
				}
			}

			public Node(T value)
			{
				Value = value;
				Prod = value;
			}

			public void Update()
			{
				Count = Left.Count + Right.Count + 1;
				Height = Math.Max(Left.Height, Right.Height) + 1;
				Prod = op.Operate(Value, op.Operate(Left.Prod, Right.Prod));
			}

			public override string ToString() => $"Count = {Count}, Value = {Value}";
		}
	}
}
