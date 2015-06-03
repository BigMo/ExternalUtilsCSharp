using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    [StructLayout(LayoutKind.Explicit)]
    public struct CSGOWeaponInfo
    {
        [FieldOffset(0x748)]
        public int m_nType;

        [FieldOffset(0x750)]
        public int m_nID;

        [FieldOffset(0x988)]
        public byte m_IsFullAuto;

        [FieldOffset(0x9A8)]
        public float m_flPenetration;

        [FieldOffset(0x9AC)]
        public int m_nDamage;

        [FieldOffset(0x8B0)]
        public float m_flRange;

        [FieldOffset(0x9B4)]
        public float m_flRangeModifier;

        [FieldOffset(0x9B8)]
        public int m_nBullets;
    }
}
