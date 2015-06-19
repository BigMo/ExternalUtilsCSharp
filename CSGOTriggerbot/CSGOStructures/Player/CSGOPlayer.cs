using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CSGOPlayer
    {
        [FieldOffset(0x8)]
        public int m_iVirtualTable;

        [FieldOffset(0x64)]
        public int m_iID;

        [FieldOffset(0xE9)]
        public byte m_iDormant;

        [FieldOffset(0xF0)]
        public int m_iTeam;

        [FieldOffset(0xFC)]
        public int m_iHealth;

        [FieldOffset(0x134)]
        public Vector3 m_vecOrigin;

        [FieldOffset(0x935)]
        public bool m_bSpotted;

        [FieldOffset(0xA78)]
        public int m_pBoneMatrix;

        [FieldOffset(0x12C0)]
        public uint m_hActiveWeapon;

        public bool IsValid(MemUtils memUtils)
        {
            return this.m_iID != 0 && this.m_iDormant != 1 && this.m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }

        public int GetBoneAddress(int boneIndex)
        {
            return m_pBoneMatrix + boneIndex * 0x30;
        }
        public int GetClientClass(MemUtils memUtils)
        {
            int function = memUtils.Read<int>((IntPtr)(m_iVirtualTable + 2 * 0x04));
            return memUtils.Read<int>((IntPtr)(function + 0x01));
        }
        public int GetClassID(MemUtils memUtils)
        {
            return memUtils.Read<int>((IntPtr)(GetClientClass(memUtils) + 20));
        }
        public String GetName(MemUtils memUtils)
        {
            int ptr = memUtils.Read<int>((IntPtr)(GetClassID(memUtils) + 8));
            return memUtils.ReadString((IntPtr)(ptr + 8), 32, Encoding.ASCII);
        }
        public CSGOWeapon GetActiveWeapon(MemUtils memUtils)
        {
            if (this.m_hActiveWeapon == 0xFFFFFFFF)
                return new CSGOWeapon() { m_iItemDefinitionIndex = 0, m_iWeaponID = 0 };

            uint handle = this.m_hActiveWeapon & 0xFFF;
            int weapAddress = 0;// memUtils.Read<int>((IntPtr)(Program.entityAddresses[handle - 1]));
            return memUtils.Read<CSGOWeapon>((IntPtr)weapAddress);
        }
    }
}
