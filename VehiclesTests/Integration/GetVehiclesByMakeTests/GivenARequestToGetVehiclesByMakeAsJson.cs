using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Vehicles.Api.Features.GetVehiclesByMake;

namespace VehiclesTests.Integration.GetVehiclesByMakeTests;

[TestFixture("Honda")]
[TestFixture("Ford")]
[TestFixture("Nissan")]
[TestFixture("Jeep")]
[TestFixture("Tesla")]
[Parallelizable]
public class GivenARequestToGetVehiclesByMakeAsJson(string make) : CustomWebApplicationFactory<Vehicles.Program>
{
    private GetVehiclesByMakeRequest? _request;
    private HttpResponseMessage? _response;
    private GetVehiclesByMakeResponse? _responseContent;

    [OneTimeSetUp]
    public async Task Setup()
    {
        HttpClient client = CreateClient();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        _request = new GetVehiclesByMakeRequest() { Make = make };
        _response = await client.PostAsync("api/Vehicles/GetVehiclesByMake", new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"), CancellationToken.None);
        _responseContent = JsonSerializer.Deserialize<GetVehiclesByMakeResponse>(await _response.Content.ReadAsStringAsync());
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
