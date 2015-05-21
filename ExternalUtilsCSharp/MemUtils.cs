using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace ExternalUtilsCSharp
{
    public class MemUtils
    {
        #region CONSTANTS
        private const int SIZE_BYTE = sizeof(byte);
        private const int SIZE_INT16 = sizeof(short);
        private const int SIZE_INT32 = sizeof(int);
        private const int SIZE_INT64 = sizeof(long);
        private const int SIZE_UINT16 = sizeof(ushort);
        private const int SIZE_UINT32 = sizeof(uint);
        private const int SIZE_UINT64 = sizeof(ulong);
        private const int SIZE_FLOAT = sizeof(float);
        private const int SIZE_DOUBLE = sizeof(double);
        #endregion
        #region PROPERTIES
        public static IntPtr Handle { get; set; }
        #endregion
        #region METHODS
        #region PRIMITIVE WRAPPERS
        public static bool Read( IntPtr address, out byte[] data, int length)
        {
            IntPtr numBytes = IntPtr.Zero;
            data = new byte[length];
            bool result = WinAPI.ReadProcessMemory(Handle, address, data, length, out numBytes);
            if (!result)
                return false;
            return numBytes.ToInt32() == length;
        }

        public static bool Write(IntPtr address, byte[] data)
        {
            IntPtr numBytes = IntPtr.Zero;
            bool result = WinAPI.WriteProcessMemory(Handle, address, data, data.Length, out numBytes);
            if (!result)
                return false;
            return numBytes.ToInt32() == data.Length;
        }
        #endregion
        #region SPECIALIZED FUNCTIONS
        #region READ
        public static byte ReadByte(IntPtr address, byte defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_BYTE))
                return data[0];
            return defaultValue;
        }
        public static char ReadChar(IntPtr address, char defaultValue = '\x0')
        {
            return (char)ReadByte(address, (byte)defaultValue);
        }
        public static short ReadInt16(IntPtr address, short defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_INT16))
                return BitConverter.ToInt16(data, 0);
            return defaultValue;
        }
        public static ushort ReadUInt16(IntPtr address, ushort defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_UINT16))
                return BitConverter.ToUInt16(data, 0);
            return defaultValue;
        }
        public static int ReadInt32(IntPtr address, int defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_INT32))
                return BitConverter.ToInt32(data, 0);
            return defaultValue;
        }
        public static uint ReadUInt32(IntPtr address, uint defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_UINT32))
                return BitConverter.ToUInt32(data, 0);
            return defaultValue;
        }
        public static long ReadInt64(IntPtr address, long defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_INT64))
                return BitConverter.ToInt64(data, 0);
            return defaultValue;
        }
        public static ulong ReadUInt64(IntPtr address, ulong defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_UINT64))
                return BitConverter.ToUInt64(data, 0);
            return defaultValue;
        }
        public static float ReadFloat(IntPtr address, float defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_FLOAT))
                return BitConverter.ToSingle(data, 0);
            return defaultValue;
        }
        public static double ReadDouble(IntPtr address, double defaultValue = 0)
        {
            byte[] data;
            if (Read(address, out data, SIZE_DOUBLE))
                return BitConverter.ToDouble(data, 0);
            return defaultValue;
        }
        public static String ReadString(IntPtr address, int length, Encoding encoding)
        {
            byte[] data;
            if (Read(address, out data, length))
                return encoding.GetString(data);
            return null;
        }
        public static T ReadStruct<T>(IntPtr address, int structSize = 0) where T : struct
        {
            byte[] data;
            if (structSize == 0)
                structSize = Marshal.SizeOf(typeof(T));
            Read(address, out data, structSize);
            GCHandle gcHandle = GCHandle.Alloc(data, GCHandleType.Pinned);
            T structure = (T)Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof(T));
            gcHandle.Free();
            return structure;
        }
        public static Vector2 ReadVector2(IntPtr address)
        {
            return ReadStruct<Vector2>(address);
        }
        public static Vector3 ReadVector3(IntPtr address)
        {
            return ReadStruct<Vector3>(address);
        }
        public static Matrix ReadMatrix(IntPtr address, int rows, int columns)
        {
            Matrix matrix = new Matrix(rows, columns);
            byte[] data;
            if (Read(address, out data, SIZE_FLOAT * rows * columns))
                matrix.Read(data);
            return matrix;
        }
        #endregion
        #region WRITE
        public bool WriteByte(IntPtr address, byte value)
        {
            return Write(address, new byte[] { value });
        }
        public bool WriteChar(IntPtr address, char value)
        {
            return Write(address, new byte[] { (byte)value });
        }
        public bool WriteInt16(IntPtr address, short value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteUInt16(IntPtr address, ushort value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteInt32(IntPtr address, int value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteUInt32(IntPtr address, uint value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteInt64(IntPtr address, long value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteUInt64(IntPtr address, ulong value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteFloat(IntPtr address, float value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteDouble(IntPtr address, double value)
        {
            return Write(address, BitConverter.GetBytes(value));
        }
        public bool WriteString(IntPtr address, string text, Encoding encoding)
        {
            return Write(address, encoding.GetBytes(text));
        }
        public bool WriteVector2(IntPtr address, Vector2 vec)
        {
            byte[] data = new byte[SIZE_FLOAT * 2];
            Array.Copy(BitConverter.GetBytes(vec.X), 0, data, 0, SIZE_FLOAT);
            Array.Copy(BitConverter.GetBytes(vec.Y), 0, data, SIZE_FLOAT, SIZE_FLOAT);
            return Write(address, data);
        }
        public bool WriteVector3(IntPtr address, Vector3 vec)
        {
            byte[] data = new byte[SIZE_FLOAT * 3];
            Array.Copy(BitConverter.GetBytes(vec.X), 0, data, 0, SIZE_FLOAT);
            Array.Copy(BitConverter.GetBytes(vec.Y), 0, data, SIZE_FLOAT, SIZE_FLOAT);
            Array.Copy(BitConverter.GetBytes(vec.Z), 0, data, SIZE_FLOAT * 2, SIZE_FLOAT);
            return Write(address, data);
        }
        #endregion
        #endregion
        #endregion
    }
}