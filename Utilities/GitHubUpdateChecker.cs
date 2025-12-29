using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AsBuiltExplorer.Utilities
{
    public class UpdateInfo
    {
        public string CurrentVersion { get; set; }
        public string NewVersion { get; set; }
        public string ReleaseNotes { get; set; }
        public string DownloadUrl { get; set; }
        public bool IsNewer { get; set; }
    }

    public static class GitHubUpdateChecker
    {
        private const string RepoOwner = "Eliminater74";
        private const string RepoName = "AsBuiltExplorer";
        private const string ApiUrl = "https://api.github.com/repos/" + RepoOwner + "/" + RepoName + "/releases/latest";

        public static async Task<UpdateInfo> CheckForUpdateAsync()
        {
            var info = new UpdateInfo();
            
            // Get Current Version
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            info.CurrentVersion = fvi.FileVersion; // e.g. "1.0.0.6"

            try
            {
                using (var client = new WebClient())
                {
                    // GitHub API requires User-Agent
                    client.Headers.Add("User-Agent", "AsBuiltExplorer-Updater");
                    
                    var json = await client.DownloadStringTaskAsync(new Uri(ApiUrl));
                    
                    if (string.IsNullOrEmpty(json)) return null;

                    // Parse JSON using Regex (avoid dependencies)
                    // "tag_name": "v1.0.0.6",
                    var matchTag = Regex.Match(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
                    // "body": "Release notes...",
                    var matchBody = Regex.Match(json, "\"body\"\\s*:\\s*\"(.*?)\"(?=\\s*,\\s*\"\\w+\":)", RegexOptions.Singleline); 
                    // Note: Body regex is tricky with JSON escaping. 
                    // Safer to extract body by looking for "body": " and closing ", but escaped quotes issue.
                    // Let's use a simpler heuristic or just extract known fields.
                    
                    // Simple JSON string extraction helper
                    string tag = GetJsonValue(json, "tag_name");
                    string body = GetJsonValue(json, "body");
                    string url = GetJsonValue(json, "html_url");

                    if (!string.IsNullOrEmpty(tag))
                    {
                        info.NewVersion = tag.TrimStart('v', 'V');
                        info.DownloadUrl = url;
                        info.ReleaseNotes = UnescapeJson(body);

                        // Compare Versions
                        Version vCurrent, vNew;
                        // Clean up versions (remove -beta, -RC)
                        var cleanCurrent = Unsuffix(info.CurrentVersion);
                        var cleanNew = Unsuffix(info.NewVersion);

                        if (Version.TryParse(cleanCurrent, out vCurrent) && Version.TryParse(cleanNew, out vNew))
                        {
                            if (vNew > vCurrent)
                            {
                                info.IsNewer = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Update Check Failed: " + ex.Message);
                return null;
            }

            return info;
        }

        static string Unsuffix(string v)
        {
            if (v.Contains("-")) return v.Split('-')[0];
            return v;
        }

        static string GetJsonValue(string json, string key)
        {
            // Look for "key": "value"
            // Handle escaped quotes in value?
            // Basic regex: "key"\s*:\s*"(.*?)"
            // But value might contain quotes.
            // JSON strings escape quotes as \".
            // Regex: "key"\s*:\s*"((?:[^"\\]|\\.)*)"
            var match = Regex.Match(json, $"\"{key}\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"");
            if (match.Success) return match.Groups[1].Value;
            return "";
        }

        static string UnescapeJson(string val)
        {
            if (string.IsNullOrEmpty(val)) return "";
            // \r\n -> Newline
            // \" -> "
            return val.Replace("\\r\\n", Environment.NewLine)
                      .Replace("\\n", Environment.NewLine)
                      .Replace("\\t", "\t")
                      .Replace("\\\"", "\"")
                      .Replace("\\\\", "\\");
        }
    }
}
