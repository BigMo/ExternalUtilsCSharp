using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExternalUtilsCSharp
{
    public class ProcUtils
    {
        #region PROPERTIES
        public Process Process { get; private set; }
        public IntPtr Handle { get; private set; }
        #endregion
        #region STATIC METHODS
        public static bool ProcessIsRunning(string name)
        {
            return Process.GetProcessesByName(name).Length > 0;
        }
        public static bool ProcessIsRunning(int id)
        {
            return Process.GetProcessById(id) != null;
        }
        public static IntPtr OpenHandleByProcessID(int id, WinAPI.ProcessAccessFlags flags)
        {
            return WinAPI.OpenProcess(flags, false, id);
        }
        public static IntPtr OpenHandleByProcessName(string name, WinAPI.ProcessAccessFlags flags)
        {
            return OpenHandleByProcessID(Process.GetProcessesByName(name)[0].Id, flags);
        }
        public static IntPtr OpenHandleByProcess(Process process, WinAPI.ProcessAccessFlags flags)
        {
            return OpenHandleByProcessID(process.Id, flags);
        }
        public static void CloseHandleToProcess(IntPtr handle)
        {
            WinAPI.CloseHandle(handle);
        }
        #endregion
        #region CONSTRUCTOR/DESTRUCTOR
        public ProcUtils(string processName, WinAPI.ProcessAccessFlags handleFlags)
            : this(Process.GetProcessesByName(processName)[0],handleFlags)
        { }
        public ProcUtils(int id, WinAPI.ProcessAccessFlags handleFlags)
            : this(Process.GetProcessById(id), handleFlags)
        { }
        public ProcUtils(Process process, WinAPI.ProcessAccessFlags handleFlags)
        {
            this.Process = process;
            this.Handle = ProcUtils.OpenHandleByProcess(process, handleFlags);
        }
        ~ProcUtils()
        {
            CloseHandleToProcess(Handle);
        }
        #endregion
    }
}
