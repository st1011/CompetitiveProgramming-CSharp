using System;
using System.Collections.Generic;

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
        /// <param name="array"></param>
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
        /// arrayを辞書順で次の順列へ置き換える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns>次の順列が存在する場合はtrue</returns>
        /// <remarks>
        /// C++のnext_permutation()のような関数
        /// arrayは初回ソート済みで2回目以降は前回の結果をそのまま渡す
        /// </remarks>
        public static bool NextPermutation<T>(T[] array) where T : IComparable<T>
        {
            return NextPermutation(array, 0, array.Length, Comparer<T>.Default.Compare);
        }

        /// <summary>
        /// sourceを
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">ソート済み配列</param>
        /// <returns></returns>
        public static IEnumerable<IReadOnlyList<T>> Permutations<T>(T[] source) where T : IComparable<T>
        {
            var temporary = source.Clone() as T[];
            do
            {
                yield return temporary;
            } while (NextPermutation(temporary));
        }
    }
}
