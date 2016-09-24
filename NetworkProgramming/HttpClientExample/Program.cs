using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpClientExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = Stopwatch.StartNew();
            DoWork().GetAwaiter().GetResult();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds} ms.");
            Console.ReadKey();
        }

        private static async Task DoWork()
        {
            var client = new HttpClient();
            var uries = GetUries(await client.GetStreamAsync("http://localhost:1590/files/names"));
            Task.WaitAll(uries.Select(async uri =>
            {
                var fileName = uri.AbsoluteUri.Substring("http://localhost:1590/files/image/".Length) + ".jpg";
                var message = await client.GetByteArrayAsync(uri);
                File.WriteAllBytes(fileName, message);
            }).ToArray());
        }

        private static IEnumerable<Uri> GetUries(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                while (!reader.EndOfStream)
                {
                    var url = reader.ReadLine();
                    yield return new Uri(url);
                }
            }
        }

    }
}
