using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_3_C&lang=ja
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 強連結成分分解
    /// </summary>
    class StronglyConnectedComponents
    {
        readonly List<int>[] es;
        // 逆向きの辺
        readonly List<int>[] res;
        readonly bool[] Visited;
        readonly int[] Orders;
        readonly List<List<int>> sccs;
        readonly int[] grouping;

        int Order;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public StronglyConnectedComponents(int v)
        {
            es = Enumerable.Repeat(0, v)
                .Select(_ => new List<int>())
                .ToArray();
            res = Enumerable.Repeat(0, v)
                .Select(_ => new List<int>())
                .ToArray();
            sccs = new List<List<int>>();
            grouping = new int[v];

            Visited = new bool[v];
            Orders = new int[v];
            Order = 0;
        }

        /// <summary>
        /// 辺の追加
        /// </summary>
        public void Add(int from, int to)
        {
            es[from].Add(to);
            res[to].Add(from);

            if (sccs.Any()) sccs.Clear();
        }

        /// <summary>
        /// 逆向きの巡回の順番を決める
        /// </summary>
        void GoRound(int s)
        {
            if (Visited[s]) return;

            Visited[s] = true;
            foreach (var e in es[s])
            {
                GoRound(e);
            }

            Orders[s] = Order++;
        }

        /// <summary>
        /// 逆向きの辺を巡回しながら格納する
        /// </summary>
        bool Resolve(int id, int s, List<int> scc)
        {
            if (Visited[s]) return false;

            Visited[s] = true;

            scc.Add(s);
            grouping[s] = id;
            foreach (var e in res[s])
            {
                Resolve(id, e, scc);
            }

            return true;
        }

        /// <summary>
        /// 強連結成分分解
        /// </summary>
        void Resolve()
        {
            if (sccs.Any()) return;

            // 一度全体を巡回して、逆順巡回の順番を決める
            for (int i = 0; i < es.Length; i++)
            {
                GoRound(i);
            }

            for (int i = 0; i < Visited.Length; i++)
            {
                Visited[i] = false;
            }

            // 逆順に巡回する
            var ords = Orders.Select((x, i) => new { Id = i, Order = x })
                .OrderByDescending(x => x.Order);
            foreach (var item in ords)
            {
                var scc = new List<int>();
                if (Resolve(item.Id, item.Id, scc))
                {
                    sccs.Add(scc);
                }
            }
        }

        /// <summary>
        /// 強連結成分ごとのリストを返却
        /// </summary>
        public List<List<int>> Sccs()
        {
            if (!sccs.Any()) Resolve();

            return sccs;
        }

        /// <summary>
        /// 強連結成分ごとに独立な番号を格納した配列を返却
        /// (Union Findみたいな感じ)
        /// </summary>
        public int[] Grouping()
        {
            if (!sccs.Any()) Resolve();

            return grouping;
        }

        /// <summary>
        /// 同一の強連結成分に属するか
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Same(int u, int v)
        {
            if (!sccs.Any()) Resolve();

            return grouping[u] == grouping[v];
        }
    }
}
