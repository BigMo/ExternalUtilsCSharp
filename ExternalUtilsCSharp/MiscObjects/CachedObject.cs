using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.MiscObjects
{
    /// <summary>
    /// An abstract class meant for managing reading and holding data of an object in memory
    /// Utilizes a hashtable to manage reading data and cache data
    /// </summary>
    public abstract class CachedObject
    {

        #region PROPERTIES
        public Hashtable Fields { get; private set; }
        public int Address { get; protected set; }
        public MemUtils MemUtils { get; set; }
        #endregion

        #region CONSTRUCTORS
        public CachedObject(int address)
        {
            this.Address = address;
            this.Fields = new Hashtable();
            this.SetupFields();
        }
        public CachedObject()
            : this(0)
        { }
        #endregion

        #region METHODS
        public override string ToString()
        {
            return string.Format("[Entity Address={0}]", this.Address.ToString("X"));
        }
        #endregion

        #region HELPERS
        protected void AddField<T>(string fieldName, int offset, T value = default(T)) where T : struct
        {
            Fields[fieldName] = new CachedField<T>(offset, value);
        }
        /// <summary>
        /// Returns the value of the given field if the field has read its value before
        /// Makes the field read its value if it did not do so before
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected T ReadFieldProxy<T>(string fieldName) where T : struct
        {
            CachedField<T> field = (CachedField<T>)Fields[fieldName];
            if (!field.ValueRead)
                field.ReadValue(this.Address, this.MemUtils);
            return field.Value;
        }
        /// <summary>
        /// Copies the fields of one Entity to another one;
        /// Used for copy-constructors
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="other"></param>
        protected void CopyFieldsFrom<T>(T other) where T : CachedObject
        {
            foreach (string key in other.Fields.Keys)
                this.Fields[key] = other.Fields[key];
        }

        protected virtual void SetupFields()
        { }
        #endregion
    }
}
