using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming.Utils
{
    public static class Time
    {
        public static void Measure(Action action, int repeat = 100)
        {
            var sw = new System.Diagnostics.Stopwatch();
            var tsList = new List<TimeSpan>();

            foreach (var _ in Enumerable.Range(0, repeat))
            {
                sw.Restart();
                action();
                sw.Stop();

                tsList.Add(sw.Elapsed);
            }

            Console.WriteLine("試行回数: {0}", repeat);
            Console.WriteLine("総時間: {0}ms", tsList.Sum(x => x.Milliseconds));
            Console.WriteLine("平均時間: {0}ms", tsList.Average(x => x.Milliseconds));
        }
    }
}
