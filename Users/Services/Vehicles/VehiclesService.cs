using System.Text;
using System.Text.Json;
using Users.Exceptions;
using Users.Infrastructure;
using Users.Services.Vehicles.Models;

namespace Users.Services.Vehicles;

public interface IVehicleService
{
    public Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest make);
}

public class VehiclesService(HttpClient httpClient, IFaultHandlingPolicies policies) : IVehicleService
{
    private const string _url = "/api/Vehicles/GetVehiclesByMake";
    private readonly HttpClient _httpClient = httpClient;

    public async Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest request)
    {
        HttpResponseMessage responseMessage = await policies.ExponentialBackoffRetryPolicyAsync(() => _httpClient.PostAsync(_url, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")));

        if (!responseMessage.IsSuccessStatusCode)
        {
            Logs.Add.ErrorLog($"{this}: got a status code {responseMessage.StatusCode} when trying to perform {nameof(GetVehiclesByMake)} with reason {responseMessage.ReasonPhrase}");
            throw new ServiceUnavailableException($"Unable to call the {nameof(VehiclesService)}");
        }

        return ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await responseMessage.Content.ReadAsByteArrayAsync());
    }
}
