# .NET Core Interview Preparation Demo

A comprehensive .NET 9 Web API project demonstrating various concepts and patterns commonly asked in interviews.

## Features

### Middleware
- **Custom Exception Handler** - Global exception handling
- **Rate Limiting** - Redis-based request throttling
- **Performance Monitoring** - Request timing middleware
- **Request Logging** - HTTP request/response logging
- **Maintenance Mode** - Application maintenance middleware

### Controllers
- **Patient API** - CRUD operations with Entity Framework
- **Security Controller** - JWT authentication examples

### Services
- **GuidService** - Demonstrates primary constructors (C# 12)
- **UserService** - User management operations

### Data Access
- **Entity Framework Core** - SQL Server integration
- **DbContext** - Patient data management

### Interview Questions
- Algorithm implementations
- LINQ examples
- Exception handling patterns
- Parallel processing examples

## Quick Start

```bash
# Clone and run
dotnet restore
dotnet run

# Watch mode (full restart)
dotnet watch --no-hot-reload
```

## Packages

```bash
# Core packages
dotnet add package Swashbuckle.AspNetCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package StackExchange.Redis
dotnet add package Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer
```

## API Endpoints

- `GET /api/patient` - Get all patients
- `POST /api/patient` - Create patient
- `GET /swagger` - API documentation

## Primary Constructor Example

```csharp
// Traditional approach
public class GuidService
{
    public Guid Id { get; set; }
    public GuidService() => Id = Guid.NewGuid();
}

// Primary constructor (C# 12)
public class GuidService(Guid id = default)
{
    public Guid Id { get; set; } = id == default ? Guid.NewGuid() : id;
}
```


