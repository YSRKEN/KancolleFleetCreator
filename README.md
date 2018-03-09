# 艦これ編成まとめ隊

Create Fleet's picture of Kantai Collection's Kammusues

## 概要

- [艦これ](http://www.dmm.com/netgame/feature/kancolle.html)において、出撃時に使用する編成を「まとめる」ソフトウェアです
- 艦娘部分・基地航空隊部分・支援艦隊部分をそれぞれまとめるプロダクトはありましたが、全部一緒にするのは無かったので作りました
- もちろん、それぞれの部分だけのまとめ画像を作ることもできます
- Eto.Formsライブラリ採用により、WindowsだけでなくLinuxでの動作も可能です(その際は[Mono](https://www.mono-project.com)が必要)

## 画面構成・使い方

![image](https://user-images.githubusercontent.com/3734392/37199896-5b9f1724-23c6-11e8-88b4-a33323ffb2fe.png)

- 自艦隊・支援艦隊・基地航空隊はそれぞれ違う色の番号が付きます
- 自艦隊や支援艦隊については、それが連合艦隊や複数艦隊になる場合、「1-3」や「2-2」といった表示になります
- 「ヘルプ(H)」→「バージョン情報(V)」から、プログラムのバージョン情報などが読めます

## 注意

- ソフトウェアの動作には、Windowsなら **.NET Framework 4.6.1** 以上、Linuxなら  **Mono** が必要です
- Monoの導入は、[こちらの説明ページ](http://www.mono-project.com/download/stable/#download-lin)を参照してください

## 作者

　YSR([Twitter](https://twitter.com/YSRKEN), [GitHub](https://github.com/YSRKEN/))

## 謝辞

- Readmeやヘルプファイルなどの表示に、[tatesuke](https://github.com/tatesuke) さんの「 [かんたんMarkdown](https://github.com/tatesuke/KanTanMarkdown) 」を使用しました
- ソフトウェア開発に協力してくださった、[おいがみ](https://twitter.com/oigami013)さんに深く感謝いたします

## License
　MIT License

## 使用ライブラリ

- [Eto.Forms](https://www.nuget.org/packages/Eto.Forms/)
 - Windows・Linuxどちらでも動くGUIフレームワーク

- [ReactiveProperty](https://www.nuget.org/packages/ReactiveProperty/)
 - MVVMによるソフトウェア構築に使用

## 更新履歴

### Ver.1.0.0

- 初公開
