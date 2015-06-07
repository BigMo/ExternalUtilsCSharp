using System;
using System.Collections.Generic;
using System.Text;

namespace ExternalUtilsCSharp.UI.UIObjects
{
    /// <summary>
    /// A structure that holds information about location and size of a rectangle
    /// </summary>
    public struct Rectangle
    {
        #region PROPERTIES
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Top { get { return this.Y; } }
        public float Bottom { get { return this.Y + this.Height; } }
        public float Left { get { return this.X; } }
        public float Right { get { return this.X + this.Width; } }
        public static Rectangle Empty { get { return new Rectangle(0f, 0f, 0f, 0f); } }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// Initializes a new rectangle using the given values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
        /// <summary>
        /// Initializes a new rectangle by copying the values of the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        public Rectangle(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }
        #endregion

        #region METHODS
        /// <summary>
        /// Returns whether this rectangle intersects with the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public bool IntersectsWith(Rectangle rect)
        {
            return rect.X < this.X + this.Width && this.X < rect.X + rect.Width && rect.Y < this.Y + this.Height && this.Y < rect.Y + rect.Height;
        }
        /// <summary>
        /// Returns the intersection of this rectangle and the given rectangle
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public Rectangle Intersect(Rectangle rect)
        {
            float x = Math.Max(this.X, rect.X);
            float width = Math.Min(this.X + this.Width, rect.X + rect.Width);
            float y = Math.Max(this.Y, rect.Y);
            float height = Math.Min(this.Y + this.Height, rect.Y + rect.Height);
            if (width >= x && height >= y)
            {
                return new Rectangle(x, y, y - x, height - y);
            }
            return Rectangle.Empty;
        }
        #endregion
    }
}
