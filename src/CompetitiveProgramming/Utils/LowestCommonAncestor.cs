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
    public class Lca
    {
        private readonly int _nodeCount;
        private readonly int _root;

        private readonly List<int>[] _nodes;
        // [i][j]: jの2^i個上のノード番号
        private int[][] _parent;
        // 該当ノードのrootからの深さ
        private int[] _depth;

        /// <summary>
        /// </summary>
        /// <param name="n">ノード数</param>
        /// <param name="root">rootノード番号</param>
        public Lca(int n, int root = 0)
        {
            _nodeCount = n;
            _root = root;

            _nodes = Enumerable.Range(0, n)
                .Select(_ => new List<int>())
                .ToArray();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void Add(int parent, int child)
        {
            _nodes[parent].Add(child);
            _nodes[child].Add(parent);
        }

        /// <summary>
        /// LCA用のダブリング作成
        /// </summary>
        private void Build()
        {
            int n = 1;
            while ((1 << n) < _nodeCount) n++;

            _parent = Enumerable.Range(0, n)
                .Select(_ => Enumerable.Repeat(-1, _nodeCount).ToArray())
                .ToArray();
            _depth = Enumerable.Repeat(-1, _nodeCount).ToArray();

            Dfs(_root, -1, 0);

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < _nodeCount; j++)
                {
                    if (_parent[i][j] != -1)
                    {
                        _parent[i + 1][j] = _parent[i][_parent[i][j]];
                    }
                }
            }
        }

        /// <summary>
        /// ダブリング
        /// </summary>
        private void Dfs(int v, int p, int d)
        {
            _parent[0][v] = p;
            _depth[v] = d;

            foreach (var e in _nodes[v])
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
            if (_depth == null) Build();

            if (_depth[u] > _depth[v]) Swap(ref u, ref v);

            for (int i = 0; i < _parent.Length; i++)
            {
                var d = _depth[v] - _depth[u];
                if (((d >> i) & 1) != 0)
                {
                    v = _parent[i][v];
                }
            }

            if (u == v) return u;

            for (int i = _parent.Length - 1; i >= 0; i--)
            {
                if (_parent[i][u] != _parent[i][v])
                {
                    u = _parent[i][u];
                    v = _parent[i][v];
                }
            }

            return _parent[0][u];
        }

        /// <summary>
        /// swap
        /// </summary>
        private static void Swap<T>(ref T x, ref T y)
        {
            var z = x;
            x = y;
            y = z;
        }
    }
}
