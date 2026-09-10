# Getting Started


### 4. Apply migrations and seed data

The application automatically applies pending migrations and seeds initial data on startup. This creates:

- **Roles**: Admin, User
- **Default Admin User**:
  - Email: `admin@personalblog.com`
  - Password: `Admin123!`


The application will start at `https://localhost:5001` (or the port configured in `.env`).

## Usage

### Admin Features

1. Navigate to `/login` and sign in with the admin credentials
2. Access the admin dashboard at `/admin`
3. Use the **Create** action to add new articles
4. Use the **Edit** action to modify existing articles

### User Roles

- **Admin**: Full access to create, edit, and delete articles
- **User**: Read-only access to view articles

## Tests
The project includes unit tests for:

- **ArticleDtoTests**: Validates DTO mapping and equality logic
- **ArticleServiceTests**: Validates article service operations (CRUD) using mocked dependencies

### Cookie Authentication

Authentication cookie is configured in `Program.cs`:

- Cookie name: `MyAuthCookie`
- Login path: `/login`
- Access denied path: `/Auth/AccessDenied`
- Expiration: 14 days (with sliding expiration)
- HttpOnly and SameSite=Lax for security
