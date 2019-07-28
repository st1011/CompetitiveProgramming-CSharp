using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 両端Queue
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class Deque<T>
    {
        T[] Buf;
        public int Count
        {
            get { return Tail - Head - 1; }
        }
        public int Capacity
        {
            get { return Buf.Length; }
        }
        public int Head { get; set; }
        public int Tail { get; set; }

        public T this[int i]
        {
            get { return Buf[Head + 1 + i]; }
        }

        public Deque(int capacity = 16)
        {
            Buf = new T[capacity];
            Head = Buf.Length / 2 - 1;
            Tail = Buf.Length / 2;
        }

        /// <summary>
        /// 内部の配列を拡張する
        /// headかtail一方だけを連続して実行されると、無駄に配列が長くなってしまう
        /// </summary>
        void Expand()
        {
            var next = new T[Buf.Length * 2];
            Array.Copy(Buf, 0, next, Buf.Length / 2, Buf.Length);

            Head += Buf.Length / 2;
            Tail += Buf.Length / 2;

            Buf = next;
        }

        /// <summary>
        /// 先頭に要素追加
        /// </summary>
        /// <param name="item"></param>
        public void PushHead(T item)
        {
            if (Head < 0)
            {
                Expand();
            }

            Buf[Head--] = item;
        }

        /// <summary>
        /// 末尾に要素追加
        /// </summary>
        /// <param name="item"></param>
        public void PushTail(T item)
        {
            if (Tail >= Buf.Length)
            {
                Expand();
            }

            Buf[Tail++] = item;
        }

        /// <summary>
        /// 先頭の要素返却・削除
        /// </summary>
        /// <returns></returns>
        public T PopHead()
        {
            if (!Any())
            {
                throw new Exception();
            }

            return Buf[++Head];
        }

        /// <summary>
        /// 末尾の要素返却・削除
        /// </summary>
        /// <returns></returns>
        public T PopTail()
        {
            if (!Any())
            {
                throw new Exception();
            }

            return Buf[--Tail];
        }

        /// <summary>
        /// 先頭の要素返却
        /// </summary>
        /// <returns></returns>
        public T PeekHead()
        {
            if (!Any())
            {
                throw new Exception();
            }

            return Buf[Head + 1];
        }

        /// <summary>
        /// 末尾の要素返却
        /// </summary>
        /// <returns></returns>
        public T PeekTail()
        {
            if (!Any())
            {
                throw new Exception();
            }

            return Buf[Tail - 1];
        }

        public bool Any()
            => Count != 0;
    }
}
