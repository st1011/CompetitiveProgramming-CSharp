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
    public class BipartiteMaximumMatching
    {
        // 前半が左側のノード、続いて右側のノードが存在するとして扱う
        private readonly List<int>[] _nodes;

        // 最大マッチングの時の相手
        private readonly int[] _matches;

        private readonly int _leftNodeCount;
        private readonly int _rightNodeCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lv">左グラフのノード数</param>
        /// <param name="rv">右グラフのノード数</param>
        public BipartiteMaximumMatching(int lv, int rv)
        {
            var v = lv + rv;

            _nodes = Enumerable.Range(0, v)
                .Select(_ => new List<int>())
                .ToArray();

            _matches = Enumerable.Repeat(-1, v).ToArray();

            _leftNodeCount = lv;
            _rightNodeCount = rv;
        }

        /// <summary>
        /// ペアの追加
        /// </summary>
        /// <param name="l">0-index</param>
        /// <param name="r">0-index</param>
        public void Add(int l, int r)
        {
            _nodes[l].Add(r + _leftNodeCount);
            _nodes[r + _leftNodeCount].Add(l);
        }

        private void Bfs(int[] dist, bool[] matched)
        {
            for (int i = 0; i < dist.Length; i++)
            {
                dist[i] = -1;
            }

            var q = new Queue<int>();
            for (int i = 0; i < _nodes.Length; i++)
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
                foreach (var e in _nodes[v])
                {
                    var next = _matches[e];
                    if (next >= 0 && dist[next] < 0)
                    {
                        dist[next] = dist[v] + 1;
                        q.Enqueue(next);
                    }
                }
            }
        }

        private bool Dfs(int v, int[] dist, bool[] matched, bool[] seen)
        {
            if (seen[v]) { return false; }

            seen[v] = true;
            foreach (var e in _nodes[v])
            {
                var next = _matches[e];
                if (next < 0 || (!seen[next] && dist[next] == dist[v] + 1 && Dfs(next, dist, matched, seen)))
                {
                    _matches[e] = v;
                    matched[v] = true;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 最大二部マッチング演算
        /// </summary>
        private int SolveMatching()
        {
            var dist = new int[_nodes.Length];
            var matched = new bool[_nodes.Length];
            var seen = new bool[_nodes.Length];

            int matchCount = 0;
            while (true)
            {
                Bfs(dist, matched);

                for (int i = 0; i < _nodes.Length; i++)
                {
                    seen[i] = false;
                }

                int flow = 0;
                for (int i = 0; i < _nodes.Length; i++)
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

            var l = new int[_leftNodeCount];
            var r = new int[_rightNodeCount];

            Array.Copy(_matches, 0, l, 0, _leftNodeCount);
            Array.Copy(_matches, _leftNodeCount, r, 0, _rightNodeCount);

            for (int i = 0; i < l.Length; i++)
            {
                if (l[i] >= 0)
                {
                    l[i] -= _leftNodeCount;
                }
            }

            return new Tuple<int, int[], int[]>(n, l, r);
        }
    }
}
