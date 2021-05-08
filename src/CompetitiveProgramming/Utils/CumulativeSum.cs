using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// https://atcoder.jp/contests/typical90/tasks/typical90_ab
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 二次元累積和
    /// </summary>
    public sealed class CumulativeSum2D
    {
        /// <summary>
        /// 領域の高さ
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        /// 領域の幅
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        /// 2次元のマップ
        /// </summary>
        private readonly long[,] _map;

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="h">幅</param>
        /// <param name="w">高さ</param>
        public CumulativeSum2D(int h, int w)
        {
            _map = new long[h + 1, w + 1];
            Height = h;
            Width = w;
        }

        /// <summary>
        /// 半開区間への加算 [{x1,y1}, {x2,y2})
        /// </summary>
        /// <param name="x1">範囲左端</param>
        /// <param name="y1">範囲上端</param>
        /// <param name="x2">範囲右端</param>
        /// <param name="y2">範囲下端</param>
        /// <param name="v">加算する値</param>
        public void Add(int x1, int y1, int x2, int y2, long v)
        {
            _map[y1, x1] += v;
            _map[y1, x2] -= v;
            _map[y2, x1] -= v;
            _map[y2, x2] += v;
        }

        /// <summary>
        /// 累積和の実施
        /// </summary>
        /// <returns>累積和結果の2Dマップ</returns>
        public long[,] Sum()
        {
            // 横方向への累積和
            for (int y = 0; y < Height; y++)
            {
                for (int x = 1; x < Width; x++)
                {
                    _map[y, x] += _map[y, x - 1];
                }
            }
            // 縦方向への累積和
            for (int y = 1; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    _map[y, x] += _map[y - 1, x];
                }
            }

            return _map.Clone() as long[,];
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    builder.Append($"{_map[y, x]} ");
                }
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}
