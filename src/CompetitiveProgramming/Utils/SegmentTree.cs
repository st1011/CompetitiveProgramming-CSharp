using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// セグメント木
    /// 点代入・区間演算系
    /// </summary>
    class SegTree<T>
    {
        readonly int N;
        readonly T[] Nodes;
        readonly Func<T, T, T> Monoid;
        readonly T InitValue;
        readonly T IgnorableValue;

        /// <summary>
        /// MinQueryの場合: (n, Math.Min, int.MaxValue)
        /// MaxQueryの場合: (n, Math.Max, int.MinValue)
        /// SumQueryの場合: (n, (x,y)=> x+y, 0)
        /// </summary>
        /// <param name="n">ノード数</param>
        /// <param name="monoid">モノイド</param>
        /// <param name="initValue">初期値</param>
        /// <param name="ignorableValue">無視できる値(MinQueryならMaxValueとか)</param>
        public SegTree(int n, Func<T, T, T> monoid, T initValue, T ignorableValue)
        {
            // 2のべき乗まで切り上げる
            N = 1;
            while (N < n) N *= 2;

            Monoid = monoid;
            InitValue = initValue;
            IgnorableValue = ignorableValue;

            Nodes = Enumerable.Repeat(InitValue, 2 * N - 1).ToArray();
        }

        /// <summary>
        /// 初期値付きの初期化
        /// </summary>
        /// <param name="monoid"></param>
        /// <param name="initValue"></param>
        /// <param name="ignorableValue"></param>
        /// <param name="ie"></param>
        public SegTree(Func<T, T, T> monoid, T initValue, T ignorableValue, IEnumerable<T> ie)
            : this(ie.Count(), monoid, initValue, ignorableValue)
        {
            var n = ie.Count();

            // 実データの上書き
            for (int i = 0; i < n; i++)
            {
                Nodes[i + N - 1] = ie.ElementAt(i);
            }

            // 実データ以外の箇所を更新していく
            for (int i = N - 2; i >= 0; i--)
            {
                Nodes[i] = Monoid(Nodes[2 * i + 1], Nodes[2 * i + 2]);
            }
        }

        /// <summary>
        /// k番目の要素をvに変更
        /// </summary>
        /// <param name="k">index(0-based)</param>
        /// <param name="v">更新する値</param>
        public void Update(int k, T v)
        {
            // 該当の最下段index
            k += N - 1;

            Nodes[k] = v;
            while (k > 0)
            {
                // 親ノードを一つずつ更新していく
                k = (k - 1) / 2;
                Nodes[k] = Monoid(Nodes[2 * k + 1], Nodes[2 * k + 2]);
            }
        }

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="k">index(0-based)</param>
        /// <param name="l">現在の対象区間 [l,</param>
        /// <param name="r">現在の対象区間 ,r)</param>
        /// <returns></returns>
        T Find(int b, int e, int k, int l, int r)
        {
            if (e <= l || b >= r)
            {
                // 対象区間が要求区間外
                return IgnorableValue;
            }
            if (b <= l && e >= r)
            {
                // 対象区間が要求区間に内包されている
                return Nodes[k];
            }

            // 対象区間の一部が要求区間
            var vl = Find(b, e, 2 * k + 1, l, (l + r) / 2);
            var vr = Find(b, e, 2 * k + 2, (l + r) / 2, r);

            return Monoid(vl, vr);
        }

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public T Find(int b, int e)
            => Find(b, e, 0, 0, N);

        /// <summary>
        /// k番目の要素の取得
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public T Peek(int k)
            => Nodes[k + N - 1];

        // メモ
        // (k-1)/2が親ノード
        // k*2+(1 or 2)が子ノード
        // 最下段が実際のデータを表していて、それより上にはN-2ノードがある
        // よって、k+N-1で最下段ノード（実際のノード）を指す
    }

    /// <summary>
    /// 遅延伝播セグメント木（Lazy Propagation Segment Tree）
    /// 区間加算とかだと遅延伝播用のデータが大きくなりがちなので、long推奨
    /// </summary>
    class LazySegTree<T>
    {
        readonly int N;
        readonly T[] Nodes;
        readonly T[] Lazy;
        readonly bool[] NeedsEval;

        #region Example
        /// <summary>
        /// 区間更新・区間最小値
        /// AOJ DSL_2_F
        /// </summary>
        public static LazySegTree<long> CreateRuqRmq(int n)
        {
            // 区間更新なので、伝播時は上書きで良い
            return new LazySegTree<long>(n,
                Math.Min,
                (x, y) => y,
                (x, y) => y,
                (x, y, z) => y,
                int.MaxValue, long.MaxValue, 0);
        }

        /// <summary>
        /// 区間加算・区間合計
        /// AOJ DSL_2_G
        /// </summary>
        public static LazySegTree<long> CreateRaqRsq(int n)
        {
            // 合計なので、最下位ノードの数だけ乗じて加算する必要がある
            // 下位へ伝播するときも上位ノードを分割して加算する
            return new LazySegTree<long>(n,
                (x, y) => x + y,
                (x, y) => x + y,
                (x, y) => x + y / 2,
                (x, y, z) => x + y * z,
                0, 0, 0);
        }

        /// <summary>
        /// 区間加算・区間最小値
        /// AOJ DSL_2_H
        /// </summary>
        public static LazySegTree<long> CreateRaqRmq(int n)
        {
            // 最小値や最大値を伝播させるので、
            // 伝播用ノードは値がそのまま伝播する
            return new LazySegTree<long>(n,
                Math.Min,
                (x, y) => x + y,
                (x, y) => x + y,
                (x, y, z) => x + y,
                0, long.MaxValue, 0);
        }

        /// <summary>
        /// 区間更新・区間合計
        /// AOJ DSL_2_I
        /// </summary>
        public static LazySegTree<long> CreateRuqRsq(int n)
        {
            // 合計なので、最下位ノードの数を乗じて更新する必要がある
            // 下位へ伝播するときも上位ノードを分割して加算する
            return new LazySegTree<long>(n,
                (x, y) => x + y,
                (x, y) => y,
                (x, y) => y / 2,
                (x, y, z) => y * z,
                0, 0, 0);
        }
        #endregion

        /// <summary>
        /// 末端の演算 MinQueryならMath.Minとか
        /// </summary>
        readonly Func<T, T, T> Monoid;
        /// <summary>
        /// LazyをNodesに伝播するときの演算
        /// </summary>
        readonly Func<T, T, T> UpdateMonoid;
        /// <summary>
        /// Lazyの上から下への伝播
        /// </summary>
        readonly Func<T, T, T> ForwardMonoid;
        /// <summary>
        /// Lazyの下から上への伝播
        /// </summary>
        readonly Func<T, T, int, T> BackwardMonoid;

        readonly T InitValue;
        readonly T IgnorableValue;
        readonly T LazyInitValue;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        /// <param name="monoid">ノード演算 モノイド</param>
        /// <param name="update">遅延評価時、ノードへの伝播方法</param>
        /// <param name="forward">遅延評価時、子ノードへの順伝播方法</param>
        /// <param name="backward">遅延評価時、親ノードへの逆伝播方法</param>
        /// <param name="nodeInitValue">初期値</param>
        /// <param name="ignorableValue">外れ値（MinクエリならMaxValueとか）</param>
        /// <param name="lazyInitValue">遅延評価ノードの初期値</param>
        public LazySegTree(int n,
            Func<T, T, T> monoid, Func<T, T, T> update,
            Func<T, T, T> forward, Func<T, T, int, T> backward,
            T nodeInitValue, T ignorableValue, T lazyInitValue)
        {
            // 2のべき乗まで切り上げる
            N = 1;
            while (N < n) N *= 2;

            Lazy = new T[2 * N - 1];
            NeedsEval = new bool[2 * N - 1];
            Monoid = monoid;
            UpdateMonoid = update;
            ForwardMonoid = forward;
            BackwardMonoid = backward;
            InitValue = nodeInitValue;
            IgnorableValue = ignorableValue;
            LazyInitValue = lazyInitValue;

            Nodes = Enumerable.Repeat(InitValue, 2 * N - 1).ToArray();
        }

        /// <summary>
        /// 実際の遅延伝播
        /// </summary>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        void Eval(int k, int l, int r)
        {
            if (!NeedsEval[k]) return;

            if (r - l > 1)
            {
                // 葉ではないので、伝播を続ける
                Lazy[k * 2 + 1] = ForwardMonoid(Lazy[k * 2 + 1], Lazy[k]);
                Lazy[k * 2 + 2] = ForwardMonoid(Lazy[k * 2 + 2], Lazy[k]);

                NeedsEval[k * 2 + 1] = NeedsEval[k * 2 + 2] = true;
            }

            Nodes[k] = UpdateMonoid(Nodes[k], Lazy[k]);

            NeedsEval[k] = false;
            Lazy[k] = LazyInitValue;
        }

        /// <summary>
        /// [b,e)区間の値の更新
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="v"></param>
        /// <param name="k">[l,r)に対応するindex</param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        void Update(int b, int e, T v, int k, int l, int r)
        {
            Eval(k, l, r);

            if (e <= l || r <= b) { return; }
            if (b <= l && r <= e)
            {
                Lazy[k] = BackwardMonoid(Lazy[k], v, r - l);
                NeedsEval[k] = true;
                Eval(k, l, r);
            }
            else
            {
                Update(b, e, v, k * 2 + 1, l, (l + r) / 2);
                Update(b, e, v, k * 2 + 2, (l + r) / 2, r);
                Nodes[k] = Monoid(Nodes[k * 2 + 1], Nodes[k * 2 + 2]);
            }
        }

        /// <summary>
        /// [b,e)区間の値の更新
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="v"></param>
        public void Update(int b, int e, T v)
            => Update(b, e, v, 0, 0, N);

        /// <summary>
        /// k番目の要素の更新
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        public void Update(int k, T v)
            => Update(k, k + 1, v);

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="k">[l,r)に対応するindex</param>
        /// <param name="l">現在の対象区間 [l,</param>
        /// <param name="r">現在の対象区間 ,r)</param>
        /// <returns></returns>
        T Find(int b, int e, int k, int l, int r)
        {
            Eval(k, l, r);

            if (e <= l || b >= r)
            {
                // 対象区間が要求区間外
                return IgnorableValue;
            }
            if (b <= l && e >= r)
            {
                // 対象区間が要求区間に内包されている
                return Nodes[k];
            }

            // 対象区間の一部が要求区間
            var vl = Find(b, e, k * 2 + 1, l, (l + r) / 2);
            var vr = Find(b, e, k * 2 + 2, (l + r) / 2, r);

            return Monoid(vl, vr);
        }

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public T Find(int b, int e)
            => Find(b, e, 0, 0, N);

        /// <summary>
        /// k番目の要素の結果を求める
        /// </summary>
        /// <param name="k"></param>
        /// <returns></returns>
        public T Find(int k)
            => Find(k, k + 1);

        // メモ
        // (k-1)/2が親ノード
        // k*2+(1 or 2)が子ノード
        // 最下段が実際のデータを表していて、それより上にはN-1ノードがある
        // そのため、k+N-1で最下段ノード（実際のノード）を指す
    }

    /// <summary>
    /// 区間加算と区間総和
    /// セグメント木による
    /// </summary>
    class RangeAddQuery
    {
        readonly int N;
        readonly int[] All;
        readonly int[] Part;

        public long this[int i]
        {
            get { return Sum(i, i + 1); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">節点数</param>
        /// <param name="v">ノードの初期値</param>
        public RangeAddQuery(int n, int v = 0)
        {
            // 2のべき乗まで切り上げる
            N = 1;
            while (N < n) N *= 2;

            this.All = Enumerable.Repeat(v, 2 * N - 1).ToArray();
            this.Part = Enumerable.Repeat(v, 2 * N - 1).ToArray();
        }

        /// <summary>
        /// [b,e)の範囲にvを加算する
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="v"></param>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        void Add(int b, int e, int v, int k, int l, int r)
        {
            if (b <= l && r <= e)
            {
                // [l,r)が[b,e)に内包されていれば、全体に足すだけ
                All[k] += v;
            }
            else if (l < e && b < r)
            {
                // [l,r)が[b,e)と交差している
                Part[k] += (Math.Min(e, r) - Math.Max(b, l)) * v;
                Add(b, e, v, k * 2 + 1, l, (l + r) / 2);
                Add(b, e, v, k * 2 + 2, (l + r) / 2, r);
            }
        }


        /// <summary>
        /// [b,e)の範囲にvを加算する
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="v"></param>
        public void Add(int b, int e, int v)
            => Add(b, e, v, 0, 0, N);

        /// <summary>
        /// [b,e)の総和を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <param name="k">index(0-based)</param>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        long Sum(int b, int e, int k, int l, int r)
        {
            // [a,b)と[l,r)が交差していない
            if (e <= l || r <= b)
            {
                return 0;
            }
            else if (b <= l && r <= e)
            {
                // 内包している
                return All[k] * (r - l) + Part[k];
            }
            else
            {
                // 交差
                long sum = (Math.Min(e, r) - Math.Max(b, l)) * All[k];
                sum += Sum(b, e, k * 2 + 1, l, (l + r) / 2);
                sum += Sum(b, e, k * 2 + 2, (l + r) / 2, r);

                return sum;
            }
        }

        /// <summary>
        /// [b,e)の総和を求める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public long Sum(int b, int e)
            => Sum(b, e, 0, 0, N);

        /// <summary>
        /// 全体の総和
        /// </summary>
        /// <returns></returns>
        public long Sum()
            => Sum(0, N);
    }
}
