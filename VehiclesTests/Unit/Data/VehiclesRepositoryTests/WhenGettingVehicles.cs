using Vehicles.Data.Repositories;

namespace VehiclesTests.Unit.Data.VehiclesRepositoryTests
{
    [Parallelizable]
    public class WhenGettingVehicles
    {
        private readonly VehiclesRepository _sut = new();

        [Test]
        [Description("This is just showing how the verify package can be used for snapshot testing")]
        public async Task TheExpectedVehicleDataIsReturned()
        {
            var result = await _sut.GetVehiclesAsync(CancellationToken.None);
            await Verify(result);
        }
    }
}
