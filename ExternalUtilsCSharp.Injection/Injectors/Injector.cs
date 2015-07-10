using ExternalUtilsCSharp.Injection.Injectors;
using ExternalUtilsCSharp.MemObjects.PE;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.Injection
{
    /// <summary>
    /// An abstract base-class providing methods to perform an DLL-injection
    /// </summary>
    public abstract class Injector
    {
        #region VARIABLES
        protected IntPtr hModule;
        #endregion
        
        #region PROPERTIES
        /// <summary>
        /// Pointer to the base-address of the injected module
        /// </summary>
        public IntPtr Module { get { return hModule; } }

        /// <summary>
        /// MemUtils-instance that is used to read and write memory from the target-process
        /// </summary>
        public MemUtils MemUtils { get; protected set; }

        /// <summary>
        /// ProcUtils-instance that is used to access the target-process
        /// </summary>
        protected ProcUtils ProcUtils { get; set; }

        /// <summary>
        /// Holds data about the PE-header of the injected module
        /// </summary>
        public PEInfo DllInfo { get; protected set; }

        /// <summary>
        /// Holds the name of exported functions and their address (relative to the image-base)
        /// </summary>
        public Tuple<string, int>[] ExportedFunctions { get; set; }
        #endregion

        #region CONSTRUCTOR
        public Injector()
        {
            ProcUtils = null;
            MemUtils = null;
            hModule = IntPtr.Zero;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Performs an DLL-injection
        /// </summary>
        /// <param name="processName">Name of the process to inject into</param>
        /// <param name="dllPath">Path of the DLL-file to inject</param>
        /// <returns>A DLLInjectionResult holding data about this injection-attempt</returns>
        public DllInjectionResult Inject(string processName, string dllPath)
        {
            if (string.IsNullOrEmpty(processName))
                throw new ArgumentException("String must not be null or empty", "processName");
            if (!ProcUtils.ProcessIsRunning(processName))
                throw new Exception(string.Format("Process \"{0}\" not found", processName));

            return Inject(Process.GetProcessesByName(processName)[0], dllPath);
        }
        /// <summary>
        /// Performs an DLL-injection
        /// </summary>
        /// <param name="pid">ID of the process to inject into</param>
        /// <param name="dllPath">Path of the DLL-file to inject</param>
        /// <returns>A DLLInjectionResult holding data about this injection-attempt</returns>
        public DllInjectionResult Inject(int pid, string dllPath, string moduleName = null)
        {
            if (pid < 0)
                throw new ArgumentException("Process-ID is invalid", "pid");
            if (!ProcUtils.ProcessIsRunning(pid))
                throw new Exception(string.Format("Process with id {0} not found", pid));

            return Inject(Process.GetProcessById(pid), dllPath);
        }
        /// <summary>
        /// Performs an DLL-injection
        /// </summary>
        /// <param name="proc">Process to inject into</param>
        /// <param name="dllPath">Path of the DLL-file to inject</param>
        /// <returns>A DLLInjectionResult holding data about this injection-attempt</returns>
        public DllInjectionResult Inject(Process proc, string dllPath, string moduleName = null)
        {
            if (string.IsNullOrEmpty(dllPath))
                throw new ArgumentException("String must not be null or empty", "dllPath");
            if (!File.Exists(dllPath))
                throw new FileNotFoundException("Dll not found", dllPath);

            DllInjectionResult result = this.PerformInjection(proc, dllPath);
            this.ParsePEInfo();

            return result;
        }

        /// <summary>
        /// Performs an DLL-injection; Must be implemented by subclasses
        /// </summary>
        /// <param name="proc">Process to inject into</param>
        /// <param name="dllPath">Path of the DLL-file to inject</param>
        /// <returns>A DLLInjectionResult holding data about this injection-attempt</returns>
        protected abstract DllInjectionResult PerformInjection(Process proc, string dllPath);

        /// <summary>
        /// Unloads the library from RAM
        /// -> Subclasses may need to override this method in case they cloak the module
        /// </summary>
        /// <returns></returns>
        public virtual bool UnloadLibrary()
        {
            IntPtr lpFreeLibrary = WinAPI.GetProcAddress(WinAPI.GetModuleHandle("Kernel32"), "FreeLibrary");
            RemoteThreadResult result = ExecuteRemoteThread(lpFreeLibrary, hModule);
            if (!result.Success)
                return false;
            else
                return result.ReturnValue == 1;
        }

        /// <summary>
        /// Executes a function by executing a thread in the process the module was injected into
        /// </summary>
        /// <param name="startAddress">Address of the function to execute</param>
        /// <param name="parameters">(Optional) Address of parameters to pass</param>
        /// <returns></returns>
        public RemoteThreadResult ExecuteRemoteThread(IntPtr startAddress, IntPtr parameters)
        {
            IntPtr hThread = WinAPI.CreateRemoteThread(ProcUtils.Handle, (IntPtr)null, IntPtr.Zero, startAddress, parameters, 0, (IntPtr)null);
            if (hThread == IntPtr.Zero)
            {
                return new RemoteThreadResult("Could not find address of LoadLibraryA", new Win32Exception(Marshal.GetLastWin32Error()));
            }
            WinAPI.WaitForSingleObject(hThread, (uint)WinAPI.WaitForSingleObjectMilliseconds.INFINITE);
            IntPtr returnVal = IntPtr.Zero;

            if(!WinAPI.GetExitCodeThread(hThread, out returnVal))
            {
                return new RemoteThreadResult("Failed to get exit thread code", new Win32Exception(Marshal.GetLastWin32Error()));
            }
            return new RemoteThreadResult(returnVal.ToInt64());
        }

        /// <summary>
        /// Allocates memory in the remote process
        /// </summary>
        /// <param name="size">Size of data (in bytes) to allocate</param>
        /// <returns></returns>
        public IntPtr AllocateMemory(int size)
        {
            return WinAPI.VirtualAllocEx(ProcUtils.Handle, (IntPtr)null, (IntPtr)size, (uint)WinAPI.AllocationType.Commit | (uint)WinAPI.AllocationType.Reserve, (uint)WinAPI.MemoryProtection.ExecuteReadWrite);
        }

        /// <summary>
        /// Frees memory in the remote process
        /// </summary>
        /// <param name="address">Address of previously allocated memory</param>
        /// <returns></returns>
        public bool FreeMemory(IntPtr address)
        {
            return WinAPI.VirtualFreeEx(ProcUtils.Handle, address, 0, WinAPI.FreeType.Release);
        }

        /// <summary>
        /// Writes memory to the remote process
        /// </summary>
        /// <param name="address">Address where to write data to</param>
        /// <param name="data">Array of bytes to write to the process</param>
        public void WriteMemory(IntPtr address, byte[] data)
        {
            MemUtils.Write(address, data);
        }

        /// <summary>
        /// Executes a function that was exported by the injected module
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="functionName">Name of the function that shall be executed</param>
        /// <param name="_data">Data to pass as an argument to the function</param>
        /// <param name="eraseMemory">Whether to erase the passed argument after executing the function</param>
        /// <returns></returns>
        public virtual int ExecuteRemoteFunction<T>(string functionName, T _data, bool eraseMemory = true) where T : struct
        {
            byte[] data = MemUtils.TToBytes(_data);
            return ExecuteRemoteFunction(functionName, data, eraseMemory);
        }
        /// <summary>
        /// Executes a function that was exported by the injected module
        /// </summary>
        /// <param name="functionName">Name of the function that shall be executed</param>
        /// <param name="text">Text to pass as an argument</param>
        /// <param name="enc"></param>
        /// <param name="eraseMemory">Whether to erase the passed argument after executing the function</param>
        /// <returns></returns>
        public virtual int ExecuteRemoteFunction(string functionName, string text, Encoding enc, bool eraseMemory = true)
        {
            byte[] data = enc.GetBytes(text);
            return ExecuteRemoteFunction(functionName, data, eraseMemory);
        }
        /// <summary>
        /// Executes a function that was exported by the injected module
        /// </summary>
        /// <param name="functionName">Name of the function that shall be executed</param>
        /// <param name="data">An array of data that is passed as an argument to the function</param>
        /// <param name="eraseMemory">Whether to erase the passed argument after executing the function</param>
        /// <returns></returns>
        public virtual int ExecuteRemoteFunction(string functionName, byte[] data, bool eraseMemory = true)
        {
            IntPtr address = this.AllocateMemory(data.Length);
            if (address == IntPtr.Zero)
                throw new Win32Exception(Marshal.GetLastWin32Error());
            this.WriteMemory(address, data);
            int result = ExecuteRemoteFunction(functionName, address);
            if(eraseMemory)
                this.WriteMemory(address, new byte[data.Length]);
            this.FreeMemory(address);
            return result;
        }
        /// <summary>
        /// Executes a function that was exported by the injected module
        /// </summary>
        /// <param name="functionName">Name of the function that shall be executed</param>
        /// <returns></returns>
        public virtual int ExecuteRemoteFunction(string functionName)
        {
            return ExecuteRemoteFunction(functionName, IntPtr.Zero);
        }
        /// <summary>
        /// Executes a function that was exported by the injected module
        /// </summary>
        /// <param name="functionName">Name of the function that shall be executed</param>
        /// <param name="parameters">Address of parameters that are passed to the functions</param>
        /// <returns></returns>
        public virtual int ExecuteRemoteFunction(string functionName, IntPtr parameters)
        {
            if(this.ExportedFunctions == null)
                this.ReadExportedFunctions();
            if(ExportedFunctions.Count(x=>x.Item1 == functionName) == 0)
                throw new Exception (string.Format("Unknown function \"{0}\"", functionName));
            RemoteThreadResult result = ExecuteRemoteThread((IntPtr)(hModule.ToInt64() + ExportedFunctions.First(x => x.Item1 == functionName).Item2), parameters);
            return (int)result.ReturnValue;
        }

        /// <summary>
        /// Reads the functions exported by the injected module
        /// -> Subclasses may need to override this method in case they cloak the module
        /// </summary>
        public void ReadExportedFunctions()
        {
            this.ExportedFunctions = this.DllInfo.ReadExportedFunctions(MemUtils, hModule);
        }

        /// <summary>
        /// This method must parse the PE-header of the injected module
        /// </summary>
        public abstract void ParsePEInfo();
        #endregion
    }
}
