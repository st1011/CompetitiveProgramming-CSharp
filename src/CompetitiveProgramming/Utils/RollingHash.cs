using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 文字列専用ローリングハッシュ
    /// </summary>
    class RollingHash
    {
        // 衝突防止で、複数個のmodを指定しておく
        static readonly long[] Mods = { 999999893, 999999937 };
        static readonly long Base = 1013;
        readonly string S;
        readonly long[,] Hashes;
        readonly long[,] Powers;

        public RollingHash(string s)
        {
            var mcount = Mods.Length;

            S = s;
            Hashes = new long[mcount, s.Length + 1];
            Powers = new long[mcount, s.Length + 1];

            for (int i = 0; i < mcount; i++)
            {
                Powers[i, 0] = 1;
                for (int j = 0; j < s.Length; j++)
                {
                    Hashes[i, j + 1] = (Hashes[i, j] * Base + s[j]) % Mods[i];
                    Powers[i, j + 1] = (Powers[i, j] * Base) % Mods[i];
                }
            }
        }

        /// <summary>
        /// S[l,r)のハッシュを取得
        /// </summary>
        long GetHash(int l, int r, int i)
        {
            var res = (Hashes[i, r] - Hashes[i, l] * Powers[i, r - l]) % Mods[i];
            if (res < 0) res += Mods[i];

            return res;
        }

        /// <summary>
        /// 全てのModsでS[l,r)が一致するか
        /// 2つのrhashを比較する
        /// </summary>
        public bool Match(RollingHash other, int l1, int l2, int len)
        {
            for (int i = 0; i < Mods.Length; i++)
            {
                if (GetHash(l1, l1 + len, i) != other.GetHash(l2, l2 + len, i))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 最長共通接頭辞の長さ
        /// this.S[l1:]とother.S[l2:]
        /// </summary>
        public int GetLcp(RollingHash other, int l1, int l2)
        {
            int len = Math.Min(S.Length - l1, other.S.Length - l2);

            int low = -1, high = len + 1;
            while (high - low > 1)
            {
                var mid = (low + high) / 2;

                if (!Match(other, l1, l2, mid))
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }
            }

            return low;
        }

        /// <summary>
        /// 最長共通接頭辞の長さ
        /// </summary>
        public int GetLcp(int l1, int l2)
            => GetLcp(this, l1, l2);

        /// <summary>
        /// this.S[l1:]とother全体が一致しているか
        /// </summary>
        public bool BeginWith(RollingHash other, int l1)
            => Match(other, l1, 0, other.S.Length);
    }
}
