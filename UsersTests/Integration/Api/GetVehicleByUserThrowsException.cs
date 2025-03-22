using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Services.Users.Models;

namespace UsersTests.Integration.Api;

public class GetVehicleByUserThrowsException : IAsyncLifetime
{
    private readonly TestHelper _testHelper = new();
    private ActionResult<GetAvailableVehiclesResponse>? _response;

    public async Task InitializeAsync()
    {
        _testHelper.UseFakeHttpClient(new InvalidCastException());
        _response = await _testHelper.UsersController.GetVehicleByUser(_testHelper.getAvailableVehiclesRequest, CancellationToken.None);
    }

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public void ReturnsTheExpectedResponseType() => Assert.IsType<ObjectResult>(_response?.Result);

    [Fact]
    public void ReturnsTheExpectedStatusCode() => Assert.Equal(StatusCodes.Status500InternalServerError, (_response?.Result as ObjectResult)?.StatusCode);

    [Fact]
    public void ReturnsTheExpectedMessage() => Assert.Equal("An internal server error occurred while processing your request.", ((_response?.Result as ObjectResult)?.Value as ProblemDetails)?.Detail);

    [Fact]
    public void ReturnsTheExpectedTitle() => Assert.Equal("Internal server error", ((_response?.Result as ObjectResult)?.Value as ProblemDetails)?.Title);
}
