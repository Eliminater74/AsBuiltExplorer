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
            this.Load += FrmSettings_Load;
        }

        private void FrmSettings_Load(object sender, EventArgs e)
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
        }

        private void UpdateDBStats()
        {
            int count = VehicleDatabase.Entries.Count;
            lblDatabaseStats.Text = $"Stored Vehicles: {count}";
            
            long size = 0;
            try
            {
                string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.db");
                if (File.Exists(dbPath))
                {
                    size = new FileInfo(dbPath).Length / 1024;
                }
            }
            catch { }
            
            lblDatabaseStats.Text += $"  ({size} KB)";
        }

        private void btnClearDatabase_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to COMPLETELY WIPE the vehicle database?\nThis cannot be undone.", 
                "Confirm Wipe", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                VehicleDatabase.ClearDatabase();
                UpdateDBStats();
                MessageBox.Show("Database cleared successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Save settings
            string newTheme = radThemeDark.Checked ? "Dark" : "Light";
            
            if (My.MySettings.Default.AppTheme != newTheme)
            {
                My.MySettings.Default.AppTheme = newTheme;
                My.MySettings.Default.Save();
                ThemeChanged = true;
                SelectedTheme = newTheme;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
