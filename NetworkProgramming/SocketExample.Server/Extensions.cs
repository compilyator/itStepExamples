using System;

namespace SocketExample.Server
{
    public static class Extensions
    {
        public static void WriteError(this Exception exception)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:SS}] ERROR: {exception.Message}");
            Console.ForegroundColor = oldColor;
        }

        public static void WriteMessage(this string message)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now:dd.MM.yyyy HH:mm:SS}] INFO: {message}");
            Console.ForegroundColor = oldColor;
        }
    }
}