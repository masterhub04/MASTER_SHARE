$ErrorActionPreference = "Stop"
$root = Split-Path -Parent $PSScriptRoot
$publish = Join-Path $root "publish"
$runtime = Join-Path $root "runtime"
$dist = Join-Path $root "dist"

Write-Host "== MASTER SHARE - Gerador de instalador ==" -ForegroundColor Cyan

$dotnet = Get-Command dotnet -ErrorAction SilentlyContinue
if (-not $dotnet) { throw ".NET 8 SDK não encontrado. Instale o SDK .NET 8." }

$iscc = Get-Command ISCC.exe -ErrorAction SilentlyContinue
if (-not $iscc) { throw "Inno Setup 6 não encontrado. Instale o Inno Setup 6 e tente novamente." }

New-Item -ItemType Directory -Force -Path $publish, $runtime, $dist | Out-Null
Get-ChildItem $publish -Force -ErrorAction SilentlyContinue | Remove-Item -Recurse -Force
Get-ChildItem $dist -Force -ErrorAction SilentlyContinue | Remove-Item -Recurse -Force

Write-Host "Publicando aplicativo Windows x64..." -ForegroundColor Yellow
dotnet publish (Join-Path $root "MASTER_SHARE.csproj") -c Release -r win-x64 --self-contained true -p:PublishSingleFile=false -o $publish
if ($LASTEXITCODE -ne 0) { throw "Falha ao publicar o aplicativo." }

$webViewBootstrapper = Join-Path $runtime "MicrosoftEdgeWebView2Setup.exe"
Write-Host "Baixando instalador oficial do WebView2 Runtime..." -ForegroundColor Yellow
Invoke-WebRequest -Uri "https://go.microsoft.com/fwlink/p/?LinkId=2124703" -OutFile $webViewBootstrapper
if (-not (Test-Path $webViewBootstrapper)) { throw "Não foi possível obter o instalador do WebView2." }

Write-Host "Compilando instalador..." -ForegroundColor Yellow
& $iscc.Source (Join-Path $PSScriptRoot "MASTER_SHARE.iss")
if ($LASTEXITCODE -ne 0) { throw "Falha ao compilar o instalador Inno Setup." }

Write-Host ""
Write-Host "Instalador criado em: $dist" -ForegroundColor Green
Get-ChildItem $dist -Filter "*.exe" | Select-Object FullName, Length
