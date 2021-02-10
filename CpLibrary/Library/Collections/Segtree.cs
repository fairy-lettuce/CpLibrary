using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Diagnostics;

namespace CpLibrary.Collections
{
	public interface ISegtreeOperator<T>
	{
		T Identity { get; }
		T Operate(T x, T y);
	}

	public class Segtree<TValue, TOp> where TOp : ISegtreeOperator<TValue>
	{
		private readonly TOp op;
		public int Count { get; private set; }
		TValue[] tree;

		public Segtree(int size)
		{
			var n = 1;
			while (n < size)
			{
				n <<= 1;
			}
			Count = n;
			tree = Enumerable.Repeat(op.Identity, 2 * n).ToArray();
		}

		public Segtree(TValue[] items) : this(items.Length)
		{
			for (int i = 0; i < items.Length; i++)
			{
				tree[Count + i] = items[i];
			}
			UpdateAll();
		}

		public TValue this[int index] => tree[Count + index];

		public void Update(int index, TValue value)
		{
			var cur = Count + index;
			tree[cur] = value;
			while (cur > 1)
			{
				cur >>= 1;
				tree[cur] = op.Operate(tree[2 * cur], tree[2 * cur + 1]);
			}
		}

		private void UpdateAll()
		{
			for (int i = Count - 1; i >= 1; i--)
			{
				tree[i] = op.Operate(tree[2 * i], tree[2 * i + 1]);
			}
		}

		public TValue ProdAll => tree[1];

		public TValue Prod(int l, int r)
		{
			var retL = op.Identity;
			var retR = op.Identity;
			l += Count;
			r += Count;
			while (l < r)
			{
				if ((l & 1) > 0) retL = op.Operate(retL, tree[l++]);
				if ((r & 1) > 0) retR = op.Operate(tree[--r], retR);
				l >>= 1;
				r >>= 1;
			}
			return op.Operate(retL, retR);
		}

		public int MaxRight(int l, Predicate<TValue> pred)
		{
			if (l == Count) return Count;
			l += Count;
			var prod = op.Identity;
			do
			{
				while ((l & 1) == 0)
				{
					l >>= 1;
				}
				if (pred(op.Operate(prod, tree[l])))
				{
					prod = op.Operate(prod, tree[l]);
					l++;
				}
				else
				{
					while (l < Count)
					{
						l <<= 1;
						if (pred(op.Operate(prod, tree[l])))
						{
							prod = op.Operate(prod, tree[l]);
							l++;
						}
					}
					return l - Count;
				}
			} while ((l & -l) != l);
			return Count;
		}

		public int MinLeft(int r, Predicate<TValue> pred)
		{
			if (r == 0) return 0;
			r += Count;
			var prod = op.Identity;
			do
			{
				r--;
				while ((r & 1) != 0)
				{
					r >>= 1;
				}
				if (pred(op.Operate(tree[r], prod)))
				{
					prod = op.Operate(tree[r], prod);
				}
				else
				{
					while (r < Count)
					{
						r <<= 1;
						r++;
						if (pred(op.Operate(tree[r], prod)))
						{
							r--;
							prod = op.Operate(tree[r], prod);
						}
					}
					return r - Count;
				}
			} while ((r & -r) != r);
			return 0;
		}
	}
}
