using Polly;
using Users.Services.Vehicles.Models;

namespace Users.Services.Vehicles;

public interface IVehicleService
{
    public Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest make, CancellationToken cancellationToken);
}

public class VehiclesService(HttpClient httpClient, ResiliencePipeline<HttpResponseMessage> resiliencePipeline) : IVehicleService
{
    private const string _url = "/api/Vehicles/GetVehiclesByMake";
    private readonly HttpClient _httpClient = httpClient;

    public async Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest getVehiclesByMakeRequest, CancellationToken cancellationToken)
    {
        var context = ResilienceContextPool.Shared.Get(cancellationToken);

        try
        {
            HttpResponseMessage responseMessage = await resiliencePipeline
                .ExecuteAsync(async (args) => await _httpClient.PostAsJsonAsync(_url, getVehiclesByMakeRequest, args.CancellationToken), context);

            return ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await responseMessage.Content.ReadAsByteArrayAsync(cancellationToken));
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }
}
