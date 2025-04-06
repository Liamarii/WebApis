using WireMock.Server;
using WireMock.Settings;


namespace UsersTests.Integration.Api.WireMockTests
{
    public class GetVehicleByUserReturnsClassFixture : IDisposable
    {
        public WireMockServer server;

        public GetVehicleByUserReturnsClassFixture()
        {
            server = WireMockServer.Start(new WireMockServerSettings
            {
                Port = 7264,
                UseSSL = true
            });
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            server.Stop();
        }
    }

    /*
        Needs this to point at the class that can't be run in parallel.
        This was a problem because the prod code had to point to a specific resource / port and I wanted tests to get different responses
        to the same request so they couldn't be running at the same time or responses would conflict.
    */
    [CollectionDefinition("WireMock Collection", DisableParallelization = true)]
    public class WireMockCollection : ICollectionFixture<GetVehicleByUserReturnsClassFixture> { }
}
