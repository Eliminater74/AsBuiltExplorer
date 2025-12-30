using System.Configuration;

namespace AsBuiltExplorer.Properties
{
    internal sealed partial class Settings : ApplicationSettingsBase
    {
        private static Settings defaultInstance = ((Settings)(ApplicationSettingsBase.Synchronized(new Settings())));

        public static Settings Default
        {
            get
            {
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

        [UserScopedSetting()]
        [DefaultSettingValue("True")]
        public bool AutoCheckForUpdates
        {
            get { return ((bool)(this["AutoCheckForUpdates"])); }
            set { this["AutoCheckForUpdates"] = value; }
        }

        [UserScopedSetting()]
        [DefaultSettingValue("")]
        public string SkipUpdateVersion
        {
            get { return ((string)(this["SkipUpdateVersion"])); }
            set { this["SkipUpdateVersion"] = value; }
        }
    }
}
