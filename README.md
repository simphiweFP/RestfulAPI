# ShippingApi

ShippingApi is a .NET 7 REST API for managing addresses, drivers, and orders. It is built as a portfolio-ready backend sample with unit tests, integration tests, Docker support, GitHub Actions CI, health checks, logging, pagination, filtering, and Swagger documentation.

## Highlights

- ASP.NET Core Web API on .NET 7
- Entity Framework Core with SQL Server in production
- In-memory database for integration tests
- JWT/Azure AD authentication via Microsoft Identity Web
- Global exception handling
- Swagger/OpenAPI documentation with bearer authentication support
- Bounded pagination and filtering for order, driver, and address queries
- Structured logging and health checks
- Unit tests and integration tests
- Dockerized runtime image
- GitHub Actions CI pipeline

## Architecture

The solution is split into three projects:

- `ShippingApi` - the API application
- `ShippingApi.UnitTests` - unit tests for services, controllers, and mapping
- `ShippingApi.IntegrationTests` - end-to-end HTTP tests using `WebApplicationFactory`

### Key folders in the API project

- `Controllers` - HTTP endpoints for addresses, drivers, and orders
- `Data` - Entity Framework DbContext and Unit of Work
- `Dtos` - request and response contracts
- `Middleware` - exception handling
- `Models` - domain entities
- `Services` - application services and logging decorator
- `UseCase` - repositories and abstractions

## Requirements

- .NET SDK 7.0
- SQL Server for the API runtime
- Docker Desktop if you want to run the container image

## Run locally

Restore and build:

```powershell
dotnet restore .\ShippingApi.sln
dotnet build .\ShippingApi.sln
```

Run the API:

```powershell
dotnet run --project .\ShippingApi\ShippingApi.csproj
```

## Configuration

The API expects these configuration values when running outside tests:

- `ConnectionStrings:DefaultConnection`
- `AzureAd`

You can provide them in `appsettings.Development.json`, user secrets, or environment variables.

## API documentation

When the API runs in Development mode, Swagger is available at:

- `https://localhost:7283/swagger`
- or the configured local HTTPS URL from `launchSettings.json`

Swagger includes:

- endpoint descriptions
- response codes
- JWT bearer authentication scheme
- XML documentation comments

## Endpoints

### Health

- `GET /health` - health check endpoint

### Orders

- `GET /api/orders` - returns paginated, filterable order results
- `GET /api/orders/{id}` - returns one order
- `POST /api/orders` - creates an order
- `PUT /api/orders/{id}` - updates an order
- `DELETE /api/orders/{id}` - deletes an order

#### Order query parameters

`GET /api/orders` supports:

- `pageNumber`
- `pageSize`
- `userId`
- `minTotalAmount`
- `maxTotalAmount`

Example:

```http
GET /api/orders?pageNumber=1&pageSize=10&userId=101
```

### Drivers

- `GET /api/drivers` - returns paginated, filterable driver results
- `GET /api/drivers/{id}`
- `POST /api/drivers`
- `PUT /api/drivers/{id}`
- `DELETE /api/drivers/{id}`

Driver query parameters include `pageNumber`, `pageSize`, `team`, and `search`.

Example:

```http
GET /api/drivers?pageNumber=1&pageSize=10&team=Red
```

### Addresses

- `GET /api/addresses` - returns paginated, filterable address results
- `GET /api/addresses/{id}`
- `POST /api/addresses`
- `PUT /api/addresses/{id}`
- `DELETE /api/addresses/{id}`

Address query parameters include `pageNumber`, `pageSize`, `city`, and `search`.

Example:

```http
GET /api/addresses?pageNumber=1&pageSize=10&city=Cape%20Town
```

## Testing

### Unit tests

```powershell
dotnet test .\ShippingApi.UnitTests\ShippingApi.UnitTests.csproj
```

### Integration tests

```powershell
dotnet test .\ShippingApi.IntegrationTests\ShippingApi.IntegrationTests.csproj
```

### Full solution test run

```powershell
dotnet test .\ShippingApi.sln
```

## Docker

Build the container image:

```powershell
docker build -t shippingapi:latest .
```

Run the container locally for development or portfolio demonstration:

```powershell
docker run --rm -p 8080:8080 shippingapi:latest
```

The API will listen on port `8080` in the container.

## CI

GitHub Actions workflow:

- `.github/workflows/dotnet-ci.yml`

It restores, builds, tests, and uploads test results for pushes and pull requests targeting `master`.

## Logging

The API uses structured logging for service-level operations and built-in HTTP logging for request metadata.

## Notes

- Integration tests use EF Core InMemory and a test authentication handler.
- The testing environment skips database migrations and HTTPS redirection.
- Swagger is exposed only in the Development environment.
- This repository is a GitHub portfolio project; no public deployment, hosting, or cloud provisioning is included.

## License

No license file has been added yet.
