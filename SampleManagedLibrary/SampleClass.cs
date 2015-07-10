using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SampleManagedLibrary
{
    public class SampleClass
    {
        public static int SampleMethod(string pwzArgument)
        {
            MessageBox.Show(string.Format("Process: {0}\nArgument: \"{1}\"", System.Diagnostics.Process.GetCurrentProcess().ProcessName, pwzArgument));
            return 0;
        }
    }
}
