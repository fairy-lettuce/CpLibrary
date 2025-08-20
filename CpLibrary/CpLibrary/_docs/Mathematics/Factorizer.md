---
title: 素因数分解
documentation_of: //CpLibrary/Mathematics/Factorizer.cs
---

## 概要

素因数分解に関連する関数を収録した static クラスです。


## メソッド

- `Factorize(long)`
  素因数分解を実行します。その数が素数 (`IsPrime()` で判定) でないなら `FindFactor()` で素因数を探し、再帰的に行います。
- `IsPrime(long)`
  [Deterministic な Miller-Rabin 法](http://miller-rabin.appspot.com/) を用いて素数判定をします。$O(\log^2 n)$
- `FindFactor(long)`
  Pollard の ρ 法を用いて素因数を探します。乱択をします。$O(n^{\frac{1}{4}})$

## 参考

- [高速なMOD演算 Barrett Reduction, Montgomery 乗算](https://natsugiri.hatenablog.com/entry/2020/04/06/030559)
  Miller-Rabin 法は剰余の計算が多いので、Barrett Reduction や Montgomery 乗算により定数倍高速化ができる。素数判定をする数が $2^{32}$ を超えると `long` の範囲内で剰余を取れないのでかなり恩恵あり。
- [間違ったポラードのロー法の使い方](https://lpha-z.hatenablog.com/entry/2023/01/15/231500)
  $x\mapsto x^2+1\bmod n$ では疑似乱数として弱いという話。こわい。
- Deterministic Miller-Rabin の場合、$a\equiv 0\pmod n$ の場合素数であると判定しなければならない。しかも、底は小さい素因数 ($2, 3, 5$) を持っているため、それらを枝刈りしなければならない。