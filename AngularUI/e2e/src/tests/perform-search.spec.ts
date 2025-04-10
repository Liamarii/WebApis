import { expect } from '@playwright/test';
import { test } from './fixtures';

test.describe('Perform a search', () => {
  const knownUser = 'Arthur Strong';
  const usernameFoundResponse = `${knownUser} drives a Hyundai Xi`

  test.beforeEach(async({ homePage, page }) => {
    await homePage.goto();

    await page.route('https://localhost:7146/Users', async (route, request) => {
      if(request.method() == 'POST' && request.postData() == JSON.stringify({ name: `${knownUser}` }))
      {
        await route.fulfill({
          status: 200,
          contentType: 'text/plain',
          body: JSON.stringify({ message: `${usernameFoundResponse}` }),
        });
      }
    });
  })

  test('Should return the expected response when the username is found', async ({ homePage }) => { 
    await homePage.usernameInputBox.fill(knownUser);
    await homePage.sendRequestButton.click();
    await expect(homePage.responseContainer).toBeVisible();
    
    const containerText = await homePage.responseContainer.textContent();
    
    expect(containerText).toBe(usernameFoundResponse);
  })

  test('Should show the expected error when attempting to search without adding a username', async ({ homePage }) => { 
    await homePage.sendRequestButton.click();
    await expect(homePage.responseContainer).toBeVisible();
    
    const containerText = await homePage.responseContainer.textContent(); 
    expect(containerText).toBe('Error: username is required for the api call');
  })
});