using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ExternalUtilsCSharp
{
    public class ProcUtils
    {
        #region PROPERTIES
        /// <summary>
        /// The process this ProcUtils wraps
        /// </summary>
        public Process Process { get; private set; }
        /// <summary>
        /// The (opened) handle to the process of this ProcUtils
        /// </summary>
        public IntPtr Handle { get; private set; }
        #endregion
        #region STATIC METHODS
        /// <summary>
        /// Returns whether a specific process is running
        /// </summary>
        /// <param name="name">The name of the process</param>
        /// <returns></returns>
        public static bool ProcessIsRunning(string name)
        {
            return Process.GetProcessesByName(name).Length > 0;
        }
        /// <summary>
        /// Returns whether a specific process is running
        /// </summary>
        /// <param name="id">The ID of the process</param>
        /// <returns></returns>
        public static bool ProcessIsRunning(int id)
        {
            return Process.GetProcessById(id) != null;
        }
        /// <summary>
        /// Opens a handle to a process
        /// </summary>
        /// <param name="id">ID of the process</param>
        /// <param name="flags">ProcessAccessFlags to use</param>
        /// <returns>A handle to the process</returns>
        public static IntPtr OpenHandleByProcessID(int id, WinAPI.ProcessAccessFlags flags)
        {
            return WinAPI.OpenProcess(flags, false, id);
        }
        /// <summary>
        /// Opens a handle to a process
        /// </summary>
        /// <param name="name">Name of the process</param>
        /// <param name="flags">ProcessAccessFlags to use</param>
        /// <returns>A handle to the process</returns>
        public static IntPtr OpenHandleByProcessName(string name, WinAPI.ProcessAccessFlags flags)
        {
            return OpenHandleByProcessID(Process.GetProcessesByName(name)[0].Id, flags);
        }
        /// <summary>
        /// Opens a handle to a process
        /// </summary>
        /// <param name="process">The process-object of the process</param>
        /// <param name="flags">ProcessAccessFlags to use</param>
        /// <returns>A handle to the process</returns>
        public static IntPtr OpenHandleByProcess(Process process, WinAPI.ProcessAccessFlags flags)
        {
            return OpenHandleByProcessID(process.Id, flags);
        }
        /// <summary>
        /// Closes the given handle to the process
        /// </summary>
        /// <param name="handle">Handle to the process</param>
        public static void CloseHandleToProcess(IntPtr handle)
        {
            WinAPI.CloseHandle(handle);
        }
        #endregion
        #region CONSTRUCTOR/DESTRUCTOR
        /// <summary>
        /// Initializes a new ProcUtils
        /// </summary>
        /// <param name="processName">Name of the process</param>
        /// <param name="handleFlags">ProcessAccessFlags to use</param>
        public ProcUtils(string processName, WinAPI.ProcessAccessFlags handleFlags)
            : this(Process.GetProcessesByName(processName)[0],handleFlags)
        { }
        /// <summary>
        /// Initializes a new ProcUtils
        /// </summary>
        /// <param name="id">ID of the process</param>
        /// <param name="handleFlags">ProcessAccessFlags to use</param>
        public ProcUtils(int id, WinAPI.ProcessAccessFlags handleFlags)
            : this(Process.GetProcessById(id), handleFlags)
        { }
        /// <summary>
        /// Initializes a new ProcUtils
        /// </summary>
        /// <param name="process">Process-object of the process</param>
        /// <param name="handleFlags">ProcessAccessFlags to use</param>
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
