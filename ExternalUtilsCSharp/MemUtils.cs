using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace ExternalUtilsCSharp
{
    /// <summary>
    /// A class that simplifies read- and write-operations to processes
    /// Includes signature-scanning
    /// </summary>
    public class MemUtils
    {
        #region CONSTANTS
        private const int SIZE_FLOAT = sizeof(float);
        private const int MAX_DUMP_SIZE = 0xFFFF;
        #endregion
        #region PROPERTIES
        /// <summary>
        /// The handle to the process this class reads memory from/writes memory to
        /// </summary>
        public static IntPtr Handle { get; set; }
        /// <summary>
        /// Determines whether data will be read/written using unsafe code or not.
        /// Implementation of unsafe code comes from:
        /// https://github.com/Aevitas/bluerain/blob/master/src/BlueRain/ExternalProcessMemory.cs
        /// </summary>
        public static bool UseUnsafeReadWrite { get; set; }
        #endregion
        #region METHODS
        #region PRIMITIVE WRAPPERS
        /// <summary>
        /// Reads a chunk of memory
        /// </summary>
        /// <param name="address">The address of the chunk of memory</param>
        /// <param name="data">The byte-array to write the read data to</param>
        /// <param name="length">The number (in bytes) of bytes to read</param>
        /// <returns>True if successful, false if not</returns>
        public static bool Read(IntPtr address, out byte[] data, int length)
        {
            IntPtr numBytes = IntPtr.Zero;
            data = new byte[length];
            bool result = WinAPI.ReadProcessMemory(Handle, address, data, length, out numBytes);
            if (!result)
                return false;
            return numBytes.ToInt32() == length;
        }

        /// <summary>
        /// Writes a chunk of memory
        /// </summary>
        /// <param name="address">The address to write to</param>
        /// <param name="data">A byte-array of data to write</param>
        /// <returns>True if successful, false if not</returns>
        public static bool Write(IntPtr address, byte[] data)
        {
            IntPtr numBytes = IntPtr.Zero;
            bool result = WinAPI.WriteProcessMemory(Handle, address, data, data.Length, out numBytes);
            if (!result)
                return false;
            return numBytes.ToInt32() == data.Length;
        }
        /// <summary>
        /// Writes a chunk of memory using the given offset and length of data
        /// It will apply the offset to the address as well as to the data, length defines the number of bytes to write (beginning at offset)
        /// </summary>
        /// <param name="address">The address to write to</param>
        /// <param name="data">A byte-array of data to write</param>
        /// <param name="offset">Skips the given number of bytes (applies to address and data)</param>
        /// <param name="length">Number of bytes to write (beginning at offset)</param>
        /// <returns>True if successful, false if not</returns>
        public static bool Write(IntPtr address, byte[] data, int offset, int length)
        {
            byte[] writeData = new byte[length];
            Array.Copy(data, offset, writeData, 0, writeData.Length);
            return Write((IntPtr)(address.ToInt32() + offset), writeData);
        }
        #endregion
        #region SPECIALIZED FUNCTIONS
        #region READ
        /// <summary>
        /// Reads a string from memory using the given encoding
        /// </summary>
        /// <param name="address">The address of the string to read</param>
        /// <param name="length">The length of the string</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>The string read from memory</returns>
        public static String ReadString(IntPtr address, int length, Encoding encoding)
        {
            byte[] data;
            if (Read(address, out data, length))
                return encoding.GetString(data);
            return null;
        }
        /// <summary>
        /// Generic function to read data from memory using the given type
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="address">The address to read data at</param>
        /// <param name="defVal">The default value of this operation (which is returned in case the Read-operation fails)</param>
        /// <returns>The value read from memory</returns>
        public unsafe static T Read<T>(IntPtr address, T defVal = default(T)) where T : struct
        {
            byte[] data;
            int size = Marshal.SizeOf(typeof(T));
            T structure = defVal;

            if (Read(address, out data, size))
            {
                if (UseUnsafeReadWrite)
                {
                    fixed (byte* b = data)
                        structure = (T)Marshal.PtrToStructure((IntPtr)b, typeof(T));
                }
                else
                {
                    GCHandle gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
                    structure = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
                    gcHandle.Free();
                }
            }

            return structure;
        }
        /// <summary>
        /// Reads a matrix from memory
        /// </summary>
        /// <param name="address">The address of the matrix in memory</param>
        /// <param name="rows">The number of rows of this matrix</param>
        /// <param name="columns">The number of columns of this matrix</param>
        /// <returns>The matrix read from memory</returns>
        public static Matrix ReadMatrix(IntPtr address, int rows, int columns)
        {
            Matrix matrix = new Matrix(rows, columns);
            byte[] data;
            if (Read(address, out data, SIZE_FLOAT * rows * columns))
                matrix.Read(data);
            return matrix;
        }
        /// <summary>
        /// Generic function to read an array from memory using the given type and offsets.
        /// Offsets will be added to the address. (They will not be summed up but rather applied individually)
        /// </summary>
        /// <typeparam name="T">The type of the value</typeparam>
        /// <param name="address">The address to read data at</param>
        /// <param name="offsets">Offsets that will be applied to the address</param>
        /// <returns></returns>
        public static T[] Read<T>(IntPtr address, params int[] offsets) where T : struct
        {
            T[] values = new T[offsets.Length];
            for (int i = 0; i < offsets.Length; i++)
                values[i] = Read<T>((IntPtr)(address.ToInt32() + offsets[i]));
            return values;
        }
        #endregion
        #region WRITE
        /// <summary>
        /// Writes a string to memory using the given encoding
        /// </summary>
        /// <param name="address">The address to write the string to</param>
        /// <param name="text">The text to write</param>
        /// <param name="encoding">The encoding of the string</param>
        /// <returns>True if successful, false if not</returns>
        public bool WriteString(IntPtr address, string text, Encoding encoding)
        {
            return Write(address, encoding.GetBytes(text));
        }
        /// <summary>
        /// Generic function to write data to memory using the given type
        /// </summary>
        /// <typeparam name="T">The type that of the value</typeparam>
        /// <param name="address">The address to write data to</param>
        /// <param name="value">The value to write to memory</param>
        /// <returns>True if successful, false if not</returns>
        public static unsafe bool Write<T>(IntPtr address, T value) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            byte[] data = new byte[size];

            if (UseUnsafeReadWrite)
            {
                fixed (byte* b = data)
                    Marshal.StructureToPtr(value, (IntPtr)b, true);
            }
            else
            {
                IntPtr ptr = Marshal.AllocHGlobal(size);
                Marshal.StructureToPtr(value, ptr, true);
                Marshal.Copy(ptr, data, 0, size);
                Marshal.FreeHGlobal(ptr);
            }

            return Write(address, data);
        }
        /// <summary>
        /// Writes a matrix to memory
        /// </summary>
        /// <param name="address">The address to write the matrix to</param>
        /// <param name="matrix">The matrix to write to memory</param>
        /// <returns>True if successful, false if not</returns>
        public static bool WriteMatrix(IntPtr address, Matrix matrix)
        {
            return Write(address, matrix.ToByteArray());
        }
        #endregion
        #endregion
        #region SIGSCANNING
        /// <summary>
        /// Performs a signature-scan using for the given pattern and mask in the given range of the process' address space
        /// </summary>
        /// <param name="pattern">Byte-pattern to scan for</param>
        /// <param name="mask">Mask to scan for ('?' is the wildcard)</param>
        /// <param name="module">Module to scan</param>
        /// <param name="wildcard">Char that is used as wildcard in the mask</param>
        /// <returns></returns>
        public static ScanResult PerformSignatureScan(byte[] pattern, string mask, ProcessModule module, char wildcard = '?')
        {
            return PerformSignatureScan(pattern, mask, module.BaseAddress, module.ModuleMemorySize, wildcard);
        }
        /// <summary>
        /// Performs a signature-scan using for the given pattern and mask in the given range of the process' address space
        /// </summary>
        /// <param name="pattern">Byte-pattern to scan for</param>
        /// <param name="mask">Mask to scan for ('?' is the wildcard)</param>
        /// <param name="from">Where to start scanning from</param>
        /// <param name="length">The length of the range to scan in</param>
        /// <param name="wildcard">Char that is used as wildcard in the mask</param>
        /// <returns></returns>
        public static ScanResult PerformSignatureScan(byte[] pattern, string mask, IntPtr from, int length, char wildcard = '?')
        {
            return PerformSignatureScan(pattern, mask, from, (IntPtr)(from.ToInt64() + length), wildcard);
        }
        /// <summary>
        /// Performs a signature-scan using for the given pattern and mask in the given range of the process' address space
        /// Returns the address of the beginning of the pattern if found, returns IntPtr.Zero if not found
        /// </summary>
        /// <param name="pattern">Byte-pattern to scan for</param>
        /// <param name="mask">Mask to scan for</param>
        /// <param name="from">Where to start scanning from</param>
        /// <param name="to">Where to stop scanning at</param>
        /// <param name="wildcard">Char that is used as wildcard in the mask</param>
        /// <returns></returns>
        public static ScanResult PerformSignatureScan(byte[] pattern, string mask, IntPtr from, IntPtr to, char wildcard = '?')
        {
            if (from.ToInt64() >= to.ToInt64())
                throw new ArgumentException();
            if (pattern == null)
                throw new ArgumentNullException();
            if (mask.Length != pattern.Length)
                throw new ArgumentException();

            long totalLength = to.ToInt64() - from.ToInt64();
            int dumps = (int)Math.Ceiling((double)(totalLength) / (double)MAX_DUMP_SIZE);
            int length = 0;
            byte[] data;

            for (int dmp = 0; dmp < dumps; dmp++)
            {
                if (totalLength - (dmp * MAX_DUMP_SIZE) < MAX_DUMP_SIZE)
                    length = (int)(totalLength - (dmp * MAX_DUMP_SIZE));
                else
                    length = MAX_DUMP_SIZE;

                if (Read((IntPtr)(from.ToInt64() + dmp * MAX_DUMP_SIZE), out data, length))
                {
                    int idx = ScanDump(data, pattern, mask, wildcard);
                    if (idx != -1)
                    {
                        return new ScanResult()
                            {
                                Success = true,
                                Base = from,
                                Offset = (IntPtr)(dmp * MAX_DUMP_SIZE + idx),
                                Address = (IntPtr)(from + dmp * MAX_DUMP_SIZE + idx)
                            };
                    }
                }
            }

            return new ScanResult(){ Address = IntPtr.Zero, Base = IntPtr.Zero, Offset = IntPtr.Zero, Success = false };
        }
        /// <summary>
        /// Scans a dumped chunk of memory and returns the index of the pattern if found
        /// </summary>
        /// <param name="data">Chunk of memory</param>
        /// <param name="pattern">Byte-pattern to scan for</param>
        /// <param name="mask">Mask to scan for</param>
        /// <param name="wildcard">Char that is used as wildcard in the mask</param>
        /// <returns>Index of pattern if found, -1 if not found</returns>
        private static int ScanDump(byte[] data, byte[] pattern, string mask, char wildcard)
        {
            bool found = false;
            for (int idx = 0; idx < data.Length - pattern.Length; idx++)
            {
                found = true;
                for (int chr = 0; chr<mask.Length;chr++)
                {
                    if (mask[chr] != '?')
                    {
                        if(data[idx+chr] != pattern[chr])
                        {
                            found = false;
                            break;
                        }
                    }
                }
                if (found)
                    return idx;
            }
            return -1;
        }
        /// <summary>
        /// Creates a mask from a given pattern, using the given chars
        /// </summary>
        /// <param name="pattern">The pattern this functions designs a mask for</param>
        /// <param name="wildcardByte">Byte that is interpreted as a wildcard</param>
        /// <param name="wildcardChar">Char that is used as wildcard</param>
        /// <param name="matchChar">Char that is no wildcard</param>
        /// <returns></returns>
        public static string MaskFromPattern(byte[] pattern, byte wildcardByte, char wildcardChar = '?', char matchChar = 'x')
        {
            char[] chr = new char[pattern.Length];
            for (int i = 0; i < chr.Length; i++)
                chr[i] = pattern[i] == wildcardByte ? wildcardChar : matchChar;
            return new string(chr);
        }
        #region STRUCTS
        /// <summary>
        /// Struct that holds basic data about the outcome of a signature-scan
        /// </summary>
        public struct ScanResult
        {
            public bool Success;
            public IntPtr Address;
            public IntPtr Base;
            public IntPtr Offset;
        }
        #endregion
        #endregion
        #endregion
    }
}