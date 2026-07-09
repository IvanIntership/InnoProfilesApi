# InnoClinic Profiles API

## Description

This microservice is intended for:
- Managing system user's profiles(e.g. creating, editing, deleting profiles).
- Getting filtred information about doctors, specializations, offices 

## Software and tools required to work on the project

To successfully build, run, and contribute to this project locally, you will need the following software installed:

### Core Dependencies & Infrastructure
* **[.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)** — The core framework required to compile and run the Web API.
* **[Docker Desktop](https://www.docker.com/products/docker-desktop/)** (or Docker Engine) — Required to run the Microsoft SQL Server database locally in an isolated container.
* **[Git](https://git-scm.com/)** — For version control and source code management.

### Database & ORM
* **Microsoft SQL Server** — The primary relational database used by the application (hosted via Docker).
* **EF Core CLI Tools** — Required to manage database migrations and updates. You can install it globally via terminal:
  `dotnet tool install --global dotnet-ef`

### Development Environment
* **[JetBrains Rider](https://www.jetbrains.com/rider/)** — The designated IDE for this project, providing the best experience for .NET development and Clean Architecture navigation.

### API Testing & Documentation
* **Swagger UI** — Built directly into the project. Once the API is running in development mode, you can test all endpoints by navigating to `https://localhost:<port>/swagger`.

## Local Setup & Run

> **Note:** Ensure you have installed all the prerequisites mentioned in the section above before proceeding.

**1. Clone the repository** Open your terminal and clone the project to your local machine, then navigate to the project folder:
```bash
git clone git@github.com:IvanIntership/InnoProfilesApi.git
cd InnoProfilesApi
```

### 2. Configure the Database Connection

Open the`appsettings.json` file located in the main API project.

Locate the `ConnectionStrings` section and update it to point to your local Microsoft SQL Server instance.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=InnoProfilesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### 3. Apply Database Migrations

From the solution root directory, run the following command to create the database and apply all existing migrations:

```bash
dotnet ef database update --project ProfilesApi.Infrastructure --startup-project ProfilesApi
```

### 4. Build and Run the Application

You can start the application from the terminal:

```bash
dotnet run --project ProfilesApi
```

### 5. Explore the API

Once the application is running, open your browser and navigate to the Swagger UI:

```text
https://localhost:<port>/swagger
```

Replace `<port>` with the port number displayed in the application output.