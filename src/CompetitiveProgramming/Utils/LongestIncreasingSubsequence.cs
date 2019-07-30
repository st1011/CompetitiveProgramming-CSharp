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
    static class Lis
    {
        /// <summary>
        /// 最長(狭義単調)増加部分列の長さ
        /// </summary>
        public static int NarrowLength<T>(IList<T> nums) where T : IComparable<T>
        {
            if (nums.Count() <= 0) { return 0; }

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

            return iss.Count();
        }

        /// <summary>
        /// 最長(広義単調)増加部分列の長さ
        /// </summary>
        public static int BroadLength<T>(IList<T> nums, Func<T, T> increment)
            where T : IComparable<T>
        {
            if (nums.Count() <= 0) { return 0; }

            var dss = new List<T>();
            foreach (var current in nums.Skip(1))
            {
                var r = Search.LowerBound(dss, increment(current));
                if (dss.Count() <= r)
                {
                    dss.Add(current);
                }
                else
                {
                    dss[r] = current;
                }
            }

            return dss.Count();
        }

        #region LDS Sample
        /// <summary>
        /// 最長(狭義単調)減少部分列の長さ
        /// </summary>
        public static int LdsNarrowLength<T>(IList<T> nums, Func<T, T> reverse)
            where T : IComparable<T>
        {
            var numsRev = nums.Select(x => reverse(x)).ToArray();

            return NarrowLength(numsRev);
        }
        #endregion

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
