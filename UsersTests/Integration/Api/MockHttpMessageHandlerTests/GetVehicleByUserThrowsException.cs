using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Users.Controllers;
using Users.Services.Users;
using Users.Services.Users.Models;

namespace UsersTests.Integration.Api.MockHttpMessageHandlerTests;

public class GetVehicleByUserThrowsException(TestHelper testHelper) : IClassFixture<TestHelper>, IAsyncLifetime
{
    private readonly TestHelper _testHelper = testHelper;
    private ActionResult<GetAvailableVehiclesResponse>? _response;

    public async Task InitializeAsync()
    {

        UsersService usersService = _testHelper
            .CreateMockHttpMessageHandler("Uh oh", HttpStatusCode.BadGateway, new InvalidCastException("Service Gone!"))
            .CreateHttpClient()
            .CreateUsersService();
        _response = await new UsersController(usersService).GetVehicleByUser(new GetAvailableVehiclesRequest() { Name = "Anything" }, CancellationToken.None);
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
