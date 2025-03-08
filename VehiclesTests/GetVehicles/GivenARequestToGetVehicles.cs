using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System.Net;
using System.Text;
using System.Text.Json;
using Vehicles.Features.GetVehicles;

namespace VehiclesTests.GetVehicles;

[TestFixture("Volvo")]
[TestFixture("Hyundai")]
[TestFixture("Toyota")]
public class VehicleTests(string make) : WebApplicationFactory<Vehicles.Program>
{
    private GetVehiclesRequest? _request;
    private HttpResponseMessage? _response;
    private GetVehiclesResponse? _responseContent;

    [SetUp]
    public async Task Setup()
    {
        _request = new GetVehiclesRequest() { Make = make };
        _response = await CreateClient().PostAsync("api/GetVehicles", new StringContent(JsonSerializer.Serialize(_request), Encoding.UTF8, "application/json"));
        _responseContent = JsonSerializer.Deserialize<GetVehiclesResponse>(await _response.Content.ReadAsStringAsync());
    }

    [Test]
    public void TheResponseStatusCodeIsOk() => Assert.That(_response?.StatusCode, Is.EqualTo(HttpStatusCode.OK));

    [Test]
    public void TheResponseContentIsNotNull() => Assert.That(_responseContent, Is.Not.Null);

    [Test]
    public void TheResponseContainsVehicles() => Assert.That(_responseContent?.Vehicles, Is.Not.Empty);

    [Test]
    public void TheResponseOnlyContainsVehiclesOfTheExpectedMake() => Assert.That(_responseContent?.Vehicles.All(x => x.Make == make), Is.True);
}
