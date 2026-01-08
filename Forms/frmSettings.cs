using AsBuiltExplorer;
using AsBuiltExplorer.Localization;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AsBuiltExplorer.Forms
{
    public partial class frmSettings : Form
    {
        public bool ThemeChanged { get; private set; } = false;
        public string SelectedTheme { get; private set; } = "Light";
        public bool LanguageChanged { get; private set; } = false;

        private string _originalLanguage;

        public frmSettings()
        {
            InitializeComponent();
            Load += FrmSettings_Load;
        }

        void FrmSettings_Load(object sender, EventArgs e)
        {
            // Load DB Stats
            UpdateDBStats();

            // Load Theme
            if (AsBuiltExplorer.Properties.Settings.Default.AppTheme == "Dark")
            {
                radThemeDark.Checked = true;
            }
            else
            {
                radThemeLight.Checked = true;
            }

            // Load Updater Settings
            chkAutoUpdate.Checked = AsBuiltExplorer.Properties.Settings.Default.AutoCheckForUpdates;
            lblCurrentVersion.Text = string.Format(Strings.Settings_Version,
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());

            // Load Language Settings
            LoadLanguageSettings();

            // Apply localization to this form
            ApplyLocalization();
        }

        void LoadLanguageSettings()
        {
            // Get available languages
            var languages = LocalizationManager.GetAvailableLanguages();
            cmbLanguage.Items.Clear();
            cmbLanguage.Items.AddRange(languages.ToArray());

            // Select current language
            _originalLanguage = LocalizationManager.CurrentLanguage;
            var currentLang = languages.FirstOrDefault(l => l.Code == _originalLanguage);
            if (currentLang != null)
            {
                cmbLanguage.SelectedItem = currentLang;
            }
            else
            {
                cmbLanguage.SelectedIndex = 0; // Default to "Auto"
            }
        }

        void ApplyLocalization()
        {
            // Apply localized strings to this form
            this.Text = Strings.Settings_Title;
            grpAppearance.Text = Strings.Settings_Appearance;
            radThemeLight.Text = Strings.Settings_LightMode;
            radThemeDark.Text = Strings.Settings_DarkMode;
            grpData.Text = Strings.Settings_DataManagement;
            btnClearDatabase.Text = Strings.Btn_ClearDatabase;
            grpUpdates.Text = Strings.Settings_Updates;
            chkAutoUpdate.Text = Strings.Settings_CheckUpdatesOnStartup;
            btnCheckUpdate.Text = Strings.Btn_CheckNow;
            grpLanguage.Text = Strings.Settings_Language;
            lblLanguage.Text = Strings.Settings_SelectLanguage;
            btnSave.Text = Strings.Btn_Save;
            btnCancel.Text = Strings.Btn_Cancel;
        }

        void UpdateDBStats()
        {
            var count = VehicleDatabase.Entries.Count;
            lblDatabaseStats.Text = string.Format(Strings.Settings_StoredVehicles, count);

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
            if (MessageBox.Show(Strings.Msg_ConfirmWipe,
                Strings.Msg_Title_ConfirmWipe, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                VehicleDatabase.ClearDatabase();
                UpdateDBStats();
                MessageBox.Show(Strings.Msg_DatabaseCleared, Strings.Msg_Title_Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            // Save Theme settings
            var newTheme = radThemeDark.Checked ? "Dark" : "Light";

            if (AsBuiltExplorer.Properties.Settings.Default.AppTheme != newTheme)
            {
                AsBuiltExplorer.Properties.Settings.Default.AppTheme = newTheme;
                AsBuiltExplorer.Properties.Settings.Default.Save();
                ThemeChanged = true;
                SelectedTheme = newTheme;
            }

            // Save Updater Settings
            AsBuiltExplorer.Properties.Settings.Default.AutoCheckForUpdates = chkAutoUpdate.Checked;
            AsBuiltExplorer.Properties.Settings.Default.Save();

            // Save Language Settings
            var selectedLang = cmbLanguage.SelectedItem as LanguageInfo;
            if (selectedLang != null && selectedLang.Code != _originalLanguage)
            {
                LocalizationManager.ApplyLanguage(selectedLang.Code);
                LanguageChanged = true;

                // Show restart recommendation
                MessageBox.Show(Strings.Settings_RestartRequired,
                    Strings.Msg_Title_Information,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        async void btnCheckUpdate_Click(object sender, EventArgs e)
        {
            btnCheckUpdate.Enabled = false;
            btnCheckUpdate.Text = Strings.Btn_Checking;

            var info = await Utilities.GitHubUpdateChecker.CheckForUpdateAsync();

            btnCheckUpdate.Enabled = true;
            btnCheckUpdate.Text = Strings.Btn_CheckNow;

            if (info != null && info.IsNewer)
            {
                using (var frm = new frmUpdateAvailable(info))
                {
                    frm.ShowDialog();
                    // If skipped, save it
                    if (frm.Skipped)
                    {
                        AsBuiltExplorer.Properties.Settings.Default.SkipUpdateVersion = info.NewVersion;
                        AsBuiltExplorer.Properties.Settings.Default.Save();
                    }
                }
            }
            else
            {
                MessageBox.Show(Strings.Msg_UpToDate, Strings.Msg_Title_UpToDate, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}