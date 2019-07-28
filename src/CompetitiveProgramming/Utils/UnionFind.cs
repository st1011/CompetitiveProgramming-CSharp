using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
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

    /// <summary>
    /// アイテムを含むUF木
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class UnionFind<T> where T : new()
    {
        public class Node
        {
            public T Item { get; set; }
            public int Rank { get; set; }
            public int ParentId { get; set; }

            public Node(T item, int rank, int parent)
            {
                this.Item = item;
                this.Rank = rank;
                this.ParentId = parent;
            }
        }

        public List<Node> Nodes { get; set; }

        public UnionFind(int size)
        {
            this.Nodes = Enumerable.Range(0, size)
                .Select(x => new Node(new T(), 0, x)).ToList();
        }

        public UnionFind(IEnumerable<T> items)
        {
            this.Nodes = items.Select((x, i) => new { Item = x, Index = i })
                .Select(x => new Node(x.Item, 0, x.Index)).ToList();
        }

        public bool Same(int x, int y)
        {
            return FindRoot(x) == FindRoot(y);
        }

        public int FindRoot(int x)
        {
            if (x != this.Nodes[x].ParentId)
            {
                // 経路中のノードのparentを更新しておく
                this.Nodes[x].ParentId = FindRoot(this.Nodes[x].ParentId);
            }

            return this.Nodes[x].ParentId;
        }

        public void Merge(int x, int y)
        {
            x = FindRoot(x);
            y = FindRoot(y);

            if (x == y) return;

            if (this.Nodes[y].Rank < this.Nodes[x].Rank)
            {
                this.Nodes[y].ParentId = x;
            }
            else
            {
                this.Nodes[x].ParentId = y;

                if (this.Nodes[x].Rank == this.Nodes[y].Rank)
                {
                    this.Nodes[y].Rank++;
                }
            }
        }
    }
}
