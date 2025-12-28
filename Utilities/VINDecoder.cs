using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AsBuiltExplorer
{
    public class VINDecoder
    {
        public class DecodeResult
        {
            public string Position { get; set; }
            public string Value { get; set; }
            public string Meaning { get; set; }
            public string Notes { get; set; }
        }

        // --- Data Definitions ---

        // Position 1-3: WMI
        private static readonly Dictionary<string, (string Make, string Type, string Country)> WMIData = new Dictionary<string, (string, string, string)>
        {
            { "1FA", ("Ford", "Car", "USA") },
            { "1FM", ("Ford", "Truck/SUV", "USA") },
            { "1FT", ("Ford", "Truck", "USA") },
            { "1FC", ("Ford", "Stripped Chassis", "USA") },
            { "1LN", ("Lincoln", "Car", "USA") }, 
            { "1ME", ("Mercury", "Car", "USA") },
            { "2FA", ("Ford", "Car", "Canada") },
            { "2FM", ("Ford", "Truck/SUV", "Canada") },
            { "3FA", ("Ford", "Car", "Mexico") },
            { "3FT", ("Ford", "Truck", "Mexico") },
            { "NM0", ("Ford", "Truck", "Turkey") },
            { "5L", ("Lincoln", "SUV", "USA") }, 
        };

        // Position 4 (Cars): Restraint
        private static readonly Dictionary<char, string> RestraintCodes = new Dictionary<char, string>
        {
            { 'P', "Active Belts + Front Airbags" },
            { 'H', "Active Belts + Front/Side Airbags" },
            { 'R', "Active Belts + Front/Side Airbags" },
            { 'C', "Convertible + Side Airbags" },
            { '6', "Coupe (S550 Mustang)" } 
        };

        // Position 4 (Trucks): GVWR
        private static readonly Dictionary<char, string> GVWRCodes = new Dictionary<char, string>
        {
            { 'B', "Class B (3,001-4,000 lbs)" },
            { 'C', "Class C (4,001-5,000 lbs)" },
            { 'D', "Class D (5,001-6,000 lbs)" },
            { 'E', "Class E (6,001-7,000 lbs)" },
            { 'F', "Class F (7,001-8,000 lbs)" },
            { 'W', "Class H (9,001-10,000 lbs)" }
        };

        // Method B: Modern Series Codes (Pos 5-7, 3 chars)
        private static readonly Dictionary<string, (string Model, string Trim, string Drive)> ModernSeries = new Dictionary<string, (string, string, string)>
        {
            // Mustang S550
            { "P8T", ("Mustang", "EcoBoost (Base)", "RWD") },
            { "P8U", ("Mustang", "EcoBoost Premium", "RWD") },
            { "P8C", ("Mustang", "GT (Base)", "RWD") },
            { "P8F", ("Mustang", "GT Premium", "RWD") },
            { "P8J", ("Mustang", "Shelby GT350/GT500", "RWD") },
            
            // F-150 Modern (Examples)
            { "W1E", ("F-150", "Crew Cab", "4x4") },
            { "W1C", ("F-150", "Crew Cab", "4x2") },
        };

        // Method A: Legacy Body Codes (Pos 5)
        private static readonly Dictionary<char, string> LegacyBodyCodes = new Dictionary<char, string>
        {
            { 'U', "SUV Short Wheelbase (Expedition/Explorer)" },
            { 'K', "SUV Long Wheelbase (Expedition EL / Navigator L)" },
            { 'F', "Regular Cab" },
            { 'X', "SuperCab" },
            { 'W', "Crew Cab" },
            { 'M', "Mercury Mountaineer" }
        };

        // Method A: Legacy Series Codes (Pos 6-7)
        private static readonly Dictionary<string, string> LegacySeriesCodes = new Dictionary<string, string>
        {
            { "15", "XLT 2WD" },
            { "16", "XLT 4WD" },
            { "17", "Eddie Bauer / King Ranch 2WD" },
            { "18", "Eddie Bauer / King Ranch 4WD" },
            { "19", "Limited 2WD" },
            { "20", "Limited 4WD" },
            { "27", "Lincoln Navigator 2WD" },
            { "28", "Lincoln Navigator 4WD" },
            { "60", "Mountaineer Base RWD" },
            { "70", "Mountaineer Premier AWD" }
        };

        // Position 8: Engine
        private static readonly Dictionary<char, string> EngineCodes = new Dictionary<char, string>
        {
            { '5', "5.4L 3V Triton V8" },
            { 'W', "4.6L V8 (Romeo)" },
            { '6', "4.6L V8 (Windsor)" },
            { '8', "4.6L 2V V8 (Base F-150)" },
            { 'F', "5.0L Coyote V8" },
            { 'T', "3.5L EcoBoost V6" },
            { 'H', "2.3L EcoBoost I4" },
            { 'V', "5.4L 4V DOHC V8 (Navigator / Shelby)" },
            { 'J', "5.2L Voodoo V8 (Shelby)" },
            { '9', "6.7L PowerStroke Diesel" }
        };

        // Standard VIN Year Map (Pos 10)
        private static readonly Dictionary<char, string> YearCodes = new Dictionary<char, string>
        {
            { 'V', "1997" }, { 'W', "1998" }, { 'X', "1999" }, { 'Y', "2000" },
            { '1', "2001" }, { '2', "2002" }, { '3', "2003" }, { '4', "2004" },
            { '5', "2005" }, { '6', "2006" }, { '7', "2007" }, { '8', "2008" },
            { '9', "2009" }, { 'A', "2010" }, { 'B', "2011" }, { 'C', "2012" },
            { 'D', "2013" }, { 'E', "2014" }, { 'F', "2015" }, { 'G', "2016" },
            { 'H', "2017" }, { 'J', "2018" }, { 'K', "2019" }, { 'L', "2020" },
            { 'M', "2021" }, { 'N', "2022" }, { 'P', "2023" }, { 'R', "2024" }
        };

        // Position 11: Plant
        private static readonly Dictionary<char, string> PlantCodes = new Dictionary<char, string>
        {
            { 'F', "Dearborn, MI" },
            { 'K', "Claycomo / Kansas City, MO" },
            { 'L', "Michigan Truck (Wayne, MI)" },
            { 'E', "Kentucky Truck (Louisville, KY)" },
            { 'R', "Hermosillo, Mexico" },
            { '5', "Flat Rock, MI" },
            { 'G', "Chicago, IL" },
            { 'B', "Oakville, Ontario" }
        };

        public static List<DecodeResult> Decode(string vin)
        {
            var results = new List<DecodeResult>();
            if (string.IsNullOrEmpty(vin) || vin.Length != 17)
            {
                results.Add(new DecodeResult { Value = vin, Meaning = "Invalid VIN Length", Notes = "Must be 17 characters" });
                return results;
            }

            vin = vin.ToUpper();

            // --- 1-3: WMI ---
            string wmi = vin.Substring(0, 3);
            string make = "Unknown";
            string type = "Unknown";
            string country = "Unknown";
            
            if (WMIData.ContainsKey(wmi))
            {
                var d = WMIData[wmi];
                make = d.Make;
                type = d.Type;
                country = d.Country;
            }
            results.Add(new DecodeResult { Position = "1-3", Value = wmi, Meaning = $"{make} ({type})", Notes = $"Made in {country}" });

            // --- 4: VDS ---
            char c4 = vin[3];
            string m4 = "Unknown";
            bool isTruck = type.Contains("Truck") || type.Contains("SUV");
            bool isCar = type.Contains("Car");

            if (isCar)
            {
                if (RestraintCodes.ContainsKey(c4)) m4 = RestraintCodes[c4];
                else m4 = "Unknown Restraint Code";
            }
            else 
            {
                if (GVWRCodes.ContainsKey(c4)) m4 = GVWRCodes[c4];
                else m4 = "Unknown GVWR or Brake Code";
            }
            results.Add(new DecodeResult { Position = "4", Value = c4.ToString(), Meaning = isCar ? "Restraint Sys" : "GVWR/Brakes", Notes = m4 });

            // --- 5-7: Model/Series ---
            string s567 = vin.Substring(4, 3);
            if (ModernSeries.ContainsKey(s567))
            {
                var mod = ModernSeries[s567];
                results.Add(new DecodeResult { Position = "5-7", Value = s567, Meaning = $"{mod.Model} {mod.Trim}", Notes = $"Drive: {mod.Drive} (Modern Code)" });
            }
            else
            {
                char c5 = vin[4]; // Body
                string s67 = vin.Substring(5, 2); // Series/Trim

                string bodyStr = LegacyBodyCodes.ContainsKey(c5) ? LegacyBodyCodes[c5] : "Unknown Body";
                string trimStr = LegacySeriesCodes.ContainsKey(s67) ? LegacySeriesCodes[s67] : "Unknown Series";
                
                string inferredModel = "";
                if (bodyStr.Contains("Expedition")) inferredModel = "Expedition";
                else if (bodyStr.Contains("Navigator")) inferredModel = "Navigator";
                else if (bodyStr.Contains("Mountaineer")) inferredModel = "Mountaineer";
                else if (bodyStr.Contains("Cab")) inferredModel = "F-Series";

                results.Add(new DecodeResult { Position = "5", Value = c5.ToString(), Meaning = "Body Style", Notes = bodyStr });
                results.Add(new DecodeResult { Position = "6-7", Value = s67, Meaning = "Series/Trim", Notes = trimStr + (string.IsNullOrEmpty(inferredModel) ? "" : $" ({inferredModel})") });
            }

            // --- 8: Engine ---
            char c8 = vin[7];
            string engineStr = EngineCodes.ContainsKey(c8) ? EngineCodes[c8] : "Unknown Engine";
            results.Add(new DecodeResult { Position = "8", Value = c8.ToString(), Meaning = "Engine", Notes = engineStr });

            // --- 9: Check Digit Verification ---
            char checkDigit = vin[8];
            bool isValid = VerifyCheckDigit(vin);
            results.Add(new DecodeResult { Position = "9", Value = checkDigit.ToString(), Meaning = "Check Digit", Notes = isValid ? "Valid" : "INVALID CHECKSUM" });

            // --- 10: Year ---
            char c10 = vin[9];
            string yearStr = YearCodes.ContainsKey(c10) ? YearCodes[c10] : "Unknown Year";
            results.Add(new DecodeResult { Position = "10", Value = c10.ToString(), Meaning = "Model Year", Notes = yearStr });

            // --- 11: Plant ---
            char c11 = vin[10];
            string plantStr = PlantCodes.ContainsKey(c11) ? PlantCodes[c11] : "Unknown Plant";
            results.Add(new DecodeResult { Position = "11", Value = c11.ToString(), Meaning = "Assembly Plant", Notes = plantStr });

            // --- 12: Estimated Build Date ---
            string buildDateEst = "Unknown";
            string buildDateNotes = "";
            
            // Parse Year
            int modelYear = 0;
            if (int.TryParse(GetYear(c10), out modelYear))
            {
                if (modelYear < 2010)
                {
                    // Legacy Estimation: Sequence Number
                    // Start ~August of previous year (Conservative Estimate)
                    string seqStr = vin.Substring(11);
                    string digits = Regex.Replace(seqStr, "[^0-9]", "");
                    if (int.TryParse(digits, out int seqNum))
                    {
                        // User Formula: 51k = Jan/Feb 2008 (Model 2008)
                        // This implies ~8,500 units/month starting Aug 2007.
                        double monthsAfterAug = (double)seqNum / 8500.0;
                        DateTime startProduction = new DateTime(modelYear - 1, 8, 1); 
                        DateTime estDate = startProduction.AddMonths((int)monthsAfterAug);
                        
                        buildDateEst = estDate.ToString("MMMM yyyy");
                        buildDateNotes = $"Est. based on Sequence #{seqNum}";
                    }
                }
                else
                {
                    // Modern Logic: Pos 12 Month Code (A-M skip I)
                    char monthCode = vin[11]; // Pos 12
                    int month = 0;
                     switch (monthCode)
                    {
                        case 'A': month = 1; break;
                        case 'B': month = 2; break;
                        case 'C': month = 3; break;
                        case 'D': month = 4; break;
                        case 'E': month = 5; break;
                        case 'F': month = 6; break;
                        case 'G': month = 7; break;
                        case 'H': month = 8; break;
                        case 'J': month = 9; break; // Skip I
                        case 'K': month = 10; break;
                        case 'L': month = 11; break;
                        case 'M': month = 12; break;
                    }

                    if (month > 0)
                    {
                        buildDateEst = new DateTime(2000, month, 1).ToString("MMMM");
                        buildDateNotes = $"Month Code '{monthCode}'";
                    }
                }
            }
            results.Add(new DecodeResult { Position = "Calc", Value = "Date", Meaning = "Est. Build Date", Notes = $"{buildDateEst} ({buildDateNotes})" });

            string sequence = vin.Substring(11);


            // --- Window Sticker Link ---
            string stickerUrl = $"https://www.windowsticker.forddirect.com/windowsticker.pdf?vin={vin}";
            results.Add(new DecodeResult { Position = "URL", Value = "Link", Meaning = "Window Sticker", Notes = stickerUrl });

            return results;
        }

        private static bool VerifyCheckDigit(string vin)
        {
            if (vin.Length != 17) return false;
            
            int[] weights = { 8, 7, 6, 5, 4, 3, 2, 10, 0, 9, 8, 7, 6, 5, 4, 3, 2 };
            int sum = 0;

            for (int i = 0; i < 17; i++)
            {
                char c = vin[i];
                int val = 0;
                if (char.IsDigit(c)) val = c - '0';
                else if (c >= 'A' && c <= 'Z')
                {
                    switch (c)
                    {
                        case 'A': case 'J': val = 1; break;
                        case 'B': case 'K': case 'S': val = 2; break;
                        case 'C': case 'L': case 'T': val = 3; break;
                        case 'D': case 'M': case 'U': val = 4; break;
                        case 'E': case 'N': case 'V': val = 5; break;
                        case 'F': case 'W': val = 6; break;
                        case 'G': case 'P': case 'X': val = 7; break;
                        case 'H': case 'Y': val = 8; break;
                        case 'R': case 'Z': val = 9; break;
                        default: return false; // Invalid char (I, O, Q)
                    }
                }
                else return false;

                if (i == 8) continue; // Skip check digit position in sum (weight is 0 anyway)
                
                sum += val * weights[i];
            }

            int remainder = sum % 11;
            char expected = (remainder == 10) ? 'X' : (char)('0' + remainder);

            return vin[8] == expected;
        }
        public static int GetModelYear(string vin)
        {
            if (string.IsNullOrEmpty(vin) || vin.Length != 17) return 0;
            char yearChar = vin.ToUpper()[9];
            
            // Prioritize Modern Years (2010+)
            switch (yearChar)
            {
                case 'A': return 2010;
                case 'B': return 2011;
                case 'C': return 2012;
                case 'D': return 2013;
                case 'E': return 2014;
                case 'F': return 2015;
                case 'G': return 2016;
                case 'H': return 2017;
                case 'J': return 2018;
                case 'K': return 2019;
                case 'L': return 2020;
                case 'M': return 2021;
                case 'N': return 2022;
                case 'P': return 2023;
                case 'R': return 2024;
                case 'S': return 2025;
                case 'T': return 2026;
                
                // Legacy Fallbacks (1980-2009)
                // If not above, check these
                case 'V': return 1997;
                case 'W': return 1998;
                case 'X': return 1999;
                case 'Y': return 2000;
                case '1': return 2001; 
                case '2': return 2002;
                case '3': return 2003;
                case '4': return 2004;
                case '5': return 2005; 
                case '6': return 2006;
                case '7': return 2007; 
                case '8': return 2008; 
                case '9': return 2009;
                
                default: return 2000; // Unknown
            }
        }

        private static string GetYear(char c)
        {
            // Standard VIN Year Code (recycles every 30 years)
            // Keeping existing method for text compatibility
            switch (c)
            {
                case 'V': return "1997";
                case 'W': return "1998";
                case 'X': return "1999";
                case 'Y': return "2000";
                case '1': return "2001";
                case '2': return "2002";
                case '3': return "2003";
                case '4': return "2004";
                case '5': return "2005";
                case '6': return "2006";
                case '7': return "2007";
                case '8': return "2008";
                case '9': return "2009";
                case 'A': return "2010";
                case 'B': return "2011";
                case 'C': return "2012";
                case 'D': return "2013";
                case 'E': return "2014";
                case 'F': return "2015";
                case 'G': return "2016";
                case 'H': return "2017";
                case 'J': return "2018";
                case 'K': return "2019";
                case 'L': return "2020";
                case 'M': return "2021";
                case 'N': return "2022";
                case 'P': return "2023";
                case 'R': return "2024";
                case 'S': return "2025";
                case 'T': return "2026";
                default: return "Unknown";
            }
        }
    }
}
