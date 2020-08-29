using System;
using System.Linq;

/// <summary>
/// BeginWith: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=ALDS1_14_B&lang=ja
/// GetHashes: https://atcoder.jp/contests/abc141/tasks/abc141_e
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 文字列専用ローリングハッシュ
    /// </summary>
    public class RollingHash
    {
        // 衝突防止で、複数個のmodを指定しておく
        private static readonly long[] _mods = { 999999893, 999999937 };

        public string S { get; private set; }

        private readonly int _base;
        private readonly long[,] _hashes;
        private readonly long[,] _powers;

        /// <summary>
        /// 初期化
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="b">演算定数
        /// 指定しない場合は内部で乱数を使う</param>
        public RollingHash(string s, int b = 0)
        {
            if (s == null) { throw new ArgumentNullException(nameof(s)); }

            S = s;

            if (b <= 0) { b = new Random().Next(0x111, 0x1111); }
            _base = b;

            _hashes = new long[_mods.Length, s.Length + 1];
            _powers = new long[_mods.Length, s.Length + 1];
            BuildTable(s, b, _mods, _hashes, _powers);
        }

        /// <summary>
        /// 比較可能なロリハを作成する
        /// </summary>
        /// <param name="s">文字列</param>
        public RollingHash CreateComparable(string s)
            => new RollingHash(s, _base);

        /// <summary>
        /// 演算用のテーブル初期化
        /// </summary>
        private void BuildTable(string s, int b, long[] mods, long[,] hashes, long[,] powers)
        {
            for (int i = 0; i < mods.Length; i++)
            {
                powers[i, 0] = 1;
                for (int j = 0; j < s.Length; j++)
                {
                    hashes[i, j + 1] = (hashes[i, j] * b + s[j]) % mods[i];
                    powers[i, j + 1] = (powers[i, j] * b) % mods[i];
                }
            }
        }

        /// <summary>
        /// Mods[i]におけるS[l,r)のハッシュを取得
        /// </summary>
        private long GetHash(int l, int r, int i)
        {
            var res = (_hashes[i, r] - _hashes[i, l] * _powers[i, r - l]) % _mods[i];
            if (res < 0) res += _mods[i];

            return res;
        }

        /// <summary>
        /// 全てのModsにおけるs[l,r)のハッシュを取得
        /// </summary>
        /// <remarks>
        /// long[]はそのままでは等価チェックはできないので注意！
        /// DictionaryのKeyで使いたい場合などは文字列として結合するなどする
        /// </remarks>
        public long[] GetHashes(int l, int r)
        {
            return Enumerable.Range(0, _mods.Length)
                .Select(x => GetHash(l, r, x))
                .ToArray();
        }

        /// <summary>
        /// 全てのModsでS[l,r)が一致するか
        /// </summary>
        public bool Match(RollingHash other, int l1, int l2, int len)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            // 範囲外
            if (S.Length < l1 + len || other.S.Length < l2 + len)
            {
                return false;
            }

            for (int i = 0; i < _mods.Length; i++)
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
        /// S[l1:]とother.S[l2:]
        /// </summary>
        public int GetLcp(RollingHash other, int l1, int l2)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            int len = Math.Min(S.Length - l1, other.S.Length - l2);
            int low = -1, high = len + 1;
            while (high - low > 1)
            {
                var mid = low + (high - low) / 2;

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
        /// S[l1:]がotherで始まっているか
        /// </summary>
        public bool BeginWith(RollingHash other, int l1)
        {
            if (other == null) { throw new ArgumentNullException(nameof(other)); }

            return Match(other, l1, 0, other.S.Length);
        }
    }
}
