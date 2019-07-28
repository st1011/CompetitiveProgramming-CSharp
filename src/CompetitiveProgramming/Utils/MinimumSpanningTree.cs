using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                this.Rank = new int[size];
                this.Size = Enumerable.Repeat(1, size).ToArray();
                this.ParentId = Enumerable.Range(0, size).ToArray();
                this.DiffWeight = new int[size];
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
                if (x != this.ParentId[x])
                {
                    // 経路中のノードのパラメータを更新しておく
                    var root = FindRoot(this.ParentId[x]);
                    this.DiffWeight[x] += this.DiffWeight[this.ParentId[x]];
                    this.ParentId[x] = root;
                }

                return this.ParentId[x];
            }

            /// <summary>
            /// 親経路との重さを返す
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int Weight(int x)
            {
                FindRoot(x);

                return this.DiffWeight[x];
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

                if (this.Rank[x] < this.Rank[y])
                {
                    this.ParentId[x] = y;
                    this.Size[y] += this.Size[x];
                    this.DiffWeight[x] = -w;
                }
                else
                {
                    this.ParentId[y] = x;
                    this.Size[x] += this.Size[y];
                    this.DiffWeight[y] = w;

                    if (this.Rank[x] == this.Rank[y])
                    {
                        this.Rank[x]++;
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

                return this.Size[x];
            }
        }

        public struct Edge : IComparable<Edge>
        {
            public int v1;
            public int v2;
            public int Cost;

            public Edge(int v1, int v2, int cost)
            {
                this.v1 = v1;
                this.v2 = v2;
                this.Cost = cost;
            }

            public int CompareTo(Edge other)
            {
                return this.Cost - other.Cost;
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
            this.es.Sort();

            var uf = new UnionFind(V);
            var tree = new List<Edge>();
            foreach (var edge in this.es)
            {
                if (!uf.Same(edge.v1, edge.v2))
                {
                    uf.Merge(edge.v1, edge.v2);
                    tree.Add(edge);
                }
            }

            return tree;
        }
    }
}
