using Polly;
using Polly.Registry;
using Users.Services.Vehicles.Models;

namespace Users.Services.Vehicles;

public interface IVehiclesService
{
    public Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest make, CancellationToken cancellationToken);
}

public class VehiclesService(HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipelineProvider) : IVehiclesService
{
    private const string _url = "/api/Vehicles/GetVehiclesByMake";
    private readonly HttpClient _httpClient = httpClient;

    public async Task<GetVehiclesByMakeResponse> GetVehiclesByMake(GetVehiclesByMakeRequest getVehiclesByMakeRequest, CancellationToken cancellationToken)
    {
        var context = ResilienceContextPool.Shared.Get(cancellationToken);

        try
        {
            var responseMessage = await resiliencePipelineProvider
                .GetPipeline<HttpResponseMessage>("defaultPipeline")
                .ExecuteAsync(async (args) => await _httpClient.PostAsJsonAsync(_url, getVehiclesByMakeRequest, args.CancellationToken), context);

            return ProtobufHelper.DeserialiseFromProtobuf<GetVehiclesByMakeResponse>(await responseMessage.Content.ReadAsByteArrayAsync(cancellationToken));
        }
        finally
        {
            ResilienceContextPool.Shared.Return(context);
        }
    }
}
