using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MemObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    public static class CSGOScanner
    {
        static ScanResult scan;

        static ProcessModule clientDll;
        static int clientDllBase;
        static ProcessModule engineDll;
        static int engineDllBase;
        public static void ScanOffsets(MemUtils memUtils,ProcessModule client,ProcessModule engine)
        {
            clientDll = client;
            engineDll = engine;
            clientDllBase = clientDll.BaseAddress.ToInt32();
            engineDllBase = engineDll.BaseAddress.ToInt32();
            EntityOff(memUtils);
            LocalPlayer(memUtils);
            Jump(memUtils);
            ClientState(memUtils);
            SetViewAngles(memUtils);
            SignOnState(memUtils);
            GlowManager(memUtils);
            WeaponTable(memUtils);
            EntityID(memUtils);
            EntityHealth(memUtils);
            EntityVecOrigin(memUtils);
            PlayerTeamNum(memUtils);
            PlayerBoneMatrix(memUtils);
            PlayerWeaponHandle(memUtils);
            vMatrix(memUtils);
            clientDll = null;
            engineDll = null;
            clientDllBase = 0;
            engineDllBase = 0;
        }
        #region MISC
        static void vMatrix(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] {                                 
                0x53, 0x8B, 0xDC, 0x83, 0xEC, 0x08, 0x83, 0xE4,
                0xF0, 0x83, 0xC4, 0x04, 0x55, 0x8B, 0x6B, 0x04,
                0x89, 0x6C, 0x24, 0x04, 0x8B, 0xEC, 0xA1, 0x00,
                0x00, 0x00, 0x00, 0x81, 0xEC, 0x98, 0x03, 0x00,
                0x00 }, "xxxxxxxxxxxxxxxxxxxxxxx????xxxxxx", clientDll);
            if (scan.Success)
            {
                int address = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + +0x4EE));
                address -= clientDllBase;
                address += 0x80;
                CSGOOffsets.MiscViewMatrix = address;
            }
        }
        static void EntityOff(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0x05, 0x00, 0x00, 0x00, 0x00, 0xC1, 0xe9, 0x00, 0x39, 0x48, 0x04 }, "x????xx?xxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 1));
                byte tmp2 = memUtils.Read<byte>((IntPtr)(scan.Address.ToInt32() + 7));
                CSGOOffsets.MiscEntityList = tmp + tmp2 - clientDllBase;
            }
        }
        static void LocalPlayer(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0x8D, 0x34, 0x85, 0x00, 0x00, 0x00, 0x00, 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x41, 0x08, 0x8B, 0x48 }, "xxx????xx????xxxxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 3));
                byte tmp2 = memUtils.Read<byte>((IntPtr)(scan.Address.ToInt32() + 18));
                CSGOOffsets.MiscLocalPlayer = tmp + tmp2 - clientDllBase;
            }
        }
        static void Jump(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0x89, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0xF6, 0xC2, 0x03, 0x74, 0x03, 0x83, 0xCE, 0x08, 0xA8, 0x08, 0xBF }, "xx????xx????xxxxxxxxxxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 2));
                CSGOOffsets.MiscJump = tmp - clientDllBase;
            }
        }
        static void ClientState(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0xC2, 0x00, 0x00, 0xCC, 0xCC, 0x8B, 0x0D, 0x00, 0x00, 0x00, 0x00, 0x33, 0xC0, 0x83, 0xB9 }, "x??xxxx????xxxx", engineDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 7));
                CSGOOffsets.MiscClientState = tmp - engineDllBase;
            }
        }
        static void SetViewAngles(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0x8B, 0x15, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x4D, 0x08, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x01, 0x8B, 0x82, 0x00, 0x00, 0x00, 0x00, 0x89, 0x41, 0x04 }, "xx????xxxxx????xxxx????xxx", engineDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 11));
                CSGOOffsets.MiscSetViewAngles = tmp;
            }
        }
        static void SignOnState(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x51, 0xA1, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x51, 0x00, 0x83, 0xB8, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7C, 0x40, 0x3B, 0xD1 },
                "xx????xx?xx?????xxxx", engineDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 11));
                CSGOOffsets.MiscSignOnState = tmp;
            }
        }
        static void GlowManager(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0x8D, 0x8F, 0x00, 0x00, 0x00, 0x00, 0xA1, 0x00, 0x00, 0x00, 0x00, 0xC7, 0x04, 0x02, 0x00, 0x00, 0x00, 0x00, 0x89, 0x35, 0x00, 0x00, 0x00, 0x00, 0x8B, 0x51 }, "xx????x????xxx????xx????xx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 7));
                CSGOOffsets.MiscGlowManager = tmp - clientDllBase;
            }
        }
        static void WeaponTable(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(new byte[] { 0xA1, 0x00, 0x00, 0x00, 0x00, 0x0F, 0xB7, 0xC9, 0x03, 0xC9, 0x8B, 0x44, 0x00, 0x0C, 0xC3 }, "x????xxxxxxx?xx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 1));
                CSGOOffsets.MiscWeaponTable = tmp - clientDllBase;
            }
        }
        #endregion
        #region ENTITY
        static void EntityID(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x74, 0x72, 0x80, 0x79, 0x00, 0x00, 0x8B, 0x56, 0x00, 0x89, 0x55, 0x00, 0x74, 0x17 },
                "xxxx??xx?xx?xx", clientDll);
            if (scan.Success)
            {
                byte tmp = memUtils.Read<byte>((IntPtr)(scan.Address.ToInt32() + 8));
                CSGOOffsets.NetVars.C_BaseEntity.m_iID = tmp;
            }
        }
        static void EntityHealth(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x41, 0x00, 0x89, 0x41, 0x00, 0x8B, 0x4F, 0x00, 0x83, 0xB9, 0x00, 0x00, 0x00, 0x00, 0x00, 0x7F, 0x2E },
                "xx?xx?xx?xx?xx?xx?xx?xx????xxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 23));
                CSGOOffsets.NetVars.C_BaseEntity.m_iHealth = tmp;
            }
        }
        static void EntityVecOrigin(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x8A, 0x0E, 0x80, 0xE1, 0xFC, 0x0A, 0xC8, 0x88, 0x0E, 0xF3, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x87, 0x00, 0x00, 0x00, 0x00, 0x9F },
                "xxxxxxxxxx??x??????x????x", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 13));
                CSGOOffsets.NetVars.C_BaseEntity.m_vecOrigin = tmp;
            }
        }
        #endregion
        #region PLAYER
        static void PlayerTeamNum(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0xCC, 0xCC, 0xCC, 0x8B, 0x89, 0x00, 0x00, 0x00, 0x00, 0xE9, 0x00, 0x00, 0x00, 0x00, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x8B, 0x81, 0x00, 0x00, 0x00, 0x00, 0xC3, 0xCC, 0xCC },
                "xxxxx????x????xxxxxxx????xxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 5));
                CSGOOffsets.NetVars.C_BaseEntity.m_iTeamNum = tmp;
            }
        }
        static void PlayerBoneMatrix(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x83, 0x3C, 0xB0, 0xFF, 0x75, 0x15, 0x8B, 0x87, 0x00, 0x00, 0x00, 0x00, 0x8B, 0xCF, 0x8B, 0x17, 0x03, 0x44, 0x24, 0x0C, 0x50 },
                "xxxxxxxx????xxxxxxxxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 8));
                CSGOOffsets.NetVars.C_CSPlayer.m_hBoneMatrix = tmp;
            }
        }
        static void PlayerWeaponHandle(MemUtils memUtils)
        {
            scan = memUtils.PerformSignatureScan(
                new byte[] { 0x0F, 0x45, 0xF7, 0x5F, 0x8B, 0x8E, 0x00, 0x00, 0x00, 0x00, 0x5E, 0x83, 0xF9, 0xFF },
                "xxxxxx????xxxx", clientDll);
            if (scan.Success)
            {
                int tmp = memUtils.Read<int>((IntPtr)(scan.Address.ToInt32() + 6));
                CSGOOffsets.NetVars.C_CSPlayer.m_hActiveWeapon = tmp;
            }
        }
        #endregion
    }

}
