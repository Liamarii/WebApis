# **What is this**
I use this repo to both practice and try out new ideas but mainly as a future reference for myself.
The solution is not intended to be production ready code as the concepts practiced are not applied throughout.
For example error handling is listed below and is demonstrated in one place will likely not be anywhere else when in practice it'd be done throughout.

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

---
# **Ideas**
- Add scoped logging
- Add header propagation
- Add Docker containers
- Add `EF Core`
- Add telemetry
- Add webhooks
- Add SOAP requests
- Add message broker communications
- Add a UI:
  - Add an `Angular` UI
  - Add a `Blazor` UI
- Add an anti-corruption layer
- Add Selenium tests
- Add Playwright tests
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
---

# **Prerequisites**
- [Install Docker](https://docs.docker.com/desktop/setup/install/windows-install/)
- Run in terminal: ```wsl --install``` Windows Subsystem for Linux (WSL) to run a Linux environment on Windows without a virtual machine
- ```wsl --set-default-version 2``` Use WSL 2 instead of the default 1 as it has improvements.
- Open Docker Desktop
- Check both are running ```wsl --list --verbose```
- Temp: As of 25/03/25 you'd need the underlying real postgres database the run in but I'll config a container of data in soon.
---

# **Notes**
- To shutdown Ubuntu which gets left open even after existing Docker Desktop: ```wsl --shutdown```
- [Postgres Docker images](https://hub.docker.com/_/postgres)
- The VehiclesTests project uses a custom web app factory to change the environment which will point to a test database.
