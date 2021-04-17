using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=GRL_5_C
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 最小共通祖先
    /// </summary>
    /// <remarks>
    /// 探索ノード2つから一番近い共通ノード
    /// ダブリングによる実装
    /// </remarks>
    public class Lca
    {
        private readonly int _nodeCount;
        private readonly int _root;

        private readonly List<int>[] _nodes;
        // [i][j]: jの2^i個上のノード番号
        private int[][] _parent;
        // 該当ノードのrootからの深さ
        private int[] _depth;
        // 構築済みか
        private bool _built = false;

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

            // m=ceil(log2(n))
            int m = 1;
            while ((1 << m) < _nodeCount) { m++; }

            _parent = Enumerable.Range(0, m)
                .Select(_ => Enumerable.Repeat(-1, _nodeCount).ToArray())
                .ToArray();
            _depth = Enumerable.Repeat(-1, _nodeCount).ToArray();
        }

        /// <summary>
        /// 経路追加
        /// </summary>
        public void Add(int parent, int child)
        {
            _nodes[parent].Add(child);

            _parent[0][child] = parent;
            _built = false;
        }

        /// <summary>
        /// LCA用のダブリング作成
        /// </summary>
        private void Build()
        {
            InitializeDepth(_root);

            // 深さのダブリング
            for (int i = 0; i < _parent.Length - 1; i++)
            {
                for (int j = 0; j < _parent[i].Length; j++)
                {
                    if (_parent[i][j] != -1)
                    {
                        // jの2^(i+1)個上の親は、jの2^i個上の親の2^i個上の親
                        _parent[i + 1][j] = _parent[i][_parent[i][j]];
                    }
                }
            }

            _built = true;
        }

        /// <summary>
        /// 深さ初期化
        /// </summary>
        /// <param name="v">初期化するノード番号(最初は根)</param>
        /// <param name="d">初期化するノードの深さ</param>
        /// <remarks>
        /// 各ノードに対して深さを再帰的に設定する
        /// </remarks>
        private void InitializeDepth(int v, int d = 0)
        {
            _depth[v] = d;
            foreach (var e in _nodes[v])
            {
                InitializeDepth(e, d + 1);
            }
        }

        /// <summary>
        /// 共通祖先の取得
        /// </summary>
        /// <remarks>
        /// 一方がもう一方の親の場合はそれを共通祖先と見なす
        /// </remarks>
        public int Get(int u, int v)
        {
            if (!_built) Build();

            if (_depth[u] < _depth[v]) { Swap(ref u, ref v); }
            System.Diagnostics.Debug.Assert(_depth[u] >= _depth[v]);

            // uをvと同じ高さまで引き上げる
            u = SameLevelAncestor(u, v);

            // 同じならそれが共通祖先
            if (u == v) return u;

            // 親が共通となるまで上がっていく
            for (int i = _parent.Length - 1; i >= 0; i--)
            {
                // 親が共通なので、u!=vの範囲で上がっていけば良い
                if (_parent[i][u] != _parent[i][v])
                {
                    u = _parent[i][u];
                    v = _parent[i][v];
                }
            }

            // 親が共通なので適当にどちらかの直接の親を帰す
            return _parent[0][u];
        }

        /// <summary>
        /// uの(自身を含む)祖先で、vと同じ深さのノードを求める
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        /// <remarks>
        /// uの深さ >= vの深さであること
        /// </remarks>
        private int SameLevelAncestor(int u, int v)
        {
            System.Diagnostics.Debug.Assert(_depth[u] >= _depth[v]);
            // uのdiff個上の親が分かれば良い
            int diff = _depth[u] - _depth[v];
            for (int i = 0; i < _parent.Length; i++)
            {
                // 目的の親に到達するために2^i個上に移動する必要があるか？
                if (((diff >> i) & 1) != 0)
                {
                    u = _parent[i][u];
                }
            }

            return u;
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
