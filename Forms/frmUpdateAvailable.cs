using System;
using System.Diagnostics;
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
            txtReleaseNotes.Text = txtReleaseNotes.Text.Replace("\n", Environment.NewLine); 
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_info.DownloadUrl))
            {
                Process.Start(_info.DownloadUrl);
            }
            Close();
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
