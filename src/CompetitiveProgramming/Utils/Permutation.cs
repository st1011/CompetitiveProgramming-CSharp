using System;
using System.Collections.Generic;

/// <summary>
/// https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/5/ITP2_5_C
/// https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/5/ITP2_5_D
/// </summary>
namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 順列操作
    /// </summary>
    static class Permutation
    {
        /// <summary>
        /// arrayを辞書順で次の順列へ置き換える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">対象順列 / 破壊的操作が行なわれることに注意</param>
        /// <param name="index"></param>
        /// <param name="length"></param>
        /// <param name="comp"></param>
        /// <returns>次の順列が存在する場合はtrue</returns>
        private static bool NextPermutation<T>(T[] array, int index, int length, Comparison<T> comp)
        {
            if (length <= 1) return false;
            for (int i = length - 1; i > 0; i--)
            {
                int k = i - 1;
                if (comp(array[k], array[i]) < 0)
                {
                    int j = Array.FindLastIndex(array, x => comp(array[k], x) < 0);
                    Swap(ref array[k], ref array[j]);
                    Array.Reverse(array, i, length - i);
                    return true;
                }
            }
            Array.Reverse(array, index, length);
            return false;
        }

        private static void Swap<T>(ref T x, ref T y) { T tmp = x; x = y; y = tmp; }

        /// <summary>
        /// arrayを辞書順で前の順列へ置き換える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">対象順列 / 破壊的操作が行なわれることに注意</param>
        /// <returns>前の順列が存在する場合はtrue</returns>
        public static bool PrevPermutation<T>(T[] array) where T : IComparable<T>
        {
            return NextPermutation(array, 0, array.Length, (x, y) => Comparer<T>.Default.Compare(y, x));
        }

        /// <summary>
        /// arrayを辞書順で次の順列へ置き換える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">対象順列 / 破壊的操作が行なわれることに注意</param>
        /// <returns>次の順列が存在する場合はtrue</returns>
        /// <remarks>
        /// C++のnext_permutation()のような関数
        /// </remarks>
        public static bool NextPermutation<T>(T[] array) where T : IComparable<T>
        {
            return NextPermutation(array, 0, array.Length, Comparer<T>.Default.Compare);
        }

        /// <summary>
        /// arrayの辞書順順列を全列挙する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">昇順ソート済み順列 / 破壊的操作が行なわれることに注意</param>
        /// <returns></returns>
        /// <remarks>
        /// 厳密には全列挙ではなく現在の順列から辞書順末尾までの順列
        /// </remarks>
        public static IEnumerable<IReadOnlyList<T>> Permutations<T>(T[] array) where T : IComparable<T>
        {
            do
            {
                yield return array;
            } while (NextPermutation(array));
        }
    }
}
