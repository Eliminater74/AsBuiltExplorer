
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
  private ColumnHeader lvwBrowser_SortingColumn;
  public int abDownloadTriggered;

  public Form1()
  {
    this.Load += new EventHandler(this.Form1_Load);
    this.MaximumSizeChanged += new EventHandler(this.Form1_MaximumSizeChanged);
    this.Shown += new EventHandler(this.Form1_Shown);
    this.abDownloadTriggered = 0;
    this.InitializeComponent();
  }

  private string VehicleInfo_GetModuleDataByID_Binary(
    string moduleID,
    Form1.VehicleInfo vhclInfo,
    ref string modulePartNum)
  {
    modulePartNum = "";
    int num = checked (vhclInfo.abModuleAddrCount - 1);
    int index = 0;
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

  private string VehicleInfo_GetModuleDataByIDandPart_Binary(
    string moduleID,
    Form1.VehicleInfo vhclInfo)
  {
    int num = checked (vhclInfo.abModuleAddrCount - 1);
    int index = 0;
    while (index <= num)
    {
      if (moduleID.StartsWith(vhclInfo.abModuleAddresses[index]) && !Information.IsNothing((object) vhclInfo.abModuleInfo_PartNums[index]) && moduleID.EndsWith(vhclInfo.abModuleInfo_PartNums[index]))
        return vhclInfo.abModuleDatasBinStr[index];
      checked { ++index; }
    }
    return "";
  }

  private void TextBox3_TextChanged(object sender, EventArgs e)
  {
    string str1 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData1hex.Text);
    this.tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    this.tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    string str2 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData2hex.Text);
    this.tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    this.tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    string str3 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData3hex.Text);
    this.tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    this.tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
  }

  private void Label3_Click(object sender, EventArgs e)
  {
  }

  private void Button1_Click(object sender, EventArgs e)
  {
    this.tbxChecksumHex.Text = modAsBuilt.AsBuilt_CalculateChecksum(this.tbxModIDhex.Text, this.tbxData1hex.Text + this.tbxData2hex.Text + this.tbxData3hex.Text);
    this.tbxChecksumBin.Text = Strings.Mid(modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxChecksumHex.Text), 1, 8);
    string str1 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData1hex.Text);
    this.tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    this.tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    string str2 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData2hex.Text);
    this.tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    this.tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    string str3 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData3hex.Text);
    this.tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    this.tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
  }

  private void Form1_Load(object sender, EventArgs e)
  {
      ModuleDatabase.LoadDatabase();
      ModuleDatabase.LoadDatabase();
      CommonDatabase.Load();
      VehicleDatabase.Load(); // Load existing vehicles
      
      // Auto-Import from AsBuiltData folder
      try
      {
         string asBuiltDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AsBuiltData");
         if (Directory.Exists(asBuiltDir))
         {
             int count = VehicleDatabase.BulkImport(asBuiltDir);
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
      this.Text = $"AsBuiltExplorer v{version.Major}.{version.Minor}.{version.Build}";
      this.lblAboutVersion.Text = $"Version {version}";
  }

  private void pbSettings_Click(object sender, EventArgs e)
  {
      using (var frm = new AsBuiltExplorer.Forms.frmSettings())
      {
          if (frm.ShowDialog() == DialogResult.OK)
          {
              if (frm.ThemeChanged) ApplyTheme();
          }
      }
  }

  private void ApplyTheme()
  {
      bool isDark = ("Dark" == My.MySettings.Default.AppTheme);
      Color backColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
      Color foreColor = isDark ? Color.White : SystemColors.ControlText;
      
      this.BackColor = backColor;
      this.ForeColor = foreColor;
      
      
      
      // Always use OwnerDraw to ensure horizontal text and consistent styling
      TabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;
      
      foreach (Control c in this.Controls)
      {
          UpdateControlTheme(c, backColor, foreColor);
      }
  }

  private void UpdateControlTheme(Control c, Color back, Color fore)
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

  private void Button2_Click(object sender, EventArgs e)
  {

    string[] retModuleNames = new string[1];
    string[] retModuleShortNames = new string[1];
    string[] retModuleAddresses = new string[1];

    string directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    if (this.chkCompareShowNames.Checked)
    {
       // Legacy file loading removed in favor of ModuleDatabase
    }
    string[] strArray1 = new string[1];
    string[] retModuleDatas1 = new string[1];
    int num1 = 0;
    string[] retModInfo_IDs1 = new string[1];
    string[] retModInfo_PartNumbers1 = new string[1];
    string[] retModInfo_Strategies1 = new string[1];
    string[] retModInfo_Calibrations1 = new string[1];
    int retModInfo_Count1 = 0;
    string retCCCdata1 = "";
    string[] strArray2 = new string[1];
    string[] retModuleDatas2 = new string[1];
    int num2 = 0;
    string[] retModInfo_IDs2 = new string[1];
    string[] retModInfo_PartNumbers2 = new string[1];
    string[] retModInfo_Strategies2 = new string[1];
    string[] retModInfo_Calibrations2 = new string[1];
    int retModInfo_Count2 = 0;
    string retCCCdata2 = "";
    string[] strArray3 = new string[1];
    string[] retModuleDatas3 = new string[1];
    int num3 = 0;
    string[] retModInfo_IDs3 = new string[1];
    string[] retModInfo_PartNumbers3 = new string[1];
    string[] retModInfo_Strategies3 = new string[1];
    string[] retModInfo_Calibrations3 = new string[1];
    int retModInfo_Count3 = 0;
    string retCCCdata3 = "";
    string[] strArray4 = new string[1];
    string[] retModuleDatas4 = new string[1];
    int num4 = 0;
    string[] retModInfo_IDs4 = new string[1];
    string[] retModInfo_PartNumbers4 = new string[1];
    string[] retModInfo_Strategies4 = new string[1];
    string[] retModInfo_Calibrations4 = new string[1];
    int retModInfo_Count4 = 0;
    string retCCCdata4 = "";
    string text1 = this.tbxCompFile1.Text.Trim();
    bool flag1 = false;
    string[] strArray5 = new string[1];
    string retVIN1 = "";
    string fileType1 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text1);
    string[] inpFileArray1 = Strings.Split(text1, "|");
    if (Operators.CompareString(fileType1, "ABT", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray1, inpFileArray1.Length, ref strArray1, ref retModuleDatas1, ref num1);
      this.lblComp1VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType1, "AB", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFile_AB(text1, ref strArray1, ref retModuleDatas1, ref num1, ref retVIN1, ref retModInfo_IDs1, ref retModInfo_PartNumbers1, ref retModInfo_Strategies1, ref retModInfo_Calibrations1, ref retModInfo_Count1, ref retCCCdata1);
      this.lblComp1VIN.Text = retVIN1;
    }
    else if (Operators.CompareString(fileType1, "UCDS", false) == 0)
    {
      flag1 = modAsBuilt.AsBuilt_LoadFile_UCDS(text1, ref strArray1, ref retModuleDatas1, ref num1);
      this.lblComp1VIN.Text = "No VIN (UCDS)";
    }
    else if (!string.IsNullOrWhiteSpace(text1))
    {
        MessageBox.Show("Could not load File 1: " + text1 + "\r\nFile not found or unknown format.\r\n(Check if file was moved or deleted)", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    string text2 = this.tbxCompFile2.Text.Trim();
    bool flag2 = false;
    string[] strArray6 = new string[1];
    string retVIN2 = "";
    string fileType2 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text2);
    string[] inpFileArray2 = Strings.Split(text2, "|");
    if (Operators.CompareString(fileType2, "ABT", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray2, inpFileArray2.Length, ref strArray2, ref retModuleDatas2, ref num2);
      this.lblComp2VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType2, "AB", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFile_AB(text2, ref strArray2, ref retModuleDatas2, ref num2, ref retVIN2, ref retModInfo_IDs2, ref retModInfo_PartNumbers2, ref retModInfo_Strategies2, ref retModInfo_Calibrations2, ref retModInfo_Count2, ref retCCCdata2);
      this.lblComp2VIN.Text = retVIN2;
    }
    else if (Operators.CompareString(fileType2, "UCDS", false) == 0)
    {
      flag2 = modAsBuilt.AsBuilt_LoadFile_UCDS(text2, ref strArray2, ref retModuleDatas2, ref num2);
      this.lblComp2VIN.Text = "No VIN (UCDS)";
    }
    else if (!string.IsNullOrWhiteSpace(text2))
    {
        MessageBox.Show("Could not load File 2: " + text2 + "\r\nFile not found or unknown format.\r\n(Check if file was moved or deleted)", "Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    string text3 = this.tbxCompFile3.Text.Trim();
    bool flag3 = false;
    string[] strArray7 = new string[1];
    string retVIN3 = "";
    string fileType3 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text3);
    string[] inpFileArray3 = Strings.Split(text3, "|");
    if (Operators.CompareString(fileType3, "ABT", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray3, inpFileArray3.Length, ref strArray3, ref retModuleDatas3, ref num3);
      this.lblComp3VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType3, "AB", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFile_AB(text3, ref strArray3, ref retModuleDatas3, ref num3, ref retVIN3, ref retModInfo_IDs3, ref retModInfo_PartNumbers3, ref retModInfo_Strategies3, ref retModInfo_Calibrations3, ref retModInfo_Count3, ref retCCCdata3);
      this.lblComp3VIN.Text = retVIN3;
    }
    else if (Operators.CompareString(fileType3, "UCDS", false) == 0)
    {
      flag3 = modAsBuilt.AsBuilt_LoadFile_UCDS(text3, ref strArray3, ref retModuleDatas3, ref num3);
      this.lblComp3VIN.Text = "No VIN (UCDS)";
    }
    string text4 = this.tbxCompFile4.Text.Trim();
    bool flag4 = false;
    string[] strArray8 = new string[1];
    string retVIN4 = "";
    string fileType4 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text4);
    string[] inpFileArray4 = Strings.Split(this.tbxCompFile4.Text, "|");
    if (Operators.CompareString(fileType4, "ABT", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFileArray_ABT(ref inpFileArray4, inpFileArray4.Length, ref strArray4, ref retModuleDatas4, ref num4);
      this.lblComp4VIN.Text = "No VIN (ABT)";
    }
    else if (Operators.CompareString(fileType4, "AB", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFile_AB(text4, ref strArray4, ref retModuleDatas4, ref num4, ref retVIN4, ref retModInfo_IDs4, ref retModInfo_PartNumbers4, ref retModInfo_Strategies4, ref retModInfo_Calibrations4, ref retModInfo_Count4, ref retCCCdata4);
      this.lblComp4VIN.Text = retVIN4;
    }
    else if (Operators.CompareString(fileType4, "UCDS", false) == 0)
    {
      flag4 = modAsBuilt.AsBuilt_LoadFile_UCDS(text4, ref strArray4, ref retModuleDatas4, ref num4);
      this.lblComp4VIN.Text = "No VIN (UCDS)";
    }
    this.ListView1.Items.Clear();
    string retData1_1 = "";
    string retData2_1 = "";
    string retData3_1 = "";
    ListViewItem listViewItem1 = (ListViewItem) null;
    ListViewItem.ListViewSubItem listViewSubItem = (ListViewItem.ListViewSubItem) null;

    string str3 = modAsBuilt.AsBuilt_Ascii2Hex(retVIN1);
    string Right1 = "";
    int num5 = checked (num1 - 1);
    int index1 = 0;
    // Variables reused from above loop logic or just remove re-declaration
    // int num5 and int index1 were already declared above.
    // Resetting them is fine, redeclaring is not.
    num5 = checked (num1 - 1);
    index1 = 0;
    while (index1 <= num5)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas1[index1] = Strings.Left(retModuleDatas1[index1], checked (Strings.Len(retModuleDatas1[index1]) - 2));
      string str4 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray1[index1]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas1[index1], ref retData1_1, ref retData2_1, ref retData3_1);
      string text5 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      string str5 = Strings.Left(str4, 3);
      string Right2 = Strings.Left(str4, 6);
      if (Operators.CompareString(str5, Right1, false) != 0)
      {
        string str6 = "";
        int num7 = index1;
        int num8 = checked (num1 - 1);
        int index2 = num7;
        while (index2 <= num8)
        {
          string retData1_2 = "";
          string retData2_2 = "";
          string retData3_2 = "";
          modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas1[index2], ref retData1_2, ref retData2_2, ref retData3_2);
          string str7 = Strings.Left(modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray1[index2]), 3);
          string Left2 = Strings.Left(str7, 6);
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
        int num9 = Strings.InStr(1, str6, str3);
        int num10 = 0;
        if (num9 != 0)
          num10 = checked (num9 + Strings.Len(str3));
        Right1 = str5;
      }
      string text6 = "";
      string text7 = "";
      string text8 = "";
      int num11 = checked (retModInfo_Count1 - 1);
      int index3 = 0;
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
      string modName = "";
      if (this.chkCompareShowNames.Checked)
      {
        int vinYear = VINDecoder.GetModelYear(retVIN1);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str4);
      }
      ListViewItem listViewItem2 = this.ListView1.Items.Add(str4);
      listViewItem2.SubItems.Add(modName);
      listViewItem2.ForeColor = this.tbxCompFile1.ForeColor;
      listViewItem2.UseItemStyleForSubItems = false;
      listViewItem2.Tag = (object) text1;
      listViewSubItem = listViewItem2.SubItems.Add(retData1_1);
      listViewSubItem = listViewItem2.SubItems.Add(retData2_1);
      listViewSubItem = listViewItem2.SubItems.Add(retData3_1);
      listViewSubItem = listViewItem2.SubItems.Add("");
      listViewSubItem = listViewItem2.SubItems.Add(text5);
      listViewSubItem = listViewItem2.SubItems.Add(text6);
      listViewSubItem = listViewItem2.SubItems.Add(text7);
      listViewSubItem = listViewItem2.SubItems.Add(text8);
      listViewItem1 = this.ListView1.Items.Add("");
      checked { ++index1; }
    }
    int num12 = checked (num2 - 1);
    int index4 = 0;
    while (index4 <= num12)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas2[index4] = Strings.Left(retModuleDatas2[index4], checked (Strings.Len(retModuleDatas2[index4]) - 2));
      string str8 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray2[index4]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas2[index4], ref retData1_1, ref retData2_1, ref retData3_1);
      string modName = "";
      if (this.chkCompareShowNames.Checked)
      {
        int vinYear = VINDecoder.GetModelYear(retVIN2);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str8);
      }
      int index5 = -1;
      int num13 = checked (this.ListView1.Items.Count - 1);
      int index6 = 0;
      while (index6 <= num13)
      {
        if (Operators.CompareString(this.ListView1.Items[index6].Text, str8, false) == 0)
          index5 = index6;
        checked { ++index6; }
      }
      ListViewItem listViewItem3 = new ListViewItem(str8);
      listViewItem3.SubItems.Add(modName);
      if (index5 != -1)
        this.ListView1.Items.Insert(checked (index5 + 1), listViewItem3);
      else
        listViewItem3 = this.ListView1.Items.Add(str8);
      listViewItem3.ForeColor = this.tbxCompFile2.ForeColor;
      listViewItem3.UseItemStyleForSubItems = false;
      listViewItem3.Tag = (object) text2;
      string text9 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      string text10 = "";
      string text11 = "";
      string text12 = "";
      int num14 = checked (retModInfo_Count2 - 1);
      int index7 = 0;
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
      bool flag6 = true;
      if (index5 != -1)
      {
        int num15 = checked (listViewItem3.SubItems.Count - 1);
        int index8 = 0;
        while (index8 <= num15)
        {
          if (Operators.CompareString(listViewItem3.SubItems[index8].Text, this.ListView1.Items[index5].SubItems[index8].Text, false) != 0)
          {
            flag6 = false;
            break;
          }
          checked { ++index8; }
        }
      }
      listViewSubItem = index5 == -1 ? listViewItem3.SubItems.Add("No Match") : (!flag6 ? listViewItem3.SubItems.Add("No") : listViewItem3.SubItems.Add("Yes"));
      listViewSubItem = listViewItem3.SubItems.Add(text9);
      listViewSubItem = listViewItem3.SubItems.Add(text10);
      listViewSubItem = listViewItem3.SubItems.Add(text11);
      listViewSubItem = listViewItem3.SubItems.Add(text12);
      if (index5 == -1)
        listViewItem1 = this.ListView1.Items.Add("");
      checked { ++index4; }
    }
    int num16 = checked (num3 - 1);
    int index9 = 0;
    while (index9 <= num16)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas3[index9] = Strings.Left(retModuleDatas3[index9], checked (Strings.Len(retModuleDatas3[index9]) - 2));
      string str9 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray3[index9]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas3[index9], ref retData1_1, ref retData2_1, ref retData3_1);
      string modName = "";
      if (this.chkCompareShowNames.Checked)
      {
        int vinYear = VINDecoder.GetModelYear(retVIN3);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str9);
      }
      int index10 = -1;
      int num17 = checked (this.ListView1.Items.Count - 1);
      int index11 = 0;
      while (index11 <= num17)
      {
        if (Operators.CompareString(this.ListView1.Items[index11].Text, str9, false) == 0)
          index10 = index11;
        checked { ++index11; }
      }
      ListViewItem listViewItem4 = new ListViewItem(str9);
      listViewItem4.SubItems.Add(modName);
      if (index10 != -1)
        this.ListView1.Items.Insert(checked (index10 + 1), listViewItem4);
      else
        listViewItem4 = this.ListView1.Items.Add(str9);
      listViewItem4.ForeColor = this.tbxCompFile3.ForeColor;
      listViewItem4.UseItemStyleForSubItems = false;
      listViewItem4.Tag = (object) text3;
      string text13 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      string text14 = "";
      string text15 = "";
      string text16 = "";
      int num18 = checked (retModInfo_Count3 - 1);
      int index12 = 0;
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
      bool flag7 = true;
      if (index10 != -1)
      {
        int num19 = checked (listViewItem4.SubItems.Count - 1);
        int index13 = 0;
        while (index13 <= num19)
        {
          if (Operators.CompareString(listViewItem4.SubItems[index13].Text, this.ListView1.Items[index10].SubItems[index13].Text, false) != 0)
          {
            flag7 = false;
            break;
          }
          checked { ++index13; }
        }
      }
      listViewSubItem = index10 == -1 ? listViewItem4.SubItems.Add("No Match") : (!flag7 ? listViewItem4.SubItems.Add("No") : listViewItem4.SubItems.Add("Yes"));
      listViewSubItem = listViewItem4.SubItems.Add(text13);
      listViewSubItem = listViewItem4.SubItems.Add(text14);
      listViewSubItem = listViewItem4.SubItems.Add(text15);
      listViewSubItem = listViewItem4.SubItems.Add(text16);
      if (index10 == -1)
        listViewItem1 = this.ListView1.Items.Add("");
      checked { ++index9; }
    }
    int num20 = checked (num4 - 1);
    int index14 = 0;
    while (index14 <= num20)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas4[index14] = Strings.Left(retModuleDatas4[index14], checked (Strings.Len(retModuleDatas4[index14]) - 2));
      string str10 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray4[index14]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas4[index14], ref retData1_1, ref retData2_1, ref retData3_1);
      string modName = "";
      if (this.chkCompareShowNames.Checked)
      {
        int vinYear = VINDecoder.GetModelYear(retVIN4);
        var strategy = Utilities.VehicleStrategyFactory.GetStrategy(vinYear);
        modName = strategy.GetModuleName(str10);
      }
      int index15 = -1;
      int num21 = checked (this.ListView1.Items.Count - 1);
      int index16 = 0;
      while (index16 <= num21)
      {
        if (Operators.CompareString(this.ListView1.Items[index16].Text, str10, false) == 0)
          index15 = index16;
        checked { ++index16; }
      }
      ListViewItem listViewItem5 = new ListViewItem(str10);
      listViewItem5.SubItems.Add(modName);
      if (index15 != -1)
        this.ListView1.Items.Insert(checked (index15 + 1), listViewItem5);
      else
        listViewItem5 = this.ListView1.Items.Add(str10);
      listViewItem5.ForeColor = this.tbxCompFile4.ForeColor;
      listViewItem5.UseItemStyleForSubItems = false;
      listViewItem5.Tag = (object) text4;
      // Fix CS0103: str2 was removed but this line remained.
      // str2 = ""; // Removed unused assignment
      string text17 = modAsBuilt.AsBuilt_FormatReadable_Binary(modAsBuilt.AsBuilt_HexStr2BinStr(retData1_1 + retData2_1 + retData3_1));
      string text18 = "";
      string text19 = "";
      string text20 = "";
      int num22 = checked (retModInfo_Count4 - 1);
      int index17 = 0;
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
      bool flag8 = true;
      if (index15 != -1)
      {
        int num23 = checked (listViewItem5.SubItems.Count - 1);
        int index18 = 0;
        while (index18 <= num23)
        {
          if (Operators.CompareString(listViewItem5.SubItems[index18].Text, this.ListView1.Items[index15].SubItems[index18].Text, false) != 0)
          {
            flag8 = false;
            break;
          }
          checked { ++index18; }
        }
      }
      listViewSubItem = index15 == -1 ? listViewItem5.SubItems.Add("No Match") : (!flag8 ? listViewItem5.SubItems.Add("No") : listViewItem5.SubItems.Add("Yes"));
      listViewSubItem = listViewItem5.SubItems.Add(text17);
      listViewSubItem = listViewItem5.SubItems.Add(text18);
      listViewSubItem = listViewItem5.SubItems.Add(text19);
      listViewSubItem = listViewItem5.SubItems.Add(text20);
      if (index15 == -1)
        listViewItem1 = this.ListView1.Items.Add("");
      checked { ++index14; }
    }
    if (!flag1 & !flag2 & !flag3 & !flag4)
      return;
    Color color = Color.FromArgb((int) byte.MaxValue, (int) byte.MaxValue, 230, 230);
    Color lightYellow = Color.LightYellow;
    int index19 = 0;
    do
    {
      int num24 = index19;
      int num25 = checked (index19 + 1);
      int num26 = checked (this.ListView1.Items.Count - 1);
      int index20 = num25;
      while (index20 <= num26)
      {
        this.ListView1.Items[index20].Tag = (object) "";
        if (Operators.CompareString(this.ListView1.Items[index20].Text, "", false) != 0)
        {
          num24 = index20;
          checked { ++index20; }
        }
        else
          break;
      }
      int num27 = 1;
      int num28 = index19;
      int num29 = num24;
      int index21 = num28;
      while (index21 <= num29)
      {
        this.ListView1.Items[index21].Tag = (object) Conversions.ToString(num27);
        checked { ++num27; }
        checked { ++index21; }
      }
      string text21 = this.ListView1.Items[index19].SubItems[6].Text;
      bool flag9 = true;
      int num30 = checked (index19 + 1);
      int num31 = num24;
      int index22 = num30;
      while (index22 <= num31)
      {
        if (Operators.CompareString(this.ListView1.Items[index22].SubItems[6].Text, text21, false) != 0)
        {
          flag9 = false;
          break;
        }
        checked { ++index22; }
      }
      int num32 = index19;
      int num33 = num24;
      int index23 = num32;
      while (index23 <= num33)
      {
        if (!flag9)
        {
          this.ListView1.Items[index23].SubItems[5].Text = "No";
          this.ListView1.Items[index23].BackColor = Color.MistyRose;
          int num34 = checked (this.ListView1.Items[index23].SubItems.Count - 1);
          int index24 = 0;
          while (index24 <= num34)
          {
            this.ListView1.Items[index23].SubItems[index24].BackColor = Color.MistyRose;
            checked { ++index24; }
          }
        }
        else
        {
          this.ListView1.Items[index23].SubItems[5].Text = "Yes";
          this.ListView1.Items[index23].BackColor = Color.LightYellow;
          int num35 = checked (this.ListView1.Items[index23].SubItems.Count - 1);
          int index25 = 0;
          while (index25 <= num35)
          {
            this.ListView1.Items[index23].SubItems[index25].BackColor = lightYellow;
            checked { ++index25; }
          }
        }
        checked { ++index23; }
      }
      index19 = checked (num24 + 2);
    }
    while (index19 <= checked (this.ListView1.Items.Count - 2));
    int num36 = 0;
    if (Operators.CompareString(this.tbxCompFile1.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(this.tbxCompFile2.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(this.tbxCompFile3.Text, "", false) != 0)
      checked { ++num36; }
    if (Operators.CompareString(this.tbxCompFile4.Text, "", false) != 0)
      checked { ++num36; }
    if (this.chkShowOnlyMismatches.Checked && num36 > 1)
    {
      int index26 = checked (this.ListView1.Items.Count - 1);
      while (index26 >= 0)
      {
        if (this.ListView1.Items[index26].BackColor == lightYellow)
          this.ListView1.Items.RemoveAt(index26);
        checked { index26 += -1; }
      }
      int index27 = checked (this.ListView1.Items.Count - 1);
      while (index27 >= 1)
      {
        if (Operators.CompareString(this.ListView1.Items[index27].Text, "", false) == 0 && Operators.CompareString(this.ListView1.Items[checked (index27 - 1)].Text, "", false) == 0)
          this.ListView1.Items.RemoveAt(index27);
        checked { index27 += -1; }
      }
    }
    this.ListView1.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
    this.ListView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
  }

  private void btnCompBrowse1_Click(object sender, EventArgs e)
  {
    string str = "";
    string[] strArray1 = new string[1];
    int num1 = 0;
    string[] strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    string[] strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    ref string[] local1 = ref strArray3;
    ref string[] local2 = ref strArray2;
    Form form = (Form) this;
    ref Form local3 = ref form;
    ref string local4 = ref str;
    ref string[] local5 = ref strArray1;
    ref int local6 = ref num1;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    this.tbxCompFile1.Text = "";
    int num2 = checked (num1 - 1);
    int index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      this.tbxCompFile1.Text = $"{this.tbxCompFile1.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(this.tbxCompFile1.Text, 1), "|", false) == 0)
      this.tbxCompFile1.Text = Strings.Left(this.tbxCompFile1.Text, checked (Strings.Len(this.tbxCompFile1.Text) - 1));
    this.lblComp1VIN.Text = "";
  }

  private void btnCompBrowse2_Click(object sender, EventArgs e)
  {
    string str = "";
    string[] strArray1 = new string[1];
    int num1 = 0;
    string[] strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    string[] strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    ref string[] local1 = ref strArray3;
    ref string[] local2 = ref strArray2;
    Form form = (Form) this;
    ref Form local3 = ref form;
    ref string local4 = ref str;
    ref string[] local5 = ref strArray1;
    ref int local6 = ref num1;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    this.tbxCompFile2.Text = "";
    int num2 = checked (num1 - 1);
    int index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      this.tbxCompFile2.Text = $"{this.tbxCompFile2.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(this.tbxCompFile2.Text, 1), "|", false) == 0)
      this.tbxCompFile2.Text = Strings.Left(this.tbxCompFile2.Text, checked (Strings.Len(this.tbxCompFile2.Text) - 1));
    this.lblComp2VIN.Text = "";
  }

  private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
  {
  }

  private void ContextMenuStrip1_Opening(object sender, CancelEventArgs e)
  {
    if (this.ListView1.Items.Count >= 1)
      return;
    e.Cancel = true;
  }

  private void btnDeduceGo_Click(object sender, EventArgs e)
  {
      string vin = txtDeduceVIN.Text.Trim();
      wbDeducer.ScriptErrorsSuppressed = true;
      wbDeducer.Navigate("https://www.motorcraftservice.com/AsBuilt");
  }

  private void wbDeducer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
  {
      string vin = txtDeduceVIN.Text.Trim();
      if (string.IsNullOrEmpty(vin)) return;

      if (wbDeducer.Url.ToString().ToLower().Contains("motorcraftservice.com"))
      {
          HtmlElement vinBox = wbDeducer.Document.GetElementById("VIN");
          if (vinBox == null) vinBox = wbDeducer.Document.GetElementById("vin");
          
          if (vinBox != null)
          {
              vinBox.SetAttribute("value", vin);
          }
      }
  }


  
  private void btnBrowseRefresh_Click(object sender, EventArgs e)
  {
      PopulateVehicleList();
  }

  private void PopulateVehicleList()
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
              string dateStr = "--";
              try {
                  if (!string.IsNullOrEmpty(v.FilePath) && File.Exists(v.FilePath))
                      dateStr = File.GetLastWriteTime(v.FilePath).ToString("g"); // Short Date + Time
              } catch {}
              
              item.SubItems.Add(dateStr);
              
              // Year
              item.SubItems.Add(v.Year ?? "");
              
              // Model (Make + Model)
              string fullModel = (v.Make + " " + v.Model).Trim();
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

  private void cmbDeduceSavedVehicles_SelectedIndexChanged(object sender, EventArgs e)
  {
       if (cmbDeduceSavedVehicles.SelectedItem is VehicleEntry v)
       {
           txtDeduceVIN.Text = v.VIN;
       }
  }

  private void TabPage3_Enter(object sender, EventArgs e)
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



  private void wbDeducer_FileDownload(object sender, EventArgs e) => ++this.abDownloadTriggered;

  private void btnDeduceLoadOptions_Click(object sender, EventArgs e)
  {
      this.lstDeduceFactoryOptions.Items.Clear();
      this.lstDeduceModels.Items.Clear();
      this.lstDeduceYears.Items.Clear();

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

  private void btnDeduceFigureIt_Click(object sender, EventArgs e)
  {
    this.tbxDeduceReport.Text = "";
    if (Information.IsNothing(RuntimeHelpers.GetObjectValue(this.lstDeduceFactoryOptions.SelectedItem)))
    {
      int num1 = (int) Interaction.MsgBox((object) "Select a feature to deduce.");
    }
    else if (Information.IsNothing((object) this.lstDeduceModels.SelectedItems) || this.lstDeduceModels.SelectedItems.Count < 1)
    {
      int num2 = (int) Interaction.MsgBox((object) "Select at least one vehicle model.");
    }
    else if (Information.IsNothing((object) this.lstDeduceYears.SelectedItems) || this.lstDeduceYears.SelectedItems.Count < 1)
    {
      int num3 = (int) Interaction.MsgBox((object) "Select at least one vehicle year.");
    }
    else
    {
      // Refactored Logic: Filter from Database
      string feature = lstDeduceFactoryOptions.SelectedItem.ToString();
      var entries = VehicleDatabase.Entries;
      var withFeature = new List<VehicleEntry>();
      var withoutFeature = new List<VehicleEntry>();
      
      // Helper to check if entry matches selected filters
      foreach(var entry in entries)
      {
           // Filter by Model
           bool modelMatch = false;
           foreach(var m in lstDeduceModels.CheckedItems)
           {
               if (string.Equals(entry.Model, m.ToString(), StringComparison.OrdinalIgnoreCase)) { modelMatch = true; break;}
           }
           
           // Filter by Year
           bool yearMatch = false;
           foreach(var y in lstDeduceYears.CheckedItems)
           {
               if (string.Equals(entry.Year, y.ToString(), StringComparison.OrdinalIgnoreCase)) { yearMatch = true; break; }
           }

           if(modelMatch && yearMatch)
           {
               // Check Feature
               bool hasIt = false;
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
          
          string[] addrs = new string[0];
          string[] datas = new string[0];
          int count = 0;
          string _dummy = "";
          string[] dummyArr1 = new string[0];
          string[] dummyArr2 = new string[0];
          string[] dummyArr3 = new string[0];
          int dummyInt = 0;
          string dummyStr = "";
          
          // Write temp to use existing parser
          string tempFile = Path.GetTempFileName();
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
      Form1.VehicleInfo[] arySrc1 = new Form1.VehicleInfo[withFeature.Count];
      for(int i=0; i<withFeature.Count; i++) arySrc1[i] = MapEntry(withFeature[i]);
      
      Form1.VehicleInfo[] arySrc2 = new Form1.VehicleInfo[withoutFeature.Count];
      for(int i=0; i<withoutFeature.Count; i++) arySrc2[i] = MapEntry(withoutFeature[i]);

      int index14 = withFeature.Count;
      int index15 = withoutFeature.Count;
      
      // Collect Module IDs for sorting (strArray12 replacement)
      var allMods = new HashSet<string>();
      foreach(var v in arySrc1) if(v.abModuleAddresses!=null) foreach(var m in v.abModuleAddresses) allMods.Add(m);
      foreach(var v in arySrc2) if(v.abModuleAddresses!=null) foreach(var m in v.abModuleAddresses) allMods.Add(m);
      
      string[] strArray12 = new List<string>(allMods).ToArray();
      Array.Sort(strArray12);
      int index10 = strArray12.Length;

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
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("VINs with feature:   " + Conversions.ToString(index14));
      int num22 = checked (index14 - 1);
      int index18 = 0;
      while (index18 <= num22)
      {
        stringBuilder.AppendLine("  " + arySrc1[index18].carVIN);
        checked { ++index18; }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("VINs without feature: " + Conversions.ToString(index15));
      int num23 = checked (index15 - 1);
      int index19 = 0;
      while (index19 <= num23)
      {
        stringBuilder.AppendLine("  " + arySrc2[index19].carVIN);
        checked { ++index19; }
      }
      stringBuilder.AppendLine();
      string[] arySrc3 = new string[1];
      int[] arySrc4 = new int[1];
      int[] arySrc5 = new int[1];
      int index20 = 0;
      int num24 = checked (index10 - 1);
      int index21 = 0;
      while (index21 <= num24)
      {
        int[] numArray1 = new int[40];
        int[] numArray2 = new int[40];
        int Start = 1;
        do
        {
          int num25 = checked (index14 - 1);
          int index22 = 0;
          while (index22 <= num25)
          {
            string moduleID = strArray12[index21];
            Form1.VehicleInfo vhclInfo = arySrc1[index22];
            string str9 = "";
            ref string local = ref str9;
            if (Operators.CompareString(Strings.Mid(this.VehicleInfo_GetModuleDataByID_Binary(moduleID, vhclInfo, ref local), Start, 1), "1", false) == 0)
              numArray1[checked (Start - 1)] = checked (numArray1[Start - 1] + 1);
            else
              numArray2[checked (Start - 1)] = checked (numArray2[Start - 1] + 1);
            checked { ++index22; }
          }
          checked { ++Start; }
        }
        while (Start <= 40);
        stringBuilder.AppendLine("\r\n" + strArray12[index21]);
        int index23 = 0;
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
      int[] numArray3 = new int[2040];
      int Start1 = 1;
      do
      {
        int num26 = checked (index14 - 1);
        int index24 = 0;
        while (index24 <= num26)
        {
          if (Operators.CompareString(Strings.Mid(arySrc1[index24].carCCCbin, Start1, 1), "1", false) == 0)
            numArray3[checked (Start1 - 1)] = checked (numArray3[Start1 - 1] + 1);
          checked { ++index24; }
        }
        checked { ++Start1; }
      }
      while (Start1 <= 2040);
      string[] arySrc6 = new string[1];
      int[] arySrc7 = new int[1];
      int[] arySrc8 = new int[1];
      int index25 = 0;
      int num27 = checked (index10 - 1);
      int index26 = 0;
      while (index26 <= num27)
      {
        int[] numArray4 = new int[40];
        int Start2 = 1;
        do
        {
          int num28 = checked (index15 - 1);
          int index27 = 0;
          while (index27 <= num28)
          {
            string moduleID = strArray12[index26];
            Form1.VehicleInfo vhclInfo = arySrc2[index27];
            string str10 = "";
            ref string local = ref str10;
            if (Operators.CompareString(Strings.Mid(this.VehicleInfo_GetModuleDataByID_Binary(moduleID, vhclInfo, ref local), Start2, 1), "1", false) == 0)
              numArray4[checked (Start2 - 1)] = checked (numArray4[Start2 - 1] + 1);
            checked { ++index27; }
          }
          checked { ++Start2; }
        }
        while (Start2 <= 40);
        stringBuilder.AppendLine("\r\n" + strArray12[index26]);
        int index28 = 0;
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
      int[] numArray5 = new int[2040];
      int Start3 = 1;
      do
      {
        int num29 = checked (index15 - 1);
        int index29 = 0;
        while (index29 <= num29)
        {
          if (Operators.CompareString(Strings.Mid(arySrc2[index29].carCCCbin, Start3, 1), "1", false) == 0)
            numArray5[checked (Start3 - 1)] = checked (numArray5[Start3 - 1] + 1);
          checked { ++index29; }
        }
        checked { ++Start3; }
      }
      while (Start3 <= 2040);
      string[] arySrc9 = new string[1];
      int[] arySrc10 = new int[1];
      int index30 = 0;
      int[] arySrc11 = new int[1];
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("Perfect Bits:");
      int num30 = checked (index20 - 1);
      int index31 = 0;
      while (index31 <= num30)
      {
        int num31 = checked (index25 - 1);
        int index32 = 0;
        while (index32 <= num31)
        {
          if (Operators.CompareString(arySrc6[index32], arySrc3[index31], false) == 0 && arySrc7[index32] == arySrc4[index31] && arySrc8[index32] != arySrc5[index31] & checked (arySrc8[index32] * arySrc5[index31]) == 0 & checked (arySrc8[index32] + arySrc5[index31]) == index14)
          {
            if (arySrc5[index31] == 0)
              stringBuilder.AppendLine($"{arySrc3[index31]} bit {Conversions.ToString(arySrc4[index31])}  val 0");
            else if (arySrc5[index31] == index14)
              stringBuilder.AppendLine($"{arySrc3[index31]} bit {Conversions.ToString(arySrc4[index31])}  val 1");
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
      if (this.chkDeduceDoCCC.Checked)
      {
        stringBuilder.AppendLine();
        stringBuilder.AppendLine();
        stringBuilder.AppendLine("Perfect CCC Bits:");
        int index33 = 0;
        do
        {
          int num32 = index33 % 4;
          if (numArray3[index33] == index14 && numArray5[index33] == 0)
            stringBuilder.AppendLine($"bit {Conversions.ToString(index33)} - hex position {Conversions.ToString(checked (index33 - num32) / 4)} bit {Conversions.ToString(num32)}");
          checked { ++index33; }
        }
        while (index33 <= 2039);
      }
      this.tbxDeduceReport.Text = stringBuilder.ToString();
      if (index14 == 0)
      {
        int num33 = (int) Interaction.MsgBox((object) "The analysis is useless because there were no vehicles with this feature");
      }
      else if (index15 == 0)
      {
        int num34 = (int) Interaction.MsgBox((object) "The analysis is useless because there were no vehicles missing this feature");
      }
      else
      {
        int num35 = (int) Interaction.MsgBox((object) "The analysis is complete.");
      }
    }
  }

  private void TabPage4_Click(object sender, EventArgs e)
  {
  }

  private void tbxData1hex_TextChanged(object sender, EventArgs e)
  {
    string str1 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData1hex.Text);
    this.tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    this.tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    string str2 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData2hex.Text);
    this.tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    this.tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    string str3 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData3hex.Text);
    this.tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    this.tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
    ValidateChecksum();
  }

  // Added missing handler for Data2
  private void tbxData2hex_TextChanged(object sender, EventArgs e)
  {
      // Re-run logic if needed, or just validate
      ValidateChecksum();
  }

  private void tbxData3hex_TextChanged(object sender, EventArgs e)
  {
    string str1 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData1hex.Text);
    this.tbxData1bin1.Text = Strings.Mid(str1, 1, 8);
    this.tbxData1bin2.Text = Strings.Mid(str1, 9, 8);
    string str2 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData2hex.Text);
    this.tbxData2bin1.Text = Strings.Mid(str2, 1, 8);
    this.tbxData2bin2.Text = Strings.Mid(str2, 9, 8);
    string str3 = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxData3hex.Text);
    this.tbxData3bin1.Text = Strings.Mid(str3, 1, 8);
    this.tbxData3bin2.Text = Strings.Mid(str3, 9, 8);
    ValidateChecksum();
  }

  private void tbxChecksumHex_TextChanged(object sender, EventArgs e)
  {
    this.tbxChecksumBin.Text = Strings.Mid(modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxChecksumHex.Text), 1, 8);
    ValidateChecksum();
  }

  private void ValidateChecksum()
  {
      if (string.IsNullOrEmpty(tbxModIDhex.Text) || 
          string.IsNullOrEmpty(tbxData1hex.Text) || 
          string.IsNullOrEmpty(tbxData2hex.Text) || 
          string.IsNullOrEmpty(tbxData3hex.Text)) return;

      string calculated = modAsBuilt.AsBuilt_CalculateChecksum(
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

  private void chkAudio_CheckedChanged(object sender, EventArgs e)
  {
      // Byte 1: 1=Base, 4=DVD
      string d1 = chkAudio_DVD.Checked ? "4" : "1";
      // Byte 2: 0=Std, 8=Sat
      string d2 = chkAudio_Sat.Checked ? "8" : "0";
      // Byte 3: 4 (seems constant in user example 1040 vs 4848)
      string d3 = "4";
      // Byte 4: 0=Base, 8=Sub
      string d4 = chkAudio_Sub.Checked ? "8" : "0";
      
      tbxAudio_Hex.Text = d1 + d2 + d3 + d4;
  }

  private void Button2_Click_2(object sender, EventArgs e)
  {
    this.tbxConvertBin.Text = modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxConvertHex.Text);
  }

  private void Button3_Click_1(object sender, EventArgs e)
  {
    string Expression = Conversion.Hex(modAsBuilt.AsBuilt_BinStr2UINT64(this.tbxConvertBin.Text));
    if (Strings.Len(Expression) % 2 == 1)
      Expression = "0" + Expression;
    this.tbxConvertHex.Text = Expression;
  }

  private void TabPage2_Click(object sender, EventArgs e)
  {
  }

  private void TabPage1_Click(object sender, EventArgs e)
  {
  }

  private void Button4_Click(object sender, EventArgs e)
  {
    this.tbxDeduceReport2.Text = "";
    if (Information.IsNothing(RuntimeHelpers.GetObjectValue(this.lstBit_Modules.SelectedItem)) || this.lstBit_Modules.SelectedItems.Count < 1)
    {
      int num1 = (int) Interaction.MsgBox((object) "Select a module bit to deduce.");
    }
    else if (Information.IsNothing((object) this.lstBit_Models.SelectedItems) || this.lstBit_Models.SelectedItems.Count < 1)
    {
      int num2 = (int) Interaction.MsgBox((object) "Select at least one vehicle model.");
    }
    else if (Information.IsNothing((object) this.lstBit_Years.SelectedItems) || this.lstBit_Years.SelectedItems.Count < 1)
    {
      int num3 = (int) Interaction.MsgBox((object) "Select at least one vehicle year.");
    }
    else
    {
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
      int num4 = -1;
      string[] strArray1 = new string[1];
      string[] files = Directory.GetFiles(path, "*.ETIS.HTML");
      Form1.VehicleInfo[] vehicleInfoArray = new Form1.VehicleInfo[checked (files.Length + 1)];
      int index1 = 0;
      string retVIN = "";
      string str1 = "";
      string str2 = "";
      string String1_1 = ";";
      int num5 = checked (this.lstBit_Years.CheckedItems.Count - 1);
      int index2 = 0;
      while (index2 <= num5)
      {
        String1_1 = $"{String1_1}{this.lstBit_Years.CheckedItems[index2].ToString()};";
        checked { ++index2; }
      }
      string String1_2 = ";";
      int num6 = checked (this.lstBit_Models.CheckedItems.Count - 1);
      int index3 = 0;
      while (index3 <= num6)
      {
        String1_2 = $"{String1_2}{this.lstBit_Models.CheckedItems[index3].ToString()};";
        checked { ++index3; }
      }
      string Right = this.lstBit_Modules.SelectedItem.ToString();
      int num7 = checked ((int) Math.Round(Conversion.Val(this.TextBox4.Text)));
      int num8 = checked (files.Length - 1);
      int index4 = 0;
      while (index4 <= num8)
      {
        string str3 = Strings.Replace(files[index4], ".ETIS.", ".AB.");
        string[] strArray2 = new string[1];
        string[] strArray3 = new string[1];
        int num9 = 0;
        string[] strArray4 = new string[1];
        string[] strArray5 = new string[1];
        string[] strArray6 = new string[1];
        int num10 = 0;
        string inpFileName1 = str3;
        ref string[] local1 = ref strArray2;
        ref string[] local2 = ref strArray3;
        ref int local3 = ref num9;
        string str4 = "";
        ref string local4 = ref str4;
        ref string local5 = ref str1;
        ref string local6 = ref str2;
        ref string[] local7 = ref strArray4;
        ref string[] local8 = ref strArray5;
        ref string[] local9 = ref strArray6;
        ref int local10 = ref num10;
        modAsBuilt.AsBuilt_LoadFile_AB_HTML(inpFileName1, ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, ref local7, ref local8, ref local9, ref local10);
        if (Strings.InStr(1, String1_2, $";{str1};") != 0 & Strings.InStr(1, String1_1, $";{str2};") != 0)
        {
          vehicleInfoArray[index1].carModel = str1;
          vehicleInfoArray[index1].carYear = str2;
          string str5 = files[index4];
          modAsBuilt.ETIS_LoadFile_FactoryOptions_HTML(str5, ref vehicleInfoArray[index1].etisFeatures, ref vehicleInfoArray[index1].etisFeatureCount, ref retVIN);
          string str6 = Strings.Replace(str5, ".ETIS.HTML", "") + ".AB";
          string[] strArray7 = new string[1];
          string[] strArray8 = new string[1];
          string[] strArray9 = new string[1];
          string[] strArray10 = new string[1];
          string[] strArray11 = new string[1];
          int num11 = 0;
          string inpFileName2 = str6;
          ref string[] local11 = ref vehicleInfoArray[index1].abModuleAddresses;
          ref string[] local12 = ref vehicleInfoArray[index1].abModuleDatasHex;
          ref int local13 = ref vehicleInfoArray[index1].abModuleAddrCount;
          str4 = "";
          ref string local14 = ref str4;
          ref string[] local15 = ref strArray7;
          ref string[] local16 = ref strArray9;
          ref string[] local17 = ref strArray10;
          ref string[] local18 = ref strArray11;
          ref int local19 = ref num11;
          ref string local20 = ref vehicleInfoArray[index1].carCCChex;
          modAsBuilt.AsBuilt_LoadFile_AB(inpFileName2, ref local11, ref local12, ref local13, ref local14, ref local15, ref local16, ref local17, ref local18, ref local19, ref local20);
          bool flag = false;
          int num12 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
          int index5 = 0;
          while (index5 <= num12)
          {
            if (Operators.CompareString(vehicleInfoArray[index1].abModuleAddresses[index5], Right, false) == 0 && checked (vehicleInfoArray[index1].abModuleDatasHex[index5].Length - 2 * 4) > num7)
            {
              flag = true;
              break;
            }
            checked { ++index5; }
          }
          if (flag)
          {
            vehicleInfoArray[index1].carVIN = retVIN;
            vehicleInfoArray[index1].abModuleInfo_PartNums = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
            vehicleInfoArray[index1].abModuleInfo_Strategies = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
            vehicleInfoArray[index1].abModuleInfo_Calibrations = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
            int num13 = checked (num11 - 1);
            int index6 = 0;
            while (index6 <= num13)
            {
              int num14 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
              int index7 = 0;
              while (index7 <= num14)
              {
                if (Operators.CompareString(Strings.Left(vehicleInfoArray[index1].abModuleAddresses[index7], Strings.Len(strArray7[index6])), strArray7[index6], false) == 0)
                {
                  vehicleInfoArray[index1].abModuleInfo_PartNums[index7] = strArray9[index6];
                  vehicleInfoArray[index1].abModuleInfo_Strategies[index7] = strArray10[index6];
                  vehicleInfoArray[index1].abModuleInfo_Calibrations[index7] = strArray11[index6];
                }
                checked { ++index7; }
              }
              checked { ++index6; }
            }
            int num15 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
            int index8 = 0;
            while (index8 <= num15)
            {
              vehicleInfoArray[index1].abModuleDatasHex[index8] = Strings.Left(vehicleInfoArray[index1].abModuleDatasHex[index8], checked (Strings.Len(vehicleInfoArray[index1].abModuleDatasHex[index8]) - 2));
              checked { ++index8; }
            }
            vehicleInfoArray[index1].carCCChex = Strings.Right(vehicleInfoArray[index1].carCCChex, 510);
            vehicleInfoArray[index1].carCCCbin = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index1].carCCChex);
            vehicleInfoArray[index1].abModuleDatasBinStr = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
            vehicleInfoArray[index1].abModuleDatasLONG = new ulong[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
            int num16 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
            int index9 = 0;
            while (index9 <= num16)
            {
              vehicleInfoArray[index1].abModuleDatasBinStr[index9] = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index1].abModuleDatasHex[index9]);
              vehicleInfoArray[index1].abModuleDatasLONG[index9] = modAsBuilt.AsBuilt_HexStr2UINT64(vehicleInfoArray[index1].abModuleDatasHex[index9]);
              checked { ++index9; }
            }
            checked { ++index1; }
          }
        }
        checked { ++index4; }
      }
      Form1.VehicleInfo[] arySrc1 = new Form1.VehicleInfo[1];
      int index10 = 0;
      Form1.VehicleInfo[] arySrc2 = new Form1.VehicleInfo[1];
      int index11 = 0;
      int num17 = checked (index1 - 1);
      int index12 = 0;
      while (index12 <= num17)
      {
        num4 = -1;
        int num18 = checked (vehicleInfoArray[index12].abModuleAddrCount - 1);
        int index13 = 0;
        while (index13 <= num18)
        {
          if (Operators.CompareString(vehicleInfoArray[index12].abModuleAddresses[index13], Right, false) == 0)
          {
            if (Operators.CompareString(Strings.Mid(vehicleInfoArray[index12].abModuleDatasBinStr[index13], checked (num7 + 1), 1), "1", false) == 0)
            {
              num4 = index13;
              break;
            }
            break;
          }
          checked { ++index13; }
        }
        if (num4 == -1)
        {
          arySrc2 = (Form1.VehicleInfo[]) Utils.CopyArray((Array) arySrc2, (Array) new Form1.VehicleInfo[checked (index11 + 1)]);
          arySrc2[index11] = vehicleInfoArray[index12];
          checked { ++index11; }
        }
        else
        {
          arySrc1 = (Form1.VehicleInfo[]) Utils.CopyArray((Array) arySrc1, (Array) new Form1.VehicleInfo[checked (index10 + 1)]);
          arySrc1[index10] = vehicleInfoArray[index12];
          checked { ++index10; }
        }
        checked { ++index12; }
      }
      string[] dstArray = new string[1];
      int dstItemCount = 0;
      int num19 = checked (index1 - 1);
      int index14 = 0;
      while (index14 <= num19)
      {
        Array.Sort<string>(vehicleInfoArray[index14].etisFeatures);
        int num20 = checked (vehicleInfoArray[index14].etisFeatureCount - 1);
        int index15 = 0;
        while (index15 <= num20)
        {
          if (!string.IsNullOrEmpty(vehicleInfoArray[index14].etisFeatures[index15]))
            modAsBuilt.ArraySsorted_AddItem(ref dstArray, ref dstItemCount, vehicleInfoArray[index14].etisFeatures[index15], true);
          checked { ++index15; }
        }
        checked { ++index14; }
      }
      dstArray = (string[]) Utils.CopyArray((Array) dstArray, (Array) new string[checked (dstItemCount - 1 + 1)]);
      string[] strArray12 = new string[checked (dstItemCount - 1 + 1)];
      Array.Copy((Array) dstArray, (Array) strArray12, dstItemCount);
      int num21 = checked (dstItemCount - 1);
      int index16 = 0;
      while (index16 <= num21)
      {
        bool flag1 = false;
        bool flag2 = false;
        // if (Strings.InStr(1, dstArray[index16], "nav", CompareMethod.Text) != 0) num4 = num4; // Removed no-op
        int num22 = checked (index11 - 1);
        int index17 = 0;
        while (index17 <= num22)
        {
          num4 = modAsBuilt.ArraySsorted_FindItem(ref arySrc2[index17].etisFeatures, arySrc2[index17].etisFeatureCount, dstArray[index16], CompareMethod.Text);
          if (num4 != -1)
          {
            flag2 = true;
            break;
          }
          checked { ++index17; }
        }
        int num23 = checked (index10 - 1);
        int index18 = 0;
        while (index18 <= num23)
        {
          num4 = modAsBuilt.ArraySsorted_FindItem(ref arySrc1[index18].etisFeatures, arySrc1[index18].etisFeatureCount, dstArray[index16], CompareMethod.Text);
          if (num4 == -1)
          {
            // if (Strings.InStr(1, dstArray[index16], "nav", CompareMethod.Text) != 0) num4 = num4; // Removed no-op
            flag1 = true;
            break;
          }
          checked { ++index18; }
        }
        if (flag2)
          dstArray[index16] = "";
        if (flag1)
          dstArray[index16] = "";
        checked { ++index16; }
      }
      Array.Sort<string>(dstArray);
      int num24 = checked (dstItemCount - 1);
      int index19 = 0;
      while (index19 <= num24)
      {
        bool flag3 = false;
        bool flag4 = false;
        // if (Strings.InStr(1, strArray12[index19], "nav", CompareMethod.Text) != 0) num4 = num4; // Removed no-op
        int num25 = checked (index11 - 1);
        int index20 = 0;
        while (index20 <= num25)
        {
          num4 = modAsBuilt.ArraySsorted_FindItem(ref arySrc2[index20].etisFeatures, arySrc2[index20].etisFeatureCount, strArray12[index19], CompareMethod.Text);
          if (num4 == -1)
          {
            flag4 = true;
            break;
          }
          checked { ++index20; }
        }
        int num26 = checked (index10 - 1);
        int index21 = 0;
        while (index21 <= num26)
        {
          num4 = modAsBuilt.ArraySsorted_FindItem(ref arySrc1[index21].etisFeatures, arySrc1[index21].etisFeatureCount, strArray12[index19], CompareMethod.Text);
          if (num4 != -1)
          {
            // if (Strings.InStr(1, strArray12[index19], "nav", CompareMethod.Text) != 0) num4 = num4; // Removed no-op
            flag3 = true;
            break;
          }
          checked { ++index21; }
        }
        if (flag4)
          strArray12[index19] = "";
        if (flag3)
          strArray12[index19] = "";
        checked { ++index19; }
      }
      Array.Sort<string>(strArray12);
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.AppendLine("VINs with bit set:   " + Conversions.ToString(index10));
      int num27 = checked (index10 - 1);
      int index22 = 0;
      while (index22 <= num27)
      {
        stringBuilder.AppendLine("  " + arySrc1[index22].carVIN);
        checked { ++index22; }
      }
      stringBuilder.AppendLine();
      stringBuilder.AppendLine("VINs without bit set: " + Conversions.ToString(index11));
      int num28 = checked (index11 - 1);
      int index23 = 0;
      while (index23 <= num28)
      {
        stringBuilder.AppendLine("  " + arySrc2[index23].carVIN);
        checked { ++index23; }
      }
      stringBuilder.AppendLine();
      int num29 = 0;
      int num30 = checked (dstItemCount - 1);
      int index24 = 0;
      while (index24 <= num30)
      {
        if (Operators.CompareString(dstArray[index24], "", false) != 0)
          checked { ++num29; }
        checked { ++index24; }
      }
      stringBuilder.AppendLine($"Possible Features ({Conversions.ToString(num29)}) for {Right} bit {Conversions.ToString(num7)} on:");
      int num31 = checked (dstItemCount - 1);
      int index25 = 0;
      while (index25 <= num31)
      {
        if (Operators.CompareString(dstArray[index25], "", false) != 0)
          stringBuilder.AppendLine("  " + dstArray[index25]);
        checked { ++index25; }
      }
      int num32 = 0;
      int num33 = checked (dstItemCount - 1);
      int index26 = 0;
      while (index26 <= num33)
      {
        if (Operators.CompareString(strArray12[index26], "", false) != 0)
          checked { ++num32; }
        checked { ++index26; }
      }
      stringBuilder.AppendLine($"\r\nPossible Features ({Conversions.ToString(num32)}) for {Right} bit {Conversions.ToString(num7)} off:");
      int num34 = checked (dstItemCount - 1);
      int index27 = 0;
      while (index27 <= num34)
      {
        if (Operators.CompareString(strArray12[index27], "", false) != 0)
          stringBuilder.AppendLine("  " + strArray12[index27]);
        checked { ++index27; }
      }
      this.TextBox4.Focus();
      this.TextBox4.SelectAll();
      this.tbxDeduceReport2.Text = stringBuilder.ToString();
    }
  }

  private void lstDeduceFactoryOptions2_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  private void TabPage5_Click(object sender, EventArgs e)
  {
  }

  private void Button8_Click(object sender, EventArgs e)
  {
    string contents = "";
    int num1 = checked (this.lstDeduceFactoryOptions.Items.Count - 1);
    int index1 = 0;
    while (index1 <= num1)
    {
      this.lstDeduceFactoryOptions.SelectedIndices.Clear();
      this.lstDeduceFactoryOptions.SelectedIndices.Add(index1);
      contents = $"{contents}\r\nFeature:  {this.lstDeduceFactoryOptions.Items[index1].ToString()}\r\n";
      this.btnDeduceFigureIt.PerformClick();
      string text = this.tbxDeduceReport.Text;
      int Start = Strings.InStrRev(text, "Perfect Bits:", Compare: CompareMethod.Text);
      if (string.IsNullOrEmpty(text))
        Start = 0;
      if (Start > 0)
      {
        string Expression = Strings.Mid(text, Start);
        string[] strArray1 = new string[1];
        string[] strArray2 = Strings.Split(Expression, "\r\n");
        int num2 = checked (strArray2.Length - 1);
        int index2 = 1;
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

  private void Button5_Click_1(object sender, EventArgs e)
  {
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
    string[] strArray1 = new string[1];
    string[] files = Directory.GetFiles(path, "*.ETIS.HTML");
    string[] arySrc = new string[1];
    int index1 = 0;
    string[] strArray2 = new string[1];
    int index2 = 0;
    string[] strArray3 = new string[1];
    int index3 = 0;
    int num1 = checked (files.Length - 1);
    int index4 = 0;
    while (index4 <= num1)
    {
      string[] strArray4 = new string[1];
      int num2 = 0;
      string inpFileName1 = files[index4];
      ref string[] local1 = ref strArray4;
      ref int local2 = ref num2;
      string str = "";
      ref string local3 = ref str;
      modAsBuilt.ETIS_LoadFile_FactoryOptions_HTML(inpFileName1, ref local1, ref local2, ref local3);
      int num3 = checked (num2 - 1);
      int index5 = 0;
      while (index5 <= num3)
      {
        int num4 = -1;
        int num5 = checked (index1 - 1);
        int index6 = 0;
        while (index6 <= num5)
        {
          if (Operators.CompareString(arySrc[index6], strArray4[index5], false) == 0)
          {
            num4 = index6;
            break;
          }
          checked { ++index6; }
        }
        if (num4 == -1)
        {
          arySrc = (string[]) Utils.CopyArray((Array) arySrc, (Array) new string[checked (index1 + 1)]);
          arySrc[index1] = strArray4[index5];
          checked { ++index1; }
        }
        checked { ++index5; }
      }
      string inpFileName2 = Strings.Replace(files[index4], ".ETIS.", ".AB.");
      string[] retModuleAddresses = new string[1];
      string[] retModuleDatas = new string[1];
      int retModuleAddressCount = 0;
      string retVIN = "";
      string retCarModel = "";
      string retCarYear = "";
      string[] retModInfo_IDs = new string[1];
      string[] retModInfo_Names = new string[1];
      string[] retModInfo_Descs = new string[1];
      int retModInfo_Count = 0;
      modAsBuilt.AsBuilt_LoadFile_AB_HTML(inpFileName2, ref retModuleAddresses, ref retModuleDatas, ref retModuleAddressCount, ref retVIN, ref retCarModel, ref retCarYear, ref retModInfo_IDs, ref retModInfo_Names, ref retModInfo_Descs, ref retModInfo_Count);
      int num6 = -1;
      int num7 = checked (index2 - 1);
      int index7 = 0;
      while (index7 <= num7)
      {
        if (Operators.CompareString(strArray2[index7], retCarModel, false) == 0)
        {
          num6 = index7;
          break;
        }
        checked { ++index7; }
      }
      if (num6 == -1)
      {
        strArray2 = (string[]) Utils.CopyArray((Array) strArray2, (Array) new string[checked (index2 + 1)]);
        strArray2[index2] = retCarModel;
        checked { ++index2; }
      }
      int num8 = -1;
      int num9 = checked (index3 - 1);
      int index8 = 0;
      while (index8 <= num9)
      {
        if (Operators.CompareString(strArray3[index8], retCarYear, false) == 0)
        {
          num8 = index8;
          break;
        }
        checked { ++index8; }
      }
      if (num8 == -1)
      {
        strArray3 = (string[]) Utils.CopyArray((Array) strArray3, (Array) new string[checked (index3 + 1)]);
        strArray3[index3] = retCarYear;
        checked { ++index3; }
      }
      checked { ++index4; }
    }
    Array.Sort<string>(strArray2);
    this.lstBit_Models.Items.Clear();
    int num10 = checked (index2 - 1);
    int index9 = 0;
    while (index9 <= num10)
    {
      if (Operators.CompareString(Strings.Trim(strArray2[index9]), "", false) != 0)
        this.lstBit_Models.Items.Add((object) strArray2[index9]);
      checked { ++index9; }
    }
    Array.Sort<string>(strArray3);
    this.lstBit_Years.Items.Clear();
    int num11 = checked (index3 - 1);
    int index10 = 0;
    while (index10 <= num11)
    {
      if (Operators.CompareString(Strings.Trim(strArray3[index10]), "", false) != 0)
        this.lstBit_Years.Items.Add((object) strArray3[index10]);
      checked { ++index10; }
    }
  }

  private void Button6_Click_1(object sender, EventArgs e)
  {
    if (Information.IsNothing((object) this.lstBit_Models.SelectedItems) || this.lstBit_Models.SelectedItems.Count < 1)
    {
      int num1 = (int) Interaction.MsgBox((object) "Select at least one vehicle model.");
    }
    else if (Information.IsNothing((object) this.lstBit_Years.SelectedItems) || this.lstBit_Years.SelectedItems.Count < 1)
    {
      int num2 = (int) Interaction.MsgBox((object) "Select at least one vehicle year.");
    }
    else
    {
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
      string[] strArray1 = new string[1];
      string[] files = Directory.GetFiles(path, "*.ETIS.HTML");
      Form1.VehicleInfo[] vehicleInfoArray = new Form1.VehicleInfo[checked (files.Length + 1)];
      int index1 = 0;
      string retVIN = "";
      string str1 = "";
      string str2 = "";
      string String1_1 = ";";
      int num3 = checked (this.lstBit_Years.CheckedItems.Count - 1);
      int index2 = 0;
      while (index2 <= num3)
      {
        String1_1 = $"{String1_1}{this.lstBit_Years.CheckedItems[index2].ToString()};";
        checked { ++index2; }
      }
      string String1_2 = ";";
      int num4 = checked (this.lstBit_Models.CheckedItems.Count - 1);
      int index3 = 0;
      while (index3 <= num4)
      {
        String1_2 = $"{String1_2}{this.lstBit_Models.CheckedItems[index3].ToString()};";
        checked { ++index3; }
      }
      int num5 = checked (files.Length - 1);
      int index4 = 0;
      while (index4 <= num5)
      {
        string str3 = Strings.Replace(files[index4], ".ETIS.", ".AB.");
        string[] strArray2 = new string[1];
        string[] strArray3 = new string[1];
    
        string[] strArray4 = new string[1];
        string[] strArray5 = new string[1];
        string[] strArray6 = new string[1];
        int num6 = 0;
        int num7 = 0;
        string inpFileName1 = str3;
        ref string[] local1 = ref strArray2;
        ref string[] local2 = ref strArray3;
        ref int local3 = ref num6;
        string str4 = "";
        ref string local4 = ref str4;
        ref string local5 = ref str1;
        ref string local6 = ref str2;
        ref string[] local7 = ref strArray4;
        ref string[] local8 = ref strArray5;
        ref string[] local9 = ref strArray6;
        ref int local10 = ref num7;
        modAsBuilt.AsBuilt_LoadFile_AB_HTML(inpFileName1, ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, ref local7, ref local8, ref local9, ref local10);
        if (Strings.InStr(1, String1_2, $";{str1};") != 0 & Strings.InStr(1, String1_1, $";{str2};") != 0)
        {
          vehicleInfoArray[index1].carModel = str1;
          vehicleInfoArray[index1].carYear = str2;
          string str5 = files[index4];
          modAsBuilt.ETIS_LoadFile_FactoryOptions_HTML(str5, ref vehicleInfoArray[index1].etisFeatures, ref vehicleInfoArray[index1].etisFeatureCount, ref retVIN);
          string str6 = Strings.Replace(str5, ".ETIS.HTML", "") + ".AB";
          string[] strArray7 = new string[1];
          string[] strArray8 = new string[1];
          string[] strArray9 = new string[1];
          string[] strArray10 = new string[1];
          string[] strArray11 = new string[1];
          int num8 = 0;
          string inpFileName2 = str6;
          ref string[] local11 = ref vehicleInfoArray[index1].abModuleAddresses;
          ref string[] local12 = ref vehicleInfoArray[index1].abModuleDatasHex;
          ref int local13 = ref vehicleInfoArray[index1].abModuleAddrCount;
          str4 = "";
          ref string local14 = ref str4;
          ref string[] local15 = ref strArray7;
          ref string[] local16 = ref strArray9;
          ref string[] local17 = ref strArray10;
          ref string[] local18 = ref strArray11;
          ref int local19 = ref num8;
          ref string local20 = ref vehicleInfoArray[index1].carCCChex;
          modAsBuilt.AsBuilt_LoadFile_AB(inpFileName2, ref local11, ref local12, ref local13, ref local14, ref local15, ref local16, ref local17, ref local18, ref local19, ref local20);
          vehicleInfoArray[index1].carVIN = retVIN;
          vehicleInfoArray[index1].abModuleInfo_PartNums = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index1].abModuleInfo_Strategies = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index1].abModuleInfo_Calibrations = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
          int num9 = checked (num8 - 1);
          int index5 = 0;
          while (index5 <= num9)
          {
            int num10 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
            int index6 = 0;
            while (index6 <= num10)
            {
              if (Operators.CompareString(Strings.Left(vehicleInfoArray[index1].abModuleAddresses[index6], Strings.Len(strArray7[index5])), strArray7[index5], false) == 0)
              {
                vehicleInfoArray[index1].abModuleInfo_PartNums[index6] = strArray9[index5];
                vehicleInfoArray[index1].abModuleInfo_Strategies[index6] = strArray10[index5];
                vehicleInfoArray[index1].abModuleInfo_Calibrations[index6] = strArray11[index5];
              }
              checked { ++index6; }
            }
            checked { ++index5; }
          }
          int num11 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
          int index7 = 0;
          while (index7 <= num11)
          {
            vehicleInfoArray[index1].abModuleDatasHex[index7] = Strings.Left(vehicleInfoArray[index1].abModuleDatasHex[index7], checked (Strings.Len(vehicleInfoArray[index1].abModuleDatasHex[index7]) - 2));
            checked { ++index7; }
          }
          vehicleInfoArray[index1].carCCChex = Strings.Right(vehicleInfoArray[index1].carCCChex, 510);
          vehicleInfoArray[index1].carCCCbin = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index1].carCCChex);
          vehicleInfoArray[index1].abModuleDatasBinStr = new string[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
          vehicleInfoArray[index1].abModuleDatasLONG = new ulong[checked (vehicleInfoArray[index1].abModuleAddrCount - 1 + 1)];
          int num12 = checked (vehicleInfoArray[index1].abModuleAddrCount - 1);
          int index8 = 0;
          while (index8 <= num12)
          {
            vehicleInfoArray[index1].abModuleDatasBinStr[index8] = modAsBuilt.AsBuilt_HexStr2BinStr(vehicleInfoArray[index1].abModuleDatasHex[index8]);
            vehicleInfoArray[index1].abModuleDatasLONG[index8] = modAsBuilt.AsBuilt_HexStr2UINT64(vehicleInfoArray[index1].abModuleDatasHex[index8]);
            checked { ++index8; }
          }
          checked { ++index1; }
        }
        checked { ++index4; }
      }
      string[] strArray12 = new string[1];
      int index9 = 0;
      int num13 = checked (index1 - 1);
      int index10 = 0;
      while (index10 <= num13)
      {
        int num14 = checked (vehicleInfoArray[index10].abModuleAddrCount - 1);
        int index11 = 0;
        while (index11 <= num14)
        {
          int num15 = -1;
          int num16 = checked (index9 - 1);
          int index12 = 0;
          while (index12 <= num16)
          {
            if (Operators.CompareString(strArray12[index12], vehicleInfoArray[index10].abModuleAddresses[index11], false) == 0)
            {
              num15 = index12;
              break;
            }
            checked { ++index12; }
          }
          if (num15 == -1)
          {
            strArray12 = (string[]) Utils.CopyArray((Array) strArray12, (Array) new string[checked (index9 + 1)]);
            strArray12[index9] = vehicleInfoArray[index10].abModuleAddresses[index11];
            checked { ++index9; }
          }
          checked { ++index11; }
        }
        checked { ++index10; }
      }
      Array.Sort<string>(strArray12);
      this.lstBit_Modules.Items.Clear();
      int num17 = checked (index9 - 1);
      int index13 = 0;
      while (index13 <= num17)
      {
        this.lstBit_Modules.Items.Add((object) strArray12[index13]);
        checked { ++index13; }
      }
    }
  }

  private void Button7_Click(object sender, EventArgs e)
  {
    string str = "";
    string[] strArray1 = new string[1];
    int num1 = 0;
    string[] strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    string[] strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    ref string[] local1 = ref strArray3;
    ref string[] local2 = ref strArray2;
    Form form = (Form) this;
    ref Form local3 = ref form;
    ref string local4 = ref str;
    ref string[] local5 = ref strArray1;
    ref int local6 = ref num1;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    this.tbxCompFile3.Text = "";
    int num2 = checked (num1 - 1);
    int index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      this.tbxCompFile3.Text = $"{this.tbxCompFile3.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(this.tbxCompFile3.Text, 1), "|", false) == 0)
      this.tbxCompFile3.Text = Strings.Left(this.tbxCompFile3.Text, checked (Strings.Len(this.tbxCompFile3.Text) - 1));
    this.lblComp3VIN.Text = "";
  }

  private void Button9_Click(object sender, EventArgs e)
  {
    string str = "";
    string[] strArray1 = new string[1];
    int num1 = 0;
    string[] strArray2 = new string[1]
    {
      "*.ABT; *.AB; *.XML"
    };
    string[] strArray3 = new string[1]
    {
      "AB, ABT, and UCDS formats"
    };
    ref string[] local1 = ref strArray3;
    ref string[] local2 = ref strArray2;
    Form form = (Form) this;
    ref Form local3 = ref form;
    ref string local4 = ref str;
    ref string[] local5 = ref strArray1;
    ref int local6 = ref num1;
    modAsBuilt.CmDlgDLL_ShowOpenEx(ref local1, ref local2, ref local3, ref local4, ref local5, ref local6, true, "");
    if (Operators.CompareString(str, "", false) == 0)
      return;
    if (Operators.CompareString(Strings.Right(str, 1), "\\", false) != 0)
      str += "\\";
    this.tbxCompFile4.Text = "";
    int num2 = checked (num1 - 1);
    int index = 0;
    while (index <= num2)
    {
      strArray1[index] = str + strArray1[index];
      this.tbxCompFile4.Text = $"{this.tbxCompFile4.Text}{strArray1[index]}|";
      checked { ++index; }
    }
    if (Operators.CompareString(Strings.Right(this.tbxCompFile4.Text, 1), "|", false) == 0)
      this.tbxCompFile4.Text = Strings.Left(this.tbxCompFile4.Text, checked (Strings.Len(this.tbxCompFile4.Text) - 1));
    this.lblComp4VIN.Text = "";
  }

  private void EntireLineToolStripMenuItem_Click(object sender, EventArgs e)
  {
    string str1 = "Vehicle Num, ";
    int num1 = checked (this.ListView1.Columns.Count - 1);
    int index1 = 0;
    while (index1 <= num1)
    {
      str1 = $"{str1}{this.ListView1.Columns[index1].Text}, ";
      checked { ++index1; }
    }
    string str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    string text = str2 + "\r\n";
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index2 = 0;
    while (index2 <= num2)
    {
      if (this.ListView1.Items[index2].Selected)
      {
        if (this.ListView1.Items[index2].SubItems.Count == this.ListView1.Columns.Count)
        {
          string str3 = $"{text}{Conversions.ToString(this.ListView1.Items[index2].Tag)}, ";
          int num3 = checked (this.ListView1.Items[index2].SubItems.Count - 1);
          int index3 = 0;
          while (index3 <= num3)
          {
            str3 = $"{str3}{this.ListView1.Items[index2].SubItems[index3].Text}, ";
            checked { ++index3; }
          }
          string str4 = Strings.Trim(str3);
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
      int num4 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  private void Data1hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    int index1 = 1;
    string str1 = "Vehicle Num, ";
    int num1 = checked (this.ListView1.Columns.Count - 1);
    int index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{this.ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    string str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    string text = str2 + "\r\n";
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index3 = 0;
    while (index3 <= num2)
    {
      if (this.ListView1.Items[index3].Selected)
      {
        if (this.ListView1.Items[index3].SubItems.Count == this.ListView1.Columns.Count)
        {
          string str3 = Strings.Trim($"{$"{text}{Conversions.ToString(this.ListView1.Items[index3].Tag)}, "}{this.ListView1.Items[index3].SubItems[index1].Text}, ");
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
      int num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  private void Data2hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    int index1 = 2;
    string str1 = "Vehicle Num, ";
    int num1 = checked (this.ListView1.Columns.Count - 1);
    int index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{this.ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    string str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    string text = str2 + "\r\n";
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index3 = 0;
    while (index3 <= num2)
    {
      if (this.ListView1.Items[index3].Selected)
      {
        if (this.ListView1.Items[index3].SubItems.Count == this.ListView1.Columns.Count)
        {
          string str3 = Strings.Trim($"{$"{text}{Conversions.ToString(this.ListView1.Items[index3].Tag)}, "}{this.ListView1.Items[index3].SubItems[index1].Text}, ");
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
      int num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  private void Data3hexToolStripMenuItem_Click(object sender, EventArgs e)
  {
    int index1 = 3;
    string str1 = "Vehicle Num, ";
    int num1 = checked (this.ListView1.Columns.Count - 1);
    int index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{this.ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    string str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    string text = str2 + "\r\n";
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index3 = 0;
    while (index3 <= num2)
    {
      if (this.ListView1.Items[index3].Selected)
      {
        if (this.ListView1.Items[index3].SubItems.Count == this.ListView1.Columns.Count)
        {
          string str3 = Strings.Trim($"{$"{text}{Conversions.ToString(this.ListView1.Items[index3].Tag)}, "}{this.ListView1.Items[index3].SubItems[index1].Text}, ");
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
      int num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  private void BinaryToolStripMenuItem_Click(object sender, EventArgs e)
  {
    int index1 = 5;
    string str1 = "Vehicle Num, ";
    int num1 = checked (this.ListView1.Columns.Count - 1);
    int index2 = 0;
    while (index2 <= num1)
    {
      str1 = $"{str1}{this.ListView1.Columns[index2].Text}, ";
      checked { ++index2; }
    }
    string str2 = Strings.Trim(str1);
    if (Operators.CompareString(Strings.Right(str2, 1), ",", false) == 0)
      str2 = Strings.Trim(Strings.Left(str2, checked (Strings.Len(str2) - 1)));
    string text = str2 + "\r\n";
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index3 = 0;
    while (index3 <= num2)
    {
      if (this.ListView1.Items[index3].Selected)
      {
        if (this.ListView1.Items[index3].SubItems.Count == this.ListView1.Columns.Count)
        {
          string str3 = Strings.Trim($"{$"{text}{Conversions.ToString(this.ListView1.Items[index3].Tag)}, "}{this.ListView1.Items[index3].SubItems[index1].Text}, ");
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
      int num3 = (int) Interaction.MsgBox((object) "Failed to set clipboard text.");
      ProjectData.ClearProjectError();
    }
  }

  private void Button10_Click(object sender, EventArgs e)
  {
      // Legacy "Deducer" folder scraper - Disabled in favor of VehicleDatabase
  }

  private void Form1_MaximumSizeChanged(object sender, EventArgs e)
  {
  }

  private void Form1_Shown(object sender, EventArgs e)
  {
      PopulateVehicleList();
  }

  private void lvwBrowser_SelectedIndexChanged(object sender, EventArgs e)
  {
  }

  private void lvwBrowser_ColumnClick(object sender, ColumnClickEventArgs e)
  {
    ColumnHeader column = this.lvwBrowser.Columns[e.Column];
    SortOrder sort_order;
    if (this.lvwBrowser_SortingColumn == null)
    {
      sort_order = SortOrder.Ascending;
    }
    else
    {
      sort_order = !column.Equals((object) this.lvwBrowser_SortingColumn) ? SortOrder.Ascending : (!this.lvwBrowser_SortingColumn.Text.StartsWith("> ") ? SortOrder.Ascending : SortOrder.Descending);
      this.lvwBrowser_SortingColumn.Text = this.lvwBrowser_SortingColumn.Text.Substring(2);
    }
    this.lvwBrowser_SortingColumn = column;
    this.lvwBrowser_SortingColumn.Text = sort_order != SortOrder.Ascending ? "< " + this.lvwBrowser_SortingColumn.Text : "> " + this.lvwBrowser_SortingColumn.Text;
    this.lvwBrowser.ListViewItemSorter = (IComparer) new clsListviewSorter(e.Column, sort_order);
    this.lvwBrowser.Sort();
  }

  private void ContextMenuStrip2_Opening(object sender, CancelEventArgs e)
  {
    if (this.lvwBrowser.Items.Count >= 1)
      return;
    e.Cancel = true;
  }

  private void SetAsCompare1ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.lvwBrowser.SelectedItems.Count < 1)
      return;
    this.tbxCompFile1.Text = this.lvwBrowser.SelectedItems[0].Name;
    this.ListView1.Items.Clear();
  }

  private void SetAsCompare2ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.lvwBrowser.SelectedItems.Count < 1)
      return;
    this.tbxCompFile2.Text = this.lvwBrowser.SelectedItems[0].Name;
    this.ListView1.Items.Clear();
  }

  private void SetAsCompare3ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.lvwBrowser.SelectedItems.Count < 1)
      return;
    this.tbxCompFile3.Text = this.lvwBrowser.SelectedItems[0].Name;
    this.ListView1.Items.Clear();
  }

  private void SetAsCompare4ToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.lvwBrowser.SelectedItems.Count < 1)
      return;
    this.tbxCompFile4.Text = this.lvwBrowser.SelectedItems[0].Name;
    this.ListView1.Items.Clear();
  }

  private void DeleteFileToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.lvwBrowser.SelectedItems.Count < 1)
      return;
    ListViewItem selectedItem = this.lvwBrowser.SelectedItems[0];
    string name = selectedItem.Name;
    this.tbxCompFile4.Text = name;
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
        this.lvwBrowser.Items.Remove(selectedItem);
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

  private void PictureBox1_Click(object sender, EventArgs e)
  {
    Process.Start("https://www.paypal.com/donate/?business=X76ZW4RHA6T9C&no_recurring=0&item_name=I+build+and+maintain+open-source+projects+for+the+community.+Any+support+helps+me+keep+improving+and+maintaining+them.&currency_code=USD");
  }

  private void ToUCDSToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.ListView1.SelectedItems.Count < 1 || Information.IsNothing(RuntimeHelpers.GetObjectValue(this.ListView1.SelectedItems[0].Tag)) || Operators.CompareString(this.ListView1.SelectedItems[0].Text, "", false) == 0)
      return;
    ListViewItem selectedItem = this.ListView1.SelectedItems[0];
    string text = selectedItem.Text;
    string str1 = "";
    string str2 = "";
    string str3 = Strings.Left(text, 3);
    string[] retModuleNames = new string[1];
    string[] retModuleShortNames = new string[1];
    string[] retModuleAddresses = new string[1];
    int retModuleCount = 0;
    string directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    string inpFileName = directoryPath + "ModuleList.txt";
    if (this.chkCompareShowNames.Checked)
      modAsBuilt.AsBuilt_LoadFile_ModuleList(inpFileName, ref retModuleNames, ref retModuleShortNames, ref retModuleAddresses, ref retModuleCount);
    ref string[] local1 = ref retModuleNames;
    string[] modlistShortNames = retModuleShortNames;
    string[] modlistAddress = retModuleAddresses;
    int modlistCount = retModuleCount;
    string addrToFind = str3;
    int num1 = -1;
    ref int local2 = ref num1;
    ref string local3 = ref str1;
    ref string local4 = ref str2;
    modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local1, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local2, ref local3, ref local4);
    str2 = Interaction.InputBox("UCDS - Enter Module Type:", "UCDS - Enter Module Type", str2);
    if (Operators.CompareString(str2, "", false) == 0)
      return;
    string Left1 = Interaction.InputBox("UCDS - Enter Vehicle ID  (like 'U375'):", "UCDS - Enter Vehicle ID", "NONE");
    if (Operators.CompareString(Left1, "", false) == 0)
      return;
    string Left2 = Interaction.InputBox("UCDS - Enter Vehicle Year (like 'MY15'):", "UCDS - Enter Vehicle Year", "NONE");
    if (Operators.CompareString(Left2, "", false) == 0)
      return;
    Color foreColor = selectedItem.ForeColor;
    string[] strArray1 = new string[1000];
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index = 0;
    while (index <= num2)
    {
      if (this.ListView1.Items[index].ForeColor == foreColor && Operators.CompareString(Strings.Left(this.ListView1.Items[index].Text, 3), Strings.Left(str3, 3), false) == 0)
      {
        int num3 = Strings.InStr(this.ListView1.Items[index].Text, " ");
        if (num3 == 0)
          num3 = checked (Strings.Len(this.ListView1.Items[index].Text) + 1);
        string[] strArray2 = new string[1];
        int num4 = checked ((int) Math.Round(Conversion.Val(Strings.Split(Strings.Left(this.ListView1.Items[index].Text, num3 - 1), "-")[1])));
        string str4 = this.ListView1.Items[index].SubItems[1].Text + this.ListView1.Items[index].SubItems[2].Text + this.ListView1.Items[index].SubItems[3].Text;
        if (this.chkCompareShowChecksum.Checked)
          str4 = Strings.Left(str4, checked (Strings.Len(str4) - 2));
        strArray1[checked (num4 - 1)] = strArray1[checked (num4 - 1)] + str4;
      }
      checked { ++index; }
    }
    string str5 = $"{"" + "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n" + "<DirectConfiguration><!--UCDS Direct Configuration XML FILE from AsBuilt Explorer-->\r\n"}<VEHICLE MODULE=\"{str2}\" VIN=\"NONE\" VEHICLE_ID=\"{Left1}\" VEHICLE_YEAR=\"{Left2}\">\r\n";
    int Number = 0;
    do
    {
      if (Operators.CompareString(strArray1[Number], "", false) != 0)
        str5 = $"{str5}<DID ID=\"DE{Strings.Right("00" + Conversion.Hex(Number), 2)}\">{strArray1[Number]}</DID>\r\n";
      checked { ++Number; }
    }
    while (Number <= 999);
    string contents = str5 + "</VEHICLE>\r\n" + "</DirectConfiguration>";
    string str6 = modAsBuilt.CmDlgDLL_ShowSaveFile((Form) this, "All Files|*.*", "Export to UCDS...", $"Direct_{str2}_{Strings.Format((object) DateAndTime.Now.Day, "00")}{Strings.Format((object) DateAndTime.Now.Month, "00")}{Strings.Format((object) (DateAndTime.Now.Year % 100), "00")}_.XML");
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

  private void ToABTToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.ListView1.SelectedItems.Count < 1 || Information.IsNothing(RuntimeHelpers.GetObjectValue(this.ListView1.SelectedItems[0].Tag)) || Operators.CompareString(this.ListView1.SelectedItems[0].Text, "", false) == 0)
      return;
    ListViewItem selectedItem = this.ListView1.SelectedItems[0];
    string str1 = Strings.Left(selectedItem.Text, 3);
    string[] retModuleNames = new string[1];
    string[] retModuleShortNames = new string[1];
    string[] retModuleAddresses = new string[1];
    int retModuleCount = 0;
    string directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    string inpFileName = directoryPath + "ModuleList.txt";
    if (this.chkCompareShowNames.Checked)
      modAsBuilt.AsBuilt_LoadFile_ModuleList(inpFileName, ref retModuleNames, ref retModuleShortNames, ref retModuleAddresses, ref retModuleCount);
    string str2 = "";
    string Left = "";
    ref string[] local1 = ref retModuleNames;
    string[] modlistShortNames = retModuleShortNames;
    string[] modlistAddress = retModuleAddresses;
    int modlistCount = retModuleCount;
    string addrToFind = str1;
    int num1 = -1;
    ref int local2 = ref num1;
    ref string local3 = ref str2;
    ref string local4 = ref Left;
    modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local1, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local2, ref local3, ref local4);
    Color foreColor = selectedItem.ForeColor;
    string[] strArray1 = new string[1000];
    int num2 = checked (this.ListView1.Items.Count - 1);
    int index1 = 0;
    while (index1 <= num2)
    {
      if (this.ListView1.Items[index1].ForeColor == foreColor && Operators.CompareString(Strings.Left(this.ListView1.Items[index1].Text, 3), Strings.Left(str1, 3), false) == 0)
      {
        int num3 = Strings.InStr(this.ListView1.Items[index1].Text, " ");
        if (num3 == 0)
          num3 = checked (Strings.Len(this.ListView1.Items[index1].Text) + 1);
        string[] strArray2 = new string[1];
        int num4 = checked ((int) Math.Round(Conversion.Val(Strings.Split(Strings.Left(this.ListView1.Items[index1].Text, num3 - 1), "-")[1])));
        string str3 = this.ListView1.Items[index1].SubItems[1].Text + this.ListView1.Items[index1].SubItems[2].Text + this.ListView1.Items[index1].SubItems[3].Text;
        if (!this.chkCompareShowChecksum.Checked)
          str3 += modAsBuilt.AsBuilt_CalculateChecksum($"{str1}-{Strings.Mid(this.ListView1.Items[index1].Text, 5, 2)}-{Strings.Mid(this.ListView1.Items[index1].Text, 8, 2)}", str3 + "00");
        strArray1[checked (num4 - 1)] = strArray1[checked (num4 - 1)] + str3;
      }
      checked { ++index1; }
    }
    bool flag = false;
    int num5 = 1;
    int index2 = 0;
    do
    {
      if (Operators.CompareString(strArray1[index2], "", false) != 0)
      {
        int num6 = 1;
        int Start = 1;
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
    MsgBoxResult msgBoxResult = MsgBoxResult.Yes;
    if (flag)
    {
      int num7 = (int) Interaction.MsgBox((object) "The module data contains sections / blocks with an ID greater than '99', and therefore requires the ABT file to be written out in the 'new' 2019-08 ABT format.", MsgBoxStyle.Information);
    }
    else
      msgBoxResult = Interaction.MsgBox((object) "Would you like the ABT file written out in the 'new' 2019-08 ABT format (YES), or the 'old' format (NO)?", MsgBoxStyle.YesNo | MsgBoxStyle.Question);
    string contents = "";
    int num8 = 1;
    int index3 = 0;
    do
    {
      if (Operators.CompareString(strArray1[index3], "", false) != 0)
      {
        contents = $"{contents};Block {Conversions.ToString(num8)}\r\n";
        int num9 = 1;
        int Start = 1;
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
    string path = modAsBuilt.CmDlgDLL_ShowSaveFile((Form) this, "All Files|*.*", "Export to ABT...", Left + ".ABT");
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

  private void Button10_Click_1(object sender, EventArgs e)
  {
    StringBuilder stringBuilder = new StringBuilder();
    int num1 = checked (this.lstBit_Modules.Items.Count - 1);
    int num2 = 0;
    while (num2 <= num1)
    {
      this.lstBit_Modules.SelectedIndex = num2;
      int num3 = 0;
      do
      {
        this.TextBox4.Text = Conversions.ToString(num3);
        this.Button4.PerformClick();
        string text = this.tbxDeduceReport2.Text;
        if (Strings.InStr(1, text, "VINs without bit set: 0", CompareMethod.Text) == 0 && Strings.InStr(1, text, "VINs with bit set: 0", CompareMethod.Text) == 0)
        {
          int Start = Strings.InStr(1, text, "Possible Features (1)", CompareMethod.Text);
          if (Start > 0)
          {
            string str1 = Strings.Mid(text, Start);
            string str2 = $"{this.lstBit_Modules.SelectedItem.ToString()} bit # {Conversions.ToString(num3)}\r\n{str1}";
            stringBuilder.Append(str2 + "\r\n\r\n");
          }
        }
        MyProject.Application.DoEvents();
        checked { ++num3; }
      }
      while (num3 <= 39);
      checked { ++num2; }
    }
    this.tbxDeduceReport2.Text = stringBuilder.ToString();
  }

  private void IdentifyToolStripMenuItem_Click(object sender, EventArgs e)
  {
      if (this.ListView1.SelectedItems.Count == 0) return;
      
      ListViewItem lvi = this.ListView1.SelectedItems[0];
      string addr = lvi.Text; 
      
      if (lvi.SubItems.Count < 5) return;

      string d1 = lvi.SubItems[2].Text;
      string d2 = lvi.SubItems[3].Text;
      string d3 = lvi.SubItems[4].Text;

      CommonFeature f = CommonDatabase.FindMatch(addr, d1, d2, d3);
      if (f != null)
      {
          MessageBox.Show($"Found Feature:\nName: {f.Name}\nModule: {f.Module}\nNotes: {f.Notes}", "Feature Identified", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      else
      {
          MessageBox.Show("No specific feature match found in the common database for this address and data.", "No Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
  }

  private void chkCompareShowNames_CheckedChanged(object sender, EventArgs e)
  {
      Button2_Click(sender, e);
  }

  private void chkShowOnlyMismatches_CheckedChanged(object sender, EventArgs e)
  {
      Button2_Click(sender, e);
  }

  private void ExportModuleToolStripMenuItem_Click(object sender, EventArgs e)
  {
  }

  private void ExportModuleToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
  {
    if (this.chkShowOnlyMismatches.Checked)
    {
      this.ToUCDSToolStripMenuItem.Enabled = false;
      this.ToABTToolStripMenuItem.Enabled = false;
    }
    else
    {
      this.ToUCDSToolStripMenuItem.Enabled = true;
      this.ToABTToolStripMenuItem.Enabled = true;
    }
  }




  private void btnDB1_Click(object sender, EventArgs e)
  {
      ShowVehicleDB(this.tbxCompFile1, this.lblComp1VIN);
  }

  private void btnDB2_Click(object sender, EventArgs e)
  {
      ShowVehicleDB(this.tbxCompFile2, this.lblComp2VIN);
  }

  private void btnDB3_Click(object sender, EventArgs e)
  {
      ShowVehicleDB(this.tbxCompFile3, this.lblComp3VIN);
  }

  private void btnDB4_Click(object sender, EventArgs e)
  {
      ShowVehicleDB(this.tbxCompFile4, this.lblComp4VIN);
  }

  private void btnViewDefs_Click(object sender, EventArgs e)
  {
      using (frmDefinitionsDB frm = new frmDefinitionsDB())
      {
          frm.ShowDialog();
      }
  }

  private void ShowVehicleDB(TextBox tbxFile, Label lblVIN)
  {
      string currentPath = tbxFile.Text;
      string currentVIN = lblVIN.Text;
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


  private void lnkAboutGithub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
  {
      Process.Start("https://github.com/Eliminater74/AsBuiltExplorer");
  }

  private void btnDecode_Click(object sender, EventArgs e)
  {
      string vin = txtVinInput.Text.Trim();
      lvwDecodeResults.Items.Clear();

      var results = VINDecoder.Decode(vin);
      
      foreach (var r in results)
      {
          ListViewItem lvi = new ListViewItem(r.Position);
          lvi.SubItems.Add(r.Value);
          lvi.SubItems.Add(r.Meaning);
          lvi.SubItems.Add(r.Notes);
          lvwDecodeResults.Items.Add(lvi);
      }
  }

  private void lvwDecodeResults_MouseClick(object sender, MouseEventArgs e)
  {
      ListViewHitTestInfo hit = lvwDecodeResults.HitTest(e.Location);
      if (hit.Item != null)
      {
          if (hit.Item.Text == "URL")
          {
              string url = hit.Item.SubItems[3].Text; // Notes column has the URL
              if (url.StartsWith("http"))
              {
                  Process.Start(url);
              }
          }
      }
  }

  private void cmbSavedVehicles_SelectedIndexChanged(object sender, EventArgs e)
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

  private void TabPage9_Enter(object sender, EventArgs e)
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

  private void TabControl1_DrawItem(object sender, DrawItemEventArgs e)
  {
      Graphics g = e.Graphics;
      Brush _textBrush;

      // Get the item from the collection.
      TabPage _tabPage = TabControl1.TabPages[e.Index];

      // Get the real bounds for the tab rectangle.
      Rectangle _tabBounds = TabControl1.GetTabRect(e.Index);

      bool isDark = ("Dark" == My.MySettings.Default.AppTheme);
      Color backColor = isDark ? Color.FromArgb(45, 45, 48) : SystemColors.Control;
      Color foreColor = isDark ? Color.White : SystemColors.ControlText;
      Color selectedBack = isDark ? Color.FromArgb(60, 60, 60) : Color.White;
      Color selectedFore = isDark ? Color.Cyan : Color.Black; 

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
      Font _tabFont = new Font("Microsoft Sans Serif", 9.0f, FontStyle.Regular, GraphicsUnit.Point);

      // Draw string. Center the text.
      StringFormat _stringFlags = new StringFormat();
      _stringFlags.Alignment = StringAlignment.Center;
      _stringFlags.LineAlignment = StringAlignment.Center;
      
      g.DrawString(_tabPage.Text, _tabFont, _textBrush, _tabBounds, new StringFormat(_stringFlags));
  }



  private void tabMods_Enter(object sender, EventArgs e)
  {
      // Populate Platform Combo if empty
      if (cmbModPlatform.Items.Count == 0)
      {
          // Get unique platforms
          HashSet<string> platforms = new HashSet<string>();
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

  private void cmbModPlatform_SelectedIndexChanged(object sender, EventArgs e)
  {
      lvwMods.Items.Clear();
      rtbModDetails.Clear();
      string selectedPlatform = cmbModPlatform.SelectedItem?.ToString();

      if (string.IsNullOrEmpty(selectedPlatform)) return;

      foreach (var m in ModDatabase.Mods)
      {
          if (m.Platform == selectedPlatform)
          {
              ListViewItem lvi = new ListViewItem(m.Title);
              lvi.SubItems.Add(m.Category);
              lvi.Tag = m; // Store reference
              lvwMods.Items.Add(lvi);
          }
      }
  }

  private void lvwMods_SelectedIndexChanged(object sender, EventArgs e)
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
  private void numTPMS_PSI_ValueChanged(object sender, EventArgs e)
  {
      int psi = (int)numTPMS_PSI.Value;
      tbxTPMS_Hex.Text = psi.ToString("X2");
  }

  private void btnVIN_Convert_Click(object sender, EventArgs e)
  {
      string vin = txtVIN_Input.Text;
      if (string.IsNullOrWhiteSpace(vin)) return;

      StringBuilder sb = new StringBuilder();
      foreach (char c in vin)
      {
          sb.Append(((int)c).ToString("X2") + " ");
      }
      txtVIN_Hex.Text = sb.ToString().Trim();
  }





    private void EditFeaturesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (lvwBrowser.SelectedItems.Count == 0) return;
        
        // BATCH MODE
        if (lvwBrowser.SelectedItems.Count > 1)
        {
            string newTag = Interaction.InputBox($"Enter feature tag to ADD to {lvwBrowser.SelectedItems.Count} selected vehicles:", "Batch Add Feature");
            if (!string.IsNullOrWhiteSpace(newTag))
            {
                 int count = 0;
                 foreach(ListViewItem item in lvwBrowser.SelectedItems)
                 {
                      string vin = item.SubItems[4].Text;
                      var entry = VehicleDatabase.GetEntry(vin);
                      if (entry != null)
                      {
                           // Add unique
                           var parts = new List<string>((entry.Features ?? "").Split(';'));
                           bool exists = false;
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
        string vinSingle = lvwBrowser.SelectedItems[0].SubItems[4].Text; // VIN column
        var entrySingle = VehicleDatabase.GetEntry(vinSingle);
        
        if (entrySingle != null)
        {
            string currentFeatures = entrySingle.Features ?? "";
            string newFeatures = Interaction.InputBox("Edit features for this vehicle (semicolon separated):", "Edit Features", currentFeatures);
            
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

    private void DecodeNHTSAToolStripMenuItem_Click(object sender, EventArgs e)
    {
        if (lvwBrowser.SelectedItems.Count == 0) return;

        if (lvwBrowser.SelectedItems.Count > 1)
        {
             if (MessageBox.Show($"Are you sure you want to attempt online decoding for {lvwBrowser.SelectedItems.Count} vehicles? This may take a moment.", "Confirm Batch Decode", MessageBoxButtons.YesNo) == DialogResult.No) return;
        }

        int successCount = 0;
        Cursor = Cursors.WaitCursor;
        
        try
        {
            foreach (ListViewItem item in lvwBrowser.SelectedItems)
            {
                string vin = item.SubItems[4].Text;
                var entry = VehicleDatabase.GetEntry(vin);
                if (entry != null)
                {
                    var result = NHTSADecoder.Decode(vin);
                    if (result != null)
                    {
                        // Update Basic Info
                        if(!string.IsNullOrEmpty(result.Make)) entry.Make = result.Make;
                        if(!string.IsNullOrEmpty(result.Year)) entry.Year = result.Year;
                        
                        string model = result.Model;
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
                        bool changed = false;
                        foreach (var tag in newTags)
                        {
                             bool exists = false;
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

    private void btnDB_Scan_Click(object sender, EventArgs e)
    {
        using (var fbd = new FolderBrowserDialog())
        {
            fbd.Description = "Select folder containing As-Built files (.ab, .abt, .xml) to import. ETIS files in the same folder will be processed for features.";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                int count = VehicleDatabase.BulkImport(fbd.SelectedPath);
                MessageBox.Show($"Imported {count} new vehicles into the database.", "Import Complete");
                Button10_Click(sender, e); // Refresh List
            }
        }
    }
}
}


