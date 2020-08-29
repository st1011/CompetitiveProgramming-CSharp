using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// LIS(Strong): http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=DPL_1_D
/// LDS(Weak): https://atcoder.jp/contests/abc134/tasks/abc134_e
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 部分列(LIS/LDS)
    /// 減少を求めたいときは、(Max-input)しておくとか
    /// </summary>
    static class Subsequence
    {
        /// <summary>
        /// 最長単調増加部分列の長さ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nums"></param>
        /// <param name="inf">Tの最大値</param>
        /// <param name="isStrong">狭義単調増加か？</param>
        /// <returns></returns>
        public static int LisLength<T>(IReadOnlyList<T> nums, T inf, bool isStrong = true) where T : IComparable<T>
        {
            if (!nums.Any()) return 0;

            var bound = isStrong ? (Func<IList<T>, T, int>)Search.LowerBound : Search.UpperBound;
            var dp = Enumerable.Repeat(inf, nums.Count).ToList();
            foreach (var current in nums)
            {
                var i = bound(dp, current);
                dp[i] = current;
            }

            return Search.LowerBound(dp, inf);
        }

        /// <summary>
        /// 最長単調減少部分列の長さ
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="nums"></param>
        /// <param name="inf">Tの最大値</param>
        /// <param name="rev">Tの大小関係の反転関数</param>
        /// <param name="isStrong">狭義単調減少か？</param>
        /// <returns></returns>
        public static int LdsLength<T>(IReadOnlyList<T> nums, T inf, Func<T, T> rev, bool isStrong = true) where T : IComparable<T>
        {
            // 0がinfになってしまわないように調整
            var numsRev = nums.Select(x => rev(x)).ToArray();

            return LisLength(numsRev, inf, isStrong);
        }

        #region LDS Sample
        private static int LdsLengthInt(IReadOnlyList<int> nums, bool isStrong = true)
        {
            return LdsLength(nums, int.MaxValue, x => int.MaxValue - 1 - x, isStrong);
        }
        #endregion

        static class Search
        {
            /// <summary>
            /// Bound系の下請け
            /// </summary>
            private static int FindBound<T>(IList<T> list, T v, int begin, int end, Func<T, T, bool> judge) where T : IComparable<T>
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
        }
    }
}
