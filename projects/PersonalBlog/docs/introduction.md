# Introduction

## Overview

PersonalBlog is a full-featured blogging platform developed with ASP.NET Core 10. It allows multi-user article management with role-based access control, making it suitable for personal blogs as well as small team publishing.

## Architecture

The application follows the **MVC (Model-View-Controller)** pattern with a clean separation of concerns:

### Controllers

Controllers handle HTTP requests and responses:

- **HomeController** - Serves the landing page and error handling
- **ArticleController** - Manages article viewing, creation, and editing
- **AuthController** - Handles user authentication (login, logout)
- **AdminController** - Admin dashboard in a dedicated area

### Models

The domain layer consists of:

- **Article** - The core entity representing blog posts
- **User / Role** - Identity models for authentication
- **DTOs** - Data Transfer Objects for request/response handling
- **AuditableEntityBase** - Base class providing audit trail fields

### Services

Business logic is encapsulated in services:

- **ArticleService** - CRUD operations for articles
- **DbInitializer** - Seeds initial roles and admin user
- **IArticleService** - Interface for the article service
- **ValidationHelper** - Data annotation validation utility

### Persistence

- **ApplicationDbContext** - EF Core database context configured for PostgreSQL

## Authentication & Authorization

The application uses **ASP.NET Core Identity** with the following features:

- User registration and login
- Password hashing and validation
- Role-based authorization (Admin, User)
- Cookie-based authentication with configurable expiration
- Access denied handling

## Database

The application uses **PostgreSQL** via the Npgsql EF Core provider. Key database features:

- Code-first migrations
- Auto-generated article IDs (identity column)
- Audit fields (CreatedAt, CreatedBy, LastUpdatedAt, LastUpdatedBy)
- Custom table naming ("Articulos" for articles)
