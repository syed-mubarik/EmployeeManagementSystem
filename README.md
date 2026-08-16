# Employee Management System

A RESTful Employee Management System built using ASP.NET Core 8 Web API and Clean Architecture.

## Technologies

- ASP.NET Core 8
- C#
- Entity Framework Core
- SQL Server
- Clean Architecture
- Repository Pattern
- Unit of Work
- ASP.NET Core Identity
- AutoMapper
- FluentValidation
- Swagger / OpenAPI
- JWT Authentication
- Refresh Token Rotation
- Role-Based Authorization (Coming Soon)

## Project Structure

```text
-EmployeeManagement.API
-EmployeeManagement.Application
-EmployeeManagement.Domain
-EmployeeManagement.Infrastructure
-EmployeeManagement.Persistence
Features
Employee Management
-Employee CRUD
-Department Management
-Designation Management
-Validation
-Pagination
-Searching
-Sorting
-Soft Delete
-Global Exception Handling
-Swagger Documentation
-Authentication
-User Registration
-User Login
-ASP.NET Core Identity
-Password Hashing
-JWT Access Tokens
-Access Token Expiration
-Refresh Tokens
-Refresh Token Hashing
-Refresh Token Rotation
-Refresh Token Revocation
-Logout / Token Revocation
Authorization
-Role-Based Authorization (Coming Soon)
-Claims-Based Authorization (Coming Soon)
Authentication Flow

The application uses short-lived JWT access tokens together with longer-lived refresh tokens.
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
Then configure the JWT signing key:
dotnet user-secrets set "Jwt:Key" "YOUR_DEVELOPMENT_SECRET_KEY" --project EmployeeManagement.API
You can verify the configured secrets with:
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
1. Clone the repository
git clone https://github.com/syed-mubarik/EmployeeManagementSystem.git
2. Configure the database

Update the connection string in:

EmployeeManagement.API/appsettings.json

Example:

"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER;Database=EmployeeManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
3. Configure JWT User Secret
dotnet user-secrets init --project EmployeeManagement.API


dotnet user-secrets set "Jwt:Key" "YOUR_DEVELOPMENT_SECRET_KEY" --project EmployeeManagement.API
4. Apply database migrations

From the solution root:

dotnet ef database update --project EmployeeManagement.Persistence --startup-project EmployeeManagement.API
5. Run the API
dotnet run --project EmployeeManagement.API

Or run the project through Visual Studio.

6. Open Swagger

Once the application is running, open the Swagger URL displayed by the application, for example:

https://localhost:7002/swagger
Database Migrations

Migrations are managed using the .NET Entity Framework CLI.

Create a migration:

dotnet ef migrations add MigrationName --project EmployeeManagement.Persistence --startup-project EmployeeManagement.API

Apply migrations:

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

Contains controllers, middleware, dependency injection configuration, and API configuration.

Application

Contains:

-DTOs
-Interfaces
-Application services
-Validators
-AutoMapper profiles
-Configuration models
Domain

Contains:

-Entities
-Domain-level models
-Core business concepts
Infrastructure

Contains implementations of:

-Repositories
-Authentication services
-JWT services
-Refresh token services
Persistence

Contains:

-Entity Framework Core DbContext
-Entity configurations
-Database migrations
Author

Syed Mubarik

GitHub:

https://github.com/syed-mubarik/EmployeeManagementSystem
