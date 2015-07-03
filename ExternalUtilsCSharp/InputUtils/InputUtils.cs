using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.InputUtils
{
    public class InputUtilities{
        public KeyUtils keyUtils;
        public MouseHook mouse;
   
        public InputUtilities()
        {
            Init();
        }

        private void Init()
        {
            keyUtils = new KeyUtils();
            mouse = new MouseHook();
            mouse.InstallHook();
        }

        public bool MouseChangedSinceLastUpdate = false;
        public void Update()
        {
            keyUtils.Update();
            MouseChangedSinceLastUpdate = mouse.Update();
        }
    }
}
