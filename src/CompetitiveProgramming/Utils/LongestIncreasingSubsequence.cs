using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    class LongestIncreasingSubsequence
    {
        static List<int> Lis(List<int> nums)
        {
            var iss = new List<int>
            {
                nums.FirstOrDefault()
            };
            foreach (var n in nums.Skip(1))
            {
                if (iss[iss.Count() - 1] < n)
                {
                    iss.Add(n);
                }
                else
                {
                    var i = LowerBound<int>(iss, n);
                    iss[i] = n;
                }
            }

            return iss;
        }

        public static int FindBound<T>(IList<T> list, T v, int start, int end, Func<int, int, bool> judge) where T : IComparable<T>
        {
            int low = start;
            int high = end;

            if (low > high)
            {
                throw new ArgumentException();
            }

            while (low < high)
            {
                var mid = ((high - low) / 2) + low;
                if (judge(list[mid].CompareTo(v), 0))
                {
                    low = mid + 1;
                }
                else
                {
                    high = mid;
                }
            }

            return low;
        }

        /// <summary>
        /// v以上の最初のインデックスを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="v"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int LowerBound<T>(IList<T> list, T v, int start, int end) where T : IComparable<T>
        {
            return FindBound<T>(list, v, start, end, (x, y) => x < y);
        }

        /// <summary>
        /// v以上の最初のインデックスを返す
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int LowerBound<T>(IList<T> list, T v) where T : IComparable<T>
        {
            return LowerBound<T>(list, v, 0, list.Count());
        }
    }
}
