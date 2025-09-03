[![Actions Status](https://github.com/fairy-lettuce/CpLibrary/workflows/verify/badge.svg)](https://github.com/fairy-lettuce/CpLibrary/actions)
[![GitHub Pages](https://img.shields.io/static/v1?label=GitHub+Pages&message=CpLibrary+&color=brightgreen&logo=github)](https://fairy-lettuce.github.io/CpLibrary)

[fairy_lettuce](https://atcoder.jp/users/fairy_lettuce) の競技プログラミング用ライブラリです。

[competitive-verifier](https://github.com/competitive-verifier/competitive-verifier) を用いて GitHub Pages にライブラリのドキュメントを生成しています。

## リポジトリ構成

- ソリューション `CpLibrary`
  - `CpLibrary`: ライブラリ本体
  - `CpLibrary.Test`: ライブラリのテストコード
  - `CpLibrary.Verify`: competitive-verifier を用いたライブラリの verify
  - `CpLibrary.Benchmark`: ライブラリのベンチマークコード
- ソリューション `CpProject`
  - `CpProject`: コンテスト中などに使用するテンプレート

## 使い方

Visual Studio でクローンして動かせば多分上手く行きます。

## Special Thanks

[kzrnm さん](https://github.com/kzrnm) の作成した [competitive-verifier](https://github.com/competitive-verifier/competitive-verifier) および数多くの C# ライブラリを使用しており、また氏よりたくさんの助言を頂きました。ありがとうございます。

## License

このリポジトリのコードは [CC0 1.0 Universal](LICENSE) でライセンスされています。

ただし、一部のコードは MIT でライセンスされています。ライセンスの詳細は、各ファイルに記載されています。

また、内部で使用しているいくつかのパッケージは、独自のライセンスを持っています。これらのパッケージのライセンスは、各パッケージのドキュメントやリポジトリで確認できます。
