using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CSGOEntity
    {
        [FieldOffset(0x8)]
        public uint m_iVirtualTable;

        [FieldOffset(0x64)]
        public int m_iID;

        [FieldOffset(0xE9)]
        public byte m_iDormant;

        public bool IsValid(MemUtils memUtils)
        {
            return this.m_iID != 0 && this.m_iDormant != 1 && this.m_iVirtualTable != 0 && this.m_iVirtualTable != 0xFFFFFFFF;
        }
        public int GetClientClass(MemUtils memUtils)
        {
            uint function = memUtils.Read<uint>((IntPtr)(m_iVirtualTable + 2 * 0x04));
            if (function != 0xFFFFFFFF)
                return memUtils.Read<int>((IntPtr)(function + 0x01));
            else
                return -1;
        }
        public int GetClassID(MemUtils memUtils)
        {
            int clientClass = GetClientClass(memUtils);
            if (clientClass != -1)
                return memUtils.Read<int>((IntPtr)(clientClass + 20));
            return clientClass;
        }
        public string GetName(MemUtils memUtils)
        {
            int clientClass = GetClientClass(memUtils);
            if (clientClass != -1)
            {
                int ptr = memUtils.Read<int>((IntPtr)(GetClassID(memUtils) + 8));
                return memUtils.ReadString((IntPtr)(ptr + 8), 32, Encoding.ASCII);
            }
            return "none";
        }
    }
}
