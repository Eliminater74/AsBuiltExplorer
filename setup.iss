#define MyAppName "AsBuiltExplorer"
#define MyAppPublisher "Eliminater74"
#define MyAppURL "https://github.com/Eliminater74/AsBuiltExplorer"
#define MyAppExeName "AsBuiltExplorer.exe"

; This can be overridden by command line arg: /DMyAppVersion="1.2.3"
#ifndef MyAppVersion
  #define MyAppVersion "1.0.0"
#endif

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{368755ac-9958-4ed7-871d-9abdfa68218e}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
DisableProgramGroupPage=yes
; Remove the following line to run in administrative install mode (install for all users.)
PrivilegesRequired=lowest
OutputBaseFilename=AsBuiltExplorer_Setup_v{#MyAppVersion}
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "bin\Release\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\*.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\AsBuiltData\*"; DestDir: "{app}\AsBuiltData"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "bin\Release\CustomCSV\*"; DestDir: "{app}\CustomCSV"; Flags: ignoreversion recursesubdirs createallsubdirs
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#MyAppName}}"; Flags: nowait postinstall skipifsilent
