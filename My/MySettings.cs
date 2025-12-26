// Decompiled with JetBrains decompiler
// Type: AsBuiltExplorer.My.MySettings
// Assembly: AsBuiltExplorer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 9083D66F-6E27-42C7-99A4-392C98AEFBC8
// Assembly location: I:\GITHUB\Projects\AsBuiltExplorer\AsBuiltExplorer.exe

using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

#nullable disable
namespace AsBuiltExplorer.My;

[CompilerGenerated]
[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
[EditorBrowsable(EditorBrowsableState.Advanced)]
internal sealed class MySettings : ApplicationSettingsBase
{
  private static MySettings defaultInstance = (MySettings) SettingsBase.Synchronized((SettingsBase) new MySettings());
  private static bool addedHandler;
  private static object addedHandlerLockObject = RuntimeHelpers.GetObjectValue(new object());

  [DebuggerNonUserCode]
  [EditorBrowsable(EditorBrowsableState.Advanced)]
  private static void AutoSaveSettings(object sender, EventArgs e)
  {
    if (!MyProject.Application.SaveMySettingsOnExit)
      return;
    MySettingsProperty.Settings.Save();
  }

  public static MySettings Default
  {
    get
    {
      if (!MySettings.addedHandler)
      {
        object handlerLockObject = MySettings.addedHandlerLockObject;
        ObjectFlowControl.CheckForSyncLockOnValueType(handlerLockObject);
        bool lockTaken = false;
        try
        {
          Monitor.Enter(handlerLockObject, ref lockTaken);
          if (!MySettings.addedHandler)
          {
            MyProject.Application.Shutdown += (ShutdownEventHandler) ([DebuggerNonUserCode, EditorBrowsable(EditorBrowsableState.Advanced)] (sender, e) =>
            {
              if (!MyProject.Application.SaveMySettingsOnExit)
                return;
              MySettingsProperty.Settings.Save();
            });
            MySettings.addedHandler = true;
          }
        }
        finally
        {
          if (lockTaken)
            Monitor.Exit(handlerLockObject);
        }
      }
      MySettings defaultInstance = MySettings.defaultInstance;
      return defaultInstance;
    }
  }
}
