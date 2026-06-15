# Getting Started

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [PostgreSQL](https://www.postgresql.org/download/) (version 15 or later)
- A code editor (Visual Studio, VS Code, or Rider)

## Installation

### 1. Clone the repository

```bash
git clone <repository-url>
cd projects/PersonalBlog
```

### 2. Configure the database connection

Edit `appsettings.json` to set your PostgreSQL connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=PersonalBlog;Username=postgres;Password=your_password"
  }
}
```

### 3. Restore dependencies

```bash
dotnet restore
```

### 4. Apply migrations and seed data

The application automatically applies pending migrations and seeds initial data on startup. This creates:

- **Roles**: Admin, User
- **Default Admin User**:
  - Email: `admin@personalblog.com`
  - Password: `Admin123!`

### 5. Run the application

```bash
dotnet run
```

The application will start at `https://localhost:5001` (or the port configured in `launchSettings.json`).

## Usage

### Browsing Articles

- Visit `/` or `/home` to view all published articles
- Click on an article title to view its full content

### Admin Features

1. Navigate to `/login` and sign in with the admin credentials
2. Access the admin dashboard at `/admin`
3. Use the **Create** action to add new articles
4. Use the **Edit** action to modify existing articles

### User Roles

- **Admin**: Full access to create, edit, and delete articles
- **User**: Read-only access to view articles

## Running Tests

```bash
dotnet test
```

The project includes unit tests for:

- **ArticleDtoTests**: Validates DTO mapping and equality logic
- **ArticleServiceTests**: Validates article service operations (CRUD) using mocked dependencies

## Project Configuration

### appsettings.json

Key configuration sections:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=PersonalBlog;..."
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Cookie Authentication

Authentication cookie is configured in `Program.cs`:

- Cookie name: `MyAuthCookie`
- Login path: `/login`
- Access denied path: `/Auth/AccessDenied`
- Expiration: 14 days (with sliding expiration)
- HttpOnly and SameSite=Lax for security
