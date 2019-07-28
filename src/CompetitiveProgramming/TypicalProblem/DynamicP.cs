﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.TypicalProblem
{
    static class DynamicP
    {
        static int Solve(List<int> coins, int target)
        {
            // t[i]はiとなる最小のコイン枚数が格納されている
            var t = Enumerable.Repeat(int.MaxValue, target + 1).ToList();

            t[0] = 0;
            foreach (var i in Enumerable.Range(0, coins.Count()))
            {
                for (int c = coins[i]; c <= target; c++)
                {
                    // 今のコインを使った方が枚数が小さくなるなら採用する
                    t[c] = Math.Min(t[c], t[c - coins[i]] + 1);
                }
            }

            return t[target];
        }
    }
}
