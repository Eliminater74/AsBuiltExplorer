using System;
using System.Collections.Generic;

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

        public static List<DecodeResult> Decode(string vin)
        {
            var results = new List<DecodeResult>();
            if (string.IsNullOrEmpty(vin) || vin.Length != 17)
            {
                results.Add(new DecodeResult { Value = vin, Meaning = "Invalid VIN Length", Notes = "Must be 17 characters" });
                return results;
            }

            vin = vin.ToUpper();

            // --- 1-3: WMI (World Manufacturer Identifier) ---
            string wmi = vin.Substring(0, 3);
            string wmiMeaning = "Unknown Manufacturer";
            string make = "Unknown"; // Internal helper for context
            
            // Ford
            if (wmi.StartsWith("1FM")) { wmiMeaning = "Ford Motor Company (MPV)"; make = "Ford"; }
            else if (wmi.StartsWith("1FT")) { wmiMeaning = "Ford Motor Company (Truck)"; make = "Ford"; }
            else if (wmi.StartsWith("1F")) { wmiMeaning = "Ford Motor Company (USA)"; make = "Ford"; }
            // Lincoln
            else if (wmi.StartsWith("1LN")) { wmiMeaning = "Lincoln (Ford Motor Company)"; make = "Lincoln"; }
            else if (wmi.StartsWith("1L")) { wmiMeaning = "Lincoln (USA)"; make = "Lincoln"; }
            // Mercury
            else if (wmi.StartsWith("1ME")) { wmiMeaning = "Mercury (Ford Motor Company)"; make = "Mercury"; }
            else if (wmi.StartsWith("1M")) { wmiMeaning = "Mercury (USA)"; make = "Mercury"; }
            // Canada/Mexico Broad Checks
            else if (wmi.StartsWith("2")) { wmiMeaning = "Ford Canada"; make = "Ford"; }
            else if (wmi.StartsWith("3")) { wmiMeaning = "Ford Mexico"; make = "Ford"; }

            results.Add(new DecodeResult { Position = "1-3", Value = wmi, Meaning = wmiMeaning, Notes = "Manufacturer" });


            // --- 4: Brake/GVWR or Restraint System ---
            char pos4 = vin[3];
            string pos4Meaning = "Brake/GVWR / Restraint System";
            if (pos4 == 'F') pos4Meaning = "7,001-8,000 lbs GVWR (Standard Hydraulic Brakes)";
            else if (pos4 == 'B') pos4Meaning = "5,001-6,000 lbs GVWR";
            else if (pos4 == 'C') pos4Meaning = "6,001-7,000 lbs GVWR";
            else if (pos4 == 'D') pos4Meaning = "7,001-8,000 lbs GVWR";
            else if (pos4 == 'E') pos4Meaning = "8,001-9,000 lbs GVWR";
            else if (pos4 == 'P') pos4Meaning = "Passenger Car: Manual Belts w/ 2 Airbags";
            else if (pos4 == 'R') pos4Meaning = "Passenger Car: Manual Belts w/ 2 Airbags + Side Airbags";
            
            results.Add(new DecodeResult { Position = "4", Value = pos4.ToString(), Meaning = "GVWR Class / Safety", Notes = pos4Meaning });


            // --- 5-7: Model Specific (Body/Series) ---
            char body = vin[4];
            string series = vin.Substring(5, 2);
            
            string bodyMeaning = "Model Specific Body Code";
            string bodyNotes = "";
            string seriesMeaning = "Model Specific Series Code";
            string seriesNotes = "";

            // Dictionary Mapping for known Series codes (Position 6-7)
            var seriesMap = new Dictionary<string, (string Trim, string Drive, string Notes)>
            {
                // Ford Expedition
                { "15", ("XLT", "2WD", "Expedition: Standard Audio, No Power Liftgate.") },
                { "16", ("XLT", "4WD", "Expedition: Heavy Duty / Fleet. Good source for skid plates.") },
                { "17", ("Eddie Bauer / King Ranch", "2WD", "Luxury Trim.") },
                { "18", ("Eddie Bauer / King Ranch", "4WD", "Luxury Trim w/ Subwoofer/Nav.") },
                { "19", ("Limited", "2WD", "Top Tier Luxury. Chrome accents.") },
                { "20", ("Limited", "4WD", "Top Tier Luxury. HD mechanical parts.") },
                // Lincoln Navigator
                { "27", ("Luxury", "2WD", "**PARTS TIP**: Better sound deadening, double-pane glass, console parts.") },
                { "28", ("Luxury", "4WD", "**PARTS TIP**: Air Ride suspension is common.") },
                // Mercury Mountaineer
                { "60", ("Base", "RWD", "Mountaineer Base.") },
                { "70", ("Premier", "AWD", "**PARTS TIP**: Check for 'Audiophile' Subwoofer/Amps.") }
            };

            if (seriesMap.ContainsKey(series))
            {
                var s = seriesMap[series];
                seriesMeaning = $"{s.Trim} ({s.Drive})";
                seriesNotes = s.Notes;
            }
            else
            {
                 // Default Fallbacks if not in specific map
                 if (make == "Ford") seriesMeaning = "Unknown Ford Series"; 
                 else if (make == "Lincoln") seriesMeaning = "Unknown Lincoln Series";
                 else if (make == "Mercury") seriesMeaning = "Unknown Mercury Series";
            }

            // Body Style Heuristics
            if (make == "Ford")
            {
                 if (body == 'U') { bodyMeaning = "Standard / Short Wheelbase"; bodyNotes = "(Expedition) - Doors/Glass unique to SWB."; }
                 else if (body == 'K') { bodyMeaning = "Extended Length (EL/MAX)"; bodyNotes = "(Expedition EL) - **PARTS ALERT**: Rear doors/glass are unique to EL."; }
                 else if (body == 'W') { bodyMeaning = "Crew Cab"; bodyNotes = "(F-Series)"; }
                 else if (body == 'X') { bodyMeaning = "SuperCab"; bodyNotes = "(F-Series)"; }
                 else if (body == 'F') { bodyMeaning = "Regular Cab"; bodyNotes = "(F-Series)"; }
            }
            else if (make == "Lincoln")
            {
                 if (body == 'L') { bodyMeaning = "Standard Length"; bodyNotes = "(Navigator) - Matches Ford 'U'."; }
                 else if (body == 'J') { bodyMeaning = "Extended Length (L)"; bodyNotes = "(Navigator L) - Matches Ford 'K'. Unique Rear Doors."; }
            }
             else if (make == "Mercury")
            {
                 if (body == 'M') { bodyMeaning = "MPV"; bodyNotes = "(Mountaineer) - Twin to Explorer."; }
            }

            results.Add(new DecodeResult { Position = "5", Value = body.ToString(), Meaning = "Body Style", Notes = $"{bodyMeaning} {bodyNotes}" });
            results.Add(new DecodeResult { Position = "6-7", Value = series, Meaning = "Series / Trim", Notes = $"{seriesMeaning} {seriesNotes}" });


            // --- 8: Engine ---
            char engine = vin[7];
            string engineMeaning = "Unknown Engine";
            // Common Ford Family Engines
            if (engine == '5') engineMeaning = "5.4L 3V Triton V8";
            else if (engine == '8') engineMeaning = "4.6L 2V V8 (Base F-150)"; // Added from verified Python logic
            else if (engine == 'W') engineMeaning = "4.6L 2V-3V V8";
            else if (engine == 'V') engineMeaning = "5.4L 4V DOHC V8 (Navigator / Shelby)";
            // Add specific note for Mercury/Lincoln interchange if needed
            
            results.Add(new DecodeResult { Position = "8", Value = engine.ToString(), Meaning = "Engine", Notes = engineMeaning });


            // --- 9: Check Digit ---
            results.Add(new DecodeResult { Position = "9", Value = vin[8].ToString(), Meaning = "Check Digit", Notes = "Math Code for verification." });


            // --- 10: Year ---
            char year = vin[9];
            string yearStr = GetYear(year);
            results.Add(new DecodeResult { Position = "10", Value = year.ToString(), Meaning = "Model Year", Notes = yearStr });


            // --- 11: Plant ---
            char plant = vin[10];
            string plantMeaning = "Unknown Plant";
            switch(plant)
            {
                case 'L': plantMeaning = "Michigan Truck (Wayne, MI) - Expedition/Navigator"; break;
                case 'F': plantMeaning = "Dearborn, MI - F-Series / Mustang"; break;
                case 'K': plantMeaning = "Kansas City, MO (Claycomo) - F-Series / Escape"; break;
                case 'E': plantMeaning = "Kentucky Truck (Louisville, KY) - Super Duty"; break;
                case 'G': plantMeaning = "Chicago, IL - Taurus / Explorer"; break;
                case 'R': plantMeaning = "Flat Rock (AAI), MI - Mustang / Fusion"; break;
                case 'W': plantMeaning = "Wayne Stamping (Wayne, MI) - Focus"; break;
                case 'H': plantMeaning = "Lorain, OH - Econoline (Older)"; break;
                case 'B': plantMeaning = "Oakville, Ontario - Edge / Flex"; break;
                case '5': plantMeaning = "Flat Rock, MI (Mazda/AutoAlliance)"; break;
                case 'N': plantMeaning = "Chicago, IL"; break;
                case 'M': plantMeaning = "Cuautitlan, Mexico"; break;
                case 'U': plantMeaning = "Louisville, KY - Explorer / Mountaineer"; break;
            }
            results.Add(new DecodeResult { Position = "11", Value = plant.ToString(), Meaning = "Assembly Plant", Notes = plantMeaning });


            // --- 12-17: Sequence ---
            results.Add(new DecodeResult { Position = "12-17", Value = vin.Substring(11), Meaning = "Production Sequence", Notes = "Serial Number" });

            return results;
        }

        private static string GetYear(char c)
        {
            // Standard VIN Year Code (recycles every 30 years)
            // L, M, N, P, R, S, T, V, W, X, Y, 1, 2, 3, 4, 5, 6, 7, 8, 9, A, B, C, D, E, F, G, H, J, K
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
                case 'A': return "1980 / 2010"; // Context usually obvious
                case 'B': return "1981 / 2011";
                case 'C': return "1982 / 2012";
                case 'D': return "1983 / 2013";
                case 'E': return "1984 / 2014";
                case 'F': return "1985 / 2015";
                case 'G': return "1986 / 2016";
                case 'H': return "1987 / 2017";
                case 'J': return "1988 / 2018";
                case 'K': return "1989 / 2019";
                case 'L': return "1990 / 2020";
                case 'M': return "1991 / 2021";
                case 'N': return "1992 / 2022";
                case 'P': return "1993 / 2023";
                case 'R': return "1994 / 2024";
                case 'S': return "1995 / 2025";
                case 'T': return "1996 / 2026";
                default: return "Unknown";
            }
        }
    }
}
