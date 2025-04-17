import { expect } from '@playwright/test';
import { test } from '../../e2e/support/fixtures';
import { execSync } from 'child_process';

test.describe('Perform a search', () => {
  test('Should show the expected error when attempting to search without adding a username', async ({ homePage, context }) => {

    await context.tracing.start({ screenshots: true, snapshots: true });

    await homePage.goto();
    await homePage.sendRequestButton.click();
    await expect(homePage.responseContainer).toBeVisible();
    await homePage.responseContainer.textContent();

    const filePath = `tests/trace/output/perform-a-search-${Date.now()}.zip`;
    await context.tracing.stop({ path: filePath })
    execSync(`npx playwright show-trace ${filePath}`, { stdio: 'inherit' });
  })
});
