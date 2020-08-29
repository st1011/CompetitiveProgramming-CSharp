using System;
using System.Collections.Generic;

/// <summary>
/// Length: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=ALDS1_10_C&lang=ja
/// Restore: https://atcoder.jp/contests/dp/tasks/dp_f
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最長共通部分列
    /// </summary>
    public class LongestCommonSubsequence<T> where T : IEquatable<T>
    {
        public IReadOnlyList<T> X { get; private set; }
        public IReadOnlyList<T> Y { get; private set; }
        public int Length { get; }

        private readonly int[,] _dp;
        private IReadOnlyList<T> _lcs;

        public LongestCommonSubsequence(IReadOnlyList<T> x, IReadOnlyList<T> y)
        {
            if (x == null) { throw new ArgumentNullException(nameof(x)); }
            if (y == null) { throw new ArgumentNullException(nameof(y)); }

            X = x;
            Y = y;

            // 計算まで済ませておく
            _dp = LcsDP(X, Y);
            Length = _dp[X.Count, y.Count];
        }

        /// <summary>
        /// LCSのテーブルを計算する
        /// </summary>
        private int[,] LcsDP(IReadOnlyList<T> x, IReadOnlyList<T> y)
        {
            var m = x.Count;
            var n = y.Count;

            var dp = new int[m + 1, n + 1];
            for (int i = 1; i <= m; i++)
            {
                for (int j = 1; j <= n; j++)
                {
                    if (x[i - 1].Equals(y[j - 1]))
                    {
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            return dp;
        }

        /// <summary>
        /// LCSの計算用テーブルからLCSを復元する
        /// </summary>
        private IReadOnlyList<T> RestoreLcs(IReadOnlyList<T> x, int[,] l)
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

            return stack.ToArray();
        }

        /// <summary>
        /// LCSを取得
        /// </summary>
        public IReadOnlyList<T> GetLcs()
        {
            if (_lcs == null)
            {
                _lcs = RestoreLcs(X, _dp);
            }

            return _lcs;
        }
    }
}
