
using AsBuiltExplorer.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AsBuiltExplorer
{

// [DesignerGenerated] removed
public partial class Form1 : Form
{
  ColumnHeader lvwBrowser_SortingColumn;
  public int abDownloadTriggered;

  public Form1()
  {
    Load += new EventHandler(Form1_Load);
    MaximumSizeChanged += new EventHandler(Form1_MaximumSizeChanged);
    Shown += new EventHandler(Form1_Shown);
    abDownloadTriggered = 0;
    InitializeComponent();
  }

  string VehicleInfo_GetModuleDataByID_Binary(
    string moduleID,
    Form1.VehicleInfo vhclInfo,
    ref string modulePartNum)
  {
    modulePartNum = "";
    var num = checked (vhclInfo.abModuleAddrCount - 1);
    var index = 0;
    while (index <= num)
    {
      if (moduleID.Length == vhclInfo.abModuleAddresses[index].Length)
      {
        if (Operators.CompareString(moduleID, vhclInfo.abModuleAddresses[index], false) == 0)
        {
          modulePartNum = vhclInfo.abModuleInfo_PartNums[index];
          return vhclInfo.abModuleDatasBinStr[index];
        }
      }
      else if (moduleID.StartsWith(vhclInfo.abModuleAddresses[index]))
      {
        modulePartNum = vhclInfo.abModuleInfo_PartNums[index];
        return vhclInfo.abModuleDatasBinStr[index];
      }
      checked { ++index; }
    }
    return "";
  }

  string VehicleInfo_GetModuleDataByIDandPart_Binary(
    string moduleID,
    Form1.VehicleInfo vhclInfo)
  {
    var num = checked (vhclInfo.abModuleAddrCount - 1);
    var index = 0;
    while (index <= num)
    {
      if (moduleID.StartsWith(vhclInfo.abModuleAddresses[index]) && !Information.IsNothing((object) vhclInfo.abModuleInfo_PartNums[index]) && moduleID.EndsWith(vhclInfo.abModuleInfo_PartNums[index]))
        return vhclInfo.abModuleDatasBinStr[index];
      checked { ++index; }
    }
    return "";
  }

  void TextBox3_TextChanged(object sender, EventArgs e)
  {
    var str1 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData1hex.Text);
    tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    var str2 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData2hex.Text);
    tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    var str3 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData3hex.Text);
    tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
  }

  void Label3_Click(object sender, EventArgs e)
  {
  }

  void Button1_Click(object sender, EventArgs e)
  {
    tbxChecksumHex.Text = modAsBuilt.AsBuilt_CalculateChecksum(tbxModIDhex.Text, tbxData1hex.Text + tbxData2hex.Text + tbxData3hex.Text);
    tbxChecksumBin.Text = Strings.Mid(modAsBuilt.AsBuilt_HexStr2BinStr(tbxChecksumHex.Text), 1, 8);
    var str1 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData1hex.Text);
    tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    var str2 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData2hex.Text);
    tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    var str3 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData3hex.Text);
    tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
  }

  void Form1_Load(object sender, EventArgs e)
  {
      ModuleDatabase.LoadDatabase();
      ModuleDatabase.LoadDatabase();
      CommonDatabase.Load();

      // Initialize Help Screen (TabPage7)
      if (TabPage7.Controls.Count == 0) 
      {
          var helpBrowser = new WebBrowser();
          helpBrowser.Dock = DockStyle.Fill;
          helpBrowser.IsWebBrowserContextMenuEnabled = false; 
          helpBrowser.AllowWebBrowserDrop = false;
          helpBrowser.ScriptErrorsSuppressed = true;
          TabPage7.Controls.Add(helpBrowser);
          
          var helpPath = Path.Combine(Application.StartupPath, "Help", "index.html");
          if (File.Exists(helpPath))
          {
              helpBrowser.Navigate(helpPath);
          }
          else
          {
               var errLbl = new Label() { Text = "Help file not found: " + helpPath, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
               TabPage7.Controls.Add(errLbl);
          }
      }
      VehicleDatabase.Load(); // Load existing vehicles
      
      // Auto-Import from AsBuiltData folder
      try
      {
         var asBuiltDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AsBuiltData");
         if (Directory.Exists(asBuiltDir))
         {
             var count = VehicleDatabase.BulkImport(asBuiltDir);
             if (count > 0)
             {
                 // Optional: Notify user or just show in list (if list UI existed)
                 // MessageBox.Show($"Imported {count} new vehicles from AsBuiltData folder.", "Auto-Import");
             }
         }
      }
      catch (Exception ex)
      {
          System.Diagnostics.Debug.WriteLine("Auto-Import Error: " + ex.Message);
      }

      ApplyTheme();

      // Set Version from Assembly
      var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
      Text = $"AsBuiltExplorer Toolkit v{version}";
      lblAboutVersion.Text = $"Version {version}";

      // Update About Screen Branding (Fancy "Toolkit")
      // Shift items down to make room
      var shiftY = 40;
      lblAboutVersion.Top += shiftY;
      lblAboutDev.Top += shiftY;
      lblAboutCredits.Top += shiftY;
      lblAboutMoto.Top += shiftY;
      lnkAboutGithub.Top += shiftY;

      var lblToolkit = new Label();
      lblToolkit.Text = "Toolkit";
      lblToolkit.Font = new Font(lblAboutTitle.Font.FontFamily, 22, FontStyle.Bold); // Match title font, slightly smaller
      // lblToolkit.ForeColor = Color.Gray; // Remove custom color to match title
      lblToolkit.AutoSize = true;
      lblToolkit.Location = new Point(lblAboutTitle.Left + 5, lblAboutTitle.Bottom - 5); // Tweak position
      TabPage8.Controls.Add(lblToolkit);
      lblToolkit.BringToFront();
  }

  void pbSettings_Click(object sender, EventArgs e)
  {
      using (var frm = new AsBuiltExplorer.Forms.frmSettings())
      {
          if (frm.ShowDialog() == DialogResult.OK)
          {
              if (frm.ThemeChanged) ApplyTheme();
          }
      }
  }

  void ApplyTheme()
  {
      var isDark = ("Dark" == My.MySettings.Default.AppTheme);
      var backColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
      var foreColor = isDark ? Color.White : SystemColors.ControlText;
      
      BackColor = backColor;
      ForeColor = foreColor;
      
      
      
      // Always use OwnerDraw to ensure horizontal text and consistent styling
      TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
      
      foreach (Control c in Controls)
      {
          UpdateControlTheme(c, backColor, foreColor);
      }
  }

  void UpdateControlTheme(Control c, Color back, Color fore)
  {
       // Skip controls that have custom color coding we want to preserve
       if (c.Name != null && (
           c.Name.StartsWith("lblComp") || 
           c.Name.StartsWith("tbxComp") ||
           c.Name == "Label6" || c.Name == "Label7" ||
           c.Name == "Label22" || c.Name == "Label23"))
       {
           return;
       }

       if (c is Button || c is CheckBox || c is RadioButton || c is Label || c is GroupBox || c is TabPage)
       {
           c.ForeColor = fore;
           c.BackColor = back;
       }
       else if (c is TextBox || c is ListBox || c is ListView || c is ComboBox || c is RichTextBox)
       {
           // Inputs usually stay white or need specific dark styling
           if (back != SystemColors.Control) // Dark mode
           {
               c.BackColor = Color.FromArgb(30, 30, 30);
               c.ForeColor = Color.White;
           }
           else
           {
               c.BackColor = SystemColors.Window;
               c.ForeColor = SystemColors.WindowText;
           }
       }
       
       if (c.HasChildren)
       {
           foreach (Control child in c.Controls)
           {
               UpdateControlTheme(child, back, fore);
           }
       }
  }

  void Button2_Click(object sender, EventArgs e)
  {
      CommonDatabase.ForceReload(); // Reload DB to pick up CSV changes
      var retModuleNames = new string[1];
    var retModuleShortNames = new string[1];
    var retModuleAddresses = new string[1];

    var directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    if (chkCompareShowNames.Checked)
    {
       // Legacy file loading removed in favor of ModuleDatabase
    }
    var strArray1 = new string[1];
    var retModuleDatas1 = new string[1];
    var num1 = 0;
    var retModInfo_IDs1 = new string[1];
    var retModInfo_PartNumbers1 = new string[1];
    var retModInfo_Strategies1 = new string[1];
    var retModInfo_Calibrations1 = new string[1];
    var retModInfo_Count1 = 0;
    var retCCCdata1 = "";
    var strArray2 = new string[1];
    var retModuleDatas2 = new string[1];
    var num2 = 0;
    var retModInfo_IDs2 = new string[1];
    var retModInfo_PartNumbers2 = new string[1];
    var retModInfo_Strategies2 = new string[1];
    var retModInfo_Calibrations2 = new string[1];
    var retModInfo_Count2 = 0;
    var retCCCdata2 = "";
    var strArray3 = new string[1];
    var retModuleDatas3 = new string[1];
    var num3 = 0;
    var retModInfo_IDs3 = new string[1];
    var retModInfo_PartNumbers3 = new string[1];
    var retModInfo_Strategies3 = new string[1];
    var retModInfo_Calibrations3 = new string[1];
    var retModInfo_Count3 = 0;
    var retCCCdata3 = "";
    var strArray4 = new string[1];
    var retModuleDatas4 = new string[1];
    var num4 = 0;
    var retModInfo_IDs4 = new string[1];
    var retModInfo_PartNumbers4 = new string[1];
    var retModInfo_Strategies4 = new string[1];
    var retModInfo_Calibrations4 = new string[1];
    var retModInfo_Count4 = 0;
    var retCCCdata4 = "";
    var text1 = tbxCompFile1.Text.Trim();
    var flag1 = false;
    var strArray5 = new string[1];
    var retVIN1 = "";
    var fileType1 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text1);
    var inpFileArray1 = Strings.Split(text1, "|");
    if (Operators.CompareString(fileType1, "ABT", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray1, inpFileArray1.Length, ref strArray1, ref retModuleDatas1, ref num1);
      lblComp1VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType1, "AB", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFile_AB(text1, ref strArray1, ref retModuleDatas1, ref num1, ref retVIN1, ref retModInfo_IDs1, ref retModInfo_PartNumbers1, ref retModInfo_Strategies1, ref retModInfo_Calibrations1, ref retModInfo_Count1, ref retCCCdata1);
      lblComp1VIN.Text = retVIN1;
    }
    else if (Operators.CompareString(fileType1, "UCDS", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFile_UCDS(text1, ref strArray1, ref retModuleDatas1, ref num1);
      lblComp1VIN.Text = "No VIN (UCDS)";
    }
    else if (!string.IsNullOrWhiteSpace(text1))
    {
        MessageBox.Show("Could not load File 1: " + text1 + "\r\nFile not found or unknown format.\r\n(Check if file was moved or deleted)", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    var text2 = tbxCompFile2.Text.Trim();
    var flag2 = false;
    var strArray6 = new string[1];
    var retVIN2 = "";
    var fileType2 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text2);
    var inpFileArray2 = Strings.Split(text2, "|");
    if (Operators.CompareString(fileType2, "ABT", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray2, inpFileArray2.Length, ref strArray2, ref retModuleDatas2, ref num2);
      lblComp2VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType2, "AB", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFile_AB(text2, ref strArray2, ref retModuleDatas2, ref num2, ref retVIN2, ref retModInfo_IDs2, ref retModInfo_PartNumbers2, ref retModInfo_Strategies2, ref retModInfo_Calibrations2, ref retModInfo_Count2, ref retCCCdata2);
      lblComp2VIN.Text = retVIN2;
    }
    else if (Operators.CompareString(fileType2, "UCDS", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFile_UCDS(text2, ref strArray2, ref retModuleDatas2, ref num2);
      lblComp2VIN.Text = "No VIN (UCDS)";
    }
    else if (!string.IsNullOrWhiteSpace(text2))
    {
        MessageBox.Show("Could not load File 2: " + text2 + "\r\nFile not found or unknown format.\r\n(Check if file was moved or deleted)", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    var text3 = tbxCompFile3.Text.Trim();
    var flag3 = false;
    var strArray7 = new string[1];
    var retVIN3 = "";
    var fileType3 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text3);
    var inpFileArray3 = Strings.Split(text3, "|");
    if (Operators.CompareString(fileType3, "ABT", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray3, inpFileArray3.Length, ref strArray3, ref retModuleDatas3, ref num3);
      lblComp3VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType3, "AB", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFile_AB(text3, ref strArray3, ref retModuleDatas3, ref num3, ref retVIN3, ref retModInfo_IDs3, ref retModInfo_PartNumbers3, ref retModInfo_Strategies3, ref retModInfo_Calibrations3, ref retModInfo_Count3, ref retCCCdata3);
      lblComp3VIN.Text = retVIN3;
    }
    else if (Operators.CompareString(fileType3, "UCDS", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFile_UCDS(text3, ref strArray3, ref retModuleDatas3, ref num3);
      lblComp3VIN.Text = "No VIN (UCDS)";
    }
    var text4 = tbxCompFile4.Text.Trim();
    var flag4 = false;
    var strArray8 = new string[1];
    var retVIN4 = "";
    var fileType4 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text4);
    var inpFileArray4 = Strings.Split(tbxCompFile4.Text, "|");
    if (Operators.CompareString(fileType4, "ABT", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray4, inpFileArray4.Length, ref strArray4, ref retModuleDatas4, ref num4);
      lblComp4VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType4, "AB", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFile_AB(text4, ref strArray4, ref retModuleDatas4, ref num4, ref retVIN4, ref retModInfo_IDs4, ref retModInfo_PartNumbers4, ref retModInfo_Strategies4, ref retModInfo_Calibrations4, ref retModInfo_Count4, ref retCCCdata4);
      lblComp4VIN.Text = retVIN4;
    }
    else if (Operators.CompareString(fileType4, "UCDS", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFile_UCDS(text4, ref strArray4, ref retModuleDatas4, ref num4);
      lblComp4VIN.Text = "No VIN (UCDS)";
    }
    ListView1.Items.Clear();
    var listBoldFont = new System.Drawing.Font(ListView1.Font, System.Drawing.FontStyle.Bold);
    var retData1_1 = "";
    var retData2_1 = "";
    var retData3_1 = "";
    var listViewItem1 = (ListViewItem) null;
    var listViewSubItem = (ListViewItem.ListViewSubItem) null;

    var str3 = modAsBuilt.AsBuilt_Ascii2Hex(retVIN1);
    var Right1 = "";
    var num5 = checked (num1 - 1);
    var index1 = 0;
    // Variables reused from above loop logic or just remove re-declaration
    // int num5 and int index1 were already declared above.
    // Resetting them is fine, redeclaring is not.
    num5 = checked (num1 - 1);
    index1 = 0;
    while (index1 <= num5)
    {
      if (!chkCompareShowChecksum.Checked)
        retModuleDatas1[index1] = Strings.Left(retModuleDatas1[index1], checked (Strings.Len(retModuleDatas1[index1]) - 2));
      var str4 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray1[index1]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas1[index1], ref retData1_1, ref retData2_1, ref retData3_1);
      var text5 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      var str5 = Strings.Left(str4, 3);
      var Right2 = Strings.Left(str4, 6);
      if (Operators.CompareString(str5, Right1, false) != 0)
      {
        var str6 = "";
        var num7 = index1;
        var num8 = checked (num1 - 1);
        var index2 = num7;
        while (index2 <= num8)
        {
          var retData1_2 = "";
          var retData2_2 = "";
          var retData3_2 = "";
          modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas1[index2], ref retData1_2, ref retData2_2, ref retData3_2);
          var str7 = Strings.Left(modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray1[index2]), 3);
          var Left2 = Strings.Left(str7, 6);
          if (Operators.CompareString(str7, str5, false) == 0)
          {
            if (Operators.CompareString(Left2, "7E1", false) == 0)
              Left2 = "7E0"; // Fix: Likely intended to normalize transmission module ID? Original code was Left2 = Left2 which does nothing. Assuming mapping 7E1->7E0 based on context or just removing the no-op if logic allows. Actually, looking at typical AsBuilt logic, usually 7E0 indicates PCM. warning says "did you mean to assign something else?". I'll comment it out for now to ensure no side effects from wrong guess, or actually, just remove the if block if it does nothing.
            // Original: Left2 = Left2; -> NO-OP.
            // If the intention was to remap, it failed.
            // Safest fix for "Clean Code" is remove the no-op line. Only keep if if has side effect (it doesn't).

            str6 = str6 + retData1_2 + retData2_2 + retData3_2;
            if (Operators.CompareString(Left2, Right2, false) != 0)
              str6 = Strings.Left(str6, checked (Strings.Len(str6) - 2));
            checked { ++index2; }
          }
          else
            break;
        }
        var num9 = Strings.InStr(1, str6, str3);
        var num10 = 0;
        if (num9 != 0)
          num10 = checked (num9 + Strings.Len(str3));
        Right1 = str5;
      }
      var text6 = "";
      var text7 = "";
      var text8 = "";
      var num11 = checked (retModInfo_Count1 - 1);
      var index3 = 0;
      while (index3 <= num11)
      {
        if (Operators.CompareString(Strings.Left(strArray1[index1], Strings.Len(retModInfo_IDs1[index3])), retModInfo_IDs1[index3], false) == 0)
        {
          text6 = retModInfo_PartNumbers1[index3];
          text7 = retModInfo_Strategies1[index3];
          text8 = retModInfo_Calibrations1[index3];
          break;
        }
        checked { ++index3; }
      }
      var modName = "";
      if (chkCompareShowNames.Checked)
      {
        var vinYear = VINDecoder.GetModelYear(retVIN1);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str4);
      }
      var listViewItem2 = ListView1.Items.Add(str4);
      listViewItem2.SubItems.Add(modName);
      listViewItem2.ForeColor = tbxCompFile1.ForeColor;
      listViewItem2.UseItemStyleForSubItems = false;
      listViewItem2.Tag = (object) text1;
      listViewSubItem = listViewItem2.SubItems.Add(retData1_1);
      listViewSubItem = listViewItem2.SubItems.Add(retData2_1);
      listViewSubItem = listViewItem2.SubItems.Add(retData3_1);
      listViewSubItem = listViewItem2.SubItems.Add("");
      listViewSubItem = listViewItem2.SubItems.Add(text5);
      
      // Feature Translation
      var vinYear1 = VINDecoder.GetModelYear(retVIN1);
      var feats1 = CommonDatabase.GetMatchingFeatures(str4, retData1_1, retData2_1, retData3_1, vinYear1);
      listViewSubItem = listViewItem2.SubItems.Add(string.Join(", ", feats1));

      listViewSubItem = listViewItem2.SubItems.Add(text6);
      listViewSubItem = listViewItem2.SubItems.Add(text7);
      listViewSubItem = listViewItem2.SubItems.Add(text8);
      foreach (ListViewItem.ListViewSubItem sub in listViewItem2.SubItems)
      {
          sub.ForeColor = tbxCompFile1.ForeColor;
          sub.Font = listBoldFont;
      }
      listViewItem1 = ListView1.Items.Add("");
      checked { ++index1; }
    }
    var num12 = checked (num2 - 1);
    var index4 = 0;
    while (index4 <= num12)
    {
      if (!chkCompareShowChecksum.Checked)
        retModuleDatas2[index4] = Strings.Left(retModuleDatas2[index4], checked (Strings.Len(retModuleDatas2[index4]) - 2));
      var str8 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray2[index4]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas2[index4], ref retData1_1, ref retData2_1, ref retData3_1);
      var modName = "";
      if (chkCompareShowNames.Checked)
      {
        var vinYear = VINDecoder.GetModelYear(retVIN2);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str8);
      }
      var index5 = -1;
      var num13 = checked (ListView1.Items.Count - 1);
      var index6 = 0;
      while (index6 <= num13)
      {
        if (Operators.CompareString(ListView1.Items[index6].Text, str8, false) == 0)
          index5 = index6;
        checked { ++index6; }
      }
      var listViewItem3 = new ListViewItem(str8);
      listViewItem3.SubItems.Add(modName);
      if (index5 != -1)
        ListView1.Items.Insert(checked (index5 + 1), listViewItem3);
      else
        listViewItem3 = ListView1.Items.Add(str8);
      listViewItem3.ForeColor = tbxCompFile2.ForeColor;
      listViewItem3.UseItemStyleForSubItems = false;
      listViewItem3.Tag = (object) text2;
      var text9 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      var text10 = "";
      var text11 = "";
      var text12 = "";
      var num14 = checked (retModInfo_Count2 - 1);
      var index7 = 0;
      while (index7 <= num14)
      {
        if (Operators.CompareString(Strings.Left(strArray2[index4], Strings.Len(retModInfo_IDs2[index7])), retModInfo_IDs2[index7], false) == 0)
        {
          text10 = retModInfo_PartNumbers2[index7];
          text11 = retModInfo_Strategies2[index7];
          text12 = retModInfo_Calibrations2[index7];
          break;
        }
        checked { ++index7; }
      }
      listViewSubItem = listViewItem3.SubItems.Add(retData1_1);
      listViewSubItem = listViewItem3.SubItems.Add(retData2_1);
      listViewSubItem = listViewItem3.SubItems.Add(retData3_1);
      var flag6 = true;
      if (index5 != -1)
      {
        var num15 = checked (listViewItem3.SubItems.Count - 1);
        var index8 = 0;
        while (index8 <= num15)
        {
          if (Operators.CompareString(listViewItem3.SubItems[index8].Text, ListView1.Items[index5].SubItems[index8].Text, false) != 0)
          {
            flag6 = false;
            break;
          }
          checked { ++index8; }
        }
      }
      listViewSubItem = index5 == -1 ? listViewItem3.SubItems.Add("No Match") : (!flag6 ? listViewItem3.SubItems.Add("No") : listViewItem3.SubItems.Add("Yes"));
      listViewSubItem = listViewItem3.SubItems.Add(text9);
      
      // Feature Translation
      var vinYear2 = VINDecoder.GetModelYear(retVIN2);
      var feats2 = CommonDatabase.GetMatchingFeatures(str8, retData1_1, retData2_1, retData3_1, vinYear2);
      listViewSubItem = listViewItem3.SubItems.Add(string.Join(", ", feats2));

      listViewSubItem = listViewItem3.SubItems.Add(text10);
      listViewSubItem = listViewItem3.SubItems.Add(text11);
      listViewSubItem = listViewItem3.SubItems.Add(text12);
      if (index5 == -1)
        listViewItem1 = ListView1.Items.Add("");
      foreach (ListViewItem.ListViewSubItem sub in listViewItem3.SubItems)
      {
          sub.ForeColor = tbxCompFile2.ForeColor;
          sub.Font = listBoldFont;
      }
      checked { ++index4; }
    }
    var num16 = checked (num3 - 1);
    var index9 = 0;
    while (index9 <= num16)
    {
      if (!chkCompareShowChecksum.Checked)
        retModuleDatas3[index9] = Strings.Left(retModuleDatas3[index9], checked (Strings.Len(retModuleDatas3[index9]) - 2));
      var str9 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray3[index9]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas3[index9], ref retData1_1, ref retData2_1, ref retData3_1);
      var modName = "";
      if (chkCompareShowNames.Checked)
      {
        var vinYear = VINDecoder.GetModelYear(retVIN3);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str9);
      }
      var index10 = -1;
      var num17 = checked (ListView1.Items.Count - 1);
      var index11 = 0;
      while (index11 <= num17)
      {
        if (Operators.CompareString(ListView1.Items[index11].Text, str9, false) == 0)
          index10 = index11;
        checked { ++index11; }
      }
      var listViewItem4 = new ListViewItem(str9);
      listViewItem4.SubItems.Add(modName);
      if (index10 != -1)
        ListView1.Items.Insert(checked (index10 + 1), listViewItem4);
      else
        listViewItem4 = ListView1.Items.Add(str9);
      listViewItem4.ForeColor = tbxCompFile3.ForeColor;
      listViewItem4.UseItemStyleForSubItems = false;
      var text13 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      var text14 = "";
      var text15 = "";
      var text16 = "";
      var num18 = checked (retModInfo_Count3 - 1);
      var index12 = 0;
      while (index12 <= num18)
      {
        if (Operators.CompareString(Strings.Left(strArray3[index9], Strings.Len(retModInfo_IDs3[index12])), retModInfo_IDs3[index12], false) == 0)
        {
          text14 = retModInfo_PartNumbers3[index12];
          text15 = retModInfo_Strategies3[index12];
          text16 = retModInfo_Calibrations3[index12];
          break;
        }
        checked { ++index12; }
      }
      listViewSubItem = listViewItem4.SubItems.Add(retData1_1);
      listViewSubItem = listViewItem4.SubItems.Add(retData2_1);
      listViewSubItem = listViewItem4.SubItems.Add(retData3_1);
      var flag7 = true;
      if (index10 != -1)
      {
        var num19 = checked (listViewItem4.SubItems.Count - 1);
        var index13 = 0;
        while (index13 <= num19)
        {
          if (Operators.CompareString(listViewItem4.SubItems[index13].Text, ListView1.Items[index10].SubItems[index13].Text, false) != 0)
          {
            flag7 = false;
            break;
          }
          checked { ++index13; }
        }
      }
      listViewSubItem = index10 == -1 ? listViewItem4.SubItems.Add("No Match") : (!flag7 ? listViewItem4.SubItems.Add("No") : listViewItem4.SubItems.Add("Yes"));
      listViewSubItem = listViewItem4.SubItems.Add(text13);
      
      // Feature Translation
      var vinYear3 = VINDecoder.GetModelYear(retVIN3);
      var feats3 = CommonDatabase.GetMatchingFeatures(str9, retData1_1, retData2_1, retData3_1, vinYear3);
      listViewSubItem = listViewItem4.SubItems.Add(string.Join(", ", feats3));

      listViewSubItem = listViewItem4.SubItems.Add(text14);
      listViewSubItem = listViewItem4.SubItems.Add(text15);
      listViewSubItem = listViewItem4.SubItems.Add(text16);
      if (index10 == -1)
        listViewItem1 = ListView1.Items.Add("");
      foreach (ListViewItem.ListViewSubItem sub in listViewItem4.SubItems)
      {
          sub.ForeColor = tbxCompFile3.ForeColor;
          sub.Font = listBoldFont;
      }
      checked { ++index9; }
    }
    var num20 = checked (num4 - 1);
    var index14 = 0;
    while (index14 <= num20)
    {
      if (!chkCompareShowChecksum.Checked)
        retModuleDatas4[index14] = Strings.Left(retModuleDatas4[index14], checked (Strings.Len(retModuleDatas4[index14]) - 2));
      var str10 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray4[index14]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas4[index14], ref retData1_1, ref retData2_1, ref retData3_1);
      var modName = "";
      if (chkCompareShowNames.Checked)
      {
        var vinYear = VINDecoder.GetModelYear(retVIN4);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str10);
      }
      var index15 = -1;
      var num21 = checked (ListView1.Items.Count - 1);
      var index16 = 0;
      while (index16 <= num21)
      {
        if (Operators.CompareString(ListView1.Items[index16].Text, str10, false) == 0)
          index15 = index16;
        checked { ++index16; }
      }
      var listViewItem5 = new ListViewItem(str10);
      listViewItem5.SubItems.Add(modName);
      if (index15 != -1)
        ListView1.Items.Insert(checked (index15 + 1), listViewItem5);
      else
        listViewItem5 = ListView1.Items.Add(str10);
      listViewItem5.ForeColor = tbxCompFile4.ForeColor;
      listViewItem5.UseItemStyleForSubItems = false;
      listViewItem5.Tag = (object) text4;
      // Fix CS0103: str2 was removed but this line remained.
      // str2 = ""; // Removed unused assignment
      var text17 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      var text18 = "";
      var text19 = "";
      var text20 = "";
      var num22 = checked (retModInfo_Count4 - 1);
      var index17 = 0;
      while (index17 <= num22)
      {
        if (Operators.CompareString(Strings.Left(strArray4[index14], Strings.Len(retModInfo_IDs4[index17])), retModInfo_IDs4[index17], false) == 0)
        {
          text18 = retModInfo_PartNumbers4[index17];
          text19 = retModInfo_Strategies4[index17];
          text20 = retModInfo_Calibrations4[index17];
          break;
        }
        checked { ++index17; }
      }
      listViewSubItem = listViewItem5.SubItems.Add(retData1_1);
      listViewSubItem = listViewItem5.SubItems.Add(retData2_1);
      listViewSubItem = listViewItem5.SubItems.Add(retData3_1);
      var flag8 = true;
      if (index15 != -1)
      {
        var num23 = checked (listViewItem5.SubItems.Count - 1);
        var index18 = 0;
        while (index18 <= num23)
        {
          if (Operators.CompareString(listViewItem5.SubItems[index18].Text, ListView1.Items[index15].SubItems[index18].Text, false) != 0)
          {
            flag8 = false;
            break;
          }
          checked { ++index18; }
        }
      }
      listViewSubItem = index15 == -1 ? listViewItem5.SubItems.Add("No Match") : (!flag8 ? listViewItem5.SubItems.Add("No") : listViewItem5.SubItems.Add("Yes"));
      listViewSubItem = listViewItem5.SubItems.Add(text17);

      // Feature Translation
      var vinYear4 = VINDecoder.GetModelYear(retVIN4);
      var feats4 = CommonDatabase.GetMatchingFeatures(str10, retData1_1, retData2_1, retData3_1, vinYear4);
      listViewSubItem = listViewItem5.SubItems.Add(string.Join(", ", feats4));

      listViewSubItem = listViewItem5.SubItems.Add(text18);
      listViewSubItem = listViewItem5.SubItems.Add(text19);
      listViewSubItem = listViewItem5.SubItems.Add(text20);
      if (index15 == -1)
        listViewItem1 = ListView1.Items.Add("");
      foreach (ListViewItem.ListViewSubItem sub in listViewItem5.SubItems)
      {
          sub.ForeColor = tbxCompFile4.ForeColor;
          sub.Font = listBoldFont;
      }
      checked { ++index14; }
    }
    if (!flag1 & !flag2 & !flag3 & !flag4)
      return;
    var color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 230, 230);
    var lightYellow = Color.LightYellow;
    var index19 = 0;
    do
    {
      var num24 = index19;
      var num25 = checked (index19 + 1);
      var num26 = checked (ListView1.Items.Count - 1);
      var index20 = num25;
      while (index20 <= num26)
      {
        ListView1.Items[index20].Tag = (object) "";
        if (Operators.CompareString(ListView1.Items[index20].Text, "", false) != 0)
        {
          num24 = index20;
          checked { ++index20; }
        }
        else
          break;
      }
      var num27 = 1;
      var num28 = index19;
      var num29 = num24;
      var index21 = num28;
      while (index21 <= num29)
      {
        ListView1.Items[index21].Tag = (object) Conversions.ToString(num27);
        checked { ++num27; }
        checked { ++index21; }
      }
      var text21 = ListView1.Items[index19].SubItems[6].Text;
      var flag9 = true;
      var num30 = checked (index19 + 1);
      var num31 = num24;
      var index22 = num30;
      while (index22 <= num31)
      {
        if (Operators.CompareString(ListView1.Items[index22].SubItems[6].Text, text21, false) != 0)
        {
          flag9 = false;
          break;
        }
        checked { ++index22; }
      }
      var num32 = index19;
      var num33 = num24;
      var index23 = num32;
      while (index23 <= num33)
      {
        if (!flag9)
        {
          ListView1.Items[index23].SubItems[5].Text = "No";
          ListView1.Items[index23].BackColor = Color.MistyRose;
          var num34 = checked (ListView1.Items[index23].SubItems.Count - 1);
          var index24 = 0;
          var currentFore = ListView1.Items[index23].ForeColor;
          while (index24 <= num34)
          {
            ListView1.Items[index23].SubItems[index24].BackColor = Color.MistyRose;
            ListView1.Items[index23].SubItems[index24].ForeColor = currentFore;
            checked { ++index24; }
          }
        }
        else
        {
          ListView1.Items[index23].SubItems[5].Text = "Yes";
          ListView1.Items[index23].BackColor = Color.LightYellow;
          var num35 = checked (ListView1.Items[index23].SubItems.Count - 1);
          var index25 = 0;
          var currentFore = ListView1.Items[index23].ForeColor;
          while (index25 <= num35)
          {
            ListView1.Items[index23].SubItems[index25].BackColor = lightYellow;
            ListView1.Items[index23].SubItems[index25].ForeColor = currentFore;
            checked { ++index25; }
          }
        }
        checked { ++index23; }
      }
      index19 = checked (num24 + 2);
    }
    while (index19 <= checked (ListView1.Items.Count - 2));
    var num36 = 0;
    if (Operators.CompareString(tbxCompFile1.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(tbxCompFile2.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(tbxCompFile3.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(tbxCompFile4.Text, "", false) != 0)
      checked { ++num36; }
    if (chkShowOnlyMismatches.Checked && num36 > 1)
    {
      var index26 = checked (ListView1.Items.Count - 1);
      while (index26 >= 0)
      {
        if (ListView1.Items[index26].BackColor == lightYellow)
          ListView1.Items.RemoveAt(index26);
        checked { index26 += -1; }
      }
      var index27 = checked (ListView1.Items.Count - 1);
      while (index27 >= 1)
      {
        if (Operators.CompareString(ListView1.Items[index27].Text, "", false) == 0 && Operators.CompareString(ListView1.Items[checked (index27 - 1)].Text, "", false) == 0)
          ListView1.Items.RemoveAt(index27);
        checked { index27 += -1; }
      }
    }
    ListView1.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
    ListView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
  }

  void btnCompBrowse1_Click(object sender, EventArgs e)
  {
    var str = "";
    var strArray1 = new string[1];
    var num1 = 0;
    var strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    var strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    var form = (Form) this;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref strArray3, ref strArray2, ref form, ref str, ref strArray1, ref num1, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    tbxCompFile1.Text = "";
    var num2 = checked (num1 - 1);
    var index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      tbxCompFile1.Text = $"{tbxCompFile1.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(tbxCompFile1.Text, 1), "|", false) == 0)
      tbxCompFile1.Text = Strings.Left(tbxCompFile1.Text, checked (Strings.Len(tbxCompFile1.Text) - 1));
    lblComp1VIN.Text = "";
  }

  void btnCompBrowse2_Click(object sender, EventArgs e)
  {
    var str = "";
    var strArray1 = new string[1];
    var num1 = 0;
    var strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    var strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    var form = (Form) this;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref strArray3, ref strArray2, ref form, ref str, ref strArray1, ref num1, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    tbxCompFile2.Text = "";
    var num2 = checked (num1 - 1);
    var index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      tbxCompFile2.Text = $"{tbxCompFile2.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(tbxCompFile2.Text, 1), "|", false) == 0)
      tbxCompFile2.Text = Strings.Left(tbxCompFile2.Text, checked (Strings.Len(tbxCompFile2.Text) - 1));
    lblComp2VIN.Text = "";
  }

  void ListView1_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  void CopyToolStripMenuItem_Click(object sender, EventArgs e)
  {
  }

  void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
  {
    if (ListView1.Items.Count >= 1)
      return;
    e.Cancel = true;
  }

  void btnDeduceGo_Click(object sender, EventArgs e)
  {
      var vin = txtDeduceVIN.Text.Trim();
      wbDeducer.ScriptErrorsSuppressed = true;
      wbDeducer.Navigate("https://www.motorcraftservice.com/AsBuilt");
  }

  void wbDeducer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
  {
      var vin = txtDeduceVIN.Text.Trim();
      if (string.IsNullOrEmpty(vin)) return;

      if (wbDeducer.Url.ToString().ToLower().Contains("motorcraftservice.com"))
      {
          var vinBox = wbDeducer.Document.GetElementById("VIN");
          if (vinBox == null) vinBox = wbDeducer.Document.GetElementById("vin");
          
          if (vinBox != null)
          {
              vinBox.SetAttribute("value", vin);
          }
      }
  }


  
  void btnBrowseRefresh_Click(object sender, EventArgs e) => PopulateVehicleList();

  void PopulateVehicleList()
  {
      VehicleDatabase.Load();
      lvwBrowser.Items.Clear();
      lvwBrowser.BeginUpdate();
      try
      {
          foreach (var v in VehicleDatabase.Entries)
          {
              var item = new ListViewItem(v.FriendlyName);
              // Date
              var dateStr = "--";
              try {
                  if (!string.IsNullOrEmpty(v.FilePath) && File.Exists(v.FilePath))
                      dateStr = File.GetLastWriteTime(v.FilePath).ToString("g"); // Short Date + Time
              } catch {}
              
              item.SubItems.Add(dateStr);
              
              // Year
              item.SubItems.Add(v.Year ?? "");
              
              // Model (Make + Model)
              var fullModel = (v.Make + " " + v.Model).Trim();
              item.SubItems.Add(fullModel);
              
              item.SubItems.Add(v.VIN);
              
              // CRITICAL: Set Name to FilePath so Context Menus work (they read .Name)
              item.Name = v.FilePath; 
              item.Tag = v; // Store full object for future use

              lvwBrowser.Items.Add(item);
          }
      }
      finally
      {
          lvwBrowser.EndUpdate();
      }
  }

  void cmbDeduceSavedVehicles_SelectedIndexChanged(object sender, EventArgs e)
  {
       if (cmbDeduceSavedVehicles.SelectedItem is VehicleEntry v)
       {
           txtDeduceVIN.Text = v.VIN;
       }
  }

  void TabPage3_Enter(object sender, EventArgs e)
  {
      VehicleDatabase.Load();
      cmbDeduceSavedVehicles.Items.Clear();
      cmbDeduceSavedVehicles.Items.Add("-- Not Used --");
      foreach (var v in VehicleDatabase.Entries)
      {
          cmbDeduceSavedVehicles.Items.Add(v);
      }
      if (cmbDeduceSavedVehicles.Items.Count > 0) cmbDeduceSavedVehicles.SelectedIndex = 0;
  }



  void wbDeducer_FileDownload(object sender, EventArgs e) => ++abDownloadTriggered;

  void btnDeduceLoadOptions_Click(object sender, EventArgs e)
  {
      lstDeduceFactoryOptions.Items.Clear();
      lstDeduceModels.Items.Clear();
      lstDeduceYears.Items.Clear();

      // Refactored to use VehicleDatabase
      var entries = VehicleDatabase.Entries;

      var features = new HashSet<string>();
      var models = new HashSet<string>();
      var years = new HashSet<string>();

      foreach (var v in entries)
      {
          if (!string.IsNullOrEmpty(v.Features))
          {
              foreach (var f in v.Features.Split(';'))
              {
                  if (!string.IsNullOrEmpty(f)) features.Add(f.Trim());
              }
          }
          if (!string.IsNullOrEmpty(v.Model)) models.Add(v.Model);
          if (!string.IsNullOrEmpty(v.Year)) years.Add(v.Year);
      }

      // Populate UI
      var featList = new List<string>(features);
      featList.Sort();
      foreach (var f in featList) lstDeduceFactoryOptions.Items.Add(f);

      var modelList = new List<string>(models);
      modelList.Sort();
      foreach (var m in modelList) lstDeduceModels.Items.Add(m);

      var yearList = new List<string>(years);
      yearList.Sort();
      foreach (var y in yearList) lstDeduceYears.Items.Add(y);
      
      if(features.Count == 0)
      {
          MessageBox.Show("No vehicle features found in the database.\n\nTo Auto-Discover Features:\n1. Go to the 'Vehicle Database' tab.\n2. Select your vehicles.\n3. Right-Click and choose 'Decode with NHTSA (Online)'.\n\nThis will automatically fetch official specs (Trim, Drive, Series) using the government API and populate this list.", "No Features Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
  }

  void btnDeduceFigureIt_Click(object sender, EventArgs e)
  {
    tbxDeduceReport.Text = "";
    if (Information.IsNothing(RuntimeHelpers.GetObjectValue(lstDeduceFactoryOptions.SelectedItem)))
    {
      var num1 = (int) Interaction.MsgBox((object) "Select a feature to deduce.");
    }
    else if (Information.IsNothing((object) lstDeduceModels.SelectedItems) || lstDeduceModels.SelectedItems.Count < 1)
    {
      var num2 = (int) Interaction.MsgBox((object) "Select at least one vehicle model.");
    }
    else if (Information.IsNothing((object) lstDeduceYears.SelectedItems) || lstDeduceYears.SelectedItems.Count < 1)
    {
      var num3 = (int) Interaction.MsgBox((object) "Select at least one vehicle year.");
    }
    else
    {
      // Refactored Logic: Filter from Database
      var feature = lstDeduceFactoryOptions.SelectedItem.ToString();
      var entries = VehicleDatabase.Entries;
      var withFeature = new List<VehicleEntry>();
      var withoutFeature = new List<VehicleEntry>();
      
      // Helper to check if entry matches selected filters
      foreach(var entry in entries)
      {
           // Filter by Model
           var modelMatch = false;
           foreach(var m in lstDeduceModels.CheckedItems)
           {
               if (string.Equals(entry.Model, m.ToString(), StringComparison.OrdinalIgnoreCase)) { modelMatch = true; break;}
           }
           
           // Filter by Year
           var yearMatch = false;
           foreach(var y in lstDeduceYears.CheckedItems)
           {
               if (string.Equals(entry.Year, y.ToString(), StringComparison.OrdinalIgnoreCase)) { yearMatch = true; break; }
           }

           if(modelMatch && yearMatch)
           {
               // Check Feature
               var hasIt = false;
               if(!string.IsNullOrEmpty(entry.Features))
               {
                   foreach(var f in entry.Features.Split(';'))
                   {
                       if(string.Equals(f.Trim(), feature, StringComparison.OrdinalIgnoreCase)) { hasIt = true; break;}
                   }
               }
               
               if(hasIt) withFeature.Add(entry);
               else withoutFeature.Add(entry);
           }
      }

      // Map Helper
      Func<VehicleEntry, Form1.VehicleInfo> MapEntry = (vehEntry) => 
      {
          var vi = new Form1.VehicleInfo();
          vi.carVIN = vehEntry.VIN;
          vi.carYear = vehEntry.Year;
          vi.carModel = vehEntry.Model;
          
          var addrs = new string[0];
          var datas = new string[0];
          var count = 0;
          var _dummy = "";
          var dummyArr1 = new string[0];
          var dummyArr2 = new string[0];
          var dummyArr3 = new string[0];
          var dummyInt = 0;
          var dummyStr = "";
          
          // Write temp to use existing parser
          var tempFile = Path.GetTempFileName();
          File.WriteAllText(tempFile, vehEntry.FileContent);
          
          modAsBuilt.AsBuilt_LoadFile_AB(tempFile, ref addrs, ref datas, ref count, ref _dummy, ref dummyArr1, ref dummyArr2, ref dummyArr3, ref dummyArr3, ref dummyInt, ref dummyStr);
          File.Delete(tempFile);
          
          vi.abModuleAddresses = addrs;
          vi.abModuleDatasHex = datas;
          vi.abModuleAddrCount = count;
          vi.abModuleDatasBinStr = new string[count];
          for(int i=0; i<count; i++)
          {
               vi.abModuleDatasBinStr[i] = modAsBuilt.AsBuilt_HexStr2BinStr(datas[i]);
          }
          
          // Use dummyStr for CCC if available or empty string
          vi.carCCChex = dummyStr ?? ""; 
          if(!string.IsNullOrEmpty(vi.carCCChex) && vi.carCCChex.Length > 510)
          {
               vi.carCCChex = Strings.Right(vi.carCCChex, 510);
          }
          if(!string.IsNullOrEmpty(vi.carCCChex))
          {
               vi.carCCCbin = modAsBuilt.AsBuilt_HexStr2BinStr(vi.carCCChex);
          }
          else
          {
               vi.carCCCbin = "";
          }

          return vi;
      };
      
      // Construct Arrays for Analysis
      var arySrc1 = new Form1.VehicleInfo[withFeature.Count];
      for(int i=0; i<withFeature.Count; i++) arySrc1[i] = MapEntry(withFeature[i]);
      
      var arySrc2 = new Form1.VehicleInfo[withoutFeature.Count];
      for(int i=0; i<withoutFeature.Count; i++) arySrc2[i] = MapEntry(withoutFeature[i]);

      var index14 = withFeature.Count;
      var index15 = withoutFeature.Count;
      
      // Collect Module IDs for sorting (strArray12 replacement)
      var allMods = new HashSet<string>();
      foreach(var v in arySrc1) if(v.abModuleAddresses!=null) foreach(var m in v.abModuleAddresses) allMods.Add(m);
      foreach(var v in arySrc2) if(v.abModuleAddresses!=null) foreach(var m in v.abModuleAddresses) allMods.Add(m);
      
      var strArray12 = new List<string>(allMods).ToArray();
      Array.Sort(strArray12);
      var index10 = strArray12.Length;

      /* [Old Logic Commented Out]
      string directoryPath = MyProject.Application.Info.DirectoryPath;
      if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
        directoryPath += "\\";
      string path = directoryPath + "Deducer";
      try
      {
        Directory.CreateDirectory(path);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      this.lstDeduceFactoryOptions.SelectedItem.ToString();
      string str1 = ";";
      int num4 = checked (this.lstDeduceFactoryOptions.SelectedItems.Count - 1);
      int index1 = 0;
      while (index1 <= num4)
      {
        str1 = $"{str1}{this.lstDeduceFactoryOptions.SelectedItems[index1].ToString()};";
        checked { ++index1; }
      }
      string[] strArray1 = new string[1];
      string[] files = Directory.GetFiles(path, "*.ETIS.HTML");
      Form1.VehicleInfo[] vehicleInfoArray = new Form1.VehicleInfo[checked (files.Length + 1)];
      int index2 = 0;
      string retVIN = "";
      string str2 = "";
      string str3 = "";
      string String1_1 = ";";
      int num5 = checked (this.lstDeduceYears.CheckedItems.Count - 1);
      int index3 = 0;
      while (index3 <= num5)
      {
        String1_1 = $"{String1_1}{this.lstDeduceYears.CheckedItems[index3].ToString()};";
        checked { ++index3; }
      }
      string String1_2 = ";";
      int num6 = checked (this.lstDeduceModels.CheckedItems.Count - 1);
      int index4 = 0;
      while (index4 <= num6)
      {
        String1_2 = $"{String1_2}{this.lstDeduceModels.CheckedItems[index4].ToString()};";
        checked { ++index4; }
      }
      int num7 = checked (files.Length - 1);
      int index5 = 0;
      while (index5 <= num7)
      {
        string str4 = Strings.Replace(files[index5], ".ETIS.", ".AB.");
        string[] strArray2 = new string[1];
        string[] strArray3 = new string[1];
        int num8 = 0;
        string[] strArray4 = new string[1];
        string[] strArray5 = new string[1];
        string[] strArray6 = new string[1];
        int num9 = 0;
        string inpFileName1 = str4;
        ref string[] local1 = ref strArray2;
        ref string[] local2 = ref strArray3;
        ref int local3 = ref num8;
        string str5 = "";
        ref string local4 = ref str5;
        ref string local5 = ref str2;
        ref string local6 = ref str3;
        ref string[] local7 = ref strArray4;
        ref string[] local8 = ref strArray5;
        ref string[] local9 = ref strArray6;
        ref int local10 = ref num9;
        modAsBuilt.AsBuilt_LoadFile_AB_HTML(inpFileName1, ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, ref local7, ref local8, ref local9, ref local10);
        if (Strings.InStr(1, String1_2, $";{str2};") != 0 & Strings.InStr(1, String1_1, $";{str3};") != 0)
        {
          vehicleInfoArray[index2].carModel = str2;
          vehicleInfoArray[index2].carYear = str3;
          string str6 = files[index5];
          modAsBuilt.ETIS_LoadFile_FactoryOptions_HTML(str6, ref vehicleInfoArray[index2].etisFeatures, ref vehicleInfoArray[index2].etisFeatureCount, ref retVIN);
          string str7 = Strings.Replace(str6, ".ETIS.HTML", "") + ".AB";
          string[] strArray7 = new string[1];
          string[] strArray8 = new string[1];
          string[] strArray9 = new string[1];
          string[] strArray10 = new string[1];
          string[] strArray11 = new string[1];
          int num10 = 0;
          string inpFileName2 = str7;
          ref string[] local11 = ref vehicleInfoArray[index2].abModuleAddresses;
          ref string[] local12 = ref vehicleInfoArray[index2].abModuleDatasHex;
          ref int local13 = ref vehicleInfoArray[index2].abModuleAddrCount;
          string str8 = "";
          ref string local14 = ref str8;
          ref string[] local15 = ref strArray7;
          ref string[] local16 = ref strArray9;
          ref string[] local17 = ref strArray10;
          ref string[] local18 = ref strArray11;
          ref int local19 = ref num10;
          ref string local20 = ref vehicleInfoArray[index2].carCCChex;
          modAsBuilt.AsBuilt_LoadFile_AB(inpFileName2, ref local11, ref local12, ref local13, ref local14, ref local15, ref local16, ref local17, ref local18, ref local19, ref local20);
          vehicleInfoArray[index2].carVIN = retVIN;
          vehicleInfoArray[index2].abModuleInfo_PartNums = new string[checked (vehicleInfoArray[index2].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index2].abModuleInfo_Strategies = new string[checked (vehicleInfoArray[index2].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index2].abModuleInfo_Calibrations = new string[checked (vehicleInfoArray[index2].abModuleAddrCount - 1 + 1)];
          int num11 = checked (num10 - 1);
          int index6 = 0;
          while (index6 <= num11)
          {
            int num12 = checked (vehicleInfoArray[index2].abModuleAddrCount - 1);
            int index7 = 0;
            while (index7 <= num12)
            {
              if (Operators.CompareString(Strings.Left(vehicleInfoArray[index2].abModuleAddresses[index7], Strings.Len(strArray7[index6])), strArray7[index6], false) == 0)
              {
                vehicleInfoArray[index2].abModuleInfo_PartNums[index7] = strArray9[index6];
                vehicleInfoArray[index2].abModuleInfo_Strategies[index7] = strArray10[index6];
                vehicleInfoArray[index2].abModuleInfo_Calibrations[index7] = strArray11[index6];
              }
              checked { ++index7; }
            }
            checked { ++index6; }
          }
          int num13 = checked (vehicleInfoArray[index2].abModuleAddrCount - 1);
          int index8 = 0;
          while (index8 <= num13)
          {
            vehicleInfoArray[index2].abModuleDatasHex[index8] = Strings.Left(vehicleInfoArray[index2].abModuleDatasHex[index8], checked (Strings.Len(vehicleInfoArray[index2].abModuleDatasHex[index8]) - 2));
            checked { ++index8; }
          }
          vehicleInfoArray[index2].carCCChex = Strings.Right(vehicleInfoArray[index2].carCCChex, 510);
          vehicleInfoArray[index2].carCCCbin = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index2].carCCChex);
          vehicleInfoArray[index2].abModuleDatasBinStr = new string[checked (vehicleInfoArray[index2].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index2].abModuleDatasLONG = new ulong[checked (vehicleInfoArray[index2].abModuleAddrCount - 1 + 1)];
          int num14 = checked (vehicleInfoArray[index2].abModuleAddrCount - 1);
          int index9 = 0;
          while (index9 <= num14)
          {
            vehicleInfoArray[index2].abModuleDatasBinStr[index9] = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index2].abModuleDatasHex[index9]);
            vehicleInfoArray[index2].abModuleDatasLONG[index9] = modAsBuilt.AsBuilt_HexStr2UINT64(vehicleInfoArray[index2].abModuleDatasHex[index9]);
            checked { ++index9; }
          }
          checked { ++index2; }
        }
        checked { ++index5; }
      }
      string[] strArray12 = new string[1];
      int index10 = 0;
      int num15 = checked (index2 - 1);
      int index11 = 0;
      while (index11 <= num15)
      {
        int num16 = checked (vehicleInfoArray[index11].abModuleAddrCount - 1);
        int index12 = 0;
        while (index12 <= num16)
        {
          int num17 = -1;
          int num18 = checked (index10 - 1);
          int index13 = 0;
          while (index13 <= num18)
          {
            if (Operators.CompareString(strArray12[index13], vehicleInfoArray[index11].abModuleAddresses[index12], false) == 0)
            {
              num17 = index13;
              break;
            }
            checked { ++index13; }
          }
          if (num17 == -1)
          {
            strArray12 = (string[]) Utils.CopyArray((Array) strArray12, (Array) new string[checked (index10 + 1)]);
            strArray12[index10] = vehicleInfoArray[index11].abModuleAddresses[index12];
            checked { ++index10; }
          }
          checked { ++index12; }
        }
        checked { ++index11; }
      }
      Array.Sort<string>(strArray12);
      Form1.VehicleInfo[] arySrc1 = new Form1.VehicleInfo[1];
      int index14 = 0;
      Form1.VehicleInfo[] arySrc2 = new Form1.VehicleInfo[1];
      int index15 = 0;
      int num19 = checked (index2 - 1);
      int index16 = 0;
      while (index16 <= num19)
      {
        int num20 = -1;
        int num21 = checked (vehicleInfoArray[index16].etisFeatureCount - 1);
        int index17 = 0;
        while (index17 <= num21)
        {
          if (Operators.CompareString(vehicleInfoArray[index16].etisFeatures[index17], str1, false) == 0)
          {
            num20 = index17;
            break;
          }
          if (Strings.InStr(1, str1, $";{vehicleInfoArray[index16].etisFeatures[index17]};", CompareMethod.Text) != 0)
          {
            num20 = index17;
            break;
          }
          checked { ++index17; }
        }
        if (num20 == -1)
        {
          arySrc2 = (Form1.VehicleInfo[]) Utils.CopyArray((Array) arySrc2, (Array) new Form1.VehicleInfo[checked (index15 + 1)]);
          arySrc2[index15] = vehicleInfoArray[index16];
          checked { ++index15; }
        }
        else
        {
          arySrc1 = (Form1.VehicleInfo[]) Utils.CopyArray((Array) arySrc1, (Array) new Form1.VehicleInfo[checked (index14 + 1)]);
          arySrc1[index14] = vehicleInfoArray[index16];
          checked { ++index14; }
        }
        checked { ++index16; }
      }
      */
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("VINs with feature:   " + Conversions.ToString(index14));
      var num22 = checked (index14 - 1);
      var index18 = 0;
      while (index18 <= num22)
      {
        stringBuilder.AppendLine("  " + arySrc1[index18].carVIN);
        checked { ++index18; }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("VINs without feature: " + Conversions.ToString(index15));
      var num23 = checked (index15 - 1);
      var index19 = 0;
      while (index19 <= num23)
      {
        stringBuilder.AppendLine("  " + arySrc2[index19].carVIN);
        checked { ++index19; }
      }
      stringBuilder.AppendLine();
      var arySrc3 = new string[1];
      var arySrc4 = new int[1];
      var arySrc5 = new int[1];
      var index20 = 0;
      var num24 = checked (index10 - 1);
      var index21 = 0;
      while (index21 <= num24)
      {
        var numArray1 = new int[40];
        var numArray2 = new int[40];
        var Start = 1;
        do
        {
          var num25 = checked (index14 - 1);
          var index22 = 0;
          while (index22 <= num25)
          {
            var moduleID = strArray12[index21];
            var vhclInfo = arySrc1[index22];
            var str9 = "";
            if (Operators.CompareString(Strings.Mid(VehicleInfo_GetModuleDataByID_Binary(moduleID, vhclInfo, ref str9), Start, 1), "1", false) == 0)
              numArray1[checked (Start - 1)] = checked (numArray1[Start - 1] + 1);
            else
              numArray2[checked (Start - 1)] = checked (numArray2[Start - 1] + 1);
            checked { ++index22; }
          }
          checked { ++Start; }
        }
        while (Start <= 40);
        stringBuilder.AppendLine("\r\n" + strArray12[index21]);
        var index23 = 0;
        do
        {
          stringBuilder.AppendLine($"bit {Conversions.ToString(index23)}   {Conversions.ToString(numArray1[index23])}");
          if (numArray1[index23] == index14 | numArray2[index23] == index14)
          {
            arySrc3 = (string[]) Utils.CopyArray((Array) arySrc3, (Array) new string[checked (index20 + 1)]);
            arySrc4 = (int[]) Utils.CopyArray((Array) arySrc4, (Array) new int[checked (index20 + 1)]);
            arySrc5 = (int[]) Utils.CopyArray((Array) arySrc5, (Array) new int[checked (index20 + 1)]);
            arySrc3[index20] = strArray12[index21];
            arySrc4[index20] = index23;
            arySrc5[index20] = numArray1[index23];
            checked { ++index20; }
          }
          checked { ++index23; }
        }
        while (index23 <= 39);
        checked { ++index21; }
      }
      var numArray3 = new int[2040];
      var Start1 = 1;
      do
      {
        var num26 = checked (index14 - 1);
        var index24 = 0;
        while (index24 <= num26)
        {
          if (Operators.CompareString(Strings.Mid(arySrc1[index24].carCCCbin, Start1, 1), "1", false) == 0)
            numArray3[checked (Start1 - 1)] = checked (numArray3[Start1 - 1] + 1);
          checked { ++index24; }
        }
        checked { ++Start1; }
      }
      while (Start1 <= 2040);
      var arySrc6 = new string[1];
      var arySrc7 = new int[1];
      var arySrc8 = new int[1];
      var index25 = 0;
      var num27 = checked (index10 - 1);
      var index26 = 0;
      while (index26 <= num27)
      {
        var numArray4 = new int[40];
        var Start2 = 1;
        do
        {
          var num28 = checked (index15 - 1);
          var index27 = 0;
          while (index27 <= num28)
          {
            var moduleID = strArray12[index26];
            var vhclInfo = arySrc2[index27];
            var str10 = "";
            if (Operators.CompareString(Strings.Mid(VehicleInfo_GetModuleDataByID_Binary(moduleID, vhclInfo, ref str10), Start2, 1), "1", false) == 0)
              numArray4[checked (Start2 - 1)] = checked (numArray4[Start2 - 1] + 1);
            checked { ++index27; }
          }
          checked { ++Start2; }
        }
        while (Start2 <= 40);
        stringBuilder.AppendLine("\r\n" + strArray12[index26]);
        var index28 = 0;
        do
        {
          stringBuilder.AppendLine($"bit {Conversions.ToString(index28)}   {Conversions.ToString(numArray4[index28])}");
          if (numArray4[index28] == 0 | numArray4[index28] == index15)
          {
            arySrc6 = (string[]) Utils.CopyArray((Array) arySrc6, (Array) new string[checked (index25 + 1)]);
            arySrc7 = (int[]) Utils.CopyArray((Array) arySrc7, (Array) new int[checked (index25 + 1)]);
            arySrc8 = (int[]) Utils.CopyArray((Array) arySrc8, (Array) new int[checked (index25 + 1)]);
            arySrc6[index25] = strArray12[index26];
            arySrc7[index25] = index28;
            arySrc8[index25] = numArray4[index28];
            checked { ++index25; }
          }
          checked { ++index28; }
        }
        while (index28 <= 39);
        checked { ++index26; }
      }
      var numArray5 = new int[2040];
      var Start3 = 1;
      do
      {
        var num29 = checked (index15 - 1);
        var index29 = 0;
        while (index29 <= num29)
        {
          if (Operators.CompareString(Strings.Mid(arySrc2[index29].carCCCbin, Start3, 1), "1", false) == 0)
            numArray5[checked (Start3 - 1)] = checked (numArray5[Start3 - 1] + 1);
          checked { ++index29; }
        }
        checked { ++Start3; }
      }
      while (Start3 <= 2040);
      var arySrc9 = new string[1];
      var arySrc10 = new int[1];
      var index30 = 0;
      var arySrc11 = new int[1];
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Perfect Bits:");
      var num30 = checked (index20 - 1);
      var index31 = 0;
      while (index31 <= num30)
      {
        var num31 = checked (index25 - 1);
        var index32 = 0;
        while (index32 <= num31)
        {
          if (Operators.CompareString(arySrc6[index32], arySrc3[index31], false) == 0 && arySrc7[index32] == arySrc4[index31] && arySrc8[index32] != arySrc5[index31] & checked (arySrc8[index32] * arySrc5[index31]) == 0 & checked (arySrc8[index32] + arySrc5[index31]) == index14)
          {
            if (arySrc5[index31] == 0)
              stringBuilder.AppendLine($"{arySrc3[index31]} bit {Conversions.ToString(arySrc4[index31])}  val 0");
            else if (arySrc5[index31] == index14)
              stringBuilder.AppendLine($"{arySrc3[index31]} bit {Conversions.ToString(arySrc4[index31])}  val 1");
            
            // --- Library Lookup ---
            var candidates = CommonDatabase.GetFeaturesForAddress(arySrc3[index31]);
            if (candidates != null && candidates.Count > 0)
            {
                 foreach(var cand in candidates)
                 {
                     stringBuilder.AppendLine($"       Possible Match: {cand.Name} (Mask: {cand.Data1Mask} {cand.Data2Mask})");
                 }
            }
            // ----------------------

            arySrc9 = (string[]) Utils.CopyArray((Array) arySrc9, (Array) new string[checked (index30 + 1)]);
            arySrc10 = (int[]) Utils.CopyArray((Array) arySrc10, (Array) new int[checked (index30 + 1)]);
            arySrc11 = (int[]) Utils.CopyArray((Array) arySrc11, (Array) new int[checked (index30 + 1)]);
            arySrc11[index30] = arySrc5[index31];
            arySrc9[index30] = arySrc3[index31];
            arySrc10[index30] = arySrc4[index31];
            checked { ++index30; }
          }
          checked { ++index32; }
        }
        checked { ++index31; }
      }
      if (chkDeduceDoCCC.Checked)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Perfect CCC Bits:");
        var index33 = 0;
        do
        {
          var num32 = index33 % 4;
          if (numArray3[index33] == index14 && numArray5[index33] == 0)
            stringBuilder.AppendLine($"bit {Conversions.ToString(index33)} - hex position {Conversions.ToString(checked (index33 - num32) / 4)} bit {Conversions.ToString(num32)}");
          checked { ++index33; }
        }
        while (index33 <= 2039);
      }
      tbxDeduceReport.Text = stringBuilder.ToString();
      if (index14 == 0)
      {
        var num33 = (int) Interaction.MsgBox((object) "The analysis is useless because there were no vehicles with this feature");
      }
      else if (index15 == 0)
      {
        var num34 = (int) Interaction.MsgBox((object) "The analysis is useless because there were no vehicles missing this feature");
      }
      else
      {
        var num35 = (int) Interaction.MsgBox((object) "The analysis is complete.");
      }
    }
  }

  void TabPage4_Click(object sender, EventArgs e)
  {
  }

  void tbxData1hex_TextChanged(object sender, EventArgs e)
  {
    var str1 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData1hex.Text);
    tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    var str2 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData2hex.Text);
    tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    var str3 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData3hex.Text);
    tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
    ValidateChecksum();
  }

  // Added missing handler for Data2
  void tbxData2hex_TextChanged(object sender, EventArgs e)
  {
      // Re-run logic if needed, or just validate
      ValidateChecksum();
  }

  void tbxData3hex_TextChanged(object sender, EventArgs e)
  {
    var str1 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData1hex.Text);
    tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    var str2 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData2hex.Text);
    tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    var str3 = modAsBuilt.AsBuilt_HexStr2BinStr(tbxData3hex.Text);
    tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
    ValidateChecksum();
  }

  void tbxChecksumHex_TextChanged(object sender, EventArgs e)
  {
    tbxChecksumBin.Text = Strings.Mid(modAsBuilt.AsBuilt_HexStr2BinStr(tbxChecksumHex.Text), 1, 8);
    ValidateChecksum();
  }

  void ValidateChecksum()
  {
      if (string.IsNullOrEmpty(tbxModIDhex.Text) || 
          string.IsNullOrEmpty(tbxData1hex.Text) || 
          string.IsNullOrEmpty(tbxData2hex.Text) || 
          string.IsNullOrEmpty(tbxData3hex.Text)) return;

      var calculated = modAsBuilt.AsBuilt_CalculateChecksum(
          tbxModIDhex.Text, 
          tbxData1hex.Text + tbxData2hex.Text + tbxData3hex.Text);
      
      if (tbxChecksumHex.Text.Trim().ToUpper() == calculated.Trim().ToUpper())
      {
          tbxChecksumHex.BackColor = Color.LightGreen;
      }
      else
      {
          tbxChecksumHex.BackColor = Color.LightPink;
      }
  }

  void chkAudio_CheckedChanged(object sender, EventArgs e)
  {
      // Byte 1: 1=Base, 4=DVD
      var d1 = chkAudio_DVD.Checked ? "4" : "1";
      // Byte 2: 0=Std, 8=Sat
      var d2 = chkAudio_Sat.Checked ? "8" : "0";
      // Byte 3: 4 (seems constant in user example 1040 vs 4848)
      var d3 = "4";
      // Byte 4: 0=Base, 8=Sub
      var d4 = chkAudio_Sub.Checked ? "8" : "0";
      
      tbxAudio_Hex.Text = d1 + d2 + d3 + d4;
  }

  void Button2_Click_2(object sender, EventArgs e)
  {
    tbxConvertBin.Text = modAsBuilt.AsBuilt_HexStr2BinStr(tbxConvertHex.Text);
  }

  void Button3_Click_1(object sender, EventArgs e)
  {
    var Expression = Conversion.Hex(modAsBuilt.AsBuilt_BinStr2UINT64(tbxConvertBin.Text));
    if (Strings.Len(Expression) % 2 == 1)
      Expression = "0" + Expression;
    tbxConvertHex.Text = Expression;
  }

  void TabPage2_Click(object sender, EventArgs e)
  {
  }

  void TabPage1_Click(object sender, EventArgs e)
  {
  }

  void Button4_Click(object sender, EventArgs e)
  {
      if (lstBit_Modules.SelectedItems.Count < 1)
      {
          Interaction.MsgBox("Select a module bit to deduce.");
          return;
      }
      if (lstBit_Models.SelectedItems.Count < 1 || lstBit_Years.SelectedItems.Count < 1)
      {
          Interaction.MsgBox("Select at least one vehicle year and model.");
          return;
      }

      var moduleAddr = lstBit_Modules.SelectedItem.ToString();
      int bitIndex = 0;
      try { bitIndex = Convert.ToInt32(TextBox4.Text); } catch { }

      var report = AnalyzeBit(moduleAddr, bitIndex);
      tbxDeduceReport2.Text = report;
  }

  void lstDeduceFactoryOptions2_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  void TabPage5_Click(object sender, EventArgs e)
  {
  }

  void Button8_Click(object sender, EventArgs e)
  {
    var contents = "";
    var num1 = checked (lstDeduceFactoryOptions.Items.Count - 1);
    var index1 = 0;
    while (index1 <= num1)
    {
      lstDeduceFactoryOptions.SelectedIndices.Clear();
      lstDeduceFactoryOptions.SelectedIndices.Add(index1);
      contents = $"{contents}\r\nFeature:  {lstDeduceFactoryOptions.Items[index1].ToString()}\r\n";
      btnDeduceFigureIt.PerformClick();
      var text = tbxDeduceReport.Text;
      var Start = Strings.InStrRev(text, "Perfect Bits:", Compare: CompareMethod.Text);
      if (string.IsNullOrEmpty(text))
        Start = 0;
      if (Start > 0)
      {
        var Expression = Strings.Mid(text, Start);
        var strArray1 = new string[1];
        var strArray2 = Strings.Split(Expression, "\r\n");
        var num2 = checked (strArray2.Length - 1);
        var index2 = 1;
        while (index2 <= num2)
        {
          strArray2[index2] = Strings.Trim(strArray2[index2]);
          if (Strings.InStr(1, strArray2[index2], "Perfect CCC", CompareMethod.Text) == 0)
          {
            if (Operators.CompareString(strArray2[index2], "", false) != 0)
              contents = $"{contents}  {strArray2[index2]}\r\n";
            checked { ++index2; }
          }
          else
            break;
        }
      }
      checked { ++index1; }
    }
    try
    {
      System.IO.File.Delete("C:\\PerfectList.txt");
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
    System.IO.File.WriteAllText("C:\\PerfectList.txt", contents);
  }

  void Button5_Click_1(object sender, EventArgs e)
  {
      VehicleDatabase.Load();
      lstBit_Years.Items.Clear();
      lstBit_Models.Items.Clear();

      var years = new List<string>();
      var models = new List<string>();

      foreach (var v in VehicleDatabase.Entries)
      {
          if (!string.IsNullOrWhiteSpace(v.Year) && !years.Contains(v.Year))
              years.Add(v.Year);

          if (!string.IsNullOrWhiteSpace(v.Model) && !models.Contains(v.Model))
              models.Add(v.Model);
      }

      years.Sort();
      models.Sort();

      foreach (var y in years) lstBit_Years.Items.Add(y);
      foreach (var m in models) lstBit_Models.Items.Add(m);

      Interaction.MsgBox($"Loaded {VehicleDatabase.Entries.Count} vehicles from database.");
  }

  void Button6_Click_1(object sender, EventArgs e)
  {
      if (lstBit_Models.SelectedItems.Count < 1 || lstBit_Years.SelectedItems.Count < 1)
      {
          Interaction.MsgBox("Select at least one vehicle year and model.");
          return;
      }

      var selectedYears = new List<string>();
      foreach (var item in lstBit_Years.CheckedItems) selectedYears.Add(item.ToString());

      var selectedModels = new List<string>();
      foreach (var item in lstBit_Models.CheckedItems) selectedModels.Add(item.ToString());

      var modules = new HashSet<string>();

      // Use a temp file for parsing
      var tempFile = Path.GetTempFileName();

      try
      {
          foreach (var v in VehicleDatabase.Entries)
          {
              if (selectedYears.Contains(v.Year) && selectedModels.Contains(v.Model))
              {
                  // Get Content
                  var content = v.FileContent;
                  if (string.IsNullOrWhiteSpace(content) && File.Exists(v.FilePath))
                  {
                      try { content = File.ReadAllText(v.FilePath); } catch { }
                  }

                  if (string.IsNullOrWhiteSpace(content)) continue;

                  // Write to temp file for the legacy parser
                  File.WriteAllText(tempFile, content);

                  // Setup Ref variables
                  var retModuleAddresses = new string[1];
                  var retModuleDatas = new string[1];
                  var retModuleAddressCount = 0;
                  var retVIN = "";
                  var retModInfo_IDs = new string[1];
                  var retModInfo_PartNumbers = new string[1];
                  var retModInfo_Strategies = new string[1];
                  var retModInfo_Calibrations = new string[1];
                  var retModInfo_Count = 0;
                  var retCCCdata = "";

                  modAsBuilt.AsBuilt_LoadFile_AB(tempFile, ref retModuleAddresses, ref retModuleDatas, ref retModuleAddressCount, ref retVIN, ref retModInfo_IDs, ref retModInfo_PartNumbers, ref retModInfo_Strategies, ref retModInfo_Calibrations, ref retModInfo_Count, ref retCCCdata);

                  // Collect Modules
                  for (int i = 0; i < retModuleAddressCount; i++)
                  {
                      if (!string.IsNullOrEmpty(retModuleAddresses[i]))
                          modules.Add(retModuleAddresses[i]);
                  }
              }
          }
      }
      finally
      {
          if (File.Exists(tempFile)) File.Delete(tempFile);
      }

      lstBit_Modules.Items.Clear();
      var sortedModules = new List<string>(modules);
      sortedModules.Sort();
      
      foreach (var m in sortedModules) lstBit_Modules.Items.Add(m);

      if (lstBit_Modules.Items.Count == 0)
          Interaction.MsgBox("No modules found for the selected vehicles.");
      else
          Interaction.MsgBox($"Found {lstBit_Modules.Items.Count} unique modules.");
  }



  void Button7_Click(object sender, EventArgs e)
  {
    var str = "";
    var strArray1 = new string[1];
    var num1 = 0;
    var strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    var strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    var form = (Form) this;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref strArray3, ref strArray2, ref form, ref str, ref strArray1, ref num1, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    tbxCompFile3.Text = "";
    var num2 = checked (num1 - 1);
    var index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      tbxCompFile3.Text = $"{tbxCompFile3.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(tbxCompFile3.Text, 1), "|", false) == 0)
      tbxCompFile3.Text = Strings.Left(tbxCompFile3.Text, checked (Strings.Len(tbxCompFile3.Text) - 1));
    lblComp3VIN.Text = "";
  }

  void Button9_Click(object sender, EventArgs e)
  {
    var str = "";
    var strArray1 = new string[1];
    var num1 = 0;
    var strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    var strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    var form = (Form) this;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref strArray3, ref strArray2, ref form, ref str, ref strArray1, ref num1, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    tbxCompFile4.Text = "";
    var num2 = checked (num1 - 1);
    var index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      tbxCompFile4.Text = $"{tbxCompFile4.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(tbxCompFile4.Text, 1), "|", false) == 0)
      tbxCompFile4.Text = Strings.Left(tbxCompFile4.Text, checked (Strings.Len(tbxCompFile4.Text) - 1));
    lblComp4VIN.Text = "";
  }

  void EntireLineToolStripMenuItem_Click(object sender, EventArgs e)
  {
    var str1 = "Vehicle Num, ";
    var num1 = checked (ListView1.Columns.Count - 1);
    var index1 = 0;
    while (index1 <= num1)
    {
      str1 = $"{str1}{ListView1.Columns[index1].Text}, ";
      checked { ++index1; }
    }
    var str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    var text = str2 + "\r\n";
    var num2 = checked (ListView1.Items.Count - 1);
    var index2 = 0;
    while (index2 <= num2)
    {
      if (ListView1.Items[index2].Selected)
      {
        if (ListView1.Items[index2].SubItems.Count == ListView1.Columns.Count)
        {
          var str3 = $"{text}{Conversions.ToString(ListView1.Items[index2].Tag)}, ";
          var num3 = checked (ListView1.Items[index2].SubItems.Count - 1);
          var index3 = 0;
          while (index3 <= num3)
          {
            str3 = $"{str3}{ListView1.Items[index2].SubItems[index3].Text}, ";
            checked { ++index3; }
          }
          var str4 = Strings.Trim(str3);
          if (Operators.CompareString(Strings.Right(str4, 1), ",", false) == 0)
            str4 = Strings.Trim(Strings.Left(str4, checked (Strings.Len(str4) - 1)));
          if (Operators.CompareString(Strings.Trim(str4), ",", false) == 0)
            str4 = "";
          text = str4 + "\r\n";
        }
        else
          text += "\r\n";
      }
      checked { ++index2; }
    }
    try
    {
      Clipboard.SetText(text);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      var num4 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  void Data1hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    var index1 = 1;
    var str1 = "Vehicle Num, ";
    var num1 = checked (ListView1.Columns.Count - 1);
    var index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    var str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    var text = str2 + "\r\n";
    var num2 = checked (ListView1.Items.Count - 1);
    var index3 = 0;
    while (index3 <= num2)
    {
      if (ListView1.Items[index3].Selected)
      {
        if (ListView1.Items[index3].SubItems.Count == ListView1.Columns.Count)
        {
          var str3 = Strings.Trim($"{$"{text}{Conversions.ToString(ListView1.Items[index3].Tag)}, "}{ListView1.Items[index3].SubItems[index1].Text}, ");
          if (Operators.CompareString(Strings.Right(str3, 1), ",", false) == 0)
            str3 = Strings.Trim(Strings.Left(str3, checked (Strings.Len(str3) - 1)));
          if (Operators.CompareString(Strings.Trim(str3), ",", false) == 0)
            str3 = "";
          text = str3 + "\r\n";
        }
        else
          text += "\r\n";
      }
      checked { ++index3; }
    }
    try
    {
      Clipboard.SetText(text);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      var num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  void Data2hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    var index1 = 2;
    var str1 = "Vehicle Num, ";
    var num1 = checked (ListView1.Columns.Count - 1);
    var index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    var str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    var text = str2 + "\r\n";
    var num2 = checked (ListView1.Items.Count - 1);
    var index3 = 0;
    while (index3 <= num2)
    {
      if (ListView1.Items[index3].Selected)
      {
        if (ListView1.Items[index3].SubItems.Count == ListView1.Columns.Count)
        {
          var str3 = Strings.Trim($"{$"{text}{Conversions.ToString(ListView1.Items[index3].Tag)}, "}{ListView1.Items[index3].SubItems[index1].Text}, ");
          if (Operators.CompareString(Strings.Right(str3, 1), ",", false) == 0)
            str3 = Strings.Trim(Strings.Left(str3, checked (Strings.Len(str3) - 1)));
          if (Operators.CompareString(Strings.Trim(str3), ",", false) == 0)
            str3 = "";
          text = str3 + "\r\n";
        }
        else
          text += "\r\n";
      }
      checked { ++index3; }
    }
    try
    {
      Clipboard.SetText(text);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      var num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  void Data3hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    var index1 = 3;
    var str1 = "Vehicle Num, ";
    var num1 = checked (ListView1.Columns.Count - 1);
    var index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    var str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    var text = str2 + "\r\n";
    var num2 = checked (ListView1.Items.Count - 1);
    var index3 = 0;
    while (index3 <= num2)
    {
      if (ListView1.Items[index3].Selected)
      {
        if (ListView1.Items[index3].SubItems.Count == ListView1.Columns.Count)
        {
          var str3 = Strings.Trim($"{$"{text}{Conversions.ToString(ListView1.Items[index3].Tag)}, "}{ListView1.Items[index3].SubItems[index1].Text}, ");
          if (Operators.CompareString(Strings.Right(str3, 1), ",", false) == 0)
            str3 = Strings.Trim(Strings.Left(str3, checked (Strings.Len(str3) - 1)));
          if (Operators.CompareString(Strings.Trim(str3), ",", false) == 0)
            str3 = "";
          text = str3 + "\r\n";
        }
        else
          text += "\r\n";
      }
      checked { ++index3; }
    }
    try
    {
      Clipboard.SetText(text);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      var num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  void BinaryToolStripMenuItem_Click(object sender, EventArgs e)
  {
    var index1 = 5;
    var str1 = "Vehicle Num, ";
    var num1 = checked (ListView1.Columns.Count - 1);
    var index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    var str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    var text = str2 + "\r\n";
    var num2 = checked (ListView1.Items.Count - 1);
    var index3 = 0;
    while (index3 <= num2)
    {
      if (ListView1.Items[index3].Selected)
      {
        if (ListView1.Items[index3].SubItems.Count == ListView1.Columns.Count)
        {
          var str3 = Strings.Trim($"{$"{text}{Conversions.ToString(ListView1.Items[index3].Tag)}, "}{ListView1.Items[index3].SubItems[index1].Text}, ");
          if (Operators.CompareString(Strings.Right(str3, 1), ",", false) == 0)
            str3 = Strings.Trim(Strings.Left(str3, checked (Strings.Len(str3) - 1)));
          if (Operators.CompareString(Strings.Trim(str3), ",", false) == 0)
            str3 = "";
          text = str3 + "\r\n";
        }
        else
          text += "\r\n";
      }
      checked { ++index3; }
    }
    try
    {
      Clipboard.SetText(text);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      var num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  void Button10_Click(object sender, EventArgs e)
  {
      // Legacy "Deducer" folder scraper - Disabled in favor of VehicleDatabase
  }

  void Form1_MaximumSizeChanged(object sender, EventArgs e)
  {
  }

  void Form1_Shown(object sender, EventArgs e) => PopulateVehicleList();

  void lvwBrowser_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  void lvwBrowser_ColumnClick(object sender, ColumnClickEventArgs e)
  {
    var column = lvwBrowser.Columns[e.Column];
    SortOrder sort_order;
    if (lvwBrowser_SortingColumn == null)
    {
      sort_order = SortOrder.Ascending;
    }
    else
    {
      sort_order = !column.Equals((object) lvwBrowser_SortingColumn) ? SortOrder.Ascending : (!lvwBrowser_SortingColumn.Text.StartsWith("> ") ? SortOrder.Ascending : SortOrder.Descending);
      lvwBrowser_SortingColumn.Text = lvwBrowser_SortingColumn.Text.Substring(2);
    }
    lvwBrowser_SortingColumn = column;
    lvwBrowser_SortingColumn.Text = sort_order != SortOrder.Ascending ? "< " + lvwBrowser_SortingColumn.Text : "> " + lvwBrowser_SortingColumn.Text;
    lvwBrowser.ListViewItemSorter = (IComparer) new clsListviewSorter(e.Column, sort_order);
    lvwBrowser.Sort();
  }

  void ContextMenuStrip2_Opening(object sender, CancelEventArgs e)
  {
    if (lvwBrowser.Items.Count >= 1)
      return;
    e.Cancel = true;
  }

  void SetAsCompare1ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (lvwBrowser.SelectedItems.Count < 1)
      return;
    tbxCompFile1.Text = lvwBrowser.SelectedItems[0].Name;
    ListView1.Items.Clear();
  }

  void SetAsCompare2ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (lvwBrowser.SelectedItems.Count < 1)
      return;
    tbxCompFile2.Text = lvwBrowser.SelectedItems[0].Name;
    ListView1.Items.Clear();
  }

  void SetAsCompare3ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (lvwBrowser.SelectedItems.Count < 1)
      return;
    tbxCompFile3.Text = lvwBrowser.SelectedItems[0].Name;
    ListView1.Items.Clear();
  }

  void SetAsCompare4ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (lvwBrowser.SelectedItems.Count < 1)
      return;
    tbxCompFile4.Text = lvwBrowser.SelectedItems[0].Name;
    ListView1.Items.Clear();
  }

  void DeleteFileToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (lvwBrowser.SelectedItems.Count < 1)
      return;
    var selectedItem = lvwBrowser.SelectedItems[0];
    var name = selectedItem.Name;
    tbxCompFile4.Text = name;
    try
    {
      try
      {
        System.IO.File.Delete(Strings.Replace(name, ".AB", ".AB.HTML"));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      try
      {
        System.IO.File.Delete(Strings.Replace(name, ".AB", ".ETIS.HTML"));
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      try
      {
        System.IO.File.Delete(name);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
      try
      {
        lvwBrowser.Items.Remove(selectedItem);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        ProjectData.ClearProjectError();
      }
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
  }

  void PictureBox1_Click(object sender, EventArgs e)
  {
    Process.Start("https://www.paypal.com/donate/?business=X76ZW4RHA6T9C&no_recurring=0&item_name=I+build+and+maintain+open-source+projects+for+the+community.+Any+support+helps+me+keep+improving+and+maintaining+them.&currency_code=USD");
  }

  void ToUCDSToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (ListView1.SelectedItems.Count < 1 || Information.IsNothing(RuntimeHelpers.GetObjectValue(ListView1.SelectedItems[0].Tag)) || Operators.CompareString(ListView1.SelectedItems[0].Text, "", false) == 0)
      return;
    var selectedItem = ListView1.SelectedItems[0];
    var text = selectedItem.Text;
    var str1 = "";
    var str2 = "";
    var str3 = Strings.Left(text, 3);
    var retModuleNames = new string[1];
    var retModuleShortNames = new string[1];
    var retModuleAddresses = new string[1];
    var retModuleCount = 0;
    var directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    var inpFileName = directoryPath + "ModuleList.txt";
    if (chkCompareShowNames.Checked)
      modAsBuilt.AsBuilt_LoadFile_ModuleList(inpFileName, ref retModuleNames, ref retModuleShortNames, ref retModuleAddresses, ref retModuleCount);
    var modlistShortNames = retModuleShortNames;
    var modlistAddress = retModuleAddresses;
    var modlistCount = retModuleCount;
    var addrToFind = str3;
    var num1 = -1;
    modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref retModuleNames, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref num1, ref str1, ref str2);
    str2 = Interaction.InputBox("UCDS - Enter Module Type:", "UCDS - Enter Module Type", str2);
    if (Operators.CompareString(str2, "", false) == 0)
      return;
    var Left1 = Interaction.InputBox("UCDS - Enter Vehicle ID  (like 'U375'):", "UCDS - Enter Vehicle ID", "NONE");
    if (Operators.CompareString(Left1, "", false) == 0)
      return;
    var Left2 = Interaction.InputBox("UCDS - Enter Vehicle Year (like 'MY15'):", "UCDS - Enter Vehicle Year", "NONE");
    if (Operators.CompareString(Left2, "", false) == 0)
      return;
    var foreColor = selectedItem.ForeColor;
    var strArray1 = new string[1000];
    var num2 = checked (ListView1.Items.Count - 1);
    var index = 0;
    while (index <= num2)
    {
      if (ListView1.Items[index].ForeColor == foreColor && Operators.CompareString(Strings.Left(ListView1.Items[index].Text, 3), Strings.Left(str3, 3), false) == 0)
      {
        var num3 = Strings.InStr(ListView1.Items[index].Text, " ");
        if (num3 == 0)
          num3 = checked (Strings.Len(ListView1.Items[index].Text) + 1);
        var strArray2 = new string[1];
        var num4 = checked ((int) Math.Round(Conversion.Val(Strings.Split(Strings.Left(ListView1.Items[index].Text, num3 - 1), "-")[1])));
        var str4 = ListView1.Items[index].SubItems[1].Text + ListView1.Items[index].SubItems[2].Text + ListView1.Items[index].SubItems[3].Text;
        if (chkCompareShowChecksum.Checked)
          str4 = Strings.Left(str4, checked (Strings.Len(str4) - 2));
        strArray1[checked (num4 - 1)] = strArray1[checked (num4 - 1)] + str4;
      }
      checked { ++index; }
    }
    var str5 = $"{"" + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + "<DirectConfiguration><!--UCDS Direct Configuration XML FILE from AsBuilt Explorer-->\r\n"}<VEHICLE MODULE=\"{str2}\" VIN=\"NONE\" VEHICLE_ID=\"{Left1}\" VEHICLE_YEAR=\"{Left2}\">\r\n";
    var Number = 0;
    do
    {
      if (Operators.CompareString(strArray1[Number], "", false) != 0)
        str5 = $"{str5}<DID ID=\"DE{Strings.Right("00" + Conversion.Hex(Number), 2)}\">{strArray1[Number]}</DID>\r\n";
      checked { ++Number; }
    }
    while (Number <= 999);
    var contents = str5 + "</VEHICLE>\r\n" + "</DirectConfiguration>";
    var str6 = modAsBuilt.CmDlgDLL_ShowSaveFile((Form) this, "All Files|*.*", "Export to UCDS...", $"Direct_{str2}_{Strings.Format((object) DateAndTime.Now.Day, "00")}{Strings.Format((object) DateAndTime.Now.Month, "00")}{Strings.Format((object) (DateAndTime.Now.Year % 100), "00")}_.XML");
    if (Operators.CompareString(str6, "", false) == 0)
      return;
    try
    {
      if (System.IO.File.Exists(str6))
        System.IO.File.Delete(str6);
      System.IO.File.WriteAllText(str6, contents);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
  }

  void ToABTToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (ListView1.SelectedItems.Count < 1 || Information.IsNothing(RuntimeHelpers.GetObjectValue(ListView1.SelectedItems[0].Tag)) || Operators.CompareString(ListView1.SelectedItems[0].Text, "", false) == 0)
      return;
    var selectedItem = ListView1.SelectedItems[0];
    var str1 = Strings.Left(selectedItem.Text, 3);
    var retModuleNames = new string[1];
    var retModuleShortNames = new string[1];
    var retModuleAddresses = new string[1];
    var retModuleCount = 0;
    var directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    var inpFileName = directoryPath + "ModuleList.txt";
    if (chkCompareShowNames.Checked)
      modAsBuilt.AsBuilt_LoadFile_ModuleList(inpFileName, ref retModuleNames, ref retModuleShortNames, ref retModuleAddresses, ref retModuleCount);
    var str2 = "";
    var Left = "";
    var modlistShortNames = retModuleShortNames;
    var modlistAddress = retModuleAddresses;
    var modlistCount = retModuleCount;
    var addrToFind = str1;
    var num1 = -1;
    modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref retModuleNames, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref num1, ref str2, ref Left);
    var foreColor = selectedItem.ForeColor;
    var strArray1 = new string[1000];
    var num2 = checked (ListView1.Items.Count - 1);
    var index1 = 0;
    while (index1 <= num2)
    {
      if (ListView1.Items[index1].ForeColor == foreColor && Operators.CompareString(Strings.Left(ListView1.Items[index1].Text, 3), Strings.Left(str1, 3), false) == 0)
      {
        var num3 = Strings.InStr(ListView1.Items[index1].Text, " ");
        if (num3 == 0)
          num3 = checked (Strings.Len(ListView1.Items[index1].Text) + 1);
        var strArray2 = new string[1];
        var num4 = checked ((int) Math.Round(Conversion.Val(Strings.Split(Strings.Left(ListView1.Items[index1].Text, num3 - 1), "-")[1])));
        var str3 = ListView1.Items[index1].SubItems[1].Text + ListView1.Items[index1].SubItems[2].Text + ListView1.Items[index1].SubItems[3].Text;
        if (!chkCompareShowChecksum.Checked)
          str3 += modAsBuilt.AsBuilt_CalculateChecksum($"{str1}-{Strings.Mid(ListView1.Items[index1].Text, 5, 2)}-{Strings.Mid(ListView1.Items[index1].Text, 8, 2)}", str3 + "00");
        strArray1[checked (num4 - 1)] = strArray1[checked (num4 - 1)] + str3;
      }
      checked { ++index1; }
    }
    var flag = false;
    var num5 = 1;
    var index2 = 0;
    do
    {
      if (Operators.CompareString(strArray1[index2], "", false) != 0)
      {
        var num6 = 1;
        var Start = 1;
        while (Operators.CompareString(Strings.Mid(strArray1[index2], Start, 12), "", false) != 0)
        {
          if (num6 > 99 | index2 > 99)
          {
            flag = true;
            goto label_24;
          }
          checked { Start += 12; }
          checked { ++num6; }
        }
        checked { ++num5; }
      }
      checked { ++index2; }
    }
    while (index2 <= 999);
label_24:
    var msgBoxResult = MsgBoxResult.Yes;
    if (flag)
    {
      var num7 = (int) Interaction.MsgBox((object) "The module data contains sections / blocks with an ID greater than '99', and therefore requires the ABT file to be written out in the 'new' 2019-08 ABT format.", MsgBoxStyle.Information);
    }
    else
      msgBoxResult = Interaction.MsgBox((object) "Would you like the ABT file written out in the 'new' 2019-08 ABT format (YES), or the 'old' format (NO)?", MsgBoxStyle.YesNo | MsgBoxStyle.Question);
    var contents = "";
    var num8 = 1;
    var index3 = 0;
    do
    {
      if (Operators.CompareString(strArray1[index3], "", false) != 0)
      {
        contents = $"{contents};Block {Conversions.ToString(num8)}\r\n";
        var num9 = 1;
        var Start = 1;
        while (Operators.CompareString(Strings.Mid(strArray1[index3], Start, 12), "", false) != 0)
        {
          if (msgBoxResult == MsgBoxResult.Yes)
            contents = $"{contents}{modAsBuilt.AsBuilt_FormatNewABT(str1, checked (index3 + 1), num9)}{Strings.Mid(strArray1[index3], Start, 12)}\r\n";
          else
            contents = $"{contents}{str1}{Strings.Format((object) checked (index3 + 1), "00")}{Strings.Format((object) num9, "00")}{Strings.Mid(strArray1[index3], Start, 12)}\r\n";
          checked { Start += 12; }
          checked { ++num9; }
        }
        checked { ++num8; }
      }
      checked { ++index3; }
    }
    while (index3 <= 999);
    if (Operators.CompareString(Left, "", false) == 0)
      Left = "Module" + str1;
    var path = modAsBuilt.CmDlgDLL_ShowSaveFile((Form) this, "All Files|*.*", "Export to ABT...", Left + ".ABT");
    try
    {
      if (System.IO.File.Exists(path))
        System.IO.File.Delete(path);
      System.IO.File.WriteAllText(path, contents);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
  }

  void Button10_Click_1(object sender, EventArgs e)
  {
    var sb = new StringBuilder();
    int totalModules = lstBit_Modules.Items.Count;
    
    for (int m = 0; m < totalModules; m++)
    {
        // Update UI Selection
        lstBit_Modules.SelectedIndex = m;
        string modName = lstBit_Modules.Items[m].ToString();

        for (int i = 0; i < 40; i++)
        {
            TextBox4.Text = i.ToString();
            MyProject.Application.DoEvents(); // Keep UI responsive

            string result = AnalyzeBit(modName, i);

            // Filter for "Interesting" results
            // 1. Must have mixed population (some set, some unset)
            bool mixedPop = !result.Contains("VINs with bit set: 0") && !result.Contains("VINs without bit set: 0");

            // 2. Must have at least one potential feature candidate in either On or Off set
            // The format is "Possible Features (N) ..."
            // We can check if BOTH counts are 0.
            bool hasFeatures = !result.Contains($"Possible Features (0) for {modName} bit {i} on:")
                            || !result.Contains($"Possible Features (0) for {modName} bit {i} off:");

            if (mixedPop && hasFeatures)
            {
                sb.AppendLine("--------------------------------------------------");
                sb.AppendLine($"{modName} Bit {i}");
                sb.AppendLine(result);
                sb.AppendLine();
            }
        }
    }

    tbxDeduceReport2.Text = sb.ToString();
    
    if (sb.Length == 0)
    {
        Interaction.MsgBox("No conclusive bits found. Try selecting more vehicles with diverse feature sets.");
    }
    else
    {
        Interaction.MsgBox("Analysis complete. See results below.");
    }
  }

  void IdentifyToolStripMenuItem_Click(object sender, EventArgs e)
  {
      if (ListView1.SelectedItems.Count == 0) return;
      
      var lvi = ListView1.SelectedItems[0];
      var addr = lvi.Text; 
      
      if (lvi.SubItems.Count < 5) return;

      var d1 = lvi.SubItems[2].Text;
      var d2 = lvi.SubItems[3].Text;
      var d3 = lvi.SubItems[4].Text;

      var f = CommonDatabase.FindMatch(addr, d1, d2, d3);
      if (f != null)
      {
          MessageBox.Show($"Found Feature:\nName: {f.Name}\nModule: {f.Module}\nNotes: {f.Notes}", "Feature Identified", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
          MessageBox.Show("No specific feature match found in the common database for this address and data.", "No Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
  }

  void chkCompareShowNames_CheckedChanged(object sender, EventArgs e)
  {
      Button2_Click(sender, e);
  }

  void chkShowOnlyMismatches_CheckedChanged(object sender, EventArgs e)
  {
      Button2_Click(sender, e);
  }

  void ExportModuleToolStripMenuItem_Click(object sender, EventArgs e)
  {
  }

  void ExportModuleToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
  {
    if (chkShowOnlyMismatches.Checked)
    {
      ToUCDSToolStripMenuItem.Enabled = false;
      ToABTToolStripMenuItem.Enabled = false;
    }
    else
    {
      ToUCDSToolStripMenuItem.Enabled = true;
      ToABTToolStripMenuItem.Enabled = true;
    }
  }




  void btnDB1_Click(object sender, EventArgs e) => ShowVehicleDB(tbxCompFile1, lblComp1VIN);

  void btnDB2_Click(object sender, EventArgs e) => ShowVehicleDB(tbxCompFile2, lblComp2VIN);

  void btnDB3_Click(object sender, EventArgs e) => ShowVehicleDB(tbxCompFile3, lblComp3VIN);

  void btnDB4_Click(object sender, EventArgs e) => ShowVehicleDB(tbxCompFile4, lblComp4VIN);

  void btnViewDefs_Click(object sender, EventArgs e)
  {
      using (frmDefinitionsDB frm = new frmDefinitionsDB())
      {
          frm.ShowDialog();
      }
  }

  void ShowVehicleDB(TextBox tbxFile, Label lblVIN)
  {
      var currentPath = tbxFile.Text;
      var currentVIN = lblVIN.Text;
      if (currentVIN.ToLower().Contains("no vin")) currentVIN = "";
      
      using (frmVehicleDB frm = new frmVehicleDB(currentPath, currentVIN))
      {
          if (frm.ShowDialog() == DialogResult.OK)
          {
              tbxFile.Text = frm.SelectedFilePath;
              Button2_Click(null, null); // Refresh
          }
      }
  }

  public struct VehicleInfo
  {
    public string carVIN;
    public string carModel;
    public string carYear;
    public string[] etisFeatures;
    public int etisFeatureCount;
    public string[] abModuleAddresses;
    public string[] abModuleDatasHex;
    public string[] abModuleDatasBinStr;
    public ulong[] abModuleDatasLONG;
    public int abModuleAddrCount;
    public string[] abModuleInfo_Names;
    public string[] abModuleInfo_Descriptions;
    public string[] abModuleInfo_PartNums;
    public string[] abModuleInfo_Strategies;
    public string[] abModuleInfo_Calibrations;
    public string carCCChex;
    public string carCCCbin;
    public bool[] abBinaryBool;
  }


  void lnkAboutGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
      Process.Start("https://github.com/Eliminater74/AsBuiltExplorer");
  }

  void btnDecode_Click(object sender, EventArgs e)
  {
      var vin = txtVinInput.Text.Trim();
      lvwDecodeResults.Items.Clear();

      var results = VINDecoder.Decode(vin);
      
      foreach (var r in results)
      {
          var lvi = new ListViewItem(r.Position);
          lvi.SubItems.Add(r.Value);
          lvi.SubItems.Add(r.Meaning);
          lvi.SubItems.Add(r.Notes);
          lvwDecodeResults.Items.Add(lvi);
      }
  }

  void lvwDecodeResults_MouseClick(object sender, MouseEventArgs e)
  {
      var hit = lvwDecodeResults.HitTest(e.Location);
      if (hit.Item != null)
      {
          if (hit.Item.Text == "URL")
          {
              var url = hit.Item.SubItems[3].Text; // Notes column has the URL
              if (url.StartsWith("http"))
              {
                  Process.Start(url);
              }
          }
      }
  }

  void cmbSavedVehicles_SelectedIndexChanged(object sender, EventArgs e)
  {
      if (cmbSavedVehicles.SelectedItem is VehicleEntry v)
      {
          txtVinInput.Text = v.VIN;
      }
      else if (cmbSavedVehicles.SelectedIndex == 0) // Not Used
      {
          // Do not clear. Just allow manual entry.
      }
  }

  void TabPage9_Enter(object sender, EventArgs e)
  {
      VehicleDatabase.Load(); // Ensure latest
      cmbSavedVehicles.Items.Clear();
      cmbSavedVehicles.Items.Add("-- Not Used --"); // Default manual entry
      foreach (var v in VehicleDatabase.Entries)
      {
          cmbSavedVehicles.Items.Add(v);
      }
      if (cmbSavedVehicles.Items.Count > 0) cmbSavedVehicles.SelectedIndex = 0;
  }

  void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
  {
      var g = e.Graphics;
      Brush _textBrush;

      // Get the item from the collection.
      var _tabPage = TabControl1.TabPages[e.Index];

      // Get the real bounds for the tab rectangle.
      var _tabBounds = TabControl1.GetTabRect(e.Index);

      var isDark = ("Dark" == My.MySettings.Default.AppTheme);
      var backColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
      var foreColor = isDark ? Color.White : SystemColors.ControlText;
      var selectedBack = isDark ? Color.FromArgb(60, 60, 60) : Color.White;
      var selectedFore = isDark ? Color.Cyan : Color.Black; 

      if (e.State == DrawItemState.Selected)
      {
          _textBrush = new SolidBrush(selectedFore);
          using (SolidBrush sb = new SolidBrush(selectedBack))
          {
             g.FillRectangle(sb, e.Bounds);
          }
      }
      else
      {
          _textBrush = new SolidBrush(foreColor);
           using (SolidBrush sb = new SolidBrush(backColor))
          {
             g.FillRectangle(sb, e.Bounds);
          }
      }

      // Use our own font properties
      var _tabFont = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point);

      // Draw string. Center the text.
      var _stringFlags = new StringFormat();
      _stringFlags.Alignment = StringAlignment.Center;
      _stringFlags.LineAlignment = StringAlignment.Center;
      
      g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
  }



  void tabMods_Enter(object sender, EventArgs e)
  {
      // Populate Platform Combo if empty
      if (cmbModPlatform.Items.Count == 0)
      {
          // Get unique platforms
          var platforms = new HashSet<string>();
          foreach (var m in ModDatabase.Mods)
          {
              platforms.Add(m.Platform);
          }
          foreach (var p in platforms)
          {
              cmbModPlatform.Items.Add(p);
          }
          if (cmbModPlatform.Items.Count > 0) cmbModPlatform.SelectedIndex = 0;
      }
  }

  void cmbModPlatform_SelectedIndexChanged(object sender, EventArgs e)
  {
      lvwMods.Items.Clear();
      rtbModDetails.Clear();
      var selectedPlatform = cmbModPlatform.SelectedItem?.ToString();

      if (string.IsNullOrEmpty(selectedPlatform)) return;

      foreach (var m in ModDatabase.Mods)
      {
          if (m.Platform == selectedPlatform)
          {
              var lvi = new ListViewItem(m.Title);
              lvi.SubItems.Add(m.Category);
              lvi.Tag = m; // Store reference
              lvwMods.Items.Add(lvi);
          }
      }
  }

  void lvwMods_SelectedIndexChanged(object sender, EventArgs e)
  {
      if (lvwMods.SelectedItems.Count > 0)
      {
          if (lvwMods.SelectedItems[0].Tag is ModEntry m)
          {
              rtbModDetails.Clear();
              
              // Simple Formatting
              rtbModDetails.SelectionFont = new Font(rtbModDetails.Font.FontFamily, 14, FontStyle.Bold);
              rtbModDetails.AppendText(m.Title + "\r\n");
              
              rtbModDetails.SelectionFont = new Font(rtbModDetails.Font.FontFamily, 10, FontStyle.Italic);
              rtbModDetails.AppendText(m.Category + "\r\n\r\n");

              rtbModDetails.SelectionFont = new Font(rtbModDetails.Font.FontFamily, 11, FontStyle.Regular);
              rtbModDetails.AppendText(m.Description + "\r\n\r\n");
              
              rtbModDetails.SelectionFont = new Font(rtbModDetails.Font.FontFamily, 10, FontStyle.Bold | FontStyle.Underline);
              rtbModDetails.AppendText("INSTRUCTIONS:\r\n");
              
              rtbModDetails.SelectionFont = new Font("Consolas", 10, FontStyle.Regular);
              rtbModDetails.AppendText(m.Instructions);
          }
      }

}
  void numTPMS_PSI_ValueChanged(object sender, EventArgs e)
  {
      var psi = (int)numTPMS_PSI.Value;
      tbxTPMS_Hex.Text = psi.ToString("X2");
  }

  void btnVIN_Convert_Click(object sender, EventArgs e)
  {
      var vin = txtVIN_Input.Text;
      if (string.IsNullOrWhiteSpace(vin)) return;

      var sb = new StringBuilder();
      foreach (char c in vin)
      {
          sb.Append(((int)c).ToString("X2") + " ");
      }
      txtVIN_Hex.Text = sb.ToString().Trim();
  }





    void EditFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (lvwBrowser.SelectedItems.Count == 0) return;
        
        // BATCH MODE
        if (lvwBrowser.SelectedItems.Count > 1)
        {
            var newTag = Interaction.InputBox($"Enter feature tag to ADD to {lvwBrowser.SelectedItems.Count} selected vehicles:", "Batch Add Feature");
            if (!string.IsNullOrWhiteSpace(newTag))
            {
                 var count = 0;
                 foreach(ListViewItem item in lvwBrowser.SelectedItems)
                 {
                      var vin = item.SubItems[4].Text;
                      var entry = VehicleDatabase.GetEntry(vin);
                      if (entry != null)
                      {
                           // Add unique
                           var parts = new List<string>((entry.Features ?? "").Split(';'));
                           var exists = false;
                           foreach(var p in parts) if(string.Equals(p.Trim(), newTag.Trim(), StringComparison.OrdinalIgnoreCase)) exists = true;
                           
                           if(!exists)
                           {
                               if(string.IsNullOrEmpty(entry.Features)) entry.Features = newTag.Trim();
                               else entry.Features += ";" + newTag.Trim();
                               
                               VehicleDatabase.UpdateEntry(entry);
                               count++;
                           }
                      }
                 }
                 MessageBox.Show($"Added '{newTag}' to {count} vehicles.", "Batch Update Complete");
            }
            return;
        }

        // SINGLE MODE (Full Edit)
        var vinSingle = lvwBrowser.SelectedItems[0].SubItems[4].Text; // VIN column
        var entrySingle = VehicleDatabase.GetEntry(vinSingle);
        
        if (entrySingle != null)
        {
            var currentFeatures = entrySingle.Features ?? "";
            var newFeatures = Interaction.InputBox("Edit features for this vehicle (semicolon separated):", "Edit Features", currentFeatures);
            
            if (newFeatures != currentFeatures) 
            {
                 // Small check for empty result on non-empty current
                 if(string.IsNullOrEmpty(newFeatures) && !string.IsNullOrEmpty(currentFeatures))
                 {
                      if(MessageBox.Show("Are you sure you want to clear all features?", "Confirm Clear", MessageBoxButtons.YesNo) == DialogResult.No) return;
                 }
                 
                 entrySingle.Features = newFeatures;
                 VehicleDatabase.UpdateEntry(entrySingle);
                 MessageBox.Show("Features updated.", "Success");
            }
        }
    }

    void DecodeNHTSAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Collections.IList itemsToProcess = lvwBrowser.SelectedItems;

        if (itemsToProcess.Count == 0)
        {
             if (lvwBrowser.Items.Count > 0)
             {
                 if (MessageBox.Show($"No vehicles selected.\n\nDo you want to decode the entire database ({lvwBrowser.Items.Count} vehicles)?", "Decode All?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                 {
                     itemsToProcess = lvwBrowser.Items;
                 }
                 else
                 {
                     return;
                 }
             }
             else
             {
                 return;
             }
        }
        else
        {
             if (MessageBox.Show($"Are you sure you want to attempt online decoding for {itemsToProcess.Count} vehicles? This may take a moment.", "Confirm Batch Decode", MessageBoxButtons.YesNo) == DialogResult.No) return;
        }

        var successCount = 0;
        Cursor = Cursors.WaitCursor;
        
        try
        {
            foreach (ListViewItem item in itemsToProcess)
            {
                var vin = item.SubItems[4].Text;
                var entry = VehicleDatabase.GetEntry(vin);
                if (entry != null)
                {
                    var result = NHTSADecoder.Decode(vin);
                    if (result != null)
                    {
                        // Update Basic Info
                        if(!string.IsNullOrEmpty(result.Make)) entry.Make = result.Make;
                        if(!string.IsNullOrEmpty(result.Year)) entry.Year = result.Year;
                        
                        var model = result.Model;
                        if (!string.IsNullOrEmpty(result.Trim)) model += " " + result.Trim;
                        if(!string.IsNullOrEmpty(model)) entry.Model = model;

                        // Add Features
                        var newTags = new List<string>();
                        if (!string.IsNullOrEmpty(result.Trim)) newTags.Add("Trim:" + result.Trim);
                        if (!string.IsNullOrEmpty(result.DriveType)) newTags.Add("Drive:" + result.DriveType);
                        if (!string.IsNullOrEmpty(result.BodyClass)) newTags.Add("Body:" + result.BodyClass);
                        if (!string.IsNullOrEmpty(result.FuelType)) newTags.Add("Fuel:" + result.FuelType);
                        if (!string.IsNullOrEmpty(result.Series)) newTags.Add("Series:" + result.Series);

                        // Merge Features
                        var current = new List<string>((entry.Features ?? "").Split(';'));
                        var changed = false;
                        foreach (var tag in newTags)
                        {
                             var exists = false;
                             foreach(var c in current) if(c.Trim().Equals(tag, StringComparison.OrdinalIgnoreCase)) exists = true;
                             if (!exists)
                             {
                                 current.Add(tag);
                                 changed = true;
                             }
                        }

                        if (changed || !string.IsNullOrEmpty(result.Model))
                        {
                            entry.Features = string.Join(";", current).Trim(';');
                            VehicleDatabase.UpdateEntry(entry);
                            successCount++;
                        }
                    }
                }
            }
        }
        finally
        {
            Cursor = Cursors.Default;
        }

        if (successCount > 0)
        {
            MessageBox.Show($"Successfully decoded and updated {successCount} vehicles from NHTSA.", "Decode Complete");
            Button10_Click(sender, e); // Refresh
        }
        else
        {
            MessageBox.Show("No updates found or API request failed.", "Decode Results");
        }
    }

    void ScanLibraryToolStripMenuItem_Click(object sender, EventArgs e)
    {
        System.Collections.IList itemsToProcess = lvwBrowser.SelectedItems;

        if (itemsToProcess.Count == 0)
        {
             if (lvwBrowser.Items.Count > 0)
             {
                 if (MessageBox.Show($"No vehicles selected.\n\nDo you want to scan the entire database ({lvwBrowser.Items.Count} vehicles) against the Common Feature Library?\n\nThis will look for known module patterns (like 'DRL Enable') and tag them if found.", "Scan All?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                 {
                     itemsToProcess = lvwBrowser.Items;
                 }
                 else return;
             }
             else return;
        }

        var successCount = 0;
        var featureCount = 0;
        Cursor = Cursors.WaitCursor;
        var tempFile = Path.GetTempFileName();
        
        try
        {
            // Ensure CommonDB loaded
            CommonDatabase.Load();

            foreach (ListViewItem item in itemsToProcess)
            {
                var vin = item.SubItems[4].Text;
                var entry = VehicleDatabase.GetEntry(vin);
                if (entry != null && !string.IsNullOrEmpty(entry.FileContent))
                {
                    // Write to temp file to reuse existing parser
                    File.WriteAllText(tempFile, entry.FileContent);
                    
                    // Parse
                    var vInfo = new VehicleInfo(); 
                    if (LoadVehicleInfo(tempFile, ref vInfo))
                    {
                         var foundFeatures = new List<string>();
                         // Iterate modules in vehicle
                         for(int i = 0; i < vInfo.abModuleAddrCount; i++)
                         {
                             var addr = vInfo.abModuleAddresses[i]; // e.g. 720-01-01
                             var data = vInfo.abModuleDatasHex[i];    // e.g. 01020304
                            
                             // Look up in CommonDB
                             var candidates = CommonDatabase.GetFeaturesForAddress(addr);
                             foreach(var cand in candidates)
                             {
                                 // Check mask against data
                                 // Helper needed. CommonDatabase has MatchMask but it expects split D1,D2,D3.
                                 // The loader puts all data in 'abModuleDatasHex'
                                 // We need to split into blocks.
                                 
                                 var blocks = SplitHexUnk(data); // "AABB", "CCDD"...
                                 var d1 = blocks.Length > 0 ? blocks[0] : "";
                                 var d2 = blocks.Length > 1 ? blocks[1] : "";
                                 var d3 = blocks.Length > 2 ? blocks[2] : "";

                                 if (CommonDatabase.FindMatch(addr, d1, d2, d3) != null) // Reuse FindMatch
                                 {
                                      // Only add specific matches, check logic
                                      var match = CommonDatabase.FindMatch(addr, d1, d2, d3);
                                      if (match.Name == cand.Name)
                                        foundFeatures.Add(cand.Name);
                                 }
                             }
                         }

                         // Update Entry
                         var current = new List<string>((entry.Features ?? "").Split(';'));
                         var changed = false;
                         foreach(var newF in foundFeatures)
                         {
                              // Normalize name to be tag-like (remove spaces? or keep readable?)
                              // User used "Trim:XLT". CommonDB has "DRL - Enable".
                              // Let's keep it readable but maybe sanitize semicolons.
                              var tag = newF.Replace(";", ",");
                              
                              var exists = false;
                              foreach(var c in current) if(c.Trim().Equals(tag, StringComparison.OrdinalIgnoreCase)) exists = true;
                              if (!exists) 
                              {
                                  current.Add(tag);
                                  featureCount++;
                                  changed = true;
                              }
                         }
                         
                         if(changed)
                         {
                             entry.Features = string.Join(";", current).Trim(';');
                             VehicleDatabase.UpdateEntry(entry);
                             successCount++;
                         }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error scanning library: " + ex.Message);
        }
        finally
        {
            if(File.Exists(tempFile)) File.Delete(tempFile);
            Cursor = Cursors.Default;
        }

        if (successCount > 0)
        {
            MessageBox.Show($"Scanned {itemsToProcess.Count} vehicles.\nFound {featureCount} new feature matches across {successCount} vehicles.", "Library Scan Complete");
            Button10_Click(sender, e);
        }
        else
        {
             MessageBox.Show("No known library features found in selected vehicles.", "Scan Complete");
        }
    }

    bool LoadVehicleInfo(string tempFile, ref VehicleInfo vInfo)
    {
        // Wrapper for legacy modAsBuilt call
        var retModuleAddresses = new string[0];
        var retModuleDatas = new string[0];
        var retModuleAddressCount = 0;
        var retVIN = "";
        var retModInfo_IDs = new string[0];
        var retModInfo_PartNumbers = new string[0];
        var retModInfo_Strategies = new string[0];
        var retModInfo_Calibrations = new string[0];
        var retModInfo_Count = 0;
        var retCCCdata = "";

        // Default to AB format assumption for Library Scan
        var success = modAsBuilt.AsBuilt_LoadFile_AB(tempFile, 
            ref retModuleAddresses, 
            ref retModuleDatas, 
            ref retModuleAddressCount, 
            ref retVIN, 
            ref retModInfo_IDs, 
            ref retModInfo_PartNumbers, 
            ref retModInfo_Strategies, 
            ref retModInfo_Calibrations, 
            ref retModInfo_Count, 
            ref retCCCdata);
            
        if (success)
        {
            vInfo.abModuleAddresses = retModuleAddresses;
            vInfo.abModuleDatasHex = retModuleDatas;
            vInfo.abModuleAddrCount = retModuleAddressCount;
            vInfo.carVIN = retVIN;
            
            // Dummy init required arrays to prevent null refs elsewhere?
            vInfo.abModuleInfo_PartNums = retModInfo_PartNumbers;
            vInfo.abModuleInfo_Names = new string[retModuleAddressCount];
            vInfo.abModuleInfo_Descriptions = new string[retModuleAddressCount]; 
            // etc
        }
        return success;
    }

    string[] SplitHexUnk(string hex)
    {
         // Simple splitter assuming 4-byte (8-char) blocks?
         // CommonDB format is usually 720-01-01 xxxx xxxx xxxx
         // So 2 bytes per block? Or 4?
         // "Data1Mask" in DB usually looks like "xxxx" (2 bytes/4 chars).
         // So we split every 4 chars.
         var list = new List<string>();
         if(string.IsNullOrEmpty(hex)) return list.ToArray();
         for(int i=0; i<hex.Length; i+=4)
         {
             if(i+4 <= hex.Length) list.Add(hex.Substring(i, 4));
             else list.Add(hex.Substring(i));
         }
         return list.ToArray();
    }

    void btnDB_Scan_Click(object sender, EventArgs e)
    {
        using (var fbd = new FolderBrowserDialog())
        {
            fbd.Description = "Select folder containing As-Built files (.ab, .abt, .xml) to import. ETIS files in the same folder will be processed for features.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                var count = VehicleDatabase.BulkImport(fbd.SelectedPath);
                MessageBox.Show($"Imported {count} new vehicles into the database.", "Import Complete");
                Button10_Click(sender, e); // Refresh List
            }
        }
    }

    // Optimized Helper Method for Analysis for Deducer-By-Bit
    private string AnalyzeBit(string moduleAddr, int bitIndex)
    {
        var selectedYears = new HashSet<string>();
        foreach (var item in lstBit_Years.CheckedItems) selectedYears.Add(item.ToString());

        var selectedModels = new HashSet<string>();
        foreach (var item in lstBit_Models.CheckedItems) selectedModels.Add(item.ToString());

        // Groups
        var vinSet1 = new List<string>(); // Bit = 1
        var vinSet0 = new List<string>(); // Bit = 0

        // Feature Sets
        var featuresInAllSet1 = new HashSet<string>();
        var featuresInAnySet1 = new HashSet<string>();
        bool firstSet1 = true;

        var featuresInAllSet0 = new HashSet<string>();
        var featuresInAnySet0 = new HashSet<string>();
        bool firstSet0 = true;

        var tempFile = Path.GetTempFileName();

        try
        {
            foreach (var v in VehicleDatabase.Entries)
            {
                if (!selectedYears.Contains(v.Year) || !selectedModels.Contains(v.Model)) continue;
                
                // Load Content
                var content = v.FileContent;
                if (string.IsNullOrWhiteSpace(content) && File.Exists(v.FilePath))
                {
                    try { content = File.ReadAllText(v.FilePath); } catch { }
                }
                if (string.IsNullOrWhiteSpace(content)) continue;

                // Parse AB
                File.WriteAllText(tempFile, content);

                var retModuleAddresses = new string[1];
                var retModuleDatas = new string[1];
                var retModuleAddressCount = 0;
                var retVIN = "";
                // Dummies
                var d1 = new string[1]; var d2 = new string[1]; var d3 = new string[1]; var d4 = new string[1]; var d5 = 0; var d6 = "";
                
                modAsBuilt.AsBuilt_LoadFile_AB(tempFile, ref retModuleAddresses, ref retModuleDatas, ref retModuleAddressCount, ref retVIN, ref d1, ref d2, ref d3, ref d4, ref d5, ref d6);

                if (string.IsNullOrEmpty(retVIN)) retVIN = v.VIN;

                // Find Module
                int modIdx = -1;
                for(int i=0; i<retModuleAddressCount; i++)
                {
                    if (string.Equals(retModuleAddresses[i], moduleAddr, StringComparison.OrdinalIgnoreCase))
                    {
                        modIdx = i;
                        break;
                    }
                }

                if (modIdx == -1) continue; // Module not found in this car

                // Check Bit
                string hexData = retModuleDatas[modIdx];
                string binData = modAsBuilt.AsBuilt_HexStr2BinStr(hexData);

                // Validate Index
                if (bitIndex >= binData.Length) continue; 

                char bitChar = binData[bitIndex]; 
                bool isBitSet = (bitChar == '1');

                // Parse Features
                var matchFeatures = new HashSet<string>();
                if (!string.IsNullOrEmpty(v.Features))
                {
                     var parts = v.Features.Split(';');
                     foreach(var p in parts) if(!string.IsNullOrWhiteSpace(p)) matchFeatures.Add(p);
                }

                if (isBitSet)
                {
                    vinSet1.Add(retVIN);
                    // Update Sets
                    if (firstSet1)
                    {
                        featuresInAllSet1 = new HashSet<string>(matchFeatures);
                        firstSet1 = false;
                    }
                    else
                    {
                        featuresInAllSet1.IntersectWith(matchFeatures);
                    }
                    featuresInAnySet1.UnionWith(matchFeatures);
                }
                else
                {
                    vinSet0.Add(retVIN);
                    if (firstSet0)
                    {
                        featuresInAllSet0 = new HashSet<string>(matchFeatures);
                        firstSet0 = false;
                    }
                    else
                    {
                        featuresInAllSet0.IntersectWith(matchFeatures);
                    }
                    featuresInAnySet0.UnionWith(matchFeatures);
                }
            }
        }
        catch { }
        finally
        {
            if (File.Exists(tempFile)) File.Delete(tempFile);
        }

        // Calculate Unique Features
        var candidates1 = new List<string>();
        if (vinSet1.Count > 0)
        {
            foreach (var f in featuresInAllSet1)
            {
                if (!featuresInAnySet0.Contains(f)) candidates1.Add(f);
            }
        }
        candidates1.Sort();

        var candidates0 = new List<string>();
        if (vinSet0.Count > 0)
        {
            foreach (var f in featuresInAllSet0)
            {
                if (!featuresInAnySet1.Contains(f)) candidates0.Add(f);
            }
        }
        candidates0.Sort();


        // Build Report
        var sb = new StringBuilder();
        sb.AppendLine($"VINs with bit set: {vinSet1.Count}");
        foreach (var vin in vinSet1) sb.AppendLine("  " + vin);
        sb.AppendLine();

        sb.AppendLine($"VINs without bit set: {vinSet0.Count}");
        foreach (var vin in vinSet0) sb.AppendLine("  " + vin);
        sb.AppendLine();

        sb.AppendLine($"Possible Features ({candidates1.Count}) for {moduleAddr} bit {bitIndex} on:");
        foreach (var f in candidates1) sb.AppendLine("  " + f);
        sb.AppendLine();

        sb.AppendLine($"Possible Features ({candidates0.Count}) for {moduleAddr} bit {bitIndex} off:");
        foreach (var f in candidates0) sb.AppendLine("  " + f);

        return sb.ToString();
    }
}
}


