---
documentation_of: //CpLibrary/Collections/Set.cs
---

## 概要

平衡二分探索木による順序つき集合のコレクションです。

値の追加・削除、$k$ 番目の取得、特に最大値や最小値の取得、要素の二分探索が $O(\log N)$ で行えます。

.NET の API にある `SortedSet<T>` クラスの機能強化版です (`SortedSet` は $k$ 番目の取得、要素の二分探索が不可能)。

## コンストラクタ

全コンストラクタ共通: `isMultiSet` を `true` にすると要素の重複を許容します (多重集合)。

- `Set()`: 空の集合で初期化します。
- `Set(IComparer<T>)`: `IComparer<T>` で順序を指定し、空の集合で初期化します。
- `Set(Comparison<T>)`: `Comparison<T>` で順序を指定し、空の集合で初期化します。
- `Set(IEnumerable<T>)`: `IEnumerable<T>` の各要素で集合を初期化します。
- `Set(IEnumerable<T>, IComparer<T>)`: `IComparer<T>` で順序を指定し、`IEnumerable<T>` の各要素で集合を初期化します。
- `Set(IEnumerable<T>, Comparison<T>)`: `Comparison<T>` で順序を指定し、`IEnumerable<T>` の各要素で集合を初期化します。

## プロパティ

- `IsMultiSet`: 要素の重複を許容するかを示します。外部からの変更はできません。`IComparable<T>.Compare()` の結果が `0` であるときに要素が重複するものと判定します。
- `this[int]`: 指定したインデックスの要素を取得します。インデックスはコンストラクタで指定した順序に基づきます。外部からの変更はできません。
- `Items`: $O(N\log N)$ 時間をかけ、全要素を列挙します。
- `Count`: 要素数を取得します。$O(1)$ 時間。
- `Height`: 平衡二分探索木の高さを取得します。$O(1)$ 時間。

## メソッド

- `Add(T)`: 要素を集合に追加します。`IsMultiSet` が `false` のときに重複する要素を追加しようとすると、要素を追加しない代わりに `false` を返します。
- `Remove(T)`: 要素を集合から削除します。要素が存在しない場合、`false` を返します。`IsMultiSet` が `true` のときに重複する要素を削除しようとした場合、要素は 1 つのみ削除されます。
- `RemoveAt(int)`: 指定したインデックスの要素を集合から削除します。要素が存在しない場合、`false` を返します。
- `Contains(T)`: 指定した要素が存在するかを返します。
- `LowerBound(T)`: 指定した要素 `x` について、`x <= this[index]` を満たす最小の整数 `index` を返します。
- `UpperBound(T)`: 指定した要素 `x` について、`x < this[index]` を満たす最小の整数 `index` を返します。
- `EqualRange(T)`: 集合の家、指定した要素と等しい要素の個数を返します。
- `Clear()`: 集合をクリアし、空の集合にします。
- `Min()`: 最小の要素を返します。
- `Max()`: 最大の要素を返します。

## 明示的なインターフェースの実装

- `IEnumerable<T>`

## 参考

- [https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.generic.sortedset-1](https://learn.microsoft.com/ja-jp/dotnet/api/system.collections.generic.sortedset-1)
