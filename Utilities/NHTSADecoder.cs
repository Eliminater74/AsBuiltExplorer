using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;

namespace AsBuiltExplorer
{
    public class NHTSAResult
    {
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
            try
            {
                string url = $"https://vpic.nhtsa.dot.gov/api/vehicles/decodevin/{vin}?format=json";
                string json = "";

                using (var client = new WebClient())
                {
                    json = client.DownloadString(url);
                }

                if (string.IsNullOrEmpty(json)) return null;

                var res = new NHTSAResult();

                // Simple Regex Parsing to avoid dependencies
                // Structure: { "Value": "Something", ... "Variable": "Make" ... }
                // We will look for "Variable": "X" and then find the corresponding "Value": "Y" 
                // Since the JSON is a list of objects, we can split by objects.
                
                // Matches objects: { ... }
                // This rough regex finds objects inside the Results array
                var objectMatches = Regex.Matches(json, @"\{[^{}]*\}");

                foreach (Match m in objectMatches)
                {
                    string content = m.Value;
                    
                    // Extract Variable
                    string variable = ExtractField(content, "Variable");
                    string value = ExtractField(content, "Value");

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

        private static string ExtractField(string jsonObject, string key)
        {
            // "key": "value" or "key": value
            // Look for "key"\s*:\s*"([^"]*)"
            var match = Regex.Match(jsonObject, $"\"{key}\"\\s*:\\s*\"([^\"]*)\"");
            if (match.Success) return match.Groups[1].Value;
            
            // Try formatting without quotes if value is number (though API usually returns strings)
            match = Regex.Match(jsonObject, $"\"{key}\"\\s*:\\s*([0-9]+)");
            if (match.Success) return match.Groups[1].Value;

            return null;
        }
    }
}
