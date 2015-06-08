using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.UI
{
    /// <summary>
    /// An abstract class for rendering
    /// </summary>
    /// <typeparam name="TColor">Color-type</typeparam>
    /// <typeparam name="TVector2">Vector2-/Point-type</typeparam>
    /// <typeparam name="TFont">Font-type</typeparam>
    public abstract class Renderer<TColor, TVector2, TFont> : IDisposable
    {
        #region METHODS
        /// <summary>
        /// Initialization of the device
        /// </summary>
        /// <param name="hWnd">A handle to a window this renderer shall draw on</param>
        /// <param name="size">Size of the window/area to draw on</param>
        public abstract void InitializeDevice(IntPtr hWnd, TVector2 size);
        /// <summary>
        /// Destroys the device
        /// </summary>
        public abstract void DestroyDevice();
        public abstract void Dispose();
        #endregion
        #region DRAW-METHODS
        /// <summary>
        /// Draws a line from one point to another
        /// </summary>
        /// <param name="color">Color of the line</param>
        /// <param name="from">Starting point</param>
        /// <param name="to">Ending point</param>
        /// <param name="strokeWidth">Width of the line</param>
        public abstract void DrawLine(TColor color, TVector2 from, TVector2 to, float strokeWidth = 1f);
        /// <summary>
        /// Draws lines between the given points (in the given order)
        /// </summary>
        /// <param name="color">Color of the lines</param>
        /// <param name="points">Points to draw a line between</param>
        public virtual void DrawLines(TColor color, params TVector2[] points)
        {
            this.DrawLines(color, 1f, points);
        }
        /// <summary>
        /// Draws lines between the given points (in the given order)
        /// </summary>
        /// <param name="color">Color of the lines</param>
        /// <param name="strokeWidth">Width of the lines</param>
        /// <param name="points">Points to draw a line between</param>
        public virtual void DrawLines(TColor color, float strokeWidth, params TVector2[] points)
        {
            if (points.Length < 2)
                throw new ArgumentException("There must be at least two points to connect", "points");
            for (int i = 0; i < points.Length - 1; i++)
                DrawLine(color, points[i], points[i + 1], strokeWidth);
        }
        /// <summary>
        /// Draws a text using the given font in the given color at the given position
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Color of the text</param>
        /// <param name="font">Font of the text</param>
        /// <param name="position">Position of the text</param>
        public abstract void DrawText(string text, TColor color, TFont font, TVector2 position);
        /// <summary>
        /// Draws a text using the given font in the given color at the given position and applies a shadow
        /// </summary>
        /// <param name="text">Text to draw</param>
        /// <param name="color">Color of the text</param>
        /// <param name="shadowColor">Color of the text-shadow</param>
        /// <param name="font">Font of the text</param>
        /// <param name="position">Position of the text</param>
        /// <param name="shadowPosition">Position of the text-shadow</param>
        public virtual void DrawText(string text, TColor color, TColor shadowColor, TFont font, TVector2 position, TVector2 shadowPosition)
        {
            this.DrawText(text, shadowColor, font, shadowPosition);
            this.DrawText(text, color, font, position);
        }
        /// <summary>
        /// Draws a rectangle
        /// </summary>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="position">Position of the rectangle</param>
        /// <param name="size">Size of the rectangle</param>
        /// <param name="strokeWidth">Strokewidth of the rectangle</param>
        public abstract void DrawRectangle(TColor color, TVector2 position, TVector2 size, float strokeWidth = 1f);
        /// <summary>
        /// Fills a rectangle
        /// </summary>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="position">Position of the rectangle</param>
        /// <param name="size">Size of the rectangle</param>
        public abstract void FillRectangle(TColor color, TVector2 position, TVector2 size);
        /// <summary>
        /// Draws a Ellipse
        /// </summary>
        /// <param name="color">Color of the Ellipse</param>
        /// <param name="position">Position of the Ellipse</param>
        /// <param name="size">Size of the Ellipse</param>
        /// <param name="centered">Determines whether the position is the upperleft corner or the center of this ellipse</param>
        /// <param name="strokeWidth">Strokewidth of the Ellipse</param>
        public abstract void DrawEllipse(TColor color, TVector2 position, TVector2 size, bool centered = false, float strokeWidth = 1f);
        /// <summary>
        /// Fills a Ellipse
        /// </summary>
        /// <param name="color">Color of the Ellipse</param>
        /// <param name="position">Position of the Ellipse</param>
        /// <param name="centered">Determines whether the position is the upperleft corner or the center of this ellipse</param>
        /// <param name="size">Size of the Ellipse</param>
        public abstract void FillEllipse(TColor color, TVector2 position, TVector2 size, bool centered = false);
        /// <summary>
        /// Draws a polygon
        /// </summary>
        /// <param name="color">Color of the polygon</param>
        /// <param name="strokeWidth">Strokewidth of the polygon</param>
        /// <param name="points">Edges of the polygon</param>
        public virtual void DrawPolygon(TColor color, float strokeWidth, params TVector2[] points)
        {
            if (points.Length < 3)
                throw new ArgumentException("A polygon must at least have three edges", "points");
            for (int i = 0; i < points.Length - 1; i++)
            {
                DrawLine(color, points[i], points[i + 1], strokeWidth);
            }
            DrawLine(color, points[points.Length - 1], points[0], strokeWidth);
        }
        /// <summary>
        /// Draws a polygon
        /// </summary>
        /// <param name="color">Color of the polygon</param>
        /// <param name="points">Edges of the polygon</param>
        public virtual void DrawPolygon(TColor color, params TVector2[] points)
        {
            this.DrawPolygon(color, 1f, points);
        }
        /// <summary>
        /// Fills a polygon
        /// </summary>
        /// <param name="color">Color of the polygon</param>
        /// <param name="points">Edges of the polygon</param>
        public abstract void FillPolygon(TColor color, params TVector2[] points);
        /// <summary>
        /// Measures the given string using the given font
        /// </summary>
        /// <param name="text">Text to measure</param>
        /// <param name="font">Font to use</param>
        /// <returns>Size of the string</returns>
        public abstract TVector2 MeasureString(string text, TFont font);
        /// <summary>
        /// Clears the screen using the given color
        /// </summary>
        /// <param name="color"></param>
        public abstract void Clear(TColor color);
        /// <summary>
        /// Is called before drawing
        /// </summary>
        public abstract void BeginDraw();
        /// <summary>
        /// Is called after drawing
        /// </summary>
        public abstract void EndDraw();
        /// <summary>
        /// Resizes the backbuffer of this renderer
        /// </summary>
        /// <param name="size"></param>
        public abstract void Resize(TVector2 size);
        /// <summary>
        /// Returns the renderer's backcolor
        /// </summary>
        /// <returns></returns>
        public abstract TColor GetRendererBackColor();
        #endregion
    }
}
