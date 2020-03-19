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
    class MaxFlow
    {
        static readonly int INF = int.MaxValue / 3;

        class Edge
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

        readonly List<Edge>[] G;

        // sからの距離
        readonly int[] Level;

        // 調査済みフラグ
        readonly int[] Iter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n">ノード数</param>
        public MaxFlow(int n)
        {
            G = Enumerable.Range(0, n)
                .Select(_ => new List<Edge>())
                .ToArray();

            Level = new int[n];
            Iter = new int[n];
        }

        /// <summary>
        /// 流路追加
        /// </summary>
        public void Add(int from, int to, int cap)
        {
            G[from].Add(new Edge(to, cap, G[to].Count));
            G[to].Add(new Edge(from, 0, G[from].Count - 1));
        }

        void Bfs(int s)
        {
            for (int i = 0; i < Level.Length; i++)
            {
                Level[i] = -1;
            }
            var q = new Queue<int>();

            Level[s] = 0;
            q.Enqueue(s);

            while (q.Any())
            {
                var v = q.Dequeue();
                for (int i = 0; i < G[v].Count; i++)
                {
                    var e = G[v][i];
                    if (e.Capacity > 0 && Level[e.To] < 0)
                    {
                        Level[e.To] = Level[v] + 1;
                        q.Enqueue(e.To);
                    }
                }
            }
        }

        int Dfs(int v, int t, int f)
        {
            if (v == t) return f;
            for (int i = Iter[v]; i < G[v].Count; Iter[v] = ++i)
            {
                var e = G[v][i];
                if (e.Capacity > 0 && Level[v] < Level[e.To])
                {
                    int d = Dfs(e.To, t, Math.Min(f, e.Capacity));
                    if (d > 0)
                    {
                        e.Capacity -= d;
                        G[e.To][e.Rev].Capacity += d;
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
                if (Level[t] < 0) return flow;
                for (int i = 0; i < Iter.Length; i++)
                {
                    Iter[i] = 0;
                }

                int f;
                while ((f = Dfs(s, t, INF)) > 0)
                {
                    flow += f;
                }
            }
        }
    }
}
