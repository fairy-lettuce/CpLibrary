using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace CpLibrary.Collections
{
	public class SlidingWindowAggregation<T>
	{
		Stack<(T value, T prod)> front, back;
		Func<T, T, T> operate;

		public int Count { get; private set; }

		public SlidingWindowAggregation(Func<T, T, T> operate)
		{
			this.operate = operate;
			front = new Stack<(T value, T prod)>();
			back = new Stack<(T value, T prod)>();
		}

		public SlidingWindowAggregation(IEnumerable<T> list, Func<T, T, T> monoid) : this(monoid)
		{
			foreach (var e in list)
			{
				Push(e);
			}
		}

		public T Prod()
		{
			Debug.Assert(front.Count + back.Count > 0);
			if (front.Count == 0) return back.Peek().prod;
			if (back.Count == 0) return front.Peek().prod;
			return operate(front.Peek().prod, back.Peek().prod);
		}

		public void Push(T x)
		{
			Count++;
			if (back.Count == 0) back.Push((x, x));
			else back.Push((x, operate(back.Peek().prod, x)));
		}

		public T Pop()
		{
			Debug.Assert(Count > 0);
			Count--;
			if (front.Count == 0)
			{
				while (back.Count > 0)
				{
					var e = back.Pop();
					if (front.Count == 0) front.Push((e.value, e.value));
					else front.Push((e.value, operate(e.value, front.Peek().prod)));
				}
			}
			return front.Pop().value;
		}
	}

}
