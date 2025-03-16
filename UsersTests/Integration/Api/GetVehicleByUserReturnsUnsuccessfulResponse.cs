using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Users.Services.Users.Models;

namespace UsersTests.Integration.Api;

public class GetVehicleByUserReturnsUnsuccessfulResponse : IAsyncLifetime
{
    private readonly TestHelper _testHelper = new();
    private ActionResult<GetAvailableVehiclesResponse>? _response;

    public async Task InitializeAsync()
    {
        _testHelper.UseFakeHttpClient("Does not matter", HttpStatusCode.BadRequest);
        _response = await _testHelper.UsersController.GetVehicleByUser(_testHelper.getAvailableVehiclesRequest);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsTheExpectedResponseType() => Assert.IsType<ObjectResult>(_response?.Result);

    [Fact]
    public void ReturnsTheExpectedStatusCode() => Assert.Equal(StatusCodes.Status502BadGateway, (_response?.Result as ObjectResult)?.StatusCode);

    [Fact]
    public void ReturnsTheExpectedMessage() => Assert.Equal("External service is unavailable.", (_response?.Result as ObjectResult)?.Value);
}
