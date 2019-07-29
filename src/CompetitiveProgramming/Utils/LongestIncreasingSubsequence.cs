using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 部分列
    /// 減少を求めたいときは、(Max-input)しておくとか
    /// </summary>
    static class Subsequence
    {
        /// <summary>
        /// (狭義)最長増加部分列
        /// </summary>
        public static List<T> Lis<T>(IList<T> nums)
            where T : IComparable<T>
        {
            if (nums.Count() <= 0) { return new List<T>(); }

            var iss = new List<T> { nums[0] };
            foreach (var current in nums.Skip(1))
            {
                if (iss.Last().CompareTo(current) < 0)
                {
                    iss.Add(current);
                }
                else
                {
                    var i = Search.LowerBound(iss, current);
                    iss[i] = current;
                }
            }

            return iss;
        }

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

                if (low > high) { throw new ArgumentException(); }

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
                => LowerBound<T>(list, v, 0, list.Count());

            /// <summary>
            /// vより大きな最小のインデックスを返す
            /// </summary>
            public static int UpperBound<T>(IList<T> list, T v, int begin, int end) where T : IComparable<T>
                => FindBound(list, v, begin, end, (x, y) => x.CompareTo(y) < 0);

            /// <summary>
            /// vより大きな最小のインデックスを返す
            /// </summary>
            public static int UpperBound<T>(IList<T> list, T v) where T : IComparable<T>
                => UpperBound(list, v, 0, list.Count());
        }
    }
}
