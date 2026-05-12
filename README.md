# UGenix | High-Performance Spatial Discovery & Marketplace

UGenix is a production-grade platform engineered for real-time spatial discovery and secure marketplace transactions. Built with a focus on **Runtime Safety**, **Data Integrity**, and **Extreme Performance**.

![Architecture](https://img.shields.io/badge/Architecture-Clean_Arch-blue?style=for-the-badge)
![Backend](https://img.shields.io/badge/Backend-.NET_9-512bd4?style=for-the-badge&logo=dotnet)
![Frontend](https://img.shields.io/badge/Frontend-React_18-61dafb?style=for-the-badge&logo=react)
![Database](https://img.shields.io/badge/Database-PostGIS-336791?style=for-the-badge&logo=postgresql)

---

## 🚀 Key Features

- **Spatial Discovery Engine**: Real-time radius search powered by **PostGIS** and **VietMap GL**, featuring grid-based marker clustering for 10k+ data points.
- **Identity & Security**: JWT Rotation (RTR), HttpOnly cookies, and strict **Content Security Policy (CSP)**.
- **Voucher Marketplace**: Atomic inventory management with **Optimistic Concurrency Control** (RowVersion) to prevent overselling.
- **Anti-Fraud Reviews**: Multi-layered reputation system with device/IP metadata tracking.
- **Observability**: Distributed tracing with **OpenTelemetry**, structured logging with Serilog, and zero-leak masking.

---

## 🏗️ Technical Stack

### Backend
- **Core**: ASP.NET Core 9 (Clean Architecture & DDD)
- **Persistence**: Entity Framework Core + PostGIS
- **Caching**: Redis (Stampede protection & distributed locking)
- **API**: Versioned REST APIs (RFC7807 Problem Details)

### Frontend
- **Runtime**: React 18 + TypeScript + Vite
- **State**: Zustand (Atomic store)
- **Data Flow**: TanStack Query (SWR pattern)
- **Maps**: VietMap GL SDK (WebGL optimized)

---

## 🛠️ Quick Start

### 1. Prerequisites
- Docker & Docker Compose
- Node.js 20+

### 2. Run with Docker (Full Stack)
```bash
git clone https://github.com/Rabuno/UGenix.git
cd UGenix/ugem-backend
docker-compose up --build -d
```
Access the application at: `http://localhost`

---

## 🛡️ Hardening & Governance
UGenix is built for production from day one:
- **Zero-Leak Logging**: Sensitive PII/Secrets are automatically masked.
- **Fail-Fast Configuration**: System validates all environment variables on startup.
- **Non-Root Runtime**: Backend services run under restricted user privileges.
- **SRI & CSP**: Frontend assets are protected against supply-chain attacks.

---

## 🗺️ Roadmap
- [x] Phase 1-8: Core Business Features
- [x] Phase 9: Governance & Type-Safety
- [x] Phase 10: Production Hardening
- [ ] Phase 11: Real-time Analytics & OTel Dashboards

---

Developed by **Lead Staff Engineer** @ UGenix Team.
