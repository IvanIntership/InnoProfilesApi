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
* **PostgreSQL** — The primary relational database used by the application (hosted via Docker).
* **MongoDB** — The database is used by the application for logging.
* **EF Core CLI Tools** — Required to manage database migrations and updates. You can install it globally via terminal:
  `dotnet tool install --global dotnet-ef`

### Development Environment
* **[JetBrains Rider](https://www.jetbrains.com/rider/)** — The designated IDE for this project, providing the best experience for .NET development and Clean Architecture navigation.

### API Testing & Documentation
* **Swagger UI** — Built directly into the project. Once the API is running in development mode, you can test all endpoints by navigating to `https://localhost:<port>/swagger`.

## Local Setup & Run

> **Note:** Ensure you have installed all the prerequisites mentioned in the section above before proceeding.

### 1. Clone the repository. 
Open your terminal and clone the project to your local machine, then navigate to the project folder:
```bash
git clone git@github.com:IvanIntership/InnoProfilesApi.git
cd InnoProfilesApi/ProfilesApi
```

### 2. Configure Environment Variables.
The application uses a .env file to securely manage database credentials and secrets.

In the ProfilesApi folder (right next to your docker-compose.yml), create a new file named .env and add the following configuration:

```bash
POSTGRES_PASSWORD=YourStrongPostgresPassword123!
PROFILES_DB_CONN=Host=postgresql;Port=5432;Database=ProfilesApiDb;Username=postgres;Password=YourStrongPostgresPassword123!
MONGO_LOGGING_CONN=mongodb://mongodb:27017/ProfilesLogs
PASSWORD_SECRET_KEY=YourSuperSecretPepperKey123!
```

### 3. Build and Run the Application.
You can start the entire infrastructure (API, PostgreSQL, and MongoDB) with a single command. From the terminal, run:
```bash
docker compose up --build
```

### 4. Explore the API.

Once the application is running, open your browser and navigate to the Swagger UI:

```text
https://localhost:5242/swagger
```

### 5. Stopping the Application.

To safely stop the application while preserving all your database data, press Ctrl + C in the terminal where Docker is running, or open a new terminal in the same folder and run:

```bash
docker compose down
```