using CSGOTriggerbot.CSGOClasses.Fields;
using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses
{
    public class BaseEntity : Entity
    {
        #region VARIABLES
        private uint iClientClass, iClassID;
        private string szClassName;
        #endregion

        #region PROPERTIES
        public uint m_iClientClass
        {
            get
            {
                if (iClientClass == 0)
                    iClientClass = GetClientClass();
                return iClientClass;
            }
            protected set { iClientClass = value; }
        }
        public uint m_iClassID
        {
            get
            {
                if (iClassID == 0)
                    iClassID = GetClassID();
                return iClassID;
            }
            protected set { iClassID = value; }
        }
        public string m_szClassName
        {
            get
            {
                if (szClassName == "<none>")
                    szClassName = GetClassName();
                return szClassName;
            }
            protected set { szClassName = value; }
        }
        #endregion

        #region FIELDS
        public int m_iHealth
        {
            get { return this.ReadFieldProxy<int>("CSPlayer.m_iHealth"); }
        }
        public int m_iVirtualTable
        {
            get { return ReadFieldProxy<int>("Entity.m_iVirtualTable"); }
        }
        public int m_iID
        {
            get { return ReadFieldProxy<int>("Entity.m_iID"); }
        }
        public byte m_iDormant
        {
            get { return ReadFieldProxy<byte>("Entity.m_iDormant"); }
        }
        public int m_hOwnerEntity
        {
            get { return ReadFieldProxy<int>("Entity.m_hOwnerEntity"); }
        }
        public int m_iTeamNum
        {
            get { return ReadFieldProxy<int>("Entity.m_iTeamNum"); }
        }
        public int m_bSpotted
        {
            get { return ReadFieldProxy<int>("Entity.m_bSpotted"); }
        }
        public long m_bSpottedByMask
        {
            get { return ReadFieldProxy<long>("Entity.m_bSpottedByMask"); }
        }
        public Vector3 m_vecOrigin
        {
            get { return ReadFieldProxy<Vector3>("Entity.m_vecOrigin"); }
        }
        public Vector3 m_angRotation
        {
            get { return ReadFieldProxy<Vector3>("Entity.m_angRotation"); }
        }
        #endregion

        #region CONSTRUCTOR
        public BaseEntity(int address)
            : base(address)
        {
            this.iClassID = 0;
            this.iClientClass = 0;
            this.szClassName = "<none>";
        }
        public BaseEntity(BaseEntity copyFrom)
            : base(copyFrom.Address)
        {
            this.Address = copyFrom.Address;
            this.CopyFieldsFrom(copyFrom);
            this.iClassID = copyFrom.m_iClassID;
            this.iClientClass = copyFrom.m_iClientClass;
            this.szClassName = copyFrom.m_szClassName;
        }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();
            this.AddField<int>("CSPlayer.m_iHealth", CSGOOffsets.NetVars.C_BaseEntity.m_iHealth);
            this.AddField<int>("Entity.m_iVirtualTable", 0x08);
            this.AddField<int>("Entity.m_iID", CSGOOffsets.NetVars.C_BaseEntity.m_iID);
            this.AddField<byte>("Entity.m_iDormant", CSGOOffsets.NetVars.C_BaseEntity.m_bDormant);
            this.AddField<int>("Entity.m_hOwnerEntity", CSGOOffsets.NetVars.C_BaseEntity.m_hOwnerEntity);
            this.AddField<int>("Entity.m_iTeamNum", CSGOOffsets.NetVars.C_BaseEntity.m_iTeamNum);
            this.AddField<int>("Entity.m_bSpotted", CSGOOffsets.NetVars.C_BaseEntity.m_bSpotted);
            this.AddField<long>("Entity.m_bSpottedByMask", CSGOOffsets.NetVars.C_BaseEntity.m_bSpottedByMask);
            this.AddField<Vector3>("Entity.m_vecOrigin", CSGOOffsets.NetVars.C_BaseEntity.m_vecOrigin);
            this.AddField<Vector3>("Entity.m_angRotation", CSGOOffsets.NetVars.C_BaseEntity.m_angRotation);
        }
        public override string ToString()
        {
            return string.Format("[BaseEntity m_iID={0}, m_iClassID={3}, m_szClassName={4}, m_vecOrigin={1}]\n{2}", this.m_iID, this.m_vecOrigin, base.ToString(), this.m_iClassID, this.m_szClassName);
        }
        public virtual bool IsValid()
        {
            return this.m_iDormant != 1 && this.m_iID > 0 && this.m_iClassID > 0;
        }
        public bool SeenBy(int entityIndex)
        {
            return (m_bSpottedByMask & (0x1 << entityIndex)) != 0;
        }
        public bool SeenBy(BaseEntity ent)
        {
            return SeenBy(ent.m_iID - 1);
        }
        protected uint GetClientClass()
        {
            try
            {
                if (m_iVirtualTable == 0)
                    return 0;
                uint function = WithOverlay.MemUtils.Read<uint>((IntPtr)(m_iVirtualTable + 2 * 0x04));
                if (function != 0xFFFFFFFF)
                    return WithOverlay.MemUtils.Read<uint>((IntPtr)(function + 0x01));
                else
                    return 0;
            }
            catch { return 0; }
        }
        protected uint GetClassID()
        {
            try
            {
                uint clientClass = GetClientClass();
                if (clientClass != 0)
                    return WithOverlay.MemUtils.Read<uint>((IntPtr)((long)clientClass + 20));
                return clientClass;
            }
            catch { return 0; }
        }
        protected string GetClassName()
        {
            try
            {
                uint clientClass = GetClientClass();
                if (clientClass != 0)
                {
                    int ptr = WithOverlay.MemUtils.Read<int>((IntPtr)(clientClass + 8));
                    return WithOverlay.MemUtils.ReadString((IntPtr)(ptr), 32, Encoding.ASCII);
                }
                return "none";
            }
            catch { return "none"; }
        }
        public bool IsPlayer()
        {
            return
                this.m_iClassID == (int)CSGO.ClassID.CSPlayer;
        }
        public bool IsWeapon()
        {
            return
                this.m_iClassID == (int)CSGO.ClassID.AK47 ||
                this.m_iClassID == (int)CSGO.ClassID.DEagle ||
                this.m_iClassID == (int)CSGO.ClassID.Knife ||
                this.m_iClassID == (int)CSGO.ClassID.KnifeGG ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponAUG ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponAWP ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponBizon ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponDualBerettas ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponElite ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponFiveSeven ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponG3SG1 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponG3SG1x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponGalilAR ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponGlock ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponHKP2000 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponM249 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponM249x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponM4 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponM4A1 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponMAG ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponMag7 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponMP7 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponMP9 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponNegev ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponNova ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponNOVA ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponP250 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponP90 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponP90x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponPPBizon ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponSCAR20 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponSCAR20x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponSG556 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponSSG08 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponTaser ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponTec9 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponTec9x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponUMP45 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponUMP45x ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponXM1014 ||
                this.m_iClassID == (int)CSGO.ClassID.WeaponXM1014x ||
                this.m_iClassID == (int)CSGO.ClassID.DecoyGrenade ||
                this.m_iClassID == (int)CSGO.ClassID.HEGrenade ||
                this.m_iClassID == (int)CSGO.ClassID.IncendiaryGrenade ||
                this.m_iClassID == (int)CSGO.ClassID.MolotovGrenade ||
                this.m_iClassID == (int)CSGO.ClassID.SmokeGrenade ||
                this.m_iClassID == (int)CSGO.ClassID.Flashbang;
        }
        public bool IsProp()
        {
            return
                this.m_iClassID == (int)CSGO.ClassID.DynamicProp ||
                this.m_iClassID == (int)CSGO.ClassID.PhysicsProp ||
                this.m_iClassID == (int)CSGO.ClassID.PhysicsPropMultiplayer;
        }
        #endregion
    }
}
