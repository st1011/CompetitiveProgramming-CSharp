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
        private const int _defaultCapacity = 4;

        private T[] _data;
        private int _head;

        public int Count { get; private set; }

        public T this[int index]
        {
            get { return _data[(_head + index) % _data.Length]; }
            set { _data[(_head + index) % _data.Length] = value; }
        }

        /// <summary>
        /// 空のDeque
        /// </summary>
        public ArrayDeque()
        {
            _data = new T[_defaultCapacity];
            _head = 0;
            Count = 0;
        }

        /// <summary>
        /// 引数を初期として保持するDeque
        /// </summary>
        public ArrayDeque(IEnumerable<T> collection)
        {
            _head = 0;
            var arr = collection.ToArray();

            var m = 1;
            while (m < arr.Length)
            {
                m *= 2;
            }

            _data = new T[m];
            arr.CopyTo(_data, 0);
            Count = arr.Length;
        }


        /// <summary>
        /// [i]にxを追加
        /// </summary>
        private void Add(int i, T x)
        {
            if (Count + 1 >= _data.Length)
            {
                Resize();
            }

            if (i < Count / 2)
            {
                _head = (_head == 0) ? _data.Length - 1 : _head - 1;
                for (int k = 0; k <= i - 1; k++)
                {
                    _data[(_head + k) % _data.Length]
                        = _data[(_head + k + 1) % _data.Length];
                }
            }
            else
            {
                for (int k = Count; k > i; k--)
                {
                    _data[(_head + k) % _data.Length]
                        = _data[(_head + k - 1) % _data.Length];
                }
            }

            _data[(_head + i) % _data.Length] = x;
            Count++;
        }

        /// <summary>
        /// [i]のアイテムを削除
        /// </summary>
        private T Remove(int i)
        {
            var x = _data[(_head + 1) % _data.Length];

            if (i < Count / 2)
            {
                for (int k = i; k > 0; k--)
                {
                    _data[(_head + k) % _data.Length]
                        = _data[(_head + k - 1) % _data.Length];
                }

                _head = (_head + 1) % _data.Length;
            }
            else
            {
                for (int k = i; k < Count - i; k++)
                {
                    _data[(_head + k) % _data.Length]
                        = _data[(_head + k + 1) % _data.Length];
                }
            }

            Count--;
            if (3 * Count < _data.Length)
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
            var arr = new T[Math.Max(_defaultCapacity, 2 * Count)];
            for (int k = 0; k < Count; k++)
            {
                arr[k] = _data[(_head + k) % _data.Length];
            }

            _data = arr;
            _head = 0;
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
            System.Array.Clear(_data, 0, _data.Length);
            Count = 0;
            _head = 0;
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
