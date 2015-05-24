using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOTriggerbot
{
    class CSGOConfigUtils : ConfigUtils
    {
        public override void ReadSettings(byte[] data)
        {
            string text = Encoding.Unicode.GetString(data);

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
                InterpretSetting(parts[0], parts[1]);
            }
        }

        private void InterpretSetting(string name, string value)
        {
            switch (name)
            {
                case "triggerbotKey":
                    this.SetValue("triggerbotKey", ParseEnum<WinAPI.VirtualKeyShort>(value));
                    break;
                case "bunnyhopEnabled":
                    this.SetValue("bunnyhopEnabled", Convert.ToBoolean(value));
                    break;
                case "rcsEnabled":
                    this.SetValue("rcsEnabled", Convert.ToBoolean(value));
                    break;
                case "rcsFullCompensation":
                    this.SetValue("rcsFullCompensation", Convert.ToBoolean(value));
                    break;
                case "aimlockEnabled":
                    this.SetValue("aimlockEnabled", Convert.ToBoolean(value));
                    break;
                case "glowEnabled":
                    this.SetValue("glowEnabled", Convert.ToBoolean(value));
                    break;
                default:
                    Console.WriteLine("Unknown settings-field \"{0}\" (value: \"{1}\")", name, value);
                    break;
            }
        }

        public override byte[] SaveSettings()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"###################################################################################################################");
            builder.AppendLine(@"#    _________   _________ ________ ___________________      .__   *puddin tells lies*    ___.           __       #");
            builder.AppendLine(@"#    \_   ___ \ /   _____//  _____/ \_____  \__    ___/______|__| ____   ____   __________\_ |__   _____/  |_     #");
            builder.AppendLine(@"#    /    \  \/ \_____  \/   \  ___  /   |   \|    |  \_  __ \  |/ ___\ / ___\_/ __ \_  __ \ __ \ /  _ \   __\    #");
            builder.AppendLine(@"#    \     \____/        \    \_\  \/    |    \    |   |  | \/  / /_/  > /_/  >  ___/|  | \/ \_\ (  <_> )  |      #");
            builder.AppendLine(@"#     \______  /_______  /\______  /\_______  /____|   |__|  |__\___  /\___  / \___  >__|  |___  /\____/|__|      #");
            builder.AppendLine(@"# *u w0t m8* \/        \/        \/         \/  uc exclusive   /_____//_____/      \/          \/  (C) by Zat     #");
            builder.AppendLine(@"###################################################################################################################");
            builder.AppendLine(@"#                              [Sick ascii art from http://patorjk.com/software/taag/]                            #");
            builder.AppendLine(@"###################################################################################################################");
            foreach (string key in this.GetKeys())
            {
                builder.AppendFormat("{0} = {1}\n", key, this.GetValue(key));
            }
            return Encoding.Unicode.GetBytes(builder.ToString());
        }
    }
}
