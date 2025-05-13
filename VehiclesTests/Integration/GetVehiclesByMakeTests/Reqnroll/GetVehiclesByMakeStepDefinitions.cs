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

        [Given(@"a request is made for ""(.*)"" with the ""(.*)"" header")]
        public async Task GivenARequestIsMadeForMake(string make, string header)
        {
            var request = new GetVehiclesByMakeRequest { Make = make };
            var message = new HttpRequestMessage(HttpMethod.Post, _endpoint)
            {
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            message.Headers.Add("Accept", header);
            _response = await CreateClient().SendAsync(message);
        }

        [When(@"the response is received and deserialised from ""(.*)""")]
        public async Task WhenTheResponseIsReceivedAndDeserialisedAsync(string responseType)
        {
            if(_response == null)
            {
                Assert.Fail("The response was null");
            }
            else
            {
                var protobufSerialisedResponse = await _response.Content.ReadAsByteArrayAsync();

                if(responseType?.ToLower() == "protobuf")
                {
                    _responseContent = ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(protobufSerialisedResponse);
                }
                if (responseType?.ToLower() == "json")
                {
                    _responseContent = JsonSerializer.Deserialize<GetVehiclesByMakeResponse>(protobufSerialisedResponse);
                }

                Assert.That(_responseContent, Is.Not.Null);
            }
        }

        [Then(@"the response status code is ""(.*)""")]
        public void ThenTheResponseStatusCodeIsAsExpected(int statusCode)
        {
            Assert.That((int?) _response?.StatusCode, Is.EqualTo(statusCode));
        }

        [Then(@"the response only contains vehicles of the expected ""(.*)""")]
        public void ThenTheResponseOnlyContainsVehiclesOfTheExpectedMake(string make)
        {
            var vehiclesMatchingTheGivenMake = _responseContent?.Vehicles.Count(x => x.Make == make);
            Assert.That(vehiclesMatchingTheGivenMake, Is.GreaterThan(0));
        }
    }
}
