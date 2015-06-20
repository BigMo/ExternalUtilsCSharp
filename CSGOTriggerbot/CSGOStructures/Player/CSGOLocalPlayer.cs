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
    struct CSGOLocalPlayer
    {
        [FieldOffset(0x64)]
        public int m_iID;

        [FieldOffset(0xF0)]
        public int m_iTeam;

        [FieldOffset(0xFC)]
        public int m_iHealth;

        [FieldOffset(0x100)]
        public int m_iFlags;

        [FieldOffset(0x104)]
        public Vector3 m_vecViewOffset;

        [FieldOffset(0x110)]
        public Vector3 m_vecVelocity;

        [FieldOffset(0x134)]
        public Vector3 m_vecOrigin;

        [FieldOffset(0x12C0)]
        public uint m_hActiveWeapon;

        [FieldOffset(0x13E8)]
        public Vector3 m_vecPunch;

        [FieldOffset(0x1d6C)]
        public int m_iShotsFired;

        [FieldOffset(0x2410)]
        public int m_iCrosshairIdx;

        public bool IsValid()
        {
            return this.m_iID != 0 && this.m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }
        public CSGOWeapon GetActiveWeapon(MemUtils memUtils)
        {
            if (this.m_hActiveWeapon == 0xFFFFFFFF)
                return new CSGOWeapon() { m_iItemDefinitionIndex = 0, m_iWeaponID = 0 };

            uint handle = this.m_hActiveWeapon & 0xFFF;
            int weapAddress = 0;// Program.entityAddresses[handle - 1];
            return memUtils.Read<CSGOWeapon>((IntPtr)weapAddress);
        }
    }
}
