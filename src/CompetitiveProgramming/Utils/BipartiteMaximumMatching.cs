using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_7_A&lang=ja
/// https://judge.yosupo.jp/problem/bipartitematching
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大二部マッチング
    /// Hopcroft-Karp
    /// O(E sqrt(V))
    /// </summary>
    class BipartiteMaximumMatching
    {
        // 前半が左側のノード、続いて右側のノードが存在するとして扱う
        readonly List<int>[] G;

        // 最大マッチングの時の相手
        readonly int[] Match;

        readonly int lV, rV;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv">左グラフのノード数</param>
        /// <param name="rv">右グラフのノード数</param>
        public BipartiteMaximumMatching(int lv, int rv)
        {
            var v = lv + rv;

            G = Enumerable.Range(0, v)
                .Select(_ => new List<int>())
                .ToArray();

            Match = Enumerable.Repeat(-1, v).ToArray();

            lV = lv;
            rV = rv;
        }

        /// <summary>
        /// ペアの追加
        /// </summary>
        /// <param name="l">0-index</param>
        /// <param name="r">0-index</param>
        public void Add(int l, int r)
        {
            G[l].Add(r + lV);
            G[r + lV].Add(l);
        }

        void Bfs(int[] dist, bool[] matched)
        {
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = -1;
            }

            var q = new Queue<int>();
            for (int i = 0; i < G.Length; i++)
            {
                if (!matched[i])
                {
                    q.Enqueue(i);
                    dist[i] = 0;
                }
            }

            while (q.Any())
            {
                var v = q.Dequeue();
                foreach (var e in G[v])
                {
                    var next = Match[e];
                    if (next >= 0 && dist[next] < 0)
                    {
                        dist[next] = dist[v] + 1;
                        q.Enqueue(next);
                    }
                }
            }
        }

        bool Dfs(int v, int[] dist, bool[] matched, bool[] seen)
        {
            if (seen[v]) { return false; }

            seen[v] = true;
            foreach (var e in G[v])
            {
                var next = Match[e];
                if (next < 0 || (!seen[next] && dist[next] == dist[v] + 1 && Dfs(next, dist, matched, seen)))
                {
                    Match[e] = v;
                    matched[v] = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 最大二部マッチング演算
        /// </summary>
        int SolveMatching()
        {
            var dist = new int[G.Length];
            var matched = new bool[G.Length];
            var seen = new bool[G.Length];

            int matchCount = 0;
            while (true)
            {
                Bfs(dist, matched);

                for (int i = 0; i < G.Length; i++)
                {
                    seen[i] = false;
                }

                int flow = 0;
                for (int i = 0; i < G.Length; i++)
                {
                    if (!matched[i] && Dfs(i, dist, matched, seen))
                    {
                        flow++;
                    }
                }

                if (flow == 0)
                {
                    // 無向辺として登録したので、2倍カウントされている
                    return matchCount / 2;
                }

                matchCount += flow;
            }
        }

        /// <summary>
        /// 最大二部マッチング演算
        /// 2回以上呼び出されることは想定していないので注意
        /// </summary>
        /// <returns>
        /// Item1: マッチング数
        /// Item2: 左グラフ[i]とマッチングする相手
        /// Item3: 右グラフ[i]とマッチングする相手
        /// </returns>
        public Tuple<int, int[], int[]> Matching()
        {
            var n = SolveMatching();

            var l = new int[lV];
            var r = new int[rV];

            Array.Copy(Match, 0, l, 0, lV);
            Array.Copy(Match, lV, r, 0, rV);

            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] >= 0)
                {
                    l[i] -= lV;
                }
            }

            return new Tuple<int, int[], int[]>(n, l, r);
        }
    }
}
