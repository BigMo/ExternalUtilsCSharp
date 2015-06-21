using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    class CSGOOffsets
    {
        public class Misc
        {
            public static int EntityList = 0x00;
            public static int LocalPlayer = 0x00;
            public static int Jump = 0x00;
            public static int GlowManager = 0x00;
            public static int SignOnState = 0xE8;
            public static int WeaponTable = 0x04A5DC4C;
            public static int ViewMatrix = 0x00;
            public static int MouseEnable = 0xA7A4C0;
        }
        public class ClientState
        {
            public static int Base = 0x00;
            public static int SignOnState = 0xE8;
            public static int SetViewAngles = 0x00;
        }
        public class GameResources
        {
            public static int Base = 0x04A36E8C;
            public static int Names = 0x9D0;
            public static int Kills = 0xBD8;
            public static int Assists = 0xCDC;
            public static int Deaths = 0xDE0;
            public static int Armor = 0x182C;
            public static int Score = 0x192C;
            public static int Clantag = 0x4110;
        }
        public class NetVars
        {
            public class C_BaseEntity
            {
                #region DUMP
                /*DT_BaseEntity
   DT_AnimTimeMustBeFirst
     m_flAnimTime : 0x25c [INT]
  AnimTimeMustBeFirst : 0x0 [TABLE]
  m_flSimulationTime : 0x264 [INT]
  m_cellbits : 0x74 [INT]
  m_cellX : 0x7c [INT]
  m_cellY : 0x80 [INT]
  m_cellZ : 0x84 [INT]
  m_vecOrigin : 0x134 [VECTOR3]
  m_angRotation : 0x128 [VECTOR3]
  m_nModelIndex : 0x254 [INT]
  m_fEffects : 0xec [INT]
  m_nRenderMode : 0x257 [INT]
  m_nRenderFX : 0x256 [INT]
  m_clrRender : 0x70 [INT]
  m_iTeamNum : 0xf0 [INT]
  m_iPendingTeamNum : 0xf4 [INT]
  m_CollisionGroup : 0x46c [INT]
  m_flElasticity : 0x394 [FLOAT]
  m_flShadowCastDistance : 0x398 [FLOAT]
  m_hOwnerEntity : 0x148 [INT]
  m_hEffectEntity : 0x990 [INT]
  moveparent : 0x144 [INT]
  m_iParentAttachment : 0x2e4 [INT]
  m_iName : 0x150 [STRING]
  movetype : 0x0 [INT]
  movecollide : 0x0 [INT]
   DT_CollisionProperty
     m_vecMins : 0x8 [VECTOR3]
     m_vecMaxs : 0x14 [VECTOR3]
     m_nSolidType : 0x22 [INT]
     m_usSolidFlags : 0x20 [INT]
     m_nSurroundType : 0x2a [INT]
     m_triggerBloat : 0x23 [INT]
     m_vecSpecifiedSurroundingMins : 0x2c [VECTOR3]
     m_vecSpecifiedSurroundingMaxs : 0x38 [VECTOR3]
  m_Collision : 0x314 [TABLE]
  m_iTextureFrameIndex : 0x984 [INT]
  m_bSimulatedEveryTick : 0x932 [INT]
  m_bAnimatedEveryTick : 0x933 [INT]
  m_bAlternateSorting : 0x934 [INT]
  m_bSpotted : 0x935 [INT]
   m_bSpottedBy
  m_bSpottedBy : 0x936 [TABLE]
   m_bSpottedByMask
  m_bSpottedByMask : 0x978 [TABLE]
  m_bIsAutoaimTarget : 0x60 [INT]
  m_fadeMinDist : 0x2ec [FLOAT]
  m_fadeMaxDist : 0x2f0 [FLOAT]
  m_flFadeScale : 0x2f4 [FLOAT]
  m_nMinCPULevel : 0x980 [INT]
  m_nMaxCPULevel : 0x981 [INT]
  m_nMinGPULevel : 0x982 [INT]
  m_nMaxGPULevel : 0x983 [INT]*/
                #endregion
                public static int m_iHealth = 0x00;
                public static int m_iID = 0x00;
                public static int m_iTeamNum = 0x00;
                public static int m_vecOrigin = 0x134;
                public static int m_angRotation = 0x128;
                public static int m_bSpotted = 0x935;
                public static int m_bSpottedByMask = 0x978;
                public static int m_hOwnerEntity = 0x148;
                public static int m_bDormant = 0xE9;
            }

            public class C_CSPlayer
            {
                #region DUMP
                /*DT_CSPlayer
   DT_BasePlayer
      DT_BaseCombatCharacter
         DT_BaseFlex
            DT_BaseAnimatingOverlay
               DT_BaseAnimating
                  DT_BaseEntity
                     DT_AnimTimeMustBeFirst
                       m_flAnimTime : 0x25c [INT]
                    AnimTimeMustBeFirst : 0x0 [TABLE]
                    m_flSimulationTime : 0x264 [INT]
                    m_cellbits : 0x74 [INT]
                    m_cellX : 0x7c [INT]
                    m_cellY : 0x80 [INT]
                    m_cellZ : 0x84 [INT]
                    m_vecOrigin : 0x134 [VECTOR3]
                    m_angRotation : 0x128 [VECTOR3]
                    m_nModelIndex : 0x254 [INT]
                    m_fEffects : 0xec [INT]
                    m_nRenderMode : 0x257 [INT]
                    m_nRenderFX : 0x256 [INT]
                    m_clrRender : 0x70 [INT]
                    m_iTeamNum : 0xf0 [INT]
                    m_iPendingTeamNum : 0xf4 [INT]
                    m_CollisionGroup : 0x46c [INT]
                    m_flElasticity : 0x394 [FLOAT]
                    m_flShadowCastDistance : 0x398 [FLOAT]
                    m_hOwnerEntity : 0x148 [INT]
                    m_hEffectEntity : 0x990 [INT]
                    moveparent : 0x144 [INT]
                    m_iParentAttachment : 0x2e4 [INT]
                    m_iName : 0x150 [STRING]
                    movetype : 0x0 [INT]
                    movecollide : 0x0 [INT]
                     DT_CollisionProperty
                       m_vecMins : 0x8 [VECTOR3]
                       m_vecMaxs : 0x14 [VECTOR3]
                       m_nSolidType : 0x22 [INT]
                       m_usSolidFlags : 0x20 [INT]
                       m_nSurroundType : 0x2a [INT]
                       m_triggerBloat : 0x23 [INT]
                       m_vecSpecifiedSurroundingMins : 0x2c [VECTOR3]
                       m_vecSpecifiedSurroundingMaxs : 0x38 [VECTOR3]
                    m_Collision : 0x314 [TABLE]
                    m_iTextureFrameIndex : 0x984 [INT]
                    m_bSimulatedEveryTick : 0x932 [INT]
                    m_bAnimatedEveryTick : 0x933 [INT]
                    m_bAlternateSorting : 0x934 [INT]
                    m_bSpotted : 0x935 [INT]
                     m_bSpottedBy
                    m_bSpottedBy : 0x936 [TABLE]
                     m_bSpottedByMask
                    m_bSpottedByMask : 0x978 [TABLE]
                    m_bIsAutoaimTarget : 0x60 [INT]
                    m_fadeMinDist : 0x2ec [FLOAT]
                    m_fadeMaxDist : 0x2f0 [FLOAT]
                    m_flFadeScale : 0x2f4 [FLOAT]
                    m_nMinCPULevel : 0x980 [INT]
                    m_nMaxCPULevel : 0x981 [INT]
                    m_nMinGPULevel : 0x982 [INT]
                    m_nMaxGPULevel : 0x983 [INT]
                 baseclass : 0x0 [TABLE]
                 m_nSequence : 0xc90 [INT]
                 m_nForceBone : 0xa5c [INT]
                 m_vecForce : 0xa50 [VECTOR3]
                 m_nSkin : 0xa14 [INT]
                 m_nBody : 0xa18 [INT]
                 m_nHitboxSet : 0x9f0 [INT]
                 m_flModelScale : 0xb18 [FLOAT]
                  m_flPoseParameter
                 m_flPoseParameter : 0xb48 [TABLE]
                 m_flPlaybackRate : 0xa10 [FLOAT]
                  m_flEncodedController
                 m_flEncodedController : 0xa2c [TABLE]
                 m_bClientSideAnimation : 0xc70 [INT]
                 m_bClientSideFrameReset : 0xa90 [INT]
                 m_bClientSideRagdoll : 0x275 [INT]
                 m_nNewSequenceParity : 0xa1c [INT]
                 m_nResetEventsParity : 0xa20 [INT]
                 m_nMuzzleFlashParity : 0xa3c [INT]
                 m_hLightingOrigin : 0xd18 [INT]
                  DT_ServerAnimationData
                    m_flCycle : 0xa0c [FLOAT]
                 serveranimdata : 0x0 [TABLE]
                 m_flFrozen : 0xac8 [FLOAT]
                 m_ScaleType : 0xb1c [INT]
                 m_bSuppressAnimSounds : 0xd1e [INT]
              baseclass : 0x0 [TABLE]
               DT_OverlayVars
                  _ST_m_AnimOverlay_15
                     _LPT_m_AnimOverlay_15
                       lengthprop15 : 0x0 [INT]
                    lengthproxy : 0x0 [TABLE]
                 m_AnimOverlay : 0x0 [TABLE]
              overlay_vars : 0x0 [TABLE]
           baseclass : 0x0 [TABLE]
            m_flexWeight
           m_flexWeight : 0xdf0 [TABLE]
           m_blinktoggle : 0xf9c [INT]
           m_viewtarget : 0xdb8 [VECTOR3]
        baseclass : 0x0 [TABLE]
         DT_BCCLocalPlayerExclusive
           m_flNextAttack : 0x1138 [FLOAT]
        bcc_localdata : 0x0 [TABLE]
         DT_BCCNonLocalPlayerExclusive
            m_hMyWeapons
           m_hMyWeapons : 0x11c0 [TABLE]
        bcc_nonlocaldata : 0x0 [TABLE]
        m_LastHitGroup : 0x113c [INT]
        m_hActiveWeapon : 0x12c0 [INT]
        m_flTimeOfLastInjury : 0x12c4 [FLOAT]
        m_nRelativeDirectionOfLastInjury : 0x12c8 [INT]
         m_hMyWeapons
        m_hMyWeapons : 0x11c0 [TABLE]
     baseclass : 0x0 [TABLE]
      DT_LocalPlayerExclusive
         DT_Local
            m_chAreaBits
           m_chAreaBits : 0x4 [TABLE]
            m_chAreaPortalBits
           m_chAreaPortalBits : 0x24 [TABLE]
           m_iHideHUD : 0x48 [INT]
           m_flFOVRate : 0x44 [FLOAT]
           m_bDucked : 0x88 [INT]
           m_bDucking : 0x89 [INT]
           m_bInDuckJump : 0x8a [INT]
           m_nDuckTimeMsecs : 0x4c [INT]
           m_nDuckJumpTimeMsecs : 0x50 [INT]
           m_nJumpTimeMsecs : 0x54 [INT]
           m_flFallVelocity : 0x58 [FLOAT]
           m_viewPunchAngle : 0x64 [VECTOR3]
           m_aimPunchAngle : 0x70 [VECTOR3]
           m_aimPunchAngleVel : 0x7c [VECTOR3]
           m_bDrawViewmodel : 0x8b [INT]
           m_bWearingSuit : 0x8c [INT]
           m_bPoisoned : 0x8d [INT]
           m_flStepSize : 0x60 [FLOAT]
           m_bAllowAutoMovement : 0x8e [INT]
           m_skybox3d.scale : 0x12c [INT]
           m_skybox3d.origin : 0x130 [VECTOR3]
           m_skybox3d.area : 0x13c [INT]
           m_skybox3d.fog.enable : 0x184 [INT]
           m_skybox3d.fog.blend : 0x185 [INT]
           m_skybox3d.fog.dirPrimary : 0x144 [VECTOR3]
           m_skybox3d.fog.colorPrimary : 0x150 [INT]
           m_skybox3d.fog.colorSecondary : 0x154 [INT]
           m_skybox3d.fog.start : 0x160 [FLOAT]
           m_skybox3d.fog.end : 0x164 [FLOAT]
           m_skybox3d.fog.maxdensity : 0x16c [FLOAT]
           m_skybox3d.fog.HDRColorScale : 0x18c [FLOAT]
           m_audio.localSound[0] : 0x194 [VECTOR3]
           m_audio.localSound[1] : 0x1a0 [VECTOR3]
           m_audio.localSound[2] : 0x1ac [VECTOR3]
           m_audio.localSound[3] : 0x1b8 [VECTOR3]
           m_audio.localSound[4] : 0x1c4 [VECTOR3]
           m_audio.localSound[5] : 0x1d0 [VECTOR3]
           m_audio.localSound[6] : 0x1dc [VECTOR3]
           m_audio.localSound[7] : 0x1e8 [VECTOR3]
           m_audio.soundscapeIndex : 0x1f4 [INT]
           m_audio.localBits : 0x1f8 [INT]
           m_audio.entIndex : 0x1fc [INT]
        m_Local : 0x1378 [TABLE]
        m_vecViewOffset[0] : 0x104 [FLOAT]
        m_vecViewOffset[1] : 0x108 [FLOAT]
        m_vecViewOffset[2] : 0x10c [FLOAT]
        m_flFriction : 0x140 [FLOAT]
        m_fOnTarget : 0x16e4 [INT]
        m_nTickBase : 0x17cc [INT]
        m_nNextThinkTick : 0xf8 [INT]
        m_hLastWeapon : 0x16b8 [INT]
        m_vecVelocity[0] : 0x110 [FLOAT]
        m_vecVelocity[1] : 0x114 [FLOAT]
        m_vecVelocity[2] : 0x118 [FLOAT]
        m_vecBaseVelocity : 0x11c [VECTOR3]
        m_hConstraintEntity : 0x1700 [INT]
        m_vecConstraintCenter : 0x1704 [VECTOR3]
        m_flConstraintRadius : 0x1710 [FLOAT]
        m_flConstraintWidth : 0x1714 [FLOAT]
        m_flConstraintSpeedFactor : 0x1718 [FLOAT]
        m_bConstraintPastRadius : 0x171c [INT]
        m_flDeathTime : 0x1764 [FLOAT]
        m_fForceTeam : 0x1768 [FLOAT]
        m_nWaterLevel : 0x25a [INT]
        m_flLaggedMovementValue : 0x1930 [FLOAT]
        m_hTonemapController : 0x1580 [INT]
     localdata : 0x0 [TABLE]
      DT_PlayerState
        deadflag : 0x4 [INT]
     pl : 0x1584 [TABLE]
     m_iFOV : 0x1598 [INT]
     m_iFOVStart : 0x159c [INT]
     m_flFOVTime : 0x15bc [FLOAT]
     m_iDefaultFOV : 0x16ec [INT]
     m_hZoomOwner : 0x1610 [INT]
     m_afPhysicsFlags : 0x16b0 [INT]
     m_hVehicle : 0x16b4 [INT]
     m_hUseEntity : 0x16e8 [INT]
     m_hGroundEntity : 0x14c [INT]
     m_iHealth : 0xfc [INT]
     m_lifeState : 0x25b [INT]
      m_iAmmo
     m_iAmmo : 0x1140 [TABLE]
     m_iBonusProgress : 0x1604 [INT]
     m_iBonusChallenge : 0x1608 [INT]
     m_flMaxspeed : 0x160c [FLOAT]
     m_fFlags : 0x100 [INT]
     m_iObserverMode : 0x1728 [INT]
     m_bActiveCameraMan : 0x172c [INT]
     m_bCameraManXRay : 0x172d [INT]
     m_bCameraManOverview : 0x172e [INT]
     m_bCameraManScoreBoard : 0x172f [INT]
     m_uCameraManGraphs : 0x1730 [INT]
     m_hObserverTarget : 0x173c [INT]
     m_hViewModel[0] : 0x16bc [INT]
     m_hViewModel : 0x0 [ARRAY]
     m_iCoachingTeam : 0x1310 [INT]
     m_szLastPlaceName : 0x1950 [STRING]
     m_vecLadderNormal : 0x15d4 [VECTOR3]
     m_ladderSurfaceProps : 0x15b4 [INT]
     m_ubEFNoInterpParity : 0x1974 [INT]
     m_hPostProcessCtrl : 0x1b34 [INT]
     m_hColorCorrectionCtrl : 0x1b38 [INT]
     m_PlayerFog.m_hCtrl : 0x1b40 [INT]
     m_vphysicsCollisionState : 0x1620 [INT]
     m_hViewEntity : 0x16f8 [INT]
     m_bShouldDrawPlayerWhileUsingViewEntity : 0x16fc [INT]
  baseclass : 0x0 [TABLE]
   DT_CSLocalPlayerExclusive
     m_vecOrigin : 0x134 [VECTOR2]
     m_vecOrigin[2] : 0x13c [FLOAT]
     m_flStamina : 0x1d64 [FLOAT]
     m_iDirection : 0x1d68 [INT]
     m_iShotsFired : 0x1d6c [INT]
     m_nNumFastDucks : 0x1d70 [INT]
     m_bDuckOverride : 0x1d74 [INT]
     m_flVelocityModifier : 0x1d78 [FLOAT]
      m_bPlayerDominated
     m_bPlayerDominated : 0x24e8 [TABLE]
      m_bPlayerDominatingMe
     m_bPlayerDominatingMe : 0x2529 [TABLE]
      m_iWeaponPurchasesThisRound
     m_iWeaponPurchasesThisRound : 0x256c [TABLE]
  cslocaldata : 0x0 [TABLE]
   DT_CSNonLocalPlayerExclusive
     m_vecOrigin : 0x134 [VECTOR2]
     m_vecOrigin[2] : 0x13c [FLOAT]
  csnonlocaldata : 0x0 [TABLE]
  m_angEyeAngles[0] : 0x23b4 [FLOAT]
  m_angEyeAngles[1] : 0x23b8 [FLOAT]
  m_iAddonBits : 0x1d50 [INT]
  m_iPrimaryAddon : 0x1d54 [INT]
  m_iSecondaryAddon : 0x1d58 [INT]
  m_iThrowGrenadeCounter : 0x1c34 [INT]
  m_bWaitForNoAttack : 0x1c38 [INT]
  m_bIsRespawningForDMBonus : 0x1c39 [INT]
  m_iPlayerState : 0x1c04 [INT]
  m_iAccount : 0x23a4 [INT]
  m_iStartAccount : 0x1d80 [INT]
  m_totalHitsOnServer : 0x1d84 [INT]
  m_bInBombZone : 0x1c30 [INT]
  m_bInBuyZone : 0x1c31 [INT]
  m_bInNoDefuseArea : 0x1c32 [INT]
  m_bKilledByTaser : 0x1c44 [INT]
  m_iMoveState : 0x1c48 [INT]
  m_iClass : 0x23ac [INT]
  m_ArmorValue : 0x23b0 [INT]
  m_angEyeAngles : 0x23b4 [VECTOR3]
  m_bHasDefuser : 0x23c0 [INT]
  m_bNightVisionOn : 0x1d75 [INT]
  m_bHasNightVision : 0x1d76 [INT]
  m_bInHostageRescueZone : 0x23c1 [INT]
  m_bIsDefusing : 0x1c08 [INT]
  m_bIsGrabbingHostage : 0x1c09 [INT]
  m_bIsScoped : 0x1c00 [INT]
  m_bIsWalking : 0x1c01 [INT]
  m_bResumeZoom : 0x1c02 [INT]
  m_fImmuneToGunGameDamageTime : 0x1c0c [FLOAT]
  m_bGunGameImmunity : 0x1c14 [INT]
  m_bHasMovedSinceSpawn : 0x1c15 [INT]
  m_bMadeFinalGunGameProgressiveKill : 0x1c16 [INT]
  m_iGunGameProgressiveWeaponIndex : 0x1c18 [INT]
  m_iNumGunGameTRKillPoints : 0x1c1c [INT]
  m_iNumGunGameKillsWithCurrentWeapon : 0x1c20 [INT]
  m_iNumRoundKills : 0x1c24 [INT]
  m_fMolotovUseTime : 0x1c2c [FLOAT]
  m_szArmsModel : 0x1c4f [STRING]
  m_hCarriedHostage : 0x1d90 [INT]
  m_hCarriedHostageProp : 0x1d94 [INT]
  m_bIsRescuing : 0x1c0a [INT]
  m_flGroundAccelLinearFracLastTime : 0x1d7c [FLOAT]
  m_bCanMoveDuringFreezePeriod : 0x1c4c [INT]
  m_isCurrentGunGameLeader : 0x1c4d [INT]
  m_isCurrentGunGameTeamLeader : 0x1c4e [INT]
  m_flGuardianTooFarDistFrac : 0x1c3c [FLOAT]
   m_iMatchStats_Kills
  m_iMatchStats_Kills : 0x1dfc [TABLE]
   m_iMatchStats_Damage
  m_iMatchStats_Damage : 0x1e74 [TABLE]
   m_iMatchStats_EquipmentValue
  m_iMatchStats_EquipmentValue : 0x1eec [TABLE]
   m_iMatchStats_MoneySaved
  m_iMatchStats_MoneySaved : 0x1f64 [TABLE]
   m_iMatchStats_KillReward
  m_iMatchStats_KillReward : 0x1fdc [TABLE]
   m_iMatchStats_LiveTime
  m_iMatchStats_LiveTime : 0x2054 [TABLE]
   m_iMatchStats_Deaths
  m_iMatchStats_Deaths : 0x20cc [TABLE]
   m_iMatchStats_Assists
  m_iMatchStats_Assists : 0x2144 [TABLE]
   m_iMatchStats_HeadShotKills
  m_iMatchStats_HeadShotKills : 0x21bc [TABLE]
   m_iMatchStats_Objective
  m_iMatchStats_Objective : 0x2234 [TABLE]
   m_iMatchStats_CashEarned
  m_iMatchStats_CashEarned : 0x22ac [TABLE]
   m_rank
  m_rank : 0x2388 [TABLE]
  m_unMusicID : 0x23a0 [INT]
  m_bHasHelmet : 0x23a8 [INT]
  m_flFlashDuration : 0x1db4 [FLOAT]
  m_flFlashMaxAlpha : 0x1db0 [FLOAT]
  m_iProgressBarDuration : 0x1d5c [INT]
  m_flProgressBarStartTime : 0x1d60 [FLOAT]
  m_hRagdoll : 0x1d8c [INT]
  m_cycleLatch : 0x24e0 [INT]
  m_unCurrentEquipmentValue : 0x2382 [INT]
  m_unRoundStartEquipmentValue : 0x2384 [INT]
  m_unFreezetimeEndEquipmentValue : 0x2386 [INT]
  m_bIsControllingBot : 0x270d [INT]
  m_bHasControlledBotThisRound : 0x2714 [INT]
  m_bCanControlObservedBot : 0x270e [INT]
  m_iControlledBotEntIndex : 0x2710 [INT]
  m_bHud_MiniScoreHidden : 0x23d3 [INT]
  m_bHud_RadarHidden : 0x23d4 [INT]
  m_nLastKillerIndex : 0x23d8 [INT]
  m_nLastConcurrentKilled : 0x23dc [INT]
  m_nDeathCamMusic : 0x23e0 [INT]
  m_bIsHoldingLookAtWeapon : 0x2675 [INT]
  m_bIsLookingAtWeapon : 0x2674 [INT]
  m_iNumRoundKillsHeadshots : 0x1c28 [INT]*/
                #endregion
                public static int m_lifeState = 0x25B;
                public static int m_hBoneMatrix = 0x00;
                public static int m_hActiveWeapon = 0x12C0;   // m_hActiveWeapon
                public static int m_iFlags = 0x100;
                public static int m_hObserverTarget = 0x173C;
                public static int m_iObserverMode = 0x1728;
                public static int m_vecVelocity = 0x110;
            }

            public class LocalPlayer
            {
                public static int m_vecViewOffset = 0x104;
                public static int m_vecPunch = 0x13E8;
                public static int m_iShotsFired = 0x1d6C;
                public static int m_iCrosshairIdx = 0x2410;
            }

            public class Weapon
            {
                public static int m_iItemDefinitionIndex = 0x131C;
                public static int m_iState = 0x15B4;
                public static int m_iClip1 = 0x15c0;
                public static int m_flNextPrimaryAttack = 0x159C;
                public static int m_iWeaponID = 0x1690;   // Search for weaponid
                public static int m_bCanReload = 0x15F9;
                public static int m_iWeaponTableIndex = 0x162C;
                public static int m_fAccuracyPenalty = 0x1670;
            }
        }
    }
}
