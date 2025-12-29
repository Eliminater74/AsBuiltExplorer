using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AsBuiltExplorer.Forms
{
    public class frmAboutDev : Form
    {
        private WebBrowser wbContent;

        public frmAboutDev()
        {
            InitializeComponent();
            LoadContent();
        }

        private void InitializeComponent()
        {
            this.wbContent = new WebBrowser();
            this.SuspendLayout();
            
            // 
            // wbContent
            // 
            this.wbContent.Dock = DockStyle.Fill;
            this.wbContent.Location = new Point(0, 0);
            this.wbContent.MinimumSize = new Size(20, 20);
            this.wbContent.Name = "wbContent";
            this.wbContent.ScriptErrorsSuppressed = true;
            this.wbContent.IsWebBrowserContextMenuEnabled = false;
            this.wbContent.AllowWebBrowserDrop = false;
            this.wbContent.ScrollBarsEnabled = true;
            this.wbContent.TabIndex = 0;

            // 
            // frmAboutDev
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(950, 700);
            this.Controls.Add(this.wbContent);
            this.Name = "frmAboutDev";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About the Developer";
            this.Icon = SystemIcons.Information; // Or app icon if available
            this.ResumeLayout(false);
        }

        private void LoadContent()
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Help", "about_dev.html");
                if (File.Exists(path))
                {
                    wbContent.Navigate(path);
                }
                else
                {
                    wbContent.DocumentText = "<html><body style='background:black;color:white'><h1>Error: Content not found.</h1></body></html>";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading content: " + ex.Message);
            }
        }
    }
}
