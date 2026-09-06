# 🎓 University System

> A scalable backend for smarter university course planning — connecting **student demand**, **professor preferences**, and **availability** before the semester begins.

## ✨ The Idea

University semester planning usually starts with incomplete information:

- Which courses do students actually need?
- Which professors are willing to teach them?
- When are those professors available?

**University System** brings these inputs together.

Students pre-select courses they want for the next term, while professors submit the courses they can teach and their available time ranges. This gives the education department better data for planning course offerings, assignments, and schedules.

```text
Students ──► Course Demand ──► Academic Planning ◄── Teaching Preferences ◄── Professors
```

## 🏗️ Architecture

The backend follows **Clean Architecture**, keeping business rules independent from frameworks and infrastructure.

```text
┌─────────────────────────────────┐
│              API                │
│     Controllers · OpenAPI       │
├─────────────────────────────────┤
│ Infrastructure │  Persistence   │
│ JWT · Services │ EF Core · SQL  │
├─────────────────────────────────┤
│          Application            │
│ CQRS · Validation · Use Cases   │
├─────────────────────────────────┤
│            Domain               │
│     Business Model & Rules      │
└─────────────────────────────────┘
```

Dependencies point toward the core:

```text
Domain
  ↑
Application
  ↑
Infrastructure / Persistence
  ↑
API
```

## ⚡ Tech Stack

| Technology | Purpose |
|---|---|
| .NET 10 / ASP.NET Core | Backend API |
| EF Core 10 + SQL Server | Persistence |
| Code First | Database schema management |
| MediatR + CQRS | Application use cases |
| FluentValidation | Request validation |
| AutoMapper | Object mapping |
| JWT Bearer | Authentication foundation |
| Swagger / OpenAPI | API documentation |
| xUnit | Unit & integration testing |

## 📦 Solution Structure

```text
src/
├── UniversitySystem.Domain
├── UniversitySystem.Application
├── UniversitySystem.Infrastructure
├── UniversitySystem.Persistence
└── UniversitySystem.Api

tests/
├── UniversitySystem.UnitTests
└── UniversitySystem.IntegrationTests
```

## 🧩 Engineering Principles

The project favors clear and maintainable code over unnecessary abstractions.

- Clean Architecture & SOLID
- Clear naming and explicit dependencies
- High cohesion and low coupling
- Centralized validation, logging, auditing, and error handling
- Async-friendly and testable design
- No generic Repository or Unit of Work unless a real domain need appears
- No secrets or sensitive configuration committed to source control

## 🚀 Getting Started

Requirements:

- .NET 10 SDK
- SQL Server

```bash
git clone <repository-url>
cd UniversitySystem

dotnet restore
dotnet build
dotnet test
dotnet run --project src/UniversitySystem.Api
```

Database configuration is read from:

```text
ConnectionStrings:DefaultConnection
```

JWT configuration uses:

```text
Jwt:Issuer
Jwt:Audience
Jwt:SecretKey
Jwt:ExpirationMinutes
```

Use environment variables, User Secrets, or a proper secret store for sensitive values.

## 🗺️ Roadmap

### ✅ Foundation

- Clean Architecture
- CQRS + MediatR
- FluentValidation & AutoMapper
- EF Core / SQL Server foundation
- Auditing infrastructure
- JWT authentication foundation
- Global exception handling & ProblemDetails
- Swagger / OpenAPI
- Health checks
- Unit & integration test foundation

### 🚧 Next

- Academic domain model & database schema
- Users, roles, students, professors, majors and courses
- Authentication & role-based authorization
- Student course pre-registration
- Professor teaching requests & availability
- Academic planning and reporting

## 🌱 Vision

The first goal is to collect reliable demand and teaching availability data.

The architecture is designed to grow toward:

```text
Pre-Registration
      ↓
Course Offering
      ↓
Professor Assignment
      ↓
Schedule Generation
      ↓
Conflict Detection & Optimization
```

---

<p align="center">
  <strong>University System</strong><br/>
  Better data. Better planning. Better semesters.
</p>
