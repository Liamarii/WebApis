using Reqnroll;
using System.Text;
using System.Text.Json;
using Vehicles.Api.Features.GetVehiclesByMake;
using Vehicles.Infrastructure.Serialisation;

namespace VehiclesTests.Integration.GetVehiclesByMakeTests.Reqnroll
{
    [Binding]
    public class GetVehiclesByMakeStepDefinitions : CustomWebApplicationFactory<Vehicles.Program>
    {
        private HttpResponseMessage? _response;
        private GetVehiclesByMakeResponse? _responseContent;
        private const string _endpoint = "api/Vehicles/GetVehiclesByMake";

        [Given(@"a request is made for ""(.*)""")]
        public async Task GivenARequestIsMadeForMake(string make)
        {
            _response = await CreateClient().PostAsync(_endpoint, new StringContent(JsonSerializer.Serialize(new GetVehiclesByMakeRequest() { Make = make }), Encoding.UTF8, "application/json"));
            _responseContent = ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await _response.Content.ReadAsByteArrayAsync());
        }

        [When("the response is received")]
        public void WhenTheResponseIsReceived()
        {
            Assert.That(_responseContent, Is.Not.Null);
        }

        [Then(@"the response status code is ""(.*)""")]
        public void ThenTheResponseStatusCodeIsAsExpected(int statusCode)
        {
            Assert.That((int?) _response?.StatusCode, Is.EqualTo(statusCode));
        }

        [Then("the response contains a list of vehicles")]
        public void ThenTheResponseContainsAListOfVehicles()
        {
            var numberOfReturnedVehicles = _responseContent?.Vehicles.Count() ?? 0;
            Assert.That(numberOfReturnedVehicles, Is.GreaterThan(0));
        }

        [Then(@"all vehicles in the response have the make ""(.*)""")]
        public void ThenAllVehiclesInTheResponseHaveTheMake(string make)
        {
            var vehiclesMatchingTheGivenMake = _responseContent?.Vehicles.Count(x => x.Make == make);
            Assert.That(vehiclesMatchingTheGivenMake, Is.GreaterThan(0));
        }
    }
}
