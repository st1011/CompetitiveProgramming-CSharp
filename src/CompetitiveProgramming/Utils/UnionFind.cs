using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 同一集合判定: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=DSL_1_A&lang=ja
/// 重みの差: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=DSL_1_B&lang=ja
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 重み付きUF木
    /// </summary>
    public class UnionFind
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

    /// <summary>
    /// アイテムを含むUF木
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnionFind<T> where T : new()
    {
        public List<Node> Nodes { get; private set; }

        public UnionFind(int size)
        {
            Nodes = Enumerable.Range(0, size)
                .Select(x => new Node(new T(), 0, x)).ToList();
        }

        public UnionFind(IEnumerable<T> items)
        {
            Nodes = items.Select((x, i) => new { Item = x, Index = i })
                .Select(x => new Node(x.Item, 0, x.Index)).ToList();
        }

        public bool Same(int x, int y)
        {
            return FindRoot(x) == FindRoot(y);
        }

        public int FindRoot(int x)
        {
            if (x != Nodes[x].ParentId)
            {
                // 経路中のノードのparentを更新しておく
                Nodes[x].ParentId = FindRoot(Nodes[x].ParentId);
            }

            return Nodes[x].ParentId;
        }

        public void Merge(int x, int y)
        {
            x = FindRoot(x);
            y = FindRoot(y);

            if (x == y) return;

            if (Nodes[y].Rank < Nodes[x].Rank)
            {
                Nodes[y].ParentId = x;
            }
            else
            {
                Nodes[x].ParentId = y;

                if (Nodes[x].Rank == Nodes[y].Rank)
                {
                    Nodes[y].Rank++;
                }
            }
        }

        public class Node
        {
            public T Item { get; set; }
            public int Rank { get; set; }
            public int ParentId { get; set; }

            public Node(T item, int rank, int parent)
            {
                Item = item;
                Rank = rank;
                ParentId = parent;
            }
        }
    }
}
