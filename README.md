# EnterpriseIdentity_Auth

A RESTful authentication service built with **.NET 8, ASP.NET Core, C#, SQL Server, JWT, and Clean Architecture**.

EnterpriseIdentity_Auth is a backend project focused on identity management, token-based authentication, and session auditing. It provides endpoints for user registration, login, token renewal, logout, account activation, and authenticated user information.

The project demonstrates a layered approach to backend development, with an emphasis on separation of concerns, maintainability, and authentication workflows.

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [Architecture](#architecture)
- [Authentication and Session Management](#authentication-and-session-management)
- [API Reference](#api-reference)
- [Project Structure](#project-structure)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Database Configuration](#database-configuration)
- [Testing with Postman](#testing-with-postman)
- [Security Considerations](#security-considerations)
- [Roadmap](#roadmap)
- [License](#license)
- [Author](#author)

## Overview

The goal of this project is to implement an authentication API with a structured architecture and a dedicated token management workflow.

It provides a foundation for applications that require user authentication, protected resources, and session-related auditing.

## Features

- User registration and login.
- Password hashing.
- JWT-based authentication.
- Access token expiration.
- Refresh token management.
- Token revocation.
- User account activation.
- Logout functionality.
- Authenticated user information.
- Session tracking across multiple platforms.
- Client metadata collection, including IP address, browser or client information, and operating system.
- Audit timestamps and dates.
- RESTful API endpoints grouped under `/api/v1/auth`.
- SQL Server persistence.
- Clean Architecture and separation of responsibilities.
- API workflow validation using Postman.

## Technology Stack

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| .NET 8 | Application platform |
| ASP.NET Core Web API | REST API development |
| JWT | Token-based authentication |
| SQL Server | Relational database |
| Clean Architecture | Separation of concerns |
| Postman | API testing and validation |

## Architecture

The project follows a Clean Architecture approach to organize application responsibilities and separate core business logic from infrastructure concerns.

The architecture is intended to support maintainability, testability, and clear dependency boundaries.

### Architectural Overview

```text
┌──────────────────────────────────────┐
│                  API                 │
│       HTTP / Controllers / Auth      │
└───────────────────┬──────────────────┘
                    │
                    ▼
┌──────────────────────────────────────┐
│              Application             │
│         Application Use Cases        │
└───────────────────┬──────────────────┘
                    │
                    ▼
┌──────────────────────────────────────┐
│                Domain                │
│        Entities / Business Rules     │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│            Infrastructure            │
│       Persistence / External I/O     │
└──────────────────────────────────────┘

┌──────────────────────────────────────┐
│                Shared                │
│       Shared Components / Types      │
└──────────────────────────────────────┘
```

*Note: This diagram presents the conceptual responsibilities of the project layers. Refer to the solution's project references and dependency configuration for the exact dependency graph.*

### Layer Responsibilities

- **API:** Exposes HTTP endpoints and handles incoming requests.
- **Application:** Organizes application operations and use cases.
- **Domain:** Contains core domain concepts and business rules.
- **Infrastructure:** Handles persistence and external technical concerns.
- **Shared:** Contains shared components used across the solution, according to the actual implementation.

## Authentication and Session Management

### Authentication Workflow

The API supports registration and login, followed by authenticated access to protected resources.

```text
Client
  │
  ▼
User Registration
  │
  ▼
Credential Validation
  │
  ▼
User Authentication
  │
  ▼
Token Issuance
  │
  ▼
Access Protected Resources
```

### Token Lifecycle

The service provides endpoints for refreshing and revoking tokens.

```text
Access Token
     │
     ▼
Expiration
     │
     ▼
Refresh Request
     │
     ▼
Refresh Credential Validation
     │
     ▼
Token Renewal
```

The exact expiration policies, refresh token storage, rotation strategy, and revocation rules depend on the implementation.

### Session Auditing

The service includes session-related metadata to support auditing across clients and platforms.

The tracked information includes:

- IP address.
- Browser or client information.
- Operating system.
- Audit dates and timestamps.
- Session-related information for multiple platforms.

The uniqueness rules for sessions and the behavior of concurrent sessions are defined by the implemented session management logic.

## API Reference

The API uses the following base path:

```text
/api/v1/auth
```

| Endpoint | Description |
|---|---|
| `/api/v1/auth/register` | Register a new user |
| `/api/v1/auth/login` | Authenticate a user |
| `/api/v1/auth/logout` | Log out |
| `/api/v1/auth/refresh` | Refresh authentication tokens |
| `/api/v1/auth/revoke` | Revoke a token or session |
| `/api/v1/auth/activate` | Activate a user account |
| `/api/v1/auth/me` | Retrieve the current authenticated user's information |

For request methods, request bodies, response schemas, status codes, and authentication requirements, refer to the corresponding API controllers and the Postman collection.

## Project Structure

The solution is organized into the following main components:

```text
EnterpriseIdentity_Auth/
├── API/
├── Application/
├── Domain/
├── Infrastructure/
├── Shared/
├── Documents/
├── Properties/
├── Program.cs
├── appsettings.json
└── EnterpriseIdentity_Auth.csproj
```

The main components have the following responsibilities:

| Component | Responsibility |
|---|---|
| `API` | HTTP endpoints and API configuration |
| `Application` | Application operations and use cases |
| `Domain` | Domain concepts and business rules |
| `Infrastructure` | Persistence and infrastructure integrations |
| `Shared` | Shared application components |
| `Documents` | Supporting database scripts and API testing resources |
| `Program.cs` | Application entry point and service configuration |
| `appsettings.json` | Application configuration |

The directory listing is representative of the solution's main components. Consult the source tree for the complete structure.

## Prerequisites

Before running the application, ensure you have:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/downloads/)

The database must be available and configured before testing the API.

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/AndresDGarcesDev/EnterpriseIdentity_Auth.git
cd EnterpriseIdentity_Auth
```

### 2. Configure Application Settings

Configure the SQL Server connection string and authentication settings required by the application.

Use the configuration keys expected by the project.

Do not commit production credentials, private signing keys, or other sensitive configuration values.

### 3. Restore Dependencies

From the directory containing the solution file, run:

```bash
dotnet restore
```

### 4. Build the Solution

```bash
dotnet build
```

### 5. Run the API

Navigate to the directory containing the API project and run:

```bash
dotnet run
```

If the solution file and API project are located in different directories, adjust the working directory or specify the API project path.

Check the application output to determine the local URL and port.

## Database Configuration

The application uses SQL Server for persistence.

The target database must exist and be accessible through the configured connection string.

Database-related scripts and supporting resources are available in the repository's documentation directory.

Before running the application:

1. Configure the SQL Server connection string.
2. Ensure the target database exists.
3. Verify the required tables and schema.
4. Ensure the database account has the necessary permissions.

The exact database initialization procedure depends on the scripts and database configuration included in the repository.

## Testing with Postman

The project includes a Postman collection for exercising the authentication API.

The collection is available in the repository's supporting documents.

Recommended workflow:

1. Register a user.
2. Log in with the registered credentials.
3. Inspect the returned authentication information.
4. Access the authenticated user endpoint.
5. Test the token refresh workflow.
6. Test logout and token revocation.
7. Test account activation.
8. Validate expected authentication failures.

Use the actual request bodies, environment variables, and expected responses defined by the API and Postman collection.

**Testing status:** API workflows are currently validated using Postman. Automated unit and integration tests are not documented as part of the current testing setup.

## Security Considerations

The project addresses several important aspects of authentication and session management:

- Password hashing.
- JWT-based authentication.
- Token expiration and renewal.
- Token revocation.
- Session metadata and auditing.
- Configuration of authentication credentials.

A complete security assessment should additionally verify signing-key management, refresh token storage, token rotation, authorization policies, input validation, and the handling of sensitive session information.

The presence of authentication features does not, by itself, constitute a formal security audit.

## Roadmap

Potential future improvements include:

- Automated unit and integration tests.
- Docker-based development and deployment.
- CI/CD pipeline integration.
- Expanded API documentation.
- Additional security tests.
- More detailed architecture and database documentation.

These items are potential extensions rather than confirmed implementation gaps.

## License

Refer to the repository's license file for the applicable license terms.

## Author

**Andrés Garcés**

- GitHub: [AndresDGarcesDev](https://github.com/AndresDGarcesDev)
- Repository: [EnterpriseIdentity_Auth](https://github.com/AndresDGarcesDev/EnterpriseIdentity_Auth)
