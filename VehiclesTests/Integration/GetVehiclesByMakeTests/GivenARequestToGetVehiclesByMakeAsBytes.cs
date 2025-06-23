using System.Net;
using System.Net.Http.Json;
using Vehicles.Api.Features.GetVehiclesByMake;
using Vehicles.Infrastructure.Serialisation;

namespace VehiclesTests.Integration.GetVehiclesByMakeTests;

[TestFixture("Honda")]
[TestFixture("Ford")]
[TestFixture("Nissan")]
[TestFixture("Jeep")]
[TestFixture("Tesla")]
[Parallelizable]
public class GivenARequestToGetVehiclesByMakeAsBytes(string make) : WebApplicationFactoryWithFakeDatabase<Vehicles.Program>
{
    private GetVehiclesByMakeRequest? _request;
    private HttpResponseMessage? _response;
    private GetVehiclesByMakeResponse? _responseContent;

    [OneTimeSetUp]
    public async Task Setup()
    {
        await InitializeAsync(GetVehiclesByMakeTestData.CreateTableSql, GetVehiclesByMakeTestData.InsertDataSql);
        _request = new GetVehiclesByMakeRequest() { Make = make };
        _response = await CreateClient().PostAsJsonAsync("api/Vehicles/GetVehiclesByMake", _request);
        _responseContent = ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await _response.Content.ReadAsByteArrayAsync());
    }

    [Test]
    public void TheResponseStatusCodeIsOk() => Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    [Test]
    public void TheResponseContentIsNotNull() => Assert.That(_responseContent, Is.Not.Null);

    [Test]
    public void TheResponseContainsVehicles() => Assert.That(_responseContent?.Vehicles, Is.Not.Empty);

    [Test]
    public void TheResponseOnlyContainsVehiclesOfTheExpectedMake() => Assert.That(_responseContent?.Vehicles.All(x => string.Equals(x.Make, make, StringComparison.OrdinalIgnoreCase)), Is.True);
}