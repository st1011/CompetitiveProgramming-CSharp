using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Dijkstra: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_A&lang=jp
/// BellmanFord(負の経路 簡略）: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_B&lang=jp
/// BellmanFord(負の経路 詳細）: https://atcoder.jp/contests/abc137/tasks/abc137_e
/// WarshallFloyd: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_1_C&lang=jp
/// 
/// 経路復元(Dijkstra, WarshallFloyd): http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=0155
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みは非負数だけ
    /// </summary>
    class Dijkstra
    {
        const long Inf = long.MaxValue / 3;

        // ノード数
        readonly int V;
        // 隣接リスト
        readonly List<List<Edge>> Edges;
        // 前回の最短経路結果
        readonly long[] Dist;
        // 経路復元用情報
        readonly int[] Prev;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Dijkstra(int v)
        {
            Edges = Enumerable.Repeat(0, v)
                .Select(_ => new List<Edge>())
                .ToList();
            Dist = new long[v];
            Prev = new int[v];

            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => Edges[from].Add(new Edge(to, cost));

        /// <summary>
        /// sから各ノードへの最短経路
        /// </summary>
        public long[] ShortestPath(int s)
        {
            for (int i = 0; i < V; i++)
            {
                Dist[i] = Inf;
                Prev[i] = -1;
            }

            // 昇順ソートにする
            var pq = new PriorityQueue<Edge>((x, y) => y.CompareTo(x));

            Dist[s] = 0;
            pq.Push(new Edge(s, 0));

            while (pq.Any())
            {
                var p = pq.Pop();
                var v = p.To;

                // 他の経路の方がコストが低い
                if (Dist[v] < p.Cost) continue;

                foreach (var e in Edges[v])
                {
                    if (Dist[e.To] > Dist[v] + e.Cost)
                    {
                        // こっちの経路の方が効率的
                        Dist[e.To] = Dist[v] + e.Cost;
                        Prev[e.To] = v;
                        pq.Push(new Edge(e.To, Dist[e.To]));
                    }
                }
            }

            return Dist;
        }

        /// <summary>
        /// 経路を復元する
        /// </summary>
        /// <param name="t">目的ノード</param>
        /// <returns>探索したことがないときはnull</returns>
        public int[] RestorePath(int t)
        {
            if (Prev == null) return null;

            var stack = new Stack<int>();
            while (t >= 0)
            {
                stack.Push(t);
                t = Prev[t];
            }

            return stack.ToArray();
        }

        /// <summary>
        /// 接続されているか?
        /// </summary>
        public bool IsConnected(int e) => Dist[e] != Inf;

        struct Edge : IComparable<Edge>
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
                var n = Heap.Count;
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

                var n = Heap.Count - 1;
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
                if (!Heap.Any()) throw new Exception();

                return Heap[0];
            }

            /// <summary>
            /// ヒープに格納されているアイテム数
            /// </summary>
            public int Count() => Heap.Count;

            /// <summary>
            /// 一つでも値が格納されているか
            /// </summary>
            public bool Any() => Heap.Any();
        }
    }

    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みが負数でもOK（てか負数ないならDijkstra使って）
    /// </summary>
    class BellmanFord
    {
        /// <summary>
        /// 負の閉路を持つか？
        /// </summary>
        public bool HasNegCycle { get; private set; } = false;

        const long Inf = long.MaxValue / 3;
        const long NegCycle = -Inf;

        // ノード数
        readonly int V;
        // 隣接リスト
        readonly List<Edge> Edges;
        // 前回の最短経路結果
        readonly long[] Dist;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public BellmanFord(int v)
        {
            Edges = new List<Edge>();
            Dist = new long[v];
            V = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => Edges.Add(new Edge(from, to, cost));

        /// <summary>
        /// sから各ノードへの最短経路
        /// 負の閉路がある場合HasNegCycleがtrueになる
        /// </summary>
        /// <param name="s">探索始点</param>
        /// <param name="needsDetail">負の閉路との接続をノードごとに確かめるか</param>
        /// <remarks>
        /// needsDetailがtrue
        /// 負の閉路と接続されている経路情報は NegCycle となり
        /// IsConnectedNegCycle で負の閉路との接続を確認できる
        /// 負の閉路と無関係な経路情報を使いたいときはこっち
        /// 
        /// needsDetailがfalse
        /// 負の閉路と接続されている経路情報不定のまま
        /// </remarks>
        public long[] ShortestPath(int s, bool needsDetail=false)
        {
            var E = Edges.Count;
            for (int i = 0; i < V; i++)
            {
                Dist[i] = Inf;
            }

            Dist[s] = 0;
            HasNegCycle = false;
            int count = RelaxEdges(E, Dist, (x, y) => x + y);

            // 閉路が一つでもある
            HasNegCycle = count > V - 1;

            if (needsDetail)
            {
                // 詳細な負の閉路検出
                RelaxEdges(E, Dist, (x, y) => NegCycle);
            }

            return Dist;
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
                    var e = Edges[i];
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

        /// <summary>
        /// 接続されているか?
        /// </summary>
        public bool IsConnected(int t) => Dist[t] != Inf;

        /// <summary>
        /// 負の閉路と接続されているか？
        /// </summary>
        /// <remarks>
        /// ShortestPath で needsDetail=false を指定した場合は使えない
        /// </remarks>
        public bool IsConnectedNegCycle(int t) => Dist[t] == NegCycle;

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
    }

    /// <summary>
    /// 重み付き・有向グラフの全ノード間最短経路
    /// </summary>
    class WarshallFloyd
    {
        /// <summary>
        /// 負の閉路を持つか？
        /// </summary>
        public bool HasNegCycle { get; private set; } = false;

        const long Inf = long.MaxValue / 3;

        // ノード数
        readonly int V;
        // 隣接行列
        readonly long[,] Matrix;
        // 前回の最短経路結果
        readonly long[,] Dist;
        // 経路復元用
        readonly int[,] Next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public WarshallFloyd(int v)
        {
            Matrix = new long[v, v];
            Next = new int[v, v];
            Dist = new long[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    Matrix[i, j] = (i == j) ? 0 : Inf;
                    Next[i, j] = j;
                }
            }

            V = v;
            Dist[0, 0] = Inf;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => Matrix[from, to] = cost;

        /// <summary>
        /// sから各ノードへの最短経路
        /// 負の閉路がある場合HasNegCycleがtrueになる
        /// </summary>
        public long[,] ShortestPath()
        {
            Array.Copy(Matrix, Dist, Matrix.Length);
            HasNegCycle = false;

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    for (int k = 0; k < V; k++)
                    {
                        if (Dist[j, i] == Inf || Dist[i, k] == Inf) continue;
                        if (Dist[j, k] <= Dist[j, i] + Dist[i, k]) continue;

                        Dist[j, k] = Dist[j, i] + Dist[i, k];
                        Next[j, k] = Next[j, i];
                    }
                }
            }

            // 負の閉路がある？
            for (int i = 0; i < V; i++)
            {
                if (Dist[i, i] < 0)
                {
                    HasNegCycle = true;
                    break;
                }
            }

            return Dist;
        }

        /// <summary>
        /// 経路復元
        /// 探索したことがなかったり、接続されていない経路であればnull
        /// </summary>
        public IEnumerable<int> RestorePath(int s, int g)
        {
            if (Dist[0, 0] == Inf) { throw new Exception(); }
            if (!IsConnected(s, g)) { yield break; }

            var current = s;
            while (current != g)
            {
                yield return current;

                current = Next[current, g];
            }

            yield return current;
        }

        /// <summary>
        /// 接続されているか?
        /// </summary>
        public bool IsConnected(int s, int g) => Dist[s, g] != Inf;
    }
}
