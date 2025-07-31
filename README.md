# SeguradoraSystem

Sistema de teste técnico baseado em arquitetura de microserviços para gerenciamento de propostas de seguro e sua contratação.

---

## 📚 Visão Geral

Este repositório contém dois microserviços independentes que se comunicam entre si:

- **PropostaService**: Responsável pela criação, listagem e atualização de status das propostas de seguro.
- **ContratacaoService**: Responsável por contratar uma proposta aprovada e armazenar os dados da contratação.

A arquitetura segue o modelo **Hexagonal (Ports & Adapters)** com boas práticas de **DDD**, **SOLID**, **Clean Code** e **Testes Automatizados**.

---

## 🧱 Estrutura do Projeto

```
SeguradoraSystem/
├── PropostaService/
│   ├── PropostaService.API
│   ├── PropostaService.Application
│   ├── PropostaService.Domain
│   ├── PropostaService.Infrastructure
│   ├── PropostaService.Tests
│   └── PropostaService.IntegrationTests
├── ContratacaoService/
│   ├── ContratacaoService.API
│   ├── ContratacaoService.Application
│   ├── ContratacaoService.Domain
│   ├── ContratacaoService.Infrastructure
│   ├── ContratacaoService.Tests
│   └── ContratacaoService.IntegrationTests
├── docker-compose.yml
└── SeguradoraSystem.sln
```

### Migrations e Banco de Dados

O projeto utiliza **Entity Framework Core** com **migrations** para versionamento do banco de dados.  
Ao iniciar os serviços, o banco de dados é criado automaticamente com base nas migrations existentes.

Caso queira aplicar as migrations manualmente, utilize:

```bash
dotnet ef database update --project PropostaService.Infrastructure
dotnet ef database update --project ContratacaoService.Infrastructure


---

## 🚀 Como Executar com Docker

1. Certifique-se de ter o Docker instalado.
2. Execute o comando:

```bash
docker-compose up --build
```

3. Os microserviços estarão disponíveis em:
   - PropostaService: `http://localhost:5001`
   - ContratacaoService: `http://localhost:5002`

---

## 🔍 Endpoints

### PropostaService

- `POST /api/Proposta` – Criar proposta
- `GET /api/Proposta` – Listar propostas
- `GET /api/Proposta/{id}` – Obter proposta por ID
- `PATCH /api/Proposta` – Atualizar status da proposta

### ContratacaoService

- `POST /api/Contratacao` – Realizar contratação de proposta aprovada

---

## 🧪 Testes

Cada microserviço possui dois projetos de testes:

- `*.Tests`: Testes unitários
- `*.IntegrationTests`: Testes de integração com `WebApplicationFactory`

Execute os testes com:

```bash
dotnet test
```

---

## 🛠 Tecnologias Utilizadas

- .NET 8
- C#
- SQL Server
- Entity Framework Core
- xUnit + FluentAssertions
- Docker + Docker Compose
- Arquitetura Hexagonal
- Clean Architecture + DDD + SOLID

---

## 📄 Licença

Projeto com finalidade de avaliação técnica. Livre para uso educacional.
