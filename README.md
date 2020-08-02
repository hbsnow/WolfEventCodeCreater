# WolfEventCodeCreater

ウディタの以下の情報をテキストファイルで出力するツール。
* コモンイベント
* データベース（可変・ユーザー・システム）
* マップ（マップチップ・マップイベント）
* マップツリー　注：不具合により取得できないことがあります。
* タイルセット

このツールにはウディタツール開発用ライブラリである[WodiKs](http://alphastella07ks.blog.shinobi.jp/wodiks/wodiks-release_alpha-version)が使用されています。

私的利用目的なので機能追加は気まぐれで、テストコードはありません。

![WolfEventCodeCreater](https://user-images.githubusercontent.com/661266/27504262-789bd662-58c1-11e7-9247-fc0686f4efc3.png)

## 使い方

1. [ファイル] - [プロジェクト選択]からウディタプロジェクトのルートディレクトリを選択
2. [実行]を押す

## オプション設定

[ヘルプ]- [設定]、または、設定XMLファイル(settings.xml)からオプションを選択できます。

|設定項目|XMLタグ名|設定内容|
|:--|:--|:--|
|出力フォルダ|```OutputDirName```|出力ファイルのフォルダ名を設定|
|コメントアウト|```CommentOut```|出力させたくないコモンイベントの名前の先頭につける文字列を設定|
|コモン番号の出力|```IsOutputCommonNumber```|コモンイベントの出力ファイルの名前にコモン番号を付与するかどうかを設定|
|-|```IsAdditionalDateTimeToOutputDirNameSuffiix```|出力フォルダ名の末尾に作成日時を付与するかどうかを設定|

## ダウンロード

- [ダウンロード](https://drive.google.com/drive/folders/0B6XTw9szgIZtNDlJZ244Y2oyUTg?usp=sharing)
