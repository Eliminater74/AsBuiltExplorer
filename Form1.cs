// Decompiled with JetBrains decompiler
// Type: AsBuiltExplorer.Form1
// Assembly: AsBuiltExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9083D66F-6E27-42C7-99A4-392C98AEFBC8
// Assembly location: I:\GITHUB\Projects\AsBuiltExplorer\AsBuiltExplorer.exe

using AsBuiltExplorer.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

#nullable disable
namespace AsBuiltExplorer;

[DesignerGenerated]
public class Form1 : Form
{
  private ColumnHeader lvwBrowser_SortingColumn;
  public int abDownloadTriggered;
  private IContainer components;

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
    ref string modulePartNum = "")
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
  }

  private void Button2_Click(object sender, EventArgs e)
  {
    string Left1 = "";
    string str1 = "";
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
    string text1 = this.tbxCompFile1.Text;
    bool flag1 = false;
    string[] strArray5 = new string[1];
    string retVIN1 = "";
    string fileType1 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text1);
    string[] inpFileArray1 = Strings.Split(this.tbxCompFile1.Text, "|");
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
    string text2 = this.tbxCompFile2.Text;
    bool flag2 = false;
    string[] strArray6 = new string[1];
    string retVIN2 = "";
    string fileType2 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text2);
    string[] inpFileArray2 = Strings.Split(this.tbxCompFile2.Text, "|");
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
    string text3 = this.tbxCompFile3.Text;
    bool flag3 = false;
    string[] strArray7 = new string[1];
    string retVIN3 = "";
    string fileType3 = modAsBuilt.AsBuilt_LoadFile_GetFileType(text3);
    string[] inpFileArray3 = Strings.Split(this.tbxCompFile3.Text, "|");
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
    string text4 = this.tbxCompFile4.Text;
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
    string str2 = "";
    string str3 = modAsBuilt.AsBuilt_Ascii2Hex(retVIN1);
    string Right1 = "";
    int num5 = checked (num1 - 1);
    int index1 = 0;
    int num6;
    while (index1 <= num5)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas1[index1] = Strings.Left(retModuleDatas1[index1], checked (Strings.Len(retModuleDatas1[index1]) - 2));
      string str4 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray1[index1]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas1[index1], ref retData1_1, ref retData2_1, ref retData3_1);
      str2 = "";
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
              Left2 = Left2;
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
      if (this.chkCompareShowNames.Checked)
      {
        ref string[] local1 = ref retModuleNames;
        string[] modlistShortNames = retModuleShortNames;
        string[] modlistAddress = retModuleAddresses;
        int modlistCount = retModuleCount;
        string addrToFind = str4;
        num6 = 0;
        ref int local2 = ref num6;
        ref string local3 = ref Left1;
        ref string local4 = ref str1;
        modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local1, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local2, ref local3, ref local4);
        if (Operators.CompareString(Left1, "", false) != 0)
          str4 = $"{str4}  {str1}  {Left1}";
      }
      ListViewItem listViewItem2 = this.ListView1.Items.Add(str4);
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
    bool flag5 = false;
    int num12 = checked (num2 - 1);
    int index4 = 0;
    while (index4 <= num12)
    {
      if (!this.chkCompareShowChecksum.Checked)
        retModuleDatas2[index4] = Strings.Left(retModuleDatas2[index4], checked (Strings.Len(retModuleDatas2[index4]) - 2));
      string str8 = modAsBuilt.AsBuilt_FormatReadable_ModuleAddress(strArray2[index4]);
      modAsBuilt.AsBuilt_FormatReadable_ModuleData(retModuleDatas2[index4], ref retData1_1, ref retData2_1, ref retData3_1);
      if (this.chkCompareShowNames.Checked)
      {
        ref string[] local5 = ref retModuleNames;
        string[] modlistShortNames = retModuleShortNames;
        string[] modlistAddress = retModuleAddresses;
        int modlistCount = retModuleCount;
        string addrToFind = str8;
        num6 = 0;
        ref int local6 = ref num6;
        ref string local7 = ref Left1;
        ref string local8 = ref str1;
        modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local5, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local6, ref local7, ref local8);
        if (Operators.CompareString(Left1, "", false) != 0)
          str8 = $"{str8}  {str1}  {Left1}";
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
      flag5 = true;
      ListViewItem listViewItem3 = new ListViewItem(str8);
      if (index5 != -1)
        this.ListView1.Items.Insert(checked (index5 + 1), listViewItem3);
      else
        listViewItem3 = this.ListView1.Items.Add(str8);
      listViewItem3.ForeColor = this.tbxCompFile2.ForeColor;
      listViewItem3.UseItemStyleForSubItems = false;
      listViewItem3.Tag = (object) text2;
      str2 = "";
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
      if (this.chkCompareShowNames.Checked)
      {
        ref string[] local9 = ref retModuleNames;
        string[] modlistShortNames = retModuleShortNames;
        string[] modlistAddress = retModuleAddresses;
        int modlistCount = retModuleCount;
        string addrToFind = str9;
        num6 = 0;
        ref int local10 = ref num6;
        ref string local11 = ref Left1;
        ref string local12 = ref str1;
        modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local9, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local10, ref local11, ref local12);
        if (Operators.CompareString(Left1, "", false) != 0)
          str9 = $"{str9}  {str1}  {Left1}";
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
      flag5 = true;
      ListViewItem listViewItem4 = new ListViewItem(str9);
      if (index10 != -1)
        this.ListView1.Items.Insert(checked (index10 + 1), listViewItem4);
      else
        listViewItem4 = this.ListView1.Items.Add(str9);
      listViewItem4.ForeColor = this.tbxCompFile3.ForeColor;
      listViewItem4.UseItemStyleForSubItems = false;
      listViewItem4.Tag = (object) text3;
      str2 = "";
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
      if (this.chkCompareShowNames.Checked)
      {
        ref string[] local13 = ref retModuleNames;
        string[] modlistShortNames = retModuleShortNames;
        string[] modlistAddress = retModuleAddresses;
        int modlistCount = retModuleCount;
        string addrToFind = str10;
        num6 = 0;
        ref int local14 = ref num6;
        ref string local15 = ref Left1;
        ref string local16 = ref str1;
        modAsBuilt.AsBuilt_ModuleList_FindAddressInfo(ref local13, modlistShortNames, modlistAddress, modlistCount, addrToFind, ref local14, ref local15, ref local16);
        if (Operators.CompareString(Left1, "", false) != 0)
          str10 = $"{str10}  {str1}  {Left1}";
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
      flag5 = true;
      ListViewItem listViewItem5 = new ListViewItem(str10);
      if (index15 != -1)
        this.ListView1.Items.Insert(checked (index15 + 1), listViewItem5);
      else
        listViewItem5 = this.ListView1.Items.Add(str10);
      listViewItem5.ForeColor = this.tbxCompFile4.ForeColor;
      listViewItem5.UseItemStyleForSubItems = false;
      listViewItem5.Tag = (object) text4;
      str2 = "";
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
      string text21 = this.ListView1.Items[index19].SubItems[5].Text;
      bool flag9 = true;
      int num30 = checked (index19 + 1);
      int num31 = num24;
      int index22 = num30;
      while (index22 <= num31)
      {
        if (Operators.CompareString(this.ListView1.Items[index22].SubItems[5].Text, text21, false) != 0)
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
          this.ListView1.Items[index23].SubItems[4].Text = "No";
          this.ListView1.Items[index23].BackColor = color;
          int num34 = checked (this.ListView1.Items[index23].SubItems.Count - 1);
          int index24 = 0;
          while (index24 <= num34)
          {
            this.ListView1.Items[index23].SubItems[index24].BackColor = color;
            checked { ++index24; }
          }
        }
        else
        {
          this.ListView1.Items[index23].SubItems[4].Text = "Yes";
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

  private void Button2_Click_1(object sender, EventArgs e)
  {
    WebBrowser wbDeducer = this.wbDeducer;
    wbDeducer.ScriptErrorsSuppressed = true;
    wbDeducer.Navigate("http://www.etis.ford.com/vehicleRegSelector.do");
    do
    {
      MyProject.Application.DoEvents();
    }
    while (wbDeducer.IsBusy);
    wbDeducer.Navigate("http://www.etis.ford.com/vehicleRegSelector.do");
    do
    {
      MyProject.Application.DoEvents();
    }
    while (wbDeducer.IsBusy);
  }

  private void Button3_Click(object sender, EventArgs e)
  {
    string directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    string path1 = directoryPath + "Deducer";
    try
    {
      Directory.CreateDirectory(path1);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
    string documentText = this.wbDeducer.DocumentText;
    string str = "";
    int num1 = Strings.InStr(1, documentText, "VIN=");
    if (num1 > 0)
      str = Strings.Replace(Strings.Mid(documentText, checked (num1 + 4), 17), "\"", "");
    string path2 = $"{path1}\\{str}.ETIS.HTML";
    try
    {
      if (System.IO.File.Exists(path2))
      {
        if (Interaction.MsgBox((object) $"VIN {str} ETIS data already exists.  Continue?", MsgBoxStyle.YesNo) == MsgBoxResult.No)
        {
          this.btnDeduceOpenETIS.PerformClick();
          return;
        }
        System.IO.File.Delete(path2);
      }
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      ProjectData.ClearProjectError();
    }
    System.IO.File.WriteAllText(path2, this.wbDeducer.DocumentText);
    this.wbDeducer.Navigate("https://www.motorcraftservice.com/AsBuilt");
    int num2 = (int) Interaction.MsgBox((object) "ETIS Data saved");
    do
    {
      MyProject.Application.DoEvents();
    }
    while (this.wbDeducer.IsBusy);
    HtmlElement Expression = modAsBuilt.DOM_WaitForElement_ByID(this.wbDeducer, "VIN", 20.0);
    if (!Information.IsNothing((object) Expression))
    {
      Expression.SetAttribute("value", str);
      if (!Information.IsNothing((object) modAsBuilt.DOM_WaitForElement_ByTag(this.wbDeducer, "type", "submit", 10.0)))
      {
        Expression.Parent.Parent.InvokeMember("submit");
        do
        {
          MyProject.Application.DoEvents();
        }
        while (this.wbDeducer.IsBusy);
        string path3 = $"{path1}\\{str}.AB.HTML";
        try
        {
          if (System.IO.File.Exists(path3))
            System.IO.File.Delete(path3);
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
        }
        System.IO.File.WriteAllText(path3, this.wbDeducer.DocumentText);
        int num3 = (int) Interaction.MsgBox((object) "As-Built HTML saved.");
        if (this.chkDeduceDownloadAB.Checked)
        {
          long tickCount = modAsBuilt.System_GetTickCount();
          Thread.Sleep(300);
          MyProject.Application.DoEvents();
          HtmlElement htmlElement = modAsBuilt.DOM_WaitForElement_ByTag(this.wbDeducer, "type", "submit", 10.0);
          htmlElement.InvokeMember("click");
          MyProject.Application.DoEvents();
          htmlElement.InvokeMember("submit");
          MyProject.Application.DoEvents();
          do
          {
            MyProject.Application.DoEvents();
            Thread.Sleep(100);
          }
          while (checked (modAsBuilt.System_GetTickCount() - tickCount) <= 8000L);
        }
        MyProject.Application.DoEvents();
        this.btnDeduceOpenETIS.PerformClick();
      }
    }
  }

  private void wbDeducer_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
  {
  }

  private void wbDeducer_Navigating(object sender, WebBrowserNavigatingEventArgs e)
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
    string fileName = path + "\\New.ab";
    if (Operators.CompareString(Strings.UCase(Strings.Right(e.Url.ToString(), 3)), ".AB", false) != 0)
      return;
    e.Cancel = true;
    new WebClient()
    {
      Headers = {
        {
          HttpRequestHeader.Cookie,
          this.wbDeducer.Document.Cookie
        }
      }
    }.DownloadFile(e.Url, fileName);
  }

  private void wbDeducer_FileDownload(object sender, EventArgs e) => ++this.abDownloadTriggered;

  private void btnDeduceLoadOptions_Click(object sender, EventArgs e)
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
    string[] strArray2 = new string[1];
    int index1 = 0;
    string[] strArray3 = new string[1];
    int index2 = 0;
    string[] strArray4 = new string[1];
    int index3 = 0;
    int num1 = checked (files.Length - 1);
    int index4 = 0;
    while (index4 <= num1)
    {
      string[] strArray5 = new string[1];
      int num2 = 0;
      string inpFileName1 = files[index4];
      ref string[] local1 = ref strArray5;
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
          if (Operators.CompareString(strArray2[index6], strArray5[index5], false) == 0)
          {
            num4 = index6;
            break;
          }
          checked { ++index6; }
        }
        if (num4 == -1)
        {
          strArray2 = (string[]) Utils.CopyArray((Array) strArray2, (Array) new string[checked (index1 + 1)]);
          strArray2[index1] = strArray5[index5];
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
        if (Operators.CompareString(strArray3[index7], retCarModel, false) == 0)
        {
          num6 = index7;
          break;
        }
        checked { ++index7; }
      }
      if (num6 == -1)
      {
        strArray3 = (string[]) Utils.CopyArray((Array) strArray3, (Array) new string[checked (index2 + 1)]);
        strArray3[index2] = retCarModel;
        checked { ++index2; }
      }
      int num8 = -1;
      int num9 = checked (index3 - 1);
      int index8 = 0;
      while (index8 <= num9)
      {
        if (Operators.CompareString(strArray4[index8], retCarYear, false) == 0)
        {
          num8 = index8;
          break;
        }
        checked { ++index8; }
      }
      if (num8 == -1)
      {
        strArray4 = (string[]) Utils.CopyArray((Array) strArray4, (Array) new string[checked (index3 + 1)]);
        strArray4[index3] = retCarYear;
        checked { ++index3; }
      }
      checked { ++index4; }
    }
    Array.Sort<string>(strArray2);
    this.lstDeduceFactoryOptions.Items.Clear();
    int num10 = checked (index1 - 1);
    int index9 = 0;
    while (index9 <= num10)
    {
      this.lstDeduceFactoryOptions.Items.Add((object) strArray2[index9]);
      checked { ++index9; }
    }
    Array.Sort<string>(strArray3);
    this.lstDeduceModels.Items.Clear();
    int num11 = checked (index2 - 1);
    int index10 = 0;
    while (index10 <= num11)
    {
      this.lstDeduceModels.Items.Add((object) strArray3[index10]);
      checked { ++index10; }
    }
    Array.Sort<string>(strArray4);
    this.lstDeduceYears.Items.Clear();
    int num12 = checked (index3 - 1);
    int index11 = 0;
    while (index11 <= num12)
    {
      this.lstDeduceYears.Items.Add((object) strArray4[index11]);
      checked { ++index11; }
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
  }

  private void tbxChecksumHex_TextChanged(object sender, EventArgs e)
  {
    this.tbxChecksumBin.Text = Strings.Mid(modAsBuilt.AsBuilt_HexStr2BinStr(this.tbxChecksumHex.Text), 1, 8);
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
        if (Strings.InStr(1, dstArray[index16], "nav", CompareMethod.Text) != 0)
          num4 = num4;
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
            if (Strings.InStr(1, dstArray[index16], "nav", CompareMethod.Text) != 0)
              num4 = num4;
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
        if (Strings.InStr(1, strArray12[index19], "nav", CompareMethod.Text) != 0)
          num4 = num4;
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
            if (Strings.InStr(1, strArray12[index19], "nav", CompareMethod.Text) != 0)
              num4 = num4;
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
        int num6 = 0;
        string[] strArray4 = new string[1];
        string[] strArray5 = new string[1];
        string[] strArray6 = new string[1];
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
    Array.Sort<string>(files);
    this.lvwBrowser.BeginUpdate();
    this.lvwBrowser.Items.Clear();
    int num = checked (files.Length - 1);
    int index = 0;
    while (index <= num)
    {
      FileInfo fileInfo = new FileInfo(files[index]);
      string inpFileName = Strings.Replace(files[index], ".ETIS.", ".AB.");
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
      modAsBuilt.AsBuilt_LoadFile_AB_HTML(inpFileName, ref retModuleAddresses, ref retModuleDatas, ref retModuleAddressCount, ref retVIN, ref retCarModel, ref retCarYear, ref retModInfo_IDs, ref retModInfo_Names, ref retModInfo_Descs, ref retModInfo_Count);
      ListViewItem listViewItem = this.lvwBrowser.Items.Add(Path.GetFileName(files[index]));
      ListViewItem.ListViewSubItemCollection subItems = listViewItem.SubItems;
      string[] strArray2 = new string[7];
      DateTime lastWriteTime = fileInfo.LastWriteTime;
      strArray2[0] = Conversions.ToString(lastWriteTime.Year);
      strArray2[1] = "-";
      lastWriteTime = fileInfo.LastWriteTime;
      strArray2[2] = Strings.Format((object) lastWriteTime.Month, "00");
      strArray2[3] = "-";
      lastWriteTime = fileInfo.LastWriteTime;
      strArray2[4] = Strings.Format((object) lastWriteTime.Day, "00");
      strArray2[5] = "   ";
      lastWriteTime = fileInfo.LastWriteTime;
      strArray2[6] = lastWriteTime.ToShortTimeString();
      string text = string.Concat(strArray2);
      subItems.Add(text);
      listViewItem.SubItems.Add(retCarYear);
      listViewItem.SubItems.Add(retCarModel);
      listViewItem.SubItems.Add(retVIN);
      listViewItem.Name = Strings.Replace(files[index], ".ETIS.HTML", ".AB", Compare: CompareMethod.Text);
      checked { ++index; }
    }
    this.lvwBrowser.EndUpdate();
  }

  private void Form1_MaximumSizeChanged(object sender, EventArgs e)
  {
  }

  private void Form1_Shown(object sender, EventArgs e)
  {
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
    Process.Start("https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=jesse_programmer%40yahoo%2ecom&lc=US&item_name=AsBuiltExplorer&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted");
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

  private void CheckBox1_CheckedChanged(object sender, EventArgs e)
  {
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

  [DebuggerNonUserCode]
  protected override void Dispose(bool disposing)
  {
    try
    {
      if (!disposing || this.components == null)
        return;
      this.components.Dispose();
    }
    finally
    {
      base.Dispose(disposing);
    }
  }

  [DebuggerStepThrough]
  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (Form1));
    this.TabControl1 = new TabControl();
    this.TabPage1 = new TabPage();
    this.chkCompareShowNames = new CheckBox();
    this.lblComp4VIN = new Label();
    this.lblComp3VIN = new Label();
    this.lblComp2VIN = new Label();
    this.lblComp1VIN = new Label();
    this.Button9 = new Button();
    this.tbxCompFile4 = new TextBox();
    this.Label23 = new Label();
    this.Button7 = new Button();
    this.tbxCompFile3 = new TextBox();
    this.Label22 = new Label();
    this.chkCompareShowChecksum = new CheckBox();
    this.btnCompLoad = new Button();
    this.btnCompBrowse2 = new Button();
    this.btnCompBrowse1 = new Button();
    this.tbxCompFile2 = new TextBox();
    this.tbxCompFile1 = new TextBox();
    this.ListView1 = new ListView();
    this.ColumnHeader1 = new ColumnHeader();
    this.ColumnHeader2 = new ColumnHeader();
    this.ColumnHeader3 = new ColumnHeader();
    this.ColumnHeader4 = new ColumnHeader();
    this.ColumnHeader5 = new ColumnHeader();
    this.ColumnHeader6 = new ColumnHeader();
    this.ColumnHeader7 = new ColumnHeader();
    this.ColumnHeader8 = new ColumnHeader();
    this.ColumnHeader9 = new ColumnHeader();
    this.ContextMenuStrip1 = new ContextMenuStrip(this.components);
    this.CopyToolStripMenuItem = new ToolStripMenuItem();
    this.EntireLineToolStripMenuItem = new ToolStripMenuItem();
    this.Data1hexToolStripMenuItem = new ToolStripMenuItem();
    this.Data2hexToolStripMenuItem = new ToolStripMenuItem();
    this.Data3hexToolStripMenuItem = new ToolStripMenuItem();
    this.BinaryToolStripMenuItem = new ToolStripMenuItem();
    this.ExportModuleToolStripMenuItem = new ToolStripMenuItem();
    this.ToUCDSToolStripMenuItem = new ToolStripMenuItem();
    this.ToABTToolStripMenuItem = new ToolStripMenuItem();
    this.Label7 = new Label();
    this.Label6 = new Label();
    this.TabPage2 = new TabPage();
    this.TextBox5 = new TextBox();
    this.Button3 = new Button();
    this.Button2 = new Button();
    this.Label16 = new Label();
    this.Label15 = new Label();
    this.tbxChecksumBin = new TextBox();
    this.tbxConvertBin = new TextBox();
    this.tbxConvertHex = new TextBox();
    this.tbxData3bin2 = new TextBox();
    this.tbxData2bin2 = new TextBox();
    this.tbxData1bin2 = new TextBox();
    this.tbxData3bin1 = new TextBox();
    this.tbxData2bin1 = new TextBox();
    this.tbxData1bin1 = new TextBox();
    this.tbxChecksumHex = new TextBox();
    this.Label5 = new Label();
    this.Button1 = new Button();
    this.tbxData3hex = new TextBox();
    this.Label4 = new Label();
    this.tbxData2hex = new TextBox();
    this.Label3 = new Label();
    this.tbxData1hex = new TextBox();
    this.Label2 = new Label();
    this.tbxModIDhex = new TextBox();
    this.Label1 = new Label();
    this.TabPage3 = new TabPage();
    this.TextBox2 = new TextBox();
    this.Label12 = new Label();
    this.Label9 = new Label();
    this.chkDeduceDownloadAB = new CheckBox();
    this.btnDeduceSaveInfo = new Button();
    this.wbDeducer = new WebBrowser();
    this.btnDeduceOpenETIS = new Button();
    this.TabPage4 = new TabPage();
    this.chkDeduceDoCCC = new CheckBox();
    this.Button8 = new Button();
    this.Label14 = new Label();
    this.TextBox1 = new TextBox();
    this.Label13 = new Label();
    this.Label11 = new Label();
    this.Label10 = new Label();
    this.lstDeduceYears = new CheckedListBox();
    this.lstDeduceModels = new CheckedListBox();
    this.tbxDeduceReport = new TextBox();
    this.btnDeduceFigureIt = new Button();
    this.btnDeduceLoadOptions = new Button();
    this.lstDeduceFactoryOptions = new ListBox();
    this.TabPage5 = new TabPage();
    this.Button10 = new Button();
    this.TextBox4 = new TextBox();
    this.Label21 = new Label();
    this.lstBit_Modules = new ListBox();
    this.Button6 = new Button();
    this.Button5 = new Button();
    this.Label20 = new Label();
    this.Label19 = new Label();
    this.lstBit_Years = new CheckedListBox();
    this.lstBit_Models = new CheckedListBox();
    this.Label17 = new Label();
    this.TextBox3 = new TextBox();
    this.Label18 = new Label();
    this.tbxDeduceReport2 = new TextBox();
    this.Button4 = new Button();
    this.TabPage6 = new TabPage();
    this.btnBrowseRefresh = new Button();
    this.lvwBrowser = new ListView();
    this.ColumnHeader10 = new ColumnHeader();
    this.ColumnHeader11 = new ColumnHeader();
    this.ColumnHeader12 = new ColumnHeader();
    this.ColumnHeader13 = new ColumnHeader();
    this.ColumnHeader14 = new ColumnHeader();
    this.ContextMenuStrip2 = new ContextMenuStrip(this.components);
    this.SetAsCompare1ToolStripMenuItem = new ToolStripMenuItem();
    this.SetAsCompare2ToolStripMenuItem = new ToolStripMenuItem();
    this.SetAsCompare3ToolStripMenuItem = new ToolStripMenuItem();
    this.SetAsCompare4ToolStripMenuItem = new ToolStripMenuItem();
    this.ToolStripMenuItem1 = new ToolStripSeparator();
    this.DeleteFileToolStripMenuItem = new ToolStripMenuItem();
    this.Label8 = new Label();
    this.PictureBox1 = new PictureBox();
    this.chkShowOnlyMismatches = new CheckBox();
    this.TextBox6 = new TextBox();
    this.TabControl1.SuspendLayout();
    this.TabPage1.SuspendLayout();
    this.ContextMenuStrip1.SuspendLayout();
    this.TabPage2.SuspendLayout();
    this.TabPage3.SuspendLayout();
    this.TabPage4.SuspendLayout();
    this.TabPage5.SuspendLayout();
    this.TabPage6.SuspendLayout();
    this.ContextMenuStrip2.SuspendLayout();
    ((ISupportInitialize) this.PictureBox1).BeginInit();
    this.SuspendLayout();
    this.TabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.TabControl1.Controls.Add((Control) this.TabPage1);
    this.TabControl1.Controls.Add((Control) this.TabPage2);
    this.TabControl1.Controls.Add((Control) this.TabPage3);
    this.TabControl1.Controls.Add((Control) this.TabPage4);
    this.TabControl1.Controls.Add((Control) this.TabPage5);
    this.TabControl1.Controls.Add((Control) this.TabPage6);
    this.TabControl1.Location = new Point(16 /*0x10*/, 15);
    this.TabControl1.Margin = new Padding(4);
    this.TabControl1.Name = "TabControl1";
    this.TabControl1.SelectedIndex = 0;
    this.TabControl1.Size = new Size(999, 591);
    this.TabControl1.TabIndex = 0;
    this.TabPage1.BackColor = SystemColors.Control;
    this.TabPage1.Controls.Add((Control) this.chkShowOnlyMismatches);
    this.TabPage1.Controls.Add((Control) this.chkCompareShowNames);
    this.TabPage1.Controls.Add((Control) this.lblComp4VIN);
    this.TabPage1.Controls.Add((Control) this.lblComp3VIN);
    this.TabPage1.Controls.Add((Control) this.lblComp2VIN);
    this.TabPage1.Controls.Add((Control) this.lblComp1VIN);
    this.TabPage1.Controls.Add((Control) this.Button9);
    this.TabPage1.Controls.Add((Control) this.tbxCompFile4);
    this.TabPage1.Controls.Add((Control) this.Label23);
    this.TabPage1.Controls.Add((Control) this.Button7);
    this.TabPage1.Controls.Add((Control) this.tbxCompFile3);
    this.TabPage1.Controls.Add((Control) this.Label22);
    this.TabPage1.Controls.Add((Control) this.chkCompareShowChecksum);
    this.TabPage1.Controls.Add((Control) this.btnCompLoad);
    this.TabPage1.Controls.Add((Control) this.btnCompBrowse2);
    this.TabPage1.Controls.Add((Control) this.btnCompBrowse1);
    this.TabPage1.Controls.Add((Control) this.tbxCompFile2);
    this.TabPage1.Controls.Add((Control) this.tbxCompFile1);
    this.TabPage1.Controls.Add((Control) this.ListView1);
    this.TabPage1.Controls.Add((Control) this.Label7);
    this.TabPage1.Controls.Add((Control) this.Label6);
    this.TabPage1.Location = new Point(4, 25);
    this.TabPage1.Margin = new Padding(4);
    this.TabPage1.Name = "TabPage1";
    this.TabPage1.Padding = new Padding(4);
    this.TabPage1.Size = new Size(991, 562);
    this.TabPage1.TabIndex = 0;
    this.TabPage1.Text = "Compare As-Built";
    this.chkCompareShowNames.AutoSize = true;
    this.chkCompareShowNames.Checked = true;
    this.chkCompareShowNames.CheckState = CheckState.Checked;
    this.chkCompareShowNames.Location = new Point(167, 180);
    this.chkCompareShowNames.Name = "chkCompareShowNames";
    this.chkCompareShowNames.Size = new Size(154, 20);
    this.chkCompareShowNames.TabIndex = 19;
    this.chkCompareShowNames.Text = "Show Module Names";
    this.chkCompareShowNames.UseVisualStyleBackColor = true;
    this.lblComp4VIN.AutoSize = true;
    this.lblComp4VIN.ForeColor = Color.SaddleBrown;
    this.lblComp4VIN.Location = new Point(642, 116);
    this.lblComp4VIN.Name = "lblComp4VIN";
    this.lblComp4VIN.Size = new Size(50, 16 /*0x10*/);
    this.lblComp4VIN.TabIndex = 18;
    this.lblComp4VIN.Text = "[no file]";
    this.lblComp3VIN.AutoSize = true;
    this.lblComp3VIN.ForeColor = Color.BlueViolet;
    this.lblComp3VIN.Location = new Point(642, 85);
    this.lblComp3VIN.Name = "lblComp3VIN";
    this.lblComp3VIN.Size = new Size(50, 16 /*0x10*/);
    this.lblComp3VIN.TabIndex = 17;
    this.lblComp3VIN.Text = "[no file]";
    this.lblComp2VIN.AutoSize = true;
    this.lblComp2VIN.ForeColor = Color.DarkGreen;
    this.lblComp2VIN.Location = new Point(642, 54);
    this.lblComp2VIN.Name = "lblComp2VIN";
    this.lblComp2VIN.Size = new Size(50, 16 /*0x10*/);
    this.lblComp2VIN.TabIndex = 16 /*0x10*/;
    this.lblComp2VIN.Text = "[no file]";
    this.lblComp1VIN.AutoSize = true;
    this.lblComp1VIN.ForeColor = Color.Blue;
    this.lblComp1VIN.Location = new Point(642, 23);
    this.lblComp1VIN.Name = "lblComp1VIN";
    this.lblComp1VIN.Size = new Size(50, 16 /*0x10*/);
    this.lblComp1VIN.TabIndex = 15;
    this.lblComp1VIN.Text = "[no file]";
    this.Button9.Location = new Point(538, 112 /*0x70*/);
    this.Button9.Name = "Button9";
    this.Button9.Size = new Size(98, 27);
    this.Button9.TabIndex = 14;
    this.Button9.Text = "Browse...";
    this.Button9.UseVisualStyleBackColor = true;
    this.tbxCompFile4.ForeColor = Color.SaddleBrown;
    this.tbxCompFile4.Location = new Point(167, 113);
    this.tbxCompFile4.Name = "tbxCompFile4";
    this.tbxCompFile4.Size = new Size(343, 22);
    this.tbxCompFile4.TabIndex = 13;
    this.Label23.AutoSize = true;
    this.Label23.ForeColor = Color.SaddleBrown;
    this.Label23.Location = new Point(24, 116);
    this.Label23.Name = "Label23";
    this.Label23.Size = new Size(100, 16 /*0x10*/);
    this.Label23.TabIndex = 12;
    this.Label23.Text = "As-Built File # 4:";
    this.Button7.Location = new Point(538, 81);
    this.Button7.Name = "Button7";
    this.Button7.Size = new Size(98, 27);
    this.Button7.TabIndex = 11;
    this.Button7.Text = "Browse...";
    this.Button7.UseVisualStyleBackColor = true;
    this.tbxCompFile3.ForeColor = Color.BlueViolet;
    this.tbxCompFile3.Location = new Point(167, 82);
    this.tbxCompFile3.Name = "tbxCompFile3";
    this.tbxCompFile3.Size = new Size(343, 22);
    this.tbxCompFile3.TabIndex = 10;
    this.Label22.AutoSize = true;
    this.Label22.ForeColor = Color.BlueViolet;
    this.Label22.Location = new Point(24, 85);
    this.Label22.Name = "Label22";
    this.Label22.Size = new Size(100, 16 /*0x10*/);
    this.Label22.TabIndex = 9;
    this.Label22.Text = "As-Built File # 3:";
    this.chkCompareShowChecksum.AutoSize = true;
    this.chkCompareShowChecksum.Location = new Point(167, 154);
    this.chkCompareShowChecksum.Name = "chkCompareShowChecksum";
    this.chkCompareShowChecksum.Size = new Size(159, 20);
    this.chkCompareShowChecksum.TabIndex = 8;
    this.chkCompareShowChecksum.Text = "Include Checksum bits";
    this.chkCompareShowChecksum.UseVisualStyleBackColor = true;
    this.btnCompLoad.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    this.btnCompLoad.Location = new Point(523, 145);
    this.btnCompLoad.Name = "btnCompLoad";
    this.btnCompLoad.Size = new Size(113, 59);
    this.btnCompLoad.TabIndex = 7;
    this.btnCompLoad.Text = "Load Files";
    this.btnCompLoad.UseVisualStyleBackColor = true;
    this.btnCompBrowse2.Location = new Point(538, 50);
    this.btnCompBrowse2.Name = "btnCompBrowse2";
    this.btnCompBrowse2.Size = new Size(98, 27);
    this.btnCompBrowse2.TabIndex = 6;
    this.btnCompBrowse2.Text = "Browse...";
    this.btnCompBrowse2.UseVisualStyleBackColor = true;
    this.btnCompBrowse1.Location = new Point(538, 19);
    this.btnCompBrowse1.Name = "btnCompBrowse1";
    this.btnCompBrowse1.Size = new Size(98, 27);
    this.btnCompBrowse1.TabIndex = 5;
    this.btnCompBrowse1.Text = "Browse...";
    this.btnCompBrowse1.UseVisualStyleBackColor = true;
    this.tbxCompFile2.ForeColor = Color.DarkGreen;
    this.tbxCompFile2.Location = new Point(167, 51);
    this.tbxCompFile2.Name = "tbxCompFile2";
    this.tbxCompFile2.Size = new Size(343, 22);
    this.tbxCompFile2.TabIndex = 4;
    this.tbxCompFile1.ForeColor = Color.Blue;
    this.tbxCompFile1.Location = new Point(167, 20);
    this.tbxCompFile1.Name = "tbxCompFile1";
    this.tbxCompFile1.Size = new Size(343, 22);
    this.tbxCompFile1.TabIndex = 3;
    this.ListView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.ListView1.Columns.AddRange(new ColumnHeader[9]
    {
      this.ColumnHeader1,
      this.ColumnHeader2,
      this.ColumnHeader3,
      this.ColumnHeader4,
      this.ColumnHeader5,
      this.ColumnHeader6,
      this.ColumnHeader7,
      this.ColumnHeader8,
      this.ColumnHeader9
    });
    this.ListView1.ContextMenuStrip = this.ContextMenuStrip1;
    this.ListView1.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.ListView1.FullRowSelect = true;
    this.ListView1.HideSelection = false;
    this.ListView1.Location = new Point(17, 243);
    this.ListView1.Name = "ListView1";
    this.ListView1.Size = new Size(956, 297);
    this.ListView1.TabIndex = 2;
    this.ListView1.UseCompatibleStateImageBehavior = false;
    this.ListView1.View = View.Details;
    this.ColumnHeader1.Text = "Module Address";
    this.ColumnHeader1.Width = 128 /*0x80*/;
    this.ColumnHeader2.Text = "Data1";
    this.ColumnHeader2.Width = 81;
    this.ColumnHeader3.Text = "Data2";
    this.ColumnHeader3.Width = 87;
    this.ColumnHeader4.Text = "Data3";
    this.ColumnHeader4.Width = 92;
    this.ColumnHeader5.Text = "Same?";
    this.ColumnHeader5.Width = 76;
    this.ColumnHeader6.Text = "Binary";
    this.ColumnHeader6.Width = 473;
    this.ColumnHeader7.Text = "PartNumber";
    this.ColumnHeader8.Text = "Strategy";
    this.ColumnHeader8.Width = 158;
    this.ColumnHeader9.Text = "Calibration";
    this.ColumnHeader9.Width = 158;
    this.ContextMenuStrip1.ImageScalingSize = new Size(20, 20);
    this.ContextMenuStrip1.Items.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.CopyToolStripMenuItem,
      (ToolStripItem) this.ExportModuleToolStripMenuItem
    });
    this.ContextMenuStrip1.Name = "ContextMenuStrip1";
    this.ContextMenuStrip1.Size = new Size(181, 70);
    this.CopyToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[5]
    {
      (ToolStripItem) this.EntireLineToolStripMenuItem,
      (ToolStripItem) this.Data1hexToolStripMenuItem,
      (ToolStripItem) this.Data2hexToolStripMenuItem,
      (ToolStripItem) this.Data3hexToolStripMenuItem,
      (ToolStripItem) this.BinaryToolStripMenuItem
    });
    this.CopyToolStripMenuItem.Name = "CopyToolStripMenuItem";
    this.CopyToolStripMenuItem.Size = new Size(151, 22);
    this.CopyToolStripMenuItem.Text = "Copy";
    this.EntireLineToolStripMenuItem.Name = "EntireLineToolStripMenuItem";
    this.EntireLineToolStripMenuItem.Size = new Size(180, 22);
    this.EntireLineToolStripMenuItem.Text = "Entire Line";
    this.Data1hexToolStripMenuItem.Name = "Data1hexToolStripMenuItem";
    this.Data1hexToolStripMenuItem.Size = new Size(180, 22);
    this.Data1hexToolStripMenuItem.Text = "Data 1 (hex)";
    this.Data2hexToolStripMenuItem.Name = "Data2hexToolStripMenuItem";
    this.Data2hexToolStripMenuItem.Size = new Size(180, 22);
    this.Data2hexToolStripMenuItem.Text = "Data 2 (hex)";
    this.Data3hexToolStripMenuItem.Name = "Data3hexToolStripMenuItem";
    this.Data3hexToolStripMenuItem.Size = new Size(180, 22);
    this.Data3hexToolStripMenuItem.Text = "Data 3 (hex)";
    this.BinaryToolStripMenuItem.Name = "BinaryToolStripMenuItem";
    this.BinaryToolStripMenuItem.Size = new Size(180, 22);
    this.BinaryToolStripMenuItem.Text = "Binary";
    this.ExportModuleToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.ToUCDSToolStripMenuItem,
      (ToolStripItem) this.ToABTToolStripMenuItem
    });
    this.ExportModuleToolStripMenuItem.Name = "ExportModuleToolStripMenuItem";
    this.ExportModuleToolStripMenuItem.Size = new Size(180, 22);
    this.ExportModuleToolStripMenuItem.Text = "Export Module";
    this.ToUCDSToolStripMenuItem.Name = "ToUCDSToolStripMenuItem";
    this.ToUCDSToolStripMenuItem.Size = new Size(180, 22);
    this.ToUCDSToolStripMenuItem.Text = "To UCDS...";
    this.ToABTToolStripMenuItem.Name = "ToABTToolStripMenuItem";
    this.ToABTToolStripMenuItem.Size = new Size(180, 22);
    this.ToABTToolStripMenuItem.Text = "To ABT...";
    this.Label7.AutoSize = true;
    this.Label7.ForeColor = Color.DarkGreen;
    this.Label7.Location = new Point(24, 54);
    this.Label7.Name = "Label7";
    this.Label7.Size = new Size(100, 16 /*0x10*/);
    this.Label7.TabIndex = 1;
    this.Label7.Text = "As-Built File # 2:";
    this.Label6.AutoSize = true;
    this.Label6.ForeColor = Color.Blue;
    this.Label6.Location = new Point(24, 23);
    this.Label6.Name = "Label6";
    this.Label6.Size = new Size(100, 16 /*0x10*/);
    this.Label6.TabIndex = 0;
    this.Label6.Text = "As-Built File # 1:";
    this.TabPage2.BackColor = SystemColors.Control;
    this.TabPage2.Controls.Add((Control) this.TextBox5);
    this.TabPage2.Controls.Add((Control) this.Button3);
    this.TabPage2.Controls.Add((Control) this.Button2);
    this.TabPage2.Controls.Add((Control) this.Label16);
    this.TabPage2.Controls.Add((Control) this.Label15);
    this.TabPage2.Controls.Add((Control) this.tbxChecksumBin);
    this.TabPage2.Controls.Add((Control) this.tbxConvertBin);
    this.TabPage2.Controls.Add((Control) this.tbxConvertHex);
    this.TabPage2.Controls.Add((Control) this.tbxData3bin2);
    this.TabPage2.Controls.Add((Control) this.tbxData2bin2);
    this.TabPage2.Controls.Add((Control) this.tbxData1bin2);
    this.TabPage2.Controls.Add((Control) this.tbxData3bin1);
    this.TabPage2.Controls.Add((Control) this.tbxData2bin1);
    this.TabPage2.Controls.Add((Control) this.tbxData1bin1);
    this.TabPage2.Controls.Add((Control) this.tbxChecksumHex);
    this.TabPage2.Controls.Add((Control) this.Label5);
    this.TabPage2.Controls.Add((Control) this.Button1);
    this.TabPage2.Controls.Add((Control) this.tbxData3hex);
    this.TabPage2.Controls.Add((Control) this.Label4);
    this.TabPage2.Controls.Add((Control) this.tbxData2hex);
    this.TabPage2.Controls.Add((Control) this.Label3);
    this.TabPage2.Controls.Add((Control) this.tbxData1hex);
    this.TabPage2.Controls.Add((Control) this.Label2);
    this.TabPage2.Controls.Add((Control) this.tbxModIDhex);
    this.TabPage2.Controls.Add((Control) this.Label1);
    this.TabPage2.Location = new Point(4, 22);
    this.TabPage2.Margin = new Padding(4);
    this.TabPage2.Name = "TabPage2";
    this.TabPage2.Padding = new Padding(4);
    this.TabPage2.Size = new Size(991, 472);
    this.TabPage2.TabIndex = 1;
    this.TabPage2.Text = "Checksum Calc";
    this.TextBox5.BackColor = SystemColors.Info;
    this.TextBox5.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.TextBox5.Location = new Point(485, 22);
    this.TextBox5.Multiline = true;
    this.TextBox5.Name = "TextBox5";
    this.TextBox5.Size = new Size(173, 215);
    this.TextBox5.TabIndex = 24;
    this.TextBox5.Text = "This is just a checksum calculator.  Entire your module data (including the checksum).  \r\n\r\nClick Re-Calc, and a new checksum will be provided below.";
    this.Button3.Location = new Point(86, 408);
    this.Button3.Name = "Button3";
    this.Button3.Size = new Size(100, 27);
    this.Button3.TabIndex = 23;
    this.Button3.Text = "Bin to Hex";
    this.Button3.UseVisualStyleBackColor = true;
    this.Button2.Location = new Point(86, 319);
    this.Button2.Name = "Button2";
    this.Button2.Size = new Size(100, 27);
    this.Button2.TabIndex = 22;
    this.Button2.Text = "Hex to Bin";
    this.Button2.UseVisualStyleBackColor = true;
    this.Label16.AutoSize = true;
    this.Label16.Location = new Point(33, 383);
    this.Label16.Name = "Label16";
    this.Label16.Size = new Size(29, 16 /*0x10*/);
    this.Label16.TabIndex = 21;
    this.Label16.Text = "Bin:";
    this.Label15.AutoSize = true;
    this.Label15.Location = new Point(33, 355);
    this.Label15.Name = "Label15";
    this.Label15.Size = new Size(34, 16 /*0x10*/);
    this.Label15.TabIndex = 20;
    this.Label15.Text = "Hex:";
    this.tbxChecksumBin.BackColor = SystemColors.Control;
    this.tbxChecksumBin.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxChecksumBin.Location = new Point(280, 219);
    this.tbxChecksumBin.Margin = new Padding(4);
    this.tbxChecksumBin.MaxLength = 8;
    this.tbxChecksumBin.Name = "tbxChecksumBin";
    this.tbxChecksumBin.Size = new Size(89, 22);
    this.tbxChecksumBin.TabIndex = 19;
    this.tbxChecksumBin.Text = "00011111";
    this.tbxConvertBin.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxConvertBin.Location = new Point(86, 380);
    this.tbxConvertBin.Name = "tbxConvertBin";
    this.tbxConvertBin.Size = new Size(170, 22);
    this.tbxConvertBin.TabIndex = 18;
    this.tbxConvertHex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxConvertHex.Location = new Point(86, 352);
    this.tbxConvertHex.Name = "tbxConvertHex";
    this.tbxConvertHex.Size = new Size(100, 22);
    this.tbxConvertHex.TabIndex = 17;
    this.tbxData3bin2.BackColor = SystemColors.Control;
    this.tbxData3bin2.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData3bin2.Location = new Point(326, 117);
    this.tbxData3bin2.Margin = new Padding(4);
    this.tbxData3bin2.MaxLength = 8;
    this.tbxData3bin2.Name = "tbxData3bin2";
    this.tbxData3bin2.Size = new Size(89, 22);
    this.tbxData3bin2.TabIndex = 16 /*0x10*/;
    this.tbxData3bin2.Text = "00011111";
    this.tbxData2bin2.BackColor = SystemColors.Control;
    this.tbxData2bin2.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData2bin2.Location = new Point(326, 85);
    this.tbxData2bin2.Margin = new Padding(4);
    this.tbxData2bin2.MaxLength = 8;
    this.tbxData2bin2.Name = "tbxData2bin2";
    this.tbxData2bin2.Size = new Size(89, 22);
    this.tbxData2bin2.TabIndex = 15;
    this.tbxData2bin2.Text = "11110000";
    this.tbxData1bin2.BackColor = SystemColors.Control;
    this.tbxData1bin2.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData1bin2.Location = new Point(326, 53);
    this.tbxData1bin2.Margin = new Padding(4);
    this.tbxData1bin2.MaxLength = 8;
    this.tbxData1bin2.Name = "tbxData1bin2";
    this.tbxData1bin2.Size = new Size(89, 22);
    this.tbxData1bin2.TabIndex = 14;
    this.tbxData1bin2.Text = "11111100";
    this.tbxData3bin1.BackColor = SystemColors.Control;
    this.tbxData3bin1.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData3bin1.Location = new Point(229, 117);
    this.tbxData3bin1.Margin = new Padding(4);
    this.tbxData3bin1.MaxLength = 8;
    this.tbxData3bin1.Name = "tbxData3bin1";
    this.tbxData3bin1.Size = new Size(89, 22);
    this.tbxData3bin1.TabIndex = 13;
    this.tbxData3bin1.Text = "00111111";
    this.tbxData2bin1.BackColor = SystemColors.Control;
    this.tbxData2bin1.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData2bin1.Location = new Point(229, 85);
    this.tbxData2bin1.Margin = new Padding(4);
    this.tbxData2bin1.MaxLength = 8;
    this.tbxData2bin1.Name = "tbxData2bin1";
    this.tbxData2bin1.Size = new Size(89, 22);
    this.tbxData2bin1.TabIndex = 12;
    this.tbxData2bin1.Text = "01000000";
    this.tbxData1bin1.BackColor = SystemColors.Control;
    this.tbxData1bin1.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData1bin1.Location = new Point(229, 53);
    this.tbxData1bin1.Margin = new Padding(4);
    this.tbxData1bin1.MaxLength = 8;
    this.tbxData1bin1.Name = "tbxData1bin1";
    this.tbxData1bin1.Size = new Size(89, 22);
    this.tbxData1bin1.TabIndex = 11;
    this.tbxData1bin1.Text = "10001010";
    this.tbxChecksumHex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxChecksumHex.Location = new Point(143, 219);
    this.tbxChecksumHex.Margin = new Padding(4);
    this.tbxChecksumHex.MaxLength = 2;
    this.tbxChecksumHex.Name = "tbxChecksumHex";
    this.tbxChecksumHex.Size = new Size(43, 22);
    this.tbxChecksumHex.TabIndex = 10;
    this.tbxChecksumHex.Text = "1F";
    this.tbxChecksumHex.TextAlign = HorizontalAlignment.Center;
    this.Label5.AutoSize = true;
    this.Label5.Location = new Point(28, 221);
    this.Label5.Margin = new Padding(4, 0, 4, 0);
    this.Label5.Name = "Label5";
    this.Label5.Size = new Size(73, 16 /*0x10*/);
    this.Label5.TabIndex = 9;
    this.Label5.Text = "Checksum:";
    this.Button1.Location = new Point(30, 157);
    this.Button1.Margin = new Padding(4);
    this.Button1.Name = "Button1";
    this.Button1.Size = new Size(268, 41);
    this.Button1.TabIndex = 8;
    this.Button1.Text = "Re-Calc Checksum (last byte)";
    this.Button1.UseVisualStyleBackColor = true;
    this.tbxData3hex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData3hex.Location = new Point(118, 117);
    this.tbxData3hex.Margin = new Padding(4);
    this.tbxData3hex.MaxLength = 5;
    this.tbxData3hex.Name = "tbxData3hex";
    this.tbxData3hex.Size = new Size(68, 22);
    this.tbxData3hex.TabIndex = 7;
    this.tbxData3hex.Text = "3F1F";
    this.Label4.AutoSize = true;
    this.Label4.Location = new Point(27, 119);
    this.Label4.Margin = new Padding(4, 0, 4, 0);
    this.Label4.Name = "Label4";
    this.Label4.Size = new Size(39, 16 /*0x10*/);
    this.Label4.TabIndex = 6;
    this.Label4.Text = "Data:";
    this.tbxData2hex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData2hex.Location = new Point(118, 85);
    this.tbxData2hex.Margin = new Padding(4);
    this.tbxData2hex.MaxLength = 5;
    this.tbxData2hex.Name = "tbxData2hex";
    this.tbxData2hex.Size = new Size(68, 22);
    this.tbxData2hex.TabIndex = 5;
    this.tbxData2hex.Text = "40F0";
    this.Label3.AutoSize = true;
    this.Label3.Location = new Point(27, 87);
    this.Label3.Margin = new Padding(4, 0, 4, 0);
    this.Label3.Name = "Label3";
    this.Label3.Size = new Size(39, 16 /*0x10*/);
    this.Label3.TabIndex = 4;
    this.Label3.Text = "Data:";
    this.tbxData1hex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxData1hex.Location = new Point(118, 53);
    this.tbxData1hex.Margin = new Padding(4);
    this.tbxData1hex.MaxLength = 5;
    this.tbxData1hex.Name = "tbxData1hex";
    this.tbxData1hex.Size = new Size(68, 22);
    this.tbxData1hex.TabIndex = 3;
    this.tbxData1hex.Text = "8AFC";
    this.Label2.AutoSize = true;
    this.Label2.Location = new Point(27, 55);
    this.Label2.Margin = new Padding(4, 0, 4, 0);
    this.Label2.Name = "Label2";
    this.Label2.Size = new Size(39, 16 /*0x10*/);
    this.Label2.TabIndex = 2;
    this.Label2.Text = "Data:";
    this.tbxModIDhex.Font = new Font("Courier New", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.tbxModIDhex.Location = new Point(118, 23);
    this.tbxModIDhex.Margin = new Padding(4);
    this.tbxModIDhex.MaxLength = 10;
    this.tbxModIDhex.Name = "tbxModIDhex";
    this.tbxModIDhex.Size = new Size(102, 22);
    this.tbxModIDhex.TabIndex = 1;
    this.tbxModIDhex.Text = "720-01-02";
    this.Label1.AutoSize = true;
    this.Label1.Location = new Point(27, 25);
    this.Label1.Margin = new Padding(4, 0, 4, 0);
    this.Label1.Name = "Label1";
    this.Label1.Size = new Size(71, 16 /*0x10*/);
    this.Label1.TabIndex = 0;
    this.Label1.Text = "Module ID:";
    this.TabPage3.BackColor = SystemColors.Control;
    this.TabPage3.Controls.Add((Control) this.TextBox6);
    this.TabPage3.Controls.Add((Control) this.TextBox2);
    this.TabPage3.Controls.Add((Control) this.Label12);
    this.TabPage3.Controls.Add((Control) this.Label9);
    this.TabPage3.Controls.Add((Control) this.chkDeduceDownloadAB);
    this.TabPage3.Controls.Add((Control) this.btnDeduceSaveInfo);
    this.TabPage3.Controls.Add((Control) this.wbDeducer);
    this.TabPage3.Controls.Add((Control) this.btnDeduceOpenETIS);
    this.TabPage3.Location = new Point(4, 25);
    this.TabPage3.Name = "TabPage3";
    this.TabPage3.Size = new Size(991, 562);
    this.TabPage3.TabIndex = 2;
    this.TabPage3.Text = "Deducer - Get Data";
    this.TextBox2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.TextBox2.BackColor = SystemColors.Info;
    this.TextBox2.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.TextBox2.Location = new Point(596, 232);
    this.TextBox2.Multiline = true;
    this.TextBox2.Name = "TextBox2";
    this.TextBox2.Size = new Size(199, 308);
    this.TextBox2.TabIndex = 9;
    this.TextBox2.Text = componentResourceManager.GetString("TextBox2.Text");
    this.Label12.AutoSize = true;
    this.Label12.Location = new Point(214, 48 /*0x30*/);
    this.Label12.Name = "Label12";
    this.Label12.Size = new Size(456, 16 /*0x10*/);
    this.Label12.TabIndex = 2;
    this.Label12.Text = "When prompted, save the .AB file in the \"Deducer\" subfolder of this program.";
    this.Label9.AutoSize = true;
    this.Label9.Location = new Point(18, 48 /*0x30*/);
    this.Label9.Name = "Label9";
    this.Label9.Size = new Size(138, 16 /*0x10*/);
    this.Label9.TabIndex = 8;
    this.Label9.Text = "Search the VIN below:";
    this.chkDeduceDownloadAB.AutoSize = true;
    this.chkDeduceDownloadAB.Checked = true;
    this.chkDeduceDownloadAB.CheckState = CheckState.Checked;
    this.chkDeduceDownloadAB.Enabled = false;
    this.chkDeduceDownloadAB.Location = new Point(340, 21);
    this.chkDeduceDownloadAB.Name = "chkDeduceDownloadAB";
    this.chkDeduceDownloadAB.Size = new Size(111, 20);
    this.chkDeduceDownloadAB.TabIndex = 7;
    this.chkDeduceDownloadAB.Text = "Download .AB";
    this.chkDeduceDownloadAB.UseVisualStyleBackColor = true;
    this.chkDeduceDownloadAB.Visible = false;
    this.btnDeduceSaveInfo.Location = new Point(208 /*0xD0*/, 17);
    this.btnDeduceSaveInfo.Name = "btnDeduceSaveInfo";
    this.btnDeduceSaveInfo.Size = new Size(96 /*0x60*/, 26);
    this.btnDeduceSaveInfo.TabIndex = 5;
    this.btnDeduceSaveInfo.Text = "Save Info";
    this.btnDeduceSaveInfo.UseVisualStyleBackColor = true;
    this.wbDeducer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.wbDeducer.Location = new Point(15, 86);
    this.wbDeducer.MinimumSize = new Size(20, 20);
    this.wbDeducer.Name = "wbDeducer";
    this.wbDeducer.Size = new Size(552, 454);
    this.wbDeducer.TabIndex = 4;
    this.btnDeduceOpenETIS.Location = new Point(15, 17);
    this.btnDeduceOpenETIS.Name = "btnDeduceOpenETIS";
    this.btnDeduceOpenETIS.Size = new Size(106, 26);
    this.btnDeduceOpenETIS.TabIndex = 2;
    this.btnDeduceOpenETIS.Text = "Open ETIS";
    this.btnDeduceOpenETIS.UseVisualStyleBackColor = true;
    this.TabPage4.BackColor = SystemColors.Control;
    this.TabPage4.Controls.Add((Control) this.chkDeduceDoCCC);
    this.TabPage4.Controls.Add((Control) this.Button8);
    this.TabPage4.Controls.Add((Control) this.Label14);
    this.TabPage4.Controls.Add((Control) this.TextBox1);
    this.TabPage4.Controls.Add((Control) this.Label13);
    this.TabPage4.Controls.Add((Control) this.Label11);
    this.TabPage4.Controls.Add((Control) this.Label10);
    this.TabPage4.Controls.Add((Control) this.lstDeduceYears);
    this.TabPage4.Controls.Add((Control) this.lstDeduceModels);
    this.TabPage4.Controls.Add((Control) this.tbxDeduceReport);
    this.TabPage4.Controls.Add((Control) this.btnDeduceFigureIt);
    this.TabPage4.Controls.Add((Control) this.btnDeduceLoadOptions);
    this.TabPage4.Controls.Add((Control) this.lstDeduceFactoryOptions);
    this.TabPage4.Location = new Point(4, 25);
    this.TabPage4.Name = "TabPage4";
    this.TabPage4.Size = new Size(991, 562);
    this.TabPage4.TabIndex = 3;
    this.TabPage4.Text = "Deducer - Feature by Vehicles";
    this.chkDeduceDoCCC.AutoSize = true;
    this.chkDeduceDoCCC.Location = new Point(622, 20);
    this.chkDeduceDoCCC.Name = "chkDeduceDoCCC";
    this.chkDeduceDoCCC.Size = new Size(180, 20);
    this.chkDeduceDoCCC.TabIndex = 12;
    this.chkDeduceDoCCC.Text = "Compare CCC bits as well";
    this.chkDeduceDoCCC.UseVisualStyleBackColor = true;
    this.Button8.Location = new Point(622, 425);
    this.Button8.Name = "Button8";
    this.Button8.Size = new Size(173, 27);
    this.Button8.TabIndex = 11;
    this.Button8.Text = "Try Every Option";
    this.Button8.UseVisualStyleBackColor = true;
    this.Button8.Visible = false;
    this.Label14.AutoSize = true;
    this.Label14.Location = new Point(12, 428);
    this.Label14.Name = "Label14";
    this.Label14.Size = new Size(522, 16 /*0x10*/);
    this.Label14.TabIndex = 10;
    this.Label14.Text = "\"Perfect Bits\" are listed at the bottom.  Bits are numbered from left to right starting at zero.";
    this.TextBox1.BackColor = SystemColors.Info;
    this.TextBox1.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.TextBox1.Location = new Point(622, 81);
    this.TextBox1.Multiline = true;
    this.TextBox1.Name = "TextBox1";
    this.TextBox1.Size = new Size(173, 335);
    this.TextBox1.TabIndex = 9;
    this.TextBox1.Text = componentResourceManager.GetString("TextBox1.Text");
    this.Label13.AutoSize = true;
    this.Label13.Location = new Point(12, 212);
    this.Label13.Name = "Label13";
    this.Label13.Size = new Size(133, 16 /*0x10*/);
    this.Label13.TabIndex = 8;
    this.Label13.Text = "Review results below";
    this.Label11.AutoSize = true;
    this.Label11.Location = new Point(12, 140);
    this.Label11.Name = "Label11";
    this.Label11.Size = new Size(161, 16 /*0x10*/);
    this.Label11.TabIndex = 7;
    this.Label11.Text = "Select Years and Models:";
    this.Label10.AutoSize = true;
    this.Label10.Location = new Point(12, 69);
    this.Label10.Name = "Label10";
    this.Label10.Size = new Size(155, 16 /*0x10*/);
    this.Label10.TabIndex = 6;
    this.Label10.Text = "Select feature to deduce:";
    this.lstDeduceYears.FormattingEnabled = true;
    this.lstDeduceYears.IntegralHeight = false;
    this.lstDeduceYears.Location = new Point(218, 126);
    this.lstDeduceYears.Name = "lstDeduceYears";
    this.lstDeduceYears.Size = new Size(120, 72);
    this.lstDeduceYears.TabIndex = 5;
    this.lstDeduceModels.FormattingEnabled = true;
    this.lstDeduceModels.IntegralHeight = false;
    this.lstDeduceModels.Location = new Point(353, 126);
    this.lstDeduceModels.Name = "lstDeduceModels";
    this.lstDeduceModels.Size = new Size(233, 72);
    this.lstDeduceModels.TabIndex = 4;
    this.tbxDeduceReport.BorderStyle = BorderStyle.FixedSingle;
    this.tbxDeduceReport.Location = new Point(15, 235);
    this.tbxDeduceReport.Multiline = true;
    this.tbxDeduceReport.Name = "tbxDeduceReport";
    this.tbxDeduceReport.ScrollBars = ScrollBars.Vertical;
    this.tbxDeduceReport.Size = new Size(588, 181);
    this.tbxDeduceReport.TabIndex = 3;
    this.btnDeduceFigureIt.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    this.btnDeduceFigureIt.Location = new Point(622, 47);
    this.btnDeduceFigureIt.Name = "btnDeduceFigureIt";
    this.btnDeduceFigureIt.Size = new Size(173, 28);
    this.btnDeduceFigureIt.TabIndex = 2;
    this.btnDeduceFigureIt.Text = "Find Perfect Bits";
    this.btnDeduceFigureIt.UseVisualStyleBackColor = true;
    this.btnDeduceLoadOptions.Location = new Point(15, 20);
    this.btnDeduceLoadOptions.Name = "btnDeduceLoadOptions";
    this.btnDeduceLoadOptions.Size = new Size(155, 28);
    this.btnDeduceLoadOptions.TabIndex = 1;
    this.btnDeduceLoadOptions.Text = "Load Stored Data";
    this.btnDeduceLoadOptions.UseVisualStyleBackColor = true;
    this.lstDeduceFactoryOptions.FormattingEnabled = true;
    this.lstDeduceFactoryOptions.ItemHeight = 16 /*0x10*/;
    this.lstDeduceFactoryOptions.Location = new Point(218, 20);
    this.lstDeduceFactoryOptions.Name = "lstDeduceFactoryOptions";
    this.lstDeduceFactoryOptions.SelectionMode = SelectionMode.MultiExtended;
    this.lstDeduceFactoryOptions.Size = new Size(368, 84);
    this.lstDeduceFactoryOptions.TabIndex = 0;
    this.TabPage5.BackColor = SystemColors.Control;
    this.TabPage5.Controls.Add((Control) this.Button10);
    this.TabPage5.Controls.Add((Control) this.TextBox4);
    this.TabPage5.Controls.Add((Control) this.Label21);
    this.TabPage5.Controls.Add((Control) this.lstBit_Modules);
    this.TabPage5.Controls.Add((Control) this.Button6);
    this.TabPage5.Controls.Add((Control) this.Button5);
    this.TabPage5.Controls.Add((Control) this.Label20);
    this.TabPage5.Controls.Add((Control) this.Label19);
    this.TabPage5.Controls.Add((Control) this.lstBit_Years);
    this.TabPage5.Controls.Add((Control) this.lstBit_Models);
    this.TabPage5.Controls.Add((Control) this.Label17);
    this.TabPage5.Controls.Add((Control) this.TextBox3);
    this.TabPage5.Controls.Add((Control) this.Label18);
    this.TabPage5.Controls.Add((Control) this.tbxDeduceReport2);
    this.TabPage5.Controls.Add((Control) this.Button4);
    this.TabPage5.Location = new Point(4, 25);
    this.TabPage5.Name = "TabPage5";
    this.TabPage5.Padding = new Padding(3);
    this.TabPage5.Size = new Size(991, 562);
    this.TabPage5.TabIndex = 4;
    this.TabPage5.Text = "Deducer - Vehicle/Feature by Bit";
    this.Button10.Location = new Point(630, 422);
    this.Button10.Name = "Button10";
    this.Button10.Size = new Size(173, 28);
    this.Button10.TabIndex = 31 /*0x1F*/;
    this.Button10.Text = "Analyze All bits";
    this.Button10.UseVisualStyleBackColor = true;
    this.TextBox4.Location = new Point(226, 253);
    this.TextBox4.Name = "TextBox4";
    this.TextBox4.Size = new Size(76, 22);
    this.TextBox4.TabIndex = 30;
    this.TextBox4.Text = "0";
    this.TextBox4.TextAlign = HorizontalAlignment.Center;
    this.Label21.AutoSize = true;
    this.Label21.Location = new Point(20, 256 /*0x0100*/);
    this.Label21.Name = "Label21";
    this.Label21.Size = new Size(150, 16 /*0x10*/);
    this.Label21.TabIndex = 29;
    this.Label21.Text = "Select Bit Index (0 to 39):";
    this.lstBit_Modules.FormattingEnabled = true;
    this.lstBit_Modules.ItemHeight = 16 /*0x10*/;
    this.lstBit_Modules.Location = new Point(226, 152);
    this.lstBit_Modules.Name = "lstBit_Modules";
    this.lstBit_Modules.Size = new Size(365, 84);
    this.lstBit_Modules.TabIndex = 28;
    this.Button6.Location = new Point(23, 152);
    this.Button6.Name = "Button6";
    this.Button6.Size = new Size(155, 28);
    this.Button6.TabIndex = 27;
    this.Button6.Text = "Load Modules";
    this.Button6.UseVisualStyleBackColor = true;
    this.Button5.Location = new Point(23, 23);
    this.Button5.Name = "Button5";
    this.Button5.Size = new Size(155, 28);
    this.Button5.TabIndex = 26;
    this.Button5.Text = "Load Stored Data";
    this.Button5.UseVisualStyleBackColor = true;
    this.Label20.AutoSize = true;
    this.Label20.Location = new Point(20, 199);
    this.Label20.Name = "Label20";
    this.Label20.Size = new Size(96 /*0x60*/, 16 /*0x10*/);
    this.Label20.TabIndex = 25;
    this.Label20.Text = "Select Module:";
    this.Label19.AutoSize = true;
    this.Label19.Location = new Point(20, 72);
    this.Label19.Name = "Label19";
    this.Label19.Size = new Size(161, 16 /*0x10*/);
    this.Label19.TabIndex = 24;
    this.Label19.Text = "Select Years and Models:";
    this.lstBit_Years.FormattingEnabled = true;
    this.lstBit_Years.IntegralHeight = false;
    this.lstBit_Years.Location = new Point(226, 23);
    this.lstBit_Years.Name = "lstBit_Years";
    this.lstBit_Years.Size = new Size(120, 101);
    this.lstBit_Years.TabIndex = 23;
    this.lstBit_Models.FormattingEnabled = true;
    this.lstBit_Models.IntegralHeight = false;
    this.lstBit_Models.Location = new Point(358, 23);
    this.lstBit_Models.Name = "lstBit_Models";
    this.lstBit_Models.Size = new Size(233, 101);
    this.lstBit_Models.TabIndex = 22;
    this.Label17.AutoSize = true;
    this.Label17.Location = new Point(20, 495);
    this.Label17.Name = "Label17";
    this.Label17.Size = new Size(522, 16 /*0x10*/);
    this.Label17.TabIndex = 21;
    this.Label17.Text = "\"Perfect Bits\" are listed at the bottom.  Bits are numbered from left to right starting at zero.";
    this.TextBox3.BackColor = SystemColors.Info;
    this.TextBox3.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.TextBox3.Location = new Point(630, 72);
    this.TextBox3.Multiline = true;
    this.TextBox3.Name = "TextBox3";
    this.TextBox3.Size = new Size(173, 344);
    this.TextBox3.TabIndex = 20;
    this.TextBox3.Text = "This tool attempts to deduce which feature(s) corresponds to the chosen module bit.  \r\n\r\nThis is a statistical analysis and only identifies consistencies -- not guarantees.";
    this.Label18.AutoSize = true;
    this.Label18.Location = new Point(20, 320);
    this.Label18.Name = "Label18";
    this.Label18.Size = new Size(133, 16 /*0x10*/);
    this.Label18.TabIndex = 19;
    this.Label18.Text = "Review results below";
    this.tbxDeduceReport2.BorderStyle = BorderStyle.FixedSingle;
    this.tbxDeduceReport2.Location = new Point(24, 343);
    this.tbxDeduceReport2.MaxLength = 5000000;
    this.tbxDeduceReport2.Multiline = true;
    this.tbxDeduceReport2.Name = "tbxDeduceReport2";
    this.tbxDeduceReport2.ScrollBars = ScrollBars.Vertical;
    this.tbxDeduceReport2.Size = new Size(588, 119);
    this.tbxDeduceReport2.TabIndex = 14;
    this.Button4.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    this.Button4.Location = new Point(630, 23);
    this.Button4.Name = "Button4";
    this.Button4.Size = new Size(173, 28);
    this.Button4.TabIndex = 13;
    this.Button4.Text = "Find Feature";
    this.Button4.UseVisualStyleBackColor = true;
    this.TabPage6.BackColor = SystemColors.Control;
    this.TabPage6.Controls.Add((Control) this.btnBrowseRefresh);
    this.TabPage6.Controls.Add((Control) this.lvwBrowser);
    this.TabPage6.Location = new Point(4, 25);
    this.TabPage6.Name = "TabPage6";
    this.TabPage6.Padding = new Padding(3);
    this.TabPage6.Size = new Size(991, 562);
    this.TabPage6.TabIndex = 5;
    this.TabPage6.Text = "AsBuilt Browser";
    this.btnBrowseRefresh.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    this.btnBrowseRefresh.Location = new Point(6, 13);
    this.btnBrowseRefresh.Name = "btnBrowseRefresh";
    this.btnBrowseRefresh.Size = new Size(103, 32 /*0x20*/);
    this.btnBrowseRefresh.TabIndex = 1;
    this.btnBrowseRefresh.Text = "Refresh";
    this.btnBrowseRefresh.UseVisualStyleBackColor = true;
    this.lvwBrowser.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.lvwBrowser.Columns.AddRange(new ColumnHeader[5]
    {
      this.ColumnHeader10,
      this.ColumnHeader11,
      this.ColumnHeader12,
      this.ColumnHeader13,
      this.ColumnHeader14
    });
    this.lvwBrowser.ContextMenuStrip = this.ContextMenuStrip2;
    this.lvwBrowser.FullRowSelect = true;
    this.lvwBrowser.GridLines = true;
    this.lvwBrowser.HideSelection = false;
    this.lvwBrowser.Location = new Point(6, 54);
    this.lvwBrowser.Name = "lvwBrowser";
    this.lvwBrowser.Size = new Size(979, 495);
    this.lvwBrowser.TabIndex = 0;
    this.lvwBrowser.UseCompatibleStateImageBehavior = false;
    this.lvwBrowser.View = View.Details;
    this.ColumnHeader10.Text = "Name";
    this.ColumnHeader10.Width = 250;
    this.ColumnHeader11.Text = "Date";
    this.ColumnHeader11.Width = 150;
    this.ColumnHeader12.Text = "Year";
    this.ColumnHeader12.Width = 70;
    this.ColumnHeader13.Text = "Model";
    this.ColumnHeader13.Width = 220;
    this.ColumnHeader14.Text = "VIN";
    this.ColumnHeader14.Width = 225;
    this.ContextMenuStrip2.ImageScalingSize = new Size(20, 20);
    this.ContextMenuStrip2.Items.AddRange(new ToolStripItem[6]
    {
      (ToolStripItem) this.SetAsCompare1ToolStripMenuItem,
      (ToolStripItem) this.SetAsCompare2ToolStripMenuItem,
      (ToolStripItem) this.SetAsCompare3ToolStripMenuItem,
      (ToolStripItem) this.SetAsCompare4ToolStripMenuItem,
      (ToolStripItem) this.ToolStripMenuItem1,
      (ToolStripItem) this.DeleteFileToolStripMenuItem
    });
    this.ContextMenuStrip2.Name = "ContextMenuStrip2";
    this.ContextMenuStrip2.Size = new Size(176 /*0xB0*/, 120);
    this.SetAsCompare1ToolStripMenuItem.Name = "SetAsCompare1ToolStripMenuItem";
    this.SetAsCompare1ToolStripMenuItem.Size = new Size(175, 22);
    this.SetAsCompare1ToolStripMenuItem.Text = "Set as Compare # 1";
    this.SetAsCompare2ToolStripMenuItem.Name = "SetAsCompare2ToolStripMenuItem";
    this.SetAsCompare2ToolStripMenuItem.Size = new Size(175, 22);
    this.SetAsCompare2ToolStripMenuItem.Text = "Set as Compare # 2";
    this.SetAsCompare3ToolStripMenuItem.Name = "SetAsCompare3ToolStripMenuItem";
    this.SetAsCompare3ToolStripMenuItem.Size = new Size(175, 22);
    this.SetAsCompare3ToolStripMenuItem.Text = "Set as Compare # 3";
    this.SetAsCompare4ToolStripMenuItem.Name = "SetAsCompare4ToolStripMenuItem";
    this.SetAsCompare4ToolStripMenuItem.Size = new Size(175, 22);
    this.SetAsCompare4ToolStripMenuItem.Text = "Set as Compare # 4";
    this.ToolStripMenuItem1.Name = "ToolStripMenuItem1";
    this.ToolStripMenuItem1.Size = new Size(172, 6);
    this.DeleteFileToolStripMenuItem.Name = "DeleteFileToolStripMenuItem";
    this.DeleteFileToolStripMenuItem.Size = new Size(175, 22);
    this.DeleteFileToolStripMenuItem.Text = "Delete File";
    this.Label8.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.Label8.AutoSize = true;
    this.Label8.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.Label8.ForeColor = Color.FromArgb(0, 0, 192 /*0xC0*/);
    this.Label8.Location = new Point(269, 622);
    this.Label8.Name = "Label8";
    this.Label8.Size = new Size(347, 15);
    this.Label8.TabIndex = 1;
    this.Label8.Text = "Written by Jesse Yeager                    www.CompulsiveCode.com";
    this.PictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
    this.PictureBox1.Cursor = Cursors.Hand;
    this.PictureBox1.Image = (Image) componentResourceManager.GetObject("PictureBox1.Image");
    this.PictureBox1.Location = new Point(11, 601);
    this.PictureBox1.Name = "PictureBox1";
    this.PictureBox1.Size = new Size(115, 52);
    this.PictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
    this.PictureBox1.TabIndex = 3;
    this.PictureBox1.TabStop = false;
    this.chkShowOnlyMismatches.AutoSize = true;
    this.chkShowOnlyMismatches.Location = new Point(167, 206);
    this.chkShowOnlyMismatches.Name = "chkShowOnlyMismatches";
    this.chkShowOnlyMismatches.Size = new Size(222, 20);
    this.chkShowOnlyMismatches.TabIndex = 20;
    this.chkShowOnlyMismatches.Text = "Only Show Non-Matching Groups";
    this.chkShowOnlyMismatches.UseVisualStyleBackColor = true;
    this.TextBox6.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.TextBox6.BackColor = SystemColors.Info;
    this.TextBox6.Font = new Font("Microsoft Sans Serif", 9f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.TextBox6.ForeColor = Color.Red;
    this.TextBox6.Location = new Point(596, 86);
    this.TextBox6.Multiline = true;
    this.TextBox6.Name = "TextBox6";
    this.TextBox6.Size = new Size(199, 124);
    this.TextBox6.TabIndex = 10;
    this.TextBox6.Text = "The Deducer tool is now unusable, due to the ETIS site being shut down.  Unless there is another site to correlate VIN to vehicle features, this will remain broken.";
    this.AutoScaleMode = AutoScaleMode.None;
    this.ClientSize = new Size(1029, 646);
    this.Controls.Add((Control) this.Label8);
    this.Controls.Add((Control) this.TabControl1);
    this.Controls.Add((Control) this.PictureBox1);
    this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.Icon = (Icon) componentResourceManager.GetObject("$this.Icon");
    this.Margin = new Padding(4);
    this.MinimumSize = new Size(913, 500);
    this.Name = nameof (Form1);
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
    ((ISupportInitialize) this.PictureBox1).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  [field: AccessedThroughProperty("TabControl1")]
  internal virtual TabControl TabControl1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual TabPage TabPage1
  {
    get => this._TabPage1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.TabPage1_Click);
      TabPage tabPage1_1 = this._TabPage1;
      if (tabPage1_1 != null)
        tabPage1_1.Click -= eventHandler;
      this._TabPage1 = value;
      TabPage tabPage1_2 = this._TabPage1;
      if (tabPage1_2 == null)
        return;
      tabPage1_2.Click += eventHandler;
    }
  }

  internal virtual TabPage TabPage2
  {
    get => this._TabPage2;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.TabPage2_Click);
      TabPage tabPage2_1 = this._TabPage2;
      if (tabPage2_1 != null)
        tabPage2_1.Click -= eventHandler;
      this._TabPage2 = value;
      TabPage tabPage2_2 = this._TabPage2;
      if (tabPage2_2 == null)
        return;
      tabPage2_2.Click += eventHandler;
    }
  }

  internal virtual TextBox tbxData2hex
  {
    get => this._tbxData2hex;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.TextBox3_TextChanged);
      TextBox tbxData2hex1 = this._tbxData2hex;
      if (tbxData2hex1 != null)
        tbxData2hex1.TextChanged -= eventHandler;
      this._tbxData2hex = value;
      TextBox tbxData2hex2 = this._tbxData2hex;
      if (tbxData2hex2 == null)
        return;
      tbxData2hex2.TextChanged += eventHandler;
    }
  }

  internal virtual Label Label3
  {
    get => this._Label3;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Label3_Click);
      Label label3_1 = this._Label3;
      if (label3_1 != null)
        label3_1.Click -= eventHandler;
      this._Label3 = value;
      Label label3_2 = this._Label3;
      if (label3_2 == null)
        return;
      label3_2.Click += eventHandler;
    }
  }

  internal virtual TextBox tbxData1hex
  {
    get => this._tbxData1hex;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.tbxData1hex_TextChanged);
      TextBox tbxData1hex1 = this._tbxData1hex;
      if (tbxData1hex1 != null)
        tbxData1hex1.TextChanged -= eventHandler;
      this._tbxData1hex = value;
      TextBox tbxData1hex2 = this._tbxData1hex;
      if (tbxData1hex2 == null)
        return;
      tbxData1hex2.TextChanged += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label2")]
  internal virtual Label Label2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxModIDhex")]
  internal virtual TextBox tbxModIDhex { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label1")]
  internal virtual Label Label1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual TextBox tbxChecksumHex
  {
    get => this._tbxChecksumHex;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.tbxChecksumHex_TextChanged);
      TextBox tbxChecksumHex1 = this._tbxChecksumHex;
      if (tbxChecksumHex1 != null)
        tbxChecksumHex1.TextChanged -= eventHandler;
      this._tbxChecksumHex = value;
      TextBox tbxChecksumHex2 = this._tbxChecksumHex;
      if (tbxChecksumHex2 == null)
        return;
      tbxChecksumHex2.TextChanged += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label5")]
  internal virtual Label Label5 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button1
  {
    get => this._Button1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button1_Click);
      Button button1_1 = this._Button1;
      if (button1_1 != null)
        button1_1.Click -= eventHandler;
      this._Button1 = value;
      Button button1_2 = this._Button1;
      if (button1_2 == null)
        return;
      button1_2.Click += eventHandler;
    }
  }

  internal virtual TextBox tbxData3hex
  {
    get => this._tbxData3hex;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.tbxData3hex_TextChanged);
      TextBox tbxData3hex1 = this._tbxData3hex;
      if (tbxData3hex1 != null)
        tbxData3hex1.TextChanged -= eventHandler;
      this._tbxData3hex = value;
      TextBox tbxData3hex2 = this._tbxData3hex;
      if (tbxData3hex2 == null)
        return;
      tbxData3hex2.TextChanged += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label4")]
  internal virtual Label Label4 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData3bin2")]
  internal virtual TextBox tbxData3bin2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData2bin2")]
  internal virtual TextBox tbxData2bin2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData1bin2")]
  internal virtual TextBox tbxData1bin2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData3bin1")]
  internal virtual TextBox tbxData3bin1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData2bin1")]
  internal virtual TextBox tbxData2bin1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxData1bin1")]
  internal virtual TextBox tbxData1bin1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button btnCompBrowse2
  {
    get => this._btnCompBrowse2;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.btnCompBrowse2_Click);
      Button btnCompBrowse2_1 = this._btnCompBrowse2;
      if (btnCompBrowse2_1 != null)
        btnCompBrowse2_1.Click -= eventHandler;
      this._btnCompBrowse2 = value;
      Button btnCompBrowse2_2 = this._btnCompBrowse2;
      if (btnCompBrowse2_2 == null)
        return;
      btnCompBrowse2_2.Click += eventHandler;
    }
  }

  internal virtual Button btnCompBrowse1
  {
    get => this._btnCompBrowse1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.btnCompBrowse1_Click);
      Button btnCompBrowse1_1 = this._btnCompBrowse1;
      if (btnCompBrowse1_1 != null)
        btnCompBrowse1_1.Click -= eventHandler;
      this._btnCompBrowse1 = value;
      Button btnCompBrowse1_2 = this._btnCompBrowse1;
      if (btnCompBrowse1_2 == null)
        return;
      btnCompBrowse1_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("tbxCompFile2")]
  internal virtual TextBox tbxCompFile2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxCompFile1")]
  internal virtual TextBox tbxCompFile1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ListView ListView1
  {
    get => this._ListView1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.ListView1_SelectedIndexChanged);
      ListView listView1_1 = this._ListView1;
      if (listView1_1 != null)
        listView1_1.SelectedIndexChanged -= eventHandler;
      this._ListView1 = value;
      ListView listView1_2 = this._ListView1;
      if (listView1_2 == null)
        return;
      listView1_2.SelectedIndexChanged += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label7")]
  internal virtual Label Label7 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label6")]
  internal virtual Label Label6 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button btnCompLoad
  {
    get => this._btnCompLoad;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button2_Click);
      Button btnCompLoad1 = this._btnCompLoad;
      if (btnCompLoad1 != null)
        btnCompLoad1.Click -= eventHandler;
      this._btnCompLoad = value;
      Button btnCompLoad2 = this._btnCompLoad;
      if (btnCompLoad2 == null)
        return;
      btnCompLoad2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("ColumnHeader1")]
  internal virtual ColumnHeader ColumnHeader1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader2")]
  internal virtual ColumnHeader ColumnHeader2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader3")]
  internal virtual ColumnHeader ColumnHeader3 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader4")]
  internal virtual ColumnHeader ColumnHeader4 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader5")]
  internal virtual ColumnHeader ColumnHeader5 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader6")]
  internal virtual ColumnHeader ColumnHeader6 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label8")]
  internal virtual Label Label8 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ContextMenuStrip ContextMenuStrip1
  {
    get => this._ContextMenuStrip1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      CancelEventHandler cancelEventHandler = new CancelEventHandler(this.ContextMenuStrip1_Opening);
      ContextMenuStrip contextMenuStrip1_1 = this._ContextMenuStrip1;
      if (contextMenuStrip1_1 != null)
        contextMenuStrip1_1.Opening -= cancelEventHandler;
      this._ContextMenuStrip1 = value;
      ContextMenuStrip contextMenuStrip1_2 = this._ContextMenuStrip1;
      if (contextMenuStrip1_2 == null)
        return;
      contextMenuStrip1_2.Opening += cancelEventHandler;
    }
  }

  internal virtual ToolStripMenuItem CopyToolStripMenuItem
  {
    get => this._CopyToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.CopyToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._CopyToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._CopyToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._CopyToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem EntireLineToolStripMenuItem
  {
    get => this._EntireLineToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.EntireLineToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._EntireLineToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._EntireLineToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._EntireLineToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem Data1hexToolStripMenuItem
  {
    get => this._Data1hexToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Data1hexToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._Data1hexToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._Data1hexToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._Data1hexToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem Data2hexToolStripMenuItem
  {
    get => this._Data2hexToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Data2hexToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._Data2hexToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._Data2hexToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._Data2hexToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem Data3hexToolStripMenuItem
  {
    get => this._Data3hexToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Data3hexToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._Data3hexToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._Data3hexToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._Data3hexToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem BinaryToolStripMenuItem
  {
    get => this._BinaryToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.BinaryToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._BinaryToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._BinaryToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._BinaryToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("TabPage3")]
  internal virtual TabPage TabPage3 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button btnDeduceOpenETIS
  {
    get => this._btnDeduceOpenETIS;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button2_Click_1);
      Button btnDeduceOpenEtis1 = this._btnDeduceOpenETIS;
      if (btnDeduceOpenEtis1 != null)
        btnDeduceOpenEtis1.Click -= eventHandler;
      this._btnDeduceOpenETIS = value;
      Button btnDeduceOpenEtis2 = this._btnDeduceOpenETIS;
      if (btnDeduceOpenEtis2 == null)
        return;
      btnDeduceOpenEtis2.Click += eventHandler;
    }
  }

  internal virtual WebBrowser wbDeducer
  {
    get => this._wbDeducer;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      WebBrowserDocumentCompletedEventHandler completedEventHandler = new WebBrowserDocumentCompletedEventHandler(this.wbDeducer_DocumentCompleted);
      WebBrowserNavigatingEventHandler navigatingEventHandler = new WebBrowserNavigatingEventHandler(this.wbDeducer_Navigating);
      EventHandler eventHandler = new EventHandler(this.wbDeducer_FileDownload);
      WebBrowser wbDeducer1 = this._wbDeducer;
      if (wbDeducer1 != null)
      {
        wbDeducer1.DocumentCompleted -= completedEventHandler;
        wbDeducer1.Navigating -= navigatingEventHandler;
        wbDeducer1.FileDownload -= eventHandler;
      }
      this._wbDeducer = value;
      WebBrowser wbDeducer2 = this._wbDeducer;
      if (wbDeducer2 == null)
        return;
      wbDeducer2.DocumentCompleted += completedEventHandler;
      wbDeducer2.Navigating += navigatingEventHandler;
      wbDeducer2.FileDownload += eventHandler;
    }
  }

  internal virtual Button btnDeduceSaveInfo
  {
    get => this._btnDeduceSaveInfo;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button3_Click);
      Button btnDeduceSaveInfo1 = this._btnDeduceSaveInfo;
      if (btnDeduceSaveInfo1 != null)
        btnDeduceSaveInfo1.Click -= eventHandler;
      this._btnDeduceSaveInfo = value;
      Button btnDeduceSaveInfo2 = this._btnDeduceSaveInfo;
      if (btnDeduceSaveInfo2 == null)
        return;
      btnDeduceSaveInfo2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("chkDeduceDownloadAB")]
  internal virtual CheckBox chkDeduceDownloadAB { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual TabPage TabPage4
  {
    get => this._TabPage4;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.TabPage4_Click);
      TabPage tabPage4_1 = this._TabPage4;
      if (tabPage4_1 != null)
        tabPage4_1.Click -= eventHandler;
      this._TabPage4 = value;
      TabPage tabPage4_2 = this._TabPage4;
      if (tabPage4_2 == null)
        return;
      tabPage4_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("tbxDeduceReport")]
  internal virtual TextBox tbxDeduceReport { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button btnDeduceFigureIt
  {
    get => this._btnDeduceFigureIt;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.btnDeduceFigureIt_Click);
      Button btnDeduceFigureIt1 = this._btnDeduceFigureIt;
      if (btnDeduceFigureIt1 != null)
        btnDeduceFigureIt1.Click -= eventHandler;
      this._btnDeduceFigureIt = value;
      Button btnDeduceFigureIt2 = this._btnDeduceFigureIt;
      if (btnDeduceFigureIt2 == null)
        return;
      btnDeduceFigureIt2.Click += eventHandler;
    }
  }

  internal virtual Button btnDeduceLoadOptions
  {
    get => this._btnDeduceLoadOptions;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.btnDeduceLoadOptions_Click);
      Button deduceLoadOptions1 = this._btnDeduceLoadOptions;
      if (deduceLoadOptions1 != null)
        deduceLoadOptions1.Click -= eventHandler;
      this._btnDeduceLoadOptions = value;
      Button deduceLoadOptions2 = this._btnDeduceLoadOptions;
      if (deduceLoadOptions2 == null)
        return;
      deduceLoadOptions2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("lstDeduceFactoryOptions")]
  internal virtual ListBox lstDeduceFactoryOptions { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label9")]
  internal virtual Label Label9 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lstDeduceModels")]
  internal virtual CheckedListBox lstDeduceModels { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lstDeduceYears")]
  internal virtual CheckedListBox lstDeduceYears { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label11")]
  internal virtual Label Label11 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label10")]
  internal virtual Label Label10 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label12")]
  internal virtual Label Label12 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TextBox2")]
  internal virtual TextBox TextBox2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TextBox1")]
  internal virtual TextBox TextBox1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label13")]
  internal virtual Label Label13 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label14")]
  internal virtual Label Label14 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxConvertBin")]
  internal virtual TextBox tbxConvertBin { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxConvertHex")]
  internal virtual TextBox tbxConvertHex { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxChecksumBin")]
  internal virtual TextBox tbxChecksumBin { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label15")]
  internal virtual Label Label15 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button3
  {
    get => this._Button3;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button3_Click_1);
      Button button3_1 = this._Button3;
      if (button3_1 != null)
        button3_1.Click -= eventHandler;
      this._Button3 = value;
      Button button3_2 = this._Button3;
      if (button3_2 == null)
        return;
      button3_2.Click += eventHandler;
    }
  }

  internal virtual Button Button2
  {
    get => this._Button2;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button2_Click_2);
      Button button2_1 = this._Button2;
      if (button2_1 != null)
        button2_1.Click -= eventHandler;
      this._Button2 = value;
      Button button2_2 = this._Button2;
      if (button2_2 == null)
        return;
      button2_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label16")]
  internal virtual Label Label16 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual TabPage TabPage5
  {
    get => this._TabPage5;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.TabPage5_Click);
      TabPage tabPage5_1 = this._TabPage5;
      if (tabPage5_1 != null)
        tabPage5_1.Click -= eventHandler;
      this._TabPage5 = value;
      TabPage tabPage5_2 = this._TabPage5;
      if (tabPage5_2 == null)
        return;
      tabPage5_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label17")]
  internal virtual Label Label17 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TextBox3")]
  internal virtual TextBox TextBox3 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label18")]
  internal virtual Label Label18 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("tbxDeduceReport2")]
  internal virtual TextBox tbxDeduceReport2 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button4
  {
    get => this._Button4;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button4_Click);
      Button button4_1 = this._Button4;
      if (button4_1 != null)
        button4_1.Click -= eventHandler;
      this._Button4 = value;
      Button button4_2 = this._Button4;
      if (button4_2 == null)
        return;
      button4_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("ColumnHeader7")]
  internal virtual ColumnHeader ColumnHeader7 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader8")]
  internal virtual ColumnHeader ColumnHeader8 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader9")]
  internal virtual ColumnHeader ColumnHeader9 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button8
  {
    get => this._Button8;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button8_Click);
      Button button8_1 = this._Button8;
      if (button8_1 != null)
        button8_1.Click -= eventHandler;
      this._Button8 = value;
      Button button8_2 = this._Button8;
      if (button8_2 == null)
        return;
      button8_2.Click += eventHandler;
    }
  }

  internal virtual Button Button6
  {
    get => this._Button6;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button6_Click_1);
      Button button6_1 = this._Button6;
      if (button6_1 != null)
        button6_1.Click -= eventHandler;
      this._Button6 = value;
      Button button6_2 = this._Button6;
      if (button6_2 == null)
        return;
      button6_2.Click += eventHandler;
    }
  }

  internal virtual Button Button5
  {
    get => this._Button5;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button5_Click_1);
      Button button5_1 = this._Button5;
      if (button5_1 != null)
        button5_1.Click -= eventHandler;
      this._Button5 = value;
      Button button5_2 = this._Button5;
      if (button5_2 == null)
        return;
      button5_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("Label20")]
  internal virtual Label Label20 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label19")]
  internal virtual Label Label19 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lstBit_Years")]
  internal virtual CheckedListBox lstBit_Years { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lstBit_Models")]
  internal virtual CheckedListBox lstBit_Models { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lstBit_Modules")]
  internal virtual ListBox lstBit_Modules { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TextBox4")]
  internal virtual TextBox TextBox4 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label21")]
  internal virtual Label Label21 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("chkCompareShowChecksum")]
  internal virtual CheckBox chkCompareShowChecksum { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("chkDeduceDoCCC")]
  internal virtual CheckBox chkDeduceDoCCC { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button9
  {
    get => this._Button9;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button9_Click);
      Button button9_1 = this._Button9;
      if (button9_1 != null)
        button9_1.Click -= eventHandler;
      this._Button9 = value;
      Button button9_2 = this._Button9;
      if (button9_2 == null)
        return;
      button9_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("tbxCompFile4")]
  internal virtual TextBox tbxCompFile4 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label23")]
  internal virtual Label Label23 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual Button Button7
  {
    get => this._Button7;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button7_Click);
      Button button7_1 = this._Button7;
      if (button7_1 != null)
        button7_1.Click -= eventHandler;
      this._Button7 = value;
      Button button7_2 = this._Button7;
      if (button7_2 == null)
        return;
      button7_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("tbxCompFile3")]
  internal virtual TextBox tbxCompFile3 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("Label22")]
  internal virtual Label Label22 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TextBox5")]
  internal virtual TextBox TextBox5 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("TabPage6")]
  internal virtual TabPage TabPage6 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ListView lvwBrowser
  {
    get => this._lvwBrowser;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.lvwBrowser_SelectedIndexChanged);
      ColumnClickEventHandler clickEventHandler = new ColumnClickEventHandler(this.lvwBrowser_ColumnClick);
      ListView lvwBrowser1 = this._lvwBrowser;
      if (lvwBrowser1 != null)
      {
        lvwBrowser1.SelectedIndexChanged -= eventHandler;
        lvwBrowser1.ColumnClick -= clickEventHandler;
      }
      this._lvwBrowser = value;
      ListView lvwBrowser2 = this._lvwBrowser;
      if (lvwBrowser2 == null)
        return;
      lvwBrowser2.SelectedIndexChanged += eventHandler;
      lvwBrowser2.ColumnClick += clickEventHandler;
    }
  }

  internal virtual Button btnBrowseRefresh
  {
    get => this._btnBrowseRefresh;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button10_Click);
      Button btnBrowseRefresh1 = this._btnBrowseRefresh;
      if (btnBrowseRefresh1 != null)
        btnBrowseRefresh1.Click -= eventHandler;
      this._btnBrowseRefresh = value;
      Button btnBrowseRefresh2 = this._btnBrowseRefresh;
      if (btnBrowseRefresh2 == null)
        return;
      btnBrowseRefresh2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("ColumnHeader10")]
  internal virtual ColumnHeader ColumnHeader10 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader11")]
  internal virtual ColumnHeader ColumnHeader11 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader12")]
  internal virtual ColumnHeader ColumnHeader12 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader13")]
  internal virtual ColumnHeader ColumnHeader13 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("ColumnHeader14")]
  internal virtual ColumnHeader ColumnHeader14 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ContextMenuStrip ContextMenuStrip2
  {
    get => this._ContextMenuStrip2;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      CancelEventHandler cancelEventHandler = new CancelEventHandler(this.ContextMenuStrip2_Opening);
      ContextMenuStrip contextMenuStrip2_1 = this._ContextMenuStrip2;
      if (contextMenuStrip2_1 != null)
        contextMenuStrip2_1.Opening -= cancelEventHandler;
      this._ContextMenuStrip2 = value;
      ContextMenuStrip contextMenuStrip2_2 = this._ContextMenuStrip2;
      if (contextMenuStrip2_2 == null)
        return;
      contextMenuStrip2_2.Opening += cancelEventHandler;
    }
  }

  internal virtual ToolStripMenuItem SetAsCompare1ToolStripMenuItem
  {
    get => this._SetAsCompare1ToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.SetAsCompare1ToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._SetAsCompare1ToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._SetAsCompare1ToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._SetAsCompare1ToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem SetAsCompare2ToolStripMenuItem
  {
    get => this._SetAsCompare2ToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.SetAsCompare2ToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._SetAsCompare2ToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._SetAsCompare2ToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._SetAsCompare2ToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem SetAsCompare3ToolStripMenuItem
  {
    get => this._SetAsCompare3ToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.SetAsCompare3ToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._SetAsCompare3ToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._SetAsCompare3ToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._SetAsCompare3ToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem SetAsCompare4ToolStripMenuItem
  {
    get => this._SetAsCompare4ToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.SetAsCompare4ToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._SetAsCompare4ToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._SetAsCompare4ToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._SetAsCompare4ToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("ToolStripMenuItem1")]
  internal virtual ToolStripSeparator ToolStripMenuItem1 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ToolStripMenuItem DeleteFileToolStripMenuItem
  {
    get => this._DeleteFileToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.DeleteFileToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._DeleteFileToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._DeleteFileToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._DeleteFileToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual PictureBox PictureBox1
  {
    get => this._PictureBox1;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.PictureBox1_Click);
      PictureBox pictureBox1_1 = this._PictureBox1;
      if (pictureBox1_1 != null)
        pictureBox1_1.Click -= eventHandler;
      this._PictureBox1 = value;
      PictureBox pictureBox1_2 = this._PictureBox1;
      if (pictureBox1_2 == null)
        return;
      pictureBox1_2.Click += eventHandler;
    }
  }

  [field: AccessedThroughProperty("lblComp4VIN")]
  internal virtual Label lblComp4VIN { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lblComp3VIN")]
  internal virtual Label lblComp3VIN { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lblComp2VIN")]
  internal virtual Label lblComp2VIN { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("lblComp1VIN")]
  internal virtual Label lblComp1VIN { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  [field: AccessedThroughProperty("chkCompareShowNames")]
  internal virtual CheckBox chkCompareShowNames { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

  internal virtual ToolStripMenuItem ExportModuleToolStripMenuItem
  {
    get => this._ExportModuleToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler1 = new EventHandler(this.ExportModuleToolStripMenuItem_Click);
      EventHandler eventHandler2 = new EventHandler(this.ExportModuleToolStripMenuItem_DropDownOpening);
      ToolStripMenuItem toolStripMenuItem1 = this._ExportModuleToolStripMenuItem;
      if (toolStripMenuItem1 != null)
      {
        toolStripMenuItem1.Click -= eventHandler1;
        toolStripMenuItem1.DropDownOpening -= eventHandler2;
      }
      this._ExportModuleToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._ExportModuleToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler1;
      toolStripMenuItem2.DropDownOpening += eventHandler2;
    }
  }

  internal virtual ToolStripMenuItem ToUCDSToolStripMenuItem
  {
    get => this._ToUCDSToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.ToUCDSToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._ToUCDSToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._ToUCDSToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._ToUCDSToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual ToolStripMenuItem ToABTToolStripMenuItem
  {
    get => this._ToABTToolStripMenuItem;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.ToABTToolStripMenuItem_Click);
      ToolStripMenuItem toolStripMenuItem1 = this._ToABTToolStripMenuItem;
      if (toolStripMenuItem1 != null)
        toolStripMenuItem1.Click -= eventHandler;
      this._ToABTToolStripMenuItem = value;
      ToolStripMenuItem toolStripMenuItem2 = this._ToABTToolStripMenuItem;
      if (toolStripMenuItem2 == null)
        return;
      toolStripMenuItem2.Click += eventHandler;
    }
  }

  internal virtual Button Button10
  {
    get => this._Button10;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.Button10_Click_1);
      Button button10_1 = this._Button10;
      if (button10_1 != null)
        button10_1.Click -= eventHandler;
      this._Button10 = value;
      Button button10_2 = this._Button10;
      if (button10_2 == null)
        return;
      button10_2.Click += eventHandler;
    }
  }

  internal virtual CheckBox chkShowOnlyMismatches
  {
    get => this._chkShowOnlyMismatches;
    [MethodImpl(MethodImplOptions.Synchronized)] set
    {
      EventHandler eventHandler = new EventHandler(this.CheckBox1_CheckedChanged);
      CheckBox showOnlyMismatches1 = this._chkShowOnlyMismatches;
      if (showOnlyMismatches1 != null)
        showOnlyMismatches1.CheckedChanged -= eventHandler;
      this._chkShowOnlyMismatches = value;
      CheckBox showOnlyMismatches2 = this._chkShowOnlyMismatches;
      if (showOnlyMismatches2 == null)
        return;
      showOnlyMismatches2.CheckedChanged += eventHandler;
    }
  }

  [field: AccessedThroughProperty("TextBox6")]
  internal virtual TextBox TextBox6 { get; [MethodImpl(MethodImplOptions.Synchronized)] set; }

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
}
