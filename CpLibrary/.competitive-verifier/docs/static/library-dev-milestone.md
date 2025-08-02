---
title: 2023/07 大規模ライブラリ改築 仕様書
---

以下は自分用の覚書です。改築が終わり次第この内容は消えます。

### 動機

不便なところを直して生産性の上がる・信頼性の上がるライブラリにする

### 概要

- CpProject の動作をもっと簡便にする

- [oj](https://github.com/online-judge-tools/oj) と同等の機能を導入する

- CpLibrary に [competitive-verifier](https://github.com/competitive-verifier/competitive-verifier) を導入して信頼性の高いライブラリにする

### CpProject

- [**DONE!!**] A.cs とかのソースコードをキレイにして共通部分をくくりだす
  - どうせ現状 SourceExpander がよしなにやってくれてるので共通処理を書く必要もない
- いろんな動作をちゃんと拡充する
  - [**DONE!!**] 手元ジャッジの実装
  - ~~サンプルテストケースのダウンロード~~
    - AtCoder が困るので deprecated
  - [**DONE!!**] ランダムテスト
  - 特殊ジャッジへの対応 (誤差ジャッジ / スペシャルジャッジ / インタラクティブジャッジ)
  - ベンチマークできるようにする
- あまり A.cs とかで分割するの面倒になってきたし、いい感じにしたい

### CpLibrary

ライブラリがカスの整備状況なので、もうちょっと頑張って整備する

また信頼のおけるライブラリになるように、competitive-verifier を用いて verify およびドキュメンテーション作成を行う

- [**DONE!**] competitive-verifier の導入
- ベンチマークできるようにする

### FertiLib から未移植のライブラリ

- [ ] BinarySarch
- [ ] PriorityQueue
  - ただし PriorityQueue は .NET 6 から標準ライブラリに入ったので、移植しなくてもいいかも？
  - でも自作の方が改造しやすいので移植するかも
- [ ] Segtree
- [ ] LazySegtree
  - ac-library-csharp があるからいらないかも？
- [x] SWAG
- [ ] UnionFind
- [ ] Retroactive UnionFind
  - どの問題かで書いたことがある (忘れた)
- [ ] Geometry
- [ ] GraphBase
  - グラフ周りはもうちょっと賢い構成のクラスにしたいなあ
- [ ] HeavyLightDecomposition
- [ ] LowestCommonAncestor
- [ ] Relooting
- [ ] ShortestPaths
  - Dijkstra と BellmanFord が同じファイルにあるので、別ファイルに分ける必要がある
- [ ] TopologicalSort
- [ ] 数学関連の細々したやつ
- [ ] ExtGcd
- [ ] Matrix
  - .NET 7 の Generic Math に対応させる
- [ ] Rational
  - .NET 7 の Generic Math に対応させる
- [ ] KMPSearch
- [ ] Trie
