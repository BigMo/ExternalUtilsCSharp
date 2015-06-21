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
                case "espEnabled":
                case "espBox":
                case "espSkeleton":
                case "espName":
                case "espHealth":
                case "aimEnabled":
                case "aimToggle":
                case "aimHold":
                case "aimSmoothEnabled":
                case "aimFilterSpotted":
                case "aimFilterSpottedBy":
                case "aimFilterEnemies":
                case "aimFilterAllies":
                case "rcsEnabled":
                    this.SetValue(name, Convert.ToBoolean(value));
                    break;
                case "aimFov":
                case "aimSmoothValue":
                case "rcsForce":
                    this.SetValue(name, Convert.ToSingle(value));
                    break;
                case "aimKey":
                    this.SetValue(name, ParseEnum<WinAPI.VirtualKeyShort>(value));
                    break;
                default:
                    Console.WriteLine("Unknown settings-field \"{0}\" (value: \"{1}\")", name, value);
                    break;
            }
        }
        /*ConfigUtils.SetValue("espEnabled", true);
            ConfigUtils.SetValue("espBox", false);
            ConfigUtils.SetValue("espSkeleton", true);
            ConfigUtils.SetValue("espName", false);
            ConfigUtils.SetValue("espHealth", true);

            ConfigUtils.SetValue("aimEnabled", true);
            ConfigUtils.SetValue("aimKey", WinAPI.VirtualKeyShort.XBUTTON1);
            ConfigUtils.SetValue("aimToggle", false);
            ConfigUtils.SetValue("aimHold", true);
            ConfigUtils.SetValue("aimFov", 30f);
            ConfigUtils.SetValue("aimSmoothEnabled", true);
            ConfigUtils.SetValue("aimSmoothValue", 0.2f);
            ConfigUtils.SetValue("aimFilterSpotted", false);
            ConfigUtils.SetValue("aimFilterSpottedBy", false);
            ConfigUtils.SetValue("aimFilterEnemies", true);
            ConfigUtils.SetValue("aimFilterAllies", false);*/

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
