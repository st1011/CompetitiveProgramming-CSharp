using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大公約数
    /// 最小公倍数
    /// 拡張ユークリッド互除法
    /// nPm, nCr
    /// </summary>
    static class Number
    {
        /// <summary>
        /// 最大公約数
        /// ユークリッド互除法
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
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
        /// <param name="nums"></param>
        /// <returns></returns>
        public static long Gcd(IEnumerable<long> nums)
            => nums.Aggregate((n, next) => n = Gcd(n, next));

        public static int Gcd(IEnumerable<int> nums)
            => (int)Gcd(nums.Select(x => (long)x));

        /// <summary>
        /// 最小公倍数
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static long Lcm(long a, long b)
            => a / Gcd(a, b) * b;

        /// <summary>
        /// 最小公倍数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static long Lcm(IEnumerable<long> nums)
            => nums.Aggregate((n, next) => n = Lcm(n, next));

        /// <summary>
        /// 最小公倍数
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public static long Lcm(IEnumerable<int> nums)
            => Lcm(nums.Select(x => (long)x));

        /// <summary>
        /// 拡張ユークリッドの互除法
        /// ax+by = gcd(a,b)の整数解
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
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

        /// <summary>
        /// ルジャンドルの公式
        /// n!をpで何回割り切れるか
        /// </summary>
        /// <param name="n">正の整数</param>
        /// <param name="p">素数</param>
        /// <returns></returns>
        public static long Legendres(long n, long p)
        {
            long count = 0;
            for (long m = p; m < n; m *= p)
            {
                count += n / m;
            }

            return count;
        }

        /// <summary>
        /// n!
        /// 12! < int(2^31 - 1) < 13!
        /// 20! < long(2^63 - 1) < 21!
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long Factorial(long n)
        {
            var m = 1L;

            for (long i = 2; i <= n; i++)
            {
                m *= i;
            }

            return m;
        }

        /// <summary>
        /// nPr
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static long Permutation(long n, long r)
        {
            var m = n;

            for (long i = n - 1; i > n - r; i--)
            {
                m *= i;
            }

            return m;
        }

        /// <summary>
        /// nCr
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static long Combination(long n, long r)
        {
            return Permutation(n, r) / Factorial(r);
        }
    }
}
