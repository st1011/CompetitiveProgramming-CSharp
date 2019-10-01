# CompetitiveProgramming-CSharp
C#で記述された競技プログラミング用のライブラリです。

主に[AtCoder](https://atcoder.jp)や[AOJ](https://onlinejudge.u-aizu.ac.jp/home)用に作成されています。
メインターゲットはAtCoderなので、C#6までの機能だけで記述されています。

[プログラミングコンテスト攻略のためのアルゴリズムとデータ構造(a.k.a. 螺旋本)](https://www.amazon.co.jp/dp/B00U5MVXZO)や[プログラミングコンテストチャレンジブック(a.k.a. 蟻本)](https://www.amazon.co.jp/dp/B00CY9256C)を参考にしているライブラリも含まれています。

時々思い出したように既存のライブラリを改造するので、インターフェースの互換性は保たれないことがあります。


## 対応アルゴリズム
### データ構造
- BIT / Fenwick Tree
- Deque
- 優先度付きキュー(Priority Queue)
- Rolling Hash(Base, Mod固定+Mod*2)
- セグメント木(モノイドは可変)
  - 遅延無しセグメント木
  - 遅延伝播セグメント木
- Union Find
- スパーステーブル
- Z-Algorithm
- Pair


### 数学
- 約数列挙
- GCD / LCM
- 拡張GCD
- 素数
  - 素数判定
  - 素数列挙
  - 素因数分解
  - オイラーのファイ関数(n以下でnと互いに素な自然数の個数)
  - ルジャンドルの公式(n!を割り切る素数pの最大べき指数)
- ModInt
  - 四則演算
  - Pow
  - 組み合わせ計算(nCr, nPr, nHr)

### グラフ理論
- 最短経路
  - ダイクストラ(辺の重みが非負・有向グラフ・単一始点)
  - ベルマンフォード(有向グラフ・単一始点・負閉路検出付き)
  - ワーシャルフロイド(有向グラフ・全頂点間の経路)
- 最小全域木(無向グラフだけ)
- 有向グラフの最長経路探索
- トポロジカルソート
- 強連結成分分解(SCC)
- 最小共通祖先(LCA)


### 幾何学(位置関係や射影・距離など)
- 点(ベクトル)
- 直線
- 円
- ポリゴン
  - 凸性判定
  - 凸包
  - 直径計算


### DP系
- LCS(最長共通部分列)
- LIS / LDSの長さ


### グラフネットワーク
- 最大流
- 最小費用流
- 最大二部マッチング

### 探索
- 二分探索
- 三分探索

### ソート
- **スターリンソート**


## 使い方
ライブラリ間の依存は無い（必要な場合は埋め込んでいる）ので、必要なclassだけをコピーしてください。

## TODO
- 埋め込まれているライブラリが一部古いので、更新する
- Testとか欲しい（けれど、大抵提出してverifyした気になっちゃう）
- ファイルやclassの分割をもうちょっと考える
- 典型問題は増やすか完全に消すかする
- verifyしてfixされたライブラリはスニペット化したいね
- 三分探索は黄金比パターンとかあるらしい
- ロリハ
  - Baseはランダム化する
  - [安全で爆速なRollingHashの話](https://qiita.com/keymoon/items/11fac5627672a6d6a9f6)
- 対応アルゴリズムがやっつけ感すごい……


