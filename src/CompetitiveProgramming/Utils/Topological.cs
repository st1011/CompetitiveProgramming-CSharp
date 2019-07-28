using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// トポロジカルソート
    /// </summary>
    class Topological
    {
        // 接続情報
        readonly List<List<int>> es;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public Topological(int v)
        {
            es = Enumerable.Repeat(0, v)
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
            => es[from].Add(to);

        /// <summary>
        /// 各ノードへの最短経路
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public int[] Sort()
        {
            // 各頂点の出自数
            var degs = new int[es.Count()];
            // 便宜上、逆向きの辺一覧
            var edges = Enumerable.Repeat(0, es.Count())
                .Select(_ => new List<int>())
                .ToArray();
            // 出次数0の頂点一覧
            var q = new Queue<int>();

            for (int i = 0; i < es.Count(); i++)
            {
                if (!es[i].Any())
                {
                    q.Enqueue(i);
                    continue;
                }

                for (int j = 0; j < es[i].Count(); j++)
                {
                    degs[i]++;
                    edges[es[i][j]].Add(i);
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
