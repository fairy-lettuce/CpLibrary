---
title: 二項係数の列挙 (素数 mod)
documentation_of: //CpLibrary/Mathematics/BinomialCoefficient.cs
---

## 概要

二項係数を前計算 $O(N)$, クエリ $O(1)$ で処理します。与える mod は素数である必要があります。

## コンストラクタ

- `BinomialCoefficient(int size)`: $[0, size)$ の前計算を行います。

## メソッド

$n$ はいずれも $size$ **未満**である必要があります。

ただし、`Binom(n, k)` の $k$ は範囲外を与えると $0$ を返します。

- `Binom(int n, int k)`: $\displaystyle \binom{n}{k}$ を計算します。
- `Factorial(int n)`: $n!$ を計算します。
- `Inv(int n)`: $\bmod{p}$ における $n$ の逆元 $n^{-1}$ を計算します。

## 参考

- [よくやる二項係数 (nCk mod. p)、逆元 (a\^-1 mod. p) の求め方](https://drken1215.hatenablog.com/entry/2018/06/08/210000)
