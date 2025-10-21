using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text;
using System.Text.Json;
using Users.Services;
using Users.Services.Users.Models;
using Users.Services.Vehicles.Models;
using WireMock.ResponseBuilders;

namespace UsersTests.Integration.Api.WireMockTests;

public class GetVehicleByUserReturnsSuccessfulResponse(
    WebApplicationFactory<Users.Program> factory,
    WireMockFixture wireMockFixture) :
    IClassFixture<WebApplicationFactory<Users.Program>>,
    IClassFixture<WireMockFixture>,
    IAsyncLifetime
{
    private HttpResponseMessage? _httpResponseMessage;
    private string? _response;

    public async Task InitializeAsync()
    {
        var vehiclesResponse = new GetVehiclesByMakeResponse()
        {
            Vehicles = [new Vehicle { Make = "Toyota", Model = "Corolla" }]
        };

        wireMockFixture.Server
            .Given(WireMock.RequestBuilders.Request.Create()
                .WithPath("/api/Vehicles/GetVehiclesByMake")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(200)
                .WithHeader("Content-Type", "application/x-protobuf")
                .WithBody(ProtobufHelper.SerialiseToProtobuf(vehiclesResponse)));

        var requestBody = new GetAvailableVehiclesRequest { Name = "Bob" };
        var request = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
        const string endpoint = "/Users";

        _httpResponseMessage = await wireMockFixture
            .CreateClient(factory)
            .PostAsync(endpoint, request, CancellationToken.None);

        _response = await _httpResponseMessage.Content.ReadAsStringAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsExpectedStatusCode() => Assert.Equal(HttpStatusCode.OK, _httpResponseMessage?.StatusCode);

    [Fact]
    public void ReturnsResult() => Assert.NotNull(_response);

    [Fact]
    public void ReturnsResultContainingRequestedName() => Assert.Contains("Bob drives a Toyota Corolla", _response);
}