import { test, expect } from '@playwright/test';

const filename = 'home-page';

test('homepage visual regression', async ({ page }) => {
    await page.goto('http://localhost:4200/home');
    await page.waitForLoadState('networkidle');
    await page.waitForSelector('[data-testid="app-ready"]');
    await page.locator('[data-testid="time-stamp"]').evaluate(x => x.remove());
    expect(await page.screenshot()).toMatchSnapshot(`screenshots/${filename}.png`);
});

test.afterEach(async ({ page }, testInfo) => {
    if (testInfo.status === 'failed') {
        await page.screenshot({ path: `./tests/visual/failed/${filename}.png` });
    }
});