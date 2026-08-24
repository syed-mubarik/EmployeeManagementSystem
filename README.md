# Employee Management System API

A production-style Employee Management REST API built with **ASP.NET Core 8 Web API** and **Clean Architecture**, demonstrating secure authentication, role-based authorization, database-driven business modules, and maintainable backend architecture.

## Project Structure

- `EmployeeManagement.API`
- `EmployeeManagement.Application`
- `EmployeeManagement.Domain`
- `EmployeeManagement.Infrastructure`
- `EmployeeManagement.Persistence`

## Features

### Employee Management

- Employee CRUD operations
- Department management
- Designation management
- Input validation
- Pagination
- Searching
- Sorting
- Soft delete
- Global exception handling
- Swagger / OpenAPI documentation

### Authentication

- User registration
- User login
- ASP.NET Core Identity
- Password hashing
- JWT access tokens
- Access token expiration
- Refresh tokens
- Refresh token hashing
- Refresh token rotation
- Refresh token revocation
- Logout / token revocation

### Authorization

- Role-Based Authorization
- Claims-Based Authorization

> Authorization features are currently being implemented and refined.

## Key Highlights

- Clean Architecture with clear separation of concerns
- Repository and Unit of Work patterns
- Entity Framework Core with SQL Server
- JWT-based authentication
- Role-based authorization
- Refresh token hashing, rotation, and revocation
- Global exception handling through middleware
- FluentValidation for request validation
- AutoMapper for DTO and entity mapping
- Pagination, searching, sorting, and soft delete
- Swagger / OpenAPI API documentation

## Authentication Flow

The application uses short-lived JWT access tokens together with longer-lived refresh tokens.

```text
Login
  |
  +--> Access Token
  |       |
  |       +--> Used to access protected APIs
  |
  +--> Refresh Token
          |
          +--> Stored as a hash in the database
          +--> Used to obtain a new access token
          +--> Rotated after use
          +--> Can be revoked
Configuration

Sensitive configuration such as the JWT signing key should not be stored in appsettings.json or committed to source control.

For local development, ASP.NET Core User Secrets are used.

Configure User Secrets

After cloning the repository, run:
dotnet user-secrets init --project EmployeeManagement.API

Configure the JWT signing key:

dotnet user-secrets set "Jwt:Key" "YOUR_DEVELOPMENT_SECRET_KEY" --project EmployeeManagement.API

Verify the configured secrets:
dotnet user-secrets list --project EmployeeManagement.API

Non-sensitive JWT configuration is stored in appsettings.json:

"Jwt": {
  "Issuer": "EmployeeManagementAPI",
  "Audience": "EmployeeManagementClient",
  "AccessTokenExpirationMinutes": 15,
  "RefreshTokenExpirationDays": 7
}

For production environments, secrets should be provided through environment variables or a dedicated secret-management service such as Azure Key Vault.

How to Run
1. Clone the Repository
git clone https://github.com/syed-mubarik/EmployeeManagementSystem.git
2. Configure the Database

Update the connection string in:

EmployeeManagement.API/appsettings.json

Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Configure JWT User Secret
dotnet user-secrets init --project EmployeeManagement.API
dotnet user-secrets set "Jwt:Key" "YOUR_DEVELOPMENT_SECRET_KEY" --project EmployeeManagement.API
4. Apply Database Migrations

From the solution root:

dotnet ef database update --project EmployeeManagement.Persistence --startup-project EmployeeManagement.API
5. Run the API
dotnet run --project EmployeeManagement.API

Or run the project through Visual Studio.

6. Open Swagger

Once the application is running, open the Swagger URL displayed by the application.

Example:

https://localhost:7002/swagger
Database Migrations

Migrations are managed using the .NET Entity Framework CLI.

Create a Migration
dotnet ef migrations add MigrationName --project EmployeeManagement.Persistence --startup-project EmployeeManagement.API
Apply Migrations
dotnet ef database update --project EmployeeManagement.Persistence --startup-project EmployeeManagement.API
Architecture

The application follows Clean Architecture principles:

                    API
                     |
                     ▼
               Application
                     |
          ┌──────────┴──────────┐
          ▼                     ▼
      Domain              Infrastructure
                                |
                                ▼
                           Persistence
                                |
                                ▼
                            SQL Server
API

Contains:

Controllers
Middleware
Dependency Injection configuration
API configuration

Application

Contains:

DTOs
Interfaces
Application services
Validators
AutoMapper profiles
Configuration models

Domain

Contains:

Entities
Domain-level models
Core business concepts

Infrastructure

Contains implementations of:

Repositories
Authentication services
JWT services
Refresh token services

Persistence

Contains:

Entity Framework Core DbContext
Entity configurations
Database migrations

Technologies
C#
ASP.NET Core 8
ASP.NET Core Web API
Entity Framework Core
SQL Server
ASP.NET Core Identity
JWT Authentication
Role-Based Authorization
Clean Architecture
Repository Pattern
Unit of Work
FluentValidation
AutoMapper
Swagger / OpenAPI
Git / GitHub
Author

Syed Mubarik Ali
## Connect With Me

- [LinkedIn](https://www.linkedin.com/in/syed-mubarik)
- [GitHub](https://github.com/syed-mubarik)
