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

            // 2. Import any new CSVs found in CustomCSV folder
            // (We do this every time because processed files get renamed to .bak)
            ImportFromCSV();

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

                int count = 0;
                foreach (var path in files)
                {
                    ParseAndInsertCSV(path);
                    
                    // User Request: Rename old CSV to .bak
                    try
                    {
                        string backupPath = path + ".bak";
                        if (File.Exists(backupPath)) File.Delete(backupPath);
                        File.Move(path, backupPath);
                        count++;
                    }
                    catch { /* Ignore file lock issues, migration is what matters */ }
                }

                if (count > 0)
                {
                    MessageBox.Show($"Successfully imported {count} new definition files into the Common Database!", "Library Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            
            // State for "ditto" fields (empty cells meaning "same as above")
            string lastFeatureName = "";
            string lastModule = "";
            string lastAddr = "";
            
            int detectedFormat = 0; // 0=Unknown, 1=AddrCol0, 2=AddrCol1, 3=AddrCol2

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.Trim().StartsWith("#")) continue;

                var parts = SplitCsvLine(line);
                if (parts.Count < 2) continue;

                string col0 = parts[0].Trim();
                string col1 = parts.Count > 1 ? parts[1].Trim() : "";
                string col2 = parts.Count > 2 ? parts[2].Trim() : "";

                // Attempt to detect format if explicit address is present
                if (Regex.IsMatch(col0, @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}")) detectedFormat = 1;
                else if (Regex.IsMatch(col1, @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}")) detectedFormat = 2;
                else if (Regex.IsMatch(col2, @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}")) detectedFormat = 3;

                if (detectedFormat == 0) continue;

                string name = "", module = "", address = "", d1 = "", d2 = "", d3 = "", notes = "";

                if (detectedFormat == 1)
                {
                    // Format: Address | D1 | D2 | D3 | Name
                    address = col0;
                    d1 = col1;
                    d2 = col2;
                    d3 = parts.Count > 3 ? parts[3].Trim() : "";
                    name = parts.Count > 4 ? parts[4].Trim() : "";
                }
                else if (detectedFormat == 2)
                {
                    // Format: Module | Address | D1 | D2 | D3 | Name
                    module = col0;
                    address = col1;
                    d1 = col2;
                    d2 = parts.Count > 3 ? parts[3].Trim() : "";
                    d3 = parts.Count > 4 ? parts[4].Trim() : "";
                    name = parts.Count > 5 ? parts[5].Trim() : "";
                }
                else if (detectedFormat == 3)
                {
                    // Format: Name | Module | Address | Default | Mask | Notes
                    name = col0;
                    module = col1;
                    address = col2;
                    
                    // Mask in Col 4
                    string maskRaw = parts.Count > 4 ? parts[4].Trim() : "";
                    var masks = maskRaw.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (masks.Length > 0) d1 = masks[0];
                    if (masks.Length > 1) d2 = masks[1];
                    if (masks.Length > 2) d3 = masks[2];
                    
                    notes = parts.Count > 5 ? parts[5].Trim() : "";
                }

                // Handle Persistence / Merged Cells
                if (string.IsNullOrEmpty(address)) address = lastAddr; else lastAddr = address;
                if (string.IsNullOrEmpty(module)) module = lastModule; else lastModule = module;
                if (string.IsNullOrEmpty(name)) name = lastFeatureName; else lastFeatureName = name; // If name is empty, assume same feature (e.g. multiple options)

                if (string.IsNullOrEmpty(address)) continue;

                // Cleanup Masks (Wildcards)
                // Cleanup Masks (Wildcards)
                d1 = d1.Replace(" ", "");
                d2 = d2.Replace(" ", "");
                d3 = d3.Replace(" ", "");

                // Only insert if we have at least one mask
                if (string.IsNullOrEmpty(d1) && string.IsNullOrEmpty(d2) && string.IsNullOrEmpty(d3)) continue;

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
