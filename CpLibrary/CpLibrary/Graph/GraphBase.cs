using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Globalization;
using System.Threading;

namespace CpLibrary.Graph;

public static partial class Graph
{
	public interface IEdge
	{
		int To { get; }
	}

	public interface IWeightedEdge : IEdge
	{
		long Weight { get; }
	}

	public interface IGraph<TEdge> where TEdge : IEdge
	{
		public List<TEdge> this[int index] { get; }
		public int NodeCount { get; }
		public List<TEdge> Next(int x);
	}

	public interface IWeightedGraph<TEdge> : IGraph<TEdge> where TEdge : IWeightedEdge { }

	public struct BasicEdge : IEdge
	{
		public int To { get; private set; }

		public BasicEdge(int to)
		{
			this.To = to;
		}

		public override string ToString() => $"{To}";
		public static implicit operator BasicEdge(int edge) => new BasicEdge(edge);
		public static implicit operator int(BasicEdge edge) => edge.To;
	}

	public struct WeightedEdge : IWeightedEdge
	{
		public int To { get; private set; }
		public long Weight { get; private set; }

		public WeightedEdge(int to, long weight)
		{
			To = to;
			Weight = weight;
		}

		public WeightedEdge(int to) : this(to, 1) { }

		public override string ToString() => $"[{Weight}] -> {To}";
		public void Deconstruct(out int to, out long weight) => (to, weight) = (To, Weight);
	}

	public class UndirectedGraph : IGraph<BasicEdge>
	{
		public readonly List<List<BasicEdge>> g;

		public List<BasicEdge> this[int index] => g[index];
		public int NodeCount => g.Count;

		public UndirectedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<BasicEdge>()).ToList();

		public UndirectedGraph(int nodeCount, int[] u, int[] v) : this(nodeCount)
		{
			if (u.Length != v.Length) throw new ArgumentException($"The arrays {nameof(u)} and {nameof(v)} must have the same length.");
			for (var i = 0; i < u.Length; i++)
			{
				AddEdge(u[i], v[i]);
			}
		}

		public void AddEdge(int u, int v)
		{
			g[u].Add(v);
			g[v].Add(u);
		}

		public void AddNode() => g.Add(new List<BasicEdge>());
		public List<BasicEdge> Next(int x) => g[x];
	}

	public class DirectedGraph : IGraph<BasicEdge>
	{
		public readonly List<List<BasicEdge>> g;

		public List<BasicEdge> this[int index] => g[index];
		public int NodeCount => g.Count;

		public DirectedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<BasicEdge>()).ToList();

		public DirectedGraph(int nodeCount, int[] from, int[] to) : this(nodeCount)
		{
			if (from.Length != to.Length) throw new ArgumentException($"The arrays {nameof(from)} and {nameof(to)} must have the same length.");
			for (var i = 0; i < from.Length; i++)
			{
				AddEdge(from[i], to[i]);
			}
		}

		public void AddEdge(int from, int to) => g[from].Add(to);
		public void AddNode() => g.Add(new List<BasicEdge>());
		public List<BasicEdge> Next(int x) => g[x];
	}

	public class UndirectedWeightedGraph : IWeightedGraph<WeightedEdge>
	{
		public readonly List<List<WeightedEdge>> g;

		public List<WeightedEdge> this[int index] => g[index];
		public int NodeCount => g.Count;

		public UndirectedWeightedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<WeightedEdge>()).ToList();

		public UndirectedWeightedGraph(int nodeCount, int[] u, int[] v, long[] weight) : this(nodeCount)
		{
			if (u.Length != v.Length) throw new ArgumentException($"The arrays {nameof(u)} and {nameof(v)} must have the same length.");
			if (u.Length != weight.Length) throw new ArgumentException($"The arrays {nameof(u)} and {nameof(weight)} must have the same length.");
			if (weight.Length != v.Length) throw new ArgumentException($"The arrays {nameof(v)} and {nameof(weight)} must have the same length.");
			for (var i = 0; i < u.Length; i++)
			{
				AddEdge(u[i], v[i], weight[i]);
			}
		}

		public void AddEdge(int u, int v, long w)
		{
			g[u].Add(new WeightedEdge(v, w));
			g[v].Add(new WeightedEdge(u, w));
		}
		public void AddNode() => g.Add(new List<WeightedEdge>());
		public List<WeightedEdge> Next(int x) => g[x];
	}

	public class DirectedWeightedGraph : IWeightedGraph<WeightedEdge>
	{
		public readonly List<List<WeightedEdge>> g;

		public List<WeightedEdge> this[int index] => g[index];
		public int NodeCount => g.Count;

		public DirectedWeightedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<WeightedEdge>()).ToList();

		public DirectedWeightedGraph(int nodeCount, int[] from, int[] to, long[] weight) : this(nodeCount)
		{
			if (from.Length != to.Length) throw new ArgumentException($"The arrays {nameof(from)} and {nameof(to)} must have the same length.");
			if (from.Length != weight.Length) throw new ArgumentException($"The arrays {nameof(from)} and {nameof(weight)} must have the same length.");
			if (weight.Length != to.Length) throw new ArgumentException($"The arrays {nameof(to)} and {nameof(weight)} must have the same length.");
			for (var i = 0; i < from.Length; i++)
			{
				AddEdge(from[i], to[i], weight[i]);
			}
		}

		public void AddEdge(int from, int to, long w)
		{
			g[from].Add(new WeightedEdge(to, w));
		}
		public void AddNode() => g.Add(new List<WeightedEdge>());
		public List<WeightedEdge> Next(int x) => g[x];
	}
}
