using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// ref. https://nodchip.hatenadiary.org/entry/20090303/1236058357

//verify: https://atcoder.jp/contests/arc054/tasks/arc054_b
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 三分探索
    /// </summary>
    public static class TernarySearch
    {
        /// <summary>
        /// 三分探索
        /// 極値を求める
        /// </summary>
        /// <param name="low">下限</param>
        /// <param name="high">上限</param>
        /// <param name="f">極値を(low-highの範囲内に)一つだけ持つ関数</param>
        /// <param name="isDownwardConvex">下に凸か</param>
        /// <param name="eps">許容誤差</param>
        /// <returns></returns>
        public static double Search(double low, double high, Func<double, double> f, bool isDownwardConvex, double eps = 1e-10)
        {
            // epsだけだと、大きい値の時に終われない
            // 厳密に計算はしてないけど、200回も回せば大体OKっぽい
            for (int i = 0; i < 200; i++)
            {
                double x1 = low + (high - low) / 3.0;
                double x2 = low + (high - low) / 3.0 * 2.0;

                var y1 = f(x1);
                var y2 = f(x2);

                if (isDownwardConvex)
                {
                    // 下に凸なので、大きい方を捨てる
                    if (y1 < y2) high = x2;
                    else low = x1;
                }
                else
                {
                    // 上に凸なので、小さい方を捨てる
                    if (y1 < y2) low = x1;
                    else high = x2;
                }

                if (high - low <= eps) break;
            }

            return (low + high) * 0.5;
        }
    }
}
