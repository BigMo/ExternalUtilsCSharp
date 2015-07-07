using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.ConfigObjects
{
    public class IniConfigUtils : ConfigUtils
    {
        #region CLASSES
        public class ReadingSettingEventArgs : EventArgs
        {
            public string Name { get; private set; }
            public string Value { get; private set; }

            public ReadingSettingEventArgs(string name, string value) : base()
            {
                this.Name = name;
                this.Value = value;
            }
        }
        #endregion

        #region EVENTS
        public event EventHandler<ReadingSettingEventArgs> ReadingSettingEvent;
        protected virtual void OnReadingSettingEvent(ReadingSettingEventArgs e)
        {
            if (ReadingSettingEvent != null)
                ReadingSettingEvent(this, e);
        }
        #endregion

        #region PROPERTIES
        public Encoding Encoding { get; set; }
        #endregion

        #region CONSTRUCTOR
        public IniConfigUtils() : base()
        {
            this.Encoding = Encoding.ASCII;
        }
        #endregion

        #region METHODS
        public override void ReadSettings(byte[] data)
        {
            string text = this.Encoding.GetString(data);

            //Split text into lines
            string[] lines = text.Contains("\r\n") ? text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries) : text.Split("\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                //Trim current line
                string tmpLine = line.Trim();
                //Skip invalid ones
                if (tmpLine.StartsWith("#")) // comment
                    continue;
                else if (!tmpLine.Contains("=")) // it's no key-value pair!
                    continue;

                //Trim both parts of the key-value pair
                string[] parts = tmpLine.Split('=');
                parts[0] = parts[0].Trim();
                parts[1] = parts[1].Trim();
                if (string.IsNullOrEmpty(parts[0]) || string.IsNullOrEmpty(parts[1]))
                    continue;
                if (parts[1].Contains('#')) //If value-part contains comment, split it
                    parts[1] = parts[1].Split('#')[0];
                OnReadingSettingEvent(new ReadingSettingEventArgs(parts[0], parts[1]));
            }
        }

        public override byte[] SaveSettings()
        {
            StringBuilder builder = new StringBuilder();
            foreach (string key in this.GetKeys())
            {
                builder.AppendFormat("{0} = {1}\n", key, this.GetValue(key));
            }
            return this.Encoding.GetBytes(builder.ToString());
        }
        #endregion
    }
}
