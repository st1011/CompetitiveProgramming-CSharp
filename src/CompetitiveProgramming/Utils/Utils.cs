using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompetitiveProgramming.Utils
{
    class Utils
    {
        public static readonly int INF = int.MaxValue / 11;

        /// <summary>
        /// 値を交換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap<T>(ref T x, ref T y)
        {
            var temp = x;
            x = y;
            y = temp;
        }

        /// <summary>
        /// リストの値を交換
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void Swap<T>(IList<T> items, int x, int y) where T : struct
        {
            var temp = items[x];
            items[x] = items[y];
            items[y] = temp;
        }

        /// <summary>
        /// 2次元 enumerableのテキスト取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString<T>(IEnumerable<IEnumerable<T>> enumerable, string separator = " ")
        {
            var sb = new StringBuilder();

            foreach (var lines in enumerable)
            {
                sb.AppendLine(string.Join(separator,
                    lines.Select(x => x.ToString()).ToArray()));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 1次元 enumerableのテキスト取得
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToString<T>(IEnumerable<T> enumerable, string separator = " ")
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Join(separator, 
                enumerable.Select(x => x.ToString()).ToArray()));

            return sb.ToString();
        }

        /// <summary>
        /// Consoleへのデバッグ用出力
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="preText"></param>
        public static void WriteItems<T>(IEnumerable<T> items, string preText = "[DEBUG]")
            =>Console.WriteLine(preText + ToString(items));
    }
}
