using System;

// 二分探索(小数):
// https://atcoder.jp/contests/abc034/tasks/abc034_d

// 二分探索(整数):
// https://atcoder.jp/contests/arc050/tasks/arc050_b
// https://atcoder.jp/contests/abc023/tasks/abc023_d
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 二分探索
    /// </summary>
    public static class BinarySearch
    {
        /// <summary>
        /// 二分探索(小数)
        /// </summary>
        /// <param name="okValue">fを通してOKになる値でngValueから最も離れた値</param>
        /// <param name="ngValue">fを通してNGになる値でokValueから最も離れた値</param>
        /// <param name="isOK">結果判定</param>
        /// <param name="eps">許容誤差</param>
        public static double Search(double okValue, double ngValue, Func<double, bool> isOK, double eps = 1e-20)
        {
            // epsだけだと、大きい値の時に終われない
            for (int i = 0; i < 200; i++)
            {
                var mid = (ngValue + okValue) * 0.5;

                if (isOK(mid)) { okValue = mid; }
                else { ngValue = mid; }

                if (Math.Abs(ngValue - okValue) <= eps) { break; }
            }

            return (okValue + ngValue) * 0.5;
        }

        /// <summary>
        /// 二分探索(整数)
        /// </summary>
        /// <param name="okValue">fを通してOKになる値でngValueから最も離れた値</param>
        /// <param name="ngValue">fを通してNGになる値でokValueから最も離れた値</param>
        /// <param name="isOK">結果判定</param>
        public static long Search(long okValue, long ngValue, Func<long, bool> isOK)
        {
            while (Math.Abs(okValue - ngValue) > 1)
            {
                var mid = (ngValue + okValue) / 2;

                if (isOK(mid)) { okValue = mid; }
                else { ngValue = mid; }
            }

            return okValue;
        }
    }
}
