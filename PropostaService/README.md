# PropostaService

Serviço responsável por gerenciar propostas de seguros, permitindo sua criação, consulta e alteração de status.

---

## ✅ Funcionalidades

- Criar uma nova proposta
- Listar todas as propostas
- Consultar uma proposta por ID
- Atualizar o status da proposta (`EmAnalise`, `Aprovada`, `Rejeitada`)

---

## 🧱 Arquitetura

- **Estilo:** Hexagonal (Ports & Adapters)
- **Padrões aplicados:**
  - Clean Architecture
  - DDD (Domain-Driven Design)
  - SOLID
  - Design Patterns (foco em separação de camadas)
- **Banco de Dados:** SQL Server
- **API REST:** exposta com ASP.NET Core (.NET 8)

---

## 🧰 Testes

- **Testes Unitários** com `xUnit` e `FluentAssertions`, organizados por caso de uso.
- **Testes de Integração** utilizando `WebApplicationFactory` e `HttpClient`, organizados em projeto separado (`PropostaService.IntegrationTests`).
- Cobertura de testes para todos os casos de uso principais:
  - Criação
  - Listagem
  - Consulta por ID
  - Atualização de status
  - Casos inválidos (como status inexistente)

---

## 🧰 Organização e Boas Práticas

- Camadas bem definidas:
  - `Domain`, `Application`, `Infrastructure`, `API`
- DTOs, Enums e Repositories organizados em pastas próprias
- Middleware global de tratamento de exceções
- `ServiceExtensions.cs` criado para centralizar e manter clara a injeção de dependências

---

## 📦 Endpoints

| Método | Rota                 | Descrição                     |
| ------ | -------------------- | ----------------------------- |
| POST   | `/api/Proposta`      | Cria uma nova proposta        |
| GET    | `/api/Proposta`      | Lista todas as propostas      |
| GET    | `/api/Proposta/{id}` | Consulta uma proposta por ID  |
| PATCH  | `/api/Proposta`      | Atualiza o status da proposta |

---

## ⚙️ Executando

```bash
# No diretório PropostaService
dotnet build
dotnet ef database update
dotnet run
```

Swagger UI disponível em:

```
https://localhost:7213/swagger
```

---

## 📁 Projeto

- **.NET SDK:** 8.0
- **IDE recomendada:** Visual Studio 2022 ou superior
- **Banco de dados:** SQL Server local
- **Migrations:** configuradas via EF Core

---

## 🤊 Testes

```bash
# Testes unitários
cd PropostaService.Tests
dotnet test

# Testes de integração
cd PropostaService.IntegrationTests
dotnet test
```

## 📌 Observações

- A proposta é identificada por um `Guid Id` e um `int Codigo` incremental.
- Status inicial ao criar uma proposta é `EmAnalise`.
- Apenas propostas com status `Aprovada` podem ser contratadas (validação feita no ContratacaoService).


## 🗺️ Diagrama de Arquitetura

Veja o [arquitetura.txt](./arquitetura.txt) com a representação visual da estrutura hexagonal e endpoints deste microserviço.
