using System;
using System.Collections.Generic;
using AsBuiltExplorer;

namespace AsBuiltExplorer.Utilities
{
    // Interface for Vehicle Generation Strategies
    public interface IVehicleStrategy
    {
        string GetModuleName(string address);
        string GetValidationType(string address); // Future: Summation vs CRC
    }

    // Factory to select strategy
    public static class VehicleStrategyFactory
    {
        public static IVehicleStrategy GetStrategy(int modelYear)
        {
            if (modelYear >= 2011)
            {
                return new ModernStrategy();
            }
            else
            {
                // Default to Legacy for older vehicles (2000-2010)
                return new LegacyStrategy();
            }
        }
    }

    // Legacy Architecture (Pre-2011) - e.g. 2008 Explorer
    // 726 = GEM (Generic Electronic Module) / SJB (Smart Junction Box)
    public class LegacyStrategy : IVehicleStrategy
    {
        public string GetModuleName(string address)
        {
            if (string.IsNullOrEmpty(address) || address.Length < 3) return "";
            var prefix = address.Substring(0, 3);

            switch (prefix)
            {
                case "726": return "GEM/SJB"; // Smart Junction Box
                case "720": return "Cluster (IC)"; // Instrument Cluster
                case "727": return "ACM (Audio)"; // Audio Control Module
                case "737": return "HVAC"; // Heating Ventilation Air Conditioning
                case "760": return "ABS"; // Anti-Lock Brake System
                case "7E0": return "PCM"; // Powertrain Control Module
                default:
                    // Fallback to generic database
                    var dbName = ModuleDatabase.GetModuleName(address);
                    return string.IsNullOrEmpty(dbName) ? "Unknown" : dbName;
            }
        }

        public string GetValidationType(string address)
        {
            // Almost everything in this era is Summation
            return "Summation";
        }
    }

    // Modern Architecture (CGEA 1.2 / 1.3) - e.g. 2015 F-150, 2018 Mustang
    // 726 = BCM (Body Control Module)
    public class ModernStrategy : IVehicleStrategy
    {
        public string GetModuleName(string address)
        {
            if (string.IsNullOrEmpty(address) || address.Length < 3) return "";
            var prefix = address.Substring(0, 3);

            switch (prefix)
            {
                case "726": return "BCM (Body)"; // Body Control Module
                case "720": return "IPC (Cluster)"; // Instrument Panel Cluster
                case "7D0": return "APIM (Sync)"; // Accessory Protocol Interface Module
                case "727": return "ACM (Audio)"; // Audio Control Module
                case "730": return "PSCM"; // Power Steering Control Module
                case "760": return "ABS"; // Anti-Lock Brake System
                case "754": return "TCU"; // Telematics Control Module (Modem)
                case "706": return "IPMA"; // Image Processing Module A (Camera)
                default:
                    // Fallback to generic database
                    var dbName = ModuleDatabase.GetModuleName(address);
                    return string.IsNullOrEmpty(dbName) ? "Unknown" : dbName;
            }
        }

        public string GetValidationType(string address)
        {
            // Mix of Summation and CRC-8/16
            // For now, return Generic, will implement specific logic later
            return "Modern";
        }
    }
}
