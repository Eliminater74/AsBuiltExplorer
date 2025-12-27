using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace AsBuiltExplorer
{
    public class VehicleEntry
    {
        public string FriendlyName { get; set; }
        public string VIN { get; set; }
        public string FilePath { get; set; }
        public string FileContent { get; set; } // Store the actual data

        public override string ToString()
        {
            if (string.IsNullOrEmpty(VIN))
                return FriendlyName;
            return $"{FriendlyName}  ({VIN})";
        }
    }

    public static class VehicleDatabase
    {
        private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.xml");
        public static List<VehicleEntry> Entries { get; private set; } = new List<VehicleEntry>();

        public static void Load()
        {
            try
            {
                if (File.Exists(_dbPath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<VehicleEntry>));
                    using (FileStream fs = new FileStream(_dbPath, FileMode.Open))
                    {
                        Entries = (List<VehicleEntry>)serializer.Deserialize(fs);
                    }
                }
            }
            catch (Exception)
            {
                // Silently fail or log? For now, just ensure list is empty on fail to prevent crash
                Entries = new List<VehicleEntry>();
            }
        }

        public static void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<VehicleEntry>));
                using (TextWriter writer = new StreamWriter(_dbPath))
                {
                    serializer.Serialize(writer, Entries);
                }
            }
            catch (Exception)
            {
                // Handle save error
            }
        }

        public static void AddEntry(string name, string vin, string path)
        {
            string content = "";
            try 
            {
                if(File.Exists(path)) content = File.ReadAllText(path).Trim();
            }
            catch {}

            Entries.Add(new VehicleEntry { FriendlyName = name, VIN = vin, FilePath = path, FileContent = content });
            Save();
        }

        public static void DeleteEntry(VehicleEntry entry)
        {
            if (Entries.Contains(entry))
            {
                Entries.Remove(entry);
                Save();
            }
        }
    }
}
