using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace AsBuiltExplorer
{
    public class NHTSAResult
    {
        public string VIN; // Added for Batch Mapping
        public string Make;
        public string Model;
        public string Year;
        public string Trim;
        public string DriveType;
        public string BodyClass;
        public string FuelType;
        public string Series;
    }

    public static class NHTSADecoder
    {
        public static NHTSAResult Decode(string vin)
        {
            // ... existing Decode implementation (kept for single usage if needed, or fallback) ...
            // For simplicity in this diff, I'm keeping the original Decode method but showing it might be refactored or kept.
            // But since I am replacing the file content significantly, I will provide the full updated class.
            try
            {
                 // Use Extended ID for single lookups
                var url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevinextended/{vin}?format=json";
                var json = "";

                using (var client = new WebClient())
                    json = client.DownloadString(url);
                
                if (string.IsNullOrEmpty(json)) return null;

                var res = new NHTSAResult();
                res.VIN = vin;

                var objectMatches = Regex.Matches(json, @"\{[^{}]*\}");
                foreach (Match m in objectMatches)
                {
                    var content = m.Value;
                    var variable = ExtractField(content, "Variable");
                    var value = ExtractField(content, "Value");

                    if (!string.IsNullOrEmpty(variable) && !string.IsNullOrEmpty(value) && value != "null")
                    {
                        switch (variable)
                        {
                            case "Make": res.Make = value; break;
                            case "Model": res.Model = value; break;
                            case "Model Year": res.Year = value; break;
                            case "Trim": res.Trim = value; break;
                            case "Series": res.Series = value; break;
                            case "Drive Type": res.DriveType = value; break;
                            case "Body Class": res.BodyClass = value; break;
                            case "Fuel Type - Primary": res.FuelType = value; break;
                        }
                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("NHTSA Error: " + ex.Message);
                return null;
            }
        }

        public static List<NHTSAResult> DecodeBatch(IEnumerable<string> vins)
        {
            var results = new List<NHTSAResult>();
            var vinList = new List<string>(vins);
            
            // API Limitation: Max 50 VINs per batch
            for (int i = 0; i < vinList.Count; i += 50)
            {
                var count = Math.Min(50, vinList.Count - i);
                var batch = vinList.GetRange(i, count);
                var batchResults = ProcessBatchChunk(batch);
                results.AddRange(batchResults);
            }
            return results;
        }

        private static List<NHTSAResult> ProcessBatchChunk(List<string> chunkVins)
        {
            var output = new List<NHTSAResult>();
            try
            {
                var data = string.Join(";", chunkVins);
                var postData = $"format=json&data={data}";
                var url = "https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVINValuesBatch/";

                string json = "";
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    json = client.UploadString(url, postData);
                }

                // Batch returns "Flat" format: Array of Objects with keys like "Make", "Model", etc.
                // Regex to find objects: { ... }
                // Warning: Nested braces are rare in Flat format, but safe check is needed.
                
                var objectMatches = Regex.Matches(json, @"\{[^{}]*\}");
                foreach (Match m in objectMatches)
                {
                    var content = m.Value;
                    var res = new NHTSAResult();
                    
                    // Flat Format Keys
                    res.VIN = ExtractField(content, "VIN");
                    res.Make = ExtractField(content, "Make");
                    res.Model = ExtractField(content, "Model");
                    res.Year = ExtractField(content, "ModelYear");
                    res.Trim = ExtractField(content, "Trim");
                    res.Series = ExtractField(content, "Series");
                    res.DriveType = ExtractField(content, "DriveType");
                    res.BodyClass = ExtractField(content, "BodyClass");
                    res.FuelType = ExtractField(content, "FuelTypePrimary");

                    if(!string.IsNullOrEmpty(res.VIN)) output.Add(res);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Batch Error: " + ex.Message);
            }
            return output;
        }

        static string ExtractField(string jsonObject, string key)
        {
            // "key": "value" or "key": value
            // Look for "key"\s*:\s*"([^"]*)"
            var match = Regex.Match(jsonObject, $"\"{key}\"\\s*:\\s*\"([^\"]*)\"");
            if (match.Success) return match.Groups[1].Value;

            // Try formatting without quotes if value is number
            match = Regex.Match(jsonObject, $"\"{key}\"\\s*:\\s*([0-9]+)");
            if (match.Success) return match.Groups[1].Value;

            return null;
        }
    }
}
