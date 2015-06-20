using ExternalUtilsCSharp.MathObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot.CSGOClasses.Fields
{
    public class BonesField : Field<Vector3>
    {
        public BonesField(int index) : base(index) 
        { }

        public override void ReadValue(int baseAddress)
        {
            float x, y, z;
            x = WithOverlay.MemUtils.Read<float>((IntPtr)(baseAddress + this.Offset * 0x30 + 0x0C));
            y = WithOverlay.MemUtils.Read<float>((IntPtr)(baseAddress + this.Offset * 0x30 + 0x1C));
            z = WithOverlay.MemUtils.Read<float>((IntPtr)(baseAddress + this.Offset * 0x30 + 0x2C));
            this.Value = new Vector3(x, y, z);
            this.ValueRead = true;
        }
    }
}
