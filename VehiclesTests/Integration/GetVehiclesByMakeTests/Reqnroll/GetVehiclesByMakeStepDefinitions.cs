using Reqnroll;
using Vehicles.Api.Features.GetVehiclesByMake;
using VehiclesTests.Integration.Infrastructure;

namespace VehiclesTests.Integration.GetVehiclesByMakeTests.Reqnroll
{
    [Binding]
    public sealed class GetVehiclesByMakeStepDefinitions(ScenarioContext context) : CustomWebApplicationFactory<Vehicles.Program>
    {
        private HttpClient? _httpClient;
        private const string _endPoint = "api/Vehicles/GetVehiclesByMake";

        [BeforeScenario]
        public void BeforeScenario() => _httpClient = CreateClient();

        [AfterScenario]
        public void AfterScenario() => _httpClient?.Dispose();

        [Given(@"a request is made for ""(.*)"" with the ""(.*)"" header")]
        public async Task GivenARequestIsMadeForMake(string make, string requestHeader)
        {
            ArgumentNullException.ThrowIfNull(_httpClient);
            context["response"] = await _httpClient.SendJsonRequestAsync(_endPoint, new GetVehiclesByMakeRequest { Make = make }, ("Accept", [requestHeader]));
        }

        [When(@"the response is received and deserialised from ""(.*)""")]
        public async Task WhenTheResponseIsReceivedAndDeserialisedAsync(string responseType)
        {
            var response = (HttpResponseMessage)context["response"];

            if(response.IsSuccessStatusCode)
            {
                context["responseContent"] = await response.DeserialiseResponseAsync(responseType);
                Assert.That(context["responseContent"], Is.Not.Null);
            }
            else
            {
                Assert.Fail($"status code returned: {response.StatusCode} when expected a success");
            }
        }

        [Then(@"the response status code is ""(.*)""")]
        public void ThenTheResponseStatusCodeIsAsExpected(int statusCode)
        {
            var response = (HttpResponseMessage)context["response"];
            Assert.That((int?) response?.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"the response only contains vehicles of the expected ""(.*)""")]
        public void ThenTheResponseOnlyContainsVehiclesOfTheExpectedMake(string make)
        {
            var getVehiclesByMakeResponse = (GetVehiclesByMakeResponse) context["responseContent"];
            var numberOfMatchingVehicles = getVehiclesByMakeResponse.Vehicles.Count(x => x.Make == make);
            Assert.That(numberOfMatchingVehicles, Is.AtLeast(1));
        }
    }
}