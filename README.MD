# ENSEK Meter Reading API

## Idea
This project is a .NET 9 Web API for uploading and processing meter readings from CSV files. It demonstrates clean architecture, SOLID principles, and best practices. The API validates, deduplicates, and stores meter readings for registered accounts, using PostgreSQL as the backend database. The solution is containerized with Docker and ready for local development or deployment.

## Features
- Upload CSV files with meter readings via a single API endpoint
- Validates meter reading values and account existence
- Prevents duplicate and out-of-order readings
- Uses PostgreSQL or SQLite for data storage
- Seeded with sample account data
- Follows SOLID and clean code principles
- Fully containerized with Docker Compose

## How to Run

### Prerequisites
- [Docker](https://www.docker.com/products/docker-desktop) installed
- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) (for local development)

### Quick Start (Recommended)
1. Open a terminal in the project root (`ensek-task`).
2. Run:
   ```sh
   docker compose up -d --build
   ```
3. The API will be available at `http://localhost:5000`.
4. The PostgreSQL database will be available at `localhost:5432` (see `docker-compose.yml` for credentials).
5. Send POST request to `http://localhost:5000/v1/meter-reading-uploads`

### Local Development (without Docker)
1. Update `appsettings.json` with your local PostgreSQL connection string.
2. Start the API:
   ```sh
   dotnet run --project src/MeterReadingAPI
   ```

## Notes
- The solution is designed for simplicity and clarity. For larger projects, interfaces and abstractions can be moved to separate libraries to further decouple dependencies.
- See `Task.MD` for the original task description.

---

- Due to simplicity and time constraints, advanced dependency segregation (e.g., stairway pattern, adapter pattern) is not fully applied. For production, interfaces should be moved to their own libraries, and 3rd party dependencies should only be referenced in concrete implementations (see Stairway Pattern).
