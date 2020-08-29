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
    /// デフォルトは降順ソート
    /// </summary>
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

        /// <summary> 要素追加 </summary>
        public void Enqueue(T item)
        {
            var n = _heap.Count;
            _heap.Add(item);

            while (n > 0)
            {
                var parent = (n - 1) / 2;
                if (_comparer(_heap[parent], item) > 0) break;

                _heap[n] = _heap[parent];
                n = parent;
            }

            _heap[n] = item;
        }

        /// <summary> 要素追加 </summary>
        public void Push(T item) => Enqueue(item);

        /// <summary> 先頭の値を取得し削除する </summary>
        public T Dequeue()
        {
            if (!_heap.Any())
            {
                throw new Exception();
            }
            var item = _heap[0];
            var last = _heap.Last();

            var n = _heap.Count - 1;
            var parent = 0;
            var child = 2 * parent + 1;
            while (child < n)
            {
                if (child + 1 < n && _comparer(_heap[child + 1], _heap[child]) > 0)
                {
                    child++;
                }

                if (_comparer(last, _heap[child]) > 0) break;

                _heap[parent] = _heap[child];
                parent = child;
                child = 2 * parent + 1;
            }

            _heap[parent] = last;
            _heap.RemoveAt(n);

            return item;
        }

        /// <summary> 先頭の値を取得し削除する </summary>
        public T Pop() => Dequeue();

        /// <summary>
        /// 先頭の値取得（削除はしない）
        /// </summary>
        public T Peek()
        {
            if (!_heap.Any()) throw new Exception();

            return _heap[0];
        }

        /// <summary>
        /// ヒープに格納されているアイテム数
        /// </summary>
        public int Count() => _heap.Count;

        /// <summary>
        /// 一つでも値が格納されているか
        /// </summary>
        public bool Any() => _heap.Any();
    }

    /// <summary>
    /// AIZUの教科書方式
    /// 最初の要素数を超えられない
    /// 上よりはちょっと早い
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PriorityQueueAizu<T> where T : IComparable
    {
        private readonly T[] _heap;
        private int _heapSize;
        private readonly Func<T, T, bool> _compare;

        public PriorityQueueAizu(int heapSize, bool isMinHeap = true)
        {
            // 1-originなので
            _heap = new T[heapSize + 1];
            _heapSize = 0;

            if (isMinHeap)
            {
                _compare = (x, y) => x.CompareTo(y) < 0;
            }
            else
            {
                _compare = (x, y) => x.CompareTo(y) > 0;
            }
        }

        public void Insert(T key)
        {
            _heapSize++;

            HeapIncreaseKey(_heapSize, key);
        }

        private void HeapIncreaseKey(int index, T key)
        {
            _heap[index] = key;

            var parent = ParentIndex(index);
            while (index > 1 && _compare(_heap[index], _heap[parent]))
            {
                Swap(index, parent);
                index = parent;
                parent = ParentIndex(index);
            }
        }

        public T Extract()
        {
            if (_heapSize < 1)
            {
                throw new Exception();
            }

            var max = _heap[1];
            _heap[1] = _heap[_heapSize];
            _heapSize--;
            MaxHeapify(1);

            return max;
        }

        public void MaxHeapify(int index)
        {
            var left = LeftIndex(index);
            var right = RightIndex(index);

            // 最大値のノードを選ぶ
            int largestIndex = index;
            if (Available(left) && _compare(_heap[left], _heap[largestIndex]))
            {
                largestIndex = left;
            }
            if (Available(right) && _compare(_heap[right], _heap[largestIndex]))
            {
                largestIndex = right;
            }

            if (largestIndex != index)
            {
                Swap(largestIndex, index);
                MaxHeapify(largestIndex);
            }
        }

        private void Swap(int a, int b)
        {
            T temp = _heap[a];
            _heap[a] = _heap[b];
            _heap[b] = temp;
        }

        private bool Available(int index) => index >= 1 && index <= _heapSize;
        private static int ParentIndex(int index) => index / 2;
        private static int LeftIndex(int index) => 2 * index;
        private static int RightIndex(int index) => 2 * index + 1;
    }
}
