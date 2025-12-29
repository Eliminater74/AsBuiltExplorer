using AsBuiltExplorer;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AsBuiltExplorer.Forms
{
    public partial class frmSettings : Form
    {
        public bool ThemeChanged { get; private set; } = false;
        public string SelectedTheme { get; private set; } = "Light";

        public frmSettings()
        {
            InitializeComponent();
            Load += FrmSettings_Load;
        }

        void FrmSettings_Load(object sender, EventArgs e)
        {
            // Load DB Stats
            UpdateDBStats();

            // Load Theme (This functionality will rely on a new property in MySettings)
            if (My.MySettings.Default.AppTheme == "Dark")
            {
                radThemeDark.Checked = true;
            }
            else
            {
                radThemeLight.Checked = true;
            }

            // Load Updater Settings
            chkAutoUpdate.Checked = My.MySettings.Default.AutoCheckForUpdates;
            lblCurrentVersion.Text = "Version: " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        void UpdateDBStats()
        {
            var count = VehicleDatabase.Entries.Count;
            lblDatabaseStats.Text = $"Stored Vehicles: {count}";

            long size = 0;

            try
            {
                var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.db");

                if (File.Exists(dbPath))
                {
                    size = new FileInfo(dbPath).Length / 1024;
                }
            }
            catch { }

            lblDatabaseStats.Text += $"  ({size} KB)";
        }

        void btnClearDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to COMPLETELY WIPE the vehicle database?\nThis cannot be undone.",
                "Confirm Wipe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                VehicleDatabase.ClearDatabase();
                UpdateDBStats();
                MessageBox.Show("Database cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            // Save settings
            var newTheme = radThemeDark.Checked ? "Dark" : "Light";

            if (My.MySettings.Default.AppTheme != newTheme)
            {
                My.MySettings.Default.AppTheme = newTheme;
                My.MySettings.Default.Save();
                ThemeChanged = true;
                SelectedTheme = newTheme;
            }

            // Save Updater Settings
            My.MySettings.Default.AutoCheckForUpdates = chkAutoUpdate.Checked;
            My.MySettings.Default.Save();

            DialogResult = DialogResult.OK;
            Close();
        }

        async void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            btnCheckUpdate.Enabled = false;
            btnCheckUpdate.Text = "Checking...";

            var info = await Utilities.GitHubUpdateChecker.CheckForUpdateAsync();

            btnCheckUpdate.Enabled = true;
            btnCheckUpdate.Text = "Check Now";

            if (info != null && info.IsNewer)
            {
                using (var frm = new frmUpdateAvailable(info))
                {
                    frm.ShowDialog();
                    // If skipped, save it
                    if (frm.Skipped)
                    {
                        My.MySettings.Default.SkipUpdateVersion = info.NewVersion;
                        My.MySettings.Default.Save();
                    }
                }
            }
            else
            {
                MessageBox.Show("You are running the latest version.", "Up to Date", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}