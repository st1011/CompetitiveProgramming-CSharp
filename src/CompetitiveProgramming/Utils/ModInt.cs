using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// Mを法とする数値
    /// </summary>
    struct ModInt
    {
        public static int M { get; private set; }
        private static bool mIsPrime;
        private static int[] factorials;
        private static int[] invs;
        private static int[] finvs;

        private readonly int N;

        static ModInt()
        {
            ModInt.Init();
        }

        public static void Init(int mod = (int)1e9 + 7)
        {
            ModInt.M = mod;
            ModInt.mIsPrime = IsPrime(mod);

            ModInt.factorials = Enumerable.Repeat<int>(1, 2).ToArray();
            ModInt.invs = Enumerable.Repeat<int>(1, 2).ToArray();
            ModInt.finvs = Enumerable.Repeat<int>(1, 2).ToArray();
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ModInt(int n)
        {
            this.N = n % M;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(ModInt a)
            => a.N;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static implicit operator ModInt(int a)
            => new ModInt(a);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Add(int a, int b)
            => (a + b) % M;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Sub(int a, int b)
            => (a + M - b) % M;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int Mul(int a, int b)
            => (int)(((long)(a % M) * (b % M)) % M);


        /// <summary>
        /// aの逆元（a^-1）
        /// Mとaが互いに素である必要があるが
        /// 大抵Mが素数でa<Mなのでチェックしない
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static int Inverse(int a)
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

            return u;
        }

        /// <summary>
        /// a/b
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static int Div(int a, int b)
        {
            // 厳密には条件違うけど、逆元を求めさせるときは大抵Mは素数なので……
            if (!mIsPrime)
            {
                throw new ArgumentException();
            }

            return Mul(a, Inverse(b));
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator +(ModInt a, ModInt b)
            => new ModInt(Add(a.N, b.N));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator -(ModInt a, ModInt b)
            => new ModInt(Sub(a.N, b.N));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator *(ModInt a, ModInt b)
            => new ModInt(Mul(a.N, b.N));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator /(ModInt a, ModInt b)
            => new ModInt(Div(a.N, b.N));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator +(ModInt a, int b)
            => new ModInt(Add(a.N, b));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator -(ModInt a, int b)
            => new ModInt(Sub(a.N, b));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator *(ModInt a, int b)
            => new ModInt(Mul(a.N, b));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt operator /(ModInt a, int b)
            => new ModInt(Div(a.N, b));

        public override string ToString()
        {
            return this.N.ToString();
        }

        /// <summary>
        /// x^y
        /// 繰り返し二乗法（バイナリ）
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int Power(int x, int y)
        {
            var r = 1;

            while (y > 0)
            {
                if ((y & 1) != 0)
                {
                    r = Mul(r, x);
                }
                x = Mul(x, x);

                y >>= 1;
            }

            return r;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ModInt Power(ModInt x, ModInt y)
            => new ModInt(Power(x.N, y.N));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ModInt Power(ModInt y)
            => new ModInt(Power(this, y));

        #region nCr
        /// <summary>
        /// a以下の階乗のテーブルを作成する
        /// </summary>
        /// <param name="a"></param>
        static void FactTable(int a)
        {
            if (a >= factorials.Length)
            {
                int begin = factorials.Length;

                Array.Resize(ref factorials, a + 1);

                for (int i = begin; i <= a; i++)
                {
                    factorials[i] = Mul(factorials[i - 1], i);
                }
            }
        }

        /// <summary>
        /// a以下の逆元テーブルを作成する
        /// </summary>
        /// <param name="a"></param>
        static void InvTable(int a)
        {
            if (a >= invs.Length)
            {
                int begin = invs.Length;

                Array.Resize(ref invs, a + 1);

                for (int i = begin; i <= a; i++)
                {
                    invs[i] = M - Mul(invs[M % i], M / i);
                }
            }
        }

        /// <summary>
        /// a以下の階乗の逆元テーブルを作成する
        /// </summary>
        /// <param name="a"></param>
        static void FinvTable(int a)
        {
            if (a >= finvs.Length)
            {
                int begin = finvs.Length;

                Array.Resize(ref finvs, a + 1);

                for (int i = begin; i <= a; i++)
                {
                    finvs[i] = Mul(finvs[i - 1], invs[i]);
                }
            }
        }

        /// <summary>
        /// nCr
        /// </summary>
        /// <param name="n"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        public static int Ncr(int n, int r)
        {
            if (n == r) return 1;

            if (factorials.Length <= n) FactTable(n);
            if (invs.Length <= n) InvTable(n);
            if (finvs.Length <= n) FinvTable(n);

            return Mul(factorials[n], Mul(finvs[r], finvs[n - r]));
        }
        #endregion

        #region Misc
        /// <summary>
        /// 素数か
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static bool IsPrime(long x)
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

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
        #endregion
    }
}
