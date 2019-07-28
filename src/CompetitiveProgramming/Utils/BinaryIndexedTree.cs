using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 区間内総和
    /// Binary Indexed Tree(BIT)による
    /// </summary>
    class RangeSumQuery
    {
        readonly int N;
        readonly int[] Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">節点数</param>
        /// <param name="v">ノードの初期値</param>
        public RangeSumQuery(int n, int v = 0)
        {
            N = n + 1;
            this.Data = Enumerable.Repeat(v, N).ToArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">index(1-based)</param>
        /// <param name="v">加算する値</param>
        public void Add(int k, int v)
        {
            while (k < Data.Length)
            {
                Data[k] += v;
                k += k & -k;
            }
        }

        /// <summary>
        /// [0,k)の総和
        /// </summary>
        /// <param name="k">index(1-based)</param>
        /// <returns></returns>
        public long Sum(int k)
        {
            long sum = 0;
            while (k > 0)
            {
                sum += Data[k];
                k -= k & -k;
            }

            return sum;
        }

        /// <summary>
        /// [a,b)の総和
        /// [a]+[a+1]+...[r-1]
        /// </summary>
        /// <param name="l"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public long Sum(int l, int r)
        {
            var suma = Sum(l - 1);
            var sumb = Sum(r);

            return sumb - suma;
        }
    }

    /// <summary>
    /// X番目に小さい値の探索・削除
    /// https://atcoder.jp/contests/arc033/tasks/arc033_3
    /// Binary Indexed Tree(BIT)による
    /// </summary>
    class MinOrderQuery
    {
        readonly int N;
        readonly int[] Data;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">節点数</param>
        /// <param name="v">ノードの初期値</param>
        public MinOrderQuery(int n, int v = 0)
        {
            N = n + 1;
            Data = Enumerable.Repeat(v, N).ToArray();
        }

        /// <summary>
        /// kにvを足す
        /// </summary>
        /// <param name="k"></param>
        /// <param name="v"></param>
        void Add(int k, int v)
        {
            while (k < Data.Length)
            {
                Data[k] += v;
                k += k & -k;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="k">index(1-based)</param>
        public void Add(int k)
            => Add(k, 1);

        public void Remove(int k)
            => Add(k, -1);

        /// <summary>
        /// [1,k)の総和
        /// </summary>
        /// <param name="k">index(1-based)</param>
        /// <returns></returns>
        public long Sum(int k)
        {
            long sum = 0;
            while (k > 0)
            {
                sum += Data[k];
                k -= k & -k;
            }

            return sum;
        }

        /// <summary>
        /// [b,e)の総和
        /// [b]+[b+1]+...[e-1]
        /// </summary>
        /// <param name="b"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public long Sum(int b, int e)
        {
            var suma = Sum(b - 1);
            var sumb = Sum(e);

            return sumb - suma;
        }

        /// <summary>
        /// x番目に小さい値を返す
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public int Find(int x)
        {
            int low = 0, high = N;
            while (low < high)
            {
                var mid = (low + high) / 2;
                var sum = Sum(mid);
                if (sum >= x) { high = mid; }
                else { low = mid + 1; }
            }

            return low;
        }
    }
}
