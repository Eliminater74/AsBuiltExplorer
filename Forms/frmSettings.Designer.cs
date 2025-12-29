
namespace AsBuiltExplorer.Forms
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpUpdates = new System.Windows.Forms.GroupBox();
            this.chkAutoUpdate = new System.Windows.Forms.CheckBox();
            this.lblCurrentVersion = new System.Windows.Forms.Label();
            this.btnCheckUpdate = new System.Windows.Forms.Button();
            this.grpAppearance = new System.Windows.Forms.GroupBox();
            this.radThemeDark = new System.Windows.Forms.RadioButton();
            this.radThemeLight = new System.Windows.Forms.RadioButton();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.btnClearDatabase = new System.Windows.Forms.Button();
            this.lblDatabaseStats = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.grpAppearance.SuspendLayout();
            this.grpData.SuspendLayout();
            this.grpUpdates.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpAppearance
            // 
            this.grpAppearance.Controls.Add(this.radThemeDark);
            this.grpAppearance.Controls.Add(this.radThemeLight);
            this.grpAppearance.Location = new System.Drawing.Point(12, 12);
            this.grpAppearance.Name = "grpAppearance";
            this.grpAppearance.Size = new System.Drawing.Size(300, 80);
            this.grpAppearance.TabIndex = 0;
            this.grpAppearance.TabStop = false;
            this.grpAppearance.Text = "Appearance";
            // 
            // radThemeDark
            // 
            this.radThemeDark.AutoSize = true;
            this.radThemeDark.Location = new System.Drawing.Point(150, 30);
            this.radThemeDark.Name = "radThemeDark";
            this.radThemeDark.Size = new System.Drawing.Size(78, 17);
            this.radThemeDark.TabIndex = 1;
            this.radThemeDark.Text = "Dark Mode";
            this.radThemeDark.UseVisualStyleBackColor = true;
            // 
            // radThemeLight
            // 
            this.radThemeLight.AutoSize = true;
            this.radThemeLight.Checked = true;
            this.radThemeLight.Location = new System.Drawing.Point(20, 30);
            this.radThemeLight.Name = "radThemeLight";
            this.radThemeLight.Size = new System.Drawing.Size(78, 17);
            this.radThemeLight.TabIndex = 0;
            this.radThemeLight.TabStop = true;
            this.radThemeLight.Text = "Light Mode";
            this.radThemeLight.UseVisualStyleBackColor = true;
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.btnClearDatabase);
            this.grpData.Controls.Add(this.lblDatabaseStats);
            this.grpData.Location = new System.Drawing.Point(12, 110);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(300, 100);
            this.grpData.TabIndex = 1;
            this.grpData.TabStop = false;
            this.grpData.Text = "Data Management";
            // 
            // btnClearDatabase
            // 
            this.btnClearDatabase.BackColor = System.Drawing.Color.IndianRed;
            this.btnClearDatabase.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnClearDatabase.Location = new System.Drawing.Point(165, 40);
            this.btnClearDatabase.Name = "btnClearDatabase";
            this.btnClearDatabase.Size = new System.Drawing.Size(110, 30);
            this.btnClearDatabase.TabIndex = 1;
            this.btnClearDatabase.Text = "Clear Database";
            this.btnClearDatabase.UseVisualStyleBackColor = false;
            this.btnClearDatabase.Click += new System.EventHandler(this.btnClearDatabase_Click);
            // 
            // lblDatabaseStats
            // 
            this.lblDatabaseStats.AutoSize = true;
            this.lblDatabaseStats.Location = new System.Drawing.Point(20, 49);
            this.lblDatabaseStats.Name = "lblDatabaseStats";
            this.lblDatabaseStats.Size = new System.Drawing.Size(95, 13);
            this.lblDatabaseStats.TabIndex = 0;
            this.lblDatabaseStats.Text = "Stored Vehicles: 0";
            //
            // grpUpdates
            //
            this.grpUpdates.Controls.Add(this.chkAutoUpdate);
            this.grpUpdates.Controls.Add(this.lblCurrentVersion);
            this.grpUpdates.Controls.Add(this.btnCheckUpdate);
            this.grpUpdates.Location = new System.Drawing.Point(12, 220);
            this.grpUpdates.Name = "grpUpdates";
            this.grpUpdates.Size = new System.Drawing.Size(300, 100);
            this.grpUpdates.TabIndex = 2;
            this.grpUpdates.TabStop = false;
            this.grpUpdates.Text = "Updates";
            //
            // chkAutoUpdate
            //
            this.chkAutoUpdate.AutoSize = true;
            this.chkAutoUpdate.Location = new System.Drawing.Point(20, 30);
            this.chkAutoUpdate.Name = "chkAutoUpdate";
            this.chkAutoUpdate.Size = new System.Drawing.Size(180, 17);
            this.chkAutoUpdate.TabIndex = 0;
            this.chkAutoUpdate.Text = "Check for updates on startup";
            this.chkAutoUpdate.UseVisualStyleBackColor = true;
            //
            // lblCurrentVersion
            //
            this.lblCurrentVersion.AutoSize = true;
            this.lblCurrentVersion.Location = new System.Drawing.Point(20, 60);
            this.lblCurrentVersion.Name = "lblCurrentVersion";
            this.lblCurrentVersion.Size = new System.Drawing.Size(80, 13);
            this.lblCurrentVersion.TabIndex = 1;
            this.lblCurrentVersion.Text = "Version: -";
            //
            // btnCheckUpdate
            //
            this.btnCheckUpdate.Location = new System.Drawing.Point(180, 55);
            this.btnCheckUpdate.Name = "btnCheckUpdate";
            this.btnCheckUpdate.Size = new System.Drawing.Size(100, 25);
            this.btnCheckUpdate.TabIndex = 2;
            this.btnCheckUpdate.Text = "Check Now";
            this.btnCheckUpdate.UseVisualStyleBackColor = true;
            this.btnCheckUpdate.Click += new System.EventHandler(this.btnCheckUpdate_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(135, 330);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(85, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(227, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(85, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // frmSettings
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(324, 380);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.grpAppearance);
            this.Controls.Add(this.grpUpdates);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.grpAppearance.ResumeLayout(false);
            this.grpAppearance.PerformLayout();
            this.grpData.ResumeLayout(false);
            this.grpData.PerformLayout();
            this.grpUpdates.ResumeLayout(false);
            this.grpUpdates.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpAppearance;
        private System.Windows.Forms.RadioButton radThemeDark;
        private System.Windows.Forms.RadioButton radThemeLight;
        private System.Windows.Forms.GroupBox grpData;
        private System.Windows.Forms.Button btnClearDatabase;
        private System.Windows.Forms.Label lblDatabaseStats;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox grpUpdates;
        private System.Windows.Forms.CheckBox chkAutoUpdate;
        private System.Windows.Forms.Label lblCurrentVersion;
        private System.Windows.Forms.Button btnCheckUpdate;
    }
}
