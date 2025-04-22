using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Users.Services.Users.Models;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace UsersTests.Integration.Api.WireMockTests;

[Collection("WireMock Tests")]
public class GetVehicleByUserReturnsBadGatewayResponse(WebApplicationFactory<Users.Program> factory, GetVehicleByUserReturnsClassFixture classFixture) : IClassFixture<WebApplicationFactory<Users.Program>>, IClassFixture<GetVehicleByUserReturnsClassFixture>, IAsyncLifetime
{
    private ProblemDetails? _response;
    private static readonly JsonSerializerOptions _caseInsensitiveDeserializationOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task InitializeAsync()
    {
        var server = classFixture.server;
        server.Reset();
        server
            .Given(Request.Create()
                .WithPath("/api/Vehicles/GetVehiclesByMake")
                .UsingPost())
            .RespondWith(Response.Create()
                .WithStatusCode(502)
                .WithHeader("Content-Type", "application/json")
                .WithBody("{ \"error\": \"Bad Request: invalid input\" }"));

        Assert.True(classFixture.server.IsStarted);
        var client = factory.CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var request = new GetAvailableVehiclesRequest() { Name = "Bob" };
        var responseMessage = await client.PostAsync("/Users", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"), CancellationToken.None);
        var responseMessageContent = await responseMessage.Content.ReadAsStringAsync();
        _response = JsonSerializer.Deserialize<ProblemDetails>(responseMessageContent, _caseInsensitiveDeserializationOptions);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsExpectedResponseType() => Assert.IsType<ProblemDetails>(_response);

    [Fact]
    public void ReturnExpectedStatusCode() => Assert.Equal(StatusCodes.Status502BadGateway, _response?.Status);

    [Fact]
    public void ReturnsTheExpectedTitle() => Assert.Equal("Bad Gateway", _response?.Title);

    [Fact]
    public void ReturnsTheExpectedDetail() => Assert.Equal("External service is unavailable.", _response?.Detail);
}