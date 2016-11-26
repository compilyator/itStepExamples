using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebRequestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            var stopWatch = Stopwatch.StartNew();
            DoWork();
            Console.WriteLine($"Elapsed: {stopWatch.ElapsedMilliseconds} ms.");
            Console.ReadKey();
        }

        private static void DoWork()
        {
            var request = WebRequest.CreateHttp(new Uri("http://localhost:1590/files/names"));
            var response = request.GetResponse() as HttpWebResponse;
            var uries = GetUries(response);
            Task.WaitAll(uries.Select(async uri => { await DownloadFile(uri); }).ToArray());
        }

        private static IEnumerable<Uri> GetUries(WebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                while (!reader.EndOfStream)
                {
                    var url = reader.ReadLine();
                    yield return new Uri(url);
                }
            }
        }

        private static async Task DownloadFile(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            var response = await request.GetResponseAsync();
            var header = response.Headers["Content-Disposition"];
            var fileName = header.Substring("attachment; filename=".Length);
            using (var fs = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (var writer = new BinaryWriter(fs))
                {
                    using (var reader = new BinaryReader(response.GetResponseStream()))
                    {
                        var readed = 0;
                        var bytes = new byte[10240];
                        while (readed != response.ContentLength)
                        {
                            var count = reader.Read(bytes, 0, bytes.Length);
                            readed += count;
                            writer.Write(bytes, 0, count);
                        }
                    }
                }
            }
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action?.Invoke(item);
            }
        }
    }
}