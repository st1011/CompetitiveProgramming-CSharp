using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// https://onlinejudge.u-aizu.ac.jp/problems/ITP2_1_B
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 両端Queue
    /// </summary>
    public class ArrayDeque<T> : IEnumerable<T>, IEnumerable
    {
        private const int DefaultCapacity = 4;

        private T[] Array;
        private int Head;

        public int Count { get; private set; }

        public T this[int index]
        {
            get { return Array[(Head + index) % Array.Length]; }
            set { Array[(Head + index) % Array.Length] = value; }
        }

        /// <summary>
        /// 空のDeque
        /// </summary>
        public ArrayDeque()
        {
            Array = new T[DefaultCapacity];
            Head = 0;
            Count = 0;
        }

        /// <summary>
        /// 引数を初期として保持するDeque
        /// </summary>
        public ArrayDeque(IEnumerable<T> collection)
        {
            Head = 0;
            var arr = collection.ToArray();

            var m = 1;
            while (m < arr.Length)
            {
                m *= 2;
            }

            Array = new T[m];
            arr.CopyTo(Array, 0);
            Count = arr.Length;
        }


        /// <summary>
        /// [i]にxを追加
        /// </summary>
        private void Add(int i, T x)
        {
            if (Count + 1 >= Array.Length)
            {
                Resize();
            }

            if (i < Count / 2)
            {
                Head = (Head == 0) ? Array.Length - 1 : Head - 1;
                for (int k = 0; k <= i - 1; k++)
                {
                    Array[(Head + k) % Array.Length]
                        = Array[(Head + k + 1) % Array.Length];
                }
            }
            else
            {
                for (int k = Count; k > i; k--)
                {
                    Array[(Head + k) % Array.Length]
                        = Array[(Head + k - 1) % Array.Length];
                }
            }

            Array[(Head + i) % Array.Length] = x;
            Count++;
        }

        /// <summary>
        /// [i]のアイテムを削除
        /// </summary>
        private T Remove(int i)
        {
            var x = Array[(Head + 1) % Array.Length];

            if (i < Count / 2)
            {
                for (int k = i; k > 0; k--)
                {
                    Array[(Head + k) % Array.Length]
                        = Array[(Head + k - 1) % Array.Length];
                }

                Head = (Head + 1) % Array.Length;
            }
            else
            {
                for (int k = i; k < Count - i; k++)
                {
                    Array[(Head + k) % Array.Length]
                        = Array[(Head + k + 1) % Array.Length];
                }
            }

            Count--;
            if (3 * Count < Array.Length)
            {
                Resize();
            }

            return x;
        }

        /// <summary>
        /// 配列の拡張
        /// </summary>
        private void Resize()
        {
            var arr = new T[Math.Max(DefaultCapacity, 2 * Count)];
            for (int k = 0; k < Count; k++)
            {
                arr[k] = Array[(Head + k) % Array.Length];
            }

            Array = arr;
            Head = 0;
        }

        /// <summary>
        /// 先頭へ要素xを追加
        /// </summary>
        public void PushFront(T x) => Add(0, x);

        /// <summary>
        /// 末尾へ要素xを追加
        /// </summary>
        public void PushBack(T x) => Add(Count, x);

        /// <summary>
        /// 先頭の要素取得・削除
        /// </summary>
        /// <returns></returns>
        public T PopFront() => Remove(0);

        /// <summary>
        /// 末尾の要素取得・削除
        /// </summary>
        public T PopBack() => Remove(Count);

        /// <summary>
        /// 先頭の要素取得
        /// </summary>
        public T PeekFront() => this[0];

        /// <summary>
        /// 末尾の要素取得
        /// </summary>
        public T PeekBack() => this[Count - 1];

        /// <summary>
        /// 全要素削除
        /// </summary>
        public void Clear()
        {
            System.Array.Clear(Array, 0, Array.Length);
            Count = 0;
            Head = 0;
        }

        /// <summary>
        /// 要素を保持しているか
        /// </summary>
        public bool Any() => Count != 0;

        public override string ToString() => string.Join(" ", this);

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
