---
documentation_of: //CpLibrary/Judge/Checker/NormalChecker.cs
---

## 概要

通常のジャッジを実装するチェッカーシステムです。

このクラスにおいては、想定出力と実際の出力を空白文字で区切り、各文字列が要素ごとに同じであるときに正答判定が返されます。

実行するプログラムは「入力と出力のストリームを引数に取り、何も返さないメソッド ( `Action<StreamReader, StreamWriter>` ) 」として与えることができます。

また、入力および出力は全て `string` もしくは `MemoryStream` として与えることができます。

実行結果は `JudgeResult` 構造体として返されます。

## コンストラクタ

- `NormalChecker(Action<StreamReader, StreamWriter>)`: テストしたいプログラムを与えてチェッカーシステムを初期化します。

## プロパティ

- `TimeLimit`: 実行時間制限を取得または設定します。`TimeSpan.Zero` と同一の場合は制限無しとなります。
- `MemoryLimitKB`: メモリ使用量制限を取得または設定します。0 の場合は制限なしとなります。ただし、技術上の制約から現時点ではメモリ使用量を計測することはできないため、意味のないプロパティとなっています。

## メソッド

- `Run(input, expected)`: 入力を `input` 、正答とみなされる出力を `expected` としてジャッジを行います。
- `Run(input, expectedSolution)`: 入力を `input` 、想定解を `expectedSolution` としてジャッジを行います。
