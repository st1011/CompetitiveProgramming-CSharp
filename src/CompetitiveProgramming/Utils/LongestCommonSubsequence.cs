using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    class LongestCommonSubsequence
    {
        /// <summary>
        /// LCSの途中結果から実際の文字列を復元する
        /// </summary>
        /// <param name="x"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        static string RestoreLcdString(string x, int[,] l)
        {
            var i = l.GetLength(0) - 1;
            var j = l.GetLength(1) - 1;

            var stack = new Stack<char>();
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

            return new string(stack.ToArray());
        }

        /// <summary>
        /// 最長共通部分列を取得する
        /// x:"abc" y:"axbc", return "abc"みたいなの
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static string Lcs(string x, string y)
        {
            var m = x.Length;
            var n = y.Length;
            var c = new int[m + 1, n + 1];

            foreach (var i in Enumerable.Range(1, m))
            {
                foreach (var j in Enumerable.Range(1, n))
                {
                    if (x[i - 1] == y[j - 1])
                    {
                        c[i, j] = c[i - 1, j - 1] + 1;
                    }
                    else if (c[i - 1, j] >= c[i, j - 1])
                    {
                        c[i, j] = c[i - 1, j];
                    }
                    else
                    {
                        c[i, j] = c[i, j - 1];
                    }
                }
            }

            // 文字列を復元する
            return RestoreLcdString(x, c);
        }
    }
}
