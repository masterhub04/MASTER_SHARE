# MASTER SHARE 0.1.0

Primeira base do aplicativo Windows, em WPF/.NET 8.

## Objetivo desta versão
- Estrutura visual inspirada na referência aprovada.
- Janela personalizada com controles Windows.
- Sidebar com salas selecionáveis.
- Área de transmissão com estado vazio real (sem vídeo fictício).
- Controles locais de microfone, compartilhamento, sair, opções e tela cheia.
- Componentes separados para facilitar a evolução.

## Requisitos
- Windows 10 ou Windows 11
- .NET 8 SDK (Windows)

## Executar
Abra `MASTER_SHARE.csproj` no Visual Studio 2022 ou execute no PowerShell:

```powershell
dotnet restore
dotnet run
```

## Estado honesto das funcionalidades
Esta versão é o esqueleto funcional da interface. A seleção de salas, janela, tela cheia e alternância visual do microfone funcionam localmente. Captura de tela, áudio remoto, salas online, WebRTC, NVENC, AMD AMF, H.264 por software, bitrate e estatísticas ainda não estão implementados; serão adicionados nas próximas etapas.

## Arquitetura planejada
- UI: WPF
- Captura Windows: Windows Graphics Capture
- Transporte em tempo real: WebRTC
- Codificadores: NVIDIA NVENC, AMD AMF e H.264 por software
- Áudio: WASAPI + Opus
