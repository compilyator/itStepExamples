namespace ThreadsExample
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var thread = new Thread(Start);
            thread.Start(100);
            var result = thread.Join(TimeSpan.FromSeconds(5));
            if (!result)
            {
                thread.Abort();
                Console.WriteLine("Aborted");
            }

            Console.ReadLine();
        }

        private static void Start(object o)
        {
            Console.WriteLine($"Thread id = {Thread.CurrentThread.ManagedThreadId}");
            var max = (int)o;
            var sum = 0;
            var random = new Random();
            for (int i = 1; i <= max; i++)
            {
                sum += i;
                Thread.Sleep(random.Next(100));
            }
            Console.WriteLine($"Sum = {sum}");
        }
    }
}