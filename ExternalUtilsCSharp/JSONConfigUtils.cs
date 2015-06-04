using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp
{
    /// <summary>
    /// JSON configurations helper classes
    /// Basicaly lets you to write any kind of object to file
    /// </summary>
  
    #region JsonConfigs
    public static class JSONConfigHelper
    {
        /// <summary>
        /// Creates new configuration if it does not exists
        /// </summary>
        /// <param name="configName">path or name of config</param>
        /// <param name="objToWrite">Object to write</param>
        public static void DefaultConfig<T>(string configName, T objToWrite)
        {
            if (File.Exists(configName)) return;
            JSONConfigManager.WriteFile(objToWrite, configName);
        }
        /// <summary>
        /// Read configuration
        /// </summary>
        public static T ReadConfiguration<T>(string configName) where T : new()
        {
            return JSONConfigManager.ReadFile<T>(configName);
        }
        /// <summary>
        /// Rewrites configuration
        /// </summary>
        public static void RewriteConfiguration<T>( string configFileName,T configs)
        {
            JSONConfigManager.WriteFile(configs, configFileName);
        }

    }
    public static class JSONConfigManager
    {
        public static T ReadFile<T>(string filename) where T : new()
        {
            string data;
            try
            {
                using (
                    var sr = new StreamReader(File.Open(filename,
                            FileMode.Open, FileAccess.Read)))
                {
                    data = sr.ReadToEnd();
                }
                var temp = JsonConvert.DeserializeObject<T>(data);
                return temp;
            }
            catch (Exception e)
            {
                data = null;
                return new T();
            }
        }
        public static void WriteFile<T>(T objToWrite, string fileName)
        {
            var temp = JsonConvert.SerializeObject(objToWrite, Formatting.Indented);
            using (var file = File.Open(fileName, FileMode.Create, FileAccess.Write))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.Write(temp);
                }
            }
        }
    }
    #endregion
}
