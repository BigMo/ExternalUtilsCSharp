using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalUtilsCSharp.SharpDXRenderer.Controls.Layouts
{
    public class NoneLayout : Layout
    {
        #region SINGLETON
        private static NoneLayout instance = new NoneLayout();
        public static Layout Instance { get { return instance; } }
        private NoneLayout()
            : base()
        { }
        #endregion
        
        public override void ApplyLayout(SharpDXControl parent)
        { }
    }
}
