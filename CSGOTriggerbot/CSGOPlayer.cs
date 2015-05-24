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
    struct CSGOPlayer
    {
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

        [FieldOffset(0xA78)]
        public int m_pBoneMatrix;

        public bool IsValid()
        {
            return this.m_iID != 0 && this.m_iDormant != 1 && this.m_iHealth > 0 && (m_iTeam == 2 || m_iTeam == 3);
        }

        public int GetBoneAddress(int boneIndex)
        {
            return m_pBoneMatrix + boneIndex * 0x30;
        }
    }
}
