# 🚀 .NET Employee & Auth REST API

A clean, layered **ASP.NET Core Web API** (built on **.NET 10**) demonstrating a real-world backend architecture: full **Employee CRUD**, **User Authentication** with hashed passwords, **JWT-based authorization**, and **Entity Framework Core** with **SQL Server**.

This project was built as a learning journey to practice production-style .NET backend patterns — DTOs, service/interface separation, generic API responses, and secure authentication.

---

## 📌 Features

- ✅ **User Registration & Login** with hashed passwords (no plain-text passwords ever stored)
- ✅ **JWT Authentication** — secure, stateless, token-based auth
- ✅ **Employee Full CRUD** (Create, Read, Update, Delete) — protected endpoints
- ✅ **Entity Framework Core** with **SQL Server** (Code-First + Migrations)
- ✅ **DTOs (Data Transfer Objects)** to keep entities decoupled from API contracts
- ✅ **Generic API Response wrapper** for consistent success/error responses
- ✅ **Service Layer + Interfaces** for clean separation of concerns (SOLID principles)
- ✅ **Layered / N-Tier Architecture** — easy to read, extend, and maintain

---

## 🏗️ Project Architecture

The project follows a clean, layered architecture so that each concern lives in its own folder:

```
📦 dotnet-learning-journey
├── Controllers/         # API endpoints (Auth & Employee controllers)
├── Data/                # DbContext & database configuration
├── Dto/                 # Request/response Data Transfer Objects
├── Entities/             # EF Core entity/domain models
├── GenericResponse/      # Standard wrapper for all API responses
├── IServices/            # Service interfaces (contracts)
├── Services/             # Service implementations (business logic)
├── Migrations/           # EF Core migrations
├── Properties/           # Launch settings
├── Program.cs            # App startup, DI, middleware pipeline
└── appsettings.json      # Configuration (connection string, JWT settings)
```

**Flow of a request:**

```
Client → Controller → Service (IService) → DbContext (EF Core) → SQL Server
                              ↓
                     GenericResponse<T> → Client
```

This separation means:
- **Controllers** only handle HTTP concerns (routing, status codes).
- **Services** contain the actual business logic.
- **Interfaces (IServices)** allow the services to be mocked/tested and swapped easily.
- **DTOs** prevent leaking internal entity structure to API consumers.
- **GenericResponse** ensures every endpoint returns a predictable, uniform response shape.

---

## 🛠️ Tech Stack

| Layer            | Technology                          |
|------------------|--------------------------------------|
| Framework        | ASP.NET Core Web API (.NET 10)      |
| ORM              | Entity Framework Core               |
| Database         | Microsoft SQL Server                |
| Auth             | JWT Bearer Authentication            |
| Password Hashing | ASP.NET Identity Password Hasher     |
| API Docs         | OpenAPI (Swagger-compatible)         |

---

## ⚙️ Getting Started

### 1️⃣ Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB, Express, or full instance)
- A REST client (Postman, Insomnia, or Thunder Client)

### 2️⃣ Clone the repository

```bash
git clone https://github.com/moosarehan/dotnet-learning-journey.git
cd dotnet-learning-journey
```

### 3️⃣ Restore dependencies

```bash
dotnet restore
```

### 4️⃣ Configure `appsettings.json`

Open **`appsettings.json`** in the project root and set your **SQL Server connection string** and **JWT settings**:

