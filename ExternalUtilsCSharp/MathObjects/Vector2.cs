using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.MathObjects
{
    /// <summary>
    /// Class that holds information about a 2d-coordinate and offers some basic operations
    /// </summary>
    public struct Vector2
    {
        #region VARIABLES
        private float[] data;
        #endregion

        #region PROPERTIES
        public float X
        {
            get { return data[0]; }
            set { data[0] = value; }
        }
        public float Y
        {
            get { return data[1]; }
            set { data[1] = value; }
        }
        /// <summary>
        /// Returns a new Vector2 at (0,0)
        /// </summary>
        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new Vector2 using the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Vector2(float x, float y)
        {
            data = new float[] { x, y };
        }
        /// <summary>
        /// Initializes a new Vector2 by copying the values of the given Vector2
        /// </summary>
        /// <param name="vec"></param>
        public Vector2(Vector2 vec) : this(vec.X, vec.Y) { }
        #endregion

        #region METHODS
        /// <summary>
        /// Returns the length of this Vector2
        /// </summary>
        /// <returns></returns>
        public float Length()
        {
            return (float)System.Math.Sqrt(System.Math.Pow(X, 2) + System.Math.Pow(Y, 2));
        }
        /// <summary>
        /// Returns the distance from this Vector2 to the given Vector2
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public float DistanceTo(Vector2 other)
        {
            return (this + other).Length();
        }
        #endregion

        #region OPERATORS
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        }
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        }
        public float this[int i]
        {
            get { return data[i]; }
            set { data[i] = value; }
        }
        #endregion
    }
}
