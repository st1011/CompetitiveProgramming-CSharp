using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 
/// Add: https://atcoder.jp/contests/dp/tasks/dp_h
/// Sub:
/// Mul, Div: 組み合わせが抱合
/// Power: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=NTL_1_B&lang=ja
/// Factorial: https://atcoder.jp/contests/arc076/tasks/arc076_a
/// 順列: 組合わせが抱合
/// 組合わせ: https://atcoder.jp/contests/abc034/tasks/abc034_c
/// 
/// 組み合わせ(テーブル有+無): https://atcoder.jp/contests/abc145/tasks/abc145_d
/// 組み合わせ(テーブル有): https://atcoder.jp/contests/abc132/tasks/abc132_d
/// 重複組み合わせ(テーブル有+無): https://atcoder.jp/contests/abc021/tasks/abc021_d
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// Mを法とする数値
    /// Mは素数
    /// </summary>
    public struct ModInt
    {
        public readonly int M;
        private readonly int N;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">実際の数値</param>
        /// <param name="mod">M</param>
        public ModInt(int n, int mod = (int)1e9 + 7)
        {
            N = n % mod;
            M = mod;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Add(int a, int b, int M)
            => new ModInt((a + b) % M, M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Sub(int a, int b, int M)
            => new ModInt((a + M - b) % M, M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Mul(int a, int b, int M)
            => new ModInt((int)(((long)(a % M) * (b % M)) % M), M);

        /// <summary> a/b </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Div(int a, int b, int M)
            => Mul(a, Inverse(b, M), M);

        /// <summary>
        /// aの逆元（a^-1）
        /// （厳密には違うが）Mは素数とする
        /// </summary>
        public static ModInt Inverse(int a, int M)
        {
            int b = M;
            int u = 1, v = 0;

            while (b > 0)
            {
                int t = a / b;
                a -= t * b;
                Swap(ref a, ref b);
                u -= t * v;
                Swap(ref u, ref v);
            }

            u %= M;
            if (u < 0) u += M;

            return new ModInt(u, M);
        }

        /// <summary>
        /// x^y
        /// 繰り返し二乗法
        /// </summary>
        public static ModInt Power(int a, int b, int M)
        {
            var r = new ModInt(1, M);

            while (b > 0)
            {
                if ((b & 1) != 0) r = Mul(r, a, M);

                a = Mul(a, a, M);
                b >>= 1;
            }

            return r;
        }

        public override string ToString() => N.ToString();

        #region Combinatorics
        /// <summary>
        /// 階乗(n!)
        /// </summary>
        public static ModInt Factorial(int n, int M)
        {
            var fact = new ModInt(1, M);
            while (n > 0)
            {
                fact *= n;
                n--;
            }

            return fact;
        }

        /// <summary>
        /// 順列(nPr)
        /// </summary>
        public static ModInt Npr(int n, int r, int M)
        {
            if (n < r || n < 0 || r < 0)
            {
                return new ModInt(0, M);
            }

            var npr = new ModInt(1, M);
            for (int i = n; i > n - r; i--)
            {
                npr *= i;
            }

            return npr;
        }

        /// <summary>
        /// 組み合わせ(nCr)
        /// </summary>
        public static ModInt Ncr(int n, int r, int M)
        {
            if (n < r || n < 0 || r < 0)
            {
                return new ModInt(0, M);
            }

            r = Math.Min(r, n - r);
            var npr = Npr(n, r, M);
            var fact = Factorial(r, M);

            return npr / fact;
        }

        /// <summary>
        /// 重複組み合わせ(nHr)
        /// n個から重複を許してr個取り出す
        /// </summary>
        public static ModInt Nhr(int n, int r, int M)
            => Ncr(n + r - 1, r, M);
        #endregion

        #region Operator, Alias
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(ModInt a)
            => a.N;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator +(ModInt a, int b)
            => Add(a, b, a.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator -(ModInt a, int b)
            => Sub(a, b, a.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator *(ModInt a, int b)
            => Mul(a, b, a.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator /(ModInt a, int b)
            => Div(a, b, a.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator +(int a, ModInt b)
            => Add(a, b, b.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator -(int a, ModInt b)
            => Sub(a, b, b.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator *(int a, ModInt b)
            => Mul(a, b, b.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator /(int a, ModInt b)
            => Div(a, b, b.M);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator +(ModInt a, ModInt b)
        {
            if (a.M != b.M) throw new ArgumentException();
            return Add(a, b, a.M);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator -(ModInt a, ModInt b)
        {
            if (a.M != b.M) throw new ArgumentException();
            return Sub(a, b, a.M);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator *(ModInt a, ModInt b)
        {
            if (a.M != b.M) throw new ArgumentException();
            return Mul(a, b, a.M);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator /(ModInt a, ModInt b)
        {
            if (a.M != b.M) throw new ArgumentException();
            return Div(a, b, a.M);
        }

        /// <summary> a^b </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Power(ModInt a, int b)
            => Power(a, b, a.M);

        /// <summary> a^b </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Power(ModInt a, ModInt b)
        {
            if (a.M != b.M) throw new ArgumentException();
            return Power(a, b, a.M);
        }

        /// <summary> n! </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Factorial(ModInt n)
            => Factorial(n, n.M);
        #endregion

        #region Misc
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        #endregion
    }

    /// <summary>
    /// Mを法とする数値によるCombinationなど
    /// Mは素数
    /// </summary>
    public class ModCom
    {
        public readonly int M;
        // 階乗テーブル
        private int[] factorials;
        // 逆元テーブル
        private int[] invs;
        // 階乗の逆元テーブル
        private int[] finvs;
        // 繰り返し計算するならテーブルを作成する
        // テーブル作るコストもそれなりに大きいので基本は作らない
        private readonly int maxTableSize;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="tableSize">作成するテーブル上限</param>
        public ModCom(int mod = (int)1e9 + 7, int tableSize = 0)
        {
            M = mod;
            maxTableSize = tableSize;
        }

        /// <summary>
        /// 順列(nPr)
        /// </summary>
        public int Npr(int n, int r)
        {
            if (n < r || n < 0 || r < 0)
            {
                return 0;
            }

            if (n <= maxTableSize)
            {
                MakeTable(n);
                if (factorials != null && n <= factorials.Length)
                {
                    return Mul(factorials[n], finvs[n - r]);
                }
            }

            // テーブルがない or 範囲外のため計算する
            // テーブルがある場合は続きから計算すると定数倍改善
            int npr = 1;
            for (int i = n; i > n - r; i--)
            {
                npr = Mul(npr, i);
            }

            return npr;
        }

        /// <summary>
        /// 組み合わせ(nCr)
        /// </summary>
        public int Ncr(int n, int r)
        {
            if (n < r || n < 0 || r < 0)
            {
                return 0;
            }

            r = Math.Min(r, n - r);
            if (n <= maxTableSize)
            {
                MakeTable(n);
                if (factorials != null && n <= factorials.Length)
                {
                    return Mul(Npr(n, r), finvs[r]);
                }
            }

            // テーブルがない or 範囲外のため計算する
            // テーブルがある場合は続きから計算すると定数倍改善
            int npr = 1, fact = 1;
            for (int i = 0; i < r; i++)
            {
                npr = Mul(npr, n - i);
                fact = Mul(fact, i + 1);
            }

            return Div(npr, fact);
        }

        /// <summary>
        /// 重複組み合わせ(nHr)
        /// n個から重複を許してr個取り出す
        /// </summary>
        public int Nhr(int n, int r)
            => Ncr(n + r - 1, r);

        /// <summary>
        /// n以下の各テーブルを作成する
        /// </summary>
        void MakeTable(int n)
        {
            if (n > maxTableSize)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (factorials == null)
            {
                factorials = Enumerable.Repeat(1, 2).ToArray();
                invs = Enumerable.Repeat(1, 2).ToArray();
                finvs = Enumerable.Repeat(1, 2).ToArray();
            }

            if (n < factorials.Length)
            {
                return;
            }
            var begin = factorials.Length;

            Array.Resize(ref factorials, n + 1);
            Array.Resize(ref invs, n + 1);
            Array.Resize(ref finvs, n + 1);

            for (int i = begin; i <= n; i++)
            {
                factorials[i] = Mul(factorials[i - 1], i);
                invs[i] = M - Mul(invs[M % i], M / i);
                finvs[i] = Mul(finvs[i - 1], invs[i]);
            }
        }

        #region Misc
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        int Mul(int a, int b)
            => (int)(((long)(a % M) * (b % M)) % M);

        /// <summary> a/b </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        int Div(int a, int b)
            => Mul(a, Inverse(b));

        /// <summary>
        /// aの逆元（a^-1）
        /// （厳密には違うが）Mは素数とする
        /// </summary>
        int Inverse(int a)
        {
            int b = M;
            int u = 1, v = 0;

            while (b > 0)
            {
                int t = a / b;
                a -= t * b;
                Swap(ref a, ref b);
                u -= t * v;
                Swap(ref u, ref v);
            }

            u %= M;
            if (u < 0)
            {
                u += M;
            }

            return u;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Swap<T>(ref T a, ref T b)
        {
            T t = a;
            a = b;
            b = t;
        }
        #endregion
    }
}
