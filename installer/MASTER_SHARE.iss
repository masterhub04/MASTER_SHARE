#define MyAppName "MASTER SHARE"
#define MyAppVersion "0.2.0"
#define MyAppPublisher "Master InfoTec"
#define MyAppExeName "MASTER SHARE.exe"
#define PublishDir "..\\publish"

[Setup]
AppId={{D92A4F80-4C8B-4E45-A6E5-5E1A9F4D73B1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\\MASTER SHARE
DefaultGroupName={#MyAppName}
DisableProgramGroupPage=yes
OutputDir=..\\dist
OutputBaseFilename=MASTER_SHARE_Setup_{#MyAppVersion}_x64
ArchitecturesInstallIn64BitMode=x64
ArchitecturesAllowed=x64
PrivilegesRequired=admin
WizardStyle=modern
Compression=lzma2
SolidCompression=yes
UninstallDisplayIcon={app}\\{#MyAppExeName}
SetupLogging=yes

[Languages]
Name: "brazilianportuguese"; MessagesFile: "compiler:Languages\\BrazilianPortuguese.isl"

[Tasks]
Name: "desktopicon"; Description: "Criar atalho na área de trabalho"; GroupDescription: "Atalhos adicionais:"; Flags: unchecked

[Files]
Source: "{#PublishDir}\\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs createallsubdirs
Source: "..\\runtime\\MicrosoftEdgeWebView2Setup.exe"; DestDir: "{tmp}"; Flags: deleteafterinstall

[Icons]
Name: "{autoprograms}\\MASTER SHARE"; Filename: "{app}\\{#MyAppExeName}"
Name: "{autodesktop}\\MASTER SHARE"; Filename: "{app}\\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{tmp}\\MicrosoftEdgeWebView2Setup.exe"; Parameters: "/silent /install"; StatusMsg: "Verificando o Microsoft Edge WebView2 Runtime..."; Flags: runhidden waituntilterminated
Filename: "{app}\\{#MyAppExeName}"; Description: "Abrir MASTER SHARE"; Flags: postinstall nowait skipifsilent

[UninstallDelete]
Type: filesandordirs; Name: "{app}\\HTML"
