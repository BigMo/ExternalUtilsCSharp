using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExternalUtilsCSharp.UI;
using ExternalUtilsCSharp;
using ExternalUtilsCSharp.MathObjects;
using System.ComponentModel;

namespace ExternalUtilsCSharp.InputUtils
{
    public class MouseHook
    {
        #region VARIABLES
        protected WinAPI.HookProc MouseHookProcedure;
        protected static IntPtr hMouseHook = IntPtr.Zero; // mouse hook handle
        #endregion

        #region CONSTANTS
        // The following is the definition of these two low-level hook Winuser.h: 
        private const int WH_MOUSE_LL = 14;
        #endregion

        #region PROPERTIES
        public bool MouseChanged { get; private set; }
        public MouseEventExtArgs CurrentMouseArgs { get; protected set; }
        #endregion

        #region EVENTS
        /// <summary>
        /// Event can be bound to track changes
        /// </summary>
        public event EventHandler<MouseEventExtArgs> MouseEvent;

        protected virtual void OnMouseChangedEvent(object sender, MouseEventExtArgs e)
        {
            if (MouseEvent != null)
                MouseEvent(sender, e);
        }
        #endregion

        #region CONSTRUCTOR
        public MouseHook()
        {
            CurrentMouseArgs = new MouseEventExtArgs();
        }
        #endregion

        #region METHODS
        /// <summary> 
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events. 
        /// </summary> 
        public void InstallHook()
        {
            try
            {
                MouseHookProcedure = new WinAPI.HookProc(MouseHookProc);// generate a HookProc instance. 
            }
            catch (Exception ex)
            {
                Console.WriteLine("HookProc error: " + ex.Message);
            }
            hMouseHook = WinAPI.SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, WinAPI.GetModuleHandle("user32"), 0);
            if (hMouseHook == IntPtr.Zero)
                throw new Win32Exception(1);
        }
        /// <summary> 
        /// Windows NT/2000/XP: Uninstalls a hook procedure that monitors low-level mouse input events. 
        /// </summary> 
        public void UnInstallHook()
        {
            MouseEvent = null;
            WinAPI.UnhookWindowsHookEx(hMouseHook);
        }

        /// <summary>
        /// A callback function which will be called every Time a mouse activity detected.
        /// </summary>
        /// <param name="nCode">
        /// [in] Specifies whether the hook procedure must process the message. 
        /// If nCode is HC_ACTION, the hook procedure must process the message. 
        /// If nCode is less than zero, the hook procedure must pass the message to the 
        /// CallNextHookEx function without further processing and must return the 
        /// value returned by CallNextHookEx.
        /// </param>
        /// <param name="wParam">
        /// [in] Specifies whether the message was sent by the current thread. 
        /// If the message was sent by the current thread, it is nonzero; otherwise, it is zero. 
        /// </param>
        /// <param name="lParam">
        /// [in] Pointer to a CWPSTRUCT structure that contains details about the message. 
        /// </param>
        /// <returns>
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx. 
        /// If nCode is greater than or equal to zero, it is highly recommended that you call CallNextHookEx 
        /// and return the value it returns; otherwise, other applications that have installed WH_CALLWNDPROC 
        /// hooks will not receive hook notifications and may behave incorrectly as a result. If the hook 
        /// procedure does not call CallNextHookEx, the return value should be zero. 
        /// </returns>
        private IntPtr MouseHookProc(int nCode, int wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                //Marshall the data from callback.
                WinAPI.MouseLLHookStruct mouseHookStruct = (WinAPI.MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(WinAPI.MouseLLHookStruct));

                MouseButtons button = MouseButtons.None;
                short mouseDelta = 0;
                int clickCount = 0;
                MouseEventExtArgs.UpDown upDown = MouseEventExtArgs.UpDown.None;
                bool mouseUp = false;

                //detect button clicked
                switch ((WinAPI.WindowMessage)wParam)
                {
                    case WinAPI.WindowMessage.WM_LBUTTONDOWN:
                        upDown = MouseEventExtArgs.UpDown.Down;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_LBUTTONUP:
                        upDown = MouseEventExtArgs.UpDown.Up;
                        button = MouseButtons.Left;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_LBUTTONDBLCLK:
                        button = MouseButtons.Left;
                        clickCount = 2;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONDOWN:
                        upDown = MouseEventExtArgs.UpDown.Down;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONUP:
                        upDown = MouseEventExtArgs.UpDown.Up;
                        button = MouseButtons.Right;
                        clickCount = 1;
                        break;
                    case WinAPI.WindowMessage.WM_RBUTTONDBLCLK:
                        button = MouseButtons.Right;
                        clickCount = 2;
                        break;
                    case WinAPI.WindowMessage.WM_MOUSEWHEEL:
                        //If the message is WM_MOUSEWHEEL, the high-order word of MouseData member is the wheel delta. 
                        //One wheel click is defined as WHEEL_DELTA, which is 120. 
                        //(value >> 16) & 0xffff; retrieves the high-order word from the given 32-bit value
                        mouseDelta = (short)((mouseHookStruct.MouseData >> 16) & 0xffff);
                        if (mouseDelta > 0)
                            upDown = MouseEventExtArgs.UpDown.Up;
                        if (mouseDelta < 0)
                            upDown = MouseEventExtArgs.UpDown.Down;
                        //TODO: X BUTTONS (I havent them so was unable to test)
                        //If the message is WM_XBUTTONDOWN, WM_XBUTTONUP, WM_XBUTTONDBLCLK, WM_NCXBUTTONDOWN, WM_NCXBUTTONUP, 
                        //or WM_NCXBUTTONDBLCLK, the high-order word specifies which X button was pressed or released, 
                        //and the low-order word is reserved. This value can be one or more of the following values. 
                        //Otherwise, MouseData is not used. 
                        break;
                }

                //generate event 
                //                if (CurrentMouseArgs != null)
                //
                //                    UpdateCount += 1; 
                CurrentMouseArgs = new MouseEventExtArgs(
                                                   button,
                                                   clickCount,
                                                   mouseHookStruct.Point.X,
                                                   mouseHookStruct.Point.Y,
                                                   mouseDelta) { Wheel = mouseDelta != 0, UpOrDown = upDown };
                MouseChanged = true;

                // Raise it 
                OnMouseChangedEvent(this, CurrentMouseArgs);
            }
            //call next hook
            return WinAPI.CallNextHookEx(hMouseHook, nCode, wParam, lParam);
        }

        /// <summary>
        /// Checking if mouse changed since last update call
        /// </summary>
        /// <returns>If mouse changed</returns>
        public bool Update()
        {
            if (CurrentMouseArgs != null && MouseChanged)
            {
                MouseChanged = false;
                return true;
            }
            return false;
        }
        #endregion
    }    
}
