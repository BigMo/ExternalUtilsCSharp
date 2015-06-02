using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    public static class Weapons
    {
        public enum CSGO_Weapon_ID
        {
            weapon_none,
            weapon_deagle,
            weapon_elite,
            weapon_fiveseven,
            weapon_glock,
            weapon_p228,
            weapon_usp,
            weapon_ak47,
            weapon_aug,
            weapon_awp,
            weapon_famas,
            weapon_g3sg1,
            weapon_galil,
            weapon_galilar,
            weapon_m249,
            weapon_m3,
            weapon_m4a1,
            weapon_mac10,
            weapon_mp5navy,
            weapon_p90,
            weapon_scout,
            weapon_sg550,
            weapon_sg552,
            weapon_tmp,
            weapon_ump45,
            weapon_xm1014,
            weapon_bizon,
            weapon_mag7,
            weapon_negev,
            weapon_sawedoff,
            weapon_tec9,
            weapon_taser,
            weapon_hkp2000,
            weapon_mp7,
            weapon_mp9,
            weapon_nova,
            weapon_p250,
            weapon_scar17,
            weapon_scar20,
            weapon_sg556,
            weapon_ssg08,
            weapon_knifegg,
            weapon_knife,
            weapon_flashbang,
            weapon_hegrenade,
            weapon_smokegrenade,
            weapon_molotov,
            weapon_decoy,
            weapon_incgrenade,
            weapon_c4
        };
        public static bool IsWeaponNonAim(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_knifegg || iWeaponID == (int)CSGO_Weapon_ID.weapon_knife || iWeaponID == (int)CSGO_Weapon_ID.weapon_flashbang || iWeaponID == (int)CSGO_Weapon_ID.weapon_hegrenade || iWeaponID == (int)CSGO_Weapon_ID.weapon_smokegrenade
                    || iWeaponID == (int)CSGO_Weapon_ID.weapon_molotov || iWeaponID == (int)CSGO_Weapon_ID.weapon_decoy || iWeaponID == (int)CSGO_Weapon_ID.weapon_incgrenade || iWeaponID == (int)CSGO_Weapon_ID.weapon_c4);
        }
        public static bool IsWeaponPistol(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_deagle || iWeaponID == (int)CSGO_Weapon_ID.weapon_elite || iWeaponID == (int)CSGO_Weapon_ID.weapon_fiveseven || iWeaponID == (int)CSGO_Weapon_ID.weapon_glock
                    || iWeaponID == (int)CSGO_Weapon_ID.weapon_p228 || iWeaponID == (int)CSGO_Weapon_ID.weapon_usp || iWeaponID == (int)CSGO_Weapon_ID.weapon_tec9 || iWeaponID == (int)CSGO_Weapon_ID.weapon_taser || iWeaponID == (int)CSGO_Weapon_ID.weapon_hkp2000);
        }
        public static bool IsWeaponSniper(int iWeaponID)
        {
            return (iWeaponID == (int)CSGO_Weapon_ID.weapon_awp || iWeaponID == (int)CSGO_Weapon_ID.weapon_scout
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_scar20 || iWeaponID == (int)CSGO_Weapon_ID.weapon_ssg08
                || iWeaponID == (int)CSGO_Weapon_ID.weapon_sg550 || iWeaponID == (int)CSGO_Weapon_ID.weapon_g3sg1);
        }
        public static int GetWeaponID(int entityaddress,int clientdllbase, MemUtils MemUtils)
        {
            var weapH = MemUtils.Read<int>((IntPtr)(entityaddress + Program.offsetWeaponH)) & 0xFFF;
            var weapAddress = MemUtils.Read<int>((IntPtr)(clientdllbase + Program.offsetEntityList - 0x10 + 0x10 * weapH));
            return MemUtils.Read<int>((IntPtr)(weapAddress + Program.offsetWeaponId));
        }
    }
}
