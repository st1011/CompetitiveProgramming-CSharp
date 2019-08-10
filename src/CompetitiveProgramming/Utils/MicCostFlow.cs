using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_6_B&lang=jp
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小費用流
    /// </summary>
    class MinCostFlow
    {
        static readonly int INF = int.MaxValue / 3;

        class Edge
        {
            public int To;
            public int Capacity;
            public int Cost;
            public int Rev;

            public Edge(int to, int cap, int cost, int r)
            {
                To = to;
                Capacity = cap;
                Cost = cost;
                Rev = r;
            }
        }

        readonly List<Edge>[] G;
        // ポテンシャル
        readonly int[] h;
        // 最短距離
        readonly int[] dist;
        // 直前の頂点と辺
        readonly int[] prevv;
        readonly int[] preve;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        public MinCostFlow(int n)
        {
            G = Enumerable.Range(0, n)
                .Select(_ => new List<Edge>())
                .ToArray();

            h = new int[n];
            dist = new int[n];
            prevv = new int[n];
            preve = new int[n];
        }

        /// <summary>
        /// 流路追加
        /// </summary>
        public void Add(int from, int to, int cap, int cost)
        {
            G[from].Add(new Edge(to, cap, cost, G[to].Count));
            G[to].Add(new Edge(from, 0, -cost, G[from].Count - 1));
        }

        /// <summary>
        /// sからtへ、fを流す場合の最小費用流
        /// 流せない場合は負数を返す
        /// </summary>
        public int Flow(int s, int t, int f)
        {
            int res = 0;
            Fill(h, 0);

            while (f > 0)
            {
                var pq = new PriorityQueue<Pair>();
                Fill(dist, INF);
                dist[s] = 0;
                pq.Push(new Pair(0, s));
                while (pq.Any())
                {
                    var p = pq.Pop();
                    int v = p.V;
                    if (dist[v] < p.Dist) continue;

                    for (int i = 0; i < G[v].Count(); i++)
                    {
                        var e = G[v][i];
                        var nc = dist[v] + e.Cost + h[v] - h[e.To];
                        if (e.Capacity > 0 && dist[e.To] > nc)
                        {
                            dist[e.To] = nc;
                            prevv[e.To] = v;
                            preve[e.To] = i;
                            pq.Push(new Pair(dist[e.To], e.To));
                        }
                    }
                }

                if (dist[t] == INF)
                {
                    // これ以上流せない
                    return -1;
                }

                for (int i = 0; i < G.Count(); i++)
                {
                    h[i] += dist[i];
                }

                var d = f;
                for (int v = t; v != s; v = prevv[v])
                {
                    d = Math.Min(d, G[prevv[v]][preve[v]].Capacity);
                }

                f -= d;
                res += d * h[t];
                for (int v = t; v != s; v = prevv[v])
                {
                    var e = G[prevv[v]][preve[v]];
                    e.Capacity -= d;
                    G[v][e.Rev].Capacity += d;
                }
            }

            return res;
        }

        /// <summary>
        /// aをvでfillする
        /// </summary>
        static void Fill(int[] a, int v)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = v;
            }
        }

        struct Pair : IComparable<Pair>
        {
            public int Dist;
            public int V;

            public Pair(int d, int v) { Dist = d; V = v; }

            public int CompareTo(Pair other)
                => Dist.CompareTo(other.Dist);
        }

        #region Priority Queue
        /// <summary>
        /// 優先度付きキュー
        /// デフォルトは昇順にソートされていて、小さい値から取り出される
        /// </summary>
        /// <typeparam name="T"></typeparam>
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

            /// <summary>
            /// 要素追加
            /// </summary>
            /// <param name="item"></param>
            public void Push(T item)
            {
                var n = Heap.Count();
                Heap.Add(item);

                while (n > 0)
                {
                    var parent = (n - 1) / 2;
                    if (compare(Heap[parent], item)) break;

                    Heap[n] = Heap[parent];
                    n = parent;
                }

                Heap[n] = item;
            }

            /// <summary>
            /// 先頭の値取得し削除する
            /// </summary>
            /// <returns></returns>
            public T Pop()
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
                    if (child + 1 < n && compare(Heap[child + 1], Heap[child]))
                    {
                        child++;
                    }

                    if (compare(last, Heap[child])) break;

                    Heap[parent] = Heap[child];
                    parent = child;
                    child = 2 * parent + 1;
                }

                Heap[parent] = last;
                Heap.RemoveAt(n);

                return item;
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
        #endregion
    }
}
