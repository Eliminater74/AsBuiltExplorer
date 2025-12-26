using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AsBuiltExplorer.My;

namespace AsBuiltExplorer
{
    public static class ModuleDatabase
    {
        private static Dictionary<string, string> _moduleNames = new Dictionary<string, string>();
        private static bool _isLoaded = false;

        public static void LoadDatabase()
        {
            if (_isLoaded) return;

            try
            {
                string dbPath = Path.Combine(MyProject.Application.Info.DirectoryPath, "Reference_ModuleList.txt");
                if (!File.Exists(dbPath))
                {
                    // Optionally create default or just return
                    return;
                }

                var lines = File.ReadAllLines(dbPath);
                foreach (var line in lines)
                {
                    // Format: "Full Name (Abbr)|Abbr|Address"
                    // Example: "Accessory Protocol Interface Module (APIM)|APIM|7D0"
                    // Some lines might be missing address or abbreviation
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var parts = line.Split('|');
                    if (parts.Length >= 3)
                    {
                        string fullName = parts[0].Trim();
                        string abbr = parts[1].Trim();
                        string address = parts[2].Trim(); // Hex address like "7D0"

                        if (!string.IsNullOrEmpty(address))
                        {
                            // Store by address. If duplicates, last one wins or ignore? 
                            // Let's use the first one or overwrite.
                            // Database has some duplicates/overlapping ranges, but key is address prefix.
                            // The AsBuilt data typically has "7D0-01-01". The module ID is "7D0".
                            
                            if (!_moduleNames.ContainsKey(address))
                            {
                                // We'll store "Abbr - Full Name" or just "Abbr" depending on preference.
                                // For now, let's store the Abbreviation if valid, else Full Name.
                                string displayName = !string.IsNullOrEmpty(abbr) ? abbr : fullName;
                                _moduleNames[address] = displayName;
                            }
                        }
                    }
                }
                _isLoaded = true;
            }
            catch (Exception ex)
            {
                // Silently fail or log? For now, silent as it's an enhancement.
                System.Diagnostics.Debug.WriteLine("Error loading module database: " + ex.Message);
            }
        }

        public static string GetModuleName(string address)
        {
            // Address might be full line "7D0-01-01" or just "7D0"
            // We need to extract the first part.
            
            if (string.IsNullOrEmpty(address)) return "";
            if (!_isLoaded) LoadDatabase();

            // Check exact match first
            if (_moduleNames.ContainsKey(address)) return _moduleNames[address];

            // Check if it's a formatted address like "7D0-01-01" or "7D00101"
            // Typically generic Ford modules are 3 hex chars.
            if (address.Length >= 3)
            {
                string prefix = address.Substring(0, 3);
                if (_moduleNames.ContainsKey(prefix)) return _moduleNames[prefix];
            }

            return "";
        }
    }
}
