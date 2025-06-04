using Microsoft.AspNetCore.Mvc;
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
public class GetVehicleByUserReturnsStatus429TooManyRequestsResponse(WebApplicationFactory<Users.Program> factory, GetVehicleByUserReturnsClassFixture classFixture) : IClassFixture<WebApplicationFactory<Users.Program>>, IClassFixture<GetVehicleByUserReturnsClassFixture>, IAsyncLifetime
{
    private HttpResponseMessage? _httpResponseMessage;
    private ProblemDetails? _response;
    private static readonly JsonSerializerOptions _caseInsensitiveDeserializationOptions = new() { PropertyNameCaseInsensitive = true };

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
        await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        _httpResponseMessage = await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        var responseMessageContent = await _httpResponseMessage.Content.ReadAsStringAsync();
        _response = JsonSerializer.Deserialize<ProblemDetails>(responseMessageContent, _caseInsensitiveDeserializationOptions);
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
