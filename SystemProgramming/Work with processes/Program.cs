// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="Compilyator">
//   All rights reserved
// </copyright>
// <summary>
//   Defines the Program type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace WorkWithProcesses
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The program.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <exception cref="IOException">An I/O error occurred. </exception>
        /// <exception cref="OutOfMemoryException">There is insufficient memory to allocate a buffer for the returned string. </exception>
        internal static void Main(string[] args)
        {
            NewProcessExample();

            ProcessListExample();

            Console.ReadLine();
        }

        /// <summary>
        /// The process list example.
        /// </summary>
        private static void ProcessListExample()
        {
            var processes = Process.GetProcesses().ToList();
            Console.WriteLine($"{"Id",-5} {"Name",-60} {"Priority",-10}");
            processes.ForEach(
                process =>
                    {
                        try
                        {
                            Console.WriteLine($"{process.Id,-5} {process.ProcessName,-60} {process.PriorityClass,-10}");
                        }
                        catch
                        {
                            // ignored
                        }
                    });
        }

        /// <summary>
        /// The new process example.
        /// </summary>
        private static void NewProcessExample()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "notepad.exe",
                WorkingDirectory = @"C:\Users\Compi\Desktop",
                Arguments = @"C:\Windows\WindowsUpdate.log",
                WindowStyle = ProcessWindowStyle.Maximized
            };
            var process = Process.Start(psi);
            if (process == null)
            {
                Console.WriteLine("Failed to create process");
                return;
            }

            process.Exited += (sender, eventArgs) => Console.WriteLine("Notepad closed!");
            Console.WriteLine($"{process.Id} {process.UserProcessorTime}");
            process.WaitForExit(3000);
            if (!process.HasExited)
            {
                Console.WriteLine("Terminating");
                process.Close();
            }
            else
            {
                Console.WriteLine($"Exit code: {process.ExitCode}");
            }
        }
    }
}
