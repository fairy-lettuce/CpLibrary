## 2023/07 大規模ライブラリ改築 仕様書

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
    - 手元ジャッジの実装
	- サンプルテストケースのダウンロード
	- ランダムテスト
	- 特殊ジャッジへの対応 (誤差ジャッジ / スペシャルジャッジ / インタラクティブジャッジ)
	- ベンチマークできるようにする

### CpLibrary

ライブラリがカスの整備状況なので、もうちょっと頑張って整備する

また信頼のおけるライブラリになるように、competitive-verifier を用いて verify およびドキュメンテーション作成を行う

- [**DONE!**] competitive-verifier の導入
- ベンチマークできるようにする
