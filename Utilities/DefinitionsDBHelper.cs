using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AsBuiltExplorer
{
    public static class DefinitionsDBHelper
    {
        private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "definitions.db");
        private static string _connectionString = $"Data Source={_dbPath};Version=3;";

        public static void Initialize()
        {
            // Create DB if not exists
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            // Ensure Schema
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS CommonCodes (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FeatureName TEXT,
                        Module TEXT,
                        Address TEXT,
                        Data1Mask TEXT,
                        Data2Mask TEXT,
                        Data3Mask TEXT,
                        Notes TEXT
                    )";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static SQLiteConnection GetConnection()
        {
            var conn = new SQLiteConnection(_connectionString);
            conn.Open();
            return conn;
        }

        public static void AddEntry(string name, string module, string address, string d1, string d2, string d3, string notes)
        {
            using (var conn = GetConnection())
            {
                string query = "INSERT INTO CommonCodes (FeatureName, Module, Address, Data1Mask, Data2Mask, Data3Mask, Notes) VALUES (@name, @mod, @addr, @d1, @d2, @d3, @notes)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@mod", module);
                    cmd.Parameters.AddWithValue("@addr", address);
                    cmd.Parameters.AddWithValue("@d1", d1);
                    cmd.Parameters.AddWithValue("@d2", d2);
                    cmd.Parameters.AddWithValue("@d3", d3);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        
        // Helper to check if DB is empty (candidate for migration)
        public static bool IsEmpty()
        {
            using (var conn = GetConnection())
            {
                 using (var cmd = new SQLiteCommand("SELECT COUNT(*) FROM CommonCodes", conn))
                 {
                     long count = (long)cmd.ExecuteScalar();
                     return count == 0;
                 }
            }
        }

        public static System.Data.DataTable GetAllCodes()
        {
            var dt = new System.Data.DataTable();
            using (var conn = GetConnection())
            {
                using (var cmd = new SQLiteCommand("SELECT * FROM CommonCodes ORDER BY Module, Address", conn))
                {
                    using (var da = new SQLiteDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
