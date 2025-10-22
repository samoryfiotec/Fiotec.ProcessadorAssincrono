# ProcessadorAssincrono

Este repositório implementa uma aplicação .NET 8 baseada em **Clean Architecture**, com foco em **processamento assíncrono em lote** utilizando `BackgroundService` e `Channel<Guid>`, com persistência no **SQL Server** via **Dapper**.

---

### Badges

![GitHub repo size](https://img.shields.io/github/repo-size/samoryfiotec/Fiotec.ProcessadorAssincrono?label=RepoSize&color=brown&style=flat&suffix=KB)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/) 
[![Dapper](https://img.shields.io/badge/Dapper-Library-007ACC?style=flat-square)](https://github.com/DapperLib/Dapper) 
[![SQL Server](https://img.shields.io/badge/SQL%20Server-Server-CC2927?style=flat-square&logo=microsoftsqlserver&logoColor=white)](https://www.microsoft.com/sql-server) 
[![BackgroundService](https://img.shields.io/badge/BackgroundService-Hosted-0078D4?style=flat-square)](https://learn.microsoft.com/dotnet/core/extensions/background-services) 
[![Channel<Guid>](https://img.shields.io/badge/Channel-%3CGuid%3E-00ABA9?style=flat-square)](https://learn.microsoft.com/dotnet/standard/parallel-programming/channels) 
[![Minimal APIs](https://img.shields.io/badge/Minimal_APIs-.NET-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/fundamentals/minimal-apis) 
[![xUnit](https://img.shields.io/badge/xUnit-Tests-512BD4?style=flat-square&logo=xunit&logoColor=white)](https://xunit.net/) 
[![Moq](https://img.shields.io/badge/Moq-Mocking-9B4F96?style=flat-square)](https://github.com/moq)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow?style=flat-square)](./LICENSE)

---

## Estrutura de Projetos

```markdown
ProcessadorAssincrono/
├── ProcessadorAssincrono.API           → Minimal APIs e configuração
├── ProcessadorAssincrono.Application   → Interfaces e contratos
├── ProcessadorAssincrono.Domain        → Entidades de negócio
├── ProcessadorAssincrono.Infrastructure→ Implementações (Dapper, BackgroundService)
├── ProcessadorAssincrono.Tests         → Testes unitários com xUnit e Moq
```

---

## Tecnologias Utilizadas

- .NET 8
- Dapper
- SQL Server
- BackgroundService
- `Channel<Guid>`
- Minimal APIs
- xUnit + Moq

---

## Componentes Principais

### `BackgroundService` com `Channel<Guid>`

Permite enfileirar IDs de requisições para processamento em segundo plano, desacoplando a chamada HTTP da lógica de negócio.

### `AprovacaoService` com Dapper

Realiza a atualização da entidade `Requisicao` no banco SQL Server, marcando como aprovada.

### Minimal API

Expõe o endpoint `/aprovar-em-lote` para enfileirar múltiplas requisições.

---

## Criação e configuração do SQL Server

### Crie um banco de dados SQL Server 2022 via Docker

```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=SenhaForte123!" \
-p 1433:1433 --name dbprocessador \
-v sqlvolume:/var/opt/mssql \
-d mcr.microsoft.com/mssql/server:2022-latest
```

SA_PASSWORD: Defina uma senha forte para o usuário sa. (Pode utilizar o SenhaForte123! que é um padrão)
-p 1433:1433: Expõe a porta padrão do SQL Server.

### Abra com o SQL Server Management Studio e crie o banco `Processador`

```bash
CREATE DATABASE Processador;
```

### Crie a tabela `Requisicoes`:

```sql
CREATE TABLE Requisicoes (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Aprovada BIT NOT NULL,
    DataSolicitacao DATETIME NOT NULL
);
```

### Licença

Este projeto está licenciado sob a Licença [MIT](./LICENSE)