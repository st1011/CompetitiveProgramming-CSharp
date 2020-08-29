using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// GCD / LCM: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=0005&lang=ja
/// ExGCD: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_E&lang=ja
/// 
/// 約数列挙: https://atcoder.jp/contests/abc112/tasks/abc112_d
/// (あんまりズバリなverify用問題が無い)
/// 
/// 素数判定: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=ALDS1_1_C&lang=jp
/// 素因数分解: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_A&lang=jp
/// 素数列挙: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=0044
/// オイラーのファイ関数: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_D&lang=jp
/// ルジャンドルの公式: 
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大公約数
    /// 最小公倍数
    /// 拡張ユークリッド互除法
    /// 
    /// 約数列挙
    /// 
    /// 素数判定
    /// 素因数分解
    /// 素数列挙（エラトステネスの篩）
    /// </summary>
    public static class MathEx
    {
        #region GCD / LCM
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
        #endregion

        #region 約数列挙
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
        #endregion

        #region 素数
        /// <summary>
        /// 素数判定
        /// </summary>
        public static bool IsPrime(long x)
        {
            if (x < 2) return false;
            if (x == 2) return true;
            if ((x % 2) == 0) return false;

            long rx = (long)(Math.Sqrt(x) + 1);
            for (long n = 3; n < rx; n += 2)
            {
                if ((x % n) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 素数判定リスト
        /// </summary>
        public static bool[] Sieve(int n)
        {
            // Sieve of Eratosthenes
            var sieve = Enumerable.Repeat<bool>(true, n + 1).ToArray();

            if (n > 2)
            {
                sieve[0] = sieve[1] = false;
            }

            var rn = (int)(Math.Sqrt(n) + 1);
            foreach (var i in Enumerable.Range(2, rn - 2))
            {
                if (sieve[i])
                {
                    for (int j = 2 * i; j <= n; j += i)
                    {
                        sieve[j] = false;
                    }
                }
            }

            return sieve;
        }

        /// <summary>
        /// n以下の素数リスト
        /// </summary>
        public static IEnumerable<int> GetPrimeList(int n)
        {
            var sieve = Sieve(n);

            return sieve.Select((x, i) => new { X = x, I = i })
                .Where(x => x.X)
                .Select(x => x.I);
        }

        /// <summary>
        /// 素因数分解
        /// 素数(Key)がValue個ある
        /// </summary>
        public static Dictionary<long, int> Factorization(long n)
        {
            var map = new Dictionary<long, int>();
            for (long p = 2; p * p <= n; p++)
            {
                int count = 0;
                while (n % p == 0)
                {
                    count++;
                    n /= p;
                }

                if (count != 0) map.Add(p, count);
            }

            if (n > 1) map.Add(n, 1);

            return map;
        }

        /// <summary>
        /// 素因数分解
        /// 素数をならした配列
        /// </summary>
        public static long[] FactorizationFlat(long n)
            => Factorization(n)
            .SelectMany(x => Enumerable.Repeat(x.Key, x.Value))
            .ToArray();

        /// <summary>
        /// nと互いに素でn以下である自然数の個数
        /// オイラーのファイ関数
        /// </summary>
        public static long CoprimeCount(long n)
        {
            var primes = Factorization(n);

            return primes.Aggregate(n, (t, u) => t - t / u.Key);
        }

        /// <summary>
        /// ルジャンドルの公式
        /// n!をpで何回割り切れるか(pは素数)
        /// </summary>
        public static long Legendres(long n, long p)
        {
            long count = 0;
            for (long m = p; m < n; m *= p)
            {
                count += n / m;
            }

            return count;
        }
        #endregion
    }
}
