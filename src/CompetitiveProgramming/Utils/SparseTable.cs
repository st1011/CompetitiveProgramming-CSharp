using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://code-festival-2014-qualb.contest.atcoder.jp/tasks/code_festival_qualB_d
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// スパーステーブル
    /// データの更新がない区間演算を高速に行える
    /// </summary>
    /// <remarks>
    /// Updateがないセグ木
    /// 構築O(N logN)
    /// クエリO(1)
    /// </remarks>
    public class SparseTable<T>
    {
        private readonly T[][] _st;
        private readonly int[] _lookupTable;
        private readonly Func<T, T, T> _monoid;

        /// <summary>
        /// テーブルの構築
        /// </summary>
        /// <param name="ie">値リスト</param>
        /// <param name="monoid">モノイド</param>
        public SparseTable(IReadOnlyList<T> ie, Func<T, T, T> monoid)
        {
            if (ie == null) { throw new ArgumentNullException(nameof(ie)); }
            _monoid = monoid;

            var b = 0;
            while ((1 << b) <= ie.Count)
            {
                ++b;
            }

            _lookupTable = new int[ie.Count + 1];
            for (var i = 2; i < _lookupTable.Length; i++)
            {
                _lookupTable[i] = _lookupTable[i >> 1] + 1;
            }

            var l2 = 1 << b;

            _st = Enumerable.Range(0, b)
                .Select(_ => new T[l2])
                .ToArray();
            for (var i = 0; i < ie.Count; i++)
            {
                _st[0][i] = ie[i];
            }

            for (var i = 1; i < b; i++)
            {
                for (var j = 0; j + (1 << i) <= l2; j++)
                {
                    _st[i][j] = monoid(_st[i - 1][j], _st[i - 1][j + (1 << (i - 1))]);
                }
            }
        }

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        public T Find(int b, int e)
        {
            var dist = _lookupTable[e - b];

            return _monoid(_st[dist][b], _st[dist][e - (1 << dist)]);
        }
    }
}
