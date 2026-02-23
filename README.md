# Book History API

A REST API built with ASP.NET Core that tracks the full change history of book entities.

## Features

- Add and update books
- Automatic change tracking — every modification is recorded with timestamp, description, old and new value
- Change history with pagination, filtering, ordering and grouping
- Swagger UI for easy testing

## Tech Stack

- ASP.NET Core 8
- Entity Framework Core with SQLite
- Swagger / Swashbuckle

### Run the project
```bash
git clone https://github.com/aiadomar0/book-history-api.git
cd book-history-api
dotnet run --project BookHistory.Api
```

Then open your browser at `http://localhost:5283`

The database is created and seeded automatically on first run.
