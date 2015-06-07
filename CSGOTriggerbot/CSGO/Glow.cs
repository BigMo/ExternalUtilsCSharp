using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGO
{
    public static class Glow
    {
        public static bool GlowCheck(ClassID id,CSGOPlayer entity, ref Color clr)
        {
            switch (id)
            {
                case ClassID.CSPlayer:
                    {
                        if (entity.m_iTeam == Program.localPlayer.m_iTeam)
                            clr = Color.Blue;
                        else if (entity.m_bSpotted && (entity.m_iTeam == 3 || entity.m_iTeam == 3))
                            clr = Color.Green;
                        else
                            clr = Color.Red;
                        break;
                    }
                case ClassID.AK47:
                case ClassID.DEagle:
                case ClassID.WeaponAUG:
                case ClassID.WeaponAWP:
                case ClassID.WeaponBizon:
                case ClassID.WeaponElite:
                case ClassID.WeaponFiveSeven:
                case ClassID.WeaponG3SG1:
                case ClassID.WeaponGalilAR:
                case ClassID.WeaponGlock:
                case ClassID.WeaponHKP2000:
                case ClassID.WeaponM249:
                case ClassID.WeaponM249x:
                case ClassID.WeaponM4A1:
                case ClassID.WeaponMP7:
                case ClassID.WeaponMP9:
                case ClassID.WeaponMag7:
                case ClassID.WeaponNOVA:
                case ClassID.WeaponNegev:
                case ClassID.WeaponP250:
                case ClassID.WeaponP90:
                case ClassID.WeaponP90x:
                case ClassID.WeaponSCAR20:
                case ClassID.WeaponSG556:
                case ClassID.WeaponSSG08:
                case ClassID.WeaponTaser:
                case ClassID.WeaponTec9:
                case ClassID.WeaponTec9x:
                case ClassID.WeaponUMP45:
                case ClassID.WeaponXM1014:
                case ClassID.Weapon:
                case ClassID.WeaponNova:
                case ClassID.WeaponM4:
                case ClassID.WeaponUMP45x:
                case ClassID.WeaponXM1014x:
                case ClassID.WeaponMAG:
                case ClassID.WeaponG3SG1x:
                case ClassID.WeaponDualBerettas:
                case ClassID.WeaponPPBizon:
                case ClassID.WeaponSCAR20x:
                    {
                        clr = Color.Violet;
                        break;
                    }
                case ClassID.HEGrenade:
                case ClassID.SmokeGrenade:
                case ClassID.MolotovGrenade:
                case ClassID.IncendiaryGrenade:
                case ClassID.Flashbang:
                case ClassID.DecoyGrenade:
                case ClassID.ParticleDecoy:
                case ClassID.ParticleSmokeGrenade:
                case ClassID.SmokeStack:
                case ClassID.ParticleIncendiaryGrenade:
                case ClassID.ParticleFlash:
                    {
                        clr = Color.Green;
                        break;
                    }
                case ClassID.Hostage:
                case ClassID.Chicken:
                    {
                        clr = Color.HotPink;
                        break;
                    }
                case ClassID.C4:
                case ClassID.PlantedC4:
                    {
                        clr = Color.DarkViolet;
                        break;
                    }
                //case ClassID.DynamicProp:
                //case ClassID.Inferno:
                //case ClassID.BaseAnimating:
                //case ClassID.BaseDoor:
                //case ClassID.BaseEntity:
                //case ClassID.BaseTrigger:
                //case ClassID.CSGameRulesProxy:
                //case ClassID.CSPlayerResource:
                //case ClassID.CSRagdoll:
                //case ClassID.CascadeLight:
                //case ClassID.ColorCorrection:
                //case ClassID.EnvDetailController:
                //case ClassID.EnvTonemapController:
                //case ClassID.EnvWind:
                //case ClassID.FogController:
                //case ClassID.FuncBrush:
                //case ClassID.FuncOccluder:
                //case ClassID.FuncRotating:
                //case ClassID.Func_Dust:
                //case ClassID.LightGlow:
                //case ClassID.ParticleSystem:
                //case ClassID.PhysicsProp:
                //case ClassID.PhysicsPropMultiplayer:
                //case ClassID.PostProcessController:
                //case ClassID.PredictedViewModel:
                //case ClassID.PropDoorRotating:
                //case ClassID.RopeKeyframe:
                //case ClassID.ShadowControl:
                //case ClassID.Sprite:
                //case ClassID.Sun:
                //case ClassID.VGuiScreen:
                //case ClassID.VoteController:
                //case ClassID.Knife:
                default:
                    break;
            }
            return clr == Color.Black;
        }

    }
}
