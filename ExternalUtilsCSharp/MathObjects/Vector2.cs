using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.MathObjects
{
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
        public static Vector2 Zero
        {
            get { return new Vector2(0, 0); }
        }
        #endregion

        #region CONSTRUCTOR
        public Vector2(float x, float y)
        {
            data = new float[] { x, y };
        }
        public Vector2(Vector2 vec) : this(vec.X, vec.Y) { }
        #endregion

        #region METHODS
        public float Length()
        {
            return (float)System.Math.Sqrt(System.Math.Pow(X, 2) + System.Math.Pow(Y, 2));
        }
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
