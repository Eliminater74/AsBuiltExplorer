namespace AsBuiltExplorer.Forms
{
    partial class frmUpdateAvailable
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblCurrentVer = new System.Windows.Forms.Label();
            this.lblNewVer = new System.Windows.Forms.Label();
            this.txtReleaseNotes = new System.Windows.Forms.TextBox();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnRemind = new System.Windows.Forms.Button();
            this.btnSkip = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.SeaGreen;
            this.lblHeader.Location = new System.Drawing.Point(12, 15);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(248, 20);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "A New Version is Available!";
            // 
            // lblCurrentVer
            // 
            this.lblCurrentVer.AutoSize = true;
            this.lblCurrentVer.Location = new System.Drawing.Point(16, 50);
            this.lblCurrentVer.Name = "lblCurrentVer";
            this.lblCurrentVer.Size = new System.Drawing.Size(89, 13);
            this.lblCurrentVer.TabIndex = 1;
            this.lblCurrentVer.Text = "Current Version: -";
            // 
            // lblNewVer
            // 
            this.lblNewVer.AutoSize = true;
            this.lblNewVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNewVer.Location = new System.Drawing.Point(200, 50);
            this.lblNewVer.Name = "lblNewVer";
            this.lblNewVer.Size = new System.Drawing.Size(88, 13);
            this.lblNewVer.TabIndex = 2;
            this.lblNewVer.Text = "New Version: -";
            // 
            // txtReleaseNotes
            // 
            this.txtReleaseNotes.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtReleaseNotes.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtReleaseNotes.Location = new System.Drawing.Point(16, 80);
            this.txtReleaseNotes.Multiline = true;
            this.txtReleaseNotes.Name = "txtReleaseNotes";
            this.txtReleaseNotes.ReadOnly = true;
            this.txtReleaseNotes.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReleaseNotes.Size = new System.Drawing.Size(360, 180);
            this.txtReleaseNotes.TabIndex = 3;
            // 
            // btnDownload
            // 
            this.btnDownload.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnDownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnDownload.ForeColor = System.Drawing.Color.White;
            this.btnDownload.Location = new System.Drawing.Point(250, 275);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(126, 35);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download Update";
            this.btnDownload.UseVisualStyleBackColor = false;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnRemind
            // 
            this.btnRemind.Location = new System.Drawing.Point(130, 275);
            this.btnRemind.Name = "btnRemind";
            this.btnRemind.Size = new System.Drawing.Size(100, 35);
            this.btnRemind.TabIndex = 5;
            this.btnRemind.Text = "Remind Later";
            this.btnRemind.UseVisualStyleBackColor = true;
            this.btnRemind.Click += new System.EventHandler(this.btnRemind_Click);
            // 
            // btnSkip
            // 
            this.btnSkip.Location = new System.Drawing.Point(16, 275);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(100, 35);
            this.btnSkip.TabIndex = 6;
            this.btnSkip.Text = "Skip Version";
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // frmUpdateAvailable
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 326);
            this.Controls.Add(this.btnSkip);
            this.Controls.Add(this.btnRemind);
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.txtReleaseNotes);
            this.Controls.Add(this.lblNewVer);
            this.Controls.Add(this.lblCurrentVer);
            this.Controls.Add(this.lblHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUpdateAvailable";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Update Available";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblCurrentVer;
        private System.Windows.Forms.Label lblNewVer;
        private System.Windows.Forms.TextBox txtReleaseNotes;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnRemind;
        private System.Windows.Forms.Button btnSkip;
    }
}
