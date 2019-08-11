using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Dijkstra: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_A&lang=jp
/// BellmanFord(負の経路 簡略）: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_B&lang=jp
/// BellmanFord(負の経路 詳細）: https://atcoder.jp/contests/abc137/tasks/abc137_e
/// WarshallFloyd: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_C&lang=jp
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みは非負数だけ
    /// </summary>
    class Dijkstra
    {
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

        public static readonly long Inf = long.MaxValue / 3;

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
        public void AddPath(int from, int to, long cost)
            => es[from].Add(new Edge(to, cost));

        /// <summary>
        /// sから各ノードへの最短経路
        /// </summary>
        public long[] ShortestPath(int s)
        {
            var d = Enumerable.Repeat(Inf, V).ToArray();
            // 昇順ソートにする
            var pq = new PriorityQueue<Edge>((x, y) => y.CompareTo(x));
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
        /// 経路を復元する
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

        /// <summary>
        /// 優先度付きキュー(二分ヒープ)
        /// デフォルトは降順ソート
        /// </summary>
        class PriorityQueue<T>
        {
            static readonly int DefaultCapacity = 8;
            readonly List<T> Heap;
            readonly Comparison<T> Comparer;

            public PriorityQueue(int capacity, Comparison<T> comparer)
            {
                Heap = new List<T>(capacity);
                Comparer = comparer;
            }

            public PriorityQueue() : this(DefaultCapacity) { }
            public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default.Compare) { }
            public PriorityQueue(Comparison<T> comparer) : this(DefaultCapacity, comparer) { }

            /// <summary> 要素追加 </summary>
            public void Enqueue(T item)
            {
                var n = Heap.Count();
                Heap.Add(item);

                while (n > 0)
                {
                    var parent = (n - 1) / 2;
                    if (Comparer(Heap[parent], item) > 0) break;

                    Heap[n] = Heap[parent];
                    n = parent;
                }

                Heap[n] = item;
            }

            /// <summary> 要素追加 </summary>
            public void Push(T item) => Enqueue(item);

            /// <summary> 先頭の値を取得し削除する </summary>
            public T Dequeue()
            {
                if (!Heap.Any())
                {
                    throw new Exception();
                }
                var item = Heap[0];
                var last = Heap.Last();

                var n = Heap.Count() - 1;
                var parent = 0;
                var child = 2 * parent + 1;
                while (child < n)
                {
                    if (child + 1 < n && Comparer(Heap[child + 1], Heap[child]) > 0)
                    {
                        child++;
                    }

                    if (Comparer(last, Heap[child]) > 0) break;

                    Heap[parent] = Heap[child];
                    parent = child;
                    child = 2 * parent + 1;
                }

                Heap[parent] = last;
                Heap.RemoveAt(n);

                return item;
            }

            /// <summary> 先頭の値を取得し削除する </summary>
            public T Pop() => Dequeue();

            /// <summary>
            /// 先頭の値取得（削除はしない）
            /// </summary>
            public T Peek()
            {
                if (!this.Heap.Any()) throw new Exception();

                return this.Heap[0];
            }

            /// <summary>
            /// ヒープに格納されているアイテム数
            /// </summary>
            public int Count() => this.Heap.Count();

            /// <summary>
            /// 一つでも値が格納されているか
            /// </summary>
            public bool Any() => this.Heap.Any();
        }
    }

    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みが負数でもOK（てか負数ないならDijkstra使って）
    /// </summary>
    class BellmanFord
    {
        public static readonly long Inf = long.MaxValue / 3;
        public static readonly long NegCycle = -Inf;

        public bool HasNegCycle = false;

        struct Edge
        {
            public int From;
            public int To;
            public long Cost;

            public Edge(int from, int to, long cost)
            {
                From = from;
                To = to;
                Cost = cost;
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
        public void AddPath(int from, int to, long cost)
            => es.Add(new Edge(from, to, cost));

        /// <summary>
        /// sから各ノードへの最短経路
        /// 負の経路がある場合HasNegCycleがtrueになる
        /// 
        /// needsDetailがtrue
        /// sとの経路に負の閉路があるノードはNegCycle
        /// 
        /// needsDetailがfalse
        /// 負の経路がある辺の情報は不確かのまま
        /// </summary>
        public long[] ShortestPath(int s, bool needsDetail)
        {
            var E = es.Count();
            var d = Enumerable.Repeat(Inf, V).ToArray();

            d[s] = 0;
            HasNegCycle = false;
            int count = RelaxEdges(E, d, (x, y) => x + y);

            // 閉路が一つでもある
            HasNegCycle = count > V - 1;

            if (needsDetail)
            {
                // 詳細な負の閉路検出
                RelaxEdges(E, d, (x, y) => NegCycle);
            }

            return d;
        }

        /// <summary>
        /// 辺の緩和
        /// </summary>
        int RelaxEdges(int E, long[] d, Func<long, long, long> relax)
        {
            int count;
            for (count = 0; count < V; count++)
            {
                var update = false;
                for (int i = 0; i < E; i++)
                {
                    var e = es[i];
                    if (d[e.From] == Inf) continue;
                    if (d[e.To] > d[e.From] + e.Cost)
                    {
                        d[e.To] = relax(d[e.From], e.Cost);
                        update = true;
                    }
                }

                if (!update) break;
            }

            return count;
        }
    }

    /// <summary>
    /// 重み付き・有向グラフの全ノード間最短経路
    /// </summary>
    class WarshallFloyd
    {
        public static readonly long Inf = long.MaxValue / 3;

        // 接続情報
        readonly long[,] es;
        // ノード数
        readonly int V;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public WarshallFloyd(int v)
        {
            es = new long[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    es[i, j] = (i == j) ? 0 : Inf;
                }
            }

            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => es[from, to] = cost;

        /// <summary>
        /// sから各ノードへの最短経路
        /// 負の閉路を持つ場合、nullを返す
        /// </summary>
        public long[,] ShortestPath()
        {
            var d = new long[V, V];
            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    d[i, j] = es[i, j];
                }
            }

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    for (int k = 0; k < V; k++)
                    {
                        if (d[j, i] == Inf || d[i, k] == Inf) continue;

                        d[j, k] = Math.Min(d[j, k], d[j, i] + d[i, k]);
                    }
                }
            }

            // 負の閉路がある？
            for (int i = 0; i < V; i++)
            {
                if (d[i, i] < 0) return null;
            }

            return d;
        }
    }
}
