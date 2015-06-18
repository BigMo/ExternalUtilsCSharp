using CSGOTriggerbot.CSGOClasses.Fields;
using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses
{
    public class CSPlayer : BaseEntity
    {
        #region VARIABLES
        private uint iWeaponIndex;
        #endregion

        #region FIELDS
        public int m_hBoneMatrix
        {
            get { return this.ReadFieldProxy<int>("CSPlayer.m_hBoneMatrix"); }
        }
        public int m_iFlags
        {
            get { return this.ReadFieldProxy<int>("CSPlayer.m_iFlags"); }
        }
        public uint m_hActiveWeapon
        {
            get { return this.ReadFieldProxy<uint>("CSPlayer.m_hActiveWeapon"); }
        }
        public Vector3 m_vecVelocity
        {
            get { return this.ReadFieldProxy<Vector3>("CSPlayer.m_vecVelocity"); }
        }
        public uint m_iWeaponIndex
        {
            get
            {
                if (iWeaponIndex == 0)
                {
                    if (this.m_hActiveWeapon != 0xFFFFFFFF)
                    iWeaponIndex = this.m_hActiveWeapon & 0xFFF;
                }
                return iWeaponIndex;
            }
        }
        public Skeleton Bones { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public CSPlayer(int address) : base(address)
        {
            iWeaponIndex = 0;
            this.Bones = new Skeleton(this.m_hBoneMatrix);
        }
        public CSPlayer(BaseEntity baseEntity)
            : base(baseEntity)
        {
            iWeaponIndex = 0;
            this.Bones = new Skeleton(this.m_hBoneMatrix);
        }
        public CSPlayer(CSPlayer copyFrom) : base(copyFrom)
        {
            this.CopyFieldsFrom(copyFrom);
            iWeaponIndex = 0;
            this.Bones = copyFrom.Bones;
        }
        #endregion

        #region METHODS
        protected override void SetupFields()
        {
            base.SetupFields();
            this.AddField<int>("CSPlayer.m_hBoneMatrix", CSGOOffsets.NetVars.C_CSPlayer.m_hBoneMatrix);
            this.AddField<uint>("CSPlayer.m_hActiveWeapon", CSGOOffsets.NetVars.C_CSPlayer.m_hActiveWeapon);
            this.AddField<int>("CSPlayer.m_iFlags", CSGOOffsets.NetVars.C_CSPlayer.m_iFlags);
            this.AddField<Vector3>("CSPlayer.m_vecVelocity", CSGOOffsets.NetVars.C_CSPlayer.m_vecVelocity);
        }
        public override bool IsValid()
        {
            return base.IsValid() && (this.m_iTeamNum == 2 ||this.m_iTeamNum == 3);
        }
        public override string ToString()
        {
            return string.Format("[CSPlayer m_iHealth={0}, m_iTeamNum={3}, m_iFlags={1}]\n{2}", this.m_iHealth, Convert.ToString(this.m_iFlags, 2).PadLeft(32, '0'), base.ToString(), this.m_iTeamNum);
        }
        #endregion

        #region CLASSES
        public class Skeleton : Entity
        {
            #region FIELDS
            public Vector3 Head
            {
                get { return ReadFieldProxy<Vector3>("Head"); }
            }
            public Vector3 Neck
            {
                get { return ReadFieldProxy<Vector3>("Neck"); }
            }
            public Vector3 Spine1
            {
                get { return ReadFieldProxy<Vector3>("Spine1"); }
            }
            public Vector3 Spine2
            {
                get { return ReadFieldProxy<Vector3>("Spine2"); }
            }
            public Vector3 Spine3
            {
                get { return ReadFieldProxy<Vector3>("Spine3"); }
            }
            public Vector3 Spine4
            {
                get { return ReadFieldProxy<Vector3>("Spine4"); }
            }
            public Vector3 Spine5
            {
                get { return ReadFieldProxy<Vector3>("Spine5"); }
            }
            public Vector3 LeftHand
            {
                get { return ReadFieldProxy<Vector3>("LeftHand"); }
            }
            public Vector3 LeftElbow
            {
                get { return ReadFieldProxy<Vector3>("LeftElbow"); }
            }
            public Vector3 LeftShoulder
            {
                get { return ReadFieldProxy<Vector3>("LeftShoulder"); }
            }
            public Vector3 RightShoulder
            {
                get { return ReadFieldProxy<Vector3>("RightShoulder"); }
            }
            public Vector3 RightElbow
            {
                get { return ReadFieldProxy<Vector3>("RightElbow"); }
            }
            public Vector3 RightHand
            {
                get { return ReadFieldProxy<Vector3>("RightHand"); }
            }
            public Vector3 LeftToe
            {
                get { return ReadFieldProxy<Vector3>("LeftToe"); }
            }
            public Vector3 LeftFoot
            {
                get { return ReadFieldProxy<Vector3>("LeftFoot"); }
            }
            public Vector3 LeftKnee
            {
                get { return ReadFieldProxy<Vector3>("LeftKnee"); }
            }
            public Vector3 LeftHip
            {
                get { return ReadFieldProxy<Vector3>("LeftHip"); }
            }
            public Vector3 RightHip
            {
                get { return ReadFieldProxy<Vector3>("RightHip"); }
            }
            public Vector3 RightKnee
            {
                get { return ReadFieldProxy<Vector3>("RightKnee"); }
            }
            public Vector3 RightFoot
            {
                get { return ReadFieldProxy<Vector3>("RightFoot"); }
            }
            public Vector3 RightToe
            {
                get { return ReadFieldProxy<Vector3>("RightToe"); }
            }
            public Vector3 Weapon1
            {
                get { return ReadFieldProxy<Vector3>("Weapon1"); }
            }
            public Vector3 Weapon2
            {
                get { return ReadFieldProxy<Vector3>("Weapon2"); }
            }
            #endregion

            #region CONSTRUCTORS
            public Skeleton(int address) : base(address)
            {
                this.AddBone("Head", 11);
                this.AddBone("Neck", 10);
                this.AddBone("Spine1", 1);
                this.AddBone("Spine2", 2);
                this.AddBone("Spine3", 3);
                this.AddBone("Spine4", 4);
                this.AddBone("Spine5", 5);
                this.AddBone("LeftHand", 21);
                this.AddBone("LeftElbow", 31);
                this.AddBone("LeftShoulder", 36);
                this.AddBone("RightShoulder", 37);
                this.AddBone("RightElbow", 38);
                this.AddBone("RightHand", 15);
                this.AddBone("LeftToe", 38);
                this.AddBone("LeftFoot", 28);
                this.AddBone("LeftKnee", 27);
                this.AddBone("LeftHip", 26);
                this.AddBone("RightHip", 23);
                this.AddBone("RightKnee", 24);
                this.AddBone("RightFoot", 25);
                this.AddBone("RightToe", 37);
                this.AddBone("Weapon1", 16);
                this.AddBone("Weapon2", 21);
            }
            #endregion

            #region METHODS
            protected void AddBone(string name, int index)
            {
                this.Fields[name] = new BonesField(index);
            }
            #endregion
        }
        #endregion
    }
}
