using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// http://judge.u-aizu.ac.jp/onlinejudge/description.jsp?id=ALDS1_9_C&lang=jp
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 優先度付きキュー(二分ヒープ)
    /// デフォルトは最大ヒープ
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("[0]={_heap[0]}, Count={Count}")]
    public class PriorityQueue<T>
    {
        private const int _defaultCapacity = 8;
        private readonly List<T> _heap;
        private readonly Comparison<T> _comparer;

        public PriorityQueue(int capacity, Comparison<T> comparer)
        {
            _heap = new List<T>(capacity);
            _comparer = comparer;
        }

        public PriorityQueue() : this(_defaultCapacity) { }
        public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default.Compare) { }
        public PriorityQueue(Comparison<T> comparer) : this(_defaultCapacity, comparer) { }

        public static PriorityQueue<T> CreateMinPriorityQueue(int capacity)
            => new PriorityQueue<T>(capacity, (x, y) => -Comparer<T>.Default.Compare(x, y));

        public static PriorityQueue<T> CreateMinPriorityQueue()
            => CreateMinPriorityQueue(_defaultCapacity);

        /// <summary>
        /// 要素追加
        /// </summary>
        public void Enqueue(T item)
        {
            var child = _heap.Count;
            _heap.Add(item);
            while (child > 0)
            {
                var parent = Parent(child);
                if (_comparer(_heap[parent], item) > 0) { break; }

                // 親子関係が正しくないので親を子側にずらす
                _heap[child] = _heap[parent];
                child = parent;
            }

            _heap[child] = item;
        }

        /// <summary>
        /// 先頭の値を取得し削除する
        /// </summary>
        public T Dequeue()
        {
            var rootItem = _heap[0];

            var n = _heap.Count - 1;
            var parent = 0;
            var slideItem = _heap.Last();
            while (true)
            {
                int child = GetChildNearRoot(parent, n);
                if (child < 0) { break; }

                // 根に近い方の子より、さらにlastの方が根に近いので
                // この親にならlastを入れてOK
                if (_comparer(slideItem, _heap[child]) > 0) { break; }

                // lastより子の方が根に近かった場合は
                // 子を親側にずらして、今ずらしたところへlastを入れられるか再探索する
                _heap[parent] = _heap[child];
                parent = child;
            }

            _heap[parent] = slideItem;
            // 末尾を削除する場合はコピーなどが発生しないので
            // Listを自前実装しなくてもよい
            _heap.RemoveAt(n);

            return rootItem;
        }

        /// <summary>
        /// 根に近い値を持つ子のindexを返す
        /// </summary>
        private int GetChildNearRoot(int parent, int n)
        {
            int left = Left(parent);
            if (left >= n) { return -1; }

            int right = left + 1;
            if (right < n && _comparer(_heap[right], _heap[left]) > 0)
            {
                // より根に近い方の子供を選択
                return right;
            }

            return left;
        }

        #region Alias
        /// <summary>
        /// 要素追加
        /// </summary>
        public void Push(T item) => Enqueue(item);

        /// <summary>
        /// 先頭の値を取得し削除する
        /// </summary>
        public T Pop() => Dequeue();
        #endregion

        /// <summary>
        /// 先頭の値取得（削除はしない）
        /// </summary>
        public T Peek() => _heap[0];

        /// <summary>
        /// ヒープに格納されているアイテム数
        /// </summary>
        public int Count { get { return _heap.Count; } }

        /// <summary>
        /// 一つでも値が格納されているか
        /// </summary>
        public bool Any() => _heap.Any();

        private int Left(int index) => index * 2 + 1;
        private int Parent(int index) => (index - 1) / 2;
    }
}
