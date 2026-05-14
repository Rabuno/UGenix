# Frontend Testing & Optimization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Implement a comprehensive testing strategy (Unit + E2E) and optimize the build delivery of static assets for production.

**Architecture:** We will integrate Vitest and Playwright directly into the existing Vite ecosystem. Build optimizations will be handled via Rollup configuration and Vite plugins, and served by Nginx using static gzip compression.

**Tech Stack:** React, Vite, Vitest, React Testing Library, Playwright, Nginx.

---

### Task 1: Setup Unit Testing Infrastructure & Test Auth Store

**Files:**
- Modify: `ugenix-frontend/package.json`
- Modify: `ugenix-frontend/vite.config.ts`
- Create: `ugenix-frontend/src/test/setup.ts`
- Create: `ugenix-frontend/src/store/__tests__/auth.store.test.ts`

- [ ] **Step 1: Install Vitest and RTL dependencies**

```bash
cd ugenix-frontend
npm install -D vitest @testing-library/react @testing-library/jest-dom jsdom
```

- [ ] **Step 2: Configure Vitest and Setup File**

Modify `ugenix-frontend/vite.config.ts` to include the test environment:

```typescript
// ugenix-frontend/vite.config.ts
/// <reference types="vitest" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
        secure: false,
      },
    },
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './src/test/setup.ts',
  },
});
```

Create `ugenix-frontend/src/test/setup.ts`:

```typescript
// ugenix-frontend/src/test/setup.ts
import '@testing-library/jest-dom';
```

- [ ] **Step 3: Write the failing test for Auth Store**

Create `ugenix-frontend/src/store/__tests__/auth.store.test.ts`:

```typescript
// ugenix-frontend/src/store/__tests__/auth.store.test.ts
import { describe, it, expect, beforeEach } from 'vitest';
import { useAuthStore } from '../auth.store';

describe('Auth Store', () => {
  beforeEach(() => {
    // Reset store before each test
    useAuthStore.setState({ user: null, isAuthenticated: false });
  });

  it('should initialize with default state', () => {
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
  });

  it('should set credentials', () => {
    const mockUser = { id: '1', email: 'test@example.com', role: 'User' };
    useAuthStore.getState().setCredentials(mockUser);
    
    const state = useAuthStore.getState();
    expect(state.user).toEqual(mockUser);
    expect(state.isAuthenticated).toBe(true);
  });

  it('should clear credentials on logout', () => {
    const mockUser = { id: '1', email: 'test@example.com', role: 'User' };
    useAuthStore.getState().setCredentials(mockUser);
    useAuthStore.getState().logout();
    
    const state = useAuthStore.getState();
    expect(state.user).toBeNull();
    expect(state.isAuthenticated).toBe(false);
  });
});
```

- [ ] **Step 4: Update package.json scripts and run tests**

Add `"test": "vitest run"` to the `"scripts"` block in `ugenix-frontend/package.json`.

Run:
```bash
cd ugenix-frontend
npm run test
```
Expected: PASS (assuming `auth.store.ts` already has these standard methods. If it fails, fix the store methods to match the expected interface).

- [ ] **Step 5: Commit**

```bash
git add ugenix-frontend/package.json ugenix-frontend/package-lock.json ugenix-frontend/vite.config.ts ugenix-frontend/src/test/setup.ts ugenix-frontend/src/store/__tests__/auth.store.test.ts
git commit -m "test(frontend): setup vitest and add auth store tests"
```

### Task 2: Setup E2E Testing Infrastructure (Playwright)

**Files:**
- Create: `ugenix-frontend/playwright.config.ts`
- Create: `ugenix-frontend/e2e/smoke.spec.ts`

- [ ] **Step 1: Install Playwright**

```bash
cd ugenix-frontend
npm install -D @playwright/test
npx playwright install chromium
```

- [ ] **Step 2: Configure Playwright**

Create `ugenix-frontend/playwright.config.ts`:

```typescript
// ugenix-frontend/playwright.config.ts
import { defineConfig, devices } from '@playwright/test';

export default defineConfig({
  testDir: './e2e',
  fullyParallel: true,
  forbidOnly: !!process.env.CI,
  retries: process.env.CI ? 2 : 0,
  workers: process.env.CI ? 1 : undefined,
  reporter: 'html',
  use: {
    baseURL: 'http://localhost:5173',
    trace: 'on-first-retry',
  },
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
    },
  ],
  webServer: {
    command: 'npm run dev',
    url: 'http://localhost:5173',
    reuseExistingServer: !process.env.CI,
  },
});
```

