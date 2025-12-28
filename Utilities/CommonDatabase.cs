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
        public int ID { get; set; }
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
            ForceReload();
        }

        public static void ForceReload()
        {
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
                            ID = Convert.ToInt32(reader["ID"]),
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
            
            // State for "ditto" fields
            string lastFeatureName = "";
            string lastModule = "";
            string lastAddr = "";
            int detectedFormat = -1; // 0=Addr@0, 1=Addr@1, 2=Addr@2

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var parts = SplitCsvLine(line);
                
                // Find Address Anchor
                int addrIdx = -1;
                for(int i=0; i<parts.Count; i++)
                {
                    if (Regex.IsMatch(parts[i].Trim(), @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}"))
                    {
                        addrIdx = i;
                        break;
                    }
                }

                if (addrIdx != -1) detectedFormat = addrIdx; // Latch format once found
                
                // If we haven't found a format yet and this line has no address, skip (likely header)
                if (detectedFormat == -1 && addrIdx == -1) continue;

                string name = "", module = "", address = "", d1 = "", d2 = "", d3 = "", notes = "";
                
                // Use explicit address if found, otherwise lastAddr (Ditto)
                if (addrIdx != -1) address = parts[addrIdx].Trim();
                else address = lastAddr; // Reuse last known address for blank lines (implied ditto)

                // Skip if still no address
                if (string.IsNullOrEmpty(address)) continue;

                // Apply Format Logic based on Anchor (detectedFormat)
                if (detectedFormat == 0) // Format 1 (My Export): Addr | D1 | D2 | D3 | Name
                {
                    address = parts.Count > 0 ? parts[0].Trim() : "";
                    d1 = parts.Count > 1 ? parts[1].Trim() : "";
                    d2 = parts.Count > 2 ? parts[2].Trim() : "";
                    d3 = parts.Count > 3 ? parts[3].Trim() : "";
                    name = parts.Count > 4 ? parts[4].Trim() : "";
                }
                else if (detectedFormat == 1) // Format 2 (2GFusions/CommonCodes2): Index | Polyglot(Addr/Mod/Name) | D1 | D2?
                {
                    // This format is tricky. Col 0 is Index. Col 1 is "Key".
                    string key = parts.Count > 1 ? parts[1].Trim() : "";
                    
                    if (Regex.IsMatch(key, @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}.*")) // Allow suffix text
                    {
                        address = key;
                    }
                    else if (key.StartsWith("(") || (key.Length < 10 && key.Any(char.IsLetterOrDigit))) 
                    {
                         // Likely a Module name like "(IPC)" or "APIM"
                         module = key;
                    }
                    else if (key.Length > 0)
                    {
                        // Likely a Feature Name
                        name = key;
                    }

                    // D1 is Column 2
                    d1 = parts.Count > 2 ? parts[2].Trim() : "";
                    
                    // Col 3 usually D2?
                    d2 = parts.Count > 3 ? parts[3].Trim() : "";
                    
                    // Note: Format 2 (CommonCodes2) had: Mod(0)|Addr(1)|D1(2)... 
                    // But 2GFusions has Index(0)|Addr(1)|D1(2)...
                    // Both have Address in Col 1.
                    // My Polyglot logic sees key="760..." -> Addr.
                    // But it ignores Col 0 "ABS". 
                    // Fix: Check Col 0 too!
                    
                    string col0 = parts.Count > 0 ? parts[0].Trim() : "";
                    if (!string.IsNullOrEmpty(col0) && !Regex.IsMatch(col0, @"^\d+$") && col0.Length < 10) 
                    {
                        // Col 0 is NOT an index, so it might be Module (CommonCodes2 style)
                        module = col0;
                    }
                }
                else if (detectedFormat == 2) // Format 3 (Livnitup): Name | Mod | Addr | D1 | D2 | D3 | Notes
                {
                    name = parts.Count > 0 ? parts[0].Trim() : "";
                    module = parts.Count > 1 ? parts[1].Trim() : "";
                    address = parts.Count > 2 ? parts[2].Trim() : "";

                    d1 = parts.Count > 3 ? parts[3].Trim() : "";
                    d2 = parts.Count > 4 ? parts[4].Trim() : "";
                    d3 = parts.Count > 5 ? parts[5].Trim() : "";
                    notes = parts.Count > 6 ? parts[6].Trim() : "";

                     // Fix: Some files have "D1 D2 D3" in a single column
                     if (d1.Contains(" ") && string.IsNullOrEmpty(d2))
                     {
                         var masks = d1.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                         if (masks.Length > 0) d1 = masks[0];
                         if (masks.Length > 1) d2 = masks[1];
                         if (masks.Length > 2) d3 = masks[2];
                     }
                }
                
                
                // --- 1. Fix Merged Address/Mask (e.g. "7D0-01-02 0xxx") ---
                if (address.Contains(" "))
                {
                    // The address column likely contains "Address Mask" OR "Address (Year)"
                    string[] addrParts = address.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (addrParts.Length > 1 && Regex.IsMatch(addrParts[0], @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}"))
                    {
                        address = addrParts[0]; // The clean address
                        string suffix = addrParts[1]; 

                        // Determine if the suffix is a Mask or just Info (Year, etc)
                        // Masks usually contain 'x', '*', or are purely hex/numeric.
                        // Info usually starts with '('.
                        bool looksLikeMask = Regex.IsMatch(suffix, @"[*xX]") || (Regex.IsMatch(suffix, @"^[0-9A-F]+$") && !suffix.StartsWith("("));

                        if (looksLikeMask)
                        {
                            // It's a mask trapped in the address col
                            // If we found a mask here, the "D1" column we parsed earlier was probably actually Notes
                            if (!string.IsNullOrEmpty(d1))
                            {
                                if (!string.IsNullOrEmpty(notes)) notes = d1 + " " + notes;
                                else notes = d1;
                            }
                            d1 = suffix; // Put extracted mask into D1
                        }
                        else
                        {
                            // It's just info (e.g. "(2013+"), move to notes
                            if (!string.IsNullOrEmpty(notes)) notes = suffix + " " + notes;
                            else notes = suffix;
                            // Leave D1 alone (it probably contains the real mask)
                        }
                    }
                }

                // --- 2. Clean Indices (Pre-Persistence) ---
                // Clear numeric indices so persistence can fill in the correct parent value
                if (Regex.IsMatch(module.Trim(), @"^[\d\.]+$")) module = "";
                if (Regex.IsMatch(name.Trim(), @"^[\d\.]+$")) name = "";
                
                // --- 3. Persistence / Merged Cells ---
                if (string.IsNullOrEmpty(module)) module = lastModule; else lastModule = module;
                if (string.IsNullOrEmpty(name)) name = lastFeatureName; else lastFeatureName = name;
                if (!string.IsNullOrEmpty(address)) lastAddr = address;

                // --- 4. Heuristic Cleanup (Notes in Mask columns) ---
                // Do this BEFORE stripping spaces so we preserve note readability
                
                // Check if D2 looks like a note
                if (!string.IsNullOrEmpty(d2) && (d2.ToLower().Contains("note:") || d2.Contains("(") || d2.Length > 12))
                {
                    string append = d2;
                    if (!string.IsNullOrEmpty(d3)) append += " " + d3; // D3 is likely part of the note too
                    
                    if (!string.IsNullOrEmpty(notes)) notes = append + " " + notes;
                    else notes = append;
                    
                    d2 = "";
                    d3 = "";
                }
                
                // Check if D3 looks like a note
                if (!string.IsNullOrEmpty(d3) && (d3.ToLower().Contains("note:") || d3.Contains("(") || d3.Length > 12))
                {
                     if (!string.IsNullOrEmpty(notes)) notes = d3 + " " + notes;
                     else notes = d3;
                     d3 = "";
                }

                // --- 5. Heuristic: Extract Mask from Notes (e.g. "xxxx xBxx xxxx - Description") ---
                // Some files put the specific value in the Notes column
                var maskInNotesMatch = Regex.Match(notes.Trim(), @"^([xX0-9A-Fa-f]{4}[\s\-]+[xX0-9A-Fa-f]{4}[\s\-]+[xX0-9A-Fa-f]{4})");
                if (maskInNotesMatch.Success)
                {
                    string extractedMask = maskInNotesMatch.Groups[1].Value;
                    
                    // If D1 is garbage (length mismatch) or we trust the note more?
                    // Usually if the note has a full mask, it's the specific setting for this row.
                    d1 = extractedMask;
                    
                    // Remove mask from notes
                    notes = notes.Substring(maskInNotesMatch.Length).Trim();
                    // Clean up leading " - " or similar
                    if (notes.StartsWith("-") || notes.StartsWith(":")) notes = notes.Substring(1).Trim();
                }

                // Cleanup Masks (Wildcards) - PRESERVE *
                d1 = d1.Replace(" ", "").Replace("-", ""); // Also strip hyphens if pseudo-mask used them
                d2 = d2.Replace(" ", "").Replace("-", "");
                d3 = d3.Replace(" ", "").Replace("-", "");

                // Only insert if we have at least one mask
                if (string.IsNullOrEmpty(d1) && string.IsNullOrEmpty(d2) && string.IsNullOrEmpty(d3)) continue;
                
                // Skip Header-like lines
                if (name.ToLower() == "feature name" || module.ToLower() == "module") continue;

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

        public static void UpdateEntry(CommonFeature f)
        {
            DefinitionsDBHelper.UpdateEntry(f.ID, f.Name, f.Module, f.Address, f.Data1Mask, f.Data2Mask, f.Data3Mask, f.Notes);
            var existing = Features.FirstOrDefault(x => x.ID == f.ID);
            if (existing != null)
            {
                existing.Name = f.Name;
                existing.Module = f.Module;
                existing.Address = f.Address;
                existing.Data1Mask = f.Data1Mask;
                existing.Data2Mask = f.Data2Mask;
                existing.Data3Mask = f.Data3Mask;
                existing.Notes = f.Notes;
            }
        }

        public static void ExportToCSV(string filename, List<CommonFeature> exportList = null)
        {
            var list = exportList ?? Features;
            using (var sw = new StreamWriter(filename))
            {
                sw.WriteLine("FeatureName,Module,Address,Data1Mask,Data2Mask,Data3Mask,Notes");
                foreach(var f in list)
                {
                    string line = $"{EscapeCsv(f.Name)},{EscapeCsv(f.Module)},{EscapeCsv(f.Address)},{EscapeCsv(f.Data1Mask)},{EscapeCsv(f.Data2Mask)},{EscapeCsv(f.Data3Mask)},{EscapeCsv(f.Notes)}";
                    sw.WriteLine(line);
                }
            }
        }

        private static string EscapeCsv(string val)
        {
            if (string.IsNullOrEmpty(val)) return "";
            if (val.Contains(",") || val.Contains("\"") || val.Contains("\n"))
            {
                val = val.Replace("\"", "\"\"");
                return $"\"{val}\"";
            }
            return val;
        }

        public static void ImportCSV(string filepath)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string customCsvDir = Path.Combine(baseDir, "CustomCSV");
            if (!Directory.Exists(customCsvDir)) Directory.CreateDirectory(customCsvDir);

            string dest = Path.Combine(customCsvDir, Path.GetFileName(filepath));
            File.Copy(filepath, dest, true);

            _loaded = false; 
            Load(); 
        }
    }
}
