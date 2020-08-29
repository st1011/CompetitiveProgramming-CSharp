using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    public class Input : IDisposable
    {
        // 変な改行コードがたまに混じっているので、一応セパレート指定する
        // スペース単独指定の方がもちろん早い
        private static readonly char[] _separator = { ' ', '\r', '\n' };
        private readonly StreamReader _streamReader;
        private readonly Queue<string> _queue;

        /// <summary>
        /// 特定のファイルから読み出したい場合はpath指定する
        /// </summary>
        public Input(string path = "")
        {
            _queue = new Queue<string>();

            if (string.IsNullOrEmpty(path)) { _streamReader = new StreamReader(Console.OpenStandardInput()); }
            else { _streamReader = new StreamReader(path); }
        }

        /// <summary>
        /// 入力予約
        /// </summary>
        public void SetText(IEnumerable<string> items)
        {
            if (items == null) { throw new ArgumentNullException(nameof(items)); }
            foreach (var item in items)
                SetText(item);
        }

        /// <summary>
        /// 入力予約
        /// </summary>
        public bool SetText(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;

            foreach (var elem in s.Trim().Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                _queue.Enqueue(elem);

            return true;
        }

        /// <summary>
        /// 要素が存在するか
        /// </summary>
        public bool Any() => _queue.Any() || Read();

        /// <summary>
        /// 内部queueに入力からの値をsplitして格納する
        /// </summary>
        private bool Read()
        {
            if (!SetText(_streamReader.ReadLine())) return false;

            if (!_queue.Any()) return Read();

            return _queue.Any();
        }

        /// <summary>
        /// 次のstringを一つ読み込む
        /// </summary>
        public string Next()
        {
            if (!_queue.Any() && !Read()) return "";

            return _queue.Dequeue();
        }

        /// <summary>
        /// 指定個数queueにたまるまでenqueueし続ける
        /// </summary>
        private bool Accumulate(int n)
        {
            while (_queue.Count < n)
                if (!Read()) return false;

            return true;
        }

        public int NextInt() => int.Parse(Next(), CultureInfo.InvariantCulture);
        public long NextLong() => long.Parse(Next(), CultureInfo.InvariantCulture);
        public double NextDouble() => double.Parse(Next(), CultureInfo.InvariantCulture);

        /// <summary>
        /// n個の要素をparseして、それぞれにoffsetをaddした配列を返す
        /// </summary>
        private T[] NextT<T>(int n, T offset, Func<string, T> parse, Func<T, T, T> add)
        {
            if (!Accumulate(n)) return null;

            var a = new T[n];

            for (int i = 0; i < n; i++)
                a[i] = add(parse(_queue.Dequeue()), offset);

            return a;
        }

        public string[] Next(int n) => NextT(n, "", x => x, (x, y) => x);
        public int[] NextInt(int n, int offset = 0) => NextT(n, offset, int.Parse, (x, y) => x + y);
        public long[] NextLong(int n, long offset = 0) => NextT(n, offset, long.Parse, (x, y) => x + y);
        public double[] NextDouble(int n, double offset = 0.0) => NextT(n, offset, double.Parse, (x, y) => x + y);

        #region IDisposable Support
        private bool _disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _streamReader.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
