using System.Text;
using System.Text.Json;
using Vehicles.Api.Features.GetVehiclesByMake;
using Vehicles.Infrastructure.Serialisation;

namespace VehiclesTests.Integration.Infrastructure
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendJsonRequestAsync<T>(this HttpClient httpClient, string endpoint, T requestObject, (string name, IEnumerable<string?> values)? requestHeaders)
        {
            ArgumentNullException.ThrowIfNull(httpClient);
            ArgumentNullException.ThrowIfNull(endpoint);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(
                    content: JsonSerializer.Serialize(requestObject),
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"),
            };

            requestMessage.Headers.Accept.Clear();
            if (requestHeaders is (var name, var values))
            {
                requestMessage.Headers.Add(name, values);
            }

            return await httpClient
                .SendAsync(requestMessage);
        }

        public static async Task<GetVehiclesByMakeResponse?> DeserialiseResponseAsync(this HttpResponseMessage httpResponseMessage, string responseType)
        {
            var responseContent = await httpResponseMessage.Content.ReadAsByteArrayAsync();

            if (responseContent == null || responseContent.Length == 0)
            {
                throw new InvalidDataException("The response content could not be deserialized as expected.");
            }
            else if (string.Equals(responseType, "protobuf", StringComparison.OrdinalIgnoreCase))
            {
                return ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(responseContent);
            }
            else if (string.Equals(responseType, "json", StringComparison.OrdinalIgnoreCase))
            {
                return JsonSerializer.Deserialize<GetVehiclesByMakeResponse>(responseContent);
            }
            else
            {
                throw new InvalidDataException($"The response content could not be deserialized in {responseType} as expected.");
            }
        }
    }
}