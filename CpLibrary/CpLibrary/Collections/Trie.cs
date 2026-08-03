using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Collections
{
	public class Trie<T>
	{
		int charSize;
		int start;

		const int sentinel = -1;
		List<int> count, c;
		List<int[]> next;
		List<List<T>> accept;

		public int Count { get => count[0]; }
		public int Size { get => count.Count; }
		public List<T> this[string str] => accept[Search(str)];

		public Trie(int charSize, char start)
		{
			this.charSize = charSize;
			this.start = start;
			count = new List<int>();
			c = new List<int>();
			next = new List<int[]>();
			accept = new List<List<T>>();
			AddNode();
		}

		public void AddNode()
		{
			this.count.Add(0);
			this.c.Add(-1);
			this.next.Add(Enumerable.Repeat(-1, charSize).ToArray());
			this.accept.Add(new List<T>());
		}

		public void Add(string str, T value) => Add(str, 0, str.Length, value);

		public void Add(string str) => Add(str, default);

		public void Add(string str, int l, int r, T value)
		{
			int cur = 0;
			for (var i = l; i < r; i++)
			{
				count[cur]++;
				int c = str[i] - start;
				if (next[cur][c] == -1)
				{
					AddNode();
					next[cur][c] = Size - 1;
					cur = Size - 1;
				}
				else cur = next[cur][c];
				this.c[cur] = c;
			}
			count[cur]++;
			accept[cur].Add(value);
		}

		private int Search(string str)
		{
			int cur = 0;
			for (var i = 0; i < str.Length; i++)
			{
				int c = str[i] - start;
				if (next[cur][c] == -1) return -1;
				else cur = next[cur][c];
			}
			return cur;
		}

		public bool Contains(string str, bool isPrefixSearch = false)
		{
			var index = Search(str);
			if (index == -1) return false;
			var ret = accept[Search(str)];
			if (isPrefixSearch)
			{
				return true;
			}
			else
			{
				if (ret.Count > 0) return true;
				else return false;
			}
		}

		public bool ContainsPrefix(string str) => Contains(str, true);
	}
}
