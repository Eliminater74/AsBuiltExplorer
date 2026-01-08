using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace AsBuiltExplorer.Localization
{
    /// <summary>
    /// Manages application localization and language switching.
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>
        /// Current language code (e.g., "en", "it", "auto")
        /// </summary>
        public static string CurrentLanguage { get; private set; } = "auto";

        /// <summary>
        /// Event fired when language changes (for forms to refresh UI)
        /// </summary>
        public static event EventHandler LanguageChanged;

        /// <summary>
        /// Available languages with their display names
        /// </summary>
        private static readonly List<LanguageInfo> _languages = new List<LanguageInfo>
        {
            new LanguageInfo("auto", "Auto (System Default)"),
            new LanguageInfo("en", "English"),
            new LanguageInfo("it", "Italiano")
        };

        /// <summary>
        /// Initialize localization at app startup.
        /// Applies saved language or falls back to system default.
        /// </summary>
        public static void Initialize()
        {
            var savedLanguage = Properties.Settings.Default.AppLanguage;
            if (string.IsNullOrEmpty(savedLanguage))
            {
                savedLanguage = "auto";
            }
            ApplyLanguage(savedLanguage, fireEvent: false);
        }

        /// <summary>
        /// Apply the specified language to the application.
        /// </summary>
        /// <param name="langCode">Language code ("auto", "en", "it", etc.)</param>
        /// <param name="fireEvent">Whether to fire LanguageChanged event</param>
        public static void ApplyLanguage(string langCode, bool fireEvent = true)
        {
            CurrentLanguage = langCode;

            CultureInfo culture;

            if (langCode == "auto")
            {
                // Use system culture
                culture = CultureInfo.CurrentUICulture;
            }
            else
            {
                try
                {
                    culture = new CultureInfo(langCode);
                }
                catch
                {
                    // Fallback to English if invalid code
                    culture = new CultureInfo("en");
                }
            }

            // Set thread culture for resource loading
            Thread.CurrentThread.CurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;

            // Save preference
            Properties.Settings.Default.AppLanguage = langCode;
            Properties.Settings.Default.Save();

            // Notify listeners
            if (fireEvent)
            {
                LanguageChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Get list of available languages for UI display.
        /// </summary>
        public static List<LanguageInfo> GetAvailableLanguages()
        {
            return new List<LanguageInfo>(_languages);
        }

        /// <summary>
        /// Get the display name for current language.
        /// </summary>
        public static string GetCurrentLanguageName()
        {
            var lang = _languages.Find(l => l.Code == CurrentLanguage);
            return lang?.DisplayName ?? CurrentLanguage;
        }

        /// <summary>
        /// Check if a restart is recommended for full language change.
        /// </summary>
        public static bool IsRestartRecommended()
        {
            // WinForms doesn't fully support runtime culture changes
            // for some controls, so we recommend restart
            return true;
        }
    }

    /// <summary>
    /// Represents a supported language.
    /// </summary>
    public class LanguageInfo
    {
        public string Code { get; }
        public string DisplayName { get; }

        public LanguageInfo(string code, string displayName)
        {
            Code = code;
            DisplayName = displayName;
        }

        public override string ToString() => DisplayName;
    }
}
