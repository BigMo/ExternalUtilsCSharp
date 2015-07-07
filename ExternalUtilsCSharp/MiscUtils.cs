using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp
{
    public class MiscUtils
    {
        #region CONSTANTS
        private const string FORMAT = "{0} {1}";
        private static string[] NAMES_NUMBERS = new string[] { "", "Thousand", "Million", "Billion", "Trillion", "Quadrillion", "Quintillion", "Sextillion", "Septillion", "Octillion", "Nonillion" };
        private static string[] NAMES_NUMBERS_ABBREVIATIONS = new string[] { "", "K", "M", "B", "T" };
        private static string[] NAMES_SIZES = new string[] { "Byte", "Kilobyte", "Megabyte", "Gigabyte", "Terabyte", "Petabyte", "Exabyte", "Zettabyte", "Yottabyte" };
        private static string[] NAMES_SIZESABBREVIATIONS = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
        private const long DIVIDER_NUMBERS = 1000;
        private const long DIVIDER_SIZES = 1024;
        #endregion

        #region METHODS
        /// <summary>
        /// Determines which unit can be assigned to a number based on the divider.
        /// Returns a string containing the value and the unit using the given format
        /// </summary>
        /// <param name="value">Value</param>
        /// <param name="divider">Divider of the units</param>
        /// <param name="units">Array of the names of units</param>
        /// <param name="format">Format to use, where {0} is the calculated value and {1] is the unit</param>
        /// <returns></returns>
        public static string GetUnit(long value, long divider, string[] units, string format = FORMAT)
        {
            int idx = 0;
            while (idx < units.Length && value > divider)
            {
                idx++;
                value /= divider;
            }
            return string.Format(format, value, units[idx]);
        }
        /// <summary>
        /// Shortens down the given value and appends the correct unit to it
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useAbbreviation">Whether to use abbreviations of the unit</param>
        /// <param name="format">Format to use, where {0} is the calculated value and {1] is the unit</param>
        /// <returns></returns>
        public static string GetUnitFromNumber(long value, bool useAbbreviation = false, string format = FORMAT)
        {
            return GetUnit(value, DIVIDER_NUMBERS, useAbbreviation ? NAMES_NUMBERS_ABBREVIATIONS : NAMES_NUMBERS, format);
        }
        /// <summary>
        /// Shortens down the given value and appends the correct unit to it, representing a size
        /// </summary>
        /// <param name="value"></param>
        /// <param name="useAbbreviation">Whether to use abbreviations of the unit</param>
        /// <param name="format">Format to use, where {0} is the calculated value and {1] is the unit</param>
        /// <returns></returns>
        public static string GetUnitFromSize(long value, bool useAbbreviation = false, string format = FORMAT)
        {
            return GetUnit(value, DIVIDER_SIZES, useAbbreviation ? NAMES_SIZESABBREVIATIONS : NAMES_SIZES, format);
        }
        /// <summary>
        /// Merges the given arrays to one single array (preserves the order of all elements)
        /// Improved version written by aev1tas, about 4x faster(https://www.unknowncheats.me/forum/1264082-post81.html)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arrays">Arrays to merge</param>
        /// <returns>A single array containing all elements of the given arrays</returns>
        public static T[] MergeArrays<T>(params T[][] arrays)
        {
            int totalLength = 0;
            for (int i = 0; i < arrays.GetLength(0); i++)
                totalLength += arrays[i].Length;

            T[] result = new T[totalLength];
            int idx = 0;
            foreach (T[] array in arrays)
            {
                Array.Copy(array, 0, result, idx, array.Length);
                idx += array.Length;
            }

            return result;
        }
        #endregion
    }
}
