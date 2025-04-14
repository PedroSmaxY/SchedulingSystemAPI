# Base image for build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["SchedulingSystemAPI.csproj", "./"]
RUN dotnet restore

# Copy the rest of the files and build
COPY . .
RUN dotnet build -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

# Final runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install PostgreSQL client for potential migrations
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Create a non-root user to run the app
RUN adduser --disabled-password --gecos "" appuser
USER appuser

# Environment variables - replace values in production
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
# Database connection variables will be provided at runtime
# ENV DB_SERVER=your-db-server
# ENV DB_NAME=your-db-name
# ENV DB_USER=your-db-user
# ENV DB_PASSWORD=your-db-password
# ENV Jwt__Key=your-jwt-key
# ENV Jwt__Issuer=SchedulingAPI
# ENV Jwt__Audience=SchedulingAPIClients

EXPOSE 8080

ENTRYPOINT ["dotnet", "SchedulingSystemAPI.dll"]