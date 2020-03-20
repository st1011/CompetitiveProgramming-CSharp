using System;
using System.Collections.Generic;

/// <summary>
/// LowerBound: https://atcoder.jp/contests/abc119/tasks/abc119_d
/// UpperBound: https://atcoder.jp/contests/abc143/tasks/abc143_d
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 二分探索系
    /// </summary>
    static class Search
    {
        /// <summary>
        /// Bound系の下請け
        /// </summary>
        static int FindBound<T>(IList<T> list, T v, int begin, int end, Func<T, T, bool> judge) where T : IComparable<T>
        {
            int low = begin, high = end;

            if (low > high)
            {
                throw new ArgumentException($"{nameof(low)} greater than {nameof(high)}", $"{nameof(low)} and {nameof(high)}");
            }

            while (low < high)
            {
                var mid = (high + low) / 2;

                if (judge(v, list[mid])) { high = mid; }
                else { low = mid + 1; }
            }

            return low;
        }

        /// <summary>
        /// v以上で最小のインデックスを返す
        /// </summary>
        public static int LowerBound<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return FindBound(list, v, begin, end, (x, y) => x.CompareTo(y) <= 0);
        }

        /// <summary>
        /// v以上で最小のインデックスを返す
        /// </summary>
        public static int LowerBound<T>(IList<T> list, T v) where T : IComparable<T>
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return LowerBound(list, v, 0, list.Count);
        }

        /// <summary>
        /// vより大きな最小のインデックスを返す
        /// </summary>
        public static int UpperBound<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return FindBound(list, v, begin, end, (x, y) => x.CompareTo(y) < 0);
        }

        /// <summary>
        /// vより大きな最小のインデックスを返す
        /// </summary>
        public static int UpperBound<T>(IList<T> list, T v) where T : IComparable<T>
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return UpperBound(list, v, 0, list.Count);
        }
    }

    /// <summary>
    /// 二分探索系
    /// 比較処理を外部から注入する用
    /// </summary>
    public class SearchOfComparerChangeable<T>
    {
        /// <summary> 比較用関数 </summary>
        private Comparison<T> Comparison { get; set; }

        public SearchOfComparerChangeable(Comparison<T> comparison = null)
        {
            Comparison = comparison ?? Comparer<T>.Default.Compare;
        }

        /// <summary>
        /// Bound系の下請け
        /// </summary>
        static int FindBound(IList<T> list, T v, int begin, int end, Func<T, T, bool> judge)
        {
            int low = begin, high = end;

            if (low > high)
            {
                throw new ArgumentException($"{nameof(low)} greater than {nameof(high)}", $"{nameof(low)} and {nameof(high)}");
            }

            while (low < high)
            {
                var mid = (high + low) / 2;

                if (judge(v, list[mid])) { high = mid; }
                else { low = mid + 1; }
            }

            return low;
        }

        /// <summary>
        /// v以上の中で最小のインデックスを返す
        /// </summary>
        public int LowerBound(IList<T> list, T v, int begin, int end)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return FindBound(list, v, begin, end, (x, y) => Comparison(x, y) <= 0);
        }

        /// <summary>
        /// v以上の中で最小のインデックスを返す
        /// </summary>
        public int LowerBound(IList<T> list, T v)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return LowerBound(list, v, 0, list.Count);
        }

        /// <summary>
        /// vより大きい中で最小のインデックスを返す
        /// </summary>
        public int UpperBound(IList<T> list, T v, int begin, int end)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return FindBound(list, v, begin, end, (x, y) => Comparison(x, y) < 0);
        }

        /// <summary>
        /// vより大きい中で最小のインデックスを返す
        /// </summary>
        public int UpperBound(IList<T> list, T v)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return UpperBound(list, v, 0, list.Count);
        }

        /// <summary>
        /// 範囲中でvと等価な個数を返す
        /// </summary>
        public int EqualCount(IList<T> list, T v, int begin, int end)
        {
            if (list == null) { throw new ArgumentNullException(nameof(list)); }
            return UpperBound(list, v, begin, end) - LowerBound(list, v, begin, end);
        }

        ///// <summary>
        ///// v以下で最大のインデックスを返す
        ///// リストに含まれる最小値より小さな値を指定した場合は-1を返す
        ///// </summary>
        //int OrLess(IList<int> list, int v, int begin, int end)
        //    => new SearchOfComparerChangeable<int>().UpperBound(list, v, begin, end) - 1;

        ///// <summary>
        ///// v未満で最大のインデックスを返す
        ///// リストに含まれる最小値より小さな値を指定した場合は-1を返す
        ///// </summary>
        //int LessThan(IList<T> list, T v, int begin, int end)
        //    => LowerBound(list, v, begin, end) - 1;
    }
}
