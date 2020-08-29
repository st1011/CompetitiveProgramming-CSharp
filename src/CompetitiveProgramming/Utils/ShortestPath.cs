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
    public class Dijkstra
    {
        private const long _inf = long.MaxValue / 3;

        // ノード数
        private readonly int _nodeCount;
        // 隣接リスト
        private readonly List<List<Edge>> _edges;
        // 前回の最短経路結果
        private readonly long[] _dist;
        // 経路復元用情報
        private readonly int[] _prev;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Dijkstra(int v)
        {
            _edges = Enumerable.Repeat(0, v)
                .Select(_ => new List<Edge>())
                .ToList();
            _dist = new long[v];
            _prev = new int[v];

            _nodeCount = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => _edges[from].Add(new Edge(to, cost));

        /// <summary>
        /// sから各ノードへの最短経路
        /// </summary>
        public IReadOnlyList<long> ShortestPath(int s)
        {
            for (int i = 0; i < _nodeCount; i++)
            {
                _dist[i] = _inf;
                _prev[i] = -1;
            }

            // 昇順ソートにする
            var pq = new PriorityQueue<Edge>((x, y) => y.CompareTo(x));

            _dist[s] = 0;
            pq.Push(new Edge(s, 0));

            while (pq.Any())
            {
                var p = pq.Pop();
                var v = p.To;

                // 他の経路の方がコストが低い
                if (_dist[v] < p.Cost) continue;

                foreach (var e in _edges[v])
                {
                    if (_dist[e.To] > _dist[v] + e.Cost)
                    {
                        // こっちの経路の方が効率的
                        _dist[e.To] = _dist[v] + e.Cost;
                        _prev[e.To] = v;
                        pq.Push(new Edge(e.To, _dist[e.To]));
                    }
                }
            }

            return _dist;
        }

        /// <summary>
        /// 経路を復元する
        /// </summary>
        /// <param name="t">目的ノード</param>
        /// <returns>探索したことがないときはnull</returns>
        public int[] RestorePath(int t)
        {
            if (_prev == null) return null;

            var stack = new Stack<int>();
            while (t >= 0)
            {
                stack.Push(t);
                t = _prev[t];
            }

            return stack.ToArray();
        }

        /// <summary>
        /// 接続されているか?
        /// </summary>
        public bool IsConnected(int e) => _dist[e] != _inf;

        private struct Edge : IComparable<Edge>
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
    }

    /// <summary>
    /// 重み付き・有向グラフの単一始点最短経路
    /// 辺の重みが負数でもOK（てか負数ないならDijkstra使って）
    /// </summary>
    public class BellmanFord
    {
        /// <summary>
        /// 負の閉路を持つか？
        /// </summary>
        public bool HasNegCycle { get; private set; } = false;

        private const long _inf = long.MaxValue / 3;
        private const long _negCycle = -_inf;

        // ノード数
        private readonly int _nodes;
        // 隣接リスト
        private readonly List<Edge> _edges;
        // 前回の最短経路結果
        private readonly long[] _dist;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public BellmanFord(int v)
        {
            _edges = new List<Edge>();
            _dist = new long[v];
            _nodes = v;
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => _edges.Add(new Edge(from, to, cost));

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
        public IReadOnlyList<long> ShortestPath(int s, bool needsDetail=false)
        {
            var E = _edges.Count;
            for (int i = 0; i < _nodes; i++)
            {
                _dist[i] = _inf;
            }

            _dist[s] = 0;
            HasNegCycle = false;
            int count = RelaxEdges(E, _dist, (x, y) => x + y);

            // 閉路が一つでもある
            HasNegCycle = count > _nodes - 1;

            if (needsDetail)
            {
                // 詳細な負の閉路検出
                RelaxEdges(E, _dist, (x, y) => _negCycle);
            }

            return _dist;
        }

        /// <summary>
        /// 辺の緩和
        /// </summary>
        private int RelaxEdges(int E, long[] d, Func<long, long, long> relax)
        {
            int count;
            for (count = 0; count < _nodes; count++)
            {
                var update = false;
                for (int i = 0; i < E; i++)
                {
                    var e = _edges[i];
                    if (d[e.From] == _inf) continue;
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
        public bool IsConnected(int t) => _dist[t] != _inf;

        /// <summary>
        /// 負の閉路と接続されているか？
        /// </summary>
        /// <remarks>
        /// ShortestPath で needsDetail=false を指定した場合は使えない
        /// </remarks>
        public bool IsConnectedNegCycle(int t) => _dist[t] == _negCycle;

        private struct Edge
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
    public class WarshallFloyd
    {
        /// <summary>
        /// 負の閉路を持つか？
        /// </summary>
        public bool HasNegCycle { get; private set; } = false;

        private const long _inf = long.MaxValue / 3;

        // ノード数
        private readonly int _nodeCount;
        // 隣接行列
        private readonly long[,] _matrix;
        // 前回の最短経路結果
        private readonly List<List<long>> _dist;
        // 経路復元用
        private readonly int[,] _next;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public WarshallFloyd(int v)
        {
            _matrix = new long[v, v];
            _next = new int[v, v];
            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < v; j++)
                {
                    _matrix[i, j] = (i == j) ? 0 : _inf;
                    _next[i, j] = j;
                }
            }

            _nodeCount = v;
            _dist = Enumerable.Range(0, v)
                .Select(_ => Enumerable.Repeat(_inf, v).ToList())
                .ToList();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void AddPath(int from, int to, long cost)
            => _matrix[from, to] = cost;

        /// <summary>
        /// sから各ノードへの最短経路
        /// 負の閉路がある場合HasNegCycleがtrueになる
        /// </summary>
        public IReadOnlyList<IReadOnlyList<long>> ShortestPath()
        {
            for (int i = 0; i < _dist.Count; i++)
            {
                for (int j = 0; j < _dist[0].Count; j++)
                {
                    _dist[i][j] = _matrix[i, j];
                }

            }

            HasNegCycle = false;
            for (int i = 0; i < _nodeCount; i++)
            {
                for (int j = 0; j < _nodeCount; j++)
                {
                    for (int k = 0; k < _nodeCount; k++)
                    {
                        if (_dist[j][i] == _inf || _dist[i][k] == _inf) continue;
                        if (_dist[j][k] <= _dist[j][i] + _dist[i][k]) continue;

                        _dist[j][k] = _dist[j][i] + _dist[i][k];
                        _next[j, k] = _next[j, i];
                    }
                }
            }

            // 負の閉路がある？
            for (int i = 0; i < _nodeCount; i++)
            {
                if (_dist[i][i] < 0)
                {
                    HasNegCycle = true;
                    break;
                }
            }

            return _dist;
        }

        /// <summary>
        /// 経路復元
        /// 探索したことがなかったり、接続されていない経路であればnull
        /// </summary>
        public IEnumerable<int> RestorePath(int s, int g)
        {
            if (_dist[0][0] == _inf) { throw new InvalidOperationException(); }
            if (!IsConnected(s, g)) { yield break; }

            var current = s;
            while (current != g)
            {
                yield return current;

                current = _next[current, g];
            }

            yield return current;
        }

        /// <summary>
        /// 接続されているか?
        /// </summary>
        public bool IsConnected(int s, int g) => _dist[s][g] != _inf;
    }
}
