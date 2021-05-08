using System;
using System.Linq;

/// <summary>
/// RSQ: https://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=DSL_2_B
/// MOQ: https://atcoder.jp/contests/arc033/tasks/arc033_3
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 区間内総和
    /// Binary Indexed Tree(BIT)による
    /// </summary>
    /// <remarks>
    /// BITなので内部的なindexは1-basedだが、
    /// intefaceとしては0-basedで統一している
    /// </remarks>
    public class RangeSumQuery
    {
        public int Count { get; }

        private readonly long[] _data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        /// <param name="v">ノードの初期値</param>
        public RangeSumQuery(int n, long v = 0)
        {
            _data = Enumerable.Repeat(v, n + 1).ToArray();
            Count = n;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="v">加算する値</param>
        public void Add(int index, int v)
        {
            // 内部的には1-basedで扱う
            index++;

            while (index < _data.Length)
            {
                _data[index] += v;
                index += Lsb(index);
            }
        }

        /// <summary>
        /// [0,r)の総和
        /// </summary>
        /// <param name="r">index</param>
        /// <returns></returns>
        public long Sum(int r)
        {
            long sum = 0;
            while (r > 0)
            {
                sum += _data[r];
                r -= Lsb(r);
            }

            return sum;
        }

        /// <summary>
        /// [l,r)の総和
        /// </summary>
        /// <param name="l">[l</param>
        /// <param name="r">r)</param>
        /// <returns></returns>
        public long Sum(int l, int r)
        {
            var suma = Sum(l);
            var sumb = Sum(r);

            return sumb - suma;
        }

        /// <summary>
        /// LSBだけがたった値を返す
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <remarks>
        /// 元の値と正負反転した値のANDを取ることで取得できる
        /// これは負数に2の補数表現を使っているため。
        /// 
        /// 下記を考える
        /// - 4bit環境
        /// - 負数は2の補数で表現する
        /// 
        /// 仮にx=6とすると
        /// +6=0b0110
        /// -6=0b1010
        /// 
        /// (+6)&(-6)=0b0010
        /// </remarks>
        private static int Lsb(int x) { return x & -x; }
    }

    /// <summary>
    /// X番目に小さい値の探索・削除
    /// </summary>
    /// <remarks>
    /// Binary Indexed Tree(BIT)による
    /// 入力は同値を含まない数列
    /// </remarks>
    public class MinOrderQuery
    {
        private readonly RangeSumQuery _rangeSumQuery;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">節点数</param>
        public MinOrderQuery(int n)
        {
            _rangeSumQuery = new RangeSumQuery(n);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">index</param>
        public void Register(int index)
            => _rangeSumQuery.Add(index, 1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">index</param>
        public void Unregister(int index)
            => _rangeSumQuery.Add(index, -1);

        /// <summary>
        /// x番目に小さい値を返す
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        /// <remarks>
        /// 1番目に小さい値が最も小さい値である
        /// </remarks>
        public int Find(int x)
        {
            // 0-basedと勘違いしたときに分かりやすいように例外とする
            if (x == 0) { throw new ArgumentOutOfRangeException(nameof(x), $"{nameof(x)} is an integerof 1 or more."); }

            int low = 0, high = _rangeSumQuery.Count;
            while (low < high)
            {
                var mid = (low + high) / 2;
                var sum = _rangeSumQuery.Sum(mid);
                if (sum >= x) { high = mid; }
                else { low = mid + 1; }
            }

            return low;
        }

        /// <summary>
        /// 区間内総和
        /// Binary Indexed Tree(BIT)による
        /// </summary>
        /// <remarks>
        /// BITなので内部的なindexは1-basedだが、
        /// intefaceとしては0-basedで統一している
        /// </remarks>
        private class RangeSumQuery
        {
            public int Count { get; }

            private readonly long[] _data;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="n">ノード数</param>
            /// <param name="v">ノードの初期値</param>
            public RangeSumQuery(int n, long v = 0)
            {
                _data = Enumerable.Repeat(v, n + 1).ToArray();
                Count = n;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="index">index</param>
            /// <param name="v">加算する値</param>
            public void Add(int index, int v)
            {
                // 内部的には1-basedで扱う
                index++;

                while (index < _data.Length)
                {
                    _data[index] += v;
                    index += Lsb(index);
                }
            }

            /// <summary>
            /// [0,r)の総和
            /// </summary>
            /// <param name="r">index</param>
            /// <returns></returns>
            public long Sum(int r)
            {
                long sum = 0;
                while (r > 0)
                {
                    sum += _data[r];
                    r -= Lsb(r);
                }

                return sum;
            }

            /// <summary>
            /// [l,r)の総和
            /// </summary>
            /// <param name="l">[l</param>
            /// <param name="r">r)</param>
            /// <returns></returns>
            public long Sum(int l, int r)
            {
                var suma = Sum(l);
                var sumb = Sum(r);

                return sumb - suma;
            }

            /// <summary>
            /// LSBだけがたった値を返す
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            /// <remarks>
            /// 元の値と正負反転した値のANDを取ることで取得できる
            /// これは負数に2の補数表現を使っているため。
            /// 
            /// 下記を考える
            /// - 4bit環境
            /// - 負数は2の補数で表現する
            /// 
            /// 仮にx=6とすると
            /// +6=0b0110
            /// -6=0b1010
            /// 
            /// (+6)&(-6)=0b0010
            /// </remarks>
            private static int Lsb(int x) { return x & -x; }
        }
    }
}
