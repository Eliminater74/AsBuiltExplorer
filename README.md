# AsBuiltExplorer

![Visitors](https://api.visitorbadge.io/api/visitors?path=Eliminater74/AsBuiltExplorer&label=VISITORS&countColor=%23263759)

**AsBuiltExplorer** is a powerful utility for analyzing, comparing, and modifying Ford As-Built data (`.ab` / `.abt`). Designed for enthusiasts and professionals, it helps identify feature bits, calculate checksums, and explore vehicle configuration modules with generation-aware intelligence.

---

## 🚀 Key Features

*   **Mult-Generation Support (New!)**
    *   **Smart VIN Decoding**: Automatically determines the vehicle's Model Year from the VIN.
    *   **Adaptive Strategies**:
        *   **Legacy (<2011)**: Maps addresses like `726` to **GEM/SJB** (Smart Junction Box).
        *   **Modern (2011+)**: Maps addresses like `726` to **BCM** (Body Control Module).
*   **Comparison Tool**: Load up to 4 As-Built files side-by-side to visually spot differences.
*   **Module Browser**: View module data in Hex, Binary, and ASCII with color-coded highlighting.
*   **Checksum Calculator**: Automatically fixes checksums for modified blocks.
*   **"Holy Grail" Database**: Built-in SQLite database containing thousands of known feature codes (Tire Size, TPMS, Drive Modes, etc.).
*   **Deducer**: Experimental feature to deduce which bit controls a specific feature by comparing VINs.

---

## 📂 Magic Folders

The application uses two special folders to make your life easier. Just place your files in them, and the app does the rest!

### 1. `AsBuiltData` (Auto-Import)
*   **Location**: `[AppDirectory]\AsBuiltData\`
*   **Function**: recursively scans this folder on startup to build your Vehicle Database.
*   **How to use**:
    1.  Create folders for your collection (e.g., `AsBuiltData\Ford\F-150\2018\`).
    2.  Drop your `.ab` or `.abt` files anywhere inside.
    3.  Launch AsBuiltExplorer.
    4.  The app will automatically **find, decode, and add** every new file to your searchable Vehicle Database!

### 2. `CustomCSV` (Definitions Importer)
*   **Location**: `[AppDirectory]\CustomCSV\`
*   **Function**: Imports your custom feature definitions into the main database.
*   **How to use**:
    1.  Place any `.csv` file here.
    2.  **Format**: `FeatureName, Module, Address, Data1Mask, Data2Mask, Data3Mask, Notes` (CSV Header optional).
    3.  Launch AsBuiltExplorer.
    4.  Your codes are instantly imported into the `Definitions.db` database.
    5.  Files are renamed to `.bak` to indicate a successful import.

---

## 🔮 Roadmap

We are constantly improving AsBuiltExplorer. Here is what's coming next:

*   **[To Do] "Scan Folder" Button**: A manual button in the Vehicle Database tab to recursively scan *any* folder on your PC for `.ab` files, not just the default `AsBuiltData` folder.
*   **[To Do] Advanced Checksums**: Enhancing the checksum calculator to support CRC-8/CRC-16 algorithms used in newer modules (IPMA, etc.).
*   **[To Do] Cloud Sync**: (Potential) Sync your verified definitions with a community server.

---

## Requirements

*   Windows OS (10/11)
*   .NET Framework 4.8

## Usage

1.  **Comparisons**: Go to the **Compare As-Built** tab and click "Browse" or load files from your Database.
2.  **Modifications**: Use the **Calculators** tab to verify checksums before writing back to the car.
3.  **Research**: Use the **Deducer** tab to reverse-engineer unknown features using your database of VINs.

---

## Credits

**Developed By Eliminater74**

*   Special thanks to the Forscan community for their tireless research and documentation.
