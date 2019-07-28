using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 素数判定
    /// 素因数分解
    /// 素数列挙（エラトステネスの篩）
    /// 約数列挙
    /// </summary>
    static class Prime
    {
        /// <summary>
        /// 素数か
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static bool IsPrime(long x)
        {
            if (x < 2)
            {
                return false;
            }
            if (x == 2)
            {
                return true;
            }
            if ((x % 2) == 0)
            {
                return false;
            }

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
        /// <param name="n"></param>
        /// <returns></returns>
        public static bool[] PrimeSieve(int n)
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
        /// 素数リスト
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<int> PrimeList(int n)
        {
            var sieve = PrimeSieve(n);

            return sieve.Select((x, i) => new { X = x, I = i })
                .Where(x => x.X)
                .Select(x => x.I);
        }

        /// <summary>
        /// 素因数分解
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<long> PrimeFactors(long n)
        {
            var m = n;
            var m2 = (long)Math.Sqrt(m) + 1;
            var p = 2;

            while (p < m2)
            {
                if ((m % p) == 0)
                {
                    m /= p;
                    m2 = (long)Math.Sqrt(m) + 1;
                    yield return p;
                }
                else
                {
                    p++;
                }
            }
            if (m > 1)
            {
                yield return m;
            }
        }

        /// <summary>
        /// n以下でnと互いに素な数の個数
        /// オイラーのファイ関数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long PrimeCount(long n)
        {
            var primes = PrimeFactors(n).Distinct();

            var dsum = new decimal(1);
            foreach (var p in primes)
            {
                var pp = new decimal(p);

                dsum *= 1 - (1 / pp);
            }

            return (long)Math.Round(dsum * n);
        }

        /// <summary>
        /// 約数列挙 順番はバラバラなので注意！
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static IEnumerable<long> Divisors(long n)
        {
            if (n < 1)
            {
                yield break;
            }

            var rn = (int)(Math.Sqrt(n) + 1);
            for (long i = 1; i < rn; i++)
            {
                if ((n % i) == 0)
                {
                    yield return i;

                    if (i != (n / i))
                    {
                        yield return n / i;
                    }
                }
            }
        }

        /// <summary>
        /// 約数列挙 昇順にソート
        /// </summary>
        /// <param name="n"></param>
        /// <param name="needsSort"></param>
        /// <returns></returns>
        public static IEnumerable<long> Divisors(long n, bool needsSort)
        {
            var divs = Divisors(n);

            if (needsSort)
            {
                return divs.OrderBy(x => x);
            }
            else
            {
                return divs;
            }
        }
    }
}
