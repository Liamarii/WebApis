import { Page } from "@playwright/test";

const serviceUrl = 'https://localhost:7146/Users';

export class mockUsersService {

    constructor(private readonly page: Page) { }

    public async mockPostRequestAsync(requestedUser: string, response: string, ) {
        await this.page.route(serviceUrl, async (route, request) => {
            if (request.method() == 'POST' && request.postData() == JSON.stringify({ name: `${requestedUser}` })) {
                await route.fulfill({
                    status: 200,
                    contentType: 'application/json',
                    body: JSON.stringify({ message: response })
                });
            }
        })
    }
}
