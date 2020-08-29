using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// 最大フロー: http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_6_A&lang=jp
/// 二部最大マッチング: https://atcoder.jp/contests/abc091/tasks/arc092_a
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大フロー(Dinic法)
    /// 応用：二部最大マッチング
    /// </summary>
    public class MaxFlow
    {
        private const int _inf = int.MaxValue / 3;

        private readonly List<Edge>[] _nodes;

        // sからの距離
        private readonly int[] _level;

        // 調査済みフラグ
        private readonly int[] _iter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        public MaxFlow(int n)
        {
            _nodes = Enumerable.Range(0, n)
                .Select(_ => new List<Edge>())
                .ToArray();

            _level = new int[n];
            _iter = new int[n];
        }

        /// <summary>
        /// 流路追加
        /// </summary>
        public void Add(int from, int to, int cap)
        {
            _nodes[from].Add(new Edge(to, cap, _nodes[to].Count));
            _nodes[to].Add(new Edge(from, 0, _nodes[from].Count - 1));
        }

        private void Bfs(int s)
        {
            for (int i = 0; i < _level.Length; i++)
            {
                _level[i] = -1;
            }
            var q = new Queue<int>();

            _level[s] = 0;
            q.Enqueue(s);

            while (q.Any())
            {
                var v = q.Dequeue();
                for (int i = 0; i < _nodes[v].Count; i++)
                {
                    var e = _nodes[v][i];
                    if (e.Capacity > 0 && _level[e.To] < 0)
                    {
                        _level[e.To] = _level[v] + 1;
                        q.Enqueue(e.To);
                    }
                }
            }
        }

        private int Dfs(int v, int t, int f)
        {
            if (v == t) return f;
            for (int i = _iter[v]; i < _nodes[v].Count; _iter[v] = ++i)
            {
                var e = _nodes[v][i];
                if (e.Capacity > 0 && _level[v] < _level[e.To])
                {
                    int d = Dfs(e.To, t, Math.Min(f, e.Capacity));
                    if (d > 0)
                    {
                        e.Capacity -= d;
                        _nodes[e.To][e.Rev].Capacity += d;
                        return d;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// sからtへの最大流量を調べる
        /// </summary>
        public int Flow(int s, int t)
        {
            if (s == t)
            {
                return 0;
            }

            int flow = 0;

            while (true)
            {
                Bfs(s);
                if (_level[t] < 0) return flow;
                for (int i = 0; i < _iter.Length; i++)
                {
                    _iter[i] = 0;
                }

                int f;
                while ((f = Dfs(s, t, _inf)) > 0)
                {
                    flow += f;
                }
            }
        }

        private class Edge
        {
            public int To;
            public int Capacity;
            // 逆辺
            public int Rev;

            public Edge(int to, int cap, int r)
            {
                To = to;
                Capacity = cap;
                Rev = r;
            }
        }
    }
}
