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
    public class StronglyConnectedComponents
    {
        private readonly List<int>[] _es;
        // 逆向きの辺
        private readonly List<int>[] _res;
        private readonly bool[] _visited;
        private readonly int[] _orders;
        private readonly List<List<int>> _sccs;
        private readonly int[] _grouping;
        private int _order;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public StronglyConnectedComponents(int v)
        {
            _es = Enumerable.Repeat(0, v)
                .Select(_ => new List<int>())
                .ToArray();
            _res = Enumerable.Repeat(0, v)
                .Select(_ => new List<int>())
                .ToArray();
            _sccs = new List<List<int>>();
            _grouping = new int[v];

            _visited = new bool[v];
            _orders = new int[v];
            _order = 0;
        }

        /// <summary>
        /// 辺の追加
        /// </summary>
        public void Add(int from, int to)
        {
            _es[from].Add(to);
            _res[to].Add(from);

            if (_sccs.Any()) _sccs.Clear();
        }

        /// <summary>
        /// 逆向きの巡回の順番を決める
        /// </summary>
        private void GoRound(int s)
        {
            if (_visited[s]) return;

            _visited[s] = true;
            foreach (var e in _es[s])
            {
                GoRound(e);
            }

            _orders[s] = _order++;
        }

        /// <summary>
        /// 逆向きの辺を巡回しながら格納する
        /// </summary>
        private bool Resolve(int id, int s, List<int> scc)
        {
            if (_visited[s]) return false;

            _visited[s] = true;

            scc.Add(s);
            _grouping[s] = id;
            foreach (var e in _res[s])
            {
                Resolve(id, e, scc);
            }

            return true;
        }

        /// <summary>
        /// 強連結成分分解
        /// </summary>
        private void Resolve()
        {
            if (_sccs.Any()) return;

            // 一度全体を巡回して、逆順巡回の順番を決める
            for (int i = 0; i < _es.Length; i++)
            {
                GoRound(i);
            }

            for (int i = 0; i < _visited.Length; i++)
            {
                _visited[i] = false;
            }

            // 逆順に巡回する
            var ords = _orders.Select((x, i) => new { Id = i, Order = x })
                .OrderByDescending(x => x.Order);
            foreach (var item in ords)
            {
                var scc = new List<int>();
                if (Resolve(item.Id, item.Id, scc))
                {
                    _sccs.Add(scc);
                }
            }
        }

        /// <summary>
        /// 強連結成分ごとのリストを返却
        /// </summary>
        public IReadOnlyList<IReadOnlyList<int>> Sccs()
        {
            if (!_sccs.Any()) Resolve();

            return _sccs;
        }

        /// <summary>
        /// 強連結成分ごとに独立な番号を格納した配列を返却
        /// (Union Findみたいな感じ)
        /// </summary>
        public IReadOnlyList<int> Grouping()
        {
            if (!_sccs.Any()) Resolve();

            return _grouping;
        }

        /// <summary>
        /// 同一の強連結成分に属するか
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool Same(int u, int v)
        {
            if (!_sccs.Any()) Resolve();

            return _grouping[u] == _grouping[v];
        }
    }
}
