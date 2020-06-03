using System.Collections.Generic;
using System.Linq;

/// <summary>
/// GCD\LCM: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=0005&lang=ja
/// ExGCD: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_E&lang=ja
/// 
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大公約数
    /// 最小公倍数
    /// 拡張ユークリッド互除法
    /// </summary>
    static class GcdLcm
    {
        /// <summary>
        /// 最大公約数
        /// ユークリッド互除法
        /// </summary>
        public static long Gcd(long a, long b)
        {
            long r;

            while ((r = a % b) != 0)
            {
                a = b;
                b = r;
            }

            return b;
        }

        /// <summary>
        /// 最大公約数
        /// </summary>
        public static long Gcd(IEnumerable<long> nums)
            => nums.Aggregate((n, next) => n = Gcd(n, next));

        /// <summary>
        /// 最大公約数
        /// </summary>
        public static int Gcd(IEnumerable<int> nums)
            => (int)Gcd(nums.Select(x => (long)x));

        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static long Lcm(long a, long b)
            => a / Gcd(a, b) * b;

        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static long Lcm(IEnumerable<long> nums)
            => nums.Aggregate((n, next) => n = Lcm(n, next));

        /// <summary>
        /// 最小公倍数
        /// </summary>
        public static long Lcm(IEnumerable<int> nums)
            => Lcm(nums.Select(x => (long)x));

        /// <summary>
        /// 拡張ユークリッドの互除法
        /// ax+by = gcd(a,b)の整数解
        /// </summary>
        public static void ExtendedGcd(long a, long b, out long x, out long y)
        {
            long u = 0, v = 1;
            x = 1; y = 0;

            while (b > 0)
            {
                long q = a / b;
                long t;

                t = u;
                u = x - q * u;
                x = t;

                t = v;
                v = y - q * v;
                y = t;

                t = b;
                b = a - q * b;
                a = t;
            }
        }
    }
}
