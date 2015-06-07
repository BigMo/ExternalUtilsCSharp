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
    #region not static JsonConfigs

    /// <summary>
    /// Json configs helper which Deserialize Objects from file
    /// </summary>
    /// <typeparam name="T">T is settings Object</typeparam>
    public class JsonConfigHelper<T> : ConfigUtils where T : new()
    {
        public T Settings = new T();
        private string lastConfigName;
        public JsonConfigHelper() { }
        /// <summary>
        /// Created default config to specified path
        /// </summary>
        public virtual void DefaultConfig(string configName)
        {
            if (File.Exists(configName)) return;
            JSONConfigManager.WriteFile(Settings, configName);
        }
        /// <summary>
        ///  Use ReadSettingsFromFile(string) method THIS one is not implemented
        ///  since byte[] data is not required
        /// </summary>
        public override void ReadSettings(byte[] data)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Saves Setings object to latest used configuration
        /// </summary>
        /// <returns>return is not required</returns>
        public override byte[] SaveSettings()
        {
            JSONConfigManager.WriteFile(Settings, lastConfigName);
            return null;
        }
        /// <summary>
        /// Reads settings from file and stores it into Settings Object
        /// </summary>
        /// <param name="configName">Path of file</param>
        public override void ReadSettingsFromFile(string configName) 
        {
            lastConfigName = configName;
            Settings = JSONConfigManager.ReadFile<T>(configName);
        }
        /// <summary>
        /// Saves Settings object to specified file
        /// </summary>
        /// <param name="configFileName">Path of file</param>
        public override void SaveSettingsToFile(string configFileName)
        {
            lastConfigName = configFileName;
            JSONConfigManager.WriteFile(Settings, configFileName);
        }

    }
    #endregion

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
                throw new Exception("Could not read settings\n",e);
                //data = null;
                //return new T();
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
