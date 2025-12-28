using Microsoft.VisualBasic.ApplicationServices;
using Microsoft.VisualBasic.CompilerServices;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace AsBuiltExplorer.My
{
    [CompilerGenerated]
    [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal sealed class MySettings : ApplicationSettingsBase
    {
        static MySettings defaultInstance = (MySettings)SettingsBase.Synchronized((SettingsBase)new MySettings());
        static bool addedHandler;
        static object addedHandlerLockObject = RuntimeHelpers.GetObjectValue(new object());

        [DebuggerNonUserCode]
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        static void AutoSaveSettings(object sender, EventArgs e)
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
                    var handlerLockObject = MySettings.addedHandlerLockObject;
                    ObjectFlowControl.CheckForSyncLockOnValueType(handlerLockObject);
                    var lockTaken = false;

                    try
                    {
                        Monitor.Enter(handlerLockObject, ref lockTaken);

                        if (!MySettings.addedHandler)
                        {
                            MyProject.Application.Shutdown += (ShutdownEventHandler)((sender, e) =>
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

                var defaultInstance = MySettings.defaultInstance;
                return defaultInstance;
            }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("Light")]
        public string AppTheme
        {
            get { return ((string)(this["AppTheme"])); }
            set { this["AppTheme"] = value; }
        }
    }
}