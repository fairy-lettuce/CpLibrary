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
	public class PriorityQueue<T>
	{
		private T[] heap;
		private readonly IComparer<T> comparer;

		public int Count { get; private set; }

		public PriorityQueue(IComparer<T> comparer)
		{
			this.comparer = comparer;
			heap = new T[1];
			Count = 0;
		}

		public PriorityQueue(Comparison<T> comparison) : this(Comparer<T>.Create(comparison)) { }

		public PriorityQueue() : this(Comparer<T>.Default) { }

		public T Peek => heap[0];

		public void Enqueue(T value)
		{
			if (Count == heap.Length) Expand();
			heap[Count++] = value;
			var index = Count - 1;
			while (index > 0)
			{
				var par = Parent(index);
				if (comparer.Compare(heap[index], heap[par]) >= 0) break;
				(heap[index], heap[par]) = (heap[par], heap[index]);
				index = par;
			}
		}

		public T Dequeue()
		{
			var ret = Peek;
			heap[0] = heap[Count - 1];
			--Count;
			var index = 0;
			while (index * 2 + 1 < Count)
			{
				var (l, r) = Child(index);
				var child = l;
				if (r < Count)
				{
					if (comparer.Compare(heap[l], heap[r]) > 0) child = r;
				}
				if (comparer.Compare(heap[index], heap[child]) <= 0) break;
				(heap[index], heap[child]) = (heap[child], heap[index]);
				index = child;
			}
			return ret;
		}

		private void Expand() => Array.Resize(ref heap, heap.Length << 1);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int Parent(int index) => (index - 1) / 2;
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private (int left, int right) Child(int index) => (index * 2 + 1, index * 2 + 2);
	}
}
