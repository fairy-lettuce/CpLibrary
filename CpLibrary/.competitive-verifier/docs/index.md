[fairy_lettuce](https://atcoder.jp/users/fairy_lettuce) の競技プログラミング用ライブラリです。

[competitive-verifier](https://github.com/competitive-verifier/competitive-verifier) を用いてドキュメントを生成しています。

## 使い方

動作は Visual Studio のデバッガ等を使う前提です。それ以外の動作は保証しません。

`Contest` ディレクトリ下のファイルに問題を解く C# コードを書き、ソリューション `CpProject` をビルドしてください。

実行すると現れるコンソールでは以下のコマンドが使用可能です。

`run <PROGRAM>`: `input.txt` を入力として、`A` から `J` までの一文字で表されるコードを実行し、標準出力に出力します。

`dl`: 未実装です。指定したコンテストサイトのサンプル入出力をダウンロードできるようになる予定です。仕様は変わる可能性があります。

`exit`: 実行を終了します。

## 実装予定の機能・ライブラリ等

- たくさんのライブラリ (未移植)
- verify コード
- ライブラリのドキュメント
- 他色々→ <a href="{{ site.baseurl }}/library-dev-milestone">この記事</a> に詳しく書きました
