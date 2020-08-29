using System;
using System.Collections.Generic;

namespace CompetitiveProgramming.Utils
{
    /// <summary>
    /// 順列操作
    /// </summary>
    static class Permutation
    {
        private static bool NextPermutation<T>(T[] array, int index, int length, Comparison<T> comp)
        {
            if (length <= 1) return false;
            for (int i = length - 1; i > 0; i--)
            {
                int k = i - 1;
                if (comp(array[k], array[i]) < 0)
                {
                    int j = Array.FindLastIndex(array, delegate (T x) { return comp(array[k], x) < 0; });
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
        /// 一度呼び出すと、arrayを次の順列へ置き換える
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool NextPermutation<T>(T[] array) where T : IComparable
        {
            return NextPermutation(array, 0, array.Length, Comparer<T>.Default.Compare);
        }
    }
}
