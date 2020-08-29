using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_6_B&lang=jp
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小費用流
    /// </summary>
    public class MinCostFlow
    {
        private const int _inf = int.MaxValue / 3;

        private readonly List<Edge>[] _nodes;
        // ポテンシャル
        private readonly int[] _h;
        // 最短距離
        private readonly int[] _dist;
        // 直前の頂点と辺
        private readonly int[] _prevv;
        private readonly int[] _preve;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        public MinCostFlow(int n)
        {
            _nodes = Enumerable.Range(0, n)
                .Select(_ => new List<Edge>())
                .ToArray();

            _h = new int[n];
            _dist = new int[n];
            _prevv = new int[n];
            _preve = new int[n];
        }

        /// <summary>
        /// 流路追加
        /// </summary>
        public void Add(int from, int to, int cap, int cost)
        {
            _nodes[from].Add(new Edge(to, cap, cost, _nodes[to].Count));
            _nodes[to].Add(new Edge(from, 0, -cost, _nodes[from].Count - 1));
        }

        /// <summary>
        /// sからtへ、fを流す場合の最小費用流
        /// 流せない場合は負数を返す
        /// </summary>
        public int Flow(int s, int t, int f)
        {
            int res = 0;
            Fill(_h, 0);

            while (f > 0)
            {
                var pq = new PriorityQueue<Pair>();
                Fill(_dist, _inf);
                _dist[s] = 0;
                pq.Push(new Pair(0, s));
                while (pq.Any())
                {
                    var p = pq.Pop();
                    int v = p.V;
                    if (_dist[v] < p.Dist) continue;

                    for (int i = 0; i < _nodes[v].Count; i++)
                    {
                        var e = _nodes[v][i];
                        var nc = _dist[v] + e.Cost + _h[v] - _h[e.To];
                        if (e.Capacity > 0 && _dist[e.To] > nc)
                        {
                            _dist[e.To] = nc;
                            _prevv[e.To] = v;
                            _preve[e.To] = i;
                            pq.Push(new Pair(_dist[e.To], e.To));
                        }
                    }
                }

                if (_dist[t] == _inf)
                {
                    // これ以上流せない
                    return -1;
                }

                for (int i = 0; i < _nodes.Length; i++)
                {
                    _h[i] += _dist[i];
                }

                var d = f;
                for (int v = t; v != s; v = _prevv[v])
                {
                    d = Math.Min(d, _nodes[_prevv[v]][_preve[v]].Capacity);
                }

                f -= d;
                res += d * _h[t];
                for (int v = t; v != s; v = _prevv[v])
                {
                    var e = _nodes[_prevv[v]][_preve[v]];
                    e.Capacity -= d;
                    _nodes[v][e.Rev].Capacity += d;
                }
            }

            return res;
        }

        /// <summary>
        /// aをvでfillする
        /// </summary>
        private static void Fill(int[] a, int v)
        {
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = v;
            }
        }

        private struct Pair : IComparable<Pair>
        {
            public int Dist;
            public int V;

            public Pair(int d, int v) { Dist = d; V = v; }

            public int CompareTo(Pair other)
                => Dist.CompareTo(other.Dist);
        }

        private class Edge
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

        #region Priority Queue
        /// <summary>
        /// 優先度付きキュー(二分ヒープ)
        /// デフォルトは降順ソート
        /// </summary>
        private class PriorityQueue<T>
        {
            private const int _defaultCapacity = 8;
            private readonly List<T> _heap;
            private readonly Comparison<T> _comparer;

            public PriorityQueue(int capacity, Comparison<T> comparer)
            {
                _heap = new List<T>(capacity);
                _comparer = comparer;
            }

            public PriorityQueue() : this(_defaultCapacity) { }
            public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default.Compare) { }
            public PriorityQueue(Comparison<T> comparer) : this(_defaultCapacity, comparer) { }

            /// <summary> 要素追加 </summary>
            public void Enqueue(T item)
            {
                var n = _heap.Count;
                _heap.Add(item);

                while (n > 0)
                {
                    var parent = (n - 1) / 2;
                    if (_comparer(_heap[parent], item) > 0) break;

                    _heap[n] = _heap[parent];
                    n = parent;
                }

                _heap[n] = item;
            }

            /// <summary> 要素追加 </summary>
            public void Push(T item) => Enqueue(item);

            /// <summary> 先頭の値を取得し削除する </summary>
            public T Dequeue()
            {
                if (!_heap.Any())
                {
                    throw new Exception();
                }
                var item = _heap[0];
                var last = _heap.Last();

                var n = _heap.Count - 1;
                var parent = 0;
                var child = 2 * parent + 1;
                while (child < n)
                {
                    if (child + 1 < n && _comparer(_heap[child + 1], _heap[child]) > 0)
                    {
                        child++;
                    }

                    if (_comparer(last, _heap[child]) > 0) break;

                    _heap[parent] = _heap[child];
                    parent = child;
                    child = 2 * parent + 1;
                }

                _heap[parent] = last;
                _heap.RemoveAt(n);

                return item;
            }

            /// <summary> 先頭の値を取得し削除する </summary>
            public T Pop() => Dequeue();

            /// <summary>
            /// 先頭の値取得（削除はしない）
            /// </summary>
            public T Peek()
            {
                if (!_heap.Any()) throw new Exception();

                return _heap[0];
            }

            /// <summary>
            /// ヒープに格納されているアイテム数
            /// </summary>
            public int Count() => _heap.Count;

            /// <summary>
            /// 一つでも値が格納されているか
            /// </summary>
            public bool Any() => _heap.Any();
        }
        #endregion
    }
}
