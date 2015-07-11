using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace ExternalUtilsCSharp.SharpDXRenderer
{
    /// <summary>
    /// Extension for SharpDX.Color
    /// </summary>
    // TODO test if working. As i only used System.Drawing.Color before
    public static class ColorExtension
    {
        public static readonly List<Color> ColorList = ColorStructToList();
        public static List<Color> ColorStructToList()
        {
            return typeof(Color).GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (Color)c.GetValue(null))
                                .ToList();
        }
        /// <summary>
        /// Picks previous Color 
        /// </summary>
        /// <param name="clr">base color to start with</param>
        /// <returns>PreviousColor</returns>
        public static Color PreviousColor(this Color clr)
        {
            Color color;
            var clrID = ColorList.IndexOf(clr) - 1;
            color = clrID >= 0 ? ColorList[clrID] : ColorList.Last();
            return color;
        }
        /// <summary>
        /// Picks next color 
        /// </summary>
        /// <param name="clr">base color to start with</param>
        /// <returns>Next color</returns>
        public static Color NextColor(this Color clr)
        {
            Color color;
            var clrID = ColorList.IndexOf(clr) + 1;
            color = clrID < ColorList.Count && clrID != -1 ? ColorList[clrID] : ColorList.First();
            return color;
        }

        
        public static readonly Dictionary<string,Color> ColorDictionary = ColorStructToDictionary();
        public static Dictionary<string,Color> ColorStructToDictionary()
        {
            Dictionary<string,Color> dictionary = new Dictionary<string, Color>();
            foreach (var colorInfo in typeof(Color).GetFields(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)){
                dictionary.Add(colorInfo.Name,(Color)colorInfo.GetValue(null));
                
            }
            return dictionary;
        }
        /// <summary>
        /// Gets Color name from color dictionary
        /// </summary>
        /// <param name="color">Color object</param>
        /// <returns>Color name</returns>
        public static string Name(this Color color){
            string name = "";
            var pair = ColorDictionary.First(x => x.Value.Equals(color));
            name = pair.Key;
            return name;
        }
        /// <summary>
        /// Converts string to color
        /// </summary>
        /// <param name="name">Color name</param>
        /// <returns>Color object</returns>
        public static Color StringToColor(string name)
        {
            Color color = Color.Zero;
            ColorDictionary.TryGetValue(name,out color);
            return color;
        }
        public static System.Drawing.Color ToSystemDrawingColor(this Color color){
            return System.Drawing.Color.FromArgb(color.A,color.R,color.G,color.B);;
        }        
        public static Color ToSharpDxColor(this System.Drawing.Color color){
            return Color.FromRgba(UI.UIObjects.ColorExtension.ToRGBA(color));;
        }

    }

}
