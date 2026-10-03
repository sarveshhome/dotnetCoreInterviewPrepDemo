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

### Install the Required NuGet Packages for HeatlhChecks
```
dotnet add package Microsoft.Extensions.Diagnostics.HealthChecks
dotnet add package AspNetCore.HealthChecks.SqlServer
dotnet add package AspNetCore.HealthChecks.Kafka
dotnet add package AspNetCore.HealthChecks.UI.Client
```


```
Microservices in .NET
├── Core Principles
│   ├── Independent Services: Small, autonomous, focused on single business capability
│   ├── Decentralized Data: Each service owns its database (Database per Service pattern)
│   ├── Loose Coupling: Services communicate via APIs, not direct dependencies
│   ├── Scalability & Resilience: Independent deployment and scaling
│   └── Polyglot Persistence: Different databases per service if needed
├── Key .NET Technologies
│   ├── ASP.NET Core: For building Web APIs (RESTful or Minimal APIs)
│   ├── Entity Framework Core: ORM for data access
│   ├── gRPC: For high-performance inter-service communication
│   └── Dependency Injection: Built-in container for managing services
├── Architecture Components
│   ├── API Gateway: Routes requests (e.g., Ocelot or YARP)
│   ├── Service Discovery: Find services dynamically (e.g., Consul or Eureka with Steeltoe)
│   ├── Communication
│   │   ├── Synchronous: HTTP/REST, gRPC
│   │   └── Asynchronous: Message Brokers (RabbitMQ, Azure Service Bus, MassTransit)
│   ├── Databases: SQL Server, PostgreSQL, MongoDB (via EF Core or direct drivers)
│   └── Observability
│       ├── Logging: Serilog
│       ├── Monitoring: Prometheus + Grafana
│       └── Distributed Tracing: OpenTelemetry or Jaeger
├── Deployment & Orchestration
│   ├── Containerization: Docker for packaging services
│   ├── Orchestration: Kubernetes or Docker Compose
│   └── CI/CD: Azure DevOps, GitHub Actions
├── Design Patterns
│   ├── Domain-Driven Design (DDD): Bounded Contexts for services
│   ├── CQRS & Event Sourcing: For complex domains (MediatR library)
│   ├── Saga Pattern: For distributed transactions
│   └── Circuit Breaker: Resilience (Polly library)
├── Challenges & Best Practices
│   ├── Avoid Shared Databases: Prevent tight coupling
│   ├── Eventual Consistency: Use events for data sync
│   ├── Testing: Unit, Integration, Contract Tests
│   └── Security: JWT, OAuth2, IdentityServer
└── Learning Roadmap
    ├── Fundamentals: .NET Basics, ASP.NET Core APIs
    ├── Advanced: Docker, Kubernetes, Message Queues
    └── Reference: Microsoft's eShopOnContainers sample app
```