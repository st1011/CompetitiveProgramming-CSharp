using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    class LongestPath
    {
        /// <summary>
        /// **最長** 経路
        /// n頂点の有向グラフ
        /// </summary>
        /// <param name="nodes">ノードの接続先ノード番号</param>
        /// <param name="nodeEnter">ノードに入ってくる辺の数。破壊的操作あり</param>
        /// <returns></returns>
        private static List<int> Bfs(List<List<int>> nodes, List<int> nodeEnter)
        {
            var n = nodes.Count;

            // （始点となる）誰からも接続されていないノードを追加する
            var q = new Queue<int>();
            foreach (var item in nodeEnter
                .Select((x, i) => new { V = x, I = i })
                .Where(x => x.V == 0))
            {
                q.Enqueue(item.I);
            }

            // bfs-main
            var dp = Enumerable.Repeat(0, n).ToList();
            while (q.Any())
            {
                var v = q.Dequeue();
                foreach (var node in nodes[v])
                {
                    // エッジ削除
                    nodeEnter[node]--;
                    if (nodeEnter[node] == 0)
                    {
                        // 新しい始点とする
                        // 実際には一つ前のノードとつながっていたので、加算
                        q.Enqueue(node);
                        dp[node] = Math.Max(dp[node], dp[v] + 1);
                    }
                }
            }

            return dp;
        }
    }
}
