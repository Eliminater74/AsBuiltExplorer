using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AsBuiltExplorer
{
    public static class SQLiteHelper
    {
        static string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "vehicles.db");
        static string _connectionString = $"Data Source={_dbPath};Version=3;";

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
                var sql = @"
                    CREATE TABLE IF NOT EXISTS Vehicles (
                        ID INTEGER PRIMARY KEY AUTOINCREMENT,
                        FriendlyName TEXT,
                        VIN TEXT,
                        FilePath TEXT,
                        FileContent TEXT,
                        Year TEXT,
                        Make TEXT,
                        Model TEXT,
                        Features TEXT
                    )";

                using (var cmd = new SQLiteCommand(sql, conn))
                    cmd.ExecuteNonQuery();
                

                // Migration: Add columns if they don't exist (for existing DBs)
                try { using (var cmd = new SQLiteCommand("ALTER TABLE Vehicles ADD COLUMN Year TEXT", conn)) cmd.ExecuteNonQuery();
 } catch { }

                try { using (var cmd = new SQLiteCommand("ALTER TABLE Vehicles ADD COLUMN Make TEXT", conn)) cmd.ExecuteNonQuery();
 } catch { }

                try { using (var cmd = new SQLiteCommand("ALTER TABLE Vehicles ADD COLUMN Model TEXT", conn)) cmd.ExecuteNonQuery();
 } catch { }

                try { using (var cmd = new SQLiteCommand("ALTER TABLE Vehicles ADD COLUMN Features TEXT", conn)) cmd.ExecuteNonQuery();
 } catch { }
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