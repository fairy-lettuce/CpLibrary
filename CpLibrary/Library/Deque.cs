using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace CpLibrary.Library.Collections
{
	public class Deque<T> : IEnumerable<T>
	{
		int dx;
		T[] buffer;
		int mask;

		public int Count { get; private set; }

		public Deque() : this(8) { }

		public Deque(int capacity)
		{
			if (capacity != (capacity & -capacity))
			{
				int tmp = capacity;
				capacity = 1;
				while (capacity < tmp)
				{
					capacity <<= 1;
				}
			}
			mask = capacity - 1;
			buffer = new T[capacity];
		}

		public Deque(IEnumerable<T> value) : this(value.Count())
		{
			int i = 0;
			foreach (var e in value)
			{
				buffer[i] = e;
				i++;
			}
		}

		public T this[int index]
		{
			get
			{
				return buffer[(index + dx) & mask];
			}
			set
			{
				if (0 <= index && index < Count) throw new IndexOutOfRangeException();
				buffer[(index + dx) & mask] = value;
			}
		}

		public void PushFront(T item)
		{
			if (Count == buffer.Length) extend();
			dx = (dx + buffer.Length - 1) & mask;
			buffer[dx] = item;
			Count++;
		}

		public T PopFront()
		{
			var ret = buffer[dx = (dx + 1) & mask];
			Count--;
			return ret;
		}

		public T Last => buffer[(dx + Count - 1) & mask];

		public void PushBack(T item)
		{
			if (Count == buffer.Length) extend();
			buffer[(dx + Count++) & mask] = item;
		}


		public T PopBack()
		{
			var ret = buffer[(dx + --Count) & mask];
			return ret;
		}

		public T First => buffer[dx];

		private void extend()
		{
			var newBuffer = new T[buffer.Length * 2];
			for (int i = 0; i < buffer.Length; i++)
			{
				newBuffer[i] = buffer[(dx + i) & mask];
			}
			mask = newBuffer.Length - 1;
			dx = 0;
			buffer = newBuffer;
		}

		public void Clear()
		{
			Count = 0;
			buffer = new T[8];
			dx = 0;
			mask = buffer.Length - 1;
		}

		public void AddAt(int index, T item)
		{
			PushFront(item);
			for (int i = 0; i < index; i++)
			{
				this[i] = this[i + 1];
			}
			this[index] = item;
		}

		public T RemoveAt(int index)
		{
			var ret = this[index];
			for (int i = index; i > 0; i--)
			{
				this[i] = this[i - 1];
			}
			PopFront();
			return ret;
		}

		public IEnumerator<T> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => this.GetEnumerator();

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
