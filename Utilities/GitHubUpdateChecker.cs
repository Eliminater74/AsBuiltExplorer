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
                        // Get tag details from the matching block logic (simplified for regex scan)
                        // We need the block for 'bestTag'.
                        // Scan for "tag_name": "bestTag"
                        // Then scan for "body": "..." and "html_url": "..." in proximity?
                        // Given the complexity, and assuming the FIRST item in /releases is the newest (usually true),
                        // we'll grab the first item's details IF it matches bestTag.
                        // If bestTag is NOT the first item (e.g. a patch release on an old branch), we fallback to generic logic.
                        
                        // Current logic: Find the block for bestTag.
                        string tag = "", body = "", url = "";

                        var parts = json.Split(new[] { "\"tag_name\"" }, StringSplitOptions.None);
                        foreach(var p in parts)
                        {
                            if (p.Trim().StartsWith(":"))
                            {
                                var tagMatch = Regex.Match(p, "^\\s*:\\s*\"([^\"]+)\"");
                                if (tagMatch.Success && tagMatch.Groups[1].Value == bestTag)
                                {
                                    // Extract body/url from this part (or look previous part for body/url if JSON ordered differently?)
                                    // JSON keys are unordered. This splitting approach is risky.
                                    
                                    // Better approach: Rely on the MAX version logic, but fetch the specific release details by TAG name if possible?
                                    // /releases/tags/{tag}
                                    // YES. This is cleaner.
                                    
                                    // We found the best tag. Now get its specific details.
                                    tag = bestTag;
                                    break;
                                }
                            }
                        }

                            // If URL is missing, assume standard tag URL
                            if (string.IsNullOrEmpty(url)) url = $"https://github.com/{RepoOwner}/{RepoName}/releases/tag/{tag}";

                            // ASSET PARSING: Try to find a direct .exe download link
                            // "assets": [ { "browser_download_url": "..." } ]
                            // Only if we downloaded the tag JSON
                            string assetUrl = "";
                            var assetMatches = Regex.Matches(body + url + (json ?? ""), "\"browser_download_url\"\\s*:\\s*\"([^\"]+)\"");
                            // Actually, we need to search the tagJson source if available, or original JSON if the block contains assets.
                            // The original /releases JSON contains assets for each release. 
                            // The regex `releases` splitting logic above makes it hard to pinpoint.
                            // BUT, we have `tagUrl` fetching code now. `tagJson` contains the assets.
                            
                            // Let's re-fetch tagJson if we skipped it? No, code above fetches it.
                            // But `tag` var was overwritten.
                            // Let's improve the tagJson fetching block to extract assets.
                            
                            // RE-IMPLEMENTING the tag parsing block to be robust
                            
                            string directDownload = "";
                            
                            try
                            {
                                // We always fetch the specific tag details now to be safe and get assets
                                var tagUrl = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/releases/tags/{bestTag}"; // Use bestTag
                                var tagJson = await client.DownloadStringTaskAsync(new Uri(tagUrl));
                                
                                tag = GetJsonValue(tagJson, "tag_name");
                                body = GetJsonValue(tagJson, "body");
                                url = GetJsonValue(tagJson, "html_url");
                                
                                // Find .exe asset
                                var assets = Regex.Matches(tagJson, "\"browser_download_url\"\\s*:\\s*\"([^\"]+)\"");
                                foreach (Match am in assets)
                                {
                                    var link = am.Groups[1].Value;
                                    if (link.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                                    {
                                        directDownload = link;
                                        break; 
                                    }
                                }
                            }
                            catch 
                            { 
                                // Silent fail, keep html_url if setup fails
                            }

                            info.NewVersion = (tag ?? bestTag).TrimStart('v', 'V');
                            info.DownloadUrl = !string.IsNullOrEmpty(directDownload) ? directDownload : url;
                            info.ReleaseNotes = UnescapeJson(body);

                            if (Version.TryParse(Unsuffix(info.NewVersion), out var vNew) &&
                                Version.TryParse(Unsuffix(info.CurrentVersion), out var vCur))
                            {
                                if (vNew > vCur)
                                {
                                    info.IsNewer = true;
                                    
                                    bool notesAreEmpty = string.IsNullOrWhiteSpace(info.ReleaseNotes) || 
                                                         info.ReleaseNotes.Contains("**Full Changelog**");
                                                         
                                    if (notesAreEmpty || true) 
                                    {
                                        var commits = await FetchCommits(client, "v" + info.CurrentVersion, "v" + info.NewVersion);
                                        if (!string.IsNullOrEmpty(commits))
                                        {
                                            if (string.IsNullOrWhiteSpace(info.ReleaseNotes)) 
                                                info.ReleaseNotes = "### Changes:\n" + commits;
                                            else
                                                info.ReleaseNotes += "\n\n### Recent Commits:\n" + commits;
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

        private static async Task<string> FetchCommits(WebClient client, string baseTag, string headTag)
        {
            try
            {
                var compareUrl = $"https://api.github.com/repos/{RepoOwner}/{RepoName}/compare/{baseTag}...{headTag}";
                var json = await client.DownloadStringTaskAsync(new Uri(compareUrl));
                
                // Extract commit messages
                // Look for "message": "..." inside "commit" object
                // Regex is tricky. 
                // Pattern: "message"\s*:\s*"((?:[^"\\]|\\.)*)"
                var msgs = Regex.Matches(json, "\"message\"\\s*:\\s*\"((?:[^\"\\\\]|\\\\.)*)\"");
                
                var sb = new System.Text.StringBuilder();
                int count = 0;
                
                foreach (Match m in msgs)
                {
                    if (count >= 15) break; 
                    
                    var raw = m.Groups[1].Value;
                    var msg = UnescapeJson(raw);
                    
                    // Filter: Only use first line of commit message
                    var subject = msg.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    if (subject.StartsWith("Merge pull request")) continue; // Skip merge noise
                    if (subject.StartsWith("Bump version")) continue;
                    
                    // Unique check? (Compare endpoint returns chrono list)
                    sb.AppendLine("- " + subject);
                    count++;
                }
                
                return sb.ToString();
            }
            catch
            {
                return "";
            }
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
