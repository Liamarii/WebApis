# **Done:**
- Use `HttpClient` services
- Use `MediatR`
- Use `Swagger UI`
- Use `Scalar UI`
- Add a static logger example to get a generic response structure
- Simplify the program setup with split-out infrastructure logic
- Use feature structure
- Add integration test examples
- Add unit test examples
- Add snapshot test examples
- Use error handling
- Use cancellation tokens
- Use `protobuf` to deserialize responses when consuming
- Use `protobuf` to serialize responses when producing
- Make the `protobuf` methods generic
- Use rate limiting
- Use `XUnit`
- Use `NUnit`
- Use `Moq`
- Use `NSubstitute`
- Add a mock `HttpClient` with `NSubstitute`
- Add custom exception handling
- Add standardized `ProblemDetails` error responses (RFC 7807)
- Use `Polly` for fault handling.
- Use `Resilience Pipeline` for fault handling. 
- Use `Test Containers` to hold some Postgres test data.
- Use a custom web application factory to switch configs during testing.
---

# **Ideas:**
- Add a mock `HttpClient` with `Moq`
- Add scoped logging
- Add header propagation
- Add Docker containers
- Add Test containers
- Add `NHibernate`
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
- Use LINQ
- Add PDF generator
- Add Authorisation
- Add Authentication

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
