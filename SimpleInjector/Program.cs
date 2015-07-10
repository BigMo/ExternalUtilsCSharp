using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExternalUtilsCSharp.Injection;
using ExternalUtilsCSharp.Injection.Injectors;
using System.Runtime.InteropServices;
using System.Threading;

namespace SimpleInjector
{
    /// <summary>
    /// A simple injector that demonstrates how to inject dlls and call exported functions
    /// </summary>
    class Program
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct HelloWorldData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Name;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string Place;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct AddData
        {
            public int p1;
            public int p2;
        }

        static void Main(string[] args)
        {
            LoadLibraryCRTInjector injector = new LoadLibraryCRTInjector();
            DllInjectionResult result;
            try
            {
                result = injector.Inject("notepad++", new System.IO.FileInfo(@"..\..\..\Debug\SimpleDLL.dll").FullName);
            }
            catch (Exception ex)
            {
                result = new DllInjectionResult("An exception occured during injection", ex);
            }
            if (!result.Success)
            {
                Console.WriteLine("Injection failed!");
                Console.WriteLine(result.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Injection succeded!");
                injector.ReadExportedFunctions();
                foreach (Tuple<string, int> fn in injector.ExportedFunctions)
                    Console.WriteLine("Function \"{0}\" at 0x{1}", fn.Item1, fn.Item2.ToString("X"));
                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(1000);
                    int res = 0;

                    res = injector.ExecuteRemoteFunction("ChangeTitle", string.Format("Counting: {0}", i.ToString()) + '\0', Encoding.Unicode);
                    Console.WriteLine("[ChangeTitle] Res: {0}", res);

                    Thread.Sleep(1000);

                    res = injector.ExecuteRemoteFunction("RemoveTitle");
                    Console.WriteLine("[RemoveTitle] Res: {0}", res);

                    res = injector.ExecuteRemoteFunction("Add", new AddData() { p1 = i, p2 = i + 1 });
                    Console.WriteLine("[Add] Res: {0}", res);

                    res = injector.ExecuteRemoteFunction("HelloWorld", new HelloWorldData() { Name = string.Format("UC-member #{0}", i) + '\0', Place = "UC" + '\0' });
                    Console.WriteLine("[HelloWorld] Res: {0}", res);
                }
            }
            Console.WriteLine("Done");
            Console.ReadLine();
            Console.WriteLine("Unloading...");
            if (!injector.UnloadLibrary())
                Console.WriteLine("Failed to unload!");
            else
                Console.WriteLine("Unloading succeded!");
            Console.ReadLine();
        }
    }
}