# Softplan API

Esta é uma API para gerenciamento de tarefas, desenvolvida como parte de um teste técnico. Inclui autenticação JWT, versionamento de API, logging estruturado, validação de dados, DTOs, Unit of Work, health checks e mais.

## Arquitetura

O projeto segue a arquitetura hexagonal (ports and adapters) organizada em projetos separados para melhor isolamento e manutenção:

### Estrutura de Projetos
- **Softplan.API.Domain**: Entidades de negócio e interfaces do domínio (Task, ITaskRepository, IUnitOfWork, etc.).
- **Softplan.API.Application**: Camada de aplicação com Commands, Queries, Handlers e DTOs usando MediatR para CQRS.
- **Softplan.API.Infrastructure**: Implementações concretas dos repositórios, contexto de banco de dados e Unit of Work.
- **Softplan.API.Presentation**: Controllers, validações e mapeamentos para a API REST.

### Dependências
- Domain: Não depende de outros projetos
- Application: Depende de Domain
- Infrastructure: Depende de Domain
- Presentation: Depende de Application, Domain e Infrastructure

### CQRS com MediatR

- **Commands**: CreateTaskCommand, CompleteTaskCommand, DeleteTaskCommand
- **Queries**: GetTasksByUserQuery
- **Handlers**: Implementam a lógica de negócio para cada comando/query
- O controlador injeta IMediator e envia mensagens ao invés de chamar use cases diretamente.

## Pré-requisitos

*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) ou superior.

## Configuração

1. **Banco de Dados**: Usa `InMemoryDatabase` para desenvolvimento. Para produção, configure SQL Server/PostgreSQL em `appsettings.json`.

2. **JWT**: Configurado em `appsettings.json`:
   ```json
   {
     "Jwt": {
       "Key": "YourSuperSecretKeyHere12345678901234567890",
       "Issuer": "Softplan.API",
       "Audience": "Softplan.API"
     }
   }
   ```
   Altere a chave para produção.

3. **Logging**: Serilog configurado para console e arquivo (`logs/log-.txt`).

4. **Pacotes Adicionados**:
   - **Application**: MediatR, FluentValidation, Microsoft.Extensions.Logging.Abstractions
   - **Infrastructure**: Microsoft.EntityFrameworkCore.InMemory
   - **Presentation**: Serilog.AspNetCore, Microsoft.AspNetCore.Authentication.JwtBearer, Microsoft.AspNetCore.Mvc.Versioning, Microsoft.AspNetCore.OpenApi, Microsoft.Extensions.Diagnostics.HealthChecks, Swashbuckle.AspNetCore

## Executando a API

1. Navegue até `Softplan.API.Presentation` e execute:
   ```sh
   dotnet run
   ```

2. Endereços:
   *   HTTPS: `https://localhost:7083`
   *   HTTP: `http://localhost:5029`

3. **Swagger**: `https://localhost:7083/swagger` (inclui autenticação JWT).

4. **Health Check**: `https://localhost:7083/health`.

## Autenticação

1. Faça login via POST `/api/v1.0/auth/login`:
   ```json
   {
     "username": "admin",
     "password": "password"
   }
   ```
   Receba o token JWT.

2. Use o token no cabeçalho `Authorization: Bearer <token>` para endpoints protegidos.

## Endpoints (v1.0)

- **Auth**: POST `/api/v1.0/auth/login`
- **Tasks**: 
  - POST `/api/v1.0/tasks` (criar tarefa)
  - GET `/api/v1.0/tasks/{userId}` (listar tarefas)
  - PUT `/api/v1.0/tasks/{id}/complete` (completar tarefa)
  - DELETE `/api/v1.0/tasks/{id}` (deletar tarefa)

## Executando os Testes

1. Na raiz da solução, execute:
   ```sh
   dotnet test
   ```

Executa testes unitários (`Softplan.API.UnitTests`) e integração (`Softplan.API.IntegrationTests`).
