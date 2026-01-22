# Stock Quote Alert - Desafio Técnico Inoa

Projeto desenvolvido para monitorar ações da B3 e disparar alertas de compra e venda via e-mail.

# Como Rodar

Pré-requisitos: .NET SDK 8.0 (ou superior).

1. Configuração:
   Primeiro, abra o arquivo 'appsettings.json'.
   Configure os dados do SMTP (ex: Gmail). 
   Se usar Gmail, precisa gerar uma "Senha de App" na conta do Google.

2. Execução:
   Abra o terminal na pasta do projeto e rode o comando passando: ATIVO PREÇO_VENDA PREÇO_COMPRA.
   
   Exemplo:
   dotnet run PETR4 40.50 22.00

   Obs: O sistema aceita tanto ponto quanto vírgula nos preços (40.50 ou 40,50).

---
# Decisões de Arquitetura

Criei uma estrutura que fosse fácil de testar e dar manutenção, separando em diversos arquivos.

- Injeção de Dependência (DI): 
  Em vez de ficar dando "new" nas classes manualmente, usei o container nativo do .NET. Isso deixa o código desacoplado.

- Strategy Pattern (Interfaces):
  Criei interfaces para o serviço de Cotação (IStockService) e de Email (IEmailService). 
  Assim permite trocar a API da Brapi pelo Yahoo Finance, ou trocar o envio de e-mail por SMS/Telegram, a lógica principal do monitor (StockMonitor) não quebra quando houver trocas.

- HTTP:
  Para a API, usei o IHttpClientFactory. Aprendi que instanciar HttpClient direto dentro de um loop infinito pode esgotar os sockets da máquina, então essa abordagem resolve isso e gerencia melhor as conexões.

---
# Features Extras / Facilidades

- Tratamento de Input (Ponto vs Vírgula):
  Percebi nos testes que o terminal pode confundir ponto com separador de milhar ou apenas não reconhece dependendo do idioma do computador. Implementei um parser que normaliza isso na entrada, então o usuário não precisa se preocupar com o formato.

- Monitoramento de Delay:
  Como estou usando a API gratuita da Brapi (que usa dados da B3), existe um delay natural de 15 a 30 minutos nas cotações. Adicionei no log do console a "Hora da Cotação" exata que veio da API. Assim, quem estiver rodando sabe exatamente se o dado é "real-time" ou se tem esse atraso, evitando confusão sobre o motivo do alerta ter (ou não) disparado.

- Organização:
  O código foi separado em pastas (Services, Models, Interfaces) para manter a organização profissional, em vez de deixar tudo misturado no Program.cs.