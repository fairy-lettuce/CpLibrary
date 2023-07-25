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
	public class Deque<T> : IEnumerable<T>
	{
		int cur;
		T[] buffer;
		int mask;

		public int Count { get; private set; }

		public Deque() : this(8) { }

		public Deque(int capacity)
		{
			if (capacity != (capacity & -capacity))
			{
				var t = capacity;
				capacity = 1;
				while (capacity < t)
				{
					capacity <<= 1;
				}
			}
			mask = capacity - 1;
			buffer = new T[capacity];
			cur = 0;
			Count = 0;
		}

		public Deque(IEnumerable<T> items) : this(items.Count())
		{
			var i = 0;
			foreach (var e in items)
			{
				buffer[i++] = e;
			}
			Count = i;
		}

		public T this[int index]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => buffer[(index + cur) & mask];
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set
			{
				if (index < 0 || Count <= index) throw new IndexOutOfRangeException();
				buffer[(index + cur) & mask] = value;
			}
		}

		public void PushFront(T value)
		{
			if (buffer.Length == Count) Extend();
			buffer[cur = ((cur + buffer.Length - 1) & mask)] = value;
			Count++;
		}

		public void PushBack(T value)
		{
			if (buffer.Length == Count) Extend();
			buffer[(cur + Count) & mask] = value;
			Count++;
		}

		public T PopFront()
		{
			if (Count == 0) throw new InvalidOperationException("Deque contains no elements.");
			var ret = buffer[cur];
			cur = ++cur & mask;
			Count--;
			return ret;
		}

		public T PopBack()
		{
			if (Count == 0) throw new InvalidOperationException("Deque contains no elements.");
			var ret = buffer[(cur + --Count) & mask];
			return ret;
		}

		public T Front => this[0];

		public T Back => this[Count - 1];

		public void Clear()
		{
			Count = 0;
			buffer = new T[8];
			cur = 0;
			mask = buffer.Length - 1;
		}

		private void Extend()
		{
			var newBuffer = new T[buffer.Length * 2];
			for (int i = 0; i < buffer.Length; i++)
			{
				newBuffer[i] = this[i];
			}
			mask = newBuffer.Length - 1;
			cur = 0;
			buffer = newBuffer;
		}

		public void AddAt(int index, T value)
		{
			if (index < 0 || Count < index) throw new IndexOutOfRangeException();
			if (index < Count / 2)
			{
				PushFront(value);
				for (int i = 0; i < index; i++)
				{
					this[i] = this[i + 1];
				}
				this[index] = value;
			}
			else
			{
				PushBack(value);
				for (int i = Count - 1; i >= index; i--)
				{
					this[i] = this[i - 1];
				}
				this[index] = value;
			}
		}

		public T RemoveAt(int index)
		{
			if (index < 0 || Count <= index) throw new IndexOutOfRangeException();
			var ret = this[index];
			if (index < Count / 2)
			{
				for (int i = index - 1; i >= 0; i--)
				{
					this[i + 1] = this[i];
				}
				PopFront();
			}
			else
			{
				for (int i = index; i < Count - 1; i++)
				{
					this[i] = this[i + 1];
				}
				PopBack();
			}
			return ret;
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		public T[] Items
		{
			get
			{
				var ret = new T[Count];
				for (int i = 0; i < Count; i++)
				{
					ret[i] = this[i];
				}
				return ret;
			}
		}
	}
}
