using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.MiscObjects
{
    public class CachedField<T> where T : struct
    {
        public bool ValueRead { get; protected set; }
        public int Offset { get; protected set; }
        public T Value { get; protected set; }

        public CachedField()
            : this(0)
        { }
        public CachedField(int offset)
            : this(offset, default(T))
        { }
        public CachedField(int offset, T value)
        {
            this.Offset = offset;
            this.Value = value;
            this.ValueRead = false;
        }

        public virtual void ReadValue(int baseAddress, MemUtils memUtils)
        {
            this.Value = memUtils.Read<T>((IntPtr)(baseAddress + this.Offset));
            this.ValueRead = true;
        }
    }
}
