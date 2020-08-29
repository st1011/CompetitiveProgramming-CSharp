using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

/// <summary>
/// https://docs.microsoft.com/ja-jp/dotnet/api/system.diagnostics.stopwatch
/// </summary>
namespace CompetitiveProgramming.Utils
{
    public static class Time
    {
        private static readonly long _nanosecondsPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;

        /// <summary>
        /// 計測精度の表示
        /// </summary>
        public static void DisplayTimerProperties()
        {
            // Display the timer frequency and resolution.
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                Console.WriteLine("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Console.WriteLine("  Timer frequency in ticks per second = {0}",
                frequency);
            Console.WriteLine("  Timer is accurate within {0} nanoseconds",
                _nanosecondsPerTick);
        }

        /// <summary>
        /// メソッド実行時間の計測
        /// </summary>
        /// <param name="operation">計測対象のメソッド</param>
        /// <param name="numRepeats">計測繰返し回数</param>
        /// <param name="operationSummary">計測対象の概要</param>
        /// <param name="path">呼び出しファイルパス(通常指定しない)</param>
        /// <param name="line">呼び出しファイル行(通常指定しない)</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:一般的な例外の種類はキャッチしません", Justification = "Callbackによる例外のため")]
        public static void Measure(Action operation, int numRepeats = 100, string operationSummary = "", [CallerFilePath] string path = "", [CallerLineNumber] int line = 0)
        {
            long numTicks = 0;
            long numRollovers = 0;
            long maxTicks = 0;
            long minTicks = long.MaxValue;
            int exceptionCount = 0;
            int indexFastest = -1;
            int indexSlowest = -1;
            Stopwatch timeTotalRepeats = new Stopwatch();

            // 初回の計測はスキップするので1回余分にループする
            for (int i = 0; i < numRepeats + 1; i++)
            {
                long ticksOffset = timeTotalRepeats.ElapsedTicks;
                timeTotalRepeats.Start();
                try
                {
                    operation();
                }
                catch (Exception)
                {
                    exceptionCount++;
                }
                timeTotalRepeats.Stop();

                long ticksThisTime = timeTotalRepeats.ElapsedTicks - ticksOffset;

                // 初回の計測は平均値に悪影響を及ぼす可能性があるのでスキップする
                if (i == 0)
                {
                    timeTotalRepeats.Reset();
                }
                else
                {
                    if (maxTicks < ticksThisTime)
                    {
                        indexSlowest = i;
                        maxTicks = ticksThisTime;
                    }
                    if (minTicks > ticksThisTime)
                    {
                        indexFastest = i;
                        minTicks = ticksThisTime;
                    }
                    numTicks += ticksThisTime;
                    if (numTicks < ticksThisTime)
                    {
                        // Keep track of rollovers.
                        numRollovers++;
                    }
                }
            }

            if (string.IsNullOrEmpty(operationSummary))
            {
                operationSummary = "(Nothing)";
            }

            Console.WriteLine();
            Console.WriteLine("{0} ({1}:{2}) Summary:", operation.Method.Name, Path.GetFileName(path), line);
            Console.WriteLine("  Overview: {0}", operationSummary);
            Console.WriteLine("  Exception count: {0}", exceptionCount);
            Console.WriteLine("  Slowest time:  #{0}/{1} = {2}",
                indexSlowest, numRepeats, TicksToString(maxTicks));
            Console.WriteLine("  Fastest time:  #{0}/{1} = {2}",
                indexFastest, numRepeats, TicksToString(minTicks));
            Console.WriteLine("  Average time:  {0}",
                TicksToString(numTicks, numRepeats));
            Console.WriteLine("  Total time looping through {0} operations: {1} milliseconds",
                numRepeats, timeTotalRepeats.ElapsedMilliseconds);
        }

        /// <summary>
        /// Ticksをいくつかの単位に変換した文字列を返す
        /// </summary>
        /// <param name="ticks"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        private static string TicksToString(long ticks, long divisor = 1)
        {
            if (divisor <= 0)
            {
                throw new ArgumentException($"{nameof(divisor)} cannot be 0.", nameof(divisor));
            }

            long nanosec = ticks * _nanosecondsPerTick / divisor;
            string[] list =
            {
                $"{nanosec / (1000 * 1000)} milliseconds",
                $"{nanosec} nanoseconds",
                $"{ticks / divisor} ticks",
            };

            return string.Join(" = ", list);
        }
    }
}
