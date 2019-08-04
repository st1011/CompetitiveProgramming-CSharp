using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最大二部マッチング
    /// </summary>
    class BipartiteMaximumMatching
    {
        readonly int V;
        readonly List<int>[] G;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="v">ノード数</param>
        public BipartiteMaximumMatching(int v)
        {
            G = Enumerable.Range(0, v)
                .Select(_ => new List<int>())
                .ToArray();

            V = v;
        }

        /// <summary>
        /// パスの追加
        /// </summary>
        public void Add(int v1, int v2)
        {
            G[v1].Add(v2);
            G[v2].Add(v1);
        }

        /// <summary>
        /// 増加パスを探す
        /// </summary>
        bool Dfs(int[] match, bool[] used, int v)
        {
            used[v] = true;
            for (int i = 0; i < G[v].Count(); i++)
            {
                var u = G[v][i];
                var w = match[u];
                if (w < 0 || !used[w] && Dfs(match, used, w))
                {
                    match[v] = u;
                    match[u] = v;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 最大二部マッチングのペアリストと数を返す
        /// </summary>
        public Tuple<int[], int> Matching()
        {
            int res = 0;

            // マッチする相手
            var match = Enumerable.Repeat(-1, V).ToArray();
            for (int i = 0; i < V; i++)
            {
                if (match[i] < 0)
                {
                    var used = new bool[V];
                    if (Dfs(match, used, i))
                    {
                        res++;
                    }
                }
            }

            return new Tuple<int[], int>(match, res);
        }
    }
}
