# BabylonianGate

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat&logo=.net)
![EF Core](https://img.shields.io/badge/EF_Core-10.0-5C2D91?style=flat&logo=nuget)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-Data__Store-4169E1?style=flat&logo=postgresql)
![Docker](https://img.shields.io/badge/Docker-Containerized-2496ED?style=flat&logo=docker)

A RESTful API for a Warehouse & Inventory Management System tailored for weapon and ammunition storage control. It handles item lifecycles (issuing, returns, maintenance), tracking personnel assignments, and automated audit logging.

## Tech Stack

* **.NET 10 / ASP.NET Core Web API** — modern backend runtime platform
* **Entity Framework Core** — code-first migrations with per-entity fluent configuration classes
* **PostgreSQL** — relational database containerized via Docker
* **FluentValidation** — automatic request validation pipeline intercepting incoming DTOs
* **Repository Pattern** — decoupled data access abstraction layer
* **Dependency Injection** — managed via built-in native .NET IoC container
* **DotNetEnv** — direct root-level environment variable loading and mapping
* **xUnit + Moq + FluentAssertions** — unit testing suite for BLL/Domain core isolation validation
* **Docker / Docker Compose** — localized infrastructure virtualization
* **Swagger / OpenAPI** — interactive API documentation and integrated end-to-end testing client

---

## Architecture

| Project | Layer | Responsibilities & Dependencies |
| :--- | :--- | :--- |
| **BG.Domain** | Core Layer | Pure domain logic. Contains entities (`Soldier`, `Weapon`, `AmmoCrate`), domain events, value objects, and repository specifications. **No external dependencies.** |
| **BG.App** | Application Layer | Use cases execution. Services (`SoldierService`, etc.), Data Transfer Objects (DTOs), mapping structures, and fluent request validators. |
| **BG.Infra** | Infrastructure Layer | External data concern implementation, `BabylonianDbContext` context modeling, configuration mappings, and database migrations. |
| **BG.Api** | Presentation Layer | Application entry point. Web API Controllers, error-handling middleware, .env integration root, and service container bindings. |
| **BG.Tests** | Verification Layer | Unit tests verifying aggregate integrity boundaries and validator behaviors. |

---

## API Endpoints

### Inventory Items (Weapons & Ammunition)
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/weapons` | Add a new weapon to the inventory |
| `PUT` | `/api/weapons/details` | Update weapon specifications |
| `DELETE`| `/api/weapons/:id` | Remove a weapon record from inventory |
| `POST` | `/api/weapons/issue` | Assign and issue a weapon to a soldier |
| `POST` | `/api/weapons/return` | Return an issued weapon back to the storage |
| `POST` | `/api/weapons/maintenance` | Send a weapon out for maintenance or repairs |
| `POST` | `/api/weapons/report` | Flag a specific weapon status as missing |
| `GET` | `/api/weapons/:id` | Fetch specific weapon details by its ID |
| `GET` | `/api/weapons` | List all weapons in the storage |
| `POST` | `/api/ammos` | Register a new ammunition crate |
| `DELETE`| `/api/ammos/:id` | Delete an ammunition crate record |
| `PUT` | `/api/ammos/details` | Modify ammunition crate information |
| `POST` | `/api/ammos/issue` | Hand out ammunition from crates |
| `POST` | `/api/ammos/restock` | Restock ammunition count in a crate |
| `POST` | `/api/ammos/audit` | Conduct inventory check against an ammunition crate |
| `GET` | `/api/ammos/:id` | Fetch specific ammunition crate by its ID |
| `GET` | `/api/ammos` | List all available ammunition crates |

### Personnel Management (Soldiers)
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `POST` | `/api/soldiers` | Register a new soldier into the system |
| `PUT` | `/api/soldiers` | Update an existing soldier's profile |
| `DELETE`| `/api/soldiers/:id` | Discharge a soldier record from active deployment |
| `GET` | `/api/soldiers/:id` | Get individual soldier profile data by ID |
| `GET` | `/api/soldiers` | Retrieve list of all registered personnel |

### Audit & System Operations Logs
| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/logs/:id` | Get a specific audit operation log record by its ID |
| `GET` | `/api/logs/history/:entityId` | Retrieve complete tracking logs history for an item/entity |
| `GET` | `/api/logs/recent` | Fetch a list of recent operations (Defaults to 100 entries) |

---

## Getting Started

### Prerequisites
* Docker & Docker Compose
* .NET 10 SDK (for local development execution)

### Execution Steps

1. Clone the repository and navigate to the project root:
```bash
git clone [https://github.com/Am0rr/BabylonianGate.git](https://github.com/Am0rr/BabylonianGate.git)
cd BabylonianGate
```

2. Copy .env.example to .env and fill in the values:

```env
DB_HOST=localhost
DB_PORT=5432
DB_USER=postgres
DB_PASSWORD=YourSecurePasswordHere123!
DB_NAME=BabylonianGateDb
```

3. Run the PostgreSQL database instance via Docker:

```bash
docker-compose up -d
```

4. Run the API application:

```bash
cd backend/BG.Api
dotnet run
```

> **Notice:** On startup, `DotNetEnv` automatically maps your root variables, and EF Core triggers pending database schema updates onto your PostgreSQL instance.

5. Test Endpoints:
Access the interactive OpenAPI Swagger client UI directly inside your browser:

```text
http://localhost:5236/swagger/index.html
```

---

## Testing

To run the unit test validation suite across the solution layer, execute:

```bash
dotnet test
```