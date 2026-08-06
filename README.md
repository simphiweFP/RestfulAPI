# ShippingApi 🚚

[![.NET 7](https://img.shields.io/badge/.NET-7.0-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![CI](https://img.shields.io/badge/CI-GitHub_Actions-2088FF?logo=githubactions)](.github/workflows/dotnet-ci.yml)
[![Tests](https://img.shields.io/badge/tests-unit_%2B_integration-success)](#testing)

ShippingApi is a **.NET 7 REST API** for managing **addresses, drivers, and orders**. It is a portfolio-ready backend sample with automated testing, Docker support, CI, health checks, structured logging, pagination, filtering, and Swagger documentation.

## Problem it solves

ShippingApi solves the problem of managing core shipping workflow data in one place. It provides a centralized REST API for creating, updating, validating, filtering, and retrieving driver, address, and order records so other applications and frontend clients can work with shipping data consistently.

In practical terms, it helps replace scattered or manual shipping data handling with a single backend service that supports:

- driver management
- delivery address management
- order creation and tracking
- paginated and filtered queries
- documented, testable API contracts

---

## ✨ Highlights

- ASP.NET Core Web API on .NET 7
- Entity Framework Core with SQL Server for runtime persistence
- EF Core InMemory database for integration tests
- JWT/Azure AD authentication via Microsoft Identity Web
- Global exception handling with safe `ProblemDetails` responses
- Swagger/OpenAPI with bearer authentication support
- Bounded pagination and filtering for orders, drivers, and addresses
- Structured logging and health checks
- Unit and integration test projects
- Dockerized runtime image for local use
- GitHub Actions CI pipeline

---

## 🧱 Architecture

The solution is split into three projects:

- `ShippingApi` — the API application
- `ShippingApi.UnitTests` — unit tests for services, controllers, mapping, and validation
- `ShippingApi.IntegrationTests` — end-to-end HTTP tests using `WebApplicationFactory`

### Key folders

- `Controllers` — HTTP endpoints
- `Data` — EF Core `DbContext`, migrations, and Unit of Work
- `Dtos` — request and response contracts
- `Middleware` — centralized exception handling
- `Models` — domain entities and paging models
- `Services` — application services and logging decorator
- `UseCase` — repositories and abstractions

---

## 📦 Requirements

- .NET SDK 7.0
- SQL Server for the API runtime
- Docker Desktop (optional, for local container execution)

---

## 🚀 Run locally

```powershell
dotnet restore .\ShippingApi.sln
dotnet build .\ShippingApi.sln
dotnet run --project .\ShippingApi\ShippingApi.csproj
```

---

## ⚙️ Configuration

Required outside tests:

- `ConnectionStrings:DefaultConnection`
- `AzureAd`

Provide values through `appsettings.Development.json`, .NET user secrets, or environment variables. Pending EF Core migrations are applied during normal startup; tests use an isolated in-memory database.

---

## 📚 API documentation

In **Development** mode, Swagger is available at `https://localhost:7283/swagger` or the configured local HTTPS URL.

---

## 🔌 Endpoints

### Health

- `GET /health`

### Orders

- `GET /api/orders` — paginated and filterable
- `GET /api/orders/{id}`
- `POST /api/orders`
- `PUT /api/orders/{id}`
- `DELETE /api/orders/{id}`

Query parameters: `pageNumber`, `pageSize` (maximum 100), `userId`, `minTotalAmount`, `maxTotalAmount`.

```http
GET /api/orders?pageNumber=1&pageSize=10&userId=101
```

### Drivers

- `GET /api/drivers` — paginated and filterable
- `GET /api/drivers/{id}`
- `POST /api/drivers`
- `PUT /api/drivers/{id}`
- `DELETE /api/drivers/{id}`

Query parameters: `pageNumber`, `pageSize` (maximum 100), `team`, `search`.

```http
GET /api/drivers?pageNumber=1&pageSize=10&team=Red
```

### Addresses

- `GET /api/addresses` — paginated and filterable
- `GET /api/addresses/{id}`
- `POST /api/addresses`
- `PUT /api/addresses/{id}`
- `DELETE /api/addresses/{id}`

Query parameters: `pageNumber`, `pageSize` (maximum 100), `city`, `search`.

```http
GET /api/addresses?pageNumber=1&pageSize=10&city=Cape%20Town
```

---

## 🧪 Testing

```powershell
dotnet test .\ShippingApi.UnitTests\ShippingApi.UnitTests.csproj
dotnet test .\ShippingApi.IntegrationTests\ShippingApi.IntegrationTests.csproj
dotnet test .\ShippingApi.sln
```

Coverage includes controllers, services, DTO mapping, validation, exception safety, CRUD endpoints, pagination/filtering, health checks, and Swagger.

---

## 🐳 Docker

```powershell
docker build -t shippingapi:latest .
docker run --rm -p 8080:8080 shippingapi:latest
```

Docker containerization is included for local use. Public container hosting and deployment are intentionally out of scope.

---

## 🔁 CI

GitHub Actions workflow: `.github/workflows/dotnet-ci.yml`

The pipeline runs restore, Release build, unit/integration tests, and test-result artifact upload for pushes and pull requests targeting `master`. **No deployment job is configured.**

---

## 📈 Logging and observability

- Structured service-level logging
- Built-in HTTP logging
- `/health` endpoint
- Trace IDs in validation and unexpected-error responses

---

## 📝 Portfolio notes

- Integration tests use EF Core InMemory and a test authentication handler.
- Swagger is enabled only in Development.
- The testing environment skips migrations and HTTPS redirection.
- This is a GitHub portfolio project with no public deployment, hosting, or cloud provisioning.

---

## 📄 License

No license file has been added yet.
