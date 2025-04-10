import { defineConfig } from '@playwright/test';

const twoMinutes = 2 * 60000;

export default defineConfig({
  webServer: {
    command: 'ng serve',
    port: 4200,
    timeout: twoMinutes,
    reuseExistingServer: true,
  },  testDir: './e2e/src/tests',
  use: {
    headless: true,
    baseURL: 'http://localhost:4200/'
  },
});