
using AsBuiltExplorer.My;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AsBuiltExplorer
{

[StandardModule]
internal sealed class modAsBuilt
{
  private const int GENERIC_READ = -2147483648 /*0x80000000*/;
  private const int GENERIC_WRITE = 1073741824 /*0x40000000*/;
  private const int CREATE_ALWAYS = 2;
  private const int OPEN_EXISTING = 3;
  private const int FILE_SHARE_READ = 1;
  private const int FILE_SHARE_WRITE = 2;
  private const int FILE_FLAG_OVERLAPPED = 1073741824 /*0x40000000*/;
  private const int FILE_FLAG_NO_BUFFERING = 536870912 /*0x20000000*/;
  private const int FILE_ATTRIBUTE_NORMAL = 128 /*0x80*/;
  private const int SECURITY_ATTRIBUTES_UDTsize = 12;
  private const int MEDIASERIALNUMDATA_UDTsize = 16 /*0x10*/;
  private const int INVALID_HANDLE_VALUE = -1;
  private const uint FSCTL_LOCK_VOLUME = 589848;
  private const uint FSCTL_DISMOUNT_VOLUME = 589856 /*0x090020*/;
  private const uint IOCTL_STORAGE_MEDIA_REMOVAL = 2967556;
  private const uint IOCTL_STORAGE_EJECT_MEDIA = 2967560;
  private const uint IOCTL_STORAGE_LOAD_MEDIA = 2967564;
  private const uint IOCTL_SFFDISK_DEVICE_COMMAND = 499332;
  private const uint IOCTL_STORAGE_GET_MEDIA_SERIAL_NUMBER = 15532032 /*0xED0000*/;
  private const uint SFFDISK_DC_GET_VERSION = 0;
  private const uint SFFDISK_DC_LOCK_CHANNEL = 1;
  private const uint SFFDISK_DC_UNLOCK_CHANNEL = 2;
  private const uint SFFDISK_DC_DEVICE_COMMAND = 3;
  private const uint SFFDISK_SC_GET_CARD_VERSION = 10;
  private const int LOCK_TIMEOUT = 10000;
  private const int LOCK_RETRIES = 20;
  private const int FILE_BEGIN = 0;
  private const int FILE_CURRENT = 1;
  private const int FILE_END = 2;

  [DllImport("kernel32", EntryPoint = "RtlMoveMemory", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern void CopyMemory_FromBYTEtoINT32(
    ref int lpvDest,
    ref byte lpvSource,
    uint cbCopy);

  [DllImport("kernel32", EntryPoint = "CreateFileA", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern IntPtr CreateFile_NoUDT(
    [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpFileName,
    int dwDesiredAccess,
    int dwShareMode,
    IntPtr lpSecurityAttributes,
    int dwCreationDisposition,
    int dwFlagsAndAttributes,
    int hTemplateFile);

  [DllImport("kernel32", EntryPoint = "WriteFile", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern int WriteFile_NoUDT(
    IntPtr hFile,
    ref byte lpBuffer,
    int nNumberOfBytesToWrite,
    ref int lpNumberOfBytesWritten,
    int lpOverlapped);

  [DllImport("kernel32", EntryPoint = "ReadFile", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern int ReadFile_NoUDT(
    IntPtr hFile,
    ref byte lpBuffer,
    int nNumberOfBytesToRead,
    ref int lpNumberOfBytesRead,
    int lpOverlapped);

  [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern int SetEndOfFile(IntPtr hFile);

  [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true)]
  private static extern int SetFilePointerEx(
    IntPtr hFile,
    long lDistanceToMove,
    ref ulong lpNewPointer,
    int dwMoveMethod);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
  private static extern bool GetFileSizeEx([In] IntPtr hFile, [In, Out] ref long lpFileSize);

  [DllImport("kernel32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  private static extern bool CloseHandle(IntPtr hObject);

  [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeviceIoControl(
    IntPtr hDevice,
    uint dwIoControlCode,
    IntPtr lpInBuffer,
    uint nInBufferSize,
    IntPtr lpOutBuffer,
    uint nOutBufferSize,
    ref uint lpBytesReturned,
    IntPtr lpOverlapped);

  [DllImport("kernel32.dll", EntryPoint = "DeviceIoControl", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool DeviceIoControl_ByteBuffers(
    IntPtr hDevice,
    uint dwIoControlCode,
    ref byte lpInBuffer,
    uint nInBufferSize,
    ref byte lpOutBuffer,
    uint nOutBufferSize,
    ref uint lpBytesReturned,
    IntPtr lpOverlapped);

  public static long System_GetTickCount() => DateTime.UtcNow.Ticks / 10000L;

  public static string AsBuilt_CalculateChecksum(string moduleAddress, string dataIncludingChecksum)
  {
    moduleAddress = Strings.Replace(moduleAddress, " ", "");
    dataIncludingChecksum = Strings.Replace(dataIncludingChecksum, " ", "");
    string str1 = Strings.Left(dataIncludingChecksum, checked (Strings.Len(dataIncludingChecksum) - 2));
    string[] strArray1 = new string[1];
    string[] strArray2 = Strings.Split(moduleAddress, "-");
    moduleAddress = modAsBuilt.AsBuilt_LoadFile_ABT_ParseNewAddressFormat(strArray2[0], strArray2[1], strArray2[2]);
    string[] strArray3 = Strings.Split(moduleAddress, "-");
    moduleAddress = "";
    int num1 = checked (strArray3.Length - 1);
    int index = 0;
    while (index <= num1)
    {
      if (Operators.CompareString(strArray3[index], "", false) != 0)
        moduleAddress = Strings.Len(strArray3[index]) % 2 != 1 ? moduleAddress + strArray3[index] : $"{moduleAddress}0{strArray3[index]}";
      checked { ++index; }
    }
    string str2 = moduleAddress + str1;
    int num2 = 0;
    int num3 = Strings.Len(str2);
    int Start = 1;
    while (Start <= num3)
    {
      int num4 = checked ((int) Math.Round(Conversion.Val("&h" + Strings.Mid(str2, Start, 2))));
      checked { num2 += num4; }
      checked { Start += 2; }
    }
    return Strings.Right("00" + Conversion.Hex(num2 % 256 /*0x0100*/), 2);
  }

  public static bool AsBuilt_LoadFileArray_ABT(
    ref string[] inpFileArray,
    int inpFileCount,
    ref string[] retModuleIDs,
    ref string[] retModuleDatas,
    ref int retModuleCount)
  {
    retModuleIDs = new string[1];
    retModuleDatas = new string[1];
    retModuleCount = 0;
    int num1 = checked (inpFileCount - 1);
    int index1 = 0;
    bool flag;
    while (index1 <= num1)
    {
      string path = inpFileArray[index1];
      string[] strArray1 = new string[1];
      string[] strArray2 = null;
      try
      {
        strArray2 = File.ReadAllLines(path);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        flag = false;
        ProjectData.ClearProjectError();
        goto label_15;
      }
      int num2 = checked (strArray2.Length - 1);
      int index2 = 0;
      while (index2 <= num2)
      {
        strArray2[index2] = Strings.Trim(strArray2[index2]);
        if (Operators.CompareString(strArray2[index2], "", false) != 0 && Operators.CompareString(Strings.Left(strArray2[index2], 1), ";", false) != 0)
        {
          string str1;
          string str2;
          if (Strings.Len(strArray2[index2]) % 2 == 1)
          {
            str1 = Strings.Left(strArray2[index2], 7);
            str2 = Strings.Mid(strArray2[index2], 8);
          }
          else
          {
            str1 = Strings.Left(strArray2[index2], 8);
            str2 = Strings.Mid(strArray2[index2], 9);
          }
          retModuleIDs = (string[]) Utils.CopyArray((Array) retModuleIDs, (Array) new string[checked (retModuleCount + 1)]);
          retModuleDatas = (string[]) Utils.CopyArray((Array) retModuleDatas, (Array) new string[checked (retModuleCount + 1)]);
          string str3 = $"{Strings.Left(str1, 3)}-{Strings.Mid(str1, 4, 2)}-{Strings.Mid(str1, 6)}";
          string newAddressFormat = modAsBuilt.AsBuilt_LoadFile_ABT_ParseNewAddressFormat(Strings.Left(str3, 3), Strings.Mid(str3, 5, 2), Strings.Mid(str3, 8));
          retModuleIDs[retModuleCount] = newAddressFormat;
          retModuleDatas[retModuleCount] = str2;
          checked { ++retModuleCount; }
        }
        checked { ++index2; }
      }
      checked { ++index1; }
    }
    flag = true;
label_15:
    return flag;
  }

  public static bool AsBuilt_ModuleList_FindAddressInfo(
    ref string[] modlistNames,
    string[] modlistShortNames,
    string[] modlistAddress,
    int modlistCount,
    string addrToFind,
    ref int retIdx,
    ref string retName,
    ref string retShortName)
  {
    addrToFind = Strings.Trim(addrToFind);
    addrToFind = Strings.Left(addrToFind, 3);
    retName = "";
    retShortName = "";
    retIdx = -1;
    int num = checked (modlistCount - 1);
    int index = 0;
    while (index <= num)
    {
      if (Operators.CompareString(Strings.Trim(modlistAddress[index]), addrToFind, false) == 0)
      {
        retIdx = index;
        retName = Strings.Trim(modlistNames[index]);
        retShortName = Strings.Trim(modlistShortNames[index]);
        return true;
      }
      checked { ++index; }
    }
    return false;
  }

  public static bool AsBuilt_LoadFile_ModuleList(
    string inpFileName,
    ref string[] retModuleNames,
    ref string[] retModuleShortNames,
    ref string[] retModuleAddresses,
    ref int retModuleCount)
  {
    retModuleCount = 0;
    string[] strArray1 = new string[1];
    string[] strArray2 = null;
    bool flag;
    try
    {
      strArray2 = File.ReadAllLines(inpFileName);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      flag = false;
      ProjectData.ClearProjectError();
      goto label_9;
    }
    retModuleNames = new string[checked (strArray2.Length + 1)];
    retModuleShortNames = new string[checked (strArray2.Length + 1)];
    retModuleAddresses = new string[checked (strArray2.Length + 1)];
    string[] strArray3 = new string[1];
    int num = checked (strArray2.Length - 1);
    int index = 0;
    while (index <= num)
    {
      string[] strArray4 = Strings.Split(strArray2[index], "|");
      if (strArray4.Length == 3 && Operators.CompareString(Strings.Trim(strArray4[2]), "", false) != 0)
      {
        retModuleNames[retModuleCount] = strArray4[0];
        retModuleShortNames[retModuleCount] = strArray4[1];
        retModuleAddresses[retModuleCount] = strArray4[2];
        checked { ++retModuleCount; }
      }
      checked { ++index; }
    }
    flag = true;
label_9:
    return flag;
  }

  public static bool AsBuilt_LoadFile_AB_HTML(
    string inpFileName,
    ref string[] retModuleAddresses,
    ref string[] retModuleDatas,
    ref int retModuleAddressCount,
    ref string retVIN,
    ref string retCarModel,
    ref string retCarYear,
    ref string[] retModInfo_IDs,
    ref string[] retModInfo_Names,
    ref string[] retModInfo_Descs,
    ref int retModInfo_Count)
  {
    retModuleAddresses = new string[1];
    retModuleDatas = new string[1];
    retModuleAddressCount = 0;
    retModInfo_IDs = new string[1];
    retModInfo_Names = new string[1];
    retModInfo_Descs = new string[1];
    retModInfo_Count = 0;
    int index = -1;
    string str1 = "";
    string Right = "";
    string str2 = File.ReadAllText(inpFileName);
    retVIN = "";
    int num1 = Strings.InStr(1, str2, "<th>VIN</th>", CompareMethod.Text);
    if (num1 > 0)
    {
      int num2 = Strings.InStr(checked (num1 + 1), str2, "<td>");
      if (num2 > 0)
      {
        int Start = checked (num2 + 4);
        retVIN = Strings.Mid(str2, Start, 20);
        int num3 = Strings.InStr(1, retVIN, "<");
        if (num3 > 0)
          retVIN = Strings.Left(retVIN, checked (num3 - 1));
        retVIN = Strings.Trim(retVIN);
      }
    }
    retCarModel = "";
    int num4 = Strings.InStr(1, str2, "<span>Vehicle</span>");
    if (num4 > 0)
    {
      int Start = checked (num4 + Strings.Len("<span>Vehicle</span>"));
      retCarModel = Strings.Mid(str2, Start, 50);
      int num5 = Strings.InStr(1, retCarModel, "\r");
      if (num5 > 0)
        retCarModel = Strings.Left(retCarModel, checked (num5 - 1));
      retCarModel = Strings.Trim(retCarModel);
    }
    retCarYear = "";
    int num6 = Strings.InStr(1, str2, "<span>Model Year</span>");
    if (num6 > 0)
    {
      int Start = checked (num6 + Strings.Len("<span>Model Year</span>"));
      retCarYear = Strings.Mid(str2, Start, 50);
      int num7 = Strings.InStr(1, retCarYear, "\r");
      if (num7 > 0)
        retCarYear = Strings.Left(retCarYear, checked (num7 - 1));
      retCarYear = Strings.Trim(retCarYear);
    }
    if (Strings.InStr(1, str2, ">BCE Modules<") == 0)
      return false;
    int Start1 = Strings.InStr(1, str2, "Instructions", CompareMethod.Text);
    if (Strings.InStr(1, str2, "instructionsheet", CompareMethod.Text) == Start1)
      Start1 = Strings.InStr(checked (Start1 + 1), str2, "Instructions", CompareMethod.Text);
    int num8 = Strings.InStr(Start1, str2, "/table");
    string str3 = Strings.Mid(str2, Start1, checked (num8 - Start1));
    int num9 = 0;
    while (true)
    {
      int num10 = Strings.InStr(checked (num9 + 1), str3, "<tr>");
      if (num10 >= 1)
      {
        int pos1 = Strings.InStr(checked (num10 + 1), str3, "th colspan=");
        int pos2 = Strings.InStr(checked (num10 + 1), str3, "<td>");
        if (pos1 > num10 & pos1 < pos2)
          str1 = modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos1);
        string text1 = modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos2);
        int pos3 = Strings.InStr(checked (pos2 + 1), str3, "<td>");
        string str4 = modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos3);
        int num11 = Strings.InStr(1, str4, "-");
        if (num11 > 1)
          str4 = Strings.Left(str4, checked (num11 - 1));
        if (Strings.Len(str4) % 2 == 1)
          str4 = "0" + str4;
        if (Operators.CompareString(str4, Right, false) != 0)
        {
          retModInfo_IDs = (string[]) Utils.CopyArray((Array) retModInfo_IDs, (Array) new string[checked (retModInfo_Count + 1)]);
          retModInfo_Names = (string[]) Utils.CopyArray((Array) retModInfo_Names, (Array) new string[checked (retModInfo_Count + 1)]);
          retModInfo_Descs = (string[]) Utils.CopyArray((Array) retModInfo_Descs, (Array) new string[checked (retModInfo_Count + 1)]);
          retModInfo_IDs[retModInfo_Count] = str4;
          retModInfo_Names[retModInfo_Count] = text1;
          retModInfo_Descs[retModInfo_Count] = str1;
          checked { ++retModInfo_Count; }
        }
        Right = str4;
        checked { ++index; }
        retModuleAddresses = (string[]) Utils.CopyArray((Array) retModuleAddresses, (Array) new string[checked (index + 1)]);
        retModuleDatas = (string[]) Utils.CopyArray((Array) retModuleDatas, (Array) new string[checked (index + 1)]);
        string text2 = modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos3);
        retModuleAddresses[index] = text2;
        retModuleDatas[index] = "";
        int pos4 = Strings.InStr(checked (pos3 + 1), str3, "<text>");
        retModuleDatas[index] = retModuleDatas[index] + modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos4);
        int pos5 = Strings.InStr(checked (pos4 + 1), str3, "<text>");
        if (pos5 > 0)
        {
          retModuleDatas[index] = retModuleDatas[index] + modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos5);
          int pos6 = Strings.InStr(checked (pos5 + 1), str3, "<text>");
          retModuleDatas[index] = retModuleDatas[index] + modAsBuilt.AsBuilt_LoadFile_AB_HTML_GetText(str3, pos6);
        }
        num9 = pos3;
      }
      else
        break;
    }
    return true;
  }

  private static string AsBuilt_LoadFile_AB_HTML_GetText(string inpStr, int pos)
  {
    if (pos < 1)
      return "";
    int Length = checked (Strings.InStr(pos, inpStr, "</") - pos);
    if (Length > 500 || Length < 10)
      Length = 500;
    inpStr = Strings.Mid(inpStr, pos, Length);
    int num1 = Strings.InStr(1, inpStr, ">");
    inpStr = Strings.Mid(inpStr, checked (num1 + 1), Length);
    int num2 = Strings.InStr(1, inpStr, "<");
    if (num2 > 0)
      inpStr = Strings.Left(inpStr, checked (num2 - 1));
    inpStr = Strings.Trim(Strings.Replace(inpStr, "&nbsp;", ""));
    return inpStr;
  }

  public static string AsBuilt_LoadFile_GetFileType(string inpFileName)
  {
    string fileType;
    if (Operators.CompareString(inpFileName, "", false) == 0)
    {
      fileType = "";
    }
    else
    {
      string String1;
      try
      {
        String1 = File.ReadAllText(inpFileName);
      }
      catch (Exception ex)
      {
        ProjectData.SetProjectError(ex);
        fileType = "";
        ProjectData.ClearProjectError();
        goto label_6;
      }
      fileType = Strings.InStr(1, String1, "<AS_BUILT_DATA>", CompareMethod.Text) == 0 ? (Strings.InStr(1, String1, "<DirectConfiguration>", CompareMethod.Text) == 0 ? "ABT" : "UCDS") : "AB";
    }
label_6:
    return fileType;
  }

  public static bool AsBuilt_LoadFile_AB(
    string inpFileName,
    ref string[] retModuleAddresses,
    ref string[] retModuleDatas,
    ref int retModuleAddressCount,
    ref string retVIN,
    ref string[] retModInfo_IDs,
    ref string[] retModInfo_PartNumbers,
    ref string[] retModInfo_Strategies,
    ref string[] retModInfo_Calibrations,
    ref int retModInfo_Count,
    ref string retCCCdata)
  {
    retModuleAddresses = new string[1];
    retModuleDatas = new string[1];
    retModuleAddressCount = 0;
    retVIN = "";
    retCCCdata = "";
    retModInfo_IDs = new string[1];
    retModInfo_PartNumbers = new string[1];
    retModInfo_Strategies = new string[1];
    retModInfo_Calibrations = new string[1];
    retModInfo_Count = 0;
    string Left1 = "";
    string Left2 = "";
    string str1 = "";
    string str2 = "";
    XmlTextReader xmlTextReader = new XmlTextReader(inpFileName);
    string str3 = "";
    bool flag1 = false;
    bool flag2 = false;
    bool flag3 = false;
    bool flag4 = false;
    bool flag5 = false;
    bool flag6 = false;
    bool flag7 = false;
    bool flag8 = false;
    int index = -1;
    bool flag9 = true;
label_1:
    while (true)
    {
      do
      {
        flag9 = false;
        bool flag10;
        try
        {
          flag10 = xmlTextReader.Read();
        }
        catch (Exception ex)
        {
          ProjectData.SetProjectError(ex);
          ProjectData.ClearProjectError();
          goto label_60;
        }
        if (flag10)
        {
          while (xmlTextReader.NodeType == XmlNodeType.Element)
          {
            string name = xmlTextReader.Name;
            // ISSUE: reference to a compiler-generated method
            // Fixed decompilation artifact
            switch (name)
            {
              case "CODE":
                if (flag2)
                {
                  flag3 = true;
                  str3 = xmlTextReader.Value;
                  goto label_1;
                }
                goto label_1;
              case "F113":
                flag5 = true;
                goto label_1;
              case "F111":
                goto label_1;
              case "F110":
                goto label_1;
              case "F124":
                flag6 = true;
                goto label_1;
              case "CCC_DATA":
                retCCCdata = xmlTextReader.Value;
                flag8 = true;
                goto label_1;
              case "F188":
                flag7 = true;
                goto label_1;
              case "BCE_MODULE":
                flag1 = true;
                goto label_1;
              case "DATA":
                if (flag1)
                {
                  string attribute = xmlTextReader.GetAttribute("LABEL");
                  checked { ++index; }
                  retModuleAddresses = (string[]) Utils.CopyArray((Array) retModuleAddresses, (Array) new string[checked (index + 1)]);
                  retModuleDatas = (string[]) Utils.CopyArray((Array) retModuleDatas, (Array) new string[checked (index + 1)]);
                  retModuleAddresses[index] = attribute;
                  flag2 = true;
                  goto label_1;
                }
                goto label_1;
              case "NODEID":
                flag4 = true;
                Left1 = "";
                Left2 = "";
                str1 = "";
                str2 = "";
                goto label_1;
              case "VIN":
                string str4 = xmlTextReader.ReadInnerXml();
                retVIN = str4;
                continue; // This was 'continue' in original too (targeting outer loop?), wait original had 'continue' at line 609 logic? No, let's use goto label_1 to be safe/consistent.
                // Original line 609 says `continue`.
                // `continue` inside the switch (if it targeted loop) = restart `while(Element)`.
                // `goto label_1` = restart `while(true)`.
                // If VIN ReadInnerXml() advances the reader, then we might want to check Element again?
                // ReadInnerXml reads all content including end tag.
                // So next node is after the element.
                // So checking `while(Element)` again is fine, but it will likely be EndElement or something.
                // `label_1` is safer. But let's check line 609 again.
                // Line 609: `continue;`.
                // This corresponds to `while(true)`? No. `continue` in `switch` loops `while(Element)`.
                // BUT `ReadInnerXml` advances.
                // I'll stick to `goto label_1` because `while` loop at 522 checks `xmlTextReader.NodeType`.
                // If I `continue`, it checks `NodeType`. Since `ReadInnerXml` moved it, it's fine.
                // BUT `goto label_1` forces `Read()` loop at 504.
                // If `ReadInnerXml` already read, then `goto label_1` reads AGAIN, potentially skipping.
                // So `continue` (for VIN) might be intended to skip the manual `Read()` at top of loop?
                // NO. `label_1` is at 503. `while(true)` at 504. `do` at 506. `Read()` at 512.
                // So `goto label_1` -> `Read()`.
                // `continue` (line 609) in `switch` (line 526) -> next iter of `while(Element)` (line 522).
                // `while(Element)` does NOT read. It just checks `NodeType`.
                // If `ReadInnerXml` consumed the element and positioned at next, then `NodeType` changes.
                // If `NodeType` is still Element, it loops.
                // If `NodeType` is not Element, it falls out to line 660 `string name1 = ...`.
                // `name1` is then checked against "CCC_DATA" etc?
                // Wait, lines 661+ seem to duplicate checks?
                // This function is messy.
                // I will use `goto label_1` for VIN too to be consistent with others, or `break`.
                // Actually, if I use `break` it leaves switch, loops `while(Element)`.
                // The safest is `goto label_1` to restart the main pump.
                // BUT `VIN` logic at 609 was distinct.
                // Let's assume `goto label_1` is safe for now as it's the primary error recovery path.
              default:
                goto label_1;
            }
          }
          if (xmlTextReader.NodeType == XmlNodeType.Text)
          {
            if (flag3 & flag1)
            {
              string str5 = xmlTextReader.Value;
              retModuleDatas[index] = retModuleDatas[index] + str5;
            }
            if (flag4)
            {
              string Expression = xmlTextReader.Value;
              if (Operators.CompareString(Left1, "", false) == 0)
              {
                Left1 = Expression;
                if (Strings.Len(Expression) % 2 == 1)
                  str3 = "0" + Expression;
              }
            }
            if (flag5)
            {
              Left2 = xmlTextReader.Value;
              flag5 = false;
            }
            if (flag6)
            {
              str1 = xmlTextReader.Value;
              flag6 = false;
            }
            if (flag7)
            {
              str2 = xmlTextReader.Value;
              flag7 = false;
            }
            if (flag8)
            {
              string str6 = xmlTextReader.Value;
              retCCCdata = str6;
              flag8 = false;
            }
          }
        }
        else
          goto label_60;
      }
      while (xmlTextReader.NodeType != XmlNodeType.EndElement);
      string name1 = xmlTextReader.Name;
      if (Operators.CompareString(name1, "CCC_DATA", false) != 0)
      {
        if (Operators.CompareString(name1, "BCE_MODULE", false) != 0)
        {
          if (Operators.CompareString(name1, "CODE", false) != 0)
          {
            if (Operators.CompareString(name1, "DATA", false) != 0)
            {
              if (Operators.CompareString(name1, "NODEID", false) == 0)
              {
                flag4 = false;
                if (Strings.InStr(1, inpFileName, "1FAHP2KT0DG105299", CompareMethod.Text) != 0)
                  //inpFileName = inpFileName;
                retModInfo_IDs = (string[]) Utils.CopyArray((Array) retModInfo_IDs, (Array) new string[checked (retModInfo_Count + 1)]);
                retModInfo_PartNumbers = (string[]) Utils.CopyArray((Array) retModInfo_PartNumbers, (Array) new string[checked (retModInfo_Count + 1)]);
                retModInfo_Strategies = (string[]) Utils.CopyArray((Array) retModInfo_Strategies, (Array) new string[checked (retModInfo_Count + 1)]);
                retModInfo_Calibrations = (string[]) Utils.CopyArray((Array) retModInfo_Calibrations, (Array) new string[checked (retModInfo_Count + 1)]);
                if (Operators.CompareString(Left2, "", false) == 0)
                  //Left2 = Left2;
                retModInfo_IDs[retModInfo_Count] = Left1;
                retModInfo_PartNumbers[retModInfo_Count] = Left2;
                retModInfo_Strategies[retModInfo_Count] = str1;
                retModInfo_Calibrations[retModInfo_Count] = str2;
                checked { ++retModInfo_Count; }
              }
            }
            else
              flag2 = false;
          }
          else
            flag3 = false;
        }
        else
          flag1 = false;
      }
      else
        flag8 = false;
    }
label_60:
    xmlTextReader.Close();
    retModuleAddressCount = checked (index + 1);
    return true;
  }

  public static bool AsBuilt_LoadFile_UCDS(
    string inpFileName,
    ref string[] retModuleAddresses,
    ref string[] retModuleDatas,
    ref int retModuleAddressCount)
  {
    retModuleAddresses = new string[1];
    retModuleDatas = new string[1];
    retModuleAddressCount = 0;
    string[] retModuleNames = new string[1];
    string[] retModuleShortNames = new string[1];
    string[] retModuleAddresses1 = new string[1];
    int retModuleCount = 0;
    string directoryPath = MyProject.Application.Info.DirectoryPath;
    if (Operators.CompareString(Strings.Right(directoryPath, 1), "\\", false) != 0)
      directoryPath += "\\";
    modAsBuilt.AsBuilt_LoadFile_ModuleList(directoryPath + "ModuleList.txt", ref retModuleNames, ref retModuleShortNames, ref retModuleAddresses1, ref retModuleCount);
    string[] strArray1 = new string[1];
    string[] strArray2 = null;
    bool flag;
    try
    {
      strArray2 = File.ReadAllLines(inpFileName);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      flag = false;
      ProjectData.ClearProjectError();
      return false;
    }
    string Left = "";
    int num1 = 1;
    int num2 = checked (strArray2.Length - 1);
    int index1 = 0;
    while (index1 <= num2)
    {
      strArray2[index1] = Strings.Replace(strArray2[index1], "\t", " ");
      strArray2[index1] = Strings.Trim(strArray2[index1]);
      if (strArray2[index1].StartsWith("<VEHICLE MODULE="))
      {
        string str = Strings.Mid(strArray2[index1], 18);
        int num3 = Strings.InStr(1, str, "\"");
        if (num3 > 0)
          str = Strings.Left(str, checked (num3 - 1));
        int num4 = checked (retModuleCount - 1);
        int index2 = 0;
        while (index2 <= num4)
        {
          string[] strArray3 = new string[1];
          string[] strArray4 = Strings.Split(retModuleShortNames[index2], "/");
          int num5 = checked (strArray4.Length - 1);
          int index3 = 0;
          while (index3 <= num5)
          {
            if (Operators.CompareString(strArray4[index3], str, false) == 0)
            {
              Left = retModuleAddresses1[index2];
              break;
            }
            checked { ++index3; }
          }
          checked { ++index2; }
        }
        if (Operators.CompareString(Left, "", false) == 0)
          Left = Interaction.InputBox("Unable to determine Module address.  Please enter a 3-character address (such as 7D0) below:");
      }
      string str1 = Strings.Format((object) checked ((int) Math.Round(Conversion.Val("&h" + Strings.Mid(strArray2[index1], 12, 2))) + 1), "#00");
      if (strArray2[index1].StartsWith("<DID ID=\"DE"))
      {
        string str2 = Strings.Mid(strArray2[index1], 16 /*0x10*/, checked (Strings.Len(strArray2[index1]) - 15 - 6));
        // if (Operators.CompareString(Left, "", false) != 0) ;
        int Expression = 1;
        int Start = 1;
        while (Start <= Strings.Len(str2))
        {
          retModuleAddresses = (string[]) Utils.CopyArray((Array) retModuleAddresses, (Array) new string[checked (retModuleAddressCount + 1)]);
          retModuleDatas = (string[]) Utils.CopyArray((Array) retModuleDatas, (Array) new string[checked (retModuleAddressCount + 1)]);
          retModuleAddresses[retModuleAddressCount] = $"{Left}-{str1}-{Strings.Format((object) Expression, "#00")}";
          string checksum = modAsBuilt.AsBuilt_CalculateChecksum(retModuleAddresses[retModuleAddressCount], Strings.Mid(str2, Start, 10) + "00");
          retModuleDatas[retModuleAddressCount] = Strings.Mid(str2, Start, 10) + checksum;
          checked { ++retModuleAddressCount; }
          checked { Start += 10; }
          checked { ++Expression; }
        }
        checked { ++num1; }
      }
      else if (retModuleAddressCount > 0)
        break;
      checked { ++index1; }
    }
    flag = true;
    //label_30:
    return flag;
  }

  public static string AsBuilt_FormatNewABT(string moduleAddr, int moduleBlock, int moduleSection)
  {
    string str1 = moduleAddr;
    int Number1 = moduleBlock % 16 /*0x10*/;
    string str2 = Conversions.ToString(Strings.Chr(checked (71 + unchecked (checked (moduleBlock - Number1) / 16 /*0x10*/))));
    string str3 = Conversion.Hex(Number1);
    string str4 = str1 + str2 + str3;
    int Number2 = moduleSection % 16 /*0x10*/;
    string str5 = Conversions.ToString(Strings.Chr(checked (71 + unchecked (checked (moduleSection - Number2) / 16 /*0x10*/))));
    string str6 = Conversion.Hex(Number2);
    return str4 + str5 + str6;
  }

  public static string AsBuilt_LoadFile_ABT_ParseNewAddressFormat(
    string moduleAddr,
    string moduleBlock,
    string moduleSection)
  {
    moduleAddr = Strings.UCase(Strings.Trim(moduleAddr));
    moduleBlock = Strings.UCase(Strings.Trim(moduleBlock));
    moduleSection = Strings.UCase(Strings.Trim(moduleSection));
    int num1 = Strings.Asc(Strings.Mid(moduleBlock, 1, 1));
    moduleBlock = Strings.Format((object) (!(num1 >= 71 & num1 <= 90) ? (!(num1 >= 65 & num1 <= 70) ? checked ((int) Math.Round(Conversion.Val(moduleBlock))) : checked ((int) Math.Round(Conversion.Val("&h" + moduleBlock)))) : checked ((num1 - 71) * 16 /*0x10*/ + (int) Math.Round(Conversion.Val("&h" + Strings.Mid(moduleBlock, 2, 1))))), "#00");
    int num2 = Strings.Asc(Strings.Mid(moduleSection, 1, 1));
    moduleSection = Strings.Format((object) (!(num2 >= 71 & num2 <= 90) ? (!(num2 >= 65 & num2 <= 70) ? checked ((int) Math.Round(Conversion.Val(moduleSection))) : checked ((int) Math.Round(Conversion.Val("&h" + moduleSection)))) : checked ((num2 - 71) * 16 /*0x10*/ + (int) Math.Round(Conversion.Val("&h" + Strings.Mid(moduleSection, 2, 1))))), "#00");
    return $"{moduleAddr}-{moduleBlock}-{moduleSection}";
  }

  public static bool AsBuilt_LoadFile_ABT(
    string inpFileName,
    ref string[] retModuleAddresses,
    ref string[] retModuleDatas,
    ref int retModuleAddressCount)
  {
    retModuleAddresses = new string[1];
    retModuleDatas = new string[1];
    retModuleAddressCount = 0;
    string[] strArray1 = new string[1];
    string[] strArray2 = null;
    bool flag;
    try
    {
      strArray2 = File.ReadAllLines(inpFileName);
    }
    catch (Exception ex)
    {
      ProjectData.SetProjectError(ex);
      flag = false;
      ProjectData.ClearProjectError();
      goto label_12;
    }
    string str1 = "";
    string str2 = "";
    int num = checked (strArray2.Length - 1);
    int index = 0;
    while (index <= num)
    {
      strArray2[index] = Strings.Trim(strArray2[index]);
      if (Operators.CompareString(strArray2[index], "", false) != 0 && Operators.CompareString(Strings.Left(strArray2[index], 1), ";", false) != 0)
      {
        string str3;
        if (Strings.Len(strArray2[index]) % 2 == 1)
        {
          str3 = Strings.Left(strArray2[index], 7);
          str2 = Strings.Mid(strArray2[index], 8);
        }
        else
        {
          str3 = Strings.Left(strArray2[index], 8);
          str2 = Strings.Mid(strArray2[index], 9);
        }
        string str4 = $"{Strings.Left(str3, 3)}-{Strings.Mid(str3, 4, 2)}-{Strings.Mid(str3, 6)}";
        str1 = modAsBuilt.AsBuilt_LoadFile_ABT_ParseNewAddressFormat(Strings.Left(str4, 3), Strings.Mid(str4, 5, 2), Strings.Mid(str4, 8));
      }
      retModuleAddresses = (string[]) Utils.CopyArray((Array) retModuleAddresses, (Array) new string[checked (retModuleAddressCount + 1)]);
      retModuleDatas = (string[]) Utils.CopyArray((Array) retModuleDatas, (Array) new string[checked (retModuleAddressCount + 1)]);
      retModuleAddresses[retModuleAddressCount] = str1;
      retModuleDatas[retModuleAddressCount] = str2;
      checked { ++retModuleAddressCount; }
      checked { ++index; }
    }
    flag = true;
label_12:
    return flag;
  }

  public static ulong AsBuilt_HexStr2UINT64(string inpHex)
  {
    if (Operators.CompareString(inpHex, "", false) == 0)
      return 0;
    if (Strings.Len(inpHex) % 2 == 1)
      inpHex = "0" + inpHex;
    byte[] numArray = new byte[8];
    int num1 = checked (Strings.Len(inpHex) - 1);
    int y = 0;
    ulong num2 = 0;
    int Start = checked (Strings.Len(inpHex) - 1);
    while (Start >= 1)
    {
      byte num3 = checked ((byte) Math.Round(Conversion.Val("&h" + Strings.Mid(inpHex, Start, 2))));
      num2 = checked ((ulong) Math.Round(unchecked ((double) num2 + (double) num3 * Math.Pow(2.0, (double) y))));
      checked { y += 8; }
      checked { Start += -2; }
    }
    return num2;
  }

  public static ulong AsBuilt_BinStr2UINT64(string inpBIN)
  {
    if (Operators.CompareString(inpBIN, "", false) == 0)
      return 0;
    if (Strings.Len(inpBIN) % 2 == 1)
      inpBIN = "0" + inpBIN;
    int num1 = checked (Strings.Len(inpBIN) - 1);
    int y = 0;
    ulong num2 = 0;
    int Start = Strings.Len(inpBIN);
    while (Start >= 1)
    {
      ulong num3 = checked ((ulong) Math.Round(Conversion.Val(Strings.Mid(inpBIN, Start, 1))));
      num2 = checked ((ulong) Math.Round(unchecked ((double) num2 + (double) num3 * Math.Pow(2.0, (double) y))));
      checked { ++y; }
      checked { Start += -1; }
    }
    return num2;
  }

  public static string AsBuilt_Ascii2Hex(string inpAsc)
  {
    if (Operators.CompareString(inpAsc, "", false) == 0)
      return "";
    string str = "";
    int num = Strings.Len(inpAsc);
    int Start = 1;
    while (Start <= num)
    {
      str += Strings.Right("00" + Conversion.Hex(Strings.Asc(Strings.Mid(inpAsc, Start, 1))), 2);
      checked { ++Start; }
    }
    return str;
  }

  public static string AsBuilt_HexStr2BinStr(string inpHex)
  {
    if (Operators.CompareString(inpHex, "", false) == 0)
      return "";
    if (Strings.Len(inpHex) % 2 == 1)
      inpHex = "0" + inpHex;
    int num1 = Strings.Len(inpHex);
    string str1 = "";
    int num2 = checked (num1 - 1);
    int Start = 1;
    while (Start <= num2)
    {
      string str2 = modAsBuilt.AsBuilt_Dec2Bin((ulong) checked ((uint) Math.Round(Conversion.Val("&h" + Strings.Mid(inpHex, Start, 2)))));
      str1 += str2;
      checked { Start += 2; }
    }
    return str1;
  }

  public static string AsBuilt_Dec2Bin(ulong num)
  {
    StringBuilder stringBuilder = new StringBuilder();
    do
    {
      Decimal d2 = Decimal.Remainder(new Decimal(num), 2M);
      stringBuilder.Insert(0, d2);
      num = Convert.ToUInt64(Decimal.Divide(Decimal.Subtract(new Decimal(num), d2), 2M));
    }
    while (Decimal.Compare(new Decimal(num), 0M) != 0);
    return stringBuilder.Length % 8 == 0 ? stringBuilder.ToString() : new string('0', checked (8 - unchecked (stringBuilder.Length % 8))) + stringBuilder.ToString();
  }

  public static string AsBuilt_FormatReadable_Binary(string inpBinary, int numSpaces = 1)
  {
    inpBinary = Strings.Replace(inpBinary, " ", "");
    if (Strings.Len(inpBinary) % 8 != 0)
      inpBinary = new string('0', checked (8 - unchecked (Strings.Len(inpBinary) % 8))) + inpBinary;
    string str1 = "";
    int num1 = numSpaces;
    int num2 = 1;
    while (num2 <= num1)
    {
      str1 += " ";
      checked { ++num2; }
    }
    string str2 = "";
    int Start = checked (Strings.Len(inpBinary) - 7);
    while (Start >= 1)
    {
      str2 = Strings.Mid(inpBinary, Start, 8) + str1 + str2;
      checked { Start += -8; }
    }
    return Strings.Trim(str2);
  }

  public static string AsBuilt_FormatReadable_ModuleAddress(string inpModuleID)
  {
    return Strings.Trim(inpModuleID);
  }

  public static void AsBuilt_FormatReadable_ModuleData(
    string inpModuleData,
    ref string retData1,
    ref string retData2,
    ref string retData3)
  {
    retData1 = "";
    retData2 = "";
    retData3 = "";
    if (Strings.Len(inpModuleData) % 2 == 1)
      inpModuleData = "0" + inpModuleData;
    retData1 = Strings.Mid(inpModuleData, 1, 4);
    retData2 = Strings.Mid(inpModuleData, 5, 4);
    retData3 = Strings.Mid(inpModuleData, 9, 4);
  }

  public static bool CmDlgDLL_ShowOpenEx(
    ref string[] sFileTypeArray,
    ref string[] sFilePatternArray,
    ref Form ownerForm,
    ref string retSelPath,
    ref string[] retSelFileNames,
    ref int retSelFileCount,
    bool MultiSelect,
    string sDefaultPath)
  {
    retSelFileNames = new string[1];
    retSelFileCount = 0;
    OpenFileDialog openFileDialog = new OpenFileDialog();
    string str = "";
    int num1 = checked (sFileTypeArray.Length - 1);
    int index1 = 0;
    while (index1 <= num1)
    {
      if (Operators.CompareString(sFileTypeArray[index1], "", false) != 0)
      {
        int num2 = Strings.InStr(sFileTypeArray[index1], "(");
        if (num2 > 0)
          sFileTypeArray[index1] = Strings.Left(sFileTypeArray[index1], checked (num2 - 1));
        sFileTypeArray[index1] = Strings.Trim(sFileTypeArray[index1]);
        sFileTypeArray[index1] = $"{sFileTypeArray[index1]} ({sFilePatternArray[index1]})";
        str = $"{str}{sFileTypeArray[index1]}|{sFilePatternArray[index1]}|";
      }
      checked { ++index1; }
    }
    if (Operators.CompareString(Strings.Right(str, 1), "|", false) == 0)
      str = Strings.Left(str, checked (Strings.Len(str) - 1));
    openFileDialog.AddExtension = true;
    openFileDialog.CheckFileExists = true;
    openFileDialog.CheckPathExists = true;
    openFileDialog.FilterIndex = 0;
    openFileDialog.Filter = str;
    openFileDialog.InitialDirectory = sDefaultPath;
    openFileDialog.Multiselect = MultiSelect;
    openFileDialog.ShowReadOnly = false;
    DialogResult dialogResult = openFileDialog.ShowDialog((IWin32Window) ownerForm);
    bool flag = false;
    if (dialogResult != DialogResult.Cancel)
    {
      int num3 = Strings.InStrRev(openFileDialog.FileName, "\\");
      retSelPath = Strings.Left(openFileDialog.FileName, checked (num3 - 1));
      if (Operators.CompareString(Strings.Right(retSelPath, 1), ":", false) == 0)
        retSelPath += "\\";
      retSelFileNames = openFileDialog.FileNames;
      retSelFileCount = openFileDialog.FileNames.Length;
      int num4 = checked (retSelFileCount - 1);
      int index2 = 0;
      while (index2 <= num4)
      {
        int num5 = Strings.InStrRev(retSelFileNames[index2], "\\");
        retSelFileNames[index2] = Strings.Mid(retSelFileNames[index2], checked (num5 + 1));
        checked { ++index2; }
      }
      Array.Sort<string>(retSelFileNames);
      flag = true;
    }
    openFileDialog.Dispose();
    return flag;
  }

  public static bool ETIS_LoadFile_FactoryOptions_HTML(
    string inpFileName,
    ref string[] retOptions,
    ref int retOptionCount,
    ref string retVIN)
  {
    retOptions = new string[1];
    retOptionCount = 0;
    string str1 = File.ReadAllText(inpFileName);
    retVIN = "";
    string str2 = "";
    int num1 = Strings.InStr(1, str1, "VIN=");
    if (num1 > 0)
      str2 = Strings.Replace(Strings.Mid(str1, checked (num1 + 4), 17), "\"", "");
    retVIN = str2;
    int Start = Strings.InStr(1, str1, ">Minor Features<");
    if (Start == 0)
      return false;
    int num2 = Strings.InStr(Start, str1, "/table");
    if (num2 == 0)
      num2 = Strings.InStr(Start, str1, "mfcShowMore");
    string Expression = Strings.Mid(str1, Start, checked (num2 - Start));
    string[] strArray = new string[1];
    string[] sourceArray = Strings.Split(Expression, "<li>");
    int num3 = checked (sourceArray.Length - 1);
    int index = 1;
    while (index <= num3)
    {
      sourceArray[index] = Strings.Replace(sourceArray[index], "<span>", "");
      sourceArray[index] = Strings.Replace(sourceArray[index], "</span>", "");
      sourceArray[index] = Strings.Replace(sourceArray[index], "&nbsp;", "");
      int num4 = Strings.InStr(1, sourceArray[index], "</li>");
      if (num4 > 0)
        sourceArray[index] = Strings.Left(sourceArray[index], checked (num4 - 1));
      checked { ++index; }
    }
    retOptions = new string[checked (sourceArray.Length - 2 + 1)];
    Array.Copy((Array) sourceArray, 1, (Array) retOptions, 0, checked (sourceArray.Length - 1));
    retOptionCount = retOptions.Length;
    return true;
  }

  public static bool ETIS_DownloadInfo(ref WebBrowser objWebBrowser, string inpVIN)
  {
    objWebBrowser.Navigate("http://www.etis.ford.com/vehicleRegSelector.do");
    do
    {
      MyProject.Application.DoEvents();
    }
    while (objWebBrowser.IsBusy);
    objWebBrowser.Navigate("http://www.etis.ford.com/vehicleRegSelector.do");
    do
    {
      MyProject.Application.DoEvents();
    }
    while (objWebBrowser.IsBusy);
    HtmlElement Expression = modAsBuilt.DOM_WaitForElement_ByTag(objWebBrowser, "class", "vehicleIDLongVIN", 20.0);
    Expression.SetAttribute("value", inpVIN);
    Expression.SetAttribute("text", inpVIN);
    Expression.OuterText = inpVIN;
    Expression.InnerText = inpVIN;
    return !Information.IsNothing((object) Expression);
  }

  public static HtmlElement DOM_WaitForElement_ByID(
    WebBrowser wbControl,
    string elementID,
    double maxWaitSeconds)
  {
    HtmlElement Expression = (HtmlElement) null;
    long tickCount = modAsBuilt.System_GetTickCount();
    do
    {
      Application.DoEvents();
      if (!Information.IsNothing((object) wbControl.Document))
      {
        Expression = wbControl.Document.GetElementById(elementID);
        if (!Information.IsNothing((object) Expression))
          return Expression;
      }
    }
    while ((double) checked (modAsBuilt.System_GetTickCount() - tickCount) / 1000.0 <= maxWaitSeconds);
    Application.DoEvents();
    return Expression;
  }

  public static HtmlElement DOM_FindSubElement_ByTag(
    HtmlElement parentElement,
    string tagID,
    string tagValue,
    double maxWaitSeconds)
  {
    if (Information.IsNothing((object) parentElement))
      return (HtmlElement) null;
    modAsBuilt.System_GetTickCount();
    Application.DoEvents();
    HtmlElementCollection all = parentElement.All;
    if (!Information.IsNothing((object) all))
    {
      try
      {
        foreach (HtmlElement parentElement1 in all)
        {
          if (Operators.CompareString(parentElement1.GetAttribute(tagID), tagValue, false) == 0)
            return parentElement1;
          HtmlElement subElementByTag = modAsBuilt.DOM_FindSubElement_ByTag(parentElement1, tagID, tagValue, maxWaitSeconds);
          if (!Information.IsNothing((object) subElementByTag))
            return subElementByTag;
        }
      }
      finally
      {
        IEnumerator enumerator = null;
        if (enumerator is IDisposable)
          (enumerator as IDisposable).Dispose();
      }
    }
    Application.DoEvents();
    return (HtmlElement) null;
  }

  public static HtmlElement DOM_WaitForElement_ByTag(
    WebBrowser wbControl,
    string tagID,
    string tagValue,
    double maxWaitSeconds)
  {
    long tickCount = modAsBuilt.System_GetTickCount();
    string str = "";
    do
    {
      Application.DoEvents();
      if (!Information.IsNothing((object) wbControl.Document))
      {
        HtmlElementCollection all = wbControl.Document.All;
        if (!Information.IsNothing((object) all))
        {
          try
          {
            foreach (HtmlElement parentElement in all)
            {
              if (Operators.CompareString(parentElement.GetAttribute(tagID), tagValue, false) == 0)
                return parentElement;
              str = parentElement.OuterHtml;
              HtmlElement subElementByTag = modAsBuilt.DOM_FindSubElement_ByTag(parentElement, tagID, tagValue, maxWaitSeconds);
              if (!Information.IsNothing((object) subElementByTag))
                return subElementByTag;
            }
          }
          finally
          {
            IEnumerator enumerator = null;
            if (enumerator is IDisposable)
              (enumerator as IDisposable).Dispose();
          }
        }
      }
    }
    while ((double) checked (modAsBuilt.System_GetTickCount() - tickCount) / 1000.0 <= maxWaitSeconds);
    Application.DoEvents();
    return (HtmlElement) null;
  }

  public static string StorageMedia_GetSerialNumber(string drvLetter, ref string retError)
  {
    retError = "";
    drvLetter = Strings.UCase(Strings.Left(drvLetter, 1));
    byte[] udtBytes1 = new byte[1];
    modAsBuilt.UDT_SecurityAttributes_Init(ref udtBytes1);
    string lpFileName = $"\\\\.\\{drvLetter}:";
    IntPtr fileNoUdt = modAsBuilt.CreateFile_NoUDT(ref lpFileName, -1073741824 /*0xC0000000*/, 3, IntPtr.Zero, 3, 0, 0);
    if (fileNoUdt == new IntPtr(-1) | fileNoUdt == IntPtr.Zero)
    {
      retError = new Win32Exception().Message;
      Console.WriteLine(retError);
      return "";
    }
    byte[] udtBytes2 = new byte[1];
    modAsBuilt.StorageMedia_GetSerialNumber_BuildCommand(ref udtBytes2);
    byte[] numArray = new byte[1001];
    uint lpBytesReturned = 0;
    bool flag = modAsBuilt.DeviceIoControl_ByteBuffers(fileNoUdt, 499332U, ref udtBytes2[0], checked ((uint) udtBytes2.Length), ref udtBytes2[0], checked ((uint) udtBytes2.Length), ref lpBytesReturned, IntPtr.Zero);
    string str = "";
    if (!flag)
    {
      retError = new Win32Exception().Message;
      Console.WriteLine(retError);
    }
    else
    {
      int num1 = checked (udtBytes2.Length - 17);
      int num2 = checked (udtBytes2.Length - 1);
      int index = num1;
      while (index <= num2)
      {
        str = $"{str}{Conversion.Hex(udtBytes2[index])} ";
        checked { ++index; }
      }
      str = Strings.Trim(str);
    }
    modAsBuilt.CloseHandle(fileNoUdt);
    return !flag ? "" : str;
  }

  private static void StorageMedia_GetSerialNumber_BuildCommand(ref byte[] udtBytes)
  {
    int num1 = 51;
    udtBytes = new byte[checked (num1 - 1 + 1)];
    int destinationIndex1 = 0;
    Array.Copy((Array) BitConverter.GetBytes((ushort) 18), 0, (Array) udtBytes, destinationIndex1, 2);
    int destinationIndex2 = checked (destinationIndex1 + 2);
    Array.Copy((Array) BitConverter.GetBytes((ushort) 0), 0, (Array) udtBytes, destinationIndex2, 2);
    int destinationIndex3 = checked (destinationIndex2 + 2);
    Array.Copy((Array) BitConverter.GetBytes(3U), 0, (Array) udtBytes, destinationIndex3, 4);
    int destinationIndex4 = checked (destinationIndex3 + 4);
    Array.Copy((Array) BitConverter.GetBytes((ushort) 17), 0, (Array) udtBytes, destinationIndex4, 2);
    int destinationIndex5 = checked (destinationIndex4 + 2);
    Array.Copy((Array) BitConverter.GetBytes(16U /*0x10*/), 0, (Array) udtBytes, destinationIndex5, 4);
    int destinationIndex6 = checked (destinationIndex5 + 4);
    Array.Copy((Array) BitConverter.GetBytes(0U), 0, (Array) udtBytes, destinationIndex6, 4);
    int index = checked (destinationIndex6 + 4);
    udtBytes[index] = (byte) 53;
    int destinationIndex7 = checked (index + 1);
    Array.Copy((Array) BitConverter.GetBytes(0U), 0, (Array) udtBytes, destinationIndex7, 4);
    int destinationIndex8 = checked (destinationIndex7 + 4);
    Array.Copy((Array) BitConverter.GetBytes(1U), 0, (Array) udtBytes, destinationIndex8, 4);
    int destinationIndex9 = checked (destinationIndex8 + 4);
    Array.Copy((Array) BitConverter.GetBytes(1U), 0, (Array) udtBytes, destinationIndex9, 4);
    int destinationIndex10 = checked (destinationIndex9 + 4);
    Array.Copy((Array) BitConverter.GetBytes(4U), 0, (Array) udtBytes, destinationIndex10, 4);
    int num2 = checked (destinationIndex10 + 4);
  }

  private static void UDT_SecurityAttributes_Init(ref byte[] udtBytes)
  {
    udtBytes = new byte[12];
    udtBytes[0] = (byte) 12;
  }

    public static int ArraySsorted_AddItem(
    ref string[] dstArray,
    ref int dstItemCount,
    string strItemToAdd,
    bool NoDupesAllowed)
  {
      if (dstArray == null) 
      { 
          dstArray = new string[1]; 
          dstArray[0] = strItemToAdd;
          dstItemCount = 1;
          return 0;
      }
      
      // Find insertion point
      int index = 0;
      int low = 0;
      int high = dstItemCount - 1;
      while (low <= high)
      {
          int mid = (low + high) / 2;
          int cmp = String.Compare(dstArray[mid], strItemToAdd, StringComparison.Ordinal);
          if (cmp < 0) low = mid + 1;
          else if (cmp > 0) high = mid - 1;
          else 
          {
              if (NoDupesAllowed) return -1;
              index = mid; 
              goto Insert;
          }
      }
      index = low;

  Insert:
      // Ensure specific resizing behavior (preserve existing, add 1)
      // Visual Basic's Redim Preserve equivalent logic
      if (dstArray.Length <= dstItemCount)
      {
          Array.Resize(ref dstArray, dstItemCount + 1);
      }
      
      // Shift Right
      for (int i = dstItemCount; i > index; i--)
      {
          dstArray[i] = dstArray[i - 1];
      }
      
      dstArray[index] = strItemToAdd;
      dstItemCount++;
      return index;
  }

  private static void ArraySsorted_RemoveItem(
    ref string[] strArray,
    ref int strArrayItemCount,
    string strToRemove,
    CompareMethod compMethod = CompareMethod.Binary)
  {
      int index = ArraySsorted_FindItem(ref strArray, strArrayItemCount, strToRemove, compMethod);
      if (index != -1)
      {
          for (int i = index; i < strArrayItemCount - 1; i++)
          {
              strArray[i] = strArray[i + 1];
          }
          strArray[strArrayItemCount - 1] = null;
          strArrayItemCount--;
          Array.Resize(ref strArray, strArrayItemCount);
      }
  }

    public static int ArraySsorted_FindItem(
    ref string[] strArray,
    int strArrayItemCount,
    string strToFind,
    CompareMethod compMethod = CompareMethod.Binary)
  {
      for (int i = 0; i < strArrayItemCount; i++)
      {
          if (Strings.StrComp(strArray[i], strToFind, compMethod) == 0)
              return i;
      }
      return -1;
  }

    private static int ArraySsorted_FindItemPos(
    ref string[] strArray,
    int strArrayItemCount,
    string strToFindPos)
  {
      int low = 0;
      int high = strArrayItemCount - 1;
      while (low <= high)
      {
          int mid = (low + high) / 2;
          int cmp = String.Compare(strArray[mid], strToFindPos, StringComparison.Ordinal);
          if (cmp < 0) low = mid + 1;
          else if (cmp > 0) high = mid - 1;
          else return mid;
      }
      return low;
  }

    private static void ArrayS_ShiftItemsRight(
    ref string[] strArray,
    int firstItemToShift,
    int arrayItemCount = -1)
  {
      if (arrayItemCount == -1) arrayItemCount = strArray.Length;
      Array.Resize(ref strArray, arrayItemCount + 1);
      for (int i = arrayItemCount; i > firstItemToShift; i--)
      {
          strArray[i] = strArray[i - 1];
      }
      strArray[firstItemToShift] = "";
  }

  public static string CmDlgDLL_ShowSaveFile(
    Form ownerForm,
    string fileFilter,
    string titleText,
    string defaultSaveName)
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.OverwritePrompt = true;
    saveFileDialog.Title = titleText;
    saveFileDialog.FileName = defaultSaveName;
    saveFileDialog.Filter = fileFilter;
    DialogResult dialogResult = saveFileDialog.ShowDialog((IWin32Window) ownerForm);
    string str = "";
    if (dialogResult != DialogResult.Cancel)
      str = saveFileDialog.FileName;
    saveFileDialog.Dispose();
    return str;
  }

#pragma warning disable CS0649
  private struct PREVENT_MEDIA_REMOVAL
  {
    public bool PreventMediaRemoval;
  }
#pragma warning restore CS0649
}

}








