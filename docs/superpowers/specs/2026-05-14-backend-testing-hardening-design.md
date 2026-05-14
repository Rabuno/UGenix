# Backend Testing & Production Hardening Design

## 1. Overview
The goal of this sub-project is to establish a robust testing suite for the UGenix backend and apply production-grade hardening measures to ensure reliability, security, and observability.

## 2. Testing Infrastructure

### 2.1 Unit Testing (`UGenix.Application.UnitTests`)
- **Framework**: xUnit.
- **Mocking**: NSubstitute.
- **Assertions**: FluentAssertions.
- **Scope**:
  - Domain business rules.
  - Application layer Features (Commands/Queries).
  - Custom validation logic.

### 2.2 Integration Testing (`UGenix.API.IntegrationTests`)
- **Framework**: xUnit + Microsoft.AspNetCore.Mvc.Testing (WebApplicationFactory).
- **Environment**: Testcontainers for .NET (PostgreSQL/PostGIS, Redis).
- **Scope**:
  - API Endpoints (E2E within the backend).
  - Database persistence logic.
  - Caching and Distributed Locking behavior.

## 3. Production Hardening

### 3.1 Observability & Logging
- **Structured Logging**: Finalize Serilog configuration.
- **PII Masking**: Ensure `SensitiveDataMaskingEnricher` is correctly filtering logs before they reach Seq.
- **Seq Integration**: Verify full connectivity and dashboard readiness.

### 3.2 Security
- **CORS Hardening**: Move allowed origins to environment-based configuration.
- **Security Headers**: Add `X-Content-Type-Options`, `X-Frame-Options`, and strict `Content-Security-Policy` via middleware if not already handled by Nginx.

### 3.3 Health Checks
- Implement `Microsoft.Extensions.Diagnostics.HealthChecks`.
- Add checks for:
  - PostgreSQL (PostGIS connectivity).
  - Redis (Cache connectivity).
  - External API dependencies (e.g., VietMap) if applicable.

## 4. Implementation Steps
1. Initialize test projects and add required NuGet packages.
2. Implement a base class for Integration Tests using Testcontainers.
3. Write representative Unit Tests for an Application feature.
4. Write representative Integration Tests for a core API endpoint.
5. Enhance Serilog and Health Check configurations.
6. Verify all tests pass in a CI-like environment.
