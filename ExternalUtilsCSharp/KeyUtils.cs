using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace ExternalUtilsCSharp
{
    public class KeyUtils
    {
        #region VARIABLES
        private Hashtable keys, prevKeys;
        private int[] allKeys;
        #endregion
        #region STATIC METHODS
        public static bool GetKeyDown(Keys key)
        {
            return GetKeyDown((Int32)key);
        }
        public static bool GetKeyDown(Int32 key)
        {
            return Convert.ToBoolean(WinAPI.GetKeyState(key) & WinAPI.KEY_PRESSED);
        }
        public static bool GetKeyDownAsync(Int32 key)
        {
            return GetKeyDownAsync((Keys)key);
        }
        public static bool GetKeyDownAsync(Keys key)
        {
            return Convert.ToBoolean(WinAPI.GetAsyncKeyState(key) & WinAPI.KEY_PRESSED);
        }
        #endregion
        #region CONSTRUCTOR/DESTRUCTOR
        public KeyUtils()
        {
            keys = new Hashtable();
            prevKeys = new Hashtable();
            allKeys = (int[])Enum.GetValues(typeof(Keys));
        }
        ~KeyUtils()
        {
            keys.Clear();
            prevKeys.Clear();
            Init();
        }
        #endregion
        #region METHODS
        private void Init()
        {
            foreach (Int32 key in allKeys)
            {
                if (!prevKeys.ContainsKey(key))
                {
                    prevKeys.Add(key, false);
                    keys.Add(key, false);
                }
            }
        }
        public void Update()
        {
            prevKeys = (Hashtable)keys.Clone();
            foreach (Int32 key in allKeys)
            {
                keys[key] = GetKeyDown(key);
            }
        }
        public Keys[] KeysThatWentUp()
        {
            List<Keys> keys = new List<Keys>();
            foreach (Keys key in allKeys)
            {
                if (KeyWentUp(key))
                    keys.Add(key);
            }
            return keys.ToArray();
        }
        public Keys[] KeysThatWentDown()
        {
            List<Keys> keys = new List<Keys>();
            foreach (Keys key in allKeys)
            {
                if (KeyWentDown(key))
                    keys.Add(key);
            }
            return keys.ToArray();
        }
        public Keys[] KeysThatAreDown()
        {
            List<Keys> keys = new List<Keys>();
            foreach (Keys key in allKeys)
            {
                if (KeyIsDown(key))
                    keys.Add(key);
            }
            return keys.ToArray();
        }
        public bool KeyWentUp(Keys key)
        {
            return KeyWentUp((Int32)key);
        }
        public bool KeyWentUp(Int32 key)
        {
            if (!KeyExists(key))
                return false;
            return (bool)prevKeys[key] && !(bool)keys[key];
        }
        public bool KeyWentDown(Keys key)
        {
            return KeyWentDown((Int32)key);
        }
        public bool KeyWentDown(Int32 key)
        {
            if (!KeyExists(key))
                return false;
            return !(bool)prevKeys[key] && (bool)keys[key];
        }
        public bool KeyIsDown(Keys key)
        {
            return KeyIsDown((Int32)key);
        }
        public bool KeyIsDown(Int32 key)
        {
            if (!KeyExists(key))
                return false;
            return (bool)prevKeys[key] || (bool)keys[key];
        }
        private bool KeyExists(Int32 key)
        {
            return (prevKeys.ContainsKey(key) && keys.ContainsKey(key));
        }
        #endregion
    }
}
