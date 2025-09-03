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

namespace CpLibrary.Graph;

public static partial class Graph
{
	public interface IEdge
	{
		int To { get; }
	}

	public interface IWeightedEdge<T> : IEdge where T : INumber<T>
	{
		T Weight { get; }
	}

	public interface IGraph<TEdge> where TEdge : IEdge
	{
		public List<TEdge> this[int index] { get; }
		public int NodeCount { get; }
		public List<TEdge> Next(int x);
	}

	public interface IWeightedGraph<TEdge, TWeight> : IGraph<TEdge> where TWeight : INumber<TWeight> where TEdge : IWeightedEdge<TWeight> { }

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

	public struct WeightedEdge<T> : IWeightedEdge<T> where T : INumber<T>
	{
		public int To { get; private set; }
		public T Weight { get; private set; }

		public WeightedEdge(int to, T weight)
		{
			To = to;
			Weight = weight;
		}

		public WeightedEdge(int to) : this(to, default(T)) { }

		public override string ToString() => $"[{Weight}] -> {To}";
		public void Deconstruct(out int to, out T weight) => (to, weight) = (To, Weight);
	}

	public class UndirectedGraph : IGraph<BasicEdge>
	{
		public readonly List<List<BasicEdge>> g;

		public List<BasicEdge> this[int index] => g[index];
		public int NodeCount => g.Count;

		public UndirectedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<BasicEdge>()).ToList();

		public UndirectedGraph(int nodeCount, int[] u, int[] v) : this(nodeCount)
		{
			Debug.Assert(u.Length == v.Length, $"The arrays {nameof(u)} and {nameof(v)} must have the same length.");
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
			Debug.Assert(from.Length == to.Length, $"The arrays {nameof(from)} and {nameof(to)} must have the same length.");
			for (var i = 0; i < from.Length; i++)
			{
				AddEdge(from[i], to[i]);
			}
		}

		public void AddEdge(int from, int to) => g[from].Add(to);
		public void AddNode() => g.Add(new List<BasicEdge>());
		public List<BasicEdge> Next(int x) => g[x];
	}

	public class UndirectedWeightedGraph<T> : IWeightedGraph<WeightedEdge<T>, T> where T : INumber<T>
	{
		public readonly List<List<WeightedEdge<T>>> g;

		public List<WeightedEdge<T>> this[int index] => g[index];
		public int NodeCount => g.Count;

		public UndirectedWeightedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<WeightedEdge<T>>()).ToList();

		public UndirectedWeightedGraph(int nodeCount, int[] u, int[] v, T[] weight) : this(nodeCount)
		{
			Debug.Assert(u.Length == v.Length, $"The arrays {nameof(u)} and {nameof(v)} must have the same length.");
			Debug.Assert(u.Length == weight.Length, $"The arrays {nameof(u)} and {nameof(weight)} must have the same length.");
			Debug.Assert(weight.Length == v.Length, $"The arrays {nameof(weight)} and {nameof(v)} must have the same length.");
			for (var i = 0; i < u.Length; i++)
			{
				AddEdge(u[i], v[i], weight[i]);
			}
		}

		public void AddEdge(int u, int v, T w)
		{
			g[u].Add(new WeightedEdge<T>(v, w));
			g[v].Add(new WeightedEdge<T>(u, w));
		}
		public void AddNode() => g.Add(new List<WeightedEdge<T>>());
		public List<WeightedEdge<T>> Next(int x) => g[x];
	}

	public class DirectedWeightedGraph<T> : IWeightedGraph<WeightedEdge<T>, T> where T : INumber<T>
	{
		public readonly List<List<WeightedEdge<T>>> g;

		public List<WeightedEdge<T>> this[int index] => g[index];
		public int NodeCount => g.Count;

		public DirectedWeightedGraph(int nodeCount) => g = Enumerable.Repeat(0, nodeCount).Select(_ => new List<WeightedEdge<T>>()).ToList();

		public DirectedWeightedGraph(int nodeCount, int[] from, int[] to, T[] weight) : this(nodeCount)
		{
			Debug.Assert(from.Length == to.Length, $"The arrays {nameof(from)} and {nameof(to)} must have the same length.");
			Debug.Assert(from.Length == weight.Length, $"The arrays {nameof(from)} and {nameof(weight)} must have the same length.");
			Debug.Assert(weight.Length == to.Length, $"The arrays {nameof(weight)} and {nameof(to)} must have the same length.");
			for (var i = 0; i < from.Length; i++)
			{
				AddEdge(from[i], to[i], weight[i]);
			}
		}

		public void AddEdge(int from, int to, T w)
		{
			g[from].Add(new WeightedEdge<T>(to, w));
		}
		public void AddNode() => g.Add(new List<WeightedEdge<T>>());
		public List<WeightedEdge<T>> Next(int x) => g[x];
	}
}
