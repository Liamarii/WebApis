using Moq;
using NSubstitute;
using System.Globalization;
using Users.Services.Users;
using Users.Services.Users.Models;
using Users.Services.Vehicles;
using Users.Services.Vehicles.Models;

namespace UsersTests.Unit.Services
{
    public class MoqAndNSubstitute
    {
        public static TheoryData<string, string, string> VehicleTestData => new()
        {
            { "bobby", "chevrolet", "impala" },
            { "hank", "red", "rat" },
            { "peggy", "green", "camel" }
        };

        [Theory]
        [MemberData(nameof(VehicleTestData))]
        public async Task PlayingAroundWithMoq(string name, string make, string model)
        {
            // Arrange
            Mock<IVehiclesService>? mockVehicleService = new();
            mockVehicleService
                .Setup(x => x.GetVehiclesByMake(It.IsAny<GetVehiclesByMakeRequest>(), CancellationToken.None))
                .ReturnsAsync(new GetVehiclesByMakeResponse() { Vehicles = [new() { Make = make, Model = model }] });

            UsersService sut = new(mockVehicleService.Object);

            // Act
            GetAvailableVehiclesResponse response = await sut.GetAvailableVehicles(new() { Name = name }, CancellationToken.None);

            // Assert
            Assert.Equal(response.Message, $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} drives a {make} {model}");
        }

        [Theory]
        [MemberData(nameof(VehicleTestData))]
        [InlineData("some", "other", "data")]
        public async Task PlayingAroundWithNSubstitute(string name, string make, string model)
        {
            // Arrange
            IVehiclesService mockVehicleService = Substitute.For<IVehiclesService>();
            mockVehicleService.GetVehiclesByMake(Arg.Any<GetVehiclesByMakeRequest>(), CancellationToken.None)
                .Returns(new GetVehiclesByMakeResponse() { Vehicles = [new() { Make = make, Model = model }] });

            UsersService sut = new(mockVehicleService);

            // Act
            GetAvailableVehiclesResponse response = await sut.GetAvailableVehicles(new() { Name = name }, CancellationToken.None);

            // Assert
            Assert.Equal(response.Message, $"{CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name)} drives a {make} {model}");
        }
    }
}
