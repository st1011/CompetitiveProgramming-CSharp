using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// スターリンソート
    /// </summary>
    public static class StalinSort
    {
        /// <summary>
        /// スターリンソート
        /// 計算量O(n)の画期的なソートです
        /// </summary>
        /// <returns>list.Count()との比較結果は不定です</returns>
        public static List<T> Sort<T>(IReadOnlyList<T> list, Comparison<T> comparison = null)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }

            var dest = new List<T>();

            if (!list.Any())
            {
                return dest;
            }

            if (comparison==null)
            {
                comparison = Comparer<T>.Default.Compare;
            }

            dest.Add(list[0]);
            for (int i = 1; i < list.Count; i++)
            {
                if (comparison(dest.Last(), list[i]) >= 0)
                {
                    dest.Add(list[i]);
                }
            }

            return dest;
        }
    }
}
