import { expect } from '@playwright/test';
import { mockUsersService } from './support/mocks/MockUsersService';
import { test } from './support/fixtures'

test.describe('Perform a search', () => {
  const request = 'Arthur Strong';
  const response = `${request} drives a Hyundai Xi`;

  test.beforeEach(async ({ page, homePage }) => {
    let mockService = new mockUsersService(page);
    await mockService.mockPostRequestAsync(request, response)
    await homePage.goto();
  });

  test('Should return the expected response when the username is found', async ({ homePage }) => {
    await homePage.usernameInputBox.fill(request);
    await homePage.sendRequestButton.click();
    await expect(homePage.responseContainer).toBeVisible();

    const containerText = await homePage.responseContainer.textContent();

    expect(containerText).toBe(response);
  })

  test('Should show the expected error when attempting to search without adding a username', async ({ homePage }) => {
    await homePage.sendRequestButton.click();
    await expect(homePage.responseContainer).toBeVisible();

    const containerText = await homePage.responseContainer.textContent();
    expect(containerText).toBe('Error: username is required for the api call');
  })
});