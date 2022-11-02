# UniEaseCopy

UniEaseCopy は、 Unity の Animation キーフレームをカーブの形状を維持した状態でコピー&ペーストできるエディタ拡張です。

## Contents

1. Demo
2. Usage
3. Installation
4. License

## Demo

[デモ](https://user-images.githubusercontent.com/6266016/167908285-3bf3886d-7b09-40b3-b1f9-726ced2ea2cf.gif)

## Usage

2通りの方法で使用できます。

### 1. GUI パネル

メニューの `Window > Animation > UniEaseCopy` より GUI パネルが開きます。

![image](https://user-images.githubusercontent.com/6266016/167912237-8625964e-b092-499b-b33c-4fe9e2c684c6.png)

![image](https://user-images.githubusercontent.com/6266016/167908621-62d97187-5fbb-4c36-bbd8-6e33e9847aff.png)

パネルの説明は下記のとおりです。

||説明|
|:--|:--|
|Copyボタン|選択中のキーフレームをコピーします|
|Easeボタン|選択中のキーフレームにカーブの形状をペーストします|
|Valueボタン|選択中のキーフレームに値だけペーストします|
|Log領域|エラーなどの内容が表示されます|
|Copied keyframes|コピー中のキーフレームの数が表示されます。一部のUnityバージョンではコピーしたキーフレームの情報も表示されます。|

### 2. Menu

PlayerSettings の `Scripting Define Symbols` に `UNIEASECOPY_USE_MENU_ITEM` を追加することで表示されるメニューから操作することもできます。

![image](https://user-images.githubusercontent.com/6266016/167912822-51d1789c-ce40-4de1-a208-99a73e2deec6.png)

## Installation

3つの方法でプロジェクトに追加して使用できます。

### 1. Scoped registory

ProjectSettings の `PackageManager` より以下の scoped registory を追加することで、 PackageManager の `My Registories` に表示されます。

![image](https://user-images.githubusercontent.com/6266016/168285937-41510ac6-bbd8-4bf8-88e8-ecdb8a0aecb2.png)

```json
{
    "name": "shutosg",
    "url": "https://package.openupm.com",
    "scopes": [
        "net.shutosg"
    ]
}
```

![image](https://user-images.githubusercontent.com/6266016/168287158-616e7faa-4b10-42c7-abcc-0f47e7172d54.png)

### 2. OpenUPM

[OpenUPM](https://openupm.com/) のCLIツールを用いてパッケージを追加できます

```shell
openupm add net.shutosg.uni-ease-copy
```

### 3. from git URL

![image](https://user-images.githubusercontent.com/6266016/167906590-0358137f-83bc-4d5a-981f-6eb867c261c9.png)

PackageManager の `Add package from git URL` に `https://github.com/shutosg/UniEaseCopy.git?path=Assets/UniEaseCopy` を入力することでインストールすることができます。

## License

MIT ライセンスです。
