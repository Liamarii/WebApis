# **What is this?**
This repository is used as a sandbox for demonstrating, learning and a reference for myself. 

The code is not intended to be production-ready, each concept is usually implemented in a single place rather than throughout.

# **Done**
- Use `HttpClient` services
- Use `MediatR`
- Use `Swagger UI`
- Use `Scalar UI`
- Use a static logger to get a generic response structure
- Use a simplified setup with split out infrastructure logic
- Use error handling
- Use cancellation tokens
- Use `protobuf` to serialize and deserialize in a generic way. 
- Use rate limiting
- Use feature structure
- Use custom exception handling
- Use standardized `ProblemDetails` error responses (RFC 7807)
- Use `Polly Resilience Pipelines` for fault handling. 
- Use `Fluent NHibernate` to read in real data from a Postgres database.
- Use `LINQ` or LINQ style queries.
- Use `IOptions` configuration sections.
- Use `Angular` for the UI
  - Use `Angular Signals` for better change detection.
- Use `Cors` to allow a request through from the UI.
- Use a mock `HttpClient` message handler.

- Testing
  - Use integration tests
    - Use `WebApplicationFactory` to call the service endpoints
    - Use `WebApplicationFactory` with a way to swap out the real database for a test one.
    - Use test parallelism
  - Use `Reqnroll` BDD tests
    - Use `Before & After Scenarios`
    - Use `Scenario Contexts`
    - Use `Scenario Outline`
    - Use `Examples tables`
    - Use `StepArgumentTransformation`
  - Use unit tests
    - Use `XUnit`
    - Use `NUnit`
  - Use Mocks
    - Use `Moq`
    - Use `NSubstitute`
    - Use `WireMock`
  - Use `Test Containers` to hold some Postgres test data.
  - Use a custom `web application factory` to switch configs during testing.
  - Use `Playwright` tests.
    - Use `mock` network responses.
    - Use `visual comparisons` tests with fault tolerance and mismatch recording.
    - Use `traces` for test auditing
  - Use snapshot testing.
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
- Add CDC contract tests
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
- Add Swagger docs
- Add HATEOAS
- Add PDF generator
- Add Authorisation
- Add Authentication
- Add an auto mapper
- Add more details to the notes of how to run the angular project.
- Add load tests
  - K6 in the UI
  - NBomber in the backend.
---

# **Prerequisites**
- [Install Docker](https://docs.docker.com/desktop/setup/install/windows-install/)
- Run in terminal: ```wsl --install``` Windows Subsystem for Linux (WSL) to run a Linux environment on Windows without a virtual machine
- ```wsl --set-default-version 2``` Use WSL 2 instead of the default 1 as it has improvements.
- Open Docker Desktop
- Check both are running ```wsl --list --verbose```
---

# **Notes**
- To shutdown Ubuntu which gets left open even after exiting Docker Desktop: ```wsl --shutdown```
- The VehiclesTests project uses a custom web app factory to change the environment which will point to a test database.
- The API tests which pull data out of a database use a TestContainer docker image, not the real database [Postgres Docker images](https://hub.docker.com/_/postgres)
- The UI has some Playwright tests in which I run using the Playwright test for VSCode extension.
