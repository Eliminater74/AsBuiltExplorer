using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AsBuiltExplorer
{
    public class VehicleEntry
    {
        public int ID { get; set; }
        public string FriendlyName { get; set; }
        public string VIN { get; set; }
        public string FilePath { get; set; }
        public string FileContent { get; set; }
        
        // New Columns
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(VIN))
                return FriendlyName;
            return $"{FriendlyName}  ({VIN})";
        }
    }

    public static class VehicleDatabase
    {
        public static List<VehicleEntry> Entries { get; private set; } = new List<VehicleEntry>();

        public static void Load()
        {
            try
            {
                SQLiteHelper.Initialize(); // Ensure DB exists
                MigrateFromXML(); // One-time check

                Entries.Clear();
                bool needsRefinement = false;

                using (var conn = SQLiteHelper.GetConnection())
                {
                    string sql = "SELECT * FROM Vehicles";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var v = new VehicleEntry
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                FriendlyName = reader["FriendlyName"].ToString(),
                                VIN = reader["VIN"].ToString(),
                                FilePath = reader["FilePath"].ToString(),
                                FileContent = reader["FileContent"].ToString()
                            };
                            
                            // Safe Read for new columns
                            try { v.Year = reader["Year"].ToString(); } catch {}
                            try { v.Make = reader["Make"].ToString(); } catch {}
                            try { v.Model = reader["Model"].ToString(); } catch {}

                            // Mark for refinement if VIN exists but data missing
                            if (!string.IsNullOrEmpty(v.VIN) && v.VIN.Length == 17 && 
                                (string.IsNullOrEmpty(v.Year) || string.IsNullOrEmpty(v.Model)))
                            {
                                needsRefinement = true;
                            }

                            Entries.Add(v);
                        }
                    }
                }

                if (needsRefinement) RefineEntries();
            }
            catch
            {
                // Log error?
            }
        }

        private static void RefineEntries()
        {
            // Auto-decode any entries missing data or with generic data
            foreach (var v in Entries)
            {
                // Re-decode if missing or if it has the generic "Series/Trim" bad data
                if (!string.IsNullOrEmpty(v.VIN) && v.VIN.Length == 17 &&
                    (string.IsNullOrEmpty(v.Year) || string.IsNullOrEmpty(v.Model) || v.Model.Contains("Series/Trim") || v.Model.Contains("Unknown")))
                {
                    UpdateVehicleDataFromVIN(v);
                    UpdateEntry(v);
                }
            }
        }

        public static void UpdateVehicleDataFromVIN(VehicleEntry v)
        {
            var results = VINDecoder.Decode(v.VIN);
            
            // Year
            var resYear = results.Find(x => x.Position == "10");
            if (resYear != null) v.Year = resYear.Notes; 

            // Make
            var resMake = results.Find(x => x.Position == "1-3");
            if (resMake != null) 
            {
                if (resMake.Meaning.Contains("Ford")) v.Make = "Ford";
                else if (resMake.Meaning.Contains("Lincoln")) v.Make = "Lincoln";
                else if (resMake.Meaning.Contains("Mercury")) v.Make = "Mercury";
                else v.Make = "Ford"; 
            }

            // Model Logic
            // Check for Modern Series first (Pos 5-7)
            var resModern = results.Find(x => x.Position == "5-7");
            if (resModern != null)
            {
                // Meaning: "Mustang EcoBoost (Base)"
                v.Model = resModern.Meaning;
            }
            else
            {
                // Legacy Logic: Combine Body (Pos 5) + Trim (Pos 6-7)
                var resBody = results.Find(x => x.Position == "5");
                var resTrim = results.Find(x => x.Position == "6-7");
                
                string bodyPart = "";
                string trimPart = "";

                if (resBody != null)
                {
                    // Body Notes often has junk like "(Expedition) - Doors/Glass..."
                    // We want "Expedition" or "Expedition EL"
                    // Simple heuristic: Take the bold part or the first few words?
                    // User's decoding logic put meaningful text in Notes, e.g. "Extended Length (EL/MAX) (Expedition EL)..."
                    // Let's try to grab specific keywords if present, or just use the Notes but truncated.
                    // Actually, my VINDecoder V2 puts: "Standard / Short Wheelbase (Expedition)..."
                    // Let's rely on simple keyword extraction for cleanliness:
                    string n = resBody.Notes;
                    if (n.Contains("Expedition EL")) bodyPart = "Expedition EL";
                    else if (n.Contains("Expedition")) bodyPart = "Expedition";
                    else if (n.Contains("Navigator L")) bodyPart = "Navigator L";
                    else if (n.Contains("Navigator")) bodyPart = "Navigator";
                    else if (n.Contains("Mountaineer")) bodyPart = "Mountaineer";
                    else if (n.Contains("F-Series")) bodyPart = "F-Series";
                    else if (n.Contains("Mustang")) bodyPart = "Mustang";
                    else bodyPart = resBody.Meaning; // Fallback "Body Style" - bad. Use Notes first bit.
                }

                if (resTrim != null)
                {
                    // Notes: "XLT 2WD. This is..."
                    // Split by period to get the first sentence.
                    string n = resTrim.Notes;
                    int dotIndex = n.IndexOf('.');
                    if (dotIndex > 0) trimPart = n.Substring(0, dotIndex);
                    else trimPart = n;
                }
                
                v.Model = $"{bodyPart} {trimPart}".Trim();
            }
        }


        private static void UpdateEntry(VehicleEntry v)
        {
            try
            {
                using (var conn = SQLiteHelper.GetConnection())
                {
                    string sql = "UPDATE Vehicles SET Year = @Year, Make = @Make, Model = @Model WHERE ID = @ID";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Year", v.Year ?? "");
                        cmd.Parameters.AddWithValue("@Make", v.Make ?? "");
                        cmd.Parameters.AddWithValue("@Model", v.Model ?? "");
                        cmd.Parameters.AddWithValue("@ID", v.ID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch {}
        }
        
        
        private static void MigrateFromXML()
        {
            // If XML exists but DB was just created (empty), let's import
            string xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.xml");
            if (File.Exists(xmlPath))
            {
                // Check if DB is empty
                bool isDbEmpty = true;
                using (var conn = SQLiteHelper.GetConnection())
                {
                    using (var cmd = new System.Data.SQLite.SQLiteCommand("SELECT COUNT(*) FROM Vehicles", conn))
                    {
                        long count = (long)cmd.ExecuteScalar();
                        if (count > 0) isDbEmpty = false;
                    }
                }

                if (isDbEmpty)
                {
                    try
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(List<VehicleEntry>));
                        using (FileStream fs = new FileStream(xmlPath, FileMode.Open))
                        {
                            var xmlEntries = (List<VehicleEntry>)serializer.Deserialize(fs);
                            foreach(var v in xmlEntries)
                            {
                                AddEntry(v.FriendlyName, v.VIN, v.FilePath, v.FileContent);
                            }
                        }
                        // Rename XML to indicate migration done
                        File.Move(xmlPath, xmlPath + ".bak");
                    }
                    catch { }
                }
            }
        }

        public static void AddEntry(string name, string vin, string path, string content = null)
        {
            VehicleEntry tempV = new VehicleEntry { VIN = vin };
            
            // Auto-Decode on Add
            if (!string.IsNullOrEmpty(vin) && vin.Length == 17)
            {
                UpdateVehicleDataFromVIN(tempV);
            }

            if(content == null)
            {
                try 
                {
                    if(File.Exists(path)) content = File.ReadAllText(path).Trim();
                }
                catch {}
            }

            using (var conn = SQLiteHelper.GetConnection())
            {
                string sql = "INSERT INTO Vehicles (FriendlyName, VIN, FilePath, FileContent, Year, Make, Model) VALUES (@Name, @Vin, @Path, @Content, @Year, @Make, @Model)";
                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Vin", vin);
                    cmd.Parameters.AddWithValue("@Path", path);
                    cmd.Parameters.AddWithValue("@Content", content ?? "");
                    cmd.Parameters.AddWithValue("@Year", tempV.Year ?? "");
                    cmd.Parameters.AddWithValue("@Make", tempV.Make ?? "");
                    cmd.Parameters.AddWithValue("@Model", tempV.Model ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
            Load(); // Refresh list from DB
        }

        public static void DeleteEntry(VehicleEntry entry)
        {
             using (var conn = SQLiteHelper.GetConnection())
            {
                string sql = "DELETE FROM Vehicles WHERE ID = @ID";
                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", entry.ID);
                    cmd.ExecuteNonQuery();
                }
            }
            Load();
        }

        public static void ClearDatabase()
        {
            using (var conn = SQLiteHelper.GetConnection())
            {
                using (var cmd = new System.Data.SQLite.SQLiteCommand("DELETE FROM Vehicles", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                // Vacuum to reclaim space and reset auto-increment
                using (var cmd = new System.Data.SQLite.SQLiteCommand("VACUUM", conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            Load();
        }
    }
}
