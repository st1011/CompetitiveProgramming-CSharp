using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            var n = Heap.Count();
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

            var n = Heap.Count() - 1;
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
            if (!this.Heap.Any()) throw new Exception();

            return this.Heap[0];
        }

        /// <summary>
        /// ヒープに格納されているアイテム数
        /// </summary>
        public int Count() => this.Heap.Count();

        /// <summary>
        /// 一つでも値が格納されているか
        /// </summary>
        public bool Any() => this.Heap.Any();
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
            this.Heap = new T[heapSize + 1];
            this.HeapSize = 0;

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
            this.HeapSize++;

            HeapIncreaseKey(this.HeapSize, key);
        }

        void HeapIncreaseKey(int index, T key)
        {
            this.Heap[index] = key;

            var parent = ParentIndex(index);
            while (index > 1 && compare(this.Heap[index], this.Heap[parent]))
            {
                Swap(index, parent);
                index = parent;
                parent = ParentIndex(index);
            }
        }

        public T Extract()
        {
            if (this.HeapSize < 1)
            {
                throw new Exception();
            }

            var max = this.Heap[1];
            this.Heap[1] = this.Heap[this.HeapSize];
            this.HeapSize--;
            MaxHeapify(1);

            return max;
        }

        public void MaxHeapify(int index)
        {
            var left = LeftIndex(index);
            var right = RightIndex(index);

            // 最大値のノードを選ぶ
            int largestIndex = index;
            if (Available(left) && compare(this.Heap[left], this.Heap[largestIndex]))
            {
                largestIndex = left;
            }
            if (Available(right) && compare(this.Heap[right], this.Heap[largestIndex]))
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
            T temp = this.Heap[a];
            this.Heap[a] = this.Heap[b];
            this.Heap[b] = temp;
        }

        bool Available(int index) => index >= 1 && index <= this.HeapSize;
        int ParentIndex(int index) => index / 2;
        int LeftIndex(int index) => 2 * index;
        int RightIndex(int index) => 2 * index + 1;
    }
}
