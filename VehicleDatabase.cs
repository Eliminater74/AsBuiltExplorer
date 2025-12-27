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
                using (var conn = SQLiteHelper.GetConnection())
                {
                    string sql = "SELECT * FROM Vehicles";
                    using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entries.Add(new VehicleEntry
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                FriendlyName = reader["FriendlyName"].ToString(),
                                VIN = reader["VIN"].ToString(),
                                FilePath = reader["FilePath"].ToString(),
                                FileContent = reader["FileContent"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error?
            }
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
                string sql = "INSERT INTO Vehicles (FriendlyName, VIN, FilePath, FileContent) VALUES (@Name, @Vin, @Path, @Content)";
                using (var cmd = new System.Data.SQLite.SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Vin", vin);
                    cmd.Parameters.AddWithValue("@Path", path);
                    cmd.Parameters.AddWithValue("@Content", content ?? "");
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
    }
}
