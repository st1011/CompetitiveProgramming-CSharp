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
///           https://yukicoder.me/problems/no/3030
/// 素数の個数: https://judge.yosupo.jp/problem/counting_primes
/// 素因数分解: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_A&lang=jp
/// 素数列挙: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=0044
///           https://judge.yosupo.jp/problem/enumerate_primes
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
    /// 素数の個数
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
        #region 素数判定
        /// <summary>
        /// 素数判定
        /// </summary>
        public static bool IsPrime(ulong n)
        {
            // 小さい数の時は試し割が早い気がするので分岐しておく
            // ただししきい値は適当……
            if (n < 100)
            {
                return IsPrimeTrialDiv(n);
            }
            else
            {
                return IsPrimeMillerRabin(n);
            }
        }

        /// <summary>
        /// 素数判定(試し割)
        /// </summary>
        private static bool IsPrimeTrialDiv(ulong n)
        {
            if (n <= 2) return n == 2;
            if ((n % 2) == 0) return false;

            ulong rn = (ulong)(Math.Sqrt(n) + 1);
            for (ulong m = 3; m < rn; m += 2)
            {
                if ((n % m) == 0)
                {
                    return false;
                }
            }

            return true;
        }

        private static readonly int[] _millerRabinWitness32 = new int[] { 2, 7, 61 };
        private static readonly int[] _millerRabinWitness64 = new int[] { 2, 325, 9375, 28178, 450775, 9780504, 1795265022 };

        /// <summary>
        /// 素数判定
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <remarks>
        /// ミラー・ラビンによる
        /// 本来は確率的アルゴリズムだが任意の範囲においては
        /// 対応する数列すべてにクリアすれば素数と見なせる
        /// 2^32 <=> {2, 7, 61}
        /// 2^64 <=> {2, 325, 9375, 28178, 450775, 9780504, 1795265022}
        /// 
        /// 内部処理の関係上、本当に2^64とか極端に大きな数は計算できないことに注意
        /// </remarks>
        private static bool IsPrimeMillerRabin(ulong n)
        {
            if (n >= 1ul << 63)
            {
                throw new ArgumentOutOfRangeException(nameof(n));
            }

            if (n <= 2) return n == 2;
            if ((n % 2) == 0) return false;

            ulong n1 = n - 1;
            ulong d = n1 / 2;
            while ((d & 1) == 0)
            {
                d /= 2;
            }

            var witness = ((n >> 32) != 0) ? _millerRabinWitness64 : _millerRabinWitness32;
            foreach (ulong m in witness)
            {
                if (m % n == 0) { continue; }

                var t = d;
                var r = ModPower(m, t, n);
                while (t != n1 && r != 1 && r != n1)
                {
                    r = ModPower(r, 2, n);
                    t *= 2;
                }

                if (r != n1 && (t & 1) == 0) { return false; }
            }

            return true;
        }
        #endregion

        /// <summary>
        /// n以下の素数判定リスト
        /// </summary>
        /// <remarks>
        /// アトキンの篩
        /// 偶数を除いたエラトステネスの方が早いかも？
        /// </remarks>
        public static IReadOnlyList<bool> Sieve(int n)
        {
            var sieve = new bool[n + 1];
            var rn = (int)(Math.Sqrt(n) + 1);
            for (int i = 1; i <= 5; i += 4)
            {
                for (int j = i; j <= rn; j += 6)
                {
                    for (int k = 1; k <= rn; k++)
                    {
                        var t = 4 * k * k + j * j;
                        if (t > n) { break; }
                        sieve[t] = !sieve[t];
                    }
                    for (int k = j + 1; k <= rn; k += 2)
                    {
                        var t = 3 * k * k - j * j;
                        if (t > n) { break; }
                        sieve[t] = !sieve[t];
                    }
                }
            }
            for (int i = 2; i <= 4; i += 2)
            {
                for (int j = i; j <= rn; j += 6)
                {
                    for (int k = 1; k <= rn; k += 2)
                    {
                        var t = 3 * k * k + j * j;
                        if (t > n) { break; }
                        sieve[t] = !sieve[t];
                    }
                    for (int k = j + 1; k <= rn; k += 2)
                    {
                        var t = 3 * k * k - j * j;
                        if (t > n) { break; }
                        sieve[t] = !sieve[t];
                    }
                }
            }
            for (int i = 3; i <= rn; i += 6)
            {
                for (int j = 1; j <= 2; j++)
                {
                    for (int k = j; k <= rn; k += 3)
                    {
                        var t = 4 * k * k + i * i;
                        if (t > n) { break; }
                        sieve[t] = !sieve[t];
                    }
                }
            }
            for (int i = 5; i <= rn; i++)
            {
                if (sieve[i])
                {
                    for (int j = i * i; j <= n; j += i * i)
                    {
                        sieve[j] = false;
                    }
                }
            }

            for (int i = 2; i <= 3; i++)
            {
                if (i <= n) { sieve[i] = true; }
            }

            return sieve;
        }

        /// <summary>
        /// n以下の素数リスト
        /// </summary>
        public static IReadOnlyList<int> GetPrimeList(int n)
        {
            var sieve = Sieve(n);
            var list = new List<int>();
            for (int i = 0; i < sieve.Count; i++)
            {
                if (sieve[i]) { list.Add(i); }
            }

            return list;
        }

        /// <summary>
        /// n以下の素数の個数
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        /// <remarks>
        /// http://hidnoblog.blog46.fc2.com/?m&no=1449
        /// </remarks>
        public static long CountPrime(long n)
        {
            int rn = (int)Math.Sqrt(n);
            var pp = GetPrimeList(rn + 1).ToArray();

            long len = rn + n / (rn + 1) + 1;
            var dp = new long[len + 1];

            for (int i = 1; i <= rn; i++)
            {
                dp[i] = n / i - 1;
            }
            for (int i = rn + 1; i < len; i++)
            {
                dp[i] = len - i - 1;
            }

            for (int i = 0; i < pp.Length; i++)
            {
                for (int j = 1; j < len; j++)
                {
                    var t = (j <= rn ? n / j : (len - j)) / pp[i];
                    if (t < pp[i]) { break; }

                    dp[j] -= dp[t > rn ? n / t : len - t] - dp[len - (pp[i] - 1)];
                }
            }

            // dp[i]: n以下の素数をiで除した数
            return dp[1];
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

        #region Misc
        /// <summary>
        /// Mを法とする乗算
        /// </summary>
        private static ulong ModMul(ulong a, ulong b, ulong M)
        {
            a %= M;
            b %= M;
            ulong r = 0;
            if (a < ulong.MaxValue / b)
            {
                r = (a * b) % M;
            }
            else
            {
                // オーバーフローしそうなので地道に行なう
                while (b != 0)
                {
                    if ((b & 1) != 0) { r = (r + a) % M; }
                    a = (a + a) % M;
                    b >>= 1;
                }
                return r;
            }

            return r;
        }

        /// <summary>
        /// Mを法とする累乗
        /// </summary>
        private static ulong ModPower(ulong a, ulong b, ulong M)
        {
            ulong r = 1;
            while (b > 0)
            {
                if ((b & 1) != 0) r = ModMul(r, a, M);

                a = ModMul(a, a, M);
                b >>= 1;
            }

            return r;
        }
        #endregion
    }
}
