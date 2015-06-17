using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses
{
    public class CSLocalPlayer : CSPlayer
    {
        #region FIELDS
        public Vector3 m_vecViewOffset
        {
            get { return this.ReadFieldProxy<Vector3>("CSLocalPlayer.m_vecViewOffset"); }
        }
        public Vector3 m_vecPunch
        {
            get { return this.ReadFieldProxy<Vector3>("CSLocalPlayer.m_vecPunch"); }
        }
        public int m_iShotsFired
        {
            get { return this.ReadFieldProxy<int>("CSLocalPlayer.m_iShotsFired"); }
        }
        public int m_iCrosshairIdx
        {
            get { return this.ReadFieldProxy<int>("CSLocalPlayer.m_iCrosshairIdx"); }
        }
        #endregion

        #region CONSTRUCTORS
        public CSLocalPlayer(int address)
            : base(address)
        {
            this.AddField<Vector3>("CSLocalPlayer.m_vecViewOffset", CSGOOffsets.NetVars.LocalPlayer.m_vecViewOffset);
            this.AddField<Vector3>("CSLocalPlayer.m_vecPunch", CSGOOffsets.NetVars.LocalPlayer.m_vecPunch);
            this.AddField<int>("CSLocalPlayer.m_iShotsFired", CSGOOffsets.NetVars.LocalPlayer.m_iShotsFired);
            this.AddField<int>("CSLocalPlayer.m_iCrosshairIdx", CSGOOffsets.NetVars.LocalPlayer.m_iCrosshairIdx);
        }
        public CSLocalPlayer(CSPlayer player)
            : base(player)
        {
            this.CopyFieldsFrom(player);
            this.AddField<Vector3>("CSLocalPlayer.m_vecViewOffset", CSGOOffsets.NetVars.LocalPlayer.m_vecViewOffset);
            this.AddField<Vector3>("CSLocalPlayer.m_vecPunch", CSGOOffsets.NetVars.LocalPlayer.m_vecPunch);
            this.AddField<int>("CSLocalPlayer.m_iShotsFired", CSGOOffsets.NetVars.LocalPlayer.m_iShotsFired);
            this.AddField<int>("CSLocalPlayer.m_iCrosshairIdx", CSGOOffsets.NetVars.LocalPlayer.m_iCrosshairIdx);
        }
        #endregion

        #region METHODS
        public override string ToString()
        {
            return string.Format("[CSLocalPlayer m_iCrosshairIdx={1}, m_iShotsFired={2}, m_vecPunch={0}]\n{3}", this.m_vecPunch, this.m_iCrosshairIdx, this.m_iShotsFired, base.ToString());
        }
        #endregion
    }
}
