using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ExternalUtilsCSharp
{
    /// <summary>
    /// A class that simplifies read- and write-operations to processes
    /// </summary>
    public class MemUtils
    {
        #region CONSTANTS
        private const int SIZE_FLOAT = sizeof(float);
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
        public static bool Read( IntPtr address, out byte[] data, int length)
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
        #endregion
    }
}