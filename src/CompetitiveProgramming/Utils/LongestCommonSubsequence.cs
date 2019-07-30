using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最長共通部分列
    /// </summary>
    class LongestCommonSubsequence<T> where T : IComparable<T>
    {
        public IList<T> X { get; private set; }
        public IList<T> Y { get; private set; }
        public IList<T> Lcs { get; set; }
        int[,] Dp { get; set; }

        public int Length { get { return Dp[X.Count(), Y.Count()]; } }

        public LongestCommonSubsequence(IList<T> x, IList<T> y)
        {
            X = x;
            Y = y;

            // 計算まで済ませておく
            LcsDP(X, Y);
            // 実際の部分列は使わないかもしれないので、保留
            Lcs = null;
        }

        /// <summary>
        /// LCSのテーブルを計算する
        /// </summary>
        public void LcsDP(IList<T> x, IList<T> y)
        {
            var m = x.Count();
            var n = y.Count();

            Dp = new int[m + 1, n + 1];
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (x[i - 1].CompareTo(y[j - 1]) == 0)
                    {
                        Dp[i, j] = Dp[i - 1, j - 1] + 1;
                    }
                    else if (Dp[i - 1, j] >= Dp[i, j - 1])
                    {
                        Dp[i, j] = Dp[i - 1, j];
                    }
                    else
                    {
                        Dp[i, j] = Dp[i, j - 1];
                    }
                }
            }
        }

        /// <summary>
        /// LCSの計算用テーブルからLCSを復元する
        /// </summary>
        void RestoreLcs(IList<T> x, int[,] l)
        {
            var i = l.GetLength(0) - 1;
            var j = l.GetLength(1) - 1;

            var stack = new Stack<T>();
            while (i > 0 && j > 0)
            {
                if (l[i, j] == l[i, j - 1])
                {
                    j--;
                }
                else if (l[i, j] == l[i - 1, j])
                {
                    i--;
                }
                else
                {
                    i--;
                    j--;
                    stack.Push(x[i]);
                }
            }

            Lcs = stack.ToArray();
        }

        /// <summary>
        /// LCSを取得
        /// </summary>
        /// <returns></returns>
        public IList<T> GetLcs()
        {
            if (Lcs == null) RestoreLcs(X, Dp);

            return Lcs;
        }
    }
}
