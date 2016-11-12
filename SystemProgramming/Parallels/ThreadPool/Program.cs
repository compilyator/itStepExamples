namespace ThreadPool
{
    using System;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            int minWorkers, minPorts, maxWorkers, maxPorts;
            ThreadPool.GetMinThreads(out minWorkers, out minPorts);
            ThreadPool.GetMaxThreads(out maxWorkers, out maxPorts);
            Console.WriteLine($"Workers: [{minWorkers}-{maxWorkers}]");
            Console.WriteLine($"Ports: [{minPorts}-{maxPorts}]");
            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(Start, (i + 1) * 10);
            }

            Console.ReadLine();
        }

        private static void Start(object o)
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: Started");
            var max = (int)o;
            var sum = 0;
            var random = new Random();
            for (int i = 1; i <= max; i++)
            {
                sum += i;
                Thread.Sleep(random.Next(100));
            }
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}]: Result = {sum} ");
        }
    }
}