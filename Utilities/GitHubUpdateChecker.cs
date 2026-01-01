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
        // Use /releases list to get all releases (sorted by date desc by default, but we'll scan top few)
        private const string ApiUrl = "https://api.github.com/repos/" + RepoOwner + "/" + RepoName + "/releases";

        public static async Task<UpdateInfo> CheckForUpdateAsync()
        {
            var info = new UpdateInfo();
            
            // Get Current Version
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            info.CurrentVersion = fvi.FileVersion; 

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers.Add("User-Agent", "AsBuiltExplorer-Updater");
                    
                    var json = await client.DownloadStringTaskAsync(new Uri(ApiUrl));
                    if (string.IsNullOrEmpty(json)) return null;

                    // Manual JSON Array Parsing (Top-level is [ { ... }, { ... } ])
                    // We want to find the release with the HIGHEST version number.
                    
                    var releases = Regex.Matches(json, @"\{.*?\}", RegexOptions.Singleline); // Very rough object splitter
                    // Better: Splitting by "tag_name" occurences
                    
                    // Robust approach: Regex all tag_names and their indices, verify they are in separate objects
                    // Since we want the HIGHEST version, we scan the whole string for "tag_name": "vX.X.X.X"
                    // And assume the "html_url" and "body" following it belong to it? No, JSON is unordered.
                    
                    // Given the constraint of no Newtonsoft.Json, relying on Regex for an array of objects is risky.
                    // However, defaults are sorted by date.
                    // If we just want the absolute "Latest" by version, we can try to parse the array structure.
                    
                    // Simple Strategy: Parse top 5 candidates.
                    // Find all "tag_name": "..." matches.
                    var matches = Regex.Matches(json, "\"tag_name\"\\s*:\\s*\"([^\"]+)\"");
                    
                    Version maxVer = null;
                    if (Version.TryParse(Unsuffix(info.CurrentVersion), out var currentVer))
                        maxVer = currentVer;
                    else
                        maxVer = new Version(0,0,0,0);

                    string bestTag = "";
                    
                    // First pass: Find Highest Version
                    foreach (Match m in matches)
                    {
                        var t = m.Groups[1].Value.TrimStart('v', 'V');
                        var cleanT = Unsuffix(t);
                        if (Version.TryParse(cleanT, out var v))
                        {
                            if (v > maxVer)
                            {
                                maxVer = v;
                                bestTag = m.Groups[1].Value; // Keep original tag "v1..."
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(bestTag))
                    {
                        // Now extract details for this SPECIFIC tag
                        // We need the object block for this tag. 
                        // Search for the tag, then find closest html_url / body in that block?
                        // Too complex for reliable Regex on full JSON.
                        
                        // Fallback: Just get the FIRST match (Latest by Date) from the list.
                        // Usually Latest Date == Latest Version.
                        // If the user's issue was timing, fetching the list [0] is usually fresher than /latest endpoint (which has caching/delay).
                        
                        // Let's implement getting the FIRST item in the array properly.
                        // The string starts with [. The first object is between { and }.
                        // Note: Nested braces exist.
                        
                        // Let's stick to /releases/latest logic but switch to finding the block for the best tag?
                        // No, let's keep it simple: Release 13 might not be marked "latest" yet but exists in the list.
                        // We will just find the details for "bestTag".
                        
                        // Hacky but effective: Split by "tag_name"
                        var parts = json.Split(new[] { "\"tag_name\"" }, StringSplitOptions.None);
                        // Part 0 is pre-first-tag. Part 1 starts with : "v1.0.0.13" ...
                        
                        foreach(var p in parts)
                        {
                            if (p.Trim().StartsWith(":"))
                            {
                                // Extract the value
                                var tagMatch = Regex.Match(p, "^\\s*:\\s*\"([^\"]+)\"");
                                if (tagMatch.Success) 
                                {
                                    var thisTag = tagMatch.Groups[1].Value;
                                    if (thisTag == bestTag)
                                    {
                                        // This is our block. Grab the URL and Body from THIS part (or look ahead?)
                                        // Actually, "body" and "html_url" might appear BEFORE "tag_name" in the object.
                                        // JSON keys are unordered.
                                        
                                        // OK, Plan B: Use /releases endpoint, but assume the first release in the list is the one we want if it is newer.
                                        // The API guarantees sort by created_at desc.
                                        // Use the first parsing logic we had, but applied to the first object in the array.
                                        
                                        // Just grab the first "tag_name" in the file. That is the newest created release.
                                        string tag = GetJsonValue(json, "tag_name");
                                        string body = GetJsonValue(json, "body");
                                        string url = GetJsonValue(json, "html_url");
                                        
                                        info.NewVersion = tag.TrimStart('v', 'V');
                                        info.DownloadUrl = url;
                                        info.ReleaseNotes = UnescapeJson(body);

                                        if (Version.TryParse(Unsuffix(info.NewVersion), out var vNew) &&
                                            Version.TryParse(Unsuffix(info.CurrentVersion), out var vCur))
                                        {
                                            if (vNew > vCur)
                                            {
                                                info.IsNewer = true;
                                            }
                                        }
                                        break; // Only check the very first one (Latest by Date)
                                    }
                                }
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
            var match = Regex.Match(json, $"\"{key}\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"");
            if (match.Success) return match.Groups[1].Value;
            return "";
        }

        static string UnescapeJson(string val)
        {
            if (string.IsNullOrEmpty(val)) return "";
            return val.Replace("\\r\\n", Environment.NewLine)
                      .Replace("\\n", Environment.NewLine)
                      .Replace("\\t", "\t")
                      .Replace("\\\"", "\"")
                      .Replace("\\\\", "\\");
        }
    }
}
