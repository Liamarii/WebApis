using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Users.Services;
using Users.Services.Users.Models;
using Users.Services.Vehicles.Models;
using WireMock.ResponseBuilders;

namespace UsersTests.Integration.Api.WireMockTests;

[Collection("WireMock Tests")]
public class GetVehicleByUserReturnsSuccessfulResponse(WebApplicationFactory<Users.Program> factory, GetVehicleByUserReturnsClassFixture classFixture) : IClassFixture<WebApplicationFactory<Users.Program>>, IClassFixture<GetVehicleByUserReturnsClassFixture>, IAsyncLifetime
{
    private HttpResponseMessage? _httpResponseMessage;
    private string? _response;

    public async Task InitializeAsync()
    {
        var response = new GetVehiclesByMakeResponse()
        {
            Vehicles = [new() { Make = "Toyota", Model = "Corolla" }]
        };

        var server = classFixture.server;
        server.Reset();
        server
            .Given(WireMock.RequestBuilders.Request.Create()
            .WithPath("/api/Vehicles/GetVehiclesByMake")
            .UsingPost())
            .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/x-protobuf")
            .WithBody(ProtobufHelper.SerialiseToProtobuf(response)));

        Assert.True(classFixture.server.IsStarted);

        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var request = new GetAvailableVehiclesRequest() { Name = "Bob" };
        _httpResponseMessage = await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        _response = await _httpResponseMessage.Content.ReadAsStringAsync();
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsExpectedStatusCode() => Assert.Equal(HttpStatusCode.OK, _httpResponseMessage?.StatusCode);

    [Fact]
    public void ReturnsResult() => Assert.NotNull(_response);

    [Fact]
    public void ReturnsResultContainingRequestedName() => Assert.Contains("Bob", _response);
}
