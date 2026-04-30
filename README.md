# Checkpoint IOT - Microservices e Mensageria com RabbitMQ

Projeto de exemplo em .NET 9 com arquitetura baseada em mensageria para atender ao checkpoint de Programacao de API com Microservices e RabbitMQ. A solucao foi estruturada em multiplos aplicativos console, simulando dois fluxos independentes com validacao intermediaria.

## Integrantes

- Integrantes do grupo: consultar o arquivo `integrantes.txt`
- Link do repositorio GitHub: https://github.com/letprado/checkpoint-iot.git
- Link do youtube: https://youtu.be/jyI5FSRVt94

## Estrutura da solucao

- `CheckpointIOT.Sender1`: envia mensagens sobre frutas da epoca.
- `CheckpointIOT.Sender2`: envia mensagens com cadastro ficticio de usuarios.
- `CheckpointIOT.Validation`: consome as mensagens dos senders, valida o conteudo e republica apenas o que estiver correto.
- `CheckpointIOT.Receiver1`: recebe as mensagens validadas sobre frutas.
- `CheckpointIOT.Receiver2`: recebe as mensagens validadas sobre usuarios.
- `CheckpointIOT.Shared`: biblioteca compartilhada com contratos de mensagem, topologia RabbitMQ e regras de validacao.

## Topologia RabbitMQ

### Exchanges

- `hortifruti.validation.topic`
- `hortifruti.delivery.topic`

### Queues

- `queue.validation.frutas.epoca`
- `queue.validation.usuarios.cadastro`
- `queue.receiver.frutas.epoca`
- `queue.receiver.usuarios.cadastro`

### Routing keys

- `validation.frutas.epoca`
- `validation.usuarios.cadastro`
- `receiver.frutas.epoca`
- `receiver.usuarios.cadastro`

## Fluxos implementados

### Fluxo 1: frutas da epoca

1. `Sender1` gera uma mensagem com nome da fruta, resumo descritivo e data/hora atual da solicitacao.
2. A mensagem e publicada na exchange `hortifruti.validation.topic` com a routing key `validation.frutas.epoca`.
3. `Validation` consome a fila `queue.validation.frutas.epoca` e valida:
   - nome da fruta obrigatorio;
   - resumo com pelo menos 15 caracteres;
   - data/hora da solicitacao preenchida.
4. Se a mensagem for valida, `Validation` republica para `hortifruti.delivery.topic` usando a routing key `receiver.frutas.epoca`.
5. `Receiver1` consome a fila `queue.receiver.frutas.epoca` e exibe os dados recebidos.

### Fluxo 2: dados de usuario

1. `Sender2` gera uma mensagem ficticia com nome completo, endereco, RG, CPF e data/hora do registro no sistema hortifruti.
2. A mensagem e publicada na exchange `hortifruti.validation.topic` com a routing key `validation.usuarios.cadastro`.
3. `Validation` consome a fila `queue.validation.usuarios.cadastro` e valida:
   - nome completo com pelo menos duas partes;
   - endereco preenchido;
   - RG com 8 a 10 digitos numericos;
   - CPF com 11 digitos numericos;
   - data/hora do cadastro preenchida.
4. Se a mensagem for valida, `Validation` republica para `hortifruti.delivery.topic` usando a routing key `receiver.usuarios.cadastro`.
5. `Receiver2` consome a fila `queue.receiver.usuarios.cadastro` e exibe os dados recebidos.

## Como executar

### 1. Subir o RabbitMQ no Docker Desktop

```bash
docker compose up -d
```

Broker AMQP: `localhost:5672`

Painel de administracao: `http://localhost:15672`

Usuario e senha padrao: `guest` / `guest`

### 2. Restaurar e compilar a solution

```bash
dotnet restore
dotnet build CheckpointIOT.sln
```

### 3. Executar os microsservicos de consumo e validacao

Abra terminais separados e execute:

```bash
dotnet run --project src/CheckpointIOT.Validation
dotnet run --project src/CheckpointIOT.Receiver1
dotnet run --project src/CheckpointIOT.Receiver2
```

### 4. Executar os senders

Em novos terminais:

```bash
dotnet run --project src/CheckpointIOT.Sender1
dotnet run --project src/CheckpointIOT.Sender2
```

Nos consoles dos senders:

- pressione `ENTER` para enviar uma nova mensagem;
- pressione `Q` para encerrar.

## Exemplos de teste

### Teste do Sender1 -> Validation -> Receiver1

1. Inicie `Validation` e `Receiver1`.
2. Execute `Sender1`.
3. Pressione `ENTER` no console do `Sender1`.
4. Verifique no `Validation` a confirmacao da validacao.
5. Verifique no `Receiver1` os dados da fruta exibidos.
6. No painel do RabbitMQ, confirme a existencia das exchanges, filas, connections e channels.

Exemplo esperado no `Sender1`:

```text
[Sender1] Fruta enviada para validacao: Morango | Solicitacao: 28/04/2026 20:45:10
```

Exemplo esperado no `Validation`:

```text
[Validation] Fruta validada e enviada ao Receiver 1: Morango
```

Exemplo esperado no `Receiver1`:

```text
[Receiver1] Mensagem recebida
  Id...........: 7f4a03d9-6280-4e0a-bb2d-f4cf5c84d1d2
  Fruta........: Morango
  Resumo.......: Fruta vermelha aromatica, levemente acida e muito usada em sobremesas.
  Solicitacao..: 28/04/2026 20:45:10 -03:00
```

### Teste do Sender2 -> Validation -> Receiver2

1. Inicie `Validation` e `Receiver2`.
2. Execute `Sender2`.
3. Pressione `ENTER` no console do `Sender2`.
4. Verifique no `Validation` a confirmacao da validacao.
5. Verifique no `Receiver2` os dados do usuario exibidos.
6. No painel do RabbitMQ, confirme a existencia das exchanges, filas, connections e channels.

Exemplo esperado no `Sender2`:

```text
[Sender2] Usuario enviado para validacao: Ana Paula Nogueira | Registro: 28/04/2026 20:46:00
```

Exemplo esperado no `Validation`:

```text
[Validation] Usuario validado e enviado ao Receiver 2: Ana Paula Nogueira
```

Exemplo esperado no `Receiver2`:

```text
[Receiver2] Mensagem recebida
  Id...........: d220f13a-aa8e-4a0a-98dd-c34f5eef3c74
  Nome.........: Ana Paula Nogueira
  Endereco.....: Rua das Jabuticabeiras, 150 - Sao Paulo/SP
  RG...........: 423456781
  CPF..........: 12345678909
  Registro.....: 28/04/2026 20:46:00 -03:00
```
