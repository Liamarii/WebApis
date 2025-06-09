using Moq;
using NHibernate;
using NUnit.Framework.Internal;
using Vehicles.Data.Repositories;
using Vehicles.Infrastructure.Persistence;
using Vehicles.Infrastructure.Persistence.Entities;

namespace VehiclesTests.Unit.Data.VehiclesRepositoryTests
{
    [Parallelizable]
    public class WhenGettingVehicles
    {
        private readonly VehiclesRepository _sut;

        public WhenGettingVehicles()
        {
            List<Vehicle> vehicles =[
                new(){ Id = 1,Vin = "1HGCM82633A123456", Make = "Honda", Model = "Egg", Year = 2020},
                new(){ Id = 2,Vin = "1FADP3F24JL123457", Make = "Ford", Model = "Fastcar", Year = 2020},
                new(){ Id = 3,Vin = "3N1AB7AP8HY123458", Make = "Nissan", Model = "Bagel", Year = 2021},
                new(){ Id = 4,Vin = "1C4RJFBG4FC123459", Make = "Jeep", Model = "Spongebob", Year = 2022},
                new(){ Id = 5,Vin = "5YJ3E1EA7KF123460", Make = "Tesla", Model = "Model Why", Year = 2022}];

            Mock<ISession> mockSession = new();
            mockSession
                .Setup(s => s.QueryOver<Vehicle>().ListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(vehicles);

            Mock<INHibernateDatabase> databaseMock = new();
            databaseMock.Setup(f => f.OpenSession()).Returns(mockSession.Object);

            _sut = new VehiclesRepository(databaseMock.Object);
        }

        [Test]
        [Description("This is just showing how the verify package can be used for snapshot testing")]
        public async Task TheExpectedVehicleDataIsReturned()
        {
            var result = await _sut.GetVehiclesAsync(CancellationToken.None);
            await Verify(result);
        }
    }
}
