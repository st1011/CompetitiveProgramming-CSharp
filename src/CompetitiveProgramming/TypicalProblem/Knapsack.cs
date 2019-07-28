using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompetitiveProgramming.TypicalProblem
{
    class Knapsack
    {
        class Answer
        {
            public long Max;
            public List<long> Selection;

            public Answer(long m, IEnumerable<long> s) { Max = m; Selection = s.ToList(); }
        }

        struct Item
        {
            public long Value { get; set; }
            public long Weight { get; set; }

            public Item(long v, long w)
            {
                Value = v;
                Weight = w;
            }
        }

        static Answer Solve(long wsize, List<Item> items)
        {
            var n = items.Count();
            // [i][w] iまでのitemで、wを超えない範囲で最大の価値
            var c = new long[n + 1, wsize + 1];
            var g = new bool[n + 1, wsize + 1];

            foreach (var i in Enumerable.Range(1, n))
            {
                for (long w = 1; w <= wsize; w++)
                {
                    var prev = c[i - 1, w];
                    if (items[i - 1].Weight <= w)
                    {
                        // 入れられる
                        var current = c[i - 1, w - items[i - 1].Weight] + items[i - 1].Value;
                        if (current > prev)
                        {
                            c[i, w] = current;
                            g[i, w] = true;
                        }
                        else
                        {
                            c[i, w] = prev;
                            g[i, w] = false;
                        }
                    }
                    else
                    {
                        // 入れられないので、前回を引き継ぐ
                        c[i, w] = prev;
                        g[i, w] = false;
                    }
                }
            }

            var selection = new List<long>();

            var wt = wsize;
            foreach (var i in Enumerable.Range(1, n).Reverse())
            {
                if (g[i, wt])
                {
                    selection.Add(i);
                    wt -= items[i - 1].Weight;
                }
            }

            selection.Reverse();
            return new Answer(c[n, wsize], selection);
        }
    }
}
