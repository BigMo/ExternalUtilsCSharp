using ExternalUtilsCSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClickerHeroes
{
    class CHConfig : ConfigUtils
    {
        public CHConfig()
        {
            this.SetValue("windowWidth", 0);
            this.SetValue("windowHeight", 0);
            this.SetValue("windowX", 0);
            this.SetValue("windowY", 0);
            this.SetValue("offsetX", 0);
            this.SetValue("offsetY", 0);
        }
        public override void ReadSettings(byte[] data)
        {
            string text = Encoding.ASCII.GetString(data);
            string[] lines;
            if (text.Contains("\r\n"))
                lines = text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            else
                lines = text.Split(new char[]{'\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach(string line in lines)
            {
                if (!line.Contains('='))
                    continue;
                if (line.TrimStart().StartsWith("#"))
                    continue;
                string[] parts = line.Trim().Split('=');
                parts[0] = parts[0].Trim();
                parts[1] = parts[1].Trim();
                this.SetValue(parts[0], Convert.ToInt32(parts[1]));
            }
        }

        public override byte[] SaveSettings()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(
                "#\t __________________________________________________________________________________________\n" +
                "#\t/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/\n" +
                "#\t_________ .__  .__        __                   ___ ___                                     \n" +
                "#\t\\_   ___ \\|  | |__| ____ |  | __ ___________  /   |   \\   ___________  ____   ____   ______\n" +
                "#\t/    \\  \\/|  | |  |/ ___\\|  |/ // __ \\_  __ \\/    ~    \\_/ __ \\_  __ \\/  _ \\_/ __ \\ /  ___/\n" +
                "#\t\\     \\___|  |_|  \\  \\___|    <\\  ___/|  | \\/\\    Y    /\\  ___/|  | \\(  <_> )  ___/ \\___ \\ \n" +
                "#\t \\______  /____/__|\\___  >__|_ \\\\___  >__|    \\___|_  /  \\___  >__|   \\____/ \\___  >____  >\n" +
                "#\t        \\/             \\/     \\/    \\/ puddin tells \\/       \\/                  \\/     \\/ \n" +
                "#\t   _____          __         _________ .__  .__  lies  __                                  \n" +
                "#\t  /  _  \\  __ ___/  |_  ____ \\_   ___ \\|  | |__| ____ |  | __ ___________                  \n" +
                "#\t /  /_\\  \\|  |  \\   __\\/  _ \\/    \\  \\/|  | |  |/ ___\\|  |/ // __ \\_  __ \\     by Zat      \n" +
                "#\t/    |    \\  |  /|  | (  <_> )     \\___|  |_|  \\  \\___|    <\\  ___/|  | \\/                 \n" +
                "#\t\\____|__  /____/ |__|  \\____/ \\______  /____/__|\\___  >__|_ \\\\___  >__|       part of      \n" +
                "#\t        \\/                           \\/             \\/     \\/    \\/    ExternalUtilsCSharp \n" +
                "#\t __________________________________________________________________________________________\n" +
                "#\t/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/_____/");
                
            foreach(string key in this.GetKeys())
            {
                builder.AppendFormat("{0} = {1}\n", key, this.GetValue(key));
            }
            return Encoding.ASCII.GetBytes(builder.ToString());
        }
    }
}
