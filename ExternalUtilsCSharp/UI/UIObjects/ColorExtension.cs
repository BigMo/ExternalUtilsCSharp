using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.UI.UIObjects
{
    /// <summary>
    /// Extension for System.Drawing.Color
    /// </summary>
    public static class ColorExtension
    {
        public static List<System.Drawing.Color> colorList = ColorStructToList();
        public static List<System.Drawing.Color> ColorStructToList()
        {
            return typeof(System.Drawing.Color).GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public)
                                .Select(c => (System.Drawing.Color)c.GetValue(null, null))
                                .ToList();
        }
        /// <summary>
        /// Picks previous Color 
        /// </summary>
        /// <param name="clr">base color to start with</param>
        /// <returns>PreviousColor</returns>
        public static System.Drawing.Color PreviousColor(this System.Drawing.Color clr)
        {
            System.Drawing.Color color;
            var clrID = colorList.IndexOf(clr) - 1;
            color = clrID >= 0 ? colorList[clrID] : colorList.Last();
            return color;
        }
        /// <summary>
        /// Picks next color 
        /// </summary>
        /// <param name="clr">base color to start with</param>
        /// <returns>Next color</returns>
        public static System.Drawing.Color NextColor(this System.Drawing.Color clr)
        {
            System.Drawing.Color color;
            var clrID = colorList.IndexOf(clr) + 1;
            color = clrID < colorList.Count && clrID != -1 ? colorList[clrID] : colorList.First();
            return color;
        }
        public static int ToRGBA(this System.Drawing.Color clr)
        {
            return (int)clr.R | ((int)clr.G << 8) | ((int)clr.B << 16) | ((int)clr.A << 24);
        }
    }
}
