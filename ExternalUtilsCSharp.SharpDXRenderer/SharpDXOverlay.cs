using SharpDX;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer
{
    /// <summary>
    /// An implementation of the abstract Overlay-class utilizing SharpDX
    /// </summary>
    public class SharpDXOverlay : ExternalUtilsCSharp.UI.Overlay<Color, Vector2, TextFormat>, IDisposable
    {
        #region CONSTRUCTORS
        public SharpDXOverlay() : base()
        {
            this.Renderer = new SharpDXRenderer();
        }
        #endregion

        public override void Attach(IntPtr hWnd)
        {
            WinAPI.WINDOWINFO info = new WinAPI.WINDOWINFO();
            if (!WinAPI.GetWindowInfo(hWnd, ref info))
                throw new Win32Exception(Marshal.GetLastWin32Error());

            this.hWnd = hWnd;
            this.Renderer.InitializeDevice(this.Handle, new Vector2(info.rcClient.Right - info.rcClient.Left, info.rcClient.Bottom - info.rcClient.Top));
            base.Attach(hWnd);
        }

        public override void Detach()
        {
            base.Detach();
            this.Renderer.DestroyDevice();
        }

        protected override void Dispose(bool disposing)
        {
            if (!disposing)
            {
                this.Detach();
                base.Dispose(disposing);
            }
        }

        public override void OnResize()
        {
            this.Renderer.Resize(new Vector2(this.Width, this.Height));
        }
    }
}
