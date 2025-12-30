using System;
using System.Windows.Forms;
using System.Threading;
using AsBuiltExplorer.Forms;

namespace AsBuiltExplorer
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Enforce Single Instance
            bool createdNew;
            using (Mutex mutex = new Mutex(true, "AsBuiltExplorer_Singleton_Mutex", out createdNew))
            {
                if (!createdNew)
                {
                    // Optional: Bring existing window to front (requires P/Invoke), skipping for now to keep it simple
                    // Just exit if already running
                    return;
                }

                // Global Exception Handling
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                try 
                {
                    // Registry Fixes for Legacy IE Control (MotorCraft)
                    ApplyRegistryFixes();

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Auto-Save Settings on Exit
                    Application.ApplicationExit += (sender, e) => {
                        try { AsBuiltExplorer.Properties.Settings.Default.Save(); } catch { }
                    };

                    Application.Run(new Form1());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Fatal Application Error: " + ex.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        static void ApplyRegistryFixes()
        {
            try
            {
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
                // Silently fail or warn? Existing code warned.
                MessageBox.Show("Failed to set IE11 Emulation Key: " + ex.Message, "Registry Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MessageBox.Show("Fatal Startup Error (Thread): " + e.Exception.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Fatal Startup Error (Domain): " + e.ExceptionObject.ToString(), "AsBuiltExplorer Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
