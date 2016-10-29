using System;
using System.Linq;
using Microsoft.Win32;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = Registry.LocalMachine;
            root.SetValue("TestName", "Test Value", RegistryValueKind.String);
            var valueName = "TestName";
            if (!root.GetValueNames().Contains(valueName))
            {
                Console.WriteLine("Wrong value name");
            }
            else
            {
                var kind = root.GetValueKind(valueName);
                if (kind == RegistryValueKind.String)
                {
                    var value = (string) root.GetValue(valueName, "Dick");
                    Console.WriteLine(value);
                }
                else
                {
                    Console.WriteLine("Wrong value kind");
                }
            }
            Console.ReadLine();
        }
    }
}
