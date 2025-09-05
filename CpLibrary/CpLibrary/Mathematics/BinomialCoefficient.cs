using System.Runtime.CompilerServices;

namespace CpLibrary.Mathematics;

public class BinomialCoefficient
{
	readonly uint mod;
	uint[] f, finv, inv;

	public BinomialCoefficient(uint mod, int size)
	{
		this.mod = mod;
		f = new uint[size];
		finv = new uint[size];
		inv = new uint[size];
		Init();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Binom(int n, int k)
	{
		if ((uint)k > (uint)n) return 0;
		return (uint)(((long)f[n] * finv[k] % mod) * finv[n - k] % mod);
	}
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Factorial(int n) => f[n];
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public uint Inv(int n) => inv[n];

	void Init()
	{	
		f[0] = finv[0] = inv[0] = 1;
		f[1] = finv[1] = inv[1] = 1;
		for (int i = 2; i < f.Length; i++)
		{
			f[i] = (uint)((long)f[i - 1] * i % mod);
			inv[i] = (uint)(mod - (long)inv[mod % i] * (mod / i) % mod);
			finv[i] = (uint)((long)finv[i - 1] * inv[i] % mod);
		}
	}
}
