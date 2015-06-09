using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.UI.UIObjects
{
    public struct Color
    {
        #region PROPERTIES
        public byte A;
        public byte R;
        public byte G;
        public byte B;
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a color-object using the given values for its channels
        /// </summary>
        /// <param name="a">Value of the alpha-channel (0-255)</param>
        /// <param name="r">Value of the red-channel (0-255)</param>
        /// <param name="g">Value of the green-channel (0-255)</param>
        /// <param name="b">Value of the blue-channel (0-255)</param>
        public Color(byte a, byte r, byte g, byte b)
        {
            A = a;
            R = r;
            G = g;
            B = b;
        }
        /// <summary>
        /// Initializes a color-object using the given values for its channels
        /// </summary>
        /// <param name="a">Value of the alpha-channel (0-1)</param>
        /// <param name="r">Value of the red-channel (0-1)</param>
        /// <param name="g">Value of the green-channel (0-1)</param>
        /// <param name="b">Value of the blue-channel (0-1)</param>
        public Color(float a, float r, float g, float b) : this((byte)(255f * a), (byte)(255f * r), (byte)(255f * g), (byte)(255f * b))
        { }
        #endregion

        #region METHODS
        public int ToARGB()
        {
            return (int)B + ((int)G << 8) + ((int)R << 16) + ((int)A << 24);
        }
        public int ToRGBA()
        {
            return (int)A + ((int)B << 8) + ((int)G << 16) + ((int)R << 24);
        }
        #endregion
    }
}
