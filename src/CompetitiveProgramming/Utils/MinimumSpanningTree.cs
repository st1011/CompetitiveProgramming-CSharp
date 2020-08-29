using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_2_A&lang=ja
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小全域木(Minimum Spanning Tree)を求める
    /// 無向グラフのみ
    /// 有向グラフは「Chu-Liu/Edmondsのアルゴリズム」とからしい
    /// </summary>
    class Kruskal
    {
        // 接続情報
        private readonly List<Edge> _es;

        // ノード数
        private readonly int _nodeCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Kruskal(int v)
        {
            _es = new List<Edge>();
            _nodeCount = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="cost"></param>
        public void AddPath(int v1, int v2, int cost)
            => _es.Add(new Edge(v1, v2, cost));

        /// <summary>
        /// 最小全域木
        /// </summary>
        /// <returns></returns>
        public List<Edge> MinimumSpanningTree()
        {
            _es.Sort();

            var uf = new UnionFind(_nodeCount);
            var tree = new List<Edge>();
            foreach (var edge in _es)
            {
                if (!uf.Same(edge.V1, edge.V2))
                {
                    uf.Merge(edge.V1, edge.V2);
                    tree.Add(edge);
                }
            }

            return tree;
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

        /// <summary>
        /// 重み付きUF木
        /// </summary>
        private class UnionFind
        {
            private readonly int[] _rank;
            private readonly int[] _size;
            private readonly int[] _parentId;
            private readonly int[] _diffWeight;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="size">ノードの総数</param>
            public UnionFind(int size)
            {
                _rank = new int[size];
                _size = Enumerable.Repeat(1, size).ToArray();
                _parentId = Enumerable.Range(0, size).ToArray();
                _diffWeight = new int[size];
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
                if (x != _parentId[x])
                {
                    // 経路中のノードのパラメータを更新しておく
                    var root = FindRoot(_parentId[x]);
                    _diffWeight[x] += _diffWeight[_parentId[x]];
                    _parentId[x] = root;
                }

                return _parentId[x];
            }

            /// <summary>
            /// 親経路との重さを返す
            /// </summary>
            /// <param name="x"></param>
            /// <returns></returns>
            public int Weight(int x)
            {
                FindRoot(x);

                return _diffWeight[x];
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

                if (_rank[x] < _rank[y])
                {
                    _parentId[x] = y;
                    _size[y] += _size[x];
                    _diffWeight[x] = -w;
                }
                else
                {
                    _parentId[y] = x;
                    _size[x] += _size[y];
                    _diffWeight[y] = w;

                    if (_rank[x] == _rank[y])
                    {
                        _rank[x]++;
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

                return _size[x];
            }
        }
    }
}
