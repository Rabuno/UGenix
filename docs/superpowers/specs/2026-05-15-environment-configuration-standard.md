# Environment Configuration Standard Design

**Status:** Approved
**Author:** Gemini CLI
**Date:** 2026-05-15

## 1. Goal
Establish a clear separation between Local Development and Production environments to ensure security, prevent accidental secret leaks, and provide a "plug-and-play" developer experience.

## 2. Environment Strategy

### 2.1 Local Development (Local)
- **Tooling:** Native `dotnet run`, `npm run dev`, or Docker Compose.
- **Config Source:** `.env` file (ignored by git) and `appsettings.Development.json`.
- **Defaults:** Points to `localhost` services (DB, Redis, API).
- **Frontend Port:** `5173`
- **Backend Port:** `5039`
- **Policy:** `appsettings.Development.json` should only contain non-sensitive shared development defaults. Any machine-specific or sensitive values must reside in the ignored `.env` file.

### 2.2 Production (Deploy)
- **Platform:** Render (Backend/Frontend), Supabase (Database/Auth).
- **Config Source:** Platform Environment Variables (Dashboard).
- **Secrets:** No production secrets in the codebase or git-tracked files.
- **Fail-Fast:** The application MUST fail at startup if critical production variables are missing (Validation on start).

## 3. Configuration Mapping

### 3.1 Naming Convention
We will use the double underscore (`__`) convention for nested configurations.

| Category | .NET Section | Env Variable Key | Local Value (Example) |
| :--- | :--- | :--- | :--- |
| **Database** | `ConnectionStrings` | `ConnectionStrings__Database` | `Host=localhost;...` |
| **Redis** | `ConnectionStrings` | `ConnectionStrings__Redis` | `localhost:6379` |
| **JWT** | `Jwt` | `Jwt__Secret` | `your_min_32_chars_local_secret` |
| **JWT** | `Jwt` | `Jwt__Issuer` | `UGenix.API` |
| **CORS** | `Cors` | `Cors__AllowedOrigins` | `http://localhost:5173` |

**CORS Format:** `Cors__AllowedOrigins` should be a comma-separated string (e.g., `https://app.com,https://api.com`) or a JSON array in platform settings.

### 3.2 Frontend Exposure (Vite)
Vite only exposes variables prefixed with `VITE_` to the client-side code. **NEVER** put backend secrets (Cloudinary API Secret, DB Password) in variables starting with `VITE_`.

## 4. Proposed Changes

### 4.1 Root & Global
- **.gitignore:** Ensure `.env`, `.env.local`, and any `.env.*.local` are ignored. 
- **.env.example:** Create a clean, commented template for local dev.
- **DEPLOYMENT.md:** Document all required keys for Render/Supabase.

### 4.2 Backend (UGenix.API)
- **Program.cs:** 
    - Enable `AddProductionConfiguration` when `Environment.IsProduction()`.
    - Add `ValidateOnStart()` to critical option blocks.
- **appsettings.json:** Standardize section names (e.g., rename `PostgreSQL` connection string to `Database`).
- **DependencyInjection.cs:** Standardize `.BindConfiguration("Jwt")` and add validation logic.

### 4.3 Frontend (UGenix.Frontend)
- **.env.development:** Set `VITE_API_BASE_URL=http://localhost:5039/api`.
- **.env.production:** Leave empty or set production defaults (non-sensitive).

## 5. Acceptance Criteria (Fail-Fast)
In **Production** mode, the application must log an error and exit if any of the following are missing:
- `ConnectionStrings__Database` (or `POSTGRES_URL` mapped via `AddProductionConfiguration`)
- `Jwt__Secret`
- `Supabase__Url` & `Supabase__AnonKey` (if enabled)

## 6. Documentation (DEPLOYMENT.md)
List of variables to set on Render:
- `ASPNETCORE_ENVIRONMENT=Production`
- `POSTGRES_URL` (From Supabase/Render)
- `Jwt__Secret`, `Jwt__Issuer`, `Jwt__Audience`
- `Cors__AllowedOrigins` (comma-separated origins)
- `Cloudinary__ApiKey`, `Cloudinary__ApiSecret`, `Cloudinary__CloudName`
