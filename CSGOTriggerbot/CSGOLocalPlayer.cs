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
        [FieldOffset(0x13E8)]
        public Vector3 m_vecPunch;
        [FieldOffset(0x1d60)]
        public int m_iShotsFired;
        [FieldOffset(0x2400)]
        public int m_iCrosshairIdx;

        public bool IsValid()
        {
            return this.m_iID != 0 && this.m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }
    }
}
