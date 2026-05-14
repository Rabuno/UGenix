// ugenix-frontend/e2e/smoke.spec.ts
import { test, expect } from '@playwright/test';

test('has title', async ({ page }) => {
  await page.goto('/');
  // Update this to match the actual app title
  await expect(page).toHaveTitle(/UGenix|Vite \+ React/);
});
