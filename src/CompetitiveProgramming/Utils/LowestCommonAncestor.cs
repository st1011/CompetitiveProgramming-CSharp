using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_5_C
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小共通祖先
    /// (探索ノードに一番近い共通ノード)
    /// </summary>
    class Lca
    {
        readonly int N;
        readonly int Root;

        readonly List<int>[] G;
        // [i][j]: jの2^i個上のノード番号
        int[][] Parent;
        // 該当ノードのrootからの深さ
        int[] Depth;

        /// <summary>
        /// </summary>
        /// <param name="n">ノード数</param>
        /// <param name="root">rootノード番号</param>
        public Lca(int n, int root = 0)
        {
            N = n;
            Root = root;

            G = Enumerable.Range(0, n)
                .Select(_ => new List<int>())
                .ToArray();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void Add(int parent, int child)
        {
            G[parent].Add(child);
            G[child].Add(parent);
        }

        /// <summary>
        /// LCA用のダブリング作成
        /// </summary>
        void Build()
        {
            int n = 1;
            while ((1 << n) < N) n++;

            Parent = Enumerable.Range(0, n)
                .Select(_ => Enumerable.Repeat(-1, N).ToArray())
                .ToArray();
            Depth = Enumerable.Repeat(-1, N).ToArray();

            Dfs(Root, -1, 0);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (Parent[i][j] != -1)
                    {
                        Parent[i + 1][j] = Parent[i][Parent[i][j]];
                    }
                }
            }
        }

        /// <summary>
        /// ダブリング
        /// </summary>
        void Dfs(int v, int p, int d)
        {
            Parent[0][v] = p;
            Depth[v] = d;

            foreach (var e in G[v])
            {
                if (e == p) continue;
                Dfs(e, v, d + 1);
            }
        }

        /// <summary>
        /// 共通祖先の取得
        /// </summary>
        public int Get(int u, int v)
        {
            if (Depth == null) Build();

            if (Depth[u] > Depth[v]) Swap(ref u, ref v);

            for (int i = 0; i < Parent.Length; i++)
            {
                var d = Depth[v] - Depth[u];
                if (((d >> i) & 1) != 0)
                {
                    v = Parent[i][v];
                }
            }

            if (u == v) return u;

            for (int i = Parent.Length - 1; i >= 0; i--)
            {
                if (Parent[i][u] != Parent[i][v])
                {
                    u = Parent[i][u];
                    v = Parent[i][v];
                }
            }

            return Parent[0][u];
        }

        /// <summary>
        /// swap
        /// </summary>
        static void Swap<T>(ref T x, ref T y)
        {
            var z = x;
            x = y;
            y = z;
        }
    }
}
