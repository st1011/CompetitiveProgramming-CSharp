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
    class SparseTable<T>
    {
        readonly T[][] st;
        readonly int[] lookupTable;
        readonly Func<T, T, T> Monoid;

        /// <summary>
        /// テーブルの構築
        /// </summary>
        /// <param name="ie">値リスト</param>
        /// <param name="monoid">モノイド</param>
        public SparseTable(IReadOnlyList<T> ie, Func<T, T, T> monoid)
        {
            Monoid = monoid;

            var b = 0;
            while ((1 << b) <= ie.Count)
            {
                ++b;
            }

            lookupTable = new int[ie.Count + 1];
            for (var i = 2; i < lookupTable.Length; i++)
            {
                lookupTable[i] = lookupTable[i >> 1] + 1;
            }

            var l2 = 1 << b;

            st = Enumerable.Range(0, b)
                .Select(_ => new T[l2])
                .ToArray();
            for (var i = 0; i < ie.Count; i++)
            {
                st[0][i] = ie[i];
            }

            for (var i = 1; i < b; i++)
            {
                for (var j = 0; j + (1 << i) <= l2; j++)
                {
                    st[i][j] = monoid(st[i - 1][j], st[i - 1][j + (1 << (i - 1))]);
                }
            }
        }

        /// <summary>
        /// [b,e)の演算結果を求める
        /// </summary>
        public T Find(int b, int e)
        {
            var dist = lookupTable[e - b];

            return Monoid(st[dist][b], st[dist][e - (1 << dist)]);
        }
    }
}
