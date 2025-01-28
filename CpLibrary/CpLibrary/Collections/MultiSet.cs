using System;
using System.Collections.Generic;

namespace CpLibrary.Lib
{
	public class MultiSet<T> : Set<T> where T : IComparable<T>
	{
		public MultiSet() : base(isMultiSet: true) {}
		public MultiSet(IComparer<T> comparer) : base(comparer, true) { }
		public MultiSet(Comparison<T> comparison) : base(comparison, true) { }
		public MultiSet(IEnumerable<T> list) : base(list, true) { }
		public MultiSet(IEnumerable<T> list, IComparer<T> comparer) : base(comparer, true) { }
		public MultiSet(IEnumerable<T> list, Comparison<T> comparison) : base(list, comparison, true) { }
	}
}
