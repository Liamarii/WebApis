using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;
using System.Text.Json;
using Users.Services.Users.Models;
using WireMock.ResponseBuilders;

namespace UsersTests.Integration.Api.WireMockTests;

public class GetVehicleByUserReturnsBadGatewayResponse(
    WebApplicationFactory<Users.Program> factory,
    WireMockFixture wireMockFixture) :
    IClassFixture<WebApplicationFactory<Users.Program>>,
    IClassFixture<WireMockFixture>,
    IAsyncLifetime
{
    private HttpResponseMessage? _httpResponseMessage;
    private ProblemDetails? _response;
    private static readonly JsonSerializerOptions _caseInsensitiveDeserializationOptions = new() { PropertyNameCaseInsensitive = true };

    //These tests do not run in parallel as they go through a resilience pipeline which uses circuit breaker. 
    // This has an internal lock which is prevents them running in parallel because the pipeline acts as a single instance to prevent race conditions.
    public async Task InitializeAsync()
    {
        wireMockFixture.Server
            .Given(WireMock.RequestBuilders.Request.Create()
                .WithPath("/api/Vehicles/GetVehiclesByMake")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(500));

        var requestBody = new GetAvailableVehiclesRequest { Name = "Bob" };
        var request = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        const string endpoint = "/Users";

        var x = wireMockFixture.CreateClient(factory);
        _httpResponseMessage = await x.PostAsync(endpoint, request, CancellationToken.None);

        var responseContent = await _httpResponseMessage.Content.ReadAsStringAsync();
        _response = JsonSerializer.Deserialize<ProblemDetails>(responseContent, _caseInsensitiveDeserializationOptions);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsExpectedResponseType() => Assert.IsType<ProblemDetails>(_response);

    [Fact]
    public void ReturnExpectedStatusCode() => Assert.Equal(502, _response?.Status);

    [Fact]
    public void ReturnsTheExpectedTitle() => Assert.Equal("Bad Gateway", _response?.Title);

    [Fact]
    public void ReturnsTheExpectedDetail() => Assert.Equal("External service is unavailable.", _response?.Detail);
}