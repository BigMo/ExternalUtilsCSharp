using ExternalUtilsCSharp;
using ExternalUtilsCSharp.Injection.Injectors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DotNetInjector
{
    public partial class frmMain : Form
    {
        #region STRUCTS
        /*struct BootStrapData
        {
	        wchar_t pwzVersion[64];
	        wchar_t pwzAssemblyPath[512];
	        wchar_t pwzTypeName[128];
	        wchar_t pwzMethodName[128];
	        wchar_t pwzArgument[256];
        };*/
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct BootStrapData
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string pwzVersion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)]
            public string pwzAssemblyPath;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string pwzTypeName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string pwzMethodName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string pwzArgument;
        }
        #endregion

        #region CONSTRUCTOR
        public frmMain()
        {
            InitializeComponent();
            LoadRuntimeVersions();
            RefreshProcessList();
            txbBootstrapperDLL.Text = @"C:\Users\Mo\Documents\Visual Studio 2013\Projects\ExternalUtilsCSharp\Debug\DotNetBoorstrapper.dll";
            txbManagedAssemblyPath.Text = @"C:\Users\Mo\Documents\Visual Studio 2013\Projects\ExternalUtilsCSharp\SampleManagedLibrary\bin\Debug\SampleManagedLibrary.dll";
            txbManagedAssemblyType.Text = "SampleManagedLibrary.SampleClass";
            txbManagedAssemblyMethod.Text = "SampleMethod";
            txbManagedAssemblyArgument.Text = "SampleArgument";
        }
        #endregion

        #region METHODS
        private void AppendLog(string format, params object[] values)
        {
            richTextBox1.AppendText(string.Format("{0}\n", string.Format(format, values)));
        }
        private void SelectFile(TextBox txb, string title)
        {
            using(OpenFileDialog diag = new OpenFileDialog())
            {
                diag.Title = title;
                if (diag.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    txb.Text = diag.FileName;
            }
        }
        private void RefreshProcessList()
        {
            cbbTargetProcess.Items.Clear();
            foreach (Process proc in Process.GetProcesses())
            {
                try
                {
                    bool is64bit = false;
                    WinAPI.IsWow64Process(proc.Handle, out is64bit);
                    cbbTargetProcess.Items.Add(string.Format("{2} | {0} | {1}", proc.ProcessName, proc.Id, is64bit ? "64bit" : "32bit"));
                }catch(Exception ex)
                {
                    AppendLog("Couldn't load process-info: {0}", ex.Message);
                }
            }
            if (cbbTargetProcess.Items.Count > 0)
                cbbTargetProcess.SelectedIndex = 0;
        }
        private void LoadRuntimeVersions()
        {
            foreach (DirectoryInfo dir in new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), @"Microsoft.NET\Framework")).EnumerateDirectories("v*", SearchOption.TopDirectoryOnly))
            {
                cbbNETFrameworkVersion.Items.Add(dir.Name);
            }
            if (cbbNETFrameworkVersion.Items.Count > 0)
                cbbNETFrameworkVersion.SelectedIndex = 0;
        }
        #endregion

        #region EVENTS
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.Select(richTextBox1.TextLength, 0);
            richTextBox1.ScrollToCaret();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SelectFile(txbBootstrapperDLL, "Select bootstrapper-dll");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectFile(txbBootstrapperDLL, "Select managed assembly");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            RefreshProcessList();
        }
        #endregion

        private void btnInject_Click(object sender, EventArgs e)
        {
            LoadLibraryCRTInjector injector = new LoadLibraryCRTInjector();
            try
            {
                int pid = Convert.ToInt32(cbbTargetProcess.SelectedItem.ToString().Split('|')[2].Trim());

                AppendLog("Injecting into PID {0}...", pid);
                DllInjectionResult dResult = injector.Inject(pid, txbBootstrapperDLL.Text);
                if(!dResult.Success)
                {
                    AppendLog("Injection failed: {0}", dResult.ErrorMessage);
                    return;
                }
                else
                {
                    AppendLog("Injected successfully!");
                }

                injector.ReadExportedFunctions();
                if (injector.ExportedFunctions.Count(x => x.Item1 == "RunManagedAssembly") == 0)
                {
                    AppendLog("Injection failed: Method \"RunManagedAssembly\" not found in the injected assembly!");
                    injector.UnloadLibrary();
                    return;
                }

                BootStrapData data = new BootStrapData();
                data.pwzAssemblyPath = txbManagedAssemblyPath.Text + '\0';
                data.pwzMethodName = txbManagedAssemblyMethod.Text + '\0';
                data.pwzTypeName = txbManagedAssemblyType.Text + '\0';
                data.pwzVersion = cbbNETFrameworkVersion.SelectedItem.ToString() + '\0';
                data.pwzArgument = txbManagedAssemblyArgument.Text + '\0';

                AppendLog("Running \"RunManagedAssembly\"...");
                int eResult = injector.ExecuteRemoteFunction("RunManagedAssembly", data);

                AppendLog("Function returned \"{0}\" (0x{1})", eResult.ToString(), eResult.ToString("X"));
                AppendLog("Unloading module...");
                if (injector.UnloadLibrary())
                    AppendLog("Module successfully unloaded!");
                else
                    AppendLog("Failed to unload module!");
            }
            catch(Exception ex)
            {
                AppendLog("An exception occured while injecting: {0}\n{1}\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }
    }
}
