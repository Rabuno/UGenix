# Base stage for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/UGenix.API/UGenix.API.csproj", "src/UGenix.API/"]
COPY ["src/UGenix.Application/UGenix.Application.csproj", "src/UGenix.Application/"]
COPY ["src/UGenix.Domain/UGenix.Domain.csproj", "src/UGenix.Domain/"]
COPY ["src/UGenix.Persistence/UGenix.Persistence.csproj", "src/UGenix.Persistence/"]
COPY ["src/UGenix.Infrastructure/UGenix.Infrastructure.csproj", "src/UGenix.Infrastructure/"]
COPY ["src/UGenix.Contracts/UGenix.Contracts.csproj", "src/UGenix.Contracts/"]
COPY ["src/UGenix.Shared/UGenix.Shared.csproj", "src/UGenix.Shared/"]

RUN dotnet restore "src/UGenix.API/UGenix.API.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/src/UGenix.API"
RUN dotnet build "UGenix.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UGenix.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "UGenix.API.dll"]

