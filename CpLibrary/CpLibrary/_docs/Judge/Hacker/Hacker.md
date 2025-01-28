---
documentation_of: //CpLibrary/Judge/Hacker/Hacker.cs
---

## 概要

与えられた入力生成器を用いて、引数で与えた解法が `WA` や `RE` , `TLE` などの誤答ステータスを出すような入力 (ハックケース) をブルートフォースに探します。

実行時間等の制限で正答しないが、解答出力は正しいと保証されていると分かっているプログラムを用いることで `WA` を得られる入力を探す、いわゆるランダムテストを行うことを想定しています。

## コンストラクター

- `Hacker(CheckerBase, Action<StreamWriter>, Action<StreamReader, StreamWriter>)`: ハック対象となる解法やジャッジ等を保持するチェッカー、入力生成器、想定解を与えてインスタンスを初期化します。

## メソッド

- `FindHackCase()`: チェッカーが `JudgeStatus.AC` 以外を返すような入力ケースを返します。もしかすると入力ケース以外にも `expected, actual, JudgeStatus, JudgeMessage` を含むクラスを返すように今後変わるかもしれません。開発中だからこういう仕様変更は許してください。
