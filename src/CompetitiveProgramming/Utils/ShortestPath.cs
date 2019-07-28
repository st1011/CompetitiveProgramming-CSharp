using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みは非負数だけ
    /// </summary>
    class Dijkstra
    {
        class PriorityQueue<T> where T : IComparable<T>
        {
            readonly List<T> Heap;
            readonly Func<T, T, bool> compare;

            public PriorityQueue(int capacity, bool isMinHeap = true)
            {
                this.Heap = new List<T>(capacity);

                if (isMinHeap)
                {
                    compare = (x, y) => x.CompareTo(y) < 0;
                }
                else
                {
                    compare = (x, y) => x.CompareTo(y) > 0;
                }
            }

            public PriorityQueue() : this(0) { }

            static void PushHeap(List<T> heap, Func<T, T, bool> compare, T item)
            {
                var n = heap.Count();
                heap.Add(item);

                while (n != 0)
                {
                    var parent = (n - 1) / 2;

                    if (compare(heap[n], heap[parent]))
                    {
                        var temp = heap[n];
                        heap[n] = heap[parent];
                        heap[parent] = temp;
                    }

                    n = parent;
                }
            }

            static T PopHeap(List<T> heap, Func<T, T, bool> compare)
            {
                if (!heap.Any())
                {
                    throw new Exception();
                }
                var item = heap[0];

                var n = heap.Count() - 1;
                heap[0] = heap.Last();
                heap.RemoveAt(n);

                var parent = 0;
                var child = 2 * parent + 1;
                while (child < n)
                {
                    if ((child != n - 1) && compare(heap[child + 1], heap[child]))
                    {
                        child++;
                    }

                    if (compare(heap[child], heap[parent]))
                    {
                        var temp = heap[parent];
                        heap[parent] = heap[child];
                        heap[child] = temp;
                    }

                    parent = child;
                    child = 2 * parent + 1;
                }

                return item;
            }

            /// <summary>
            /// 要素追加
            /// </summary>
            /// <param name="item"></param>
            public void Push(T item)
            {
                PushHeap(this.Heap, this.compare, item);
            }

            /// <summary>
            /// 先頭の値取得し削除する
            /// </summary>
            /// <returns></returns>
            public T Pop()
            {
                return PopHeap(this.Heap, this.compare);
            }

            /// <summary>
            /// 先頭の値取得（削除はしない）
            /// </summary>
            /// <returns></returns>
            public T Peek()
            {
                if (!this.Heap.Any())
                {
                    throw new Exception();
                }

                return this.Heap[0];
            }

            /// <summary>
            /// ヒープに格納されているアイテム数
            /// </summary>
            /// <returns></returns>
            public int Count() => this.Heap.Count();

            /// <summary>
            /// 一つでの値が格納されているか
            /// </summary>
            /// <returns></returns>
            public bool Any() => this.Heap.Any();
        }

        struct Edge : IComparable<Edge>
        {
            public int To;
            public long Cost;

            public Edge(int to, long cost)
            {
                this.To = to;
                this.Cost = cost;
            }

            public int CompareTo(Edge other)
            {
                return this.Cost.CompareTo(other.Cost);
            }
        }

        static readonly long INF = long.MaxValue / 10;

        // 接続情報
        readonly List<List<Edge>> es;

        // ノード数
        readonly int V;
        // 経路復元用情報
        int[] Prev;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Dijkstra(int v)
        {
            es = Enumerable.Repeat(0, v)
                .Select(_ => new List<Edge>())
                .ToList();
            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddPath(int from, int to, long cost)
            => es[from].Add(new Edge(to, cost));

        /// <summary>
        /// 各ノードへの最短経路
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public long[] ShortestPath(int s)
        {
            var d = Enumerable.Repeat(INF, V).ToArray();
            var pq = new PriorityQueue<Edge>();
            Prev = Enumerable.Repeat(-1, V).ToArray();

            d[s] = 0;
            pq.Push(new Edge(s, 0));

            while (pq.Any())
            {
                var p = pq.Pop();
                var v = p.To;

                // 他の経路の方がコストが低い
                if (d[v] < p.Cost) continue;

                foreach (var e in es[v])
                {
                    if (d[e.To] > d[v] + e.Cost)
                    {
                        // こっちの経路の方が効率的
                        d[e.To] = d[v] + e.Cost;
                        Prev[e.To] = v;
                        pq.Push(new Edge(e.To, d[e.To]));
                    }
                }
            }

            return d;
        }

        /// <summary>
        /// 最後に探索した経路を復元する
        /// </summary>
        /// <param name="t">目的ノード</param>
        /// <returns>探索したことがないときはnull</returns>
        public int[] Path(int t)
        {
            if (this.Prev == null) return null;

            var stack = new Stack<int>();
            while (t >= 0)
            {
                stack.Push(t);
                t = Prev[t];
            }

            return stack.ToArray();
        }
    }

    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みが負数でもOK（てか負数ないならDijkstra使って）
    /// </summary>
    class BellmanFord
    {
        static readonly int INF = int.MaxValue / 5;

        struct Edge
        {
            public int From;
            public int To;
            public int Cost;

            public Edge(int from, int to, int cost)
            {
                this.From = from;
                this.To = to;
                this.Cost = cost;
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
        public BellmanFord(int v)
        {
            es = new List<Edge>();
            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddPath(int from, int to, int cost)
            => es.Add(new Edge(from, to, cost));

        /// <summary>
        /// 各ノードへの最短経路
        /// </summary>
        /// <param name="s"></param>
        /// <returns>nullなら負経路がある</returns>
        public int[] ShortestPath(int s)
        {
            var E = this.es.Count();
            var d = Enumerable.Repeat(INF, V).ToArray();

            d[s] = 0;
            int count;
            for (count = 0; count < V; count++)
            {
                var update = false;
                for (int i = 0; i < E; i++)
                {
                    var e = es[i];
                    if (d[e.From] != INF && d[e.To] > d[e.From] + e.Cost)
                    {
                        // こっちの経路の方が効率的
                        d[e.To] = d[e.From] + e.Cost;
                        update = true;
                    }
                }

                if (!update) break;
            }

            if (count >= V - 1) return null;
            else return d;
        }
    }

    /// <summary>
    /// 重み付き・有向グラフの全ノード間最短経路
    /// </summary>
    class WarshallFloyd
    {
        static readonly int INF = int.MaxValue / 5;

        // 接続情報
        readonly int[,] es;

        // ノード数
        readonly int V;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public WarshallFloyd(int v)
        {
            es = new int[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    es[i, j] = (i == j) ? 0 : INF;
                }
            }

            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddPath(int from, int to, int cost)
            => es[from, to] = cost;

        /// <summary>
        /// 各ノードへの最短経路
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int[,] ShortestPath()
        {
            var d = new int[V, V];
            Array.Copy(es, d, d.Length);

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    for (int k = 0; k < V; k++)
                    {
                        d[j, k] = Math.Min(d[j, k], d[j, i] + d[i, k]);
                    }
                }
            }

            return d;
        }
    }
}
