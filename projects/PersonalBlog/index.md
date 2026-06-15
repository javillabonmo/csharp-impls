---
_layout: landing
---

# PersonalBlog

Welcome to the **PersonalBlog** documentation!

PersonalBlog is a modern, feature-rich blogging platform built with **ASP.NET Core 10** and **Entity Framework Core**. It provides a complete solution for managing blog articles with user authentication, role-based authorization, and a clean, responsive UI.

## Features

- **Article Management** - Create, edit, view, and delete blog articles
- **Authentication & Authorization** - Secure login with ASP.NET Core Identity
- **Role-based Access** - Admin and User roles with appropriate permissions
- **Responsive Design** - Built with Bootstrap for cross-device compatibility
- **PostgreSQL Support** - Production-ready database with Entity Framework Core
- **Admin Dashboard** - Dedicated admin area for content management
- **SEO-friendly URLs** - Clean route patterns for articles and pages

## Technology Stack

| Technology | Purpose |
|------------|---------|
| .NET 10 | Application framework |
| ASP.NET Core MVC | Web framework |
| Entity Framework Core 10 | ORM / Data access |
| PostgreSQL | Database |
| Bootstrap 5 | Frontend UI |
| ASP.NET Core Identity | Authentication & Authorization |

## Project Structure

```
PersonalBlog/
├── Areas/Admin/       # Admin area controllers and views
├── Controllers/       # MVC controllers
├── Models/            # Domain models, DTOs, enums
├── Persistence/       # Database context and configuration
├── Services/          # Business logic services
├── Views/             # Razor views
├── Migrations/        # EF Core migrations
├── Tests/             # Unit tests (xUnit)
└── wwwroot/           # Static assets
```

## Quick Links

- [Getting Started](docs/getting-started.md)
- [API Reference](api/PersonalBlog.Controllers.yml)
