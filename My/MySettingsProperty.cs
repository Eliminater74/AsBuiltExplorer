// Decompiled with JetBrains decompiler
// Type: AsBuiltExplorer.My.MySettingsProperty
// Assembly: AsBuiltExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9083D66F-6E27-42C7-99A4-392C98AEFBC8
// Assembly location: I:\GITHUB\Projects\AsBuiltExplorer\AsBuiltExplorer.exe

using Microsoft.VisualBasic;
using Microsoft.VisualBasic.CompilerServices;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Runtime.CompilerServices;

#nullable disable
namespace AsBuiltExplorer.My;

[StandardModule]
[HideModuleName]
[DebuggerNonUserCode]
[CompilerGenerated]
internal sealed class MySettingsProperty
{
  [HelpKeyword("My.Settings")]
  internal static MySettings Settings
  {
    get
    {
      MySettings settings = MySettings.Default;
      return settings;
    }
  }
}
