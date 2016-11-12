namespace Tasks
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            var task = Task<int>.Factory.StartNew(() => Start(100, token), token);
            var result = task.Wait(TimeSpan.FromSeconds(1));
            if (!result)
            {
                tokenSource.Cancel(true);
            }

            try
            {
                var sum = task.Result;
                Console.WriteLine($"Sum = {sum}");
            }
            catch (AggregateException e)
            {
                Console.WriteLine(
                    e.InnerExceptions.Aggregate(
                        seed: string.Empty,
                        func: (s, exception) => s += exception.Message + Environment.NewLine,
                        resultSelector: s => s.Length > 0 ? s.Substring(0, s.Length - 2) : s));
            }

            var tasks = Enumerable.Range(1, 10).Select(
                i => Task.Factory.StartNew(
                    () =>
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine(i);
                        })).ToArray();

            Task.WaitAll(tasks);

            Console.ReadLine();
        }

        private static int Start(int max, CancellationToken token)
        {
            Console.WriteLine($"Thread id = {Thread.CurrentThread.ManagedThreadId}");
            var sum = 0;
            var random = new Random();
            for (int i = 1; i <= max; i++)
            {
                if (token.IsCancellationRequested)
                {
                    token.ThrowIfCancellationRequested();
                }

                sum += i;
                Thread.Sleep(random.Next(100));
            }

            return sum;
        }
    }
}