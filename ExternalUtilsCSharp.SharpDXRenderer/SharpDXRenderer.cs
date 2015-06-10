using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FontFactory = SharpDX.DirectWrite.Factory;
using Factory = SharpDX.Direct2D1.Factory;
using System.Collections;
using SharpDX.DXGI;

namespace ExternalUtilsCSharp.SharpDXRenderer
{
    /// <summary>
    /// An implementation of the abstract Renderer-class utilizing SharpDX
    /// </summary>
    public class SharpDXRenderer : ExternalUtilsCSharp.UI.Renderer<Color, Vector2, TextFormat>
    {
        #region VARIABLES
        private HwndRenderTargetProperties renderTargetProperties;
        private WindowRenderTarget device;
        private FontFactory fontFactory;
        private Factory factory;
        private Hashtable fonts;
        #endregion
        #region DESTRUCTOR
        ~SharpDXRenderer()
        {
            this.Dispose();
        }
        #endregion
        #region METHODS
        /// <summary>
        /// Creates a new font
        /// </summary>
        /// <param name="fontName">User-defined name of this font</param>
        /// <param name="fontFamilyName">Name of the used font-family</param>
        /// <param name="fontSize">Size of the font</param>
        /// <returns>New font</returns>
        public TextFormat CreateFont(string fontName, string fontFamilyName, float fontSize)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            TextFormat font = new TextFormat(fontFactory, fontFamilyName, fontSize);
            fonts.Add(fontName, font);
            return font;
        }
        /// <summary>
        /// Returns a formerly created font by name
        /// </summary>
        /// <param name="fontName">Name of the font</param>
        /// <returns></returns>
        public TextFormat GetFont(string fontName)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            return (TextFormat)fonts[fontName];
        }
        public override void InitializeDevice(IntPtr hWnd, Vector2 size)
        {
            factory = new Factory();
            fontFactory = new FontFactory();
            fonts = new Hashtable();
            renderTargetProperties = new HwndRenderTargetProperties()
            {
                Hwnd = hWnd,
                PixelSize = new Size2((int)size.X, (int)size.Y),
                PresentOptions = PresentOptions.None
            };
            device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied)), renderTargetProperties);
            device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;
        }
        public override void DestroyDevice()
        {
            foreach (string key in fonts.Keys)
                GetFont(key).Dispose();
            fonts.Clear();

            factory.Dispose();
            fontFactory.Dispose();
            device.Dispose();
            this.device = null;
        }
        #endregion
        #region HELPER-METHODS
        private PathGeometry CreatePathGeometry(params Vector2[] points)
        {
            PathGeometry gmtry = new PathGeometry(factory);

            GeometrySink sink = gmtry.Open();
            sink.SetFillMode(FillMode.Winding);
            sink.BeginFigure(points[0], FigureBegin.Filled);
            sink.AddLines(points);
            sink.EndFigure(FigureEnd.Closed);
            sink.Close();

            return gmtry;
        }
        #endregion
        #region IMPLEMENTED METHODS
        public override void DrawLine(SharpDX.Color color, Vector2 from, Vector2 to, float strokeWidth = 1f)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                device.DrawLine(from, to, brush, strokeWidth);
            }
        }

        public override void DrawText(string text, SharpDX.Color color, TextFormat font, Vector2 position)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                Vector2 textSize = MeasureString(text, font);
                device.DrawText(text, font, new RectangleF(position.X,position.Y,textSize.X,textSize.Y), brush);
            }
        }

        public override void DrawRectangle(SharpDX.Color color, Vector2 position, Vector2 size, float strokeWidth = 1f)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                device.DrawRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), brush, strokeWidth);
            }
        }

        public override void FillRectangle(SharpDX.Color color, Vector2 position, Vector2 size)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                device.FillRectangle(new RectangleF(position.X, position.Y, size.X, size.Y), brush);
            }
        }

        public override void DrawEllipse(SharpDX.Color color, Vector2 position, Vector2 size, bool centered = false, float strokeWidth = 1f)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                device.DrawEllipse(
                    new Ellipse(
                       (centered ? position : new Vector2(position.X - size.X / 2f, position.Y - size.Y / 2f)),
                        size.X / 2f,
                        size.Y / 2f
                    ),
                    brush,
                    strokeWidth
                );
            }
        }

        public override void FillEllipse(SharpDX.Color color, Vector2 position, Vector2 size, bool centered = false)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (SolidColorBrush brush = new SolidColorBrush(device, color))
            {
                device.FillEllipse(
                    new Ellipse(
                       (centered ? position : new Vector2(position.X - size.X / 2f, position.Y - size.Y / 2f)),
                        size.X / 2f,
                        size.Y / 2f
                    ),
                    brush
                );
            }
        }

        public override void FillPolygon(SharpDX.Color color, params Vector2[] points)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            using (PathGeometry gmtry = CreatePathGeometry(points))
            {
                using (SolidColorBrush brush = new SolidColorBrush(device, color))
                {
                    device.FillGeometry(gmtry, brush);
                }
            }
        }

        public override Vector2 MeasureString(string text, TextFormat font)
        {
            Vector2 size;
            using (TextLayout layout = new TextLayout(fontFactory, text, font, float.MaxValue, float.MaxValue))
            {
                size = new Vector2(layout.Metrics.Width, layout.Metrics.Height);
            }
            return size;
        }
        public override void Clear(Color color)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            device.Clear(color);
        }
        public override void BeginDraw()
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            device.BeginDraw();
        }

        public override void EndDraw()
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            device.EndDraw();
        }

        public override void Resize(Vector2 size)
        {
            if (device == null)
                throw new SharpDXException("The device was not initialized yet");
            device.Resize(new Size2((int)size.X, (int)size.Y));
        }
        #endregion

        public override void Dispose()
        {
            if (this.device != null)
                this.DestroyDevice();
        }

        public override Color GetRendererBackColor()
        {
            return Color.Transparent;
        }
    }
}
