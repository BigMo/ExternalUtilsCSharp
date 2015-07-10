using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.Injection.Injectors
{
    /// <summary>
    /// Performs a DLL-injection by calling LoadLibraryA and CreateRemoteThread
    /// Credits fly out to evolution536 since I copied large parts of his C# dll injector:
    /// https://www.unknowncheats.me/forum/c/82629-basic-c-dll-injector.html
    /// </summary>
    public class LoadLibraryCRTInjector : Injector
    {
        public string DllPath { get; private set; }

        protected override DllInjectionResult PerformInjection(System.Diagnostics.Process proc, string dllPath)
        {
            this.DllPath = dllPath;

            ProcUtils = new ProcUtils(proc, WinAPI.ProcessAccessFlags.CreateThread | WinAPI.ProcessAccessFlags.VirtualMemoryOperation | WinAPI.ProcessAccessFlags.VirtualMemoryRead | WinAPI.ProcessAccessFlags.VirtualMemoryWrite | WinAPI.ProcessAccessFlags.QueryInformation);

            MemUtils = new MemUtils();
            MemUtils.Handle = ProcUtils.Handle;
            MemUtils.UseUnsafeReadWrite = true;


            if (ProcUtils.Handle == IntPtr.Zero)
            {
                return new DllInjectionResult("Could not open process", new Win32Exception(Marshal.GetLastWin32Error()));
            }
 
            IntPtr lpLLAddress = WinAPI.GetProcAddress(WinAPI.GetModuleHandle("kernel32.dll"), "LoadLibraryA");
 
            if (lpLLAddress == IntPtr.Zero)
            {
                return new DllInjectionResult("Could not find address of LoadLibraryA", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            IntPtr lpAddress = WinAPI.VirtualAllocEx(ProcUtils.Handle, (IntPtr)null, (IntPtr)dllPath.Length, (uint)WinAPI.AllocationType.Commit | (uint)WinAPI.AllocationType.Reserve, (uint)WinAPI.MemoryProtection.ExecuteReadWrite);

            if (lpAddress == IntPtr.Zero)
            {
                return new DllInjectionResult("Could not allocate memory for dllPath", new Win32Exception(Marshal.GetLastWin32Error()));
            }

            byte[] bytes = Encoding.ASCII.GetBytes(dllPath);
            
            try
            {
                MemUtils.WriteString(lpAddress, dllPath, Encoding.ASCII);
            } 
            catch(Exception ex)
            {
                return new DllInjectionResult("Failed to write dllPath to memory", ex);
            }

            RemoteThreadResult result = this.ExecuteRemoteThread(lpLLAddress, lpAddress);

            if (!result.Success)
            {
                return new DllInjectionResult(result.ErrorMessage);
            }
            hModule = (IntPtr)result.ReturnValue;

            if (hModule == IntPtr.Zero)
                return new DllInjectionResult("The base-address of the injected module is zero");

            return new DllInjectionResult(true);
        }

        public override void ParsePEInfo()
        {
            this.DllInfo = new MemObjects.PE.PEInfo(this.DllPath);
        }
    }
}