```json
{
  "ConnectionStrings": {
    "MyConnection": "Server=YOUR_SERVER_NAME;Database=EmployeeApiDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Jwt": {
    "Key": "YOUR_SUPER_SECRET_KEY_MIN_32_CHARACTERS_LONG",
    "Issuer": "http://localhost:5115",
    "Audience": "http://localhost:5115"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

> 🔑 **Notes on configuration:**
> - Replace `YOUR_SERVER_NAME` with your SQL Server instance name (e.g. `localhost`, `.\SQLEXPRESS`, or `(localdb)\MSSQLLocalDB`).
> - If using SQL authentication instead of Windows/Trusted auth, use:
>   `Server=YOUR_SERVER_NAME;Database=EmployeeApiDb;User Id=YOUR_USER;Password=YOUR_PASSWORD;TrustServerCertificate=True;`
> - The **JWT Key** must be a long, random, secret string (at least 32 characters). Never commit your real key to source control — use **User Secrets** or **environment variables** in production.
> - `Issuer` and `Audience` should match the base URL your API runs on.

### 5️⃣ Apply EF Core Migrations

This creates the database and tables based on the existing migrations:

```bash
dotnet ef database update
```

> If you don't have the EF Core CLI tool installed:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 6️⃣ Run the application

```bash
dotnet run
```

The API will start at something like:
```
http://localhost:5115
```

Since OpenAPI is enabled in development, you can explore the API schema at:
```
http://localhost:5115/openapi/v1.json
```

---

## 🔐 Authentication Flow

1. **Register** a new user → password is hashed before being saved to the database.
2. **Login** with credentials → server validates the hashed password and issues a **JWT token**.
3. Include the token in the `Authorization` header for all protected endpoints:

```
Authorization: Bearer <your_jwt_token>
```

4. The API validates the token's **issuer**, **audience**, **signing key**, and **expiry** on every request before granting access.

---

## 📡 API Endpoints

### 🔑 Auth

| Method | Endpoint              | Description                     | Auth Required |
|--------|------------------------|----------------------------------|----------------|
| POST   | `/api/Auth/register`  | Register a new user (password hashed) | ❌ No |
| POST   | `/api/Auth/login`     | Authenticate user, returns JWT   | ❌ No |

**Example — Register**
```http
POST /api/Auth/register
Content-Type: application/json

{
  "userName": "moosa.rehan",
  "email": "moosa@example.com",
  "password": "StrongP@ssw0rd!"
}
```

**Example — Login**
```http
POST /api/Auth/login
Content-Type: application/json

{
  "userName": "moosa.rehan",
  "password": "StrongP@ssw0rd!"
}
```

**Example — Response**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### 👤 Employees (Protected — Requires JWT)

| Method | Endpoint                  | Description              |
|--------|-----------------------------|---------------------------|
| GET    | `/api/Employee`            | Get all employees        |
| GET    | `/api/Employee/{id}`       | Get employee by ID       |
| POST   | `/api/Employee`            | Create a new employee    |
| PUT    | `/api/Employee/{id}`       | Update an existing employee |
| DELETE | `/api/Employee/{id}`       | Delete an employee       |

**Example — Create Employee**
```http
POST /api/Employee
Authorization: Bearer <your_jwt_token>
Content-Type: application/json

{
  "fullName": "John Doe",
  "email": "john.doe@company.com",
  "department": "Engineering",
  "salary": 75000
}
```

**Example — Generic Response Format**
```json
{
  "success": true,
  "message": "Employee created successfully",
  "data": {
    "id": 1,
    "fullName": "John Doe",
    "email": "john.doe@company.com",
    "department": "Engineering",
    "salary": 75000
  }
}
```

All endpoints — success or failure — return the same consistent shape via the **GenericResponse<T>** wrapper, making error handling on the client side predictable.

---

## 🧱 Key Design Decisions

- **DTOs over raw entities** — API consumers never see EF Core entities directly, protecting internal schema changes and preventing over-posting attacks.
- **Interface-based services (`IServices`)** — enables dependency injection, easier unit testing, and swapping implementations without touching controllers.
- **Password hashing** — passwords are never stored or compared in plain text; hashing happens before persistence.
- **JWT over session cookies** — stateless authentication that scales well and works cleanly with SPAs/mobile clients.
- **Generic API Response** — every response (success or error) follows the same JSON contract, reducing frontend guesswork.

---

## 🧪 Testing the API

You can test all endpoints using:
- **Postman** / **Insomnia** — import the routes above manually
- **.http files** in VS Code / Rider (using the built-in REST client)
- The generated **OpenAPI JSON** at `/openapi/v1.json` during development

---

## 📈 Possible Future Improvements

- [ ] Add Swagger UI for interactive API documentation
- [ ] Add Refresh Token support
- [ ] Add Role-based Authorization (Admin/User)
- [ ] Add Global Exception Handling Middleware
- [ ] Add FluentValidation for request validation
- [ ] Add Unit & Integration tests

---

## 📄 License

This project is for learning purposes. Feel free to fork, explore, and build on top of it.

---

## 🙌 Author

**Moosa Rehan**
📎 [GitHub](https://github.com/moosarehan)
