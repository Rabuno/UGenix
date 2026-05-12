# Base stage for runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/UGem.API/UGem.API.csproj", "src/UGem.API/"]
COPY ["src/UGem.Application/UGem.Application.csproj", "src/UGem.Application/"]
COPY ["src/UGem.Domain/UGem.Domain.csproj", "src/UGem.Domain/"]
COPY ["src/UGem.Persistence/UGem.Persistence.csproj", "src/UGem.Persistence/"]
COPY ["src/UGem.Infrastructure/UGem.Infrastructure.csproj", "src/UGem.Infrastructure/"]
COPY ["src/UGem.Contracts/UGem.Contracts.csproj", "src/UGem.Contracts/"]
COPY ["src/UGem.Shared/UGem.Shared.csproj", "src/UGem.Shared/"]

RUN dotnet restore "src/UGem.API/UGem.API.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/src/UGem.API"
RUN dotnet build "UGem.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UGem.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final stage
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER $APP_UID
ENTRYPOINT ["dotnet", "UGem.API.dll"]
