using System;
using System.Collections.Generic;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 昇順ソート済みリストに対しての二分探索系
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
            => FindBound(list, v, begin, end, (x, y) => x.CompareTo(y) <= 0);

        /// <summary>
        /// v以上で最小のインデックスを返す
        /// </summary>
        public static int LowerBound<T>(IList<T> list, T v) where T : IComparable<T>
            => LowerBound<T>(list, v, 0, list.Count);

        /// <summary>
        /// vより大きな最小のインデックスを返す
        /// </summary>
        public static int UpperBound<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
            => FindBound(list, v, begin, end, (x, y) => x.CompareTo(y) < 0);

        /// <summary>
        /// vより大きな最小のインデックスを返す
        /// </summary>
        public static int UpperBound<T>(IList<T> list, T v) where T : IComparable<T>
            => UpperBound(list, v, 0, list.Count);

        /// <summary>
        /// 範囲中でvと等価な個数を返す
        /// </summary>
        public static int EqualRange<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
            => UpperBound(list, v, begin, end) - LowerBound(list, v, begin, end);

        /// <summary>
        /// 範囲中でvと等価な個数を返す
        /// </summary>
        public static int EqualRange<T>(IList<T> list, T v) where T : IComparable<T>
            => UpperBound(list, v) - LowerBound(list, v);

        /// <summary>
        /// v以下で最大のインデックスを返す
        /// O(logN+N)なので注意
        /// </summary>
        public static int OrLess<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
        {
            var i = LowerBound(list, v, begin, end);

            if (list[i].CompareTo(v) != 0)
            {
                // v未満で最大の値とする
                if (i > 0) i--;
            }
            else
            {
                // vと同じ値で、一番最後へ移動させる
                while (i < end && list[i + 1].CompareTo(v) == 0) { i++; }
            }

            return i;
        }

        /// <summary>
        /// v以下で最大のインデックスを返す
        /// O(logN+N)なので注意
        /// </summary>
        public static int OrLess<T>(IList<T> list, T v) where T : IComparable<T>
            => OrLess(list, v, 0, list.Count);

        /// <summary>
        /// v未満で最大のインデックスを返す
        /// </summary>
        public static int LessThan<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
        {
            var i = LowerBound(list, v, begin, end);

            // v未満で最大の値とする
            return (i > 0) ? i - 1 : i;
        }

        /// <summary>
        /// v以下で最大のインデックスを返す
        /// </summary>
        public static int LessThan<T>(IList<T> list, T v) where T : IComparable<T>
            => LessThan(list, v, 0, list.Count);
    }
}
