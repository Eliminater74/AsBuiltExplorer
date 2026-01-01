using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;
using AsBuiltExplorer.Utilities;

namespace AsBuiltExplorer.Forms
{
    public partial class frmUpdateAvailable : Form
    {
        private UpdateInfo _info;
        public bool Skipped { get; private set; } = false;

        public frmUpdateAvailable(UpdateInfo info)
        {
            InitializeComponent();
            _info = info;

            lblCurrentVer.Text = $"Current Version: {info.CurrentVersion}";
            lblNewVer.Text = $"New Version: {info.NewVersion}";
            txtReleaseNotes.Text = info.ReleaseNotes;
            // Handle new lines correctly if they are raw \n
            if (!string.IsNullOrEmpty(txtReleaseNotes.Text))
                txtReleaseNotes.Text = txtReleaseNotes.Text.Replace("\n", Environment.NewLine); 
        }

        private async void btnDownload_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_info.DownloadUrl))
            {
                Close();
                return;
            }

            // Check if direct download (EXE/MSI)
            bool isDirect = _info.DownloadUrl.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) || 
                            _info.DownloadUrl.EndsWith(".msi", StringComparison.OrdinalIgnoreCase);

            if (isDirect)
            {
                // In-App Update
                btnDownload.Enabled = false;
                btnSkip.Enabled = false;
                btnRemind.Enabled = false;
                btnDownload.Text = "Downloading...";

                string tempPath = Path.Combine(Path.GetTempPath(), "AsBuiltExplorer_Update.exe");

                try
                {
                    using (var client = new WebClient())
                    {
                        // Simple progress tracking
                        client.DownloadProgressChanged += (s, ev) => 
                        {
                            btnDownload.Text = $"{ev.ProgressPercentage}%";
                        };

                        await client.DownloadFileTaskAsync(new Uri(_info.DownloadUrl), tempPath);
                    }

                    // Run Installer
                    btnDownload.Text = "Installing...";
                    Process.Start(new ProcessStartInfo(tempPath) { UseShellExecute = true });
                    
                    // Exit Application so installer can overwrite files
                    Application.Exit(); 
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Download failed: " + ex.Message + "\nOpening browser instead.", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Process.Start(_info.DownloadUrl); // Fallback to browser
                    Close();
                }
            }
            else
            {
                // Web Link
                Process.Start(_info.DownloadUrl);
                Close();
            }
        }

        private void btnRemind_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            Skipped = true;
            Close();
        }
    }
}
