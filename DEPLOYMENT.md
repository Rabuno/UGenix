# UGenix Platform Deployment & Environment Guide

This document outlines the environment variable standards and configuration requirements for deploying the UGenix Platform on services like Render, Supabase, or other cloud providers.

## Environment Variable Standard

The UGenix backend uses the `Section__Key` naming convention for environment variables to map them to the .NET `IConfiguration` hierarchy.

- **Format:** `SectionName__VariableName`
- **Example:** `Jwt__Secret` maps to the `Secret` property inside the `Jwt` section (bound to `JwtOptions` class).

## Backend Configuration (ugenix-backend)

These variables must be set in your deployment environment (e.g., Render Environment Variables).

### Required for All Environments
| Variable | Description | Example Value |
|----------|-------------|---------------|
| `ConnectionStrings__Database` | PostgreSQL connection string | `Host=...;Database=...;Username=...;Password=...` |
| `Jwt__Secret` | HS256 Secret Key (min 32 chars) | `[SECURE_LONG_KEY]` |
| `Jwt__Issuer` | JWT Issuer | `UGenixPlatform` |
| `Jwt__Audience` | JWT Audience | `UGenixApp` |
| `Cors__AllowedOrigins` | Allowed origins for CORS | `https://your-frontend.render.com` |

### Integrations (Optional/Required per feature)
| Variable | Description |
|----------|-------------|
| `Cloudinary__ApiKey` | Cloudinary API Key |
| `Cloudinary__ApiSecret` | Cloudinary API Secret |
| `Cloudinary__CloudName` | Cloudinary Cloud Name |
| `Mail__Password` | SMTP App Password |
| `ConnectionStrings__Redis` | Redis connection string (if caching is enabled) |

## Frontend Configuration (ugenix-frontend)

Frontend variables must be prefixed with `VITE_` for Vite to bundle them.

| Variable | Description | Default/Example |
|----------|-------------|-----------------|
| `VITE_API_BASE_URL` | Root URL of the Backend API | `https://ugenix-api.onrender.com` |
| `VITE_API_VERSION` | API version prefix | `v1` |
| `VITE_VIETMAP_API_KEY` | VietMap integration key | `[YOUR_KEY]` |

## Local Setup

1. Copy `.env.example` from the root to `.env`.
2. Fill in the missing secrets.
3. Use `docker-compose up` or your local development workflow.

> **Warning:** Never commit `.env` files. Ensure they are tracked in `.gitignore`.
