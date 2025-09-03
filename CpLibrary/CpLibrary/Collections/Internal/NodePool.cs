using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CpLibrary.Collections;

internal sealed class NodePool<T> where T : unmanaged
{
	T[] arr;
	int index;
	int[] stack;
	int sindex;

	public NodePool() : this(1 << 10) { }

	public NodePool(int size)
	{
		arr = GC.AllocateUninitializedArray<T>(size);
		index = 0;
		stack = GC.AllocateUninitializedArray<int>(1 << 10);
		sindex = 0;
	}

	public int Count => index - sindex;

	public ref T this[int index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		//get => ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(arr), (nint)(uint)index);
		get => ref arr[index];
	}
	// => ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(arr), (nint)(uint)index);
	// is slower than arr[index]

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public int Alloc(in T value)
	{
		//if (index == arr.Length) Array.Resize(ref arr, arr.Length << 1);
		//arr[index] = value;
		//return index++;
		var ret = -1;
		if (sindex > 0)
		{
			ret = stack[--sindex];
		}
		else
		{
			if (index == arr.Length) GrowArray();
			ret = index++;
		}
		arr[ret] = value;
		return ret;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public ref T AllocSlot(out int idx)
	{
		if (sindex > 0) { idx = stack[--sindex]; return ref arr[idx]; }
		if (index == arr.Length) GrowArray();
		return ref arr[idx = index++];
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	public void Free(int index)
	{
		if (sindex == stack.Length) GrowStack();
		stack[sindex++] = index;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	void GrowArray()
	{
		Array.Resize(ref arr, arr.Length << 1);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
	void GrowStack()
	{
		Array.Resize(ref stack, stack.Length << 1);
	}
}
