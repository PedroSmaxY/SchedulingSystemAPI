# Base image for build stage
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-preview AS build
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

# Final runtime image - IMPORTANTE: usar SDK ao invés de aspnet para poder usar o EF Tools
FROM mcr.microsoft.com/dotnet/nightly/sdk:9.0-preview AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install PostgreSQL client for potential migrations
RUN apt-get update && apt-get install -y postgresql-client && rm -rf /var/lib/apt/lists/*

# Copiar o script de entrypoint
COPY docker-entrypoint.sh /app/
RUN chmod +x /app/docker-entrypoint.sh

# Environment variables - replace values in production
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

# Usar o script como entrypoint
ENTRYPOINT ["/app/docker-entrypoint.sh"]