# ProcessadorAssincrono

Este repositório implementa uma aplicação .NET 8 baseada em **Clean Architecture**, com foco em **processamento assíncrono em lote** utilizando `BackgroundService` e `Channel<Guid>`, com persistência no **SQL Server** via **Dapper**.

---

## Estrutura de Projetos

```
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
- Channel<Guid>
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

## Configuração do SQL Server

1. Crie um banco de dados SQL Server (pode ser via Docker).
2. Crie a tabela `Requisicoes`:

```sql
CREATE TABLE Requisicoes (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    Aprovada BIT NOT NULL,
    DataSolicitacao DATETIME NOT NULL
);