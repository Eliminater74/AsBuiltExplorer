using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AsBuiltExplorer
{
    public static class SQLiteHelper
    {
        private static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.db");
        private static string _connectionString = $"Data Source={_dbPath};Version=3;";

        public static void Initialize()
        {
            // Self-heal: If file exists but is 0 bytes (corrupt from crash), delete it.
            if (File.Exists(_dbPath) && new FileInfo(_dbPath).Length == 0)
            {
                File.Delete(_dbPath);
            }

            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            // Always ensure schema exists
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    CREATE TABLE IF NOT EXISTS Vehicles (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FriendlyName TEXT,
                        VIN TEXT,
                        FilePath TEXT,
                        FileContent TEXT
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
    }
}
