using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://atcoder.jp/contests/abc141/tasks/abc141_e
/// https://judge.yosupo.jp/problem/zalgorithm
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// sとs[i:]の最長共通接頭辞の長さを記録した配列を作成する
    /// </summary>
    /// <remarks>
    /// 構築 O(|S|)
    /// </remarks>
    static class ZAlgorithm
    {
        /// <summary>
        /// LCP(0, i)を求める
        /// </summary>
        public static int[] Lcp(string s)
        {
            var n = s.Length;
            var lcp = new int[n];

            if (n == 0) { return lcp; }

            lcp[0] = n;
            int i = 1;
            int j = 0;
            while (i < n)
            {
                while (i + j < n && s[j] == s[i + j])
                {
                    j++;
                }
                lcp[i] = j;
                if (j == 0)
                {
                    i++;
                    continue;
                }

                int k = 1;
                while (i + k < n && k + lcp[k] < j)
                {
                    lcp[i + k] = lcp[k];
                    k++;
                }

                i += k;
                j -= k;
            }

            return lcp;
        }
    }
}
