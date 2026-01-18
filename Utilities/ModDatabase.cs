using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Text;

namespace AsBuiltExplorer
{
    public class ModEntry
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int IsUserMod { get; set; }

        public override string ToString() => Title;
    }

    public static class ModDatabase
    {
        public static List<ModEntry> Mods { get; private set; } = new List<ModEntry>();

        static ModDatabase() => Load();

        public static void Load()
        {
            Mods.Clear();
            SQLiteHelper.Initialize();

            using (var conn = SQLiteHelper.GetConnection())
            {
                // Check if empty, if so seed
                using (var cmdCount = new SQLiteCommand("SELECT COUNT(*) FROM Mods", conn))
                {
                    var count = (long)cmdCount.ExecuteScalar();
                    if (count == 0) InitializeData(conn);
                }

                var sql = "SELECT * FROM Mods ORDER BY Platform, Title";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        try
                        {
                            Mods.Add(new ModEntry
                            {
                                ID = Convert.ToInt32(reader["ID"]),
                                Title = reader["Title"].ToString(),
                                Platform = reader["Platform"].ToString(),
                                Category = reader["Category"].ToString(),
                                Description = reader["Description"].ToString(),
                                Instructions = reader["Instructions"].ToString(),
                                IsUserMod = Convert.ToInt32(reader["IsUserMod"])
                            });
                        }
                        catch { }
                    }
                }
            }
        }

        public static void Add(ModEntry mod)
        {
            using (var conn = SQLiteHelper.GetConnection())
            {
                var sql = "INSERT INTO Mods (Title, Platform, Category, Description, Instructions, IsUserMod) VALUES (@Title, @Platform, @Category, @Description, @Instructions, 1)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", mod.Title);
                    cmd.Parameters.AddWithValue("@Platform", mod.Platform);
                    cmd.Parameters.AddWithValue("@Category", mod.Category);
                    cmd.Parameters.AddWithValue("@Description", mod.Description);
                    cmd.Parameters.AddWithValue("@Instructions", mod.Instructions);
                    cmd.ExecuteNonQuery();
                }
            }
            Load();
        }

        public static void ExportToCSV(string path)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Title,Platform,Category,Description,Instructions");

            foreach (var m in Mods)
            {
                // Simple CSV escaping
                var line = $"\"{Escape(m.Title)}\",\"{Escape(m.Platform)}\",\"{Escape(m.Category)}\",\"{Escape(m.Description)}\",\"{Escape(m.Instructions)}\"";
                sb.AppendLine(line);
            }

            File.WriteAllText(path, sb.ToString());
        }

        public static int ImportFromCSV(string path)
        {
            if (!File.Exists(path)) return 0;

            var lines = File.ReadAllLines(path);
            int count = 0;

            using (var conn = SQLiteHelper.GetConnection())
            {
                using (var transaction = conn.BeginTransaction())
                {
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("Title,Platform")) continue; // Skip Header

                        // Very basic CSV parsing (assumes quotes)
                        var parts = SplitCSV(line);
                        if (parts.Count >= 5)
                        {
                            // Check duplicates based on Title + Platform
                            var title = parts[0];
                            var platform = parts[1];

                            var checkSql = "SELECT COUNT(*) FROM Mods WHERE Title = @Title AND Platform = @Platform";
                            using (var cmdCheck = new SQLiteCommand(checkSql, conn))
                            {
                                cmdCheck.Parameters.AddWithValue("@Title", title);
                                cmdCheck.Parameters.AddWithValue("@Platform", platform);
                                if ((long)cmdCheck.ExecuteScalar() > 0) continue;
                            }

                            var sql = "INSERT INTO Mods (Title, Platform, Category, Description, Instructions, IsUserMod) VALUES (@Title, @Platform, @Category, @Description, @Instructions, 1)";
                            using (var cmd = new SQLiteCommand(sql, conn))
                            {
                                cmd.Parameters.AddWithValue("@Title", title);
                                cmd.Parameters.AddWithValue("@Platform", platform);
                                cmd.Parameters.AddWithValue("@Category", parts[2]);
                                cmd.Parameters.AddWithValue("@Description", parts[3]);
                                cmd.Parameters.AddWithValue("@Instructions", parts[4]);
                                cmd.ExecuteNonQuery();
                                count++;
                            }
                        }
                    }
                    transaction.Commit();
                }
            }
            Load();
            return count;
        }

        private static string Escape(string s)
        {
            if (s == null) return "";
            return s.Replace("\"", "\"\"");
        }

        private static List<string> SplitCSV(string line)
        {
            var list = new List<string>();
            bool inQuotes = false;
            var sb = new StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];
                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"') // Escaped quote
                    {
                        sb.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    list.Add(sb.ToString());
                    sb.Clear();
                }
                else
                {
                    sb.Append(c);
                }
            }
            list.Add(sb.ToString());
            return list;
        }

        private static void InitializeData(SQLiteConnection conn)
        {
            // Seed Data
            var platformExp = "Expedition 2007-2014 (GEM)";
            var seedMods = new List<ModEntry>();

            seedMods.Add(new ModEntry
            {
                Title = "Dark Car / Police Mode",
                Platform = platformExp,
                Category = "Coding",
                Description = "Completely disables interior dome lights and puddle lamps when doors are opened. Essential for surveillance or night vision camera work.",
                Instructions = "Module: GEM / SJB\r\n\r\nMethod:\r\n1. Open FORScan.\r\n2. Go to 'Module Configuration' (Plain English) for GEM/SJB.\r\n3. Look for 'Interior Lighting - Door Ajar'.\r\n4. Set to 'Disabled'.\r\n\r\nAlternatively, look for 'Dark Car' or 'Police Mode' in the GEM module (726)."
            });

            seedMods.Add(new ModEntry
            {
                Title = "Global Windows (Summer Vent)",
                Platform = platformExp,
                Category = "Coding",
                Description = "Hold the Unlock button on your fob to roll down the front windows before you get in.",
                Instructions = "Module: GEM / SJB\r\nAddress: 726-xx-xx\r\n\r\nNote: Requires specific 'Security Package' GEM.\r\nCheck 'Module Configuration' screen to see if 'Global Open' is an available option to enable.\r\n\r\nWARNING: HARDWARE MISMATCH RISK\r\nIF Module = GEM (2007-2014) AND 'Perimeter Alarm' was Factory Disabled THEN Global Windows cannot be enabled via software.\r\nREASON: Missing internal relays and Hood/Door sensor wiring. Enabling Alarm will cause 'Double Honk' errors and Battery Drain issues."
            });

            seedMods.Add(new ModEntry
            {
                Title = "Bambi Mode (High Beams + Fogs)",
                Platform = platformExp,
                Category = "Hardware Mod",
                Description = "Keeps fog lights ON when High Beams are active. Maximum visibility on dark backroads.",
                Instructions = "The 2008 Reality: You cannot code this on a 2008. It is hardwired in the fuse box.\r\n\r\nThe Mod:\r\n1. Locate Relay 201 (Fog Lamps) in the passenger kick panel.\r\n2. Bend the leg for Pin 2 (Ground Control) so it doesn't plug in.\r\n3. Solder a wire to the bent leg.\r\n4. Ground that wire to the chassis.\r\n\r\nThis bypasses the computer's 'cut' signal."
            });

            seedMods.Add(new ModEntry
            {
                Title = "Seatbelt Minder Disable",
                Platform = platformExp,
                Category = "Cheat Code",
                Description = "Permanently silences the chime without cutting wires.",
                Instructions = "Sequence:\r\n1. Ignition ON (Engine Off).\r\n2. Wait 1 min for Seatbelt Light to turn OFF.\r\n3. Buckle -> Unbuckle (3 times).\r\n4. Wait for Light to turn ON.\r\n5. Buckle -> Unbuckle (1 time).\r\n6. Confirmation: Light will flash 4 times."
            });

            seedMods.Add(new ModEntry
            {
                Title = "Daytime Running Lights (DRL)",
                Platform = platformExp,
                Category = "Coding",
                Description = "Runs your headlights (at 80% power) or turn signals whenever the truck is in Drive.",
                Instructions = "Module: GEM / SJB\r\n\r\nFeature: 'Daytime Running Lamps'\r\n\r\nOptions: You can often switch between 'Low Beams', 'Turn Signals', or 'Fog Lights' by changing the country code (e.g. setting to 'Canada')."
            });

            seedMods.Add(new ModEntry
            {
                Title = "Instrument Cluster Engineering Mode",
                Platform = platformExp,
                Category = "Cheat Code",
                Description = "Hidden diagnostic menu showing real-time digital speed, RPM, battery voltage, and raw sensor data.",
                Instructions = "How to activate:\r\n1. Hold the 'Setup' (or 'Reset' on base XLT) button on the dash.\r\n2. Turn Ignition to ON (keep holding button).\r\n3. Wait until the screen says 'TEST' or 'ENGINEERING MODE'.\r\n4. Release button.\r\n5. Press button repeatedly to scroll through live data."
            });

            seedMods.Add(new ModEntry
            {
                Title = "TPMS Pressure Adjustment",
                Platform = platformExp,
                Category = "Coding",
                Description = "Change the tire pressure warning threshold (e.g. for Load Range E tires).",
                Instructions = "Module: BCM / GEM (726-02-01)\r\n\r\nValues:\r\n23 = 35 PSI\r\n2D = 45 PSI\r\n32 = 50 PSI"
            });

            foreach (var mod in seedMods)
            {
                var sql = "INSERT INTO Mods (Title, Platform, Category, Description, Instructions, IsUserMod) VALUES (@Title, @Platform, @Category, @Description, @Instructions, 0)";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", mod.Title);
                    cmd.Parameters.AddWithValue("@Platform", mod.Platform);
                    cmd.Parameters.AddWithValue("@Category", mod.Category);
                    cmd.Parameters.AddWithValue("@Description", mod.Description);
                    cmd.Parameters.AddWithValue("@Instructions", mod.Instructions);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
