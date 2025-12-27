# AsBuiltExplorer

![Visitors](https://api.visitorbadge.io/api/visitors?path=Eliminater74/AsBuiltExplorer&label=VISITORS&countColor=%23263759)

**AsBuiltExplorer** is a powerful utility for analyzing, comparing, and modifying Ford As-Built data (`.ab` / `.abt`). Designed for enthusiasts and professionals, it helps identify feature bits, calculate checksums, and explore vehicle configuration modules.

## Features

*   **Comparison Tool**: Load up to 4 As-Built files to quickly spot differences.
*   **Module Browser**: View module data in Hex, Binary, and ASCII.
*   **Checksum Calculator**: Automatically calculate checksums for modified blocks.
*   **"Holy Grail" Database**: Built-in SQLite database containing thousands of known feature codes (Tire Size, TPMS, Drive Modes, etc.).
*   **Deducer**: Experimental feature to deduce which bit controls a specific feature by comparing VINs with/without that feature.
*   **Modern UI**: Clean interface with left-aligned navigation tabs for easy access.

## Custom CSV Import

We support a **CustomCSV** folder for importing your own definitions!

1.  Navigate to the `CustomCSV` folder in the application directory.
2.  Place any `.csv` file containing feature codes.
3.  **Format**: `FeatureName, Module, Address, Data1Mask, Data2Mask, Data3Mask, Notes`
4.  Launch the application. It will automatically import your new codes into the `Definitions.db` database.
5.  Imported files are automatically renamed to `.bak` to indicate success.

## Requirements

*   Windows OS
*   .NET Framework 4.8

## Usage

1.  **Load Files**: Click "Browse" to select your As-Built files.
2.  **View Changes**: Differences are highlighted.
3.  **View Code Library**: Click the "View Code Library" button to search the known codes database.

## Credits

**Developed By Eliminater74**

*   Special thanks to the Forscan community.
