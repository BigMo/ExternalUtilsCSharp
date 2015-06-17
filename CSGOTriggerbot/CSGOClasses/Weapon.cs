using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses
{
    public class Weapon : BaseEntity
    {
        #region VARIABLES
        #endregion

        #region FIELDS
        public int m_iItemDefinitionIndex
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_iItemDefinitionIndex"); }
        }
        public int m_iState
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_iState"); }
        }
        public int m_iClip1
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_iClip1"); }
        }
        public float m_flNextPrimaryAttack
        {
            get { return this.ReadFieldProxy<float>("Weapon.m_flNextPrimaryAttack"); }
        }
        public int m_bCanReload
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_bCanReload"); }
        }
        public int m_iWeaponTableIndex
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_iWeaponTableIndex"); }
        }
        public float m_fAccuracyPenalty
        {
            get { return this.ReadFieldProxy<float>("Weapon.m_fAccuracyPenalty"); }
        }
        public int m_iWeaponID
        {
            get { return this.ReadFieldProxy<int>("Weapon.m_iWeaponID"); }
        }
        #endregion

        #region CONSTRUCTORS
        public Weapon(int address) : base(address)
        {
            this.AddField<int>("Weapon.m_iItemDefinitionIndex", CSGOOffsets.NetVars.Weapon.m_iItemDefinitionIndex);
            this.AddField<int>("Weapon.m_iState", CSGOOffsets.NetVars.Weapon.m_iState);
            this.AddField<int>("Weapon.m_iClip1", CSGOOffsets.NetVars.Weapon.m_iClip1);
            this.AddField<float>("Weapon.m_flNextPrimaryAttack", CSGOOffsets.NetVars.Weapon.m_flNextPrimaryAttack);
            this.AddField<int>("Weapon.m_bCanReload", CSGOOffsets.NetVars.Weapon.m_bCanReload);
            this.AddField<int>("Weapon.m_iWeaponTableIndex", CSGOOffsets.NetVars.Weapon.m_iWeaponTableIndex);
            this.AddField<float>("Weapon.m_fAccuracyPenalty", CSGOOffsets.NetVars.Weapon.m_fAccuracyPenalty);
            this.AddField<int>("Weapon.m_iWeaponID", CSGOOffsets.NetVars.Weapon.m_iWeaponID);
        }
        public Weapon(BaseEntity baseEntity)
            : base(baseEntity)
        {
            this.AddField<int>("Weapon.m_iItemDefinitionIndex", CSGOOffsets.NetVars.Weapon.m_iItemDefinitionIndex);
            this.AddField<int>("Weapon.m_iState", CSGOOffsets.NetVars.Weapon.m_iState);
            this.AddField<int>("Weapon.m_iClip1", CSGOOffsets.NetVars.Weapon.m_iClip1);
            this.AddField<int>("Weapon.m_flNextPrimaryAttack", CSGOOffsets.NetVars.Weapon.m_flNextPrimaryAttack);
            this.AddField<int>("Weapon.m_bCanReload", CSGOOffsets.NetVars.Weapon.m_bCanReload);
            this.AddField<int>("Weapon.m_iWeaponTableIndex", CSGOOffsets.NetVars.Weapon.m_iWeaponTableIndex);
            this.AddField<int>("Weapon.m_fAccuracyPenalty", CSGOOffsets.NetVars.Weapon.m_fAccuracyPenalty);
            this.AddField<int>("Weapon.m_iWeaponID", CSGOOffsets.NetVars.Weapon.m_iWeaponID);
        }
        public Weapon(Weapon other) 
            : base(other)
        {
            this.CopyFieldsFrom(other);
        }
        #endregion

        #region METHODS
        public override bool IsValid()
        {
            return base.IsValid() && this.m_iWeaponID > 0 && this.m_iItemDefinitionIndex > 0;
        }
        #endregion
    }
}
