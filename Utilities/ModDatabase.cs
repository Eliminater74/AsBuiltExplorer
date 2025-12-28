using System;
using System.Collections.Generic;

namespace AsBuiltExplorer
{
    public class ModEntry
    {
        public string Title { get; set; }
        public string Platform { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }

        public override string ToString() => Title;
    }

    public static class ModDatabase
    {
        public static List<ModEntry> Mods { get; private set; } = new List<ModEntry>();

        static ModDatabase() => InitializeData();

        static void InitializeData()
        {
            // 2008 Ford Expedition / GEM / SJB Mods
            var platformExp = "Expedition 2007-2014 (GEM)";

            Mods.Add(new ModEntry
            {
                Title = "Dark Car / Police Mode",
                Platform = platformExp,
                Category = "Coding",
                Description = "Completely disables interior dome lights and puddle lamps when doors are opened. Essential for surveillance or night vision camera work.",
                Instructions = "Module: GEM / SJB\r\n\r\nMethod:\r\n1. Open FORScan.\r\n2. Go to 'Module Configuration' (Plain English) for GEM/SJB.\r\n3. Look for 'Interior Lighting - Door Ajar'.\r\n4. Set to 'Disabled'.\r\n\r\nAlternatively, look for 'Dark Car' or 'Police Mode' in the GEM module (726)."
            });

            Mods.Add(new ModEntry
            {
                Title = "Global Windows (Summer Vent)",
                Platform = platformExp,
                Category = "Coding",
                Description = "Hold the Unlock button on your fob to roll down the front windows before you get in.",
                Instructions = "Module: GEM / SJB\r\nAddress: 726-xx-xx\r\n\r\nNote: Requires specific 'Security Package' GEM.\r\nCheck 'Module Configuration' screen to see if 'Global Open' is an available option to enable."
            });

            Mods.Add(new ModEntry
            {
                Title = "Bambi Mode (High Beams + Fogs)",
                Platform = platformExp,
                Category = "Hardware Mod",
                Description = "Keeps fog lights ON when High Beams are active. Maximum visibility on dark backroads.",
                Instructions = "The 2008 Reality: You cannot code this on a 2008. It is hardwired in the fuse box.\r\n\r\nThe Mod:\r\n1. Locate Relay 201 (Fog Lamps) in the passenger kick panel.\r\n2. Bend the leg for Pin 2 (Ground Control) so it doesn't plug in.\r\n3. Solder a wire to the bent leg.\r\n4. Ground that wire to the chassis.\r\n\r\nThis bypasses the computer's 'cut' signal."
            });

            Mods.Add(new ModEntry
            {
                Title = "Seatbelt Minder Disable",
                Platform = platformExp,
                Category = "Cheat Code",
                Description = "Permanently silences the chime without cutting wires.",
                Instructions = "Sequence:\r\n1. Ignition ON (Engine Off).\r\n2. Wait 1 min for Seatbelt Light to turn OFF.\r\n3. Buckle -> Unbuckle (3 times).\r\n4. Wait for Light to turn ON.\r\n5. Buckle -> Unbuckle (1 time).\r\n6. Confirmation: Light will flash 4 times."
            });

            Mods.Add(new ModEntry
            {
                Title = "Daytime Running Lights (DRL)",
                Platform = platformExp,
                Category = "Coding",
                Description = "Runs your headlights (at 80% power) or turn signals whenever the truck is in Drive.",
                Instructions = "Module: GEM / SJB\r\n\r\nFeature: 'Daytime Running Lamps'\r\n\r\nOptions: You can often switch between 'Low Beams', 'Turn Signals', or 'Fog Lights' by changing the country code (e.g. setting to 'Canada')."
            });

            Mods.Add(new ModEntry
            {
                Title = "Instrument Cluster Engineering Mode",
                Platform = platformExp,
                Category = "Cheat Code",
                Description = "Hidden diagnostic menu showing real-time digital speed, RPM, battery voltage, and raw sensor data.",
                Instructions = "How to activate:\r\n1. Hold the 'Setup' (or 'Reset' on base XLT) button on the dash.\r\n2. Turn Ignition to ON (keep holding button).\r\n3. Wait until the screen says 'TEST' or 'ENGINEERING MODE'.\r\n4. Release button.\r\n5. Press button repeatedly to scroll through live data."
            });

            Mods.Add(new ModEntry
            {
                Title = "TPMS Pressure Adjustment",
                Platform = platformExp,
                Category = "Coding",
                Description = "Change the tire pressure warning threshold (e.g. for Load Range E tires).",
                Instructions = "Module: BCM / GEM (726-02-01)\r\n\r\nValues:\r\n23 = 35 PSI\r\n2D = 45 PSI\r\n32 = 50 PSI"
            });
        }
    }
}
