using System.Text.Json;
using System.Text;
using Users.Services.Vehicles.Models;

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
        string responseMessageContentString = await responseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<GetVehiclesByMakeResponse>(responseMessageContentString);
    }
}
