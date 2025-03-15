using System.Text.Json;
using System.Text;
using Users.Services.Vehicles.Models;
using Users.Exceptions;
using Users.Infrastructure;

namespace Users.Services.Vehicles;

public interface IVehicleService
{
    public Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest make);
}

public class VehiclesService(HttpClient httpClient) : IVehicleService
{
    private const string _url = "/api/Vehicles/GetVehiclesByMake";
    private readonly HttpClient _httpClient = httpClient;

    public async Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest request)
    { 
        HttpResponseMessage responseMessage = await _httpClient.PostAsync(_url, new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));

        if (!responseMessage.IsSuccessStatusCode)
        {
            Logs.Add.ErrorLog($"{this}: got a status code {responseMessage.StatusCode} when trying to perform {nameof(GetVehiclesByMake)} with reason {responseMessage.ReasonPhrase}");
            throw new ServiceUnavailableException($"Unable to call the {nameof(VehiclesService)}");
        }

        return ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await responseMessage.Content.ReadAsByteArrayAsync());
    }
}
