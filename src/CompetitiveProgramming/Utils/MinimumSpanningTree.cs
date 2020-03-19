using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小全域木(Minimum Spanning Tree)を求める
    /// 無向グラフのみ
    /// 有向グラフは「Chu-Liu/Edmondsのアルゴリズム」とからしい
    /// </summary>
    class Kruskal
    {
        /// <summary>
        /// 重み付きUF木
        /// </summary>
        class UnionFind
        {
            int[] Rank { get; set; }
            int[] Size { get; set; }
            int[] ParentId { get; set; }
            int[] DiffWeight { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="size">ノードの総数</param>
            public UnionFind(int size)
            {
                Rank = new int[size];
                Size = Enumerable.Repeat(1, size).ToArray();
                ParentId = Enumerable.Range(0, size).ToArray();
                DiffWeight = new int[size];
            }

            /// <summary>
            /// 同一の親を持つか
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public bool Same(int x, int y)
            {
                return FindRoot(x) == FindRoot(y);
            }

            /// <summary>
            /// 指定ノードのrootを返す
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int FindRoot(int x)
            {
                if (x != ParentId[x])
                {
                    // 経路中のノードのパラメータを更新しておく
                    var root = FindRoot(ParentId[x]);
                    DiffWeight[x] += DiffWeight[ParentId[x]];
                    ParentId[x] = root;
                }

                return ParentId[x];
            }

            /// <summary>
            /// 親経路との重さを返す
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int Weight(int x)
            {
                FindRoot(x);

                return DiffWeight[x];
            }

            /// <summary>
            /// x,yが同一の親を持つ場合、Weightの差分（y-x）を返す
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Weight(int x, int y)
            {
                return Weight(y) - Weight(x);
            }

            /// <summary>
            /// 重みをつけて合体する
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="w"></param>
            public void Merge(int x, int y, int w = 0)
            {
                w += Weight(x) - Weight(y);

                x = FindRoot(x);
                y = FindRoot(y);

                if (x == y) return;

                if (Rank[x] < Rank[y])
                {
                    ParentId[x] = y;
                    Size[y] += Size[x];
                    DiffWeight[x] = -w;
                }
                else
                {
                    ParentId[y] = x;
                    Size[x] += Size[y];
                    DiffWeight[y] = w;

                    if (Rank[x] == Rank[y])
                    {
                        Rank[x]++;
                    }
                }
            }

            /// <summary>
            /// xと同じ親を持つノードの数
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int SameCount(int x)
            {
                x = FindRoot(x);

                return Size[x];
            }
        }

        public struct Edge : IComparable<Edge>
        {
            public int V1;
            public int V2;
            public int Cost;

            public Edge(int v1, int v2, int cost)
            {
                V1 = v1;
                V2 = v2;
                Cost = cost;
            }

            public int CompareTo(Edge other)
            {
                return Cost - other.Cost;
            }
        }

        // 接続情報
        readonly List<Edge> es;

        // ノード数
        readonly int V;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Kruskal(int v)
        {
            es = new List<Edge>();
            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="cost"></param>
        public void AddPath(int v1, int v2, int cost)
            => es.Add(new Edge(v1, v2, cost));

        /// <summary>
        /// 最小全域木
        /// </summary>
        /// <returns></returns>
        public List<Edge> MinimumSpanningTree()
        {
            es.Sort();

            var uf = new UnionFind(V);
            var tree = new List<Edge>();
            foreach (var edge in es)
            {
                if (!uf.Same(edge.V1, edge.V2))
                {
                    uf.Merge(edge.V1, edge.V2);
                    tree.Add(edge);
                }
            }

            return tree;
        }
    }
}
