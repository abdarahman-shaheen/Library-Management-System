# Library Management System API

A robust, RESTful API built with **.NET 10 Minimal APIs**, designed using **Clean Architecture** principles and **CQRS** pattern with **MediatR**. This system manages library operations including books, authors, borrowers, loans, and user authentication.

## 🚀 Technologies & Patterns

*   **Runtime**: [.NET 10](https://dotnet.microsoft.com/en-us/) (Latest)
*   **Architecture**: Clean Architecture (Domain, Application, Infrastructure, API)
*   **API Framework**: Minimal APIs
*   **Data Access**: Entity Framework Core (SQL Server) with Repository Pattern & Unit of Work.
*   **CQRS**: MediatR for command/query separation.
*   **Validation**: **FluentValidation** with automatic pipeline behavior.
*   **Mapping**: AutoMapper.
*   **Authentication**: Custom JWT (JSON Web Tokens) implementation.
*   **Documentation**: Swagger / OpenAPI.
*   **Best Practices**:
    *   Global Exception Handling Middleware.
    *   Standardized API Response Wrappers (`ApiResponse<T>`).
    *   Eager Loading with Entity Framework.

## 🏗️ Architecture Overview

The solution is divided into four main projects:

1.  **Domain**: Core business logic, Entities (Books, Authors, etc.), and Interfaces. Dependency-free.
2.  **Application**: Business use-cases (CQRS Commands/Queries), Validators (FluentValidation), DTOs, and Interfaces implementation.
3.  **Infrastructure**: Database context (EF Core), Migrations, Repository implementations, and external services (JWT).
4.  **API**: Entry point, Dependency Injection setup, Middleware, and Minimal API Endpoint definitions.

## ✨ Features

*   **Authors**: Create, Update, Delete, Get All, Get by ID, Get with Books.
*   **Books**: Manage book inventory with ISBN validation.
*   **Borrowers**: Manage library members.
*   **Loans**: Track book loans and returns.
*   **Users**: Secure generic user management with Registration and Login (JWT Auth).
*   **Validation**: All inputs are validated strictly using FluentValidation. Invalid requests return standard `400 Bad Request` responses with error details.
*   **Error Handling**: Global middleware catches unhandled exceptions and returns standardized `500` or `400` responses.

## 🛠️ Getting Started

1.  **Prerequisites**:
    *   .NET SDK (10.0 or later)
    *   SQL Server (LocalDB or Docker or dedicated instance)

2.  **Configuration**:
    *   Update `ConnectionStrings:DefaultConnection` in `appsettings.json` to point to your SQL Server instance.

3.  **Database**:
    *   Run migrations to create the database:
        ```powershell
        dotnet ef database update --project LibraryManagementSystem.Infrastructure --startup-project LibraryManagementSystem.API
        ```
    *   *Note*: The application includes a `DbInitializer` that will seed initial data on startup.

4.  **Run**:
    ```powershell
    dotnet run --project LibraryManagementSystem.API
    ```

5.  **Explore**:
    *   Navigate to `https://localhost:your-port/swagger` to view and test the API using the Swagger UI.

## 🧪 Testing

The API includes `http` files or can be tested via Swagger. Authentication is required for most endpoints - use the `/api/users/login` endpoint to obtain a token and authorize in Swagger.


