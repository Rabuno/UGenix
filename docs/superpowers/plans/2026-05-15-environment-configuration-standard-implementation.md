# Environment Configuration Standard Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Standardize environment configuration across the project, separating local dev from production and implementing fail-fast validation.

**Architecture:** Use .NET's `IConfiguration` with double-underscore naming, separate frontend Vite environment files, and a dedicated production configuration mapper.

**Tech Stack:** .NET 8, Vite, React, Git.

---

### Task 1: Repository Hygiene & Local Templates

**Files:**
- Modify: `.gitignore`
- Create: `.env.example`
- Create: `ugenix-frontend/.env.development`

- [ ] **Step 1: Update .gitignore to protect secrets**

```text
# Add to .gitignore
.env
.env.local
.env.*.local
appsettings.local.json
```

- [ ] **Step 2: Create root .env.example template**

```text
# ==========================================
# UGENIX LOCAL DEVELOPMENT (.env.example)
# ==========================================
# Copy this to .env and adjust as needed.

# --- Backend ---
ConnectionStrings__Database=Host=localhost;Database=ugenix_db;Username=postgres;Password=password
ConnectionStrings__Redis=localhost:6379

Jwt__Secret=a_very_long_local_secret_at_least_32_chars_long
Jwt__Issuer=UGenix.API
Jwt__Audience=UGenix.Frontend

Cloudinary__ApiKey=your_local_key
Cloudinary__ApiSecret=your_local_secret
Cloudinary__CloudName=your_local_name

# --- Frontend ---
VITE_API_BASE_URL=http://localhost:5039/api
```

- [ ] **Step 3: Setup Frontend local environment**

Create `ugenix-frontend/.env.development`:
```text
VITE_API_BASE_URL=http://localhost:5039/api
VITE_API_VERSION=v1
```

- [ ] **Step 4: Commit**

```bash
git add .gitignore .env.example ugenix-frontend/.env.development
git commit -m "chore: setup environment templates and ignore patterns"
```

---

### Task 2: Backend Configuration Standardization

**Files:**
- Modify: `ugenix-backend/src/UGenix.API/appsettings.json`
- Modify: `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs`

- [ ] **Step 1: Standardize appsettings.json sections**

Rename `PostgreSQL` to `Database` and `Jwt` to `Jwt` (ensure consistency).

```json
{
  "ConnectionStrings": {
    "Database": "Host=localhost;Database=ugenix_db;Username=postgres;Password=password",
    "Redis": "localhost:6379"
  },
  "Jwt": {
    "Issuer": "UGenix.API",
    "Audience": "UGenix.Frontend",
    "Secret": "A_VERY_LONG_AND_SECURE_SECRET_FOR_LOCAL_DEV_ONLY",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  }
}
```

- [ ] **Step 2: Update DependencyInjection to use standardized sections**

Update `AddInfrastructure` in `ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs`:
- Use `BindConfiguration("Jwt")`
- Add `ValidateOnStart()`

```csharp
// Inside AddInfrastructure
services.AddOptions<JwtOptions>()
    .BindConfiguration("Jwt")
    .ValidateDataAnnotations()
    .ValidateOnStart();
```

- [ ] **Step 3: Verify build**

Run: `dotnet build ugenix-backend/UGenix.sln`
Expected: PASS

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/src/UGenix.API/appsettings.json ugenix-backend/src/UGenix.Infrastructure/DependencyInjection.cs
git commit -m "refactor(backend): standardize configuration sections and enable startup validation"
```

---

### Task 3: Production Configuration & Fail-Fast

**Files:**
- Modify: `ugenix-backend/src/UGenix.API/Program.cs`
- Modify: `ugenix-backend/src/UGenix.Infrastructure/Extensions/ConfigurationExtensions.cs`

- [ ] **Step 1: Refine Production Configuration Mapping**

Update `AddProductionConfiguration` in `ugenix-backend/src/UGenix.Infrastructure/Extensions/ConfigurationExtensions.cs` to ensure it maps `POSTGRES_URL` correctly and validates critical sections.

- [ ] **Step 2: Integrate Production Config in Program.cs**

```csharp
// Inside Program.cs
if (app.Environment.IsProduction())
{
    builder.Services.AddProductionConfiguration(builder.Configuration);
}
```

- [ ] **Step 3: Add Startup Validation Logic**

Ensure `ValidateOnStart()` is called for all `ICriticalConfiguration` blocks.

- [ ] **Step 4: Commit**

```bash
git add ugenix-backend/src/UGenix.API/Program.cs ugenix-backend/src/UGenix.Infrastructure/Extensions/ConfigurationExtensions.cs
git commit -m "feat(backend): implement production configuration mapping and fail-fast validation"
```

---

### Task 4: Cleanup & Documentation

**Files:**
- Create: `DEPLOYMENT.md`
- Delete: Legacy `.env` files (after migration)

- [ ] **Step 1: Create DEPLOYMENT.md**

Document all environment variables required for production (Render/Supabase).

- [ ] **Step 2: Cleanup existing .env files**

Move any useful local values to the new ignored `.env` and remove them from the codebase if they were accidentally tracked.

- [ ] **Step 3: Commit**

```bash
git add DEPLOYMENT.md
git commit -m "docs: add deployment environment variable guide"
```
