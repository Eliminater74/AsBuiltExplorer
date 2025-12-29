# AsBuiltExplorer Toolkit

![HitCounter](https://hits.seeyoufarm.com/api/count/incr/badge.svg?url=https%3A%2F%2Fgithub.com%2FEliminater74%2FAsBuiltExplorer&count_bg=%2379C83D&title_bg=%23555555&icon=&icon_color=%23E7E7E7&title=Visitors&edge_flat=false)
[![Downloads](https://img.shields.io/github/downloads/Eliminater74/AsBuiltExplorer/total?color=blue&label=Downloads)](https://github.com/Eliminater74/AsBuiltExplorer/releases)
**AsBuiltExplorer** is a comprehensive toolkit for analyzing, comparing, and modifying Ford As-Built data (`.ab` / `.abt`). Now powered by a centralized **SQLite Vehicle Database**, it helps enthusiasts and professionals organize their fleet, identify hidden features using offline libraries, and verify modifications with generation-aware intelligence.

---

## 🚀 Key Features

### 📚 Intelligent Vehicle Database
*   **Centralized Storage**: All your vehicle files are indexed in a local `vehicles.db` SQLite database.
*   **Auto-Import**: Simply drop files into the `AsBuiltData` folder, and the app automatically imports and decodes them on startup.
*   **Scan Folder**: Manually import existing collections from any directory.
*   **Smart Decoding**: Automatically determines Model Year, Drive Type, and Engine using the **NHTSA vPIC API** (Online) or internal logic.

### 🕵️ Offline Analysis "The Holy Grail"
*   **Library Matching**: Analyzes your specific vehicle against a massive internal library of known "Common Codes" (Features).
*   **Auto-Labeling**: Automatically tags your vehicle with features like "Hill Descent Control", "Bambi Mode", or "Global Windows" if the bits match.
*   **Deducer**: A powerful reverse-engineering tool that uses your database to find which bits control specific features by comparing multiple vehicles.

### ⚖️ Professional Comparison Tool
*   **High-Contrast UI**: Redesigned for readability with clear color coding for up to 4 files side-by-side.
*   **Feature Translation**: Automatically translates cryptic Hex values into human-readable descriptions (e.g., `726-12-01: xxxx-01xx-xxxx` -> "Daytime Running Lights: Enabled") using the master definition library.
*   **Smart Era Logic**: Intelligently switches between **Legacy (<2011)** and **Modern (2011+)** definitions based on the VIN year.

### 🧮 Advanced Calculators
*   **Checksum Verification**: 
    *   **Legacy Summation**: Standard module validation.
    *   **CRC-8 (SAE J1850)**: Modern vehicle communication checksums.
    *   **CRC-16 (CCITT-FALSE)**: Extended checksum validation.
*   **Real-time Calculation**: Instant feedback as you type.

### 🛠️ Data Management
*   **Edit Definitions**: Right-click to edit feature names or masks directly in the app.
*   **Import/Export**: Share your findings! Export your definitions to CSV or import community files to expand your library.
*   **Polyglot CSV Support**: intelligently parses various community CSV formats (Livnitup, 2GFusions, etc.).

---
## 📸 Gallery

![Main Interface](Resources/Screenshots/AsBuiltExplorerToolKit%201.png)
![Compare Tab](Resources/Screenshots/AsBuiltExplorerToolKit%202.png)
![Analysis](Resources/Screenshots/AsBuiltExplorerToolKit%203.png)
![Deducer](Resources/Screenshots/AsBuiltExplorerToolKit%204.png)
![Calculators](Resources/Screenshots/AsBuiltExplorerToolKit%205.png)
![Settings](Resources/Screenshots/AsBuiltExplorerToolKit%206.png)
![Vehicle Database](Resources/Screenshots/AsBuiltExplorerToolKit%207.png)
![Mods Tab](Resources/Screenshots/AsBuiltExplorerToolKit%208.png)
![About](Resources/Screenshots/AsBuiltExplorerToolKit%209%20.png)

---

## 📂 Magic Folders

The application uses specific folders to streamline your workflow:

### 1. `AsBuiltData` (Auto-Import)
*   **Location**: `[AppDirectory]\AsBuiltData\`
*   **Function**: Recursively scans and imports new vehicles on startup.
*   **Usage**: Create folders like `\Ford\Explorer\2020\` and drop your `.ab` files there.

### 2. `CustomCSV` (Definitions Importer)
*   **Location**: `[AppDirectory]\CustomCSV\`
*   **Function**: Imports custom feature definitions into the main database.
*   **Usage**: Drop any `.csv` file here. It will be imported into `definitions.db` and renamed to `.bak`.

---

## 🔮 Roadmap

### ✅ Completed
- [x] **SQLite Migration**: Moved from flat files to a robust relational database.
- [x] **Compare Tab Refactor**: High-contrast theme and Feature Decoding.
- [x] **Offline Analysis**: "Analyze against Library" feature.
- [x] **NHTSA Integration**: Online VIN decoding.
- [x] **Checksum Calculators**: Added CRC-8 and CRC-16 support.
- [x] **CSV Engine**: Polyglot importer for various community formats.

### 🚧 In Progress / Planned
- [ ] **Cloud Sync**: Sync your verified definitions with a community server.
- [ ] **Advanced Filtering**: Filter the database by specific byte values (e.g. "Find all F-150s where 726-01-01 is 01").
- [ ] **Hex-to-Decimal Converter**: Simple utility for quick conversions.
- [ ] **Module Map Visualization**: Graphical representation of module locations.

---

## Requirements

*   Windows OS (10/11)
*   .NET Framework 4.8

---

## Credits

**Developed By Eliminater74**

*   Special thanks to the **Forscan** community for their tireless research and tireless documentation which makes tools like this possible.
