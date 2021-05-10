using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 全マンハッタン距離の最大値: https://atcoder.jp/contests/abc178/tasks/abc178_e
/// 任意の点と他の点のマンハッタン距離の最大値: https://atcoder.jp/contests/typical90/tasks/typical90_aj
/// </summary>
namespace CompetitiveProgramming.Utils
{
    using PTYPE = System.Int64;

    /// <summary>
    /// ユークリッド平面
    /// </summary>
    public class EuclideanPlane
    {
        // 通常の座標系
        public IList<Vector> Points { get; private set; }

        // 45度回転座標系
        private readonly List<Vector> _rotatedPoints;

        // minXとminYは対応しているわけではないのでVectorにはしない
        private PTYPE _minX = PTYPE.MaxValue;
        private PTYPE _minY = PTYPE.MaxValue;
        private PTYPE _maxX = PTYPE.MinValue;
        private PTYPE _maxY = PTYPE.MinValue;

        /// <summary>
        /// インスタンス生成
        /// </summary>
        public EuclideanPlane()
        {
            Points = new List<Vector>();
            _rotatedPoints = new List<Vector>();
        }

        /// <summary>
        /// 点の追加
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void Add(PTYPE x, PTYPE y)
        {
            Points.Add(new Vector(x, y));

            var v = new Vector(x + y, x - y);
            _rotatedPoints.Add(v);

            if (v.X < _minX) { _minX = v.X; }
            if (v.X > _maxX) { _maxX = v.X; }
            if (v.Y < _minY) { _minY = v.Y; }
            if (v.Y > _maxY) { _maxY = v.Y; }
        }

        /// <summary>
        /// 2点間のマンハッタン距離
        /// </summary>
        /// <param name="p1">点1のindex</param>
        /// <param name="p2">点2のindex</param>
        /// <returns></returns>
        public PTYPE ManhattanDistant(int p1, int p2)
        {
            return Points[p1].ManhattanDistant(Points[p2]);
        }

        /// <summary>
        /// マンハッタン距離が最大となる2点間の距離
        /// </summary>
        /// <returns></returns>
        public PTYPE MaxManhattanDistant()
        {
            return Math.Max(_maxX - _minX, _maxY - _minY);
        }

        /// <summary>
        /// マンハッタン距離が最大となる点1ともう一点の距離
        /// </summary>
        /// <param name="p1">点1のindex</param>
        /// <returns></returns>
        public PTYPE MaxManhattanDistant(int p1)
        {
            var p = _rotatedPoints[p1];
            var d = new[]
            {
                Math.Abs(p.X-_minX),
                Math.Abs(p.X-_maxX),
                Math.Abs(p.Y-_minY),
                Math.Abs(p.Y-_maxY),
            };

            return d.Max();
        }

#pragma warning disable CA1034 // 入れ子にされた型を参照可能にすることはできません
        public class Vector
#pragma warning restore CA1034 // 入れ子にされた型を参照可能にすることはできません
        {
            public PTYPE X { get; set; }
            public PTYPE Y { get; set; }

            public Vector(PTYPE x, PTYPE y)
            {
                X = x;
                Y = y;
            }

            public PTYPE ManhattanDistant(Vector other)
            {
                if (other == null) { throw new ArgumentNullException(nameof(other)); }

                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
            }
        }
    }
}
