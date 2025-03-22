using System.Net;
using Users.Controllers;
using Users.Infrastructure;
using Users.Infrastructure.FaultHandlers;
using Users.Services.Users;
using Users.Services.Users.Models;
using Users.Services.Vehicles;

namespace UsersTests.Integration.Api
{
    public class TestHelper
    {
        public UsersController UsersController { get; private set; }

        public TestHelper() => UsersController = CreateUserController();

        public readonly GetAvailableVehiclesRequest getAvailableVehiclesRequest = new() { Name = "anything" };

        private static UsersController CreateUserController()
        {
            return CreateUserController(new HttpClient() { BaseAddress = new Uri(Services.baseAddress) });
        }

        private static UsersController CreateUserController(HttpClient httpClient)
        {
            IVehicleService vehicleService = new VehiclesService(httpClient, new PollyFaultHandling());
            IUsersService usersService = new UsersService(vehicleService);
            return new UsersController(usersService);
        }

        public TestHelper UseFakeHttpClient(Exception exception)
        {
            var messageHandler = new MockHttpMessageHandler("does not matter", HttpStatusCode.Accepted)
            {
                ThrownException = exception
            };
            UseFakeHttpClient(messageHandler);
            return this;
        }

        public TestHelper UseFakeHttpClient(string response, HttpStatusCode httpStatusCode)
        {
            UseFakeHttpClient(new MockHttpMessageHandler(response, httpStatusCode));
            return this;
        }

        private void UseFakeHttpClient(MockHttpMessageHandler mockHttpMessageHandler)
        {
            var httpClient = new HttpClient(mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://justNeededToAppendARelativeUrl")
            };
            UsersController = CreateUserController(httpClient);
        }

        private class MockHttpMessageHandler(string response, HttpStatusCode statusCode) : HttpMessageHandler
        {
            private readonly string _response = response;
            private readonly HttpStatusCode _statusCode = statusCode;

            public string? Input { get; private set; }
            public int NumberOfCalls { get; private set; }
            public Exception? ThrownException { get; set; }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if(ThrownException != null)
                {
                    throw ThrownException;
                }

                NumberOfCalls++;
                if (request.Content != null)
                {
                    Input = await request.Content.ReadAsStringAsync(cancellationToken);
                }
                return new HttpResponseMessage
                {
                    StatusCode = _statusCode,
                    Content = new StringContent(_response)
                };
            }
        }
    }
}
