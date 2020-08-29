using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_4_B&lang=ja
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// トポロジカルソート
    /// </summary>
    public class Topological
    {
        // 接続情報
        private readonly List<List<int>> _es;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Topological(int v)
        {
            _es = Enumerable.Repeat(0, v)
                .Select(_ => new List<int>())
                .ToList();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="cost"></param>
        public void AddPath(int from, int to)
            => _es[from].Add(to);

        /// <summary>
        /// 各ノードへの最短経路
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int[] Sort()
        {
            // 各頂点の出自数
            var degs = new int[_es.Count];
            // 便宜上、逆向きの辺一覧
            var edges = Enumerable.Repeat(0, _es.Count)
                .Select(_ => new List<int>())
                .ToArray();
            // 出次数0の頂点一覧
            var q = new Queue<int>();

            for (int i = 0; i < _es.Count; i++)
            {
                if (!_es[i].Any())
                {
                    q.Enqueue(i);
                    continue;
                }

                for (int j = 0; j < _es[i].Count; j++)
                {
                    degs[i]++;
                    edges[_es[i][j]].Add(i);
                }
            }

            var s = new Stack<int>();
            while (q.Any())
            {
                var v = q.Dequeue();
                s.Push(v);

                // 本来eへと伸びていた辺を消していく
                foreach (var e in edges[v])
                {
                    degs[e]--;
                    if (degs[e] == 0)
                    {
                        q.Enqueue(e);
                    }
                }
            }

            return s.ToArray();
        }
    }
}
