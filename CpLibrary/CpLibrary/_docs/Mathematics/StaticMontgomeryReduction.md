---
documentation_of: //CpLibrary/Mathematics/StaticMontgomeryReduction.cs
---

## 概要

Montgomery reduction を行う構造体です。法 (`Mod`) はコンパイル時に決定している必要があります。

法 `Mod` は $1\le Mod\le 2^{30}$ を満たす奇数である必要があります。

使用するには、`default` を `static` メンバとして保持してください。

```cs
internal static readonly StaticMontgomeryReduction<T> mr = default;
```

これにより最適化がかかるため、オーバーヘッドがほぼ無視できます。

## メソッド

- `uint Reduce(ulong t)`: `t` を Montgomery reduction をした結果の整数を返します。
  - 引数の値は $0\le t\lt 2Mod$ を満たす必要があります。
  - 返される値は $0\le x\lt 2Mod$ が保証されています。
- `uint ToMontgomery(ulong)` / `uint ToMontgomery(long)`: `t` を Montgomery reduction した値を返します。
- `uint ToInteger(uint)`: Montgomery reduction した結果の整数を本来の整数値に変換します。
- `uint Multiply(uint x, uint y)`: Montgomery reduction した整数 `x, y` の積を Montgomery reduction 表現のまま返します。

## ToDo

- Montogomery 表現を `uint` ではなく別の構造体にすることで区別しやすくする？

## 参考

- https://github.com/kzrnm/ac-library-csharp/issues/12
  - ac-library-csharp 実装の `IStaticMod` のプロパティが `static abstract` になると便利になりそうだけどなあ
	- パフォーマンスは同じで微妙に利便性が上がる程度だが、破壊的変更のため PR をしたくない