using System.Net;
using Users.Infrastructure;
using Users.Services.Users;
using Users.Services.Vehicles;

namespace UsersTests.Integration.Api.MockHttpMessageHandlerTests
{
    public class TestHelper
    {
        private MockHttpMessageHandler? _mockHttpMessageHandler;
        private HttpClient? _httpClient;
        public TestHelper CreateHttpClient()
        {
            if (_mockHttpMessageHandler == null)
            {
                throw new InvalidOperationException($"You need to setup the handler first with {nameof(CreateMockHttpMessageHandler)}");
            }

            _httpClient = new HttpClient(_mockHttpMessageHandler)
            {
                BaseAddress = new Uri("https://localhost:7264")
            };

            return this;
        }

        public TestHelper CreateMockHttpMessageHandler(string response, HttpStatusCode httpStatusCode, Exception exception)
        {
            _mockHttpMessageHandler = new MockHttpMessageHandler(response, httpStatusCode)
            {
                ThrownException = exception
            };
            return this;
        }

        public UsersService CreateUsersService()
        {
            if (_httpClient == null)
            {
                throw new InvalidOperationException($"You need to setup the http client first with {nameof(CreateHttpClient)}");
            }
            return new UsersService(new VehiclesService(_httpClient, ResiliencePipelineProvider.GetDefaultPipeline()));
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
                if (ThrownException != null)
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
