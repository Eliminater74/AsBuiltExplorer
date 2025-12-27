using System;
using System.Collections.Generic;
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
            
            // Load all CSVs starting with "CommonCodes"
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] files = Directory.GetFiles(baseDir, "CommonCodes*.csv");

            foreach (var path in files)
            {
                try
                {
                    var lines = File.ReadAllLines(path);
                    string currentFeatureName = "";
                    string currentModule = "";
                    string currentAddr = "";

                    foreach (var line in lines)
                    {
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        var parts = SplitCsvLine(line);
                        if (parts.Count < 3) continue;

                        // Detected Header?
                        if (parts[0].Trim().StartsWith("Feature Name") || parts[0].Trim().StartsWith("Module")) continue;

                        string name = "";
                        string module = "";
                        string address = "";
                        string d1 = "", d2 = "", d3 = "";
                        string notes = "";

                        // Heuristic to detect format
                        // Format 1: Name, Module, Address, D1, D2, D3, Notes
                        // Format 2: Module, Address, D1, D2, D3, Name, Notes
                        
                        bool isFormat2 = false;
                        // Check if Col 1 is Address (Format 2)
                        if (Regex.IsMatch(parts[1].Trim(), @"^[0-9A-F]{3}-[0-9A-F]{2}-[0-9A-F]{2}"))
                        {
                            isFormat2 = true;
                        }

                        if (isFormat2) // Format 2
                        {
                            module = parts[0].Trim();
                            address = parts[1].Trim();
                            d1 = parts.Count > 2 ? parts[2].Trim() : "";
                            d2 = parts.Count > 3 ? parts[3].Trim() : "";
                            d3 = parts.Count > 4 ? parts[4].Trim() : "";
                            name = parts.Count > 5 ? parts[5].Trim() : "";
                            notes = parts.Count > 6 ? parts[6].Trim() : "";

                            // Handle blanks (copy previous)
                            if (string.IsNullOrWhiteSpace(module)) module = currentModule;
                            else currentModule = module;

                            if (string.IsNullOrWhiteSpace(address)) address = currentAddr;
                            else currentAddr = address;
                            
                            // Unlike format 1, format 2 name is last. 
                        }
                        else // Format 1
                        {
                            name = parts[0].Trim();
                            // Handle continuation
                            if (string.IsNullOrWhiteSpace(name)) name = currentFeatureName;
                            else currentFeatureName = name;

                            module = parts[1].Trim();
                            address = parts[2].Trim();
                            if (!address.Contains("-")) continue; // Invalid
                            
                            d1 = parts.Count > 3 ? parts[3].Trim() : "";
                            d2 = parts.Count > 4 ? parts[4].Trim() : "";
                            d3 = parts.Count > 5 ? parts[5].Trim() : "";
                            notes = parts.Count > 6 ? parts[6].Trim() : "";
                        }

                        var feature = new CommonFeature
                        {
                            Name = name,
                            Module = module,
                            Address = address,
                            Data1Mask = d1.Replace("*", "x"),
                            Data2Mask = d2.Replace("*", "x"),
                            Data3Mask = d3.Replace("*", "x"),
                            Notes = notes
                        };

                        if (feature.Address.Contains("-"))
                        {
                             Features.Add(feature);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading " + path + ": " + ex.Message);
                }
            }
            _loaded = true;
        }

        private static List<string> SplitCsvLine(string line)
        {
            // Basic regex for CSV splitting handling quotes
            var result = new List<string>();
            var pattern = @"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)";
            // Actually, manual parser is safer for simple CSV
            bool inQuotes = false;
            string currentValue = "";
            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ',' && !inQuotes)
                {
                    result.Add(currentValue);
                    currentValue = "";
                }
                else
                {
                    currentValue += c;
                }
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
                     // Check masks
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
            // Normalize
            value = value?.Replace(" ", "").Trim() ?? "";
            mask = mask?.Replace(" ", "").Trim() ?? "";

            if (string.IsNullOrEmpty(mask) || mask.ToLower() == "xxxx") return true; // Wildcard or empty
            
            // If value is missing but mask expects something (other than xxxx), fail? 
            // Usually value will be 4 chars. pattern might be x1xx.
            
            if (value.Length != mask.Length) 
            {
                 // Try to be lenient?
                 // If mask is xxxx, it matches anything.
                 // If mask is shorter/longer, assume no match unless empty.
                 return false;
            }

            for (int i = 0; i < mask.Length; i++)
            {
                char m = char.ToLower(mask[i]);
                if (m == 'x' || m == '*') continue; // Wildcard
                if (char.ToLower(value[i]) != m) return false;
            }

            return true;
        }
    }
}
