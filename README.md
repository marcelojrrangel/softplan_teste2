# Softplan API

Esta é uma API para gerenciamento de tarefas, desenvolvida como parte de um teste técnico. Inclui autenticação JWT, versionamento de API, logging estruturado, validação de dados, DTOs, Unit of Work, health checks e mais.

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

4. **Pacotes Adicionados**: FluentValidation, Serilog, JWT Bearer, API Versioning, Health Checks.

## Executando a API

1. Navegue até `Softplan.API` e execute:
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

1. Na raiz da solução (`softplan_teste`), execute:
   ```sh
   dotnet test
   ```

Executa testes unitários (`Softplan.API.UnitTests`) e integração (`Softplan.API.IntegrationTests`).
