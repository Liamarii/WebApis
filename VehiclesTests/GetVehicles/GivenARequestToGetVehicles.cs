using Vehicles.Features.GetVehicles;

namespace VehiclesTests.GetVehicles
{
    public class VehicleTests : IAsyncLifetime
    {
        private GetVehiclesRequest? _request;
        private GetVehiclesResponse? _response;

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            _request = new GetVehiclesRequest() { Make = "Toyota" };
            var sut = new GetVehiclesHandler();
            _response = await sut.Handle(_request, CancellationToken.None);
        }

        [Fact]
        public void TheResponseIsNotNull()
        {
            Assert.NotNull(_response);
        }

        [Fact]
        public void TheResponseContainsVehicles()
        {
            Assert.NotEmpty(_response!.Vehicles);
        }

        [Fact]
        public void TheReponseContainsOnlyToyotaVehicles()
        {
            Assert.True(_response!.Vehicles.All(x => x.Make == _request?.Make));
        }
    }
}
