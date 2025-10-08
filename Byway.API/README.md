# Byway API - Backend

This is the backend API for the Byway online learning platform built with .NET 8.

## Features

- **Authentication & Authorization**: JWT-based authentication with role-based access control
- **Course Management**: CRUD operations for courses with file upload support
- **Instructor Management**: CRUD operations for instructors with rating system
- **Category Management**: Course categorization system
- **Order Management**: E-commerce functionality for course purchases
- **User Management**: User registration and profile management
- **File Upload**: Image upload for courses and instructors
- **Search & Filtering**: Advanced search and filtering capabilities

## Technology Stack

- **.NET 8**: Latest .NET framework
- **Entity Framework Core**: ORM for database operations
- **SQL Server**: Database
- **AutoMapper**: Object-to-object mapping
- **JWT Authentication**: Secure API authentication
- **Swagger/OpenAPI**: API documentation

## Project Structure

```
Byway.API/
├── Controllers/          # API Controllers
├── Program.cs           # Application entry point
└── appsettings.json    # Configuration

Byway.Application/
├── DTOs/               # Data Transfer Objects
├── Services/           # Business Logic Services
└── Interfaces/         # Service Interfaces

Byway.Domain/
├── Entities/           # Domain Entities
├── Enums/             # Domain Enums
└── Interfaces/        # Repository Interfaces

Byway.Infrastructure/
├── Data/              # DbContext and Configuration
└── Repositories/      # Repository Implementations
```

## API Endpoints

### Authentication
- `POST /api/auth/register` - User registration
- `POST /api/auth/login` - User login
- `POST /api/auth/google` - Google OAuth login

### Courses
- `GET /api/courses` - Get all courses with pagination and filtering
- `GET /api/courses/{id}` - Get course details
- `POST /api/courses` - Create new course (Admin only)
- `PUT /api/courses/{id}` - Update course (Admin only)
- `DELETE /api/courses/{id}` - Delete course (Admin only)

### Instructors
- `GET /api/instructors` - Get all instructors
- `GET /api/instructors/{id}` - Get instructor details
- `POST /api/instructors` - Create new instructor (Admin only)
- `PUT /api/instructors/{id}` - Update instructor (Admin only)
- `DELETE /api/instructors/{id}` - Delete instructor (Admin only)

### Categories
- `GET /api/categories` - Get all categories
- `POST /api/categories` - Create new category (Admin only)
- `PUT /api/categories/{id}` - Update category (Admin only)
- `DELETE /api/categories/{id}` - Delete category (Admin only)

### Orders
- `GET /api/orders/user/{userId}` - Get user orders
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}/status` - Update order status

### Admin
- `GET /api/admin/stats` - Get platform statistics (Admin only)
- `GET /api/admin/users` - Get all users (Admin only)
- `GET /api/admin/orders` - Get all orders (Admin only)

## Database Configuration

The application uses SQL Server with Entity Framework Core. Update the connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-server;Database=BywayDB;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

## Deployment

### For SmarterASP.NET:

1. **Database Setup**:
   - Create a SQL Server database on SmarterASP.NET
   - Update the connection string in `appsettings.json`

2. **File Upload Configuration**:
   - Ensure the `wwwroot` folder has write permissions
   - The application will create `images/courses/` and `images/instructors/` folders automatically

3. **Environment Variables**:
   - Set `ASPNETCORE_ENVIRONMENT=Production`
   - Configure JWT secret key in `appsettings.json`

4. **Build and Deploy**:
   - Build the project: `dotnet build --configuration Release`
   - Upload the published files to SmarterASP.NET
   - Ensure all dependencies are installed

## Configuration

### JWT Settings
```json
{
  "JwtSettings": {
    "SecretKey": "your-secret-key-here",
    "Issuer": "BywayAPI",
    "Audience": "BywayUsers",
    "ExpiryInHours": 24
  }
}
```

### CORS Settings
The API is configured to allow requests from the frontend application.

## Security Features

- JWT-based authentication
- Role-based authorization (Admin, User)
- CORS configuration
- Input validation
- SQL injection protection through Entity Framework

## File Upload

The application supports image uploads for:
- Course images
- Instructor profile images
- Category images

Files are stored in the `wwwroot/images/` directory with organized subfolders.

## Error Handling

The API includes comprehensive error handling with appropriate HTTP status codes and error messages.

## API Documentation

Swagger/OpenAPI documentation is available at `/swagger` when running in development mode.
