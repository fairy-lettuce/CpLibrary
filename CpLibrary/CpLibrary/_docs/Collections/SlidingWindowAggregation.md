---
title: Sliding window aggregation
documentation_of: //CpLibrary/Collections/SlidingWindowAggregation.cs
---

## 概要

Sliding Window Aggregation (SWAG) です。

半群を乗せることができる Queue で、全体の総積を $O(1)$ で求められます。

Queue 自体は two-stack queue で実装しているため償却 $O(1)$ です。

## コンストラクタ

- `SlidingWindowAggregation<T>(Func<T, T, T> operate)`: 半群演算 `operate` を用いた SWAG を初期化します。

## プロパティ

- `Count`: queue 内の要素数。

## メソッド

- `void Push(T x)`: 要素 `x` を末尾に追加する。
- `T Pop()`: 最も早く追加した要素を削除し、その値を返す。`Count == 0` のときは `InvalidOperationException` を投げる。
- `T Prod()`: queue 内の全要素の総積を求める。`Count == 0` のときは `InvalidOperationException` を投げる (単位元の存在は求めていないため)。
