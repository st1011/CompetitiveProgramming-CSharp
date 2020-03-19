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
    class PriorityQueue<T>
    {
        static readonly int DefaultCapacity = 8;
        readonly List<T> Heap;
        readonly Comparison<T> Comparer;

        public PriorityQueue(int capacity, Comparison<T> comparer)
        {
            Heap = new List<T>(capacity);
            Comparer = comparer;
        }

        public PriorityQueue() : this(DefaultCapacity) { }
        public PriorityQueue(int capacity) : this(capacity, Comparer<T>.Default.Compare) { }
        public PriorityQueue(Comparison<T> comparer) : this(DefaultCapacity, comparer) { }

        /// <summary> 要素追加 </summary>
        public void Enqueue(T item)
        {
            var n = Heap.Count;
            Heap.Add(item);

            while (n > 0)
            {
                var parent = (n - 1) / 2;
                if (Comparer(Heap[parent], item) > 0) break;

                Heap[n] = Heap[parent];
                n = parent;
            }

            Heap[n] = item;
        }

        /// <summary> 要素追加 </summary>
        public void Push(T item) => Enqueue(item);

        /// <summary> 先頭の値を取得し削除する </summary>
        public T Dequeue()
        {
            if (!Heap.Any())
            {
                throw new Exception();
            }
            var item = Heap[0];
            var last = Heap.Last();

            var n = Heap.Count - 1;
            var parent = 0;
            var child = 2 * parent + 1;
            while (child < n)
            {
                if (child + 1 < n && Comparer(Heap[child + 1], Heap[child]) > 0)
                {
                    child++;
                }

                if (Comparer(last, Heap[child]) > 0) break;

                Heap[parent] = Heap[child];
                parent = child;
                child = 2 * parent + 1;
            }

            Heap[parent] = last;
            Heap.RemoveAt(n);

            return item;
        }

        /// <summary> 先頭の値を取得し削除する </summary>
        public T Pop() => Dequeue();

        /// <summary>
        /// 先頭の値取得（削除はしない）
        /// </summary>
        public T Peek()
        {
            if (!Heap.Any()) throw new Exception();

            return Heap[0];
        }

        /// <summary>
        /// ヒープに格納されているアイテム数
        /// </summary>
        public int Count() => Heap.Count;

        /// <summary>
        /// 一つでも値が格納されているか
        /// </summary>
        public bool Any() => Heap.Any();
    }

    /// <summary>
    /// AIZUの教科書方式
    /// 最初の要素数を超えられない
    /// 上よりはちょっと早い
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class PriorityQueueAizu<T> where T : IComparable
    {
        readonly T[] Heap;
        int HeapSize;
        readonly Func<T, T, bool> compare;

        public PriorityQueueAizu(int heapSize, bool isMinHeap = true)
        {
            // 1-originなので
            Heap = new T[heapSize + 1];
            HeapSize = 0;

            if (isMinHeap)
            {
                compare = (x, y) => x.CompareTo(y) < 0;
            }
            else
            {
                compare = (x, y) => x.CompareTo(y) > 0;
            }
        }

        public void Insert(T key)
        {
            HeapSize++;

            HeapIncreaseKey(HeapSize, key);
        }

        void HeapIncreaseKey(int index, T key)
        {
            Heap[index] = key;

            var parent = ParentIndex(index);
            while (index > 1 && compare(Heap[index], Heap[parent]))
            {
                Swap(index, parent);
                index = parent;
                parent = ParentIndex(index);
            }
        }

        public T Extract()
        {
            if (HeapSize < 1)
            {
                throw new Exception();
            }

            var max = Heap[1];
            Heap[1] = Heap[HeapSize];
            HeapSize--;
            MaxHeapify(1);

            return max;
        }

        public void MaxHeapify(int index)
        {
            var left = LeftIndex(index);
            var right = RightIndex(index);

            // 最大値のノードを選ぶ
            int largestIndex = index;
            if (Available(left) && compare(Heap[left], Heap[largestIndex]))
            {
                largestIndex = left;
            }
            if (Available(right) && compare(Heap[right], Heap[largestIndex]))
            {
                largestIndex = right;
            }

            if (largestIndex != index)
            {
                Swap(largestIndex, index);
                MaxHeapify(largestIndex);
            }
        }

        void Swap(int a, int b)
        {
            T temp = Heap[a];
            Heap[a] = Heap[b];
            Heap[b] = temp;
        }

        bool Available(int index) => index >= 1 && index <= HeapSize;
        static int ParentIndex(int index) => index / 2;
        static int LeftIndex(int index) => 2 * index;
        static int RightIndex(int index) => 2 * index + 1;
    }
}
