using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using Users.Services.Users.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UsersTests.Integration.Api.WireMockTests;

public class GetVehicleByUserReturnsStatus429TooManyRequestsResponse(
    WebApplicationFactory<Users.Program> factory,
    WireMockFixture wireMockFixture) :
    IClassFixture<WebApplicationFactory<Users.Program>>,
    IClassFixture<WireMockFixture>,
    IAsyncLifetime
{
    private HttpResponseMessage? _httpResponseMessage;
    private ProblemDetails? _response;
    private static readonly JsonSerializerOptions _caseInsensitiveDeserializationOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task InitializeAsync()
    {
        wireMockFixture.Server
            .Given(Request.Create()
                .WithPath("/api/Vehicles/GetVehiclesByMake")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200));

        var requestBody = new GetAvailableVehiclesRequest { Name = "Bob" };
        var request = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        const string endpoint = "/Users";

        var client = wireMockFixture.CreateClient(factory);
        await client.PostAsync(endpoint, request, CancellationToken.None);
        await client.PostAsync(endpoint, request, CancellationToken.None);
        await client.PostAsync(endpoint, request, CancellationToken.None);
        _httpResponseMessage = await client.PostAsync(endpoint, request, CancellationToken.None);

        var responseContent = await _httpResponseMessage.Content.ReadAsStringAsync();
        _response = JsonSerializer.Deserialize<ProblemDetails>(responseContent, _caseInsensitiveDeserializationOptions);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsExpectedStatusCode() => Assert.Equal(HttpStatusCode.TooManyRequests, _httpResponseMessage?.StatusCode);

    [Fact]
    public void ReturnsResult() => Assert.NotNull(_response);

    [Fact]
    public void ReturnsTheExpectedTitle() => Assert.Equal("Too Many Requests", _response?.Title);

    [Fact]
    public void ReturnsTheExpectedDetail() => Assert.Equal("stop it.", _response?.Detail);
}
