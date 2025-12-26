using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
namespace AsBuiltExplorer
{
  partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.TabControl1 = new System.Windows.Forms.TabControl();
            this.TabPage1 = new System.Windows.Forms.TabPage();
            this.chkShowOnlyMismatches = new System.Windows.Forms.CheckBox();
            this.chkCompareShowNames = new System.Windows.Forms.CheckBox();
            this.chkCompareShowNames.CheckedChanged += new System.EventHandler(this.chkCompareShowNames_CheckedChanged);
            this.lblComp4VIN = new System.Windows.Forms.Label();
            this.lblComp3VIN = new System.Windows.Forms.Label();
            this.lblComp2VIN = new System.Windows.Forms.Label();
            this.lblComp1VIN = new System.Windows.Forms.Label();
            this.Button9 = new System.Windows.Forms.Button();
            this.tbxCompFile4 = new System.Windows.Forms.TextBox();
            this.Label23 = new System.Windows.Forms.Label();
            this.Button7 = new System.Windows.Forms.Button();
            this.tbxCompFile3 = new System.Windows.Forms.TextBox();
            this.Label22 = new System.Windows.Forms.Label();
            this.chkCompareShowChecksum = new System.Windows.Forms.CheckBox();
            this.btnCompLoad = new System.Windows.Forms.Button();
            this.btnCompBrowse2 = new System.Windows.Forms.Button();
            this.btnCompBrowse1 = new System.Windows.Forms.Button();
            this.tbxCompFile2 = new System.Windows.Forms.TextBox();
            this.tbxCompFile1 = new System.Windows.Forms.TextBox();
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CopyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EntireLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Data1hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Data2hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Data3hexToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BinaryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExportModuleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToUCDSToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToABTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Label7 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.TabPage2 = new System.Windows.Forms.TabPage();
            this.TextBox5 = new System.Windows.Forms.TextBox();
            this.Button3 = new System.Windows.Forms.Button();
            this.Button2 = new System.Windows.Forms.Button();
            this.Label16 = new System.Windows.Forms.Label();
            this.Label15 = new System.Windows.Forms.Label();
            this.tbxChecksumBin = new System.Windows.Forms.TextBox();
            this.tbxConvertBin = new System.Windows.Forms.TextBox();
            this.tbxConvertHex = new System.Windows.Forms.TextBox();
            this.tbxData3bin2 = new System.Windows.Forms.TextBox();
            this.tbxData2bin2 = new System.Windows.Forms.TextBox();
            this.tbxData1bin2 = new System.Windows.Forms.TextBox();
            this.tbxData3bin1 = new System.Windows.Forms.TextBox();
            this.tbxData2bin1 = new System.Windows.Forms.TextBox();
            this.tbxData1bin1 = new System.Windows.Forms.TextBox();
            this.tbxChecksumHex = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.Button1 = new System.Windows.Forms.Button();
            this.tbxData3hex = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.tbxData2hex = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.tbxData1hex = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.tbxModIDhex = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.TabPage3 = new System.Windows.Forms.TabPage();
            this.TextBox6 = new System.Windows.Forms.TextBox();
            this.TextBox2 = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.chkDeduceDownloadAB = new System.Windows.Forms.CheckBox();
            this.btnDeduceSaveInfo = new System.Windows.Forms.Button();
            this.wbDeducer = new System.Windows.Forms.WebBrowser();
            this.btnDeduceOpenETIS = new System.Windows.Forms.Button();
            this.TabPage4 = new System.Windows.Forms.TabPage();
            this.chkDeduceDoCCC = new System.Windows.Forms.CheckBox();
            this.Button8 = new System.Windows.Forms.Button();
            this.Label14 = new System.Windows.Forms.Label();
            this.TextBox1 = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.Label11 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.lstDeduceYears = new System.Windows.Forms.CheckedListBox();
            this.lstDeduceModels = new System.Windows.Forms.CheckedListBox();
            this.tbxDeduceReport = new System.Windows.Forms.TextBox();
            this.btnDeduceFigureIt = new System.Windows.Forms.Button();
            this.btnDeduceLoadOptions = new System.Windows.Forms.Button();
            this.lstDeduceFactoryOptions = new System.Windows.Forms.ListBox();
            this.TabPage5 = new System.Windows.Forms.TabPage();
            this.Button10 = new System.Windows.Forms.Button();
            this.TextBox4 = new System.Windows.Forms.TextBox();
            this.Label21 = new System.Windows.Forms.Label();
            this.lstBit_Modules = new System.Windows.Forms.ListBox();
            this.Button6 = new System.Windows.Forms.Button();
            this.Button5 = new System.Windows.Forms.Button();
            this.Label20 = new System.Windows.Forms.Label();
            this.Label19 = new System.Windows.Forms.Label();
            this.lstBit_Years = new System.Windows.Forms.CheckedListBox();
            this.lstBit_Models = new System.Windows.Forms.CheckedListBox();
            this.Label17 = new System.Windows.Forms.Label();
            this.TextBox3 = new System.Windows.Forms.TextBox();
            this.Label18 = new System.Windows.Forms.Label();
            this.tbxDeduceReport2 = new System.Windows.Forms.TextBox();
            this.Button4 = new System.Windows.Forms.Button();
            this.TabPage6 = new System.Windows.Forms.TabPage();
            this.btnBrowseRefresh = new System.Windows.Forms.Button();
            this.lvwBrowser = new System.Windows.Forms.ListView();
            this.ColumnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader12 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader13 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader14 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ContextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.SetAsCompare1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetAsCompare2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetAsCompare3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SetAsCompare4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.DeleteFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Label8 = new System.Windows.Forms.Label();
            this.PictureBox1 = new System.Windows.Forms.PictureBox();
            this.TabControl1.SuspendLayout();
            this.TabPage1.SuspendLayout();
            this.ContextMenuStrip1.SuspendLayout();
            this.TabPage2.SuspendLayout();
            this.TabPage3.SuspendLayout();
            this.TabPage4.SuspendLayout();
            this.TabPage5.SuspendLayout();
            this.TabPage6.SuspendLayout();
            this.ContextMenuStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // TabControl1
            // 
            this.TabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl1.Controls.Add(this.TabPage1);
            this.TabControl1.Controls.Add(this.TabPage2);
            this.TabControl1.Controls.Add(this.TabPage3);
            this.TabControl1.Controls.Add(this.TabPage4);
            this.TabControl1.Controls.Add(this.TabPage5);
            this.TabControl1.Controls.Add(this.TabPage6);
            this.TabControl1.Location = new System.Drawing.Point(16, 15);
            this.TabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.TabControl1.Name = "TabControl1";
            this.TabControl1.SelectedIndex = 0;
            this.TabControl1.Size = new System.Drawing.Size(999, 591);
            this.TabControl1.TabIndex = 0;
            // 
            // TabPage1
            // 
            this.TabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage1.Controls.Add(this.chkShowOnlyMismatches);
            this.TabPage1.Controls.Add(this.chkCompareShowNames);
            this.TabPage1.Controls.Add(this.lblComp4VIN);
            this.TabPage1.Controls.Add(this.lblComp3VIN);
            this.TabPage1.Controls.Add(this.lblComp2VIN);
            this.TabPage1.Controls.Add(this.lblComp1VIN);
            this.TabPage1.Controls.Add(this.Button9);
            this.TabPage1.Controls.Add(this.tbxCompFile4);
            this.TabPage1.Controls.Add(this.Label23);
            this.TabPage1.Controls.Add(this.Button7);
            this.TabPage1.Controls.Add(this.tbxCompFile3);
            this.TabPage1.Controls.Add(this.Label22);
            this.TabPage1.Controls.Add(this.chkCompareShowChecksum);
            this.TabPage1.Controls.Add(this.btnCompLoad);
            this.TabPage1.Controls.Add(this.btnCompBrowse2);
            this.TabPage1.Controls.Add(this.btnCompBrowse1);
            this.TabPage1.Controls.Add(this.tbxCompFile2);
            this.TabPage1.Controls.Add(this.tbxCompFile1);
            this.TabPage1.Controls.Add(this.ListView1);
            this.TabPage1.Controls.Add(this.Label7);
            this.TabPage1.Controls.Add(this.Label6);
            this.TabPage1.Location = new System.Drawing.Point(4, 25);
            this.TabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage1.Name = "TabPage1";
            this.TabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage1.Size = new System.Drawing.Size(991, 562);
            this.TabPage1.TabIndex = 0;
            this.TabPage1.Text = "Compare As-Built";
            this.TabPage1.Click += new System.EventHandler(this.TabPage1_Click);
            // 
            // chkShowOnlyMismatches
            // 
            this.chkShowOnlyMismatches.AutoSize = true;
            this.chkShowOnlyMismatches.Location = new System.Drawing.Point(167, 206);
            this.chkShowOnlyMismatches.Name = "chkShowOnlyMismatches";
            this.chkShowOnlyMismatches.Size = new System.Drawing.Size(222, 20);
            this.chkShowOnlyMismatches.TabIndex = 20;
            this.chkShowOnlyMismatches.Text = "Only Show Non-Matching Groups";
            this.chkShowOnlyMismatches.UseVisualStyleBackColor = true;
            this.chkShowOnlyMismatches.CheckedChanged += new System.EventHandler(this.chkShowOnlyMismatches_CheckedChanged);
            // 
            // chkCompareShowNames
            // 
            this.chkCompareShowNames.AutoSize = true;
            this.chkCompareShowNames.Checked = true;
            this.chkCompareShowNames.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCompareShowNames.Location = new System.Drawing.Point(167, 180);
            this.chkCompareShowNames.Name = "chkCompareShowNames";
            this.chkCompareShowNames.Size = new System.Drawing.Size(154, 20);
            this.chkCompareShowNames.TabIndex = 19;
            this.chkCompareShowNames.Text = "Show Module Names";
            this.chkCompareShowNames.UseVisualStyleBackColor = true;
            this.chkCompareShowNames.CheckedChanged += new System.EventHandler(this.chkCompareShowNames_CheckedChanged);
            // 
            // lblComp4VIN
            // 
            this.lblComp4VIN.AutoSize = true;
            this.lblComp4VIN.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lblComp4VIN.Location = new System.Drawing.Point(642, 116);
            this.lblComp4VIN.Name = "lblComp4VIN";
            this.lblComp4VIN.Size = new System.Drawing.Size(50, 16);
            this.lblComp4VIN.TabIndex = 18;
            this.lblComp4VIN.Text = "[no file]";
            // 
            // lblComp3VIN
            // 
            this.lblComp3VIN.AutoSize = true;
            this.lblComp3VIN.ForeColor = System.Drawing.Color.BlueViolet;
            this.lblComp3VIN.Location = new System.Drawing.Point(642, 85);
            this.lblComp3VIN.Name = "lblComp3VIN";
            this.lblComp3VIN.Size = new System.Drawing.Size(50, 16);
            this.lblComp3VIN.TabIndex = 17;
            this.lblComp3VIN.Text = "[no file]";
            // 
            // lblComp2VIN
            // 
            this.lblComp2VIN.AutoSize = true;
            this.lblComp2VIN.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblComp2VIN.Location = new System.Drawing.Point(642, 54);
            this.lblComp2VIN.Name = "lblComp2VIN";
            this.lblComp2VIN.Size = new System.Drawing.Size(50, 16);
            this.lblComp2VIN.TabIndex = 16;
            this.lblComp2VIN.Text = "[no file]";
            // 
            // lblComp1VIN
            // 
            this.lblComp1VIN.AutoSize = true;
            this.lblComp1VIN.ForeColor = System.Drawing.Color.Blue;
            this.lblComp1VIN.Location = new System.Drawing.Point(642, 23);
            this.lblComp1VIN.Name = "lblComp1VIN";
            this.lblComp1VIN.Size = new System.Drawing.Size(50, 16);
            this.lblComp1VIN.TabIndex = 15;
            this.lblComp1VIN.Text = "[no file]";
            // 
            // Button9
            // 
            this.Button9.Location = new System.Drawing.Point(538, 112);
            this.Button9.Name = "Button9";
            this.Button9.Size = new System.Drawing.Size(98, 27);
            this.Button9.TabIndex = 14;
            this.Button9.Text = "Browse...";
            this.Button9.UseVisualStyleBackColor = true;
            // 
            // tbxCompFile4
            // 
            this.tbxCompFile4.ForeColor = System.Drawing.Color.SaddleBrown;
            this.tbxCompFile4.Location = new System.Drawing.Point(167, 113);
            this.tbxCompFile4.Name = "tbxCompFile4";
            this.tbxCompFile4.Size = new System.Drawing.Size(343, 22);
            this.tbxCompFile4.TabIndex = 13;
            // 
            // Label23
            // 
            this.Label23.AutoSize = true;
            this.Label23.ForeColor = System.Drawing.Color.SaddleBrown;
            this.Label23.Location = new System.Drawing.Point(24, 116);
            this.Label23.Name = "Label23";
            this.Label23.Size = new System.Drawing.Size(100, 16);
            this.Label23.TabIndex = 12;
            this.Label23.Text = "As-Built File # 4:";
            // 
            // Button7
            // 
            this.Button7.Location = new System.Drawing.Point(538, 81);
            this.Button7.Name = "Button7";
            this.Button7.Size = new System.Drawing.Size(98, 27);
            this.Button7.TabIndex = 11;
            this.Button7.Text = "Browse...";
            this.Button7.UseVisualStyleBackColor = true;
            // 
            // tbxCompFile3
            // 
            this.tbxCompFile3.ForeColor = System.Drawing.Color.BlueViolet;
            this.tbxCompFile3.Location = new System.Drawing.Point(167, 82);
            this.tbxCompFile3.Name = "tbxCompFile3";
            this.tbxCompFile3.Size = new System.Drawing.Size(343, 22);
            this.tbxCompFile3.TabIndex = 10;
            // 
            // Label22
            // 
            this.Label22.AutoSize = true;
            this.Label22.ForeColor = System.Drawing.Color.BlueViolet;
            this.Label22.Location = new System.Drawing.Point(24, 85);
            this.Label22.Name = "Label22";
            this.Label22.Size = new System.Drawing.Size(100, 16);
            this.Label22.TabIndex = 9;
            this.Label22.Text = "As-Built File # 3:";
            // 
            // chkCompareShowChecksum
            // 
            this.chkCompareShowChecksum.AutoSize = true;
            this.chkCompareShowChecksum.Location = new System.Drawing.Point(167, 154);
            this.chkCompareShowChecksum.Name = "chkCompareShowChecksum";
            this.chkCompareShowChecksum.Size = new System.Drawing.Size(159, 20);
            this.chkCompareShowChecksum.TabIndex = 8;
            this.chkCompareShowChecksum.Text = "Include Checksum bits";
            this.chkCompareShowChecksum.UseVisualStyleBackColor = true;
            // 
            // btnCompLoad
            // 
            this.btnCompLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCompLoad.Location = new System.Drawing.Point(523, 145);
            this.btnCompLoad.Name = "btnCompLoad";
            this.btnCompLoad.Size = new System.Drawing.Size(113, 59);
            this.btnCompLoad.TabIndex = 7;
            this.btnCompLoad.Text = "Load Files";
            this.btnCompLoad.UseVisualStyleBackColor = true;
            this.btnCompLoad.Click += new System.EventHandler(this.Button2_Click);
            // 
            // btnCompBrowse2
            // 
            this.btnCompBrowse2.Location = new System.Drawing.Point(538, 50);
            this.btnCompBrowse2.Name = "btnCompBrowse2";
            this.btnCompBrowse2.Size = new System.Drawing.Size(98, 27);
            this.btnCompBrowse2.TabIndex = 6;
            this.btnCompBrowse2.Text = "Browse...";
            this.btnCompBrowse2.UseVisualStyleBackColor = true;
            this.btnCompBrowse2.Click += new System.EventHandler(this.btnCompBrowse2_Click);
            // 
            // btnCompBrowse1
            // 
            this.btnCompBrowse1.Location = new System.Drawing.Point(538, 19);
            this.btnCompBrowse1.Name = "btnCompBrowse1";
            this.btnCompBrowse1.Size = new System.Drawing.Size(98, 27);
            this.btnCompBrowse1.TabIndex = 5;
            this.btnCompBrowse1.Text = "Browse...";
            this.btnCompBrowse1.UseVisualStyleBackColor = true;
            this.btnCompBrowse1.Click += new System.EventHandler(this.btnCompBrowse1_Click);
            // 
            // tbxCompFile2
            // 
            this.tbxCompFile2.ForeColor = System.Drawing.Color.DarkGreen;
            this.tbxCompFile2.Location = new System.Drawing.Point(167, 51);
            this.tbxCompFile2.Name = "tbxCompFile2";
            this.tbxCompFile2.Size = new System.Drawing.Size(343, 22);
            this.tbxCompFile2.TabIndex = 4;
            // 
            // tbxCompFile1
            // 
            this.tbxCompFile1.ForeColor = System.Drawing.Color.Blue;
            this.tbxCompFile1.Location = new System.Drawing.Point(167, 20);
            this.tbxCompFile1.Name = "tbxCompFile1";
            this.tbxCompFile1.Size = new System.Drawing.Size(343, 22);
            this.tbxCompFile1.TabIndex = 3;
            // 
            // ListView1
            // 
            this.ListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2,
            this.ColumnHeader3,
            this.ColumnHeader4,
            this.ColumnHeader5,
            this.ColumnHeader6,
            this.ColumnHeader7,
            this.ColumnHeader8,
            this.ColumnHeader9});
            this.ListView1.ContextMenuStrip = this.ContextMenuStrip1;
            this.ListView1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListView1.FullRowSelect = true;
            this.ListView1.HideSelection = false;
            this.ListView1.Location = new System.Drawing.Point(17, 243);
            this.ListView1.Name = "ListView1";
            this.ListView1.Size = new System.Drawing.Size(956, 294);
            this.ListView1.TabIndex = 2;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            this.ListView1.SelectedIndexChanged += new System.EventHandler(this.ListView1_SelectedIndexChanged);
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Module Address";
            this.ColumnHeader1.Width = 128;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Data1";
            this.ColumnHeader2.Width = 81;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Data2";
            this.ColumnHeader3.Width = 87;
            // 
            // ColumnHeader4
            // 
            this.ColumnHeader4.Text = "Data3";
            this.ColumnHeader4.Width = 92;
            // 
            // ColumnHeader5
            // 
            this.ColumnHeader5.Text = "Same?";
            this.ColumnHeader5.Width = 76;
            // 
            // ColumnHeader6
            // 
            this.ColumnHeader6.Text = "Binary";
            this.ColumnHeader6.Width = 473;
            // 
            // ColumnHeader7
            // 
            this.ColumnHeader7.Text = "PartNumber";
            // 
            // ColumnHeader8
            // 
            this.ColumnHeader8.Text = "Strategy";
            this.ColumnHeader8.Width = 158;
            // 
            // ColumnHeader9
            // 
            this.ColumnHeader9.Text = "Calibration";
            this.ColumnHeader9.Width = 158;
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CopyToolStripMenuItem,
            this.ExportModuleToolStripMenuItem});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(152, 48);
            this.ContextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip1_Opening);
            // 
            // CopyToolStripMenuItem
            // 
            this.CopyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.EntireLineToolStripMenuItem,
            this.Data1hexToolStripMenuItem,
            this.Data2hexToolStripMenuItem,
            this.Data3hexToolStripMenuItem,
            this.BinaryToolStripMenuItem});
            this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
            this.CopyToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.CopyToolStripMenuItem.Text = "Copy";
            // 
            // EntireLineToolStripMenuItem
            // 
            this.EntireLineToolStripMenuItem.Name = "EntireLineToolStripMenuItem";
            this.EntireLineToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.EntireLineToolStripMenuItem.Text = "Entire Line";
            // 
            // Data1hexToolStripMenuItem
            // 
            this.Data1hexToolStripMenuItem.Name = "Data1hexToolStripMenuItem";
            this.Data1hexToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.Data1hexToolStripMenuItem.Text = "Data 1 (hex)";
            // 
            // Data2hexToolStripMenuItem
            // 
            this.Data2hexToolStripMenuItem.Name = "Data2hexToolStripMenuItem";
            this.Data2hexToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.Data2hexToolStripMenuItem.Text = "Data 2 (hex)";
            // 
            // Data3hexToolStripMenuItem
            // 
            this.Data3hexToolStripMenuItem.Name = "Data3hexToolStripMenuItem";
            this.Data3hexToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.Data3hexToolStripMenuItem.Text = "Data 3 (hex)";
            // 
            // BinaryToolStripMenuItem
            // 
            this.BinaryToolStripMenuItem.Name = "BinaryToolStripMenuItem";
            this.BinaryToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.BinaryToolStripMenuItem.Text = "Binary";
            // 
            // ExportModuleToolStripMenuItem
            // 
            this.ExportModuleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToUCDSToolStripMenuItem,
            this.ToABTToolStripMenuItem});
            this.ExportModuleToolStripMenuItem.Name = "ExportModuleToolStripMenuItem";
            this.ExportModuleToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.ExportModuleToolStripMenuItem.Text = "Export Module";
            this.ExportModuleToolStripMenuItem.DropDownOpening += new System.EventHandler(this.ExportModuleToolStripMenuItem_DropDownOpening);
            this.ExportModuleToolStripMenuItem.Click += new System.EventHandler(this.ExportModuleToolStripMenuItem_Click);
            // 
            // ToUCDSToolStripMenuItem
            // 
            this.ToUCDSToolStripMenuItem.Name = "ToUCDSToolStripMenuItem";
            this.ToUCDSToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.ToUCDSToolStripMenuItem.Text = "To UCDS...";
            this.ToUCDSToolStripMenuItem.Click += new System.EventHandler(this.ToUCDSToolStripMenuItem_Click);
            // 
            // ToABTToolStripMenuItem
            // 
            this.ToABTToolStripMenuItem.Name = "ToABTToolStripMenuItem";
            this.ToABTToolStripMenuItem.Size = new System.Drawing.Size(129, 22);
            this.ToABTToolStripMenuItem.Text = "To ABT...";
            this.ToABTToolStripMenuItem.Click += new System.EventHandler(this.ToABTToolStripMenuItem_Click);
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.ForeColor = System.Drawing.Color.DarkGreen;
            this.Label7.Location = new System.Drawing.Point(24, 54);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(100, 16);
            this.Label7.TabIndex = 1;
            this.Label7.Text = "As-Built File # 2:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.ForeColor = System.Drawing.Color.Blue;
            this.Label6.Location = new System.Drawing.Point(24, 23);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(100, 16);
            this.Label6.TabIndex = 0;
            this.Label6.Text = "As-Built File # 1:";
            // 
            // TabPage2
            // 
            this.TabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage2.Controls.Add(this.TextBox5);
            this.TabPage2.Controls.Add(this.Button3);
            this.TabPage2.Controls.Add(this.Button2);
            this.TabPage2.Controls.Add(this.Label16);
            this.TabPage2.Controls.Add(this.Label15);
            this.TabPage2.Controls.Add(this.tbxChecksumBin);
            this.TabPage2.Controls.Add(this.tbxConvertBin);
            this.TabPage2.Controls.Add(this.tbxConvertHex);
            this.TabPage2.Controls.Add(this.tbxData3bin2);
            this.TabPage2.Controls.Add(this.tbxData2bin2);
            this.TabPage2.Controls.Add(this.tbxData1bin2);
            this.TabPage2.Controls.Add(this.tbxData3bin1);
            this.TabPage2.Controls.Add(this.tbxData2bin1);
            this.TabPage2.Controls.Add(this.tbxData1bin1);
            this.TabPage2.Controls.Add(this.tbxChecksumHex);
            this.TabPage2.Controls.Add(this.Label5);
            this.TabPage2.Controls.Add(this.Button1);
            this.TabPage2.Controls.Add(this.tbxData3hex);
            this.TabPage2.Controls.Add(this.Label4);
            this.TabPage2.Controls.Add(this.tbxData2hex);
            this.TabPage2.Controls.Add(this.Label3);
            this.TabPage2.Controls.Add(this.tbxData1hex);
            this.TabPage2.Controls.Add(this.Label2);
            this.TabPage2.Controls.Add(this.tbxModIDhex);
            this.TabPage2.Controls.Add(this.Label1);
            this.TabPage2.Location = new System.Drawing.Point(4, 25);
            this.TabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.TabPage2.Name = "TabPage2";
            this.TabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.TabPage2.Size = new System.Drawing.Size(991, 562);
            this.TabPage2.TabIndex = 1;
            this.TabPage2.Text = "Checksum Calc";
            this.TabPage2.Click += new System.EventHandler(this.TabPage2_Click);
            // 
            // TextBox5
            // 
            this.TextBox5.BackColor = System.Drawing.SystemColors.Info;
            this.TextBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox5.Location = new System.Drawing.Point(485, 22);
            this.TextBox5.Multiline = true;
            this.TextBox5.Name = "TextBox5";
            this.TextBox5.Size = new System.Drawing.Size(173, 215);
            this.TextBox5.TabIndex = 24;
            this.TextBox5.Text = "This is just a checksum calculator. Entire your module data (including the checks" +
    "um). \r\n\r\nClick Re-Calc, and a new checksum will be provided below.";
            // 
            // Button3
            // 
            this.Button3.Location = new System.Drawing.Point(86, 408);
            this.Button3.Name = "Button3";
            this.Button3.Size = new System.Drawing.Size(100, 27);
            this.Button3.TabIndex = 23;
            this.Button3.Text = "Bin to Hex";
            this.Button3.UseVisualStyleBackColor = true;
            // 
            // Button2
            // 
            this.Button2.Location = new System.Drawing.Point(86, 319);
            this.Button2.Name = "Button2";
            this.Button2.Size = new System.Drawing.Size(100, 27);
            this.Button2.TabIndex = 22;
            this.Button2.Text = "Hex to Bin";
            this.Button2.UseVisualStyleBackColor = true;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Location = new System.Drawing.Point(33, 383);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(29, 16);
            this.Label16.TabIndex = 21;
            this.Label16.Text = "Bin:";
            // 
            // Label15
            // 
            this.Label15.AutoSize = true;
            this.Label15.Location = new System.Drawing.Point(33, 355);
            this.Label15.Name = "Label15";
            this.Label15.Size = new System.Drawing.Size(34, 16);
            this.Label15.TabIndex = 20;
            this.Label15.Text = "Hex:";
            // 
            // tbxChecksumBin
            // 
            this.tbxChecksumBin.BackColor = System.Drawing.SystemColors.Control;
            this.tbxChecksumBin.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxChecksumBin.Location = new System.Drawing.Point(280, 219);
            this.tbxChecksumBin.Margin = new System.Windows.Forms.Padding(4);
            this.tbxChecksumBin.MaxLength = 8;
            this.tbxChecksumBin.Name = "tbxChecksumBin";
            this.tbxChecksumBin.Size = new System.Drawing.Size(89, 22);
            this.tbxChecksumBin.TabIndex = 19;
            this.tbxChecksumBin.Text = "00011111";
            // 
            // tbxConvertBin
            // 
            this.tbxConvertBin.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxConvertBin.Location = new System.Drawing.Point(86, 380);
            this.tbxConvertBin.Name = "tbxConvertBin";
            this.tbxConvertBin.Size = new System.Drawing.Size(170, 22);
            this.tbxConvertBin.TabIndex = 18;
            // 
            // tbxConvertHex
            // 
            this.tbxConvertHex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxConvertHex.Location = new System.Drawing.Point(86, 352);
            this.tbxConvertHex.Name = "tbxConvertHex";
            this.tbxConvertHex.Size = new System.Drawing.Size(100, 22);
            this.tbxConvertHex.TabIndex = 17;
            // 
            // tbxData3bin2
            // 
            this.tbxData3bin2.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData3bin2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData3bin2.Location = new System.Drawing.Point(326, 117);
            this.tbxData3bin2.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData3bin2.MaxLength = 8;
            this.tbxData3bin2.Name = "tbxData3bin2";
            this.tbxData3bin2.Size = new System.Drawing.Size(89, 22);
            this.tbxData3bin2.TabIndex = 16;
            this.tbxData3bin2.Text = "00011111";
            // 
            // tbxData2bin2
            // 
            this.tbxData2bin2.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData2bin2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData2bin2.Location = new System.Drawing.Point(326, 85);
            this.tbxData2bin2.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData2bin2.MaxLength = 8;
            this.tbxData2bin2.Name = "tbxData2bin2";
            this.tbxData2bin2.Size = new System.Drawing.Size(89, 22);
            this.tbxData2bin2.TabIndex = 15;
            this.tbxData2bin2.Text = "11110000";
            // 
            // tbxData1bin2
            // 
            this.tbxData1bin2.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData1bin2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData1bin2.Location = new System.Drawing.Point(326, 53);
            this.tbxData1bin2.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData1bin2.MaxLength = 8;
            this.tbxData1bin2.Name = "tbxData1bin2";
            this.tbxData1bin2.Size = new System.Drawing.Size(89, 22);
            this.tbxData1bin2.TabIndex = 14;
            this.tbxData1bin2.Text = "11111100";
            // 
            // tbxData3bin1
            // 
            this.tbxData3bin1.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData3bin1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData3bin1.Location = new System.Drawing.Point(229, 117);
            this.tbxData3bin1.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData3bin1.MaxLength = 8;
            this.tbxData3bin1.Name = "tbxData3bin1";
            this.tbxData3bin1.Size = new System.Drawing.Size(89, 22);
            this.tbxData3bin1.TabIndex = 13;
            this.tbxData3bin1.Text = "00111111";
            // 
            // tbxData2bin1
            // 
            this.tbxData2bin1.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData2bin1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData2bin1.Location = new System.Drawing.Point(229, 85);
            this.tbxData2bin1.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData2bin1.MaxLength = 8;
            this.tbxData2bin1.Name = "tbxData2bin1";
            this.tbxData2bin1.Size = new System.Drawing.Size(89, 22);
            this.tbxData2bin1.TabIndex = 12;
            this.tbxData2bin1.Text = "01000000";
            // 
            // tbxData1bin1
            // 
            this.tbxData1bin1.BackColor = System.Drawing.SystemColors.Control;
            this.tbxData1bin1.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData1bin1.Location = new System.Drawing.Point(229, 53);
            this.tbxData1bin1.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData1bin1.MaxLength = 8;
            this.tbxData1bin1.Name = "tbxData1bin1";
            this.tbxData1bin1.Size = new System.Drawing.Size(89, 22);
            this.tbxData1bin1.TabIndex = 11;
            this.tbxData1bin1.Text = "10001010";
            // 
            // tbxChecksumHex
            // 
            this.tbxChecksumHex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxChecksumHex.Location = new System.Drawing.Point(143, 219);
            this.tbxChecksumHex.Margin = new System.Windows.Forms.Padding(4);
            this.tbxChecksumHex.MaxLength = 2;
            this.tbxChecksumHex.Name = "tbxChecksumHex";
            this.tbxChecksumHex.Size = new System.Drawing.Size(43, 22);
            this.tbxChecksumHex.TabIndex = 10;
            this.tbxChecksumHex.Text = "1F";
            this.tbxChecksumHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbxChecksumHex.TextChanged += new System.EventHandler(this.tbxChecksumHex_TextChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(28, 221);
            this.Label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(73, 16);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "Checksum:";
            // 
            // Button1
            // 
            this.Button1.Location = new System.Drawing.Point(30, 157);
            this.Button1.Margin = new System.Windows.Forms.Padding(4);
            this.Button1.Name = "Button1";
            this.Button1.Size = new System.Drawing.Size(268, 41);
            this.Button1.TabIndex = 8;
            this.Button1.Text = "Re-Calc Checksum (last byte)";
            this.Button1.UseVisualStyleBackColor = true;
            this.Button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // tbxData3hex
            // 
            this.tbxData3hex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData3hex.Location = new System.Drawing.Point(118, 117);
            this.tbxData3hex.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData3hex.MaxLength = 5;
            this.tbxData3hex.Name = "tbxData3hex";
            this.tbxData3hex.Size = new System.Drawing.Size(68, 22);
            this.tbxData3hex.TabIndex = 7;
            this.tbxData3hex.Text = "3F1F";
            this.tbxData3hex.TextChanged += new System.EventHandler(this.tbxData3hex_TextChanged);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(27, 119);
            this.Label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(39, 16);
            this.Label4.TabIndex = 6;
            this.Label4.Text = "Data:";
            // 
            // tbxData2hex
            // 
            this.tbxData2hex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData2hex.Location = new System.Drawing.Point(118, 85);
            this.tbxData2hex.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData2hex.MaxLength = 5;
            this.tbxData2hex.Name = "tbxData2hex";
            this.tbxData2hex.Size = new System.Drawing.Size(68, 22);
            this.tbxData2hex.TabIndex = 5;
            this.tbxData2hex.Text = "40F0";
            this.tbxData2hex.TextChanged += new System.EventHandler(this.TextBox3_TextChanged);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(27, 87);
            this.Label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(39, 16);
            this.Label3.TabIndex = 4;
            this.Label3.Text = "Data:";
            this.Label3.Click += new System.EventHandler(this.Label3_Click);
            // 
            // tbxData1hex
            // 
            this.tbxData1hex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxData1hex.Location = new System.Drawing.Point(118, 53);
            this.tbxData1hex.Margin = new System.Windows.Forms.Padding(4);
            this.tbxData1hex.MaxLength = 5;
            this.tbxData1hex.Name = "tbxData1hex";
            this.tbxData1hex.Size = new System.Drawing.Size(68, 22);
            this.tbxData1hex.TabIndex = 3;
            this.tbxData1hex.Text = "8AFC";
            this.tbxData1hex.TextChanged += new System.EventHandler(this.tbxData1hex_TextChanged);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(27, 55);
            this.Label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(39, 16);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Data:";
            // 
            // tbxModIDhex
            // 
            this.tbxModIDhex.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbxModIDhex.Location = new System.Drawing.Point(118, 23);
            this.tbxModIDhex.Margin = new System.Windows.Forms.Padding(4);
            this.tbxModIDhex.MaxLength = 10;
            this.tbxModIDhex.Name = "tbxModIDhex";
            this.tbxModIDhex.Size = new System.Drawing.Size(102, 22);
            this.tbxModIDhex.TabIndex = 1;
            this.tbxModIDhex.Text = "720-01-02";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(27, 25);
            this.Label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(71, 16);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Module ID:";
            // 
            // TabPage3
            // 
            this.TabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage3.Controls.Add(this.TextBox6);
            this.TabPage3.Controls.Add(this.TextBox2);
            this.TabPage3.Controls.Add(this.Label12);
            this.TabPage3.Controls.Add(this.Label9);
            this.TabPage3.Controls.Add(this.chkDeduceDownloadAB);
            this.TabPage3.Controls.Add(this.btnDeduceSaveInfo);
            this.TabPage3.Controls.Add(this.wbDeducer);
            this.TabPage3.Controls.Add(this.btnDeduceOpenETIS);
            this.TabPage3.Location = new System.Drawing.Point(4, 25);
            this.TabPage3.Name = "TabPage3";
            this.TabPage3.Size = new System.Drawing.Size(991, 562);
            this.TabPage3.TabIndex = 2;
            this.TabPage3.Text = "Deducer - Get Data";
            // 
            // TextBox6
            // 
            this.TextBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox6.BackColor = System.Drawing.SystemColors.Info;
            this.TextBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox6.ForeColor = System.Drawing.Color.Red;
            this.TextBox6.Location = new System.Drawing.Point(596, 86);
            this.TextBox6.Multiline = true;
            this.TextBox6.Name = "TextBox6";
            this.TextBox6.Size = new System.Drawing.Size(199, 124);
            this.TextBox6.TabIndex = 10;
            this.TextBox6.Text = "The Deducer tool is now unusable, due to the ETIS site being shut down. Unless th" +
    "ere is another site to correlate VIN to vehicle features, this will remain broke" +
    "n.";
            // 
            // TextBox2
            // 
            this.TextBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TextBox2.BackColor = System.Drawing.SystemColors.Info;
            this.TextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox2.Location = new System.Drawing.Point(596, 232);
            this.TextBox2.Multiline = true;
            this.TextBox2.Name = "TextBox2";
            this.TextBox2.Size = new System.Drawing.Size(199, 308);
            this.TextBox2.TabIndex = 9;
            this.TextBox2.Text = resources.GetString("TextBox2.Text");
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Location = new System.Drawing.Point(214, 48);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(456, 16);
            this.Label12.TabIndex = 2;
            this.Label12.Text = "When prompted, save the .AB file in the \"Deducer\" subfolder of this program.";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(18, 48);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(138, 16);
            this.Label9.TabIndex = 8;
            this.Label9.Text = "Search the VIN below:";
            // 
            // chkDeduceDownloadAB
            // 
            this.chkDeduceDownloadAB.AutoSize = true;
            this.chkDeduceDownloadAB.Checked = true;
            this.chkDeduceDownloadAB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeduceDownloadAB.Enabled = false;
            this.chkDeduceDownloadAB.Location = new System.Drawing.Point(340, 21);
            this.chkDeduceDownloadAB.Name = "chkDeduceDownloadAB";
            this.chkDeduceDownloadAB.Size = new System.Drawing.Size(111, 20);
            this.chkDeduceDownloadAB.TabIndex = 7;
            this.chkDeduceDownloadAB.Text = "Download .AB";
            this.chkDeduceDownloadAB.UseVisualStyleBackColor = true;
            this.chkDeduceDownloadAB.Visible = false;
            // 
            // btnDeduceSaveInfo
            // 
            this.btnDeduceSaveInfo.Location = new System.Drawing.Point(208, 17);
            this.btnDeduceSaveInfo.Name = "btnDeduceSaveInfo";
            this.btnDeduceSaveInfo.Size = new System.Drawing.Size(96, 26);
            this.btnDeduceSaveInfo.TabIndex = 5;
            this.btnDeduceSaveInfo.Text = "Save Info";
            this.btnDeduceSaveInfo.UseVisualStyleBackColor = true;
            // 
            // wbDeducer
            // 
            this.wbDeducer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wbDeducer.Location = new System.Drawing.Point(15, 86);
            this.wbDeducer.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbDeducer.Name = "wbDeducer";
            this.wbDeducer.Size = new System.Drawing.Size(552, 451);
            this.wbDeducer.TabIndex = 4;
            this.wbDeducer.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbDeducer_DocumentCompleted);
            this.wbDeducer.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.wbDeducer_Navigating);
            // 
            // btnDeduceOpenETIS
            // 
            this.btnDeduceOpenETIS.Location = new System.Drawing.Point(15, 17);
            this.btnDeduceOpenETIS.Name = "btnDeduceOpenETIS";
            this.btnDeduceOpenETIS.Size = new System.Drawing.Size(106, 26);
            this.btnDeduceOpenETIS.TabIndex = 2;
            this.btnDeduceOpenETIS.Text = "Open ETIS";
            this.btnDeduceOpenETIS.UseVisualStyleBackColor = true;
            // 
            // TabPage4
            // 
            this.TabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage4.Controls.Add(this.chkDeduceDoCCC);
            this.TabPage4.Controls.Add(this.Button8);
            this.TabPage4.Controls.Add(this.Label14);
            this.TabPage4.Controls.Add(this.TextBox1);
            this.TabPage4.Controls.Add(this.Label13);
            this.TabPage4.Controls.Add(this.Label11);
            this.TabPage4.Controls.Add(this.Label10);
            this.TabPage4.Controls.Add(this.lstDeduceYears);
            this.TabPage4.Controls.Add(this.lstDeduceModels);
            this.TabPage4.Controls.Add(this.tbxDeduceReport);
            this.TabPage4.Controls.Add(this.btnDeduceFigureIt);
            this.TabPage4.Controls.Add(this.btnDeduceLoadOptions);
            this.TabPage4.Controls.Add(this.lstDeduceFactoryOptions);
            this.TabPage4.Location = new System.Drawing.Point(4, 22);
            this.TabPage4.Name = "TabPage4";
            this.TabPage4.Size = new System.Drawing.Size(991, 565);
            this.TabPage4.TabIndex = 3;
            this.TabPage4.Text = "Deducer - Feature by Vehicles";
            // 
            // chkDeduceDoCCC
            // 
            this.chkDeduceDoCCC.AutoSize = true;
            this.chkDeduceDoCCC.Location = new System.Drawing.Point(622, 20);
            this.chkDeduceDoCCC.Name = "chkDeduceDoCCC";
            this.chkDeduceDoCCC.Size = new System.Drawing.Size(180, 20);
            this.chkDeduceDoCCC.TabIndex = 12;
            this.chkDeduceDoCCC.Text = "Compare CCC bits as well";
            this.chkDeduceDoCCC.UseVisualStyleBackColor = true;
            // 
            // Button8
            // 
            this.Button8.Location = new System.Drawing.Point(622, 425);
            this.Button8.Name = "Button8";
            this.Button8.Size = new System.Drawing.Size(173, 27);
            this.Button8.TabIndex = 11;
            this.Button8.Text = "Try Every Option";
            this.Button8.UseVisualStyleBackColor = true;
            this.Button8.Visible = false;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Location = new System.Drawing.Point(12, 428);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(519, 16);
            this.Label14.TabIndex = 10;
            this.Label14.Text = "\"Perfect Bits\" are listed at the bottom. Bits are numbered from left to right sta" +
    "rting at zero.";
            // 
            // TextBox1
            // 
            this.TextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.TextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox1.Location = new System.Drawing.Point(622, 81);
            this.TextBox1.Multiline = true;
            this.TextBox1.Name = "TextBox1";
            this.TextBox1.Size = new System.Drawing.Size(173, 335);
            this.TextBox1.TabIndex = 9;
            this.TextBox1.Text = resources.GetString("TextBox1.Text");
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Location = new System.Drawing.Point(12, 212);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(133, 16);
            this.Label13.TabIndex = 8;
            this.Label13.Text = "Review results below";
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Location = new System.Drawing.Point(12, 140);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(161, 16);
            this.Label11.TabIndex = 7;
            this.Label11.Text = "Select Years and Models:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Location = new System.Drawing.Point(12, 69);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(155, 16);
            this.Label10.TabIndex = 6;
            this.Label10.Text = "Select feature to deduce:";
            // 
            // lstDeduceYears
            // 
            this.lstDeduceYears.FormattingEnabled = true;
            this.lstDeduceYears.IntegralHeight = false;
            this.lstDeduceYears.Location = new System.Drawing.Point(218, 126);
            this.lstDeduceYears.Name = "lstDeduceYears";
            this.lstDeduceYears.Size = new System.Drawing.Size(120, 72);
            this.lstDeduceYears.TabIndex = 5;
            // 
            // lstDeduceModels
            // 
            this.lstDeduceModels.FormattingEnabled = true;
            this.lstDeduceModels.IntegralHeight = false;
            this.lstDeduceModels.Location = new System.Drawing.Point(353, 126);
            this.lstDeduceModels.Name = "lstDeduceModels";
            this.lstDeduceModels.Size = new System.Drawing.Size(233, 72);
            this.lstDeduceModels.TabIndex = 4;
            // 
            // tbxDeduceReport
            // 
            this.tbxDeduceReport.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxDeduceReport.Location = new System.Drawing.Point(15, 235);
            this.tbxDeduceReport.Multiline = true;
            this.tbxDeduceReport.Name = "tbxDeduceReport";
            this.tbxDeduceReport.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeduceReport.Size = new System.Drawing.Size(588, 181);
            this.tbxDeduceReport.TabIndex = 3;
            // 
            // btnDeduceFigureIt
            // 
            this.btnDeduceFigureIt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeduceFigureIt.Location = new System.Drawing.Point(622, 47);
            this.btnDeduceFigureIt.Name = "btnDeduceFigureIt";
            this.btnDeduceFigureIt.Size = new System.Drawing.Size(173, 28);
            this.btnDeduceFigureIt.TabIndex = 2;
            this.btnDeduceFigureIt.Text = "Find Perfect Bits";
            this.btnDeduceFigureIt.UseVisualStyleBackColor = true;
            // 
            // btnDeduceLoadOptions
            // 
            this.btnDeduceLoadOptions.Location = new System.Drawing.Point(15, 20);
            this.btnDeduceLoadOptions.Name = "btnDeduceLoadOptions";
            this.btnDeduceLoadOptions.Size = new System.Drawing.Size(155, 28);
            this.btnDeduceLoadOptions.TabIndex = 1;
            this.btnDeduceLoadOptions.Text = "Load Stored Data";
            this.btnDeduceLoadOptions.UseVisualStyleBackColor = true;
            // 
            // lstDeduceFactoryOptions
            // 
            this.lstDeduceFactoryOptions.FormattingEnabled = true;
            this.lstDeduceFactoryOptions.ItemHeight = 16;
            this.lstDeduceFactoryOptions.Location = new System.Drawing.Point(218, 20);
            this.lstDeduceFactoryOptions.Name = "lstDeduceFactoryOptions";
            this.lstDeduceFactoryOptions.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstDeduceFactoryOptions.Size = new System.Drawing.Size(368, 84);
            this.lstDeduceFactoryOptions.TabIndex = 0;
            // 
            // TabPage5
            // 
            this.TabPage5.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage5.Controls.Add(this.Button10);
            this.TabPage5.Controls.Add(this.TextBox4);
            this.TabPage5.Controls.Add(this.Label21);
            this.TabPage5.Controls.Add(this.lstBit_Modules);
            this.TabPage5.Controls.Add(this.Button6);
            this.TabPage5.Controls.Add(this.Button5);
            this.TabPage5.Controls.Add(this.Label20);
            this.TabPage5.Controls.Add(this.Label19);
            this.TabPage5.Controls.Add(this.lstBit_Years);
            this.TabPage5.Controls.Add(this.lstBit_Models);
            this.TabPage5.Controls.Add(this.Label17);
            this.TabPage5.Controls.Add(this.TextBox3);
            this.TabPage5.Controls.Add(this.Label18);
            this.TabPage5.Controls.Add(this.tbxDeduceReport2);
            this.TabPage5.Controls.Add(this.Button4);
            this.TabPage5.Location = new System.Drawing.Point(4, 22);
            this.TabPage5.Name = "TabPage5";
            this.TabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage5.Size = new System.Drawing.Size(991, 565);
            this.TabPage5.TabIndex = 4;
            this.TabPage5.Text = "Deducer - Vehicle/Feature by Bit";
            // 
            // Button10
            // 
            this.Button10.Location = new System.Drawing.Point(630, 422);
            this.Button10.Name = "Button10";
            this.Button10.Size = new System.Drawing.Size(173, 28);
            this.Button10.TabIndex = 31;
            this.Button10.Text = "Analyze All bits";
            this.Button10.UseVisualStyleBackColor = true;
            this.Button10.Click += new System.EventHandler(this.Button10_Click_1);
            // 
            // TextBox4
            // 
            this.TextBox4.Location = new System.Drawing.Point(226, 253);
            this.TextBox4.Name = "TextBox4";
            this.TextBox4.Size = new System.Drawing.Size(76, 22);
            this.TextBox4.TabIndex = 30;
            this.TextBox4.Text = "0";
            this.TextBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Label21
            // 
            this.Label21.AutoSize = true;
            this.Label21.Location = new System.Drawing.Point(20, 256);
            this.Label21.Name = "Label21";
            this.Label21.Size = new System.Drawing.Size(150, 16);
            this.Label21.TabIndex = 29;
            this.Label21.Text = "Select Bit Index (0 to 39):";
            // 
            // lstBit_Modules
            // 
            this.lstBit_Modules.FormattingEnabled = true;
            this.lstBit_Modules.ItemHeight = 16;
            this.lstBit_Modules.Location = new System.Drawing.Point(226, 152);
            this.lstBit_Modules.Name = "lstBit_Modules";
            this.lstBit_Modules.Size = new System.Drawing.Size(365, 84);
            this.lstBit_Modules.TabIndex = 28;
            // 
            // Button6
            // 
            this.Button6.Location = new System.Drawing.Point(23, 152);
            this.Button6.Name = "Button6";
            this.Button6.Size = new System.Drawing.Size(155, 28);
            this.Button6.TabIndex = 27;
            this.Button6.Text = "Load Modules";
            this.Button6.UseVisualStyleBackColor = true;
            // 
            // Button5
            // 
            this.Button5.Location = new System.Drawing.Point(23, 23);
            this.Button5.Name = "Button5";
            this.Button5.Size = new System.Drawing.Size(155, 28);
            this.Button5.TabIndex = 26;
            this.Button5.Text = "Load Stored Data";
            this.Button5.UseVisualStyleBackColor = true;
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Location = new System.Drawing.Point(20, 199);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(96, 16);
            this.Label20.TabIndex = 25;
            this.Label20.Text = "Select Module:";
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Location = new System.Drawing.Point(20, 72);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(161, 16);
            this.Label19.TabIndex = 24;
            this.Label19.Text = "Select Years and Models:";
            // 
            // lstBit_Years
            // 
            this.lstBit_Years.FormattingEnabled = true;
            this.lstBit_Years.IntegralHeight = false;
            this.lstBit_Years.Location = new System.Drawing.Point(226, 23);
            this.lstBit_Years.Name = "lstBit_Years";
            this.lstBit_Years.Size = new System.Drawing.Size(120, 101);
            this.lstBit_Years.TabIndex = 23;
            // 
            // lstBit_Models
            // 
            this.lstBit_Models.FormattingEnabled = true;
            this.lstBit_Models.IntegralHeight = false;
            this.lstBit_Models.Location = new System.Drawing.Point(358, 23);
            this.lstBit_Models.Name = "lstBit_Models";
            this.lstBit_Models.Size = new System.Drawing.Size(233, 101);
            this.lstBit_Models.TabIndex = 22;
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Location = new System.Drawing.Point(20, 495);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(519, 16);
            this.Label17.TabIndex = 21;
            this.Label17.Text = "\"Perfect Bits\" are listed at the bottom. Bits are numbered from left to right sta" +
    "rting at zero.";
            // 
            // TextBox3
            // 
            this.TextBox3.BackColor = System.Drawing.SystemColors.Info;
            this.TextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox3.Location = new System.Drawing.Point(630, 72);
            this.TextBox3.Multiline = true;
            this.TextBox3.Name = "TextBox3";
            this.TextBox3.Size = new System.Drawing.Size(173, 344);
            this.TextBox3.TabIndex = 20;
            this.TextBox3.Text = "This tool attempts to deduce which feature(s) corresponds to the chosen module bi" +
    "t. \r\n\r\nThis is a statistical analysis and only identifies consistencies -- not g" +
    "uarantees.";
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Location = new System.Drawing.Point(20, 320);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(133, 16);
            this.Label18.TabIndex = 19;
            this.Label18.Text = "Review results below";
            // 
            // tbxDeduceReport2
            // 
            this.tbxDeduceReport2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbxDeduceReport2.Location = new System.Drawing.Point(24, 343);
            this.tbxDeduceReport2.MaxLength = 5000000;
            this.tbxDeduceReport2.Multiline = true;
            this.tbxDeduceReport2.Name = "tbxDeduceReport2";
            this.tbxDeduceReport2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxDeduceReport2.Size = new System.Drawing.Size(588, 119);
            this.tbxDeduceReport2.TabIndex = 14;
            // 
            // Button4
            // 
            this.Button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button4.Location = new System.Drawing.Point(630, 23);
            this.Button4.Name = "Button4";
            this.Button4.Size = new System.Drawing.Size(173, 28);
            this.Button4.TabIndex = 13;
            this.Button4.Text = "Find Feature";
            this.Button4.UseVisualStyleBackColor = true;
            // 
            // TabPage6
            // 
            this.TabPage6.BackColor = System.Drawing.SystemColors.Control;
            this.TabPage6.Controls.Add(this.btnBrowseRefresh);
            this.TabPage6.Controls.Add(this.lvwBrowser);
            this.TabPage6.Location = new System.Drawing.Point(4, 22);
            this.TabPage6.Name = "TabPage6";
            this.TabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.TabPage6.Size = new System.Drawing.Size(991, 565);
            this.TabPage6.TabIndex = 5;
            this.TabPage6.Text = "AsBuilt Browser";
            // 
            // btnBrowseRefresh
            // 
            this.btnBrowseRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseRefresh.Location = new System.Drawing.Point(6, 13);
            this.btnBrowseRefresh.Name = "btnBrowseRefresh";
            this.btnBrowseRefresh.Size = new System.Drawing.Size(103, 32);
            this.btnBrowseRefresh.TabIndex = 1;
            this.btnBrowseRefresh.Text = "Refresh";
            this.btnBrowseRefresh.UseVisualStyleBackColor = true;
            // 
            // lvwBrowser
            // 
            this.lvwBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwBrowser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader10,
            this.ColumnHeader11,
            this.ColumnHeader12,
            this.ColumnHeader13,
            this.ColumnHeader14});
            this.lvwBrowser.ContextMenuStrip = this.ContextMenuStrip2;
            this.lvwBrowser.FullRowSelect = true;
            this.lvwBrowser.GridLines = true;
            this.lvwBrowser.HideSelection = false;
            this.lvwBrowser.Location = new System.Drawing.Point(6, 54);
            this.lvwBrowser.Name = "lvwBrowser";
            this.lvwBrowser.Size = new System.Drawing.Size(979, 495);
            this.lvwBrowser.TabIndex = 0;
            this.lvwBrowser.UseCompatibleStateImageBehavior = false;
            this.lvwBrowser.View = System.Windows.Forms.View.Details;
            this.lvwBrowser.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwBrowser_ColumnClick);
            // 
            // ColumnHeader10
            // 
            this.ColumnHeader10.Text = "Name";
            this.ColumnHeader10.Width = 250;
            // 
            // ColumnHeader11
            // 
            this.ColumnHeader11.Text = "Date";
            this.ColumnHeader11.Width = 150;
            // 
            // ColumnHeader12
            // 
            this.ColumnHeader12.Text = "Year";
            this.ColumnHeader12.Width = 70;
            // 
            // ColumnHeader13
            // 
            this.ColumnHeader13.Text = "Model";
            this.ColumnHeader13.Width = 220;
            // 
            // ColumnHeader14
            // 
            this.ColumnHeader14.Text = "VIN";
            this.ColumnHeader14.Width = 225;
            // 
            // ContextMenuStrip2
            // 
            this.ContextMenuStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SetAsCompare1ToolStripMenuItem,
            this.SetAsCompare2ToolStripMenuItem,
            this.SetAsCompare3ToolStripMenuItem,
            this.SetAsCompare4ToolStripMenuItem,
            this.ToolStripMenuItem1,
            this.DeleteFileToolStripMenuItem});
            this.ContextMenuStrip2.Name = "ContextMenuStrip2";
            this.ContextMenuStrip2.Size = new System.Drawing.Size(176, 120);
            this.ContextMenuStrip2.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip2_Opening);
            // 
            // SetAsCompare1ToolStripMenuItem
            // 
            this.SetAsCompare1ToolStripMenuItem.Name = "SetAsCompare1ToolStripMenuItem";
            this.SetAsCompare1ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.SetAsCompare1ToolStripMenuItem.Text = "Set as Compare # 1";
            this.SetAsCompare1ToolStripMenuItem.Click += new System.EventHandler(this.SetAsCompare1ToolStripMenuItem_Click);
            // 
            // SetAsCompare2ToolStripMenuItem
            // 
            this.SetAsCompare2ToolStripMenuItem.Name = "SetAsCompare2ToolStripMenuItem";
            this.SetAsCompare2ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.SetAsCompare2ToolStripMenuItem.Text = "Set as Compare # 2";
            this.SetAsCompare2ToolStripMenuItem.Click += new System.EventHandler(this.SetAsCompare2ToolStripMenuItem_Click);
            // 
            // SetAsCompare3ToolStripMenuItem
            // 
            this.SetAsCompare3ToolStripMenuItem.Name = "SetAsCompare3ToolStripMenuItem";
            this.SetAsCompare3ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.SetAsCompare3ToolStripMenuItem.Text = "Set as Compare # 3";
            this.SetAsCompare3ToolStripMenuItem.Click += new System.EventHandler(this.SetAsCompare3ToolStripMenuItem_Click);
            // 
            // SetAsCompare4ToolStripMenuItem
            // 
            this.SetAsCompare4ToolStripMenuItem.Name = "SetAsCompare4ToolStripMenuItem";
            this.SetAsCompare4ToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.SetAsCompare4ToolStripMenuItem.Text = "Set as Compare # 4";
            this.SetAsCompare4ToolStripMenuItem.Click += new System.EventHandler(this.SetAsCompare4ToolStripMenuItem_Click);
            // 
            // ToolStripMenuItem1
            // 
            this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
            this.ToolStripMenuItem1.Size = new System.Drawing.Size(172, 6);
            // 
            // DeleteFileToolStripMenuItem
            // 
            this.DeleteFileToolStripMenuItem.Name = "DeleteFileToolStripMenuItem";
            this.DeleteFileToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.DeleteFileToolStripMenuItem.Text = "Delete File";
            this.DeleteFileToolStripMenuItem.Click += new System.EventHandler(this.DeleteFileToolStripMenuItem_Click);
            // 
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.Label8.Location = new System.Drawing.Point(269, 622);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(446, 15);
            this.Label8.TabIndex = 1;
            this.Label8.Text = "Developed By Eliminater74            https://github.com/Eliminater74/AsBuiltExplo" +
    "rer";
            // 
            // PictureBox1
            // 
            this.PictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.PictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox1.Image")));
            this.PictureBox1.Location = new System.Drawing.Point(11, 601);
            this.PictureBox1.Name = "PictureBox1";
            this.PictureBox1.Size = new System.Drawing.Size(115, 52);
            this.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox1.TabIndex = 3;
            this.PictureBox1.TabStop = false;
            this.PictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1029, 646);
            this.Controls.Add(this.Label8);
            this.Controls.Add(this.TabControl1);
            this.Controls.Add(this.PictureBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(913, 500);
            this.Name = "Form1";
            this.Text = "As-Built Compare";
            this.TabControl1.ResumeLayout(false);
            this.TabPage1.ResumeLayout(false);
            this.TabPage1.PerformLayout();
            this.ContextMenuStrip1.ResumeLayout(false);
            this.TabPage2.ResumeLayout(false);
            this.TabPage2.PerformLayout();
            this.TabPage3.ResumeLayout(false);
            this.TabPage3.PerformLayout();
            this.TabPage4.ResumeLayout(false);
            this.TabPage4.PerformLayout();
            this.TabPage5.ResumeLayout(false);
            this.TabPage5.PerformLayout();
            this.TabPage6.ResumeLayout(false);
            this.ContextMenuStrip2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox1)).EndInit();
            this.TabPage5.Click += new System.EventHandler(this.TabPage5_Click);
            this.lvwBrowser.SelectedIndexChanged += new System.EventHandler(this.lvwBrowser_SelectedIndexChanged);
            this.Button6.Click += new System.EventHandler(this.Button6_Click_1);
            this.Button5.Click += new System.EventHandler(this.Button5_Click_1);
            this.btnBrowseRefresh.Click += new System.EventHandler(this.Button10_Click);
            this.Button10.Click += new System.EventHandler(this.Button10_Click_1);
            this.Button7.Click += new System.EventHandler(this.Button7_Click);
            this.Button9.Click += new System.EventHandler(this.Button9_Click);
            this.Button8.Click += new System.EventHandler(this.Button8_Click);
            this.Button4.Click += new System.EventHandler(this.Button4_Click);
            this.btnDeduceSaveInfo.Click += new System.EventHandler(this.Button3_Click);
            this.Button3.Click += new System.EventHandler(this.Button3_Click_1);
            this.Button2.Click += new System.EventHandler(this.Button2_Click_2);
            this.btnDeduceLoadOptions.Click += new System.EventHandler(this.btnDeduceLoadOptions_Click);
            this.btnDeduceFigureIt.Click += new System.EventHandler(this.btnDeduceFigureIt_Click);
            this.TabPage4.Click += new System.EventHandler(this.TabPage4_Click);
            this.BinaryToolStripMenuItem.Click += new System.EventHandler(this.BinaryToolStripMenuItem_Click);
            this.Data3hexToolStripMenuItem.Click += new System.EventHandler(this.Data3hexToolStripMenuItem_Click);
            this.Data2hexToolStripMenuItem.Click += new System.EventHandler(this.Data2hexToolStripMenuItem_Click);
            this.Data1hexToolStripMenuItem.Click += new System.EventHandler(this.Data1hexToolStripMenuItem_Click);
            this.EntireLineToolStripMenuItem.Click += new System.EventHandler(this.EntireLineToolStripMenuItem_Click);
            this.CopyToolStripMenuItem.Click += new System.EventHandler(this.CopyToolStripMenuItem_Click);
            this.btnDeduceOpenETIS.Click += new System.EventHandler(this.Button2_Click_1);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

  private TabPage TabPage1;
  private TabPage TabPage2;
  private TextBox tbxData2hex;
  private Label Label3;
  private TextBox tbxData1hex;
  private TextBox tbxChecksumHex;
  private Button Button1;
  private TextBox tbxData3hex;
  private Button btnCompBrowse2;
  private Button btnCompBrowse1;
  private ListView ListView1;
  private Button btnCompLoad;
  private ContextMenuStrip ContextMenuStrip1;
  private ToolStripMenuItem CopyToolStripMenuItem;
  private ToolStripMenuItem EntireLineToolStripMenuItem;
  private ToolStripMenuItem Data1hexToolStripMenuItem;
  private ToolStripMenuItem Data2hexToolStripMenuItem;
  private ToolStripMenuItem Data3hexToolStripMenuItem;
  private ToolStripMenuItem BinaryToolStripMenuItem;
  private Button btnDeduceOpenETIS;
  private WebBrowser wbDeducer;
  private Button btnDeduceSaveInfo;
  private TabPage TabPage4;
  private Button btnDeduceFigureIt;
  private Button btnDeduceLoadOptions;
  private Button Button3;
  private Button Button2;
  private TabPage TabPage5;
  private Button Button4;
  private Button Button8;
  private Button Button6;
  private Button Button5;
  private Button Button9;
  private Button Button7;
  private ListView lvwBrowser;
  private Button btnBrowseRefresh;
  private ContextMenuStrip ContextMenuStrip2;
  private ToolStripMenuItem SetAsCompare1ToolStripMenuItem;
  private ToolStripMenuItem SetAsCompare2ToolStripMenuItem;
  private ToolStripMenuItem SetAsCompare3ToolStripMenuItem;
  private ToolStripMenuItem SetAsCompare4ToolStripMenuItem;
  private ToolStripMenuItem DeleteFileToolStripMenuItem;
  private PictureBox PictureBox1;
  private ToolStripMenuItem ExportModuleToolStripMenuItem;
  private ToolStripMenuItem ToUCDSToolStripMenuItem;
  private ToolStripMenuItem ToABTToolStripMenuItem;
  private Button Button10;
  private CheckBox chkShowOnlyMismatches;
  
  private TabControl TabControl1;
  private Label Label2;
  private TextBox tbxModIDhex;
  private Label Label1;
  private Label Label5;
  private Label Label4;
  private TextBox tbxData3bin2;
  private TextBox tbxData2bin2;
  private TextBox tbxData1bin2;
  private TextBox tbxData3bin1;
  private TextBox tbxData2bin1;
  private TextBox tbxData1bin1;
  private TextBox tbxCompFile2;
  private TextBox tbxCompFile1;
  private Label Label7;
  private Label Label6;
  private ColumnHeader ColumnHeader1;
  private ColumnHeader ColumnHeader2;
  private ColumnHeader ColumnHeader3;
  private ColumnHeader ColumnHeader4;
  private ColumnHeader ColumnHeader5;
  private ColumnHeader ColumnHeader6;
  private Label Label8;
  private TabPage TabPage3;
  private CheckBox chkDeduceDownloadAB;
  private TextBox tbxDeduceReport;
  private ListBox lstDeduceFactoryOptions;
  private Label Label9;
  private CheckedListBox lstDeduceModels;
  private CheckedListBox lstDeduceYears;
  private Label Label11;
  private Label Label10;
  private Label Label12;
  private TextBox TextBox2;
  private TextBox TextBox1;
  private Label Label13;
  private Label Label14;
  private TextBox tbxConvertBin;
  private TextBox tbxConvertHex;
  private TextBox tbxChecksumBin;
  private Label Label15;
  private Label Label16;
  private Label Label17;
  private TextBox TextBox3;
  private Label Label18;
  private TextBox tbxDeduceReport2;
  private ColumnHeader ColumnHeader7;
  private ColumnHeader ColumnHeader8;
  private ColumnHeader ColumnHeader9;
  private Label Label20;
  private Label Label19;
  private CheckedListBox lstBit_Years;
  private CheckedListBox lstBit_Models;
  private ListBox lstBit_Modules;
  private TextBox TextBox4;
  private Label Label21;
  private CheckBox chkCompareShowChecksum;
  private CheckBox chkDeduceDoCCC;
  private TextBox tbxCompFile4;
  private Label Label23;
  private TextBox tbxCompFile3;
  private Label Label22;
  private TextBox TextBox5;
  private TabPage TabPage6;
  private ColumnHeader ColumnHeader10;
  private ColumnHeader ColumnHeader11;
  private ColumnHeader ColumnHeader12;
  private ColumnHeader ColumnHeader13;
  private ColumnHeader ColumnHeader14;
  private ToolStripSeparator ToolStripMenuItem1;
  private Label lblComp4VIN;
  private Label lblComp3VIN;
  private Label lblComp2VIN;
  private Label lblComp1VIN;
  private CheckBox chkCompareShowNames;
  private TextBox TextBox6;
}
}






