# Scheduling System API

## Overview

SchedulingSystemAPI is a comprehensive RESTful API for appointment management. Built with ASP.NET Core and Entity Framework Core, this API provides a complete solution for managing users, services, availability slots, and appointments.

- 🔐 JWT authentication & role-based access
- 📅 Complete appointment scheduling workflow
- ⏰ Flexible availability management
- 🐳 Ready-to-run Docker configuration
- 📝 Comprehensive API documentation with Swagger UI

## Technologies Used

- **ASP.NET Core 9.0**
- **Entity Framework Core** with PostgreSQL
- **JWT Authentication** for secure access
- **Swagger UI** for interactive API documentation
- **Docker** for containerization
- **Docker Compose** for multi-container orchestration

## Key Features

- **User Management**: Registration, authentication, and profile management
- **Service Management**: Define services with durations and prices
- **Availability Management**: Configure available time slots
- **Appointment Scheduling**: Book, cancel, and manage appointments
- **Role-Based Access Control**: Separate admin and client permissions
- **Data Validation**: Comprehensive validation rules
- **Exception Handling**: Global exception middleware

## Project Structure

```

SchedulingSystemAPI/
│
├── Controllers/ # API endpoints
├── Data/ # Database configuration
├── DTOs/ # Data Transfer Objects
├── Helpers/ # Utility classes
├── Middleware/ # Custom middleware
├── Models/ # Domain models
├── Repositories/ # Data access layer
├── Services/ # Business logic
├── Validators/ # Input validation
│
├── docker-compose.yml # Container orchestration
├── Dockerfile # Container definition
└── docker-entrypoint.sh # Container startup script

```

## Getting Started

### Prerequisites

- Docker and Docker Compose

### Running the Application

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/SchedulingSystemAPI.git
   cd SchedulingSystemAPI
   ```

2. **Launch with Docker Compose**

   ```bash
   docker-compose up --build
   ```

3. **Access the API**

   The API will be available at http://localhost:8080

   Swagger documentation: http://localhost:8080/swagger

### Authentication

To use protected endpoints, obtain a JWT token:

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "password"
}
```

Use the token in the Authorization header:

```
Authorization: Bearer {your_token}
```

## API Endpoints

### Authentication

- `POST /api/auth/login` - Authenticate user
- `POST /api/auth/register` - Register new user

### Users

- `GET /api/users` - Get all users (Admin)
- `GET /api/users/{id}` - Get user by ID
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user (Admin)

### Services

- `GET /api/services` - Get all services
- `GET /api/services/{id}` - Get service by ID
- `POST /api/services` - Create service (Admin)
- `PUT /api/services/{id}` - Update service (Admin)
- `DELETE /api/services/{id}` - Delete service (Admin)

### Available Slots

- `GET /api/availableslots` - Get all available slots
- `GET /api/availableslots/{id}` - Get slot by ID
- `GET /api/availableslots/date/{date}` - Get slots by date
- `POST /api/availableslots` - Create slot (Admin)
- `DELETE /api/availableslots/{id}` - Delete slot (Admin)

### Appointments

- `GET /api/appointments` - Get all appointments (Admin)
- `GET /api/appointments/{id}` - Get appointment by ID
- `GET /api/appointments/user/{userId}` - Get user's appointments
- `POST /api/appointments` - Create appointment
- `PUT /api/appointments/{id}` - Update appointment
- `PUT /api/appointments/{id}/cancel` - Cancel appointment
- `PUT /api/appointments/{id}/confirm` - Confirm appointment (Admin)

## Development

### Environment Variables

```
# Database
DB_SERVER=postgres
DB_NAME=scheduling_system_db
DB_USER=postgres
DB_PASSWORD=postgres

# JWT
JWT_KEY=your-secret-key-with-at-least-32-characters
JWT_ISSUER=SchedulingAPI
JWT_AUDIENCE=SchedulingAPIClients
```

### Database Migrations

Migrations are automatically applied when the container starts.

To manually apply migrations:

```bash
docker-compose exec api dotnet SchedulingSystemAPI.dll --apply-migrations
```

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgements

- ASP.NET Core Team
- Entity Framework Core Team
- Npgsql Team

---

⭐️ If you find this project helpful, please star it on GitHub! ⭐️