- [ ] **Step 3: Write a basic smoke test**

Create `ugenix-frontend/e2e/smoke.spec.ts`:

```typescript
// ugenix-frontend/e2e/smoke.spec.ts
import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('/');
  // Update this to match the actual app title
  await expect(page).toHaveTitle(/UGenix|Vite \+ React/);
});
```

- [ ] **Step 4: Update scripts and run Playwright**

Add `"test:e2e": "playwright test"` to `ugenix-frontend/package.json`.

Run:
```bash
cd ugenix-frontend
npm run test:e2e
```
Expected: PASS

- [ ] **Step 5: Commit**

```bash
git add ugenix-frontend/package.json ugenix-frontend/package-lock.json ugenix-frontend/playwright.config.ts ugenix-frontend/e2e/smoke.spec.ts
git commit -m "test(frontend): setup playwright and add smoke test"
```

### Task 3: Build & Performance Optimization (Vite)

**Files:**
- Modify: `ugenix-frontend/package.json`
- Modify: `ugenix-frontend/vite.config.ts`

- [ ] **Step 1: Install compression plugin**

```bash
cd ugenix-frontend
npm install -D vite-plugin-compression
```

- [ ] **Step 2: Configure Code Splitting and Compression**

Modify `ugenix-frontend/vite.config.ts` to include the plugin and rollup options:

```typescript
// ugenix-frontend/vite.config.ts
/// <reference types="vitest" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';
import viteCompression from 'vite-plugin-compression';

export default defineConfig({
  plugins: [
    react(),
    viteCompression({
      algorithm: 'gzip',
      ext: '.gz',
    }),
    viteCompression({
      algorithm: 'brotliCompress',
      ext: '.br',
    })
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  build: {
    rollupOptions: {
      output: {
        manualChunks: {
          'vendor-react': ['react', 'react-dom', 'react-router-dom'],
          'vendor-state': ['zustand', '@tanstack/react-query'],
          'vendor-utils': ['axios', 'clsx', 'tailwind-merge', 'zod'],
        }
      }
    }
  },
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:5000',
        changeOrigin: true,
        secure: false,
      },
    },
  },
  test: {
    globals: true,
    environment: 'jsdom',
    setupFiles: './src/test/setup.ts',
  },
});
```

- [ ] **Step 3: Verify the build**

Run:
```bash
cd ugenix-frontend
npm run build
```
Expected: The `dist` folder should contain `.gz` and `.br` files alongside the normal JS/CSS chunks, and the JS chunks should be separated into vendor files.

- [ ] **Step 4: Commit**

```bash
git add ugenix-frontend/package.json ugenix-frontend/package-lock.json ugenix-frontend/vite.config.ts
git commit -m "perf(frontend): add code splitting and asset compression"
```

### Task 4: Server Optimization (Nginx gzip_static)

**Files:**
- Modify: `ugenix-frontend/nginx.conf`

- [ ] **Step 1: Enable gzip_static**

Modify `ugenix-frontend/nginx.conf` to add `gzip_static on;` inside the `server` block.

```nginx
# ugenix-frontend/nginx.conf
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    # Enable serving pre-compressed files
    gzip_static on;

    # Security Headers
    add_header X-Frame-Options "DENY";
    add_header X-Content-Type-Options "nosniff";
    add_header X-XSS-Protection "1; mode=block";
    add_header Content-Security-Policy "default-src 'self'; script-src 'self' 'unsafe-inline' https://maps.vietmap.vn; style-src 'self' 'unsafe-inline' https://fonts.googleapis.com https://maps.vietmap.vn; font-src 'self' https://fonts.gstatic.com; img-src 'self' data: https://*.cloudinary.com https://*.onrender.com https://maps.vietmap.vn; connect-src 'self' https://*.onrender.com https://*.supabase.co https://*.vietmap.vn;";

    location / {
        try_files $uri $uri/ /index.html;
    }

    # Cache Control for static assets
    location ~* \.(js|css|png|jpg|jpeg|gif|ico|svg|woff|woff2|ttf|otf)$ {
        expires 1y;
        add_header Cache-Control "public, no-transform";
    }

    # Proxy API requests to backend
    location /api/ {
        proxy_pass http://__API_URL__;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
        proxy_cache_bypass $http_upgrade;
    }
}
```

- [ ] **Step 2: Commit**

```bash
git add ugenix-frontend/nginx.conf
git commit -m "perf(frontend): enable gzip_static in nginx config"
```