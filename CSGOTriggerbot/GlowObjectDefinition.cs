using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    [StructLayout(LayoutKind.Explicit)]
    public struct GlowObjectDefinition
    {
        [FieldOffset(0x00)]
        public int pEntity;
        [FieldOffset(0x04)]
        public float r;
        [FieldOffset(0x08)]
        public float g;
        [FieldOffset(0x0C)]
        public float b;
        [FieldOffset(0x10)]
        public float a;

        //16 bytes junk
        [FieldOffset(0x14)]
        public int junk01;
        [FieldOffset(0x18)]
        public int junk02;
        [FieldOffset(0x1C)]
        public int junk03;
        [FieldOffset(0x20)]
        public int junk04;

        [FieldOffset(0x24)]
        public bool m_bRenderWhenOccluded;
        [FieldOffset(0x25)]
        public bool m_bRenderWhenUnoccluded;
        [FieldOffset(0x26)]
        public bool m_bFullBloom;

        //10 bytes junk
        [FieldOffset(0x2A)]
        public int junk05;
        [FieldOffset(0x2E)]
        public int junk06;
        [FieldOffset(0x32)]
        public short junk07;

        public byte[] GetBytes()
        {
            byte[] data = new byte[0x34];
            Array.Copy(BitConverter.GetBytes(pEntity), 0, data, 0x00, 4);
            Array.Copy(BitConverter.GetBytes(r), 0, data, 0x04, 4);
            Array.Copy(BitConverter.GetBytes(g), 0, data, 0x08, 4);
            Array.Copy(BitConverter.GetBytes(b), 0, data, 0x0C, 4);
            Array.Copy(BitConverter.GetBytes(a), 0, data, 0x10, 4);
            Array.Copy(BitConverter.GetBytes(junk01), 0, data, 0x14, 4);
            Array.Copy(BitConverter.GetBytes(junk02), 0, data, 0x18, 4);
            Array.Copy(BitConverter.GetBytes(junk03), 0, data, 0x1C, 4);
            Array.Copy(BitConverter.GetBytes(junk04), 0, data, 0x20, 4);
            Array.Copy(BitConverter.GetBytes(m_bRenderWhenOccluded), 0, data, 0x24, 1);
            Array.Copy(BitConverter.GetBytes(m_bRenderWhenUnoccluded), 0, data, 0x25, 1);
            Array.Copy(BitConverter.GetBytes(m_bFullBloom), 0, data, 0x26, 1);
            Array.Copy(BitConverter.GetBytes(junk05), 0, data, 0x2A, 4);
            Array.Copy(BitConverter.GetBytes(junk06), 0, data, 0x2E, 4);
            Array.Copy(BitConverter.GetBytes(junk07), 0, data, 0x32, 2);
            return data;
        }

        public static int GetSize() { return 0x34; }
    }
}
