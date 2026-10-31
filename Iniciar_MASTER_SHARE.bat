@echo off
setlocal
title MASTER SHARE - Inicializador

REM Pasta informada pelo usuario
set "PROJECT_DIR=E:\MASTER_SHARE\MASTER_SHARE"

REM Se o BAT estiver dentro da pasta do projeto, prioriza essa localizacao
if exist "%~dp0MASTER_SHARE.csproj" set "PROJECT_DIR=%~dp0"

if not exist "%PROJECT_DIR%\MASTER_SHARE.csproj" (
    echo [ERRO] Nao encontrei MASTER_SHARE.csproj em:
    echo "%PROJECT_DIR%"
    echo.
    echo Confira se o projeto esta na pasta correta.
    pause
    exit /b 1
)

where dotnet >nul 2>nul
if errorlevel 1 (
    echo [ERRO] .NET SDK nao encontrado.
    echo Instale o .NET 8 SDK e tente novamente.
    pause
    exit /b 1
)

cd /d "%PROJECT_DIR%"
echo Iniciando MASTER SHARE...
echo.
dotnet run --project "%PROJECT_DIR%\MASTER_SHARE.csproj"

if errorlevel 1 (
    echo.
    echo [ERRO] O aplicativo foi encerrado com erro.
    pause
)
endlocal
