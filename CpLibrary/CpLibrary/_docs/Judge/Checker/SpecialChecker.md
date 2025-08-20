---
title: スペシャルジャッジ用チェッカー
documentation_of: //CpLibrary/Judge/Checker/SpecialChecker.cs
---

## 概要

スペシャルジャッジを実装するチェッカーシステムです。

このクラスにおいては、コンストラクタにスペシャルジャッジのプログラムを表すメソッドを与えます。それ以外の使用方法は [`NormalChecker`](https://fairy-lettuce.github.io/CpLibrary/CpLibrary/Judge/Checker/NormalChecker.cs) とほぼ同じです。

実行するプログラムは「入力と出力のストリームを引数に取り、何も返さないメソッド ( `Action<StreamReader, StreamWriter>` ) 」として与えることができます。

また、入力および出力は全て `string` もしくは `MemoryStream` として与えることができます。

実行結果は `JudgeResult` 構造体として返されます。

## コンストラクタ

- `SpecialChecker(Action<StreamReader, StreamWriter> solution)`: これは使わないと思う。
- `SpecialChecker(Action<StreamReader, StreamWriter> solution, Func<StreamReader, StreamReader, StreamReader, JudgeResult> judge)`
	- `solution` にテストしたいプログラムを、`judge` にジャッジプログラムを渡して初期化します。
	- `judge` は次のようなメソッドです: `JudgeResult Judge(StreamReader input, StreamReader expected, StreamReader actual)`
	- すなわち、「元の問題の入力」「想定解の出力」「提出された解答の出力」を出力する `StreamReader` を順番に引数に取り、返り値が `JudgeStatus` であるメソッドです。

## プロパティ

- `TimeLimit`: 実行時間制限を取得または設定します。`TimeSpan.Zero` と同一の場合は制限無しとなります。
- `MemoryLimitKB`: メモリ使用量制限を取得または設定します。0 の場合は制限なしとなります。ただし、技術上の制約から現時点ではメモリ使用量を計測することはできないため、意味のないプロパティとなっています。

## メソッド

- `Run(input, expected)`: 入力を `input` 、正答とみなされる出力を `expected` としてジャッジを行います。
- `Run(input, expectedSolution)`: 入力を `input` 、想定解を `expectedSolution` としてジャッジを行います。
