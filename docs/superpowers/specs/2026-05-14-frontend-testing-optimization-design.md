# Frontend Testing & Optimization Design

## 1. Overview
The goal of this sub-project is to prepare the UGenix frontend for production by implementing a comprehensive testing strategy and optimizing the build and delivery of static assets.

## 2. Testing Infrastructure

### 2.1 Unit & Component Testing
- **Framework**: Vitest + React Testing Library (RTL).
- **Integration**: Vitest runs natively with the existing Vite configuration.
- **Scope**:
  - Test Zustand stores (`auth.store.ts`, `ui.store.ts`) for correct state transitions.
  - Test complex React components (especially those involving map rendering or forms).
  - Test custom hooks wrapping React Query logic.
- **Setup**: Require `jsdom` environment in Vitest config and setup a global test setup file.

### 2.2 End-to-End (E2E) Testing
- **Framework**: Playwright.
- **Scope**:
  - Critical user journeys: User Authentication (Login/Logout), Map Discovery, and Voucher interactions.
- **Integration**: Playwright tests will run against the locally built or development server.

## 3. Build & Performance Optimization

### 3.1 Code Splitting (Vite)
- Modify `vite.config.ts` to include `manualChunks` in the `build.rollupOptions`.
- Separate dependencies into logical chunks:
  - `vendor-react`: `react`, `react-dom`, `react-router-dom`
  - `vendor-state`: `zustand`, `@tanstack/react-query`
  - `vendor-maps`: Any VietMap specific dependencies if applicable.

### 3.2 Asset Compression
- Install `vite-plugin-compression`.
- Configure the plugin in `vite.config.ts` to generate both `.gz` (Gzip) and `.br` (Brotli) versions of JavaScript, CSS, and HTML assets during `npm run build`.

## 4. Server (Nginx) Optimization
- Update `ugenix-frontend/nginx.conf` to leverage the pre-compressed assets.
- Add `gzip_static on;` to the `server` block. This instructs Nginx to look for files with `.gz` extensions when a client supports gzip, avoiding the CPU overhead of on-the-fly compression.

## 5. Security Check (Self-Review)
- The current `Dockerfile` and `nginx.conf` have solid CSP headers. No structural changes are required beyond the compression optimizations.

## 6. Implementation Steps
1. Install testing dependencies (Vitest, RTL, Playwright).
2. Configure Vitest and write initial unit tests for the store.
3. Initialize Playwright and write a basic smoke test.
4. Update `vite.config.ts` with `manualChunks` and compression plugin.
5. Update `nginx.conf` and `Dockerfile` to support static gzip.
6. Verify the build locally.
