# ContratacaoService

Serviço responsável por realizar a contratação de propostas aprovadas. Comunica-se com o PropostaService para validar o status da proposta antes de efetivar a contratação.

---

## ✅ Funcionalidades

- Contratar uma proposta (somente se estiver **Aprovada**)
- Armazenar contratações com:
  - ID da proposta
  - Data da contratação
- Validar se a proposta já foi contratada anteriormente
- Expor API REST para integração externa

---

## 🧱 Arquitetura

- **Estilo:** Hexagonal (Ports & Adapters)
- **Padrões aplicados:**
  - Clean Architecture
  - DDD (Domain-Driven Design)
  - SOLID
  - Design Patterns (separação de responsabilidades)
- **Comunicação com PropostaService:** HTTP REST (com validação de certificado ignorada apenas nos testes)
- **Banco de Dados:** SQL Server

---

## 🧰 Endpoints

| Método | Rota               | Descrição                             |
| ------ | ------------------ | ------------------------------------- |
| POST   | `/api/Contratacao` | Realiza a contratação de uma proposta |

**Requisição:**

```json
{
  "propostaId": "GUID"
}
```

**Resposta esperada em caso de sucesso:**

```text
Proposta contratada com sucesso.
```

---

## 🧱 Testes

- **Testes Unitários** com `xUnit`, `Moq` e `FluentAssertions`
  - Casos de uso validados:
    - Contratar proposta aprovada
    - Impedir contratação duplicada
    - Impedir contratação se a proposta não for encontrada
    - Impedir contratação se proposta não estiver aprovada
- **Testes de Integração**:
  - Realizam chamadas reais ao PropostaService usando `HttpClient`
  - Validação de fluxo completo da contratação
  - Cobertura de todos os fluxos e mensagens de erro esperadas

---

## 📁 Projeto

- **.NET SDK:** 8.0
- **IDE recomendada:** Visual Studio 2022 ou superior
- **Banco de dados:** SQL Server local (mesmo servidor usado pelo PropostaService)
- **Injeção de dependências:** organizada no `ServiceExtensions`
- **Middlewares:** tratamento global de exceções

---

## ⚙️ Executando

```bash
# No diretório ContratacaoService
 dotnet build
 dotnet ef database update
 dotnet run
```

Swagger UI disponível em:

```
https://localhost:7213/swagger
```

Obs: verifique se não está usando a mesma porta do PropostaService.

---

## 🚫 Validações

- Proposta não encontrada => HTTP 422 com mensagem "Proposta não encontrada."
- Proposta não aprovada => HTTP 422 com mensagem "A proposta não está aprovada."
- Proposta já contratada => HTTP 422 com mensagem descritiva

---

## 🎡 Observações

- A comunicação com o PropostaService é feita através de chamadas REST GET e PATCH
- Testes de integração utilizam dados reais e isolados com `Guid.NewGuid()`
- Contratações são persistidas com o ID da proposta e a data atual

---

## 📊 Testes

```bash
# Testes unitários
cd ContratacaoService.Tests
 dotnet test

# Testes de integração
cd ContratacaoService.IntegrationTests
 dotnet test
```

## 🗺️ Diagrama de Arquitetura

Veja o [arquitetura.txt](./arquitetura.txt) com a representação visual da estrutura hexagonal e endpoints deste microserviço.
