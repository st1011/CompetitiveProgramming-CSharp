using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 木の直径とそのパス: https://judge.yosupo.jp/problem/tree_diameter
///                     https://onlinejudge.u-aizu.ac.jp/problems/GRL_5_A
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 木
    /// </summary>
    /// <remarks>
    /// - 直径とその経路
    /// </remarks>
    public class TreeGraph
    {
        // ノード数
        public int Count { get; private set; }

        // 有向グラフか
        public bool Directed { get; private set; }

        // 隣接リスト
        private readonly List<List<Edge>> _edges;

        // 直径の経路
        // [v]=u: 直径の経路ではuからvに向かう
        private readonly int[] _diameterPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        /// <param name="directed">有向グラフか</param>
        public TreeGraph(int v, bool directed)
        {
            _edges = Enumerable.Repeat(0, v)
                .Select(_ => new List<Edge>())
                .ToList();
            _diameterPath = Enumerable.Repeat(0, v).ToArray();
            Count = v;
            Directed = directed;
        }

        /// <summary>
        /// 辺の追加
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void Add(int from, int to, int cost = 1)
        {
            _edges[from].Add(new Edge(to, cost));
            if (!Directed)
            {
                _edges[to].Add(new Edge(from, cost));
            }
        }

        /// <summary>
        /// 木の直径を求める
        /// </summary>
        /// <returns>
        /// Item1: 直径
        /// Item2: 最遠頂点間のパス
        /// </returns>
        public Tuple<long, int[]> Diameter()
        {
            // 適当な頂点を選んでそこから最も遠い頂点を探す
            var u1 = FarthestNode(0, -1);

            // その頂点からまた最も遠い頂点を探す
            // その2点間が直径
            var u2 = FarthestNode(u1.Item1, -1);

            // 最遠頂点間のパス
            var path = DiameterPath(u1.Item1, u2.Item1);

            return new Tuple<long, int[]>(u2.Item2, path);
        }

        /// <summary>
        /// 最も遠い頂点を求める
        /// </summary>
        /// <param name="v">対象ノード</param>
        /// <param name="parent">呼び出し元ノード</param>
        /// <returns>
        /// Item1: vからの最遠頂点
        /// Item2: vから最遠頂点までのコスト
        /// </returns>
        private Tuple<int, long> FarthestNode(int v, int parent)
        {
            var q = new Queue<Tuple<int, int, long>>();
            // <対象ノード, 呼び出し元ノード, 対象ノードまでのコスト>
            q.Enqueue(new Tuple<int, int, long>(v, parent, 0));

            // 現時点の最遠頂点とそのコスト
            int farthestNode = v;
            long farthestCost = 0;
            while (q.Any())
            {
                var t = q.Dequeue();
                var u = t.Item1;
                var p = t.Item2;
                var c = t.Item3;

                foreach (var e in _edges[u])
                {
                    if (e.To == p) { continue; }

                    var newCost = c + e.Cost;
                    q.Enqueue(new Tuple<int, int, long>(e.To, u, newCost));
                    _diameterPath[e.To] = u;
                    if (newCost > farthestCost)
                    {
                        farthestNode = e.To;
                        farthestCost = newCost;
                    }
                }
            }

            return new Tuple<int, long>(farthestNode, farthestCost);
        }

        /// <summary>
        /// 直径のパス
        /// </summary>
        /// <param name="v1">最遠頂点1</param>
        /// <param name="v2">最遠頂点2</param>
        /// <returns></returns>
        private int[] DiameterPath(int v1, int v2)
        {
            var path = new List<int>();
            while (v2 != v1)
            {
                path.Add(v2);
                v2 = _diameterPath[v2];
            }
            path.Add(v2);

            return path.ToArray();
        }

        /// <summary>
        /// 辺
        /// </summary>
        private struct Edge : IComparable<Edge>
        {
            public int To;
            public long Cost;

            public Edge(int to, long cost)
            {
                To = to;
                Cost = cost;
            }

            public int CompareTo(Edge other)
            {
                return Cost.CompareTo(other.Cost);
            }
        }
    }
}
