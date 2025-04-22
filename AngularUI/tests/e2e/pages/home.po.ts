import { Page, Locator } from '@playwright/test';
import { expect } from '@playwright/test';

export class HomePage {
  readonly sendRequestButton : Locator;
  readonly usernameInputBox : Locator;
  readonly title: string;
  readonly responseContainer: Locator;

  constructor(private readonly page: Page) {
    this.title = "AngularUI"; 
    this.sendRequestButton = this.page.getByRole('button', { name: 'Send request' });
    this.usernameInputBox = page.getByRole('textbox', { name: 'username' }); 
    this.responseContainer = page.getByTestId('response-message');
  }
  
  async goto(){
    await this.page.goto('/home');
    const actualTitle = await this.page.title()
    expect(actualTitle).toBe(this.title);
  }
}