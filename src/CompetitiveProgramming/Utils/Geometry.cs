using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 幾何学系
    /// </summary>
    class Geometry
    {
        public enum Relationship
        {
            CCW = 1,            // CCW方向に存在する
            CW = -1,            // CW方向に存在する
            OnlineBack = 2,     // 折り返して、開始点を行き過ぎる
            OnlineFront = -2,   // 同じ方向に伸びている（一つの線分と見なせる）
            Onsegment = 0,      // 折り返して、開始点との間に終端がある

            Unsolved = 9,       // 未解決
        }

        static readonly double Epsilon = 1e-10;

        public static bool IsEqual(double x, double y) => Math.Abs(x - y) < Epsilon;

        public struct Point : IEquatable<Point>
        {
            public double X { get; set; }
            public double Y { get; set; }

            public Point(double x, double y)
            {
                X = x;
                Y = y;
            }

            public Point(IEnumerable<double> coords)
                : this(coords.ElementAt(0), coords.ElementAt(1)) { }

            public Point(IEnumerable<int> coords)
                : this(coords.Take(2).Select(x => (double)x)) { }

            public static double Norm(Point p) => p.X * p.X + p.Y * p.Y;
            public static double Abs(Point p) => Math.Sqrt(p.Norm());
            public static double Distance(Point p1, Point p2) => Abs(p2 - p1);

            /// <summary>
            /// 内積
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static double Dot(Point x, Point y) => x.X * y.X + x.Y * y.Y;

            /// <summary>
            /// 外積
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public static double Cross(Point x, Point y) => x.X * y.Y - x.Y * y.X;

            /// <summary>
            /// p1からp2を経由してp3へ移動する時を考え
            /// p2でどのような挙動をしたか
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="p3"></param>
            /// <returns></returns>
            public static Relationship Relation(Point p1, Point p2, Point p3)
            {
                var a = p2 - p1;
                var b = p3 - p1;

                var cross = Cross(a, b);
                if (cross > Epsilon) return Relationship.CCW;
                if (cross < -Epsilon) return Relationship.CW;
                if (Dot(a, b) < -Epsilon) return Relationship.OnlineBack;
                if (a.Norm() < b.Norm()) return Relationship.OnlineFront;

                return Relationship.Onsegment;
            }

            /// <summary>
            /// 線分1,2 と 線分3,4が直交するか
            /// </summary>
            /// <param name="p1"></param>
            /// <param name="p2"></param>
            /// <param name="p3"></param>
            /// <param name="p4"></param>
            /// <returns></returns>
            public static bool Intersect(Point p1, Point p2, Point p3, Point p4)
            {
                var r1 = (int)Relation(p1, p2, p3);
                var r2 = (int)Relation(p1, p2, p4);
                var r3 = (int)Relation(p3, p4, p1);
                var r4 = (int)Relation(p3, p4, p2);

                return r1 * r2 <= 0 && r3 * r4 <= 0;
            }

            public static Point operator +(Point x, Point y) => new Point(x.X + y.X, x.Y + y.Y);
            public static Point operator -(Point x, Point y) => new Point(x.X - y.X, x.Y - y.Y);
            public static Point operator *(Point x, double k) => new Point(x.X * k, x.Y * k);
            public static Point operator /(Point x, double k) => new Point(x.X / k, x.Y / k);
            public static bool operator <(Point x, Point y) => x.X != y.Y ? x.X < y.X : x.Y < x.Y;
            public static bool operator >(Point x, Point y) => x.X != y.Y ? x.X > y.X : x.Y > x.Y;

            public static bool operator ==(Point x, Point y)
            {
                return IsEqual(x.X, y.X) && IsEqual(x.Y, y.Y);
            }

            public static bool operator !=(Point x, Point y)
            {
                return !(x == y);
            }

            public double Norm() => Point.Norm(this);
            public double Abs() => Point.Abs(this);
            public double Distance(Point other) => Point.Distance(this, other);
            public double Dot(Point other) => Point.Dot(this, other);
            public double Cross(Point other) => Point.Cross(this, other);

            public bool Equals(Point other) => this == other;

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                return Equals((Point)obj);
            }

            public override int GetHashCode()
            {
                return X.GetHashCode() ^ Y.GetHashCode();
            }
        }

        public struct Line
        {
            public Point P1 { get; set; }
            public Point P2 { get; set; }

            public Line(Point p1, Point p2)
            {
                P1 = p1;
                P2 = p2;
            }

            public Line(double x1, double y1, double x2, double y2)
                : this(new Point(x1, y1), new Point(x2, y2)) { }

            public Line(IEnumerable<double> points)
                : this(new Point(points), new Point(points.Skip(2))) { }

            public Line(IEnumerable<int> points)
                : this(points.Take(4).Select(x => (double)x)) { }

            /// <summary>
            /// P1からの相対的なP2座標
            /// </summary>
            /// <returns></returns>
            public Point Diffrent()
            {
                return P2 - P1;
            }

            /// <summary>
            /// 直交するか？
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool IsOrthgonal(Line other)
            {
                return IsEqual(Point.Dot(Diffrent(), other.Diffrent()), 0);
            }

            /// <summary>
            /// 平行か？
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool IsParallel(Line other)
            {
                return IsEqual(Point.Cross(Diffrent(), other.Diffrent()), 0);
            }

            /// <summary>
            /// 点pの射影（pからLineへの垂線の交点）
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public Point Project(Point p)
            {
                var bs = Diffrent();

                var t = Point.Dot(p - P1, bs);
                var r = t / bs.Norm();

                return P1 + bs * r;
            }

            /// <summary>
            /// 点pの反射（Lineを軸としたときのpの線対称にある点）
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public Point Reflect(Point p)
            {
                return (Project(p) - p) * 2 + p;
            }

            /// <summary>
            /// 直線と点pとの距離
            /// （一応、直線は無限長の線のこと）
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public double DistanceLP(Point p)
            {
                return Math.Abs(Point.Cross(Diffrent(), p - P1) / Point.Abs(Diffrent()));
            }

            /// <summary>
            /// 線分と点pとの距離
            /// （一応、線分は有限長の線のこと）
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public double DistanceSP(Point p)
            {
                if (Point.Dot(P2 - P1, p - P1) < 0)
                {
                    return Point.Abs(p - P1);
                }
                if (Point.Dot(P1 - P2, p - P2) < 0)
                {
                    return Point.Abs(p - P2);
                }

                return DistanceLP(p);
            }

            /// <summary>
            /// 交差するか
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public bool IsIntersect(Line other)
            {
                return Point.Intersect(P1, P2, other.P1, other.P2);
            }

            /// <summary>
            /// 線分間の最短距離
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public double Distance(Line other)
            {
                if (IsIntersect(other)) return 0;

                var r1 = DistanceSP(other.P1);
                var r2 = DistanceSP(other.P2);
                var r3 = other.DistanceSP(P1);
                var r4 = other.DistanceSP(P2);

                return Math.Min(Math.Min(Math.Min(r1, r2), r3), r4);
            }

            /// <summary>
            /// 線分間の交点
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public Point CrossPoint(Line other)
            {
                var bs = other.P2 - other.P1;

                var d1 = Math.Abs(Point.Cross(bs, P1 - other.P1));
                var d2 = Math.Abs(Point.Cross(bs, P2 - other.P1));
                var t = d1 / (d1 + d2);

                return P1 + Diffrent() * t;
            }

            /// <summary>
            /// P2とother.P1が同一
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public Relationship Relation(Line other)
            {
                if (P2 != other.P1) { return Relationship.Unsolved; }

                return Point.Relation(P1, other.P1, other.P2);
            }
        }

        public struct Circle
        {
            public Point Center { get; set; }
            public double Radius { get; set; }

            public Circle(Point c, double r)
            {
                Center = c;
                Radius = r;
            }

            /// <summary>
            /// 線分との交点
            /// </summary>
            /// <param name="l"></param>
            /// <returns></returns>
            public IEnumerable<Point> CrossPoints(Line l)
            {
                var pr = l.Project(Center);
                var e = l.Diffrent() / l.Diffrent().Abs();

                var bs = Math.Sqrt(Radius * Radius - (pr - Center).Norm());

                if (Center.Distance(pr) > Radius)
                {
                    yield break;
                }

                yield return pr + e * bs;
                yield return pr - e * bs;
            }

            static double Radian(Point p) => Math.Atan2(p.Y, p.X);
            static Point Polar(double power, double radian) => new Point(Math.Cos(radian), Math.Sin(radian)) * power;

            /// <summary>
            /// 円との交点
            /// </summary>
            /// <param name="c"></param>
            /// <returns></returns>
            public IEnumerable<Point> CrossPoints(Circle c)
            {
                var d = Point.Distance(Center, c.Center);
                if (d > Radius + c.Radius)
                {
                    yield break;
                }

                var a = Math.Acos((Radius * Radius + d * d - c.Radius * c.Radius) / (2 * Radius * d));
                var t = Radian(c.Center - Center);

                yield return Center + Polar(Radius, t + a);
                yield return Center + Polar(Radius, t - a);
            }
        }

        public class Polygon
        {
            public List<Point> Points { get; set; }

            public Polygon(IEnumerable<Point> points)
            {
                Points = points.ToList();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="n">頂点数(coordsの個数ではない)</param>
            public Polygon(IEnumerable<double> coords, int n)
            {
                Points = new List<Point>();

                foreach (var i in Enumerable.Range(0, n))
                {
                    Points.Add(new Point(coords.ElementAt(i * 2), coords.ElementAt(i * 2 + 1)));
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="coords"></param>
            /// <param name="n">頂点数(coordsの個数ではない)</param>
            public Polygon(IEnumerable<int> coords, int n)
                : this(coords.Select(x => (double)x), n) { }

            public Polygon() : this(Enumerable.Empty<double>(), 0) { }

            public void Add(Point p) => Points.Add(p);
            public void Add(double x, double y) => Points.Add(new Point(x, y));
            public void Add(IEnumerable<double> coords) => Points.Add(new Point(coords));
            public void Add(IEnumerable<int> coords) => Points.Add(new Point(coords));

            /// <summary>
            /// 構成する線分リストを取得
            /// </summary>
            /// <returns></returns>
            public List<Line> Lines()
            {
                var lines = new List<Line>();

                var n = Points.Count;
                for (int i = 0; i < n; i++)
                {
                    lines.Add(new Line(Points[i].X, Points[i].Y,
                        Points[(i + 1) % n].X, Points[(i + 1) % n].Y));
                }

                return lines;
            }

            /// <summary>
            /// 点pとの関係
            /// </summary>
            /// <param name="p"></param>
            /// <returns>0: 外部、1: 線上、2: 内部</returns>
            public int JudgeRelationship(Point p)
            {
                var n = Points.Count;
                var x = false;

                foreach (var i in Enumerable.Range(0, n))
                {
                    var a = Points[i] - p;
                    var b = Points[(i + 1) % n] - p;

                    if ((Math.Abs(Point.Cross(a, b)) < Epsilon)
                        && (Point.Dot(a, b) < Epsilon))
                    {
                        return 1;
                    }
                    if (a.Y > b.Y)
                    {
                        var temp = a;
                        a = b;
                        b = temp;
                    }

                    if (a.Y < Epsilon && b.Y > Epsilon && Point.Cross(a, b) > Epsilon)
                    {
                        x = !x;
                    }
                }

                return x ? 2 : 0;
            }

            /// <summary>
            /// 点pが線上か内部にいる
            /// </summary>
            /// <param name="p"></param>
            /// <returns></returns>
            public bool Contains(Point p)
            {
                return JudgeRelationship(p) > 0;
            }

            /// <summary>
            /// 重心の取得
            /// </summary>
            /// <param name="points"></param>
            /// <returns></returns>
            public Point CenterOfGravity()
            {
                var n = Points.Count;
                var sum = Points.Aggregate((x, next) => x + next);

                return new Point(sum.X / n, sum.Y / n);
            }

            /// <summary>
            /// 面積の取得
            /// </summary>
            /// <returns></returns>
            public double Area()
            {
                double sum = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    sum += Points[i].Cross(Points[(i + 1) % Points.Count]);
                }

                return Math.Abs(sum) / 2.0;
            }

            /// <summary>
            /// 凸性判定（内角が全て180度以下）
            /// </summary>
            /// <param name="isCcwPoints">PointsがCCWで格納されているか？</param>
            /// <returns></returns>
            public bool IsConvex(bool isCcwPoints = true)
            {
                // CCWで格納されている点を見る場合は
                // 巡回したときCWに点がでなければ内角が180度以内と見なせる
                var dir = isCcwPoints ? Relationship.CW : Relationship.CCW;
                var n = Points.Count;
                for (int i = 0; i < n; i++)
                {
                    var r = Point.Relation(Points[i], Points[(i + 1) % n], Points[(i + 2) % n]);

                    if (r == dir) { return false; }
                }

                return true;
            }
        }

        /// <summary>
        /// 凸包の構成点を取得
        /// 左下の点から反時計回り
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static Point[] ConvexHull(IEnumerable<Point> points)
        {
            var ps = points.OrderBy(x => x.X)
                .ThenBy(x => x.Y)
                .ToArray();

            var qs = new Point[ps.Length * 2];
            int k = 0;

            // 下側凸包の作成
            for (int i = 0; i < ps.Length; i++)
            {
                // 新しく追加する点によって凸包ではなくなるのなら、以前追加した点を削除する
                while (k > 1 && (qs[k - 1] - qs[k - 2]).Cross(ps[i] - qs[k - 1]) < -Epsilon)
                {
                    k--;
                }

                qs[k++] = ps[i];
            }

            // 上側凸包の作成
            int t = k;
            for (int i = ps.Length - 2; i >= 0; i--)
            {
                // 新しく追加する点によって凸包ではなくなるのなら、以前追加した点を削除する
                while (k > t && (qs[k - 1] - qs[k - 2]).Cross(ps[i] - qs[k - 1]) < -Epsilon)
                {
                    k--;
                }

                qs[k++] = ps[i];
            }

            // 有効長に丸める
            Array.Resize(ref qs, k - 1);

            return qs;
        }

        /// <summary>
        /// 多角形の直径
        /// キャリパー法
        /// </summary>
        /// <param name="ps"></param>
        /// <returns></returns>
        public static double ConvexDiameter(IEnumerable<Point> ps)
        {
            var ch = ConvexHull(ps);
            var n = ch.Length;
            if (n == 2)
            {
                // 凸が潰れている
                return ch[0].Distance(ch[1]);
            }

            int i = 0, j = 0;
            // X軸方向に最も遠い対となる点
            for (int k = 0; k < n; k++)
            {
                if (ch[i].X < ch[k].X) i = k;
                if (ch[j].X > ch[k].X) j = k;
            }

            double r = 0;
            int si = i, sj = j;
            while (i != sj || j != si)
            {
                r = Math.Max(r, ch[i].Distance(ch[j]));

                if ((ch[(i + 1) % n] - ch[i]).Cross(ch[(j + 1) % n] - ch[j]) < Epsilon)
                {
                    i = (i + 1) % n;
                }
                else
                {
                    j = (j + 1) % n;
                }
            }

            return r;
        }
    }
}
