using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// あんまりズバリなverify用問題が無い
/// https://atcoder.jp/contests/abc112/tasks/abc112_d
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 約数列挙
    /// </summary>
    static class Divisor
    {
        /// <summary>
        /// 約数列挙 順番はバラバラなので注意！
        /// </summary>
        private static IEnumerable<long> Divisors(long n)
        {
            if (n < 1) yield break;

            var rn = (int)(Math.Sqrt(n) + 1);
            for (long i = 1; i < rn; i++)
            {
                if ((n % i) == 0)
                {
                    yield return i;

                    if (i != (n / i)) yield return n / i;
                }
            }
        }

        /// <summary>
        /// 約数列挙
        /// </summary>
        /// <param name="n"></param>
        /// <param name="needsSort">昇順ソートするか</param>
        /// <returns></returns>
        public static long[] Divisors(long n, bool needsSort = false)
        {
            var divs = Divisors(n).ToArray();

            if (needsSort) Array.Sort(divs);

            return divs;
        }
    }
}
