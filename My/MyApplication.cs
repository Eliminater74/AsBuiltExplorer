using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace AsBuiltExplorer.My
{
    [GeneratedCode("MyTemplate", "11.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal class MyApplication : WindowsFormsApplicationBase
    {
        [DebuggerStepThrough]
        public MyApplication()
          : base(AuthenticationMode.Windows)
        {
            IsSingleInstance = true;
            EnableVisualStyles = true;
            SaveMySettingsOnExit = true;
            ShutdownStyle = ShutdownMode.AfterMainFormCloses;
        }
        [STAThread]
        [DebuggerHidden]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        internal static void Main(string[] Args)
        {
            // Fix for WebBrowser control to use IE11 mode (11001)
            try
            {
                // Use both dynamic and hardcoded names to be safe
                string dynamicName = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                string[] appNames = { "AsBuiltExplorer.exe", dynamicName };

                using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION"))
                {
                    if (key != null)
                    {
                        foreach (string name in appNames)
                        {
                            key.SetValue(name, 11001, Microsoft.Win32.RegistryValueKind.DWord);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Warn user if this fails, as it breaks the MotorCraft tab
                System.Windows.Forms.MessageBox.Show("Failed to set IE11 Emulation Key: " + ex.Message, "Registry Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            try
            {
                Application.SetCompatibleTextRenderingDefault(WindowsFormsApplicationBase.UseCompatibleTextRendering);
            }
            finally
            {
            }

            // Global Exception Handling to catch "Silent Crashes"
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                MessageBox.Show("Fatal Startup Error (Domain): " + e.ExceptionObject.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };

            Application.ThreadException += (sender, e) => 
            {
                MessageBox.Show("Fatal Startup Error (Thread): " + e.Exception.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            try
            {
                MyProject.Application.Run(Args);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fatal Application Error: " + ex.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [DebuggerStepThrough]
        protected override void OnCreateMainForm() => MainForm = (Form)MyProject.Forms.Form1;
    }
}