using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://judge.yosupo.jp/problem/sqrt_mod
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 平方剰余
    /// トネリ-シャンクスのアルゴリズム
    /// </summary>
    public static class QuadraticResidue
    {
        /// <summary>
        /// sqrt(n) (mod p)
        /// </summary>
        /// <returns>存在しない場合は負数</returns>
        public static int Sqrt(long n, int p)
        {
            var x = (int)(n % p);

            int ans;
            if (x < 2)
            {
                ans = x;
            }
            else if (Power(x, p / 2, p) != 1)
            {
                ans = -1;
            }
            else if ((p & 3) == 3)
            {
                ans = Power(x, p / 4 + 1, p);
            }
            else
            {
                var q = p - 1;
                var m = 0;
                while ((q & 1) == 0)
                {
                    m++;
                    q /= 2;
                }

                var z = 1;
                while (Power(++z, p / 2, p) == 1) {; }

                var c = Power(z, q, p);
                var t = Power(x, q, p);
                ans = Power(x, q / 2 + 1, p);
                while (t != 1)
                {
                    var i = 0;
                    for (var temp = t; temp != 1; i++)
                    {
                        temp = Mul(temp, temp, p);
                    }

                    var b = c;
                    for (var j = 0; j < m - i - 1; j++)
                    {
                        b = Mul(b, b, p);
                    }

                    ans = Mul(ans, b, p);
                    c = Mul(b, b, p);
                    t = Mul(t, c, p);
                    m = i;
                }
            }

            if (ans * 2 > p)
            {
                ans = p - ans;
            }

            return ans;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static int Mul(int a, int b, int M)
            => (int)(((long)(a % M) * (b % M)) % M);

        /// <summary>
        /// x^y
        /// 繰り返し二乗法
        /// </summary>
        static int Power(int a, int b, int M)
        {
            var r = 1;

            while (b > 0)
            {
                if ((b & 1) != 0) r = Mul(r, a, M);

                a = Mul(a, a, M);
                b >>= 1;
            }

            return r;
        }
    }
}
