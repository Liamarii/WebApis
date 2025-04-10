# **What is this**
This repository is a sandbox for experimenting with backend and frontend patterns commonly used in modern applications. It serves as both a learning tool and a reference for myself. 
The code is not intended to be production-ready, each concept is usually implemented in a single place rather than throughout.

Note: Concepts are implemented in isolation. For example, error handling may be shown in one feature but not applied throughout the code base.

# **Done**
- Use `HttpClient` services
- Use `MediatR`
- Use `Swagger UI`
- Use `Scalar UI`
- Use a static logger to get a generic response structure
- Use a simplified setup with split out infrastructure logic
- Use snapshot testing.
- Use error handling
- Use cancellation tokens
- Use `protobuf` to serialize and deserialize in a generic way. 
- Use rate limiting
- Use feature structure
- Use custom exception handling
- Use standardized `ProblemDetails` error responses (RFC 7807)
- Use `Polly` for fault handling.
- Use `Resilience Pipeline` for fault handling. 
- Use `Fluent NHibernate` to read in real data from a Postgres database.
- Use `LINQ` or LINQ style queries.
- Use `IOptions` configuration sections.
- Use `Angular` for the UI
- Use `Cors` to allow a request through from the UI.
- Use a mock `HttpClient` message handler.

- Testing
  - Use integration tests
  - Use unit tests
  - Use `XUnit`
  - Use `NUnit`
  - Use `Moq`
  - Use `NSubstitute`
  - Use `WireMock` tests.
  - Use `Test Containers` to hold some Postgres test data.
  - Use a custom `web application factory` to switch configs during testing.
  - Use `Playwright` tests.
    - Use `Playwright` to fake network responses.
---
# **Ideas / To do**
- Add `Jest` tests
- Style the Angular UI with `Tailwind` and `Shadcn`
- Move over some of the code from the `AngularSite` to have some functioning forms / pages / loading spinner to hook tests into.
- Add scoped logging
- Add header propagation
- Add Docker containers
- Add `EF Core`
- Add telemetry
- Add webhooks
- Add SOAP requests
- Add message broker communications
- Add a `Blazor` UI
- Add an anti-corruption layer
- Add Selenium tests
- Add Hurl tests
- Add caching:
  - Add `Redis` cache
  - Add In-Memory cache
  - Add `Dragonfly` cache
- Add secret keys
- Add a shared area:
  - Common project
  - Shared project
  - NuGet
- Add middleware
- Add events
- Add a static analyzer with auto-fix
- Add a circuit breaker
- Add Swagger docs
- Add HATEOAS
- Add PDF generator
- Add Authorisation
- Add Authentication
- Add an auto mapper
- Add more details to the notes of how to run the angular project.
---

# **Prerequisites**
- [Install Docker](https://docs.docker.com/desktop/setup/install/windows-install/)
- Run in terminal: ```wsl --install``` Windows Subsystem for Linux (WSL) to run a Linux environment on Windows without a virtual machine
- ```wsl --set-default-version 2``` Use WSL 2 instead of the default 1 as it has improvements.
- Open Docker Desktop
- Check both are running ```wsl --list --verbose```
---

# **Notes**
- To shutdown Ubuntu which gets left open even after existing Docker Desktop: ```wsl --shutdown```
- [Postgres Docker images](https://hub.docker.com/_/postgres)
- The VehiclesTests project uses a custom web app factory to change the environment which will point to a test database.
- The code currently uses a real Postgres database I have locally which will be swapped out at some point.
- Run playwright tests with `npx playwright test`
- Launch playwright with `npm run start` after doing an import `npm i` and build `npm run build`.
