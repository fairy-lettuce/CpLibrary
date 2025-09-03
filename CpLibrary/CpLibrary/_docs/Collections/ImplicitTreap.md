---
title: Implicit Treap
documentation_of: //CpLibrary/Collections/Treap.cs
---

## 概要

Implicit key (その時点でのノードの位置) によって値にアクセスする平衡二分探索木です。

注: まだ制作途中です。

## コンストラクタ

- `ImplicitTreap(int capacity)`: `capacity` を容量として空の要素で初期化します。
- `ImplicitTreap(IList<T> list)`: ソート済みの `list` を用いて $O(N)$ で初期化します。

## プロパティ

TBW

## メソッド

TBW

## 補足

- ポインタベースではなく配列ベースで実装しています。`nodes[0]` は必ず `nil` ノードである前提で書いています。
