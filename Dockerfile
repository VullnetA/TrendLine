# Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__DefaultConnection="Host=database;Port=5432;Database=TrendLineDB;Username=postgres;Password=YourStrong!Passw0rd"

# Image for building
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy and restore dependencies
COPY ["TrendLine.csproj", "."]
RUN dotnet restore "./TrendLine.csproj"

# Install Entity Framework Core tools
RUN dotnet tool install --global dotnet-ef --version 7.0.15
ENV PATH="${PATH}:/root/.dotnet/tools"

COPY . .
WORKDIR "/src/"
RUN dotnet build "./TrendLine.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "./TrendLine.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Install PostgreSQL client
RUN apt-get update \
    && apt-get install -y postgresql-client \
    && rm -rf /var/lib/apt/lists/*

# Create startup script
RUN echo '#!/bin/bash\n\
until pg_isready -h database -p 5432 -U postgres; do\n\
  >&2 echo "Postgres is unavailable - sleeping"\n\
  sleep 1\n\
done\n\
\n\
>&2 echo "Postgres is up - executing command"\n\
dotnet TrendLine.dll' > /app/startup.sh && \
chmod +x /app/startup.sh

ENTRYPOINT ["/app/startup.sh"]