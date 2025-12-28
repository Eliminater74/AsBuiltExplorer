using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace AsBuiltExplorer
{
    public class CommonFeature
    {
        public string Name { get; set; }
        public string Module { get; set; }
        public string Address { get; set; }
        public string Data1Mask { get; set; }
        public string Data2Mask { get; set; }
        public string Data3Mask { get; set; }
        public string Notes { get; set; }

        public override string ToString()
        {
            return $"{Name} ({Module})";
        }
    }

    public static class CommonDatabase
    {
        public static List<CommonFeature> Features = new List<CommonFeature>();
        private static bool _loaded = false;

        public static void Load()
        {
            if (_loaded) return;

            // 1. Ensure DB exists
            DefinitionsDBHelper.Initialize();

            // 2. Check if we need to migrate (First Run)
            if (DefinitionsDBHelper.IsEmpty())
            {
                ImportFromCSV();
            }

            // 3. Load from SQL to Memory (for fast FindMatch)
            LoadFromSQL();

            _loaded = true;
        }

        private static Dictionary<string, List<CommonFeature>> _lookup = new Dictionary<string, List<CommonFeature>>();

        private static void LoadFromSQL()
        {
            Features.Clear();
            _lookup.Clear();
            using (var conn = DefinitionsDBHelper.GetConnection())
            {
                string sql = "SELECT * FROM CommonCodes";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var f = new CommonFeature
                        {
                            Name = reader["FeatureName"].ToString(),
                            Module = reader["Module"].ToString(),
                            Address = reader["Address"].ToString(),
                            Data1Mask = reader["Data1Mask"].ToString(),
                            Data2Mask = reader["Data2Mask"].ToString(),
                            Data3Mask = reader["Data3Mask"].ToString(),
                            Notes = reader["Notes"].ToString()
                        };
                        Features.Add(f);

                        if (!_lookup.ContainsKey(f.Address)) _lookup[f.Address] = new List<CommonFeature>();
                        _lookup[f.Address].Add(f);
                    }
                }
            }
        }

        public static List<CommonFeature> GetFeaturesForAddress(string address)
        {
            if (!_loaded) Load();
            if (_lookup.ContainsKey(address)) return _lookup[address];
            return new List<CommonFeature>();
        }

        private static void ImportFromCSV()
        {
            try
            {
                // Look in CustomCSV directory
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string customCsvDir = Path.Combine(baseDir, "CustomCSV");

                if (!Directory.Exists(customCsvDir)) return;

                // Load ALL .csv files
                string[] files = Directory.GetFiles(customCsvDir, "*.csv");

                if (files.Length == 0) return;

                foreach (var path in files)
                {
                    ParseAndInsertCSV(path);
                    
                    // User Request: Rename old CSV to .bak
                    try
                    {
                        string backupPath = path + ".bak";
                        if (File.Exists(backupPath)) File.Delete(backupPath);
                        File.Move(path, backupPath);
                    }
                    catch { /* Ignore file lock issues, migration is what matters */ }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error migrating CSVs to Database: " + ex.Message);
            }
        }

        private static void ParseAndInsertCSV(string path)
        {
            var lines = File.ReadAllLines(path);
            string currentFeatureName = "";
            string currentModule = "";
            string currentAddr = "";

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Trim().StartsWith("#")) continue; // Skip comments

                var parts = SplitCsvLine(line);
                if (parts.Count < 3) continue;

                if (parts[0].Trim().StartsWith("Feature Name") || parts[0].Trim().StartsWith("Module")) continue;

                string name = "", module = "", address = "", d1 = "", d2 = "", d3 = "", notes = "";

                // Detect Format
                bool isFormat2 = Regex.IsMatch(parts[1].Trim(), @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}");

                if (isFormat2) // Module, Address, D1...
                {
                    module = parts[0].Trim();
                    address = parts[1].Trim();
                    d1 = parts.Count > 2 ? parts[2].Trim() : "";
                    d2 = parts.Count > 3 ? parts[3].Trim() : "";
                    d3 = parts.Count > 4 ? parts[4].Trim() : "";
                    name = parts.Count > 5 ? parts[5].Trim() : "";
                    notes = parts.Count > 6 ? parts[6].Trim() : "";

                    if (string.IsNullOrWhiteSpace(module)) module = currentModule; else currentModule = module;
                    if (string.IsNullOrWhiteSpace(address)) address = currentAddr; else currentAddr = address;
                }
                else // Name, Module, Address...
                {
                    name = parts[0].Trim();
                    if (string.IsNullOrWhiteSpace(name)) name = currentFeatureName; else currentFeatureName = name;
                    
                    module = parts[1].Trim();
                    address = parts[2].Trim();
                    if (!address.Contains("-")) continue;

                    d1 = parts.Count > 3 ? parts[3].Trim() : "";
                    d2 = parts.Count > 4 ? parts[4].Trim() : "";
                    d3 = parts.Count > 5 ? parts[5].Trim() : "";
                    notes = parts.Count > 6 ? parts[6].Trim() : "";
                }

                // Normalize Wildcards
                d1 = d1.Replace("*", "x");
                d2 = d2.Replace("*", "x");
                d3 = d3.Replace("*", "x");

                // Insert into SQLite
                DefinitionsDBHelper.AddEntry(name, module, address, d1, d2, d3, notes);
            }
        }

        private static List<string> SplitCsvLine(string line)
        {
            var result = new List<string>();
            bool inQuotes = false;
            string currentValue = "";
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"') inQuotes = !inQuotes;
                else if (c == ',' && !inQuotes) { result.Add(currentValue); currentValue = ""; }
                else currentValue += c;
            }
            result.Add(currentValue);
            return result;
        }

        public static CommonFeature FindMatch(string address, string d1, string d2, string d3)
        {
             if (!_loaded) Load();

             foreach(var f in Features)
             {
                 if (f.Address == address)
                 {
                     if (MatchMask(d1, f.Data1Mask) && 
                         MatchMask(d2, f.Data2Mask) && 
                         MatchMask(d3, f.Data3Mask))
                     {
                         return f;
                     }
                 }
             }
             return null;
        }

        private static bool MatchMask(string value, string mask)
        {
            value = value?.Replace(" ", "").Trim() ?? "";
            mask = mask?.Replace(" ", "").Trim() ?? "";

            if (string.IsNullOrEmpty(mask) || mask.ToLower() == "xxxx") return true;
            
            if (value.Length != mask.Length) return false;

            for (int i = 0; i < mask.Length; i++)
            {
                char m = char.ToLower(mask[i]);
                if (m == 'x' || m == '*') continue;
                if (char.ToLower(value[i]) != m) return false;
            }

            return true;
        }
    }
}
