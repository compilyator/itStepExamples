namespace Step.Parallel.Pi
{
    using System;
    using System.Collections.Concurrent;
    using System.Diagnostics;
    using System.Linq;

    class Program
    {
        private static void Main()
        {
            int stepsCount;

            Console.Write("Enter steps count: ");
            if (!int.TryParse(Console.ReadLine(), out stepsCount))
            {
                Console.WriteLine("Wrong enter");
            }
            else
            {
                RunCalc(SerialPi, stepsCount, nameof(SerialPi));
                RunCalc(ParallelPi, stepsCount, nameof(ParallelPi));
                RunCalc(ParallelPartitionerPi, stepsCount, nameof(ParallelPartitionerPi));
                RunCalc(LinqPi, stepsCount, nameof(LinqPi));
                RunCalc(LinqAsParallelPi, stepsCount, nameof(LinqAsParallelPi));
                RunCalc(PLinqPi, stepsCount, nameof(PLinqPi));
            }

            Console.ReadKey();
        }

        private static void RunCalc(Func<int, double> func, int stepsCount, string name)
        {
            var stopWatch = Stopwatch.StartNew();
            var result = func(stepsCount);
            Console.WriteLine("{0,20} result: {1:F15}\tTime elapsed: {2} ms", name, result, stopWatch.ElapsedMilliseconds);
        }

        private static double SerialPi(int stepsCount)
        {
            var step = 1.0 / stepsCount;

            var pi = .0;
            for (var i = 0L; i < stepsCount; i++)
            {
                var x = step * i;
                var y = 4.0 / (1.0 + x * x);
                pi += y;
            }
            return pi * step;
        }

        private static double ParallelPi(int stepsCount)
        {
            var step = 1.0 / stepsCount;

            var pi = .0;
            var lockObj = new object();
            System.Threading.Tasks.Parallel.For(0L, stepsCount, i =>
            {
                var x = step * i;
                var y = 4.0 / (1.0 + x * x);
                lock (lockObj)
                {
                    pi += y;
                }
            });
            return pi * step;
        }

        private static double LinqPi(int stepsCount)
        {
            var step = 1.0 / stepsCount;

            return (from i in Enumerable.Range(0, stepsCount)
                    let x = step * i
                    select 4.0 / (1.0 + x * x)).Sum() * step;
        }

        private static double LinqAsParallelPi(int stepsCount)
        {
            var step = 1.0 / stepsCount;

            return (from i in Enumerable.Range(0, stepsCount)
                    let x = step * i
                    select 4.0 / (1.0 + x * x)).AsParallel().Sum() * step;
        }

        private static double PLinqPi(int stepsCount)
        {
            var step = 1.0 / stepsCount;

            return (from i in ParallelEnumerable.Range(0, stepsCount)
                    let x = step * i
                    select 4.0 / (1.0 + x * x)).Sum() * step;
        }

        private static double ParallelPartitionerPi(int stepsCount)
        {
            var sum = 0.0;
            var step = 1.0 / stepsCount;
            var monitor = new object();
            System.Threading.Tasks.Parallel.ForEach(Partitioner.Create(0, stepsCount), () => 0.0,
            (range, state, local) =>
            {
                for (var i = range.Item1; i < range.Item2; i++)
                {
                    var x = (i + 0.5) * step;
                    local += 4.0 / (1.0 + x * x);
                }
                return local;
            }, local => { lock (monitor) sum += local; });
            return step * sum;
        }
    }
}
