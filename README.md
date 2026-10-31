# MASTER SHARE 0.2.0

Aplicativo Windows de salas de voz e compartilhamento de tela, com interface WPF + WebView2 e mídia em tempo real via LiveKit.

## O que esta versão já implementa

- Interface de canais de texto e voz.
- Nome de usuário salvo localmente.
- Conexão a uma sala LiveKit usando endereço do servidor e token JWT.
- Lista de participantes conectados e atualização de entrada/saída.
- Recepção de áudio e vídeo compartilhados por outros participantes.
- Ativação/desativação do microfone.
- Publicação e encerramento do compartilhamento de tela pela sala LiveKit.
- Verificação de atualizações pelo GitHub.

## Requisitos

- Windows 10 ou Windows 11.
- .NET 8 SDK para executar pelo código-fonte.
- Conexão com a internet.
- Um projeto LiveKit Cloud ou um servidor LiveKit próprio.
- Um token JWT individual para cada participante, autorizado para a mesma sala.

## Executar pelo código-fonte

Na pasta do projeto:

```powershell
dotnet restore
dotnet run
```

## Configurar uma chamada LiveKit

1. Crie um projeto no LiveKit Cloud ou configure um servidor LiveKit acessível pelos dois computadores.
2. Copie o endereço WebSocket do projeto/servidor. Normalmente começa com `wss://`.
3. Gere um token JWT para cada pessoa. Os tokens devem autorizar a mesma sala e permitir `roomJoin`, `canPublish` e `canSubscribe`.
4. Abra o MASTER SHARE nos dois computadores e selecione o mesmo canal de voz (Geral, Jogos ou Privado).
5. Clique em **Conectar ao canal** e informe o endereço LiveKit e o token individual.
6. Depois de entrar, use o botão do microfone para ativar o áudio e o botão de tela para compartilhar.

O endereço do servidor fica salvo no computador. O token não é salvo. Cada participante deve usar identidade/token próprios, mas os dois tokens precisam apontar para o mesmo nome de sala.

## Segurança dos tokens

A chave secreta da API LiveKit nunca deve ser colocada no aplicativo, no repositório ou distribuída aos usuários. Os tokens JWT devem ser gerados por um backend confiável usando a API key e API secret mantidas em variáveis de ambiente. Não use tokens de desenvolvimento em uma implantação pública.

## Limitações conhecidas desta etapa

- O aplicativo ainda exige que o token JWT seja fornecido manualmente na janela de conexão.
- A geração automática de tokens por um backend do MASTER SHARE será adicionada em uma etapa seguinte.
- O chat de texto continua local; as mensagens ainda não são transmitidas entre computadores.
- Não há autenticação/contas MASTER SHARE nem gerenciamento próprio de convites.
- A interface de compartilhamento e a conexão precisam ser validadas em testes com dois computadores Windows reais; esta alteração no repositório ainda não foi compilada/testada no ambiente Windows.

## Arquitetura

- Interface: WPF + WebView2.
- SDK de mídia: LiveKit Client SDK para JavaScript.
- Transporte de áudio/vídeo: WebRTC com LiveKit SFU.
- Autorização: JWT emitido no backend usando LiveKit API key/secret.
