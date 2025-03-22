using Users.Infrastructure.FaultHandlers;

namespace Users.Infrastructure;

public interface IFaultHandling
{
    Task<HttpResponseMessage> ExponentialBackoffAsync(Func<CancellationToken, Task<HttpResponseMessage>> input, CancellationToken cancellationToken);
}

public static class FaultHandling
{
    public static IServiceCollection AddFaultHandling(this IServiceCollection services, FaultHandler faultHandler)
    {
        switch (faultHandler)
        {
            case FaultHandler.ResiliencePipelines:
                services.AddSingleton<IFaultHandling>(provider => new ResiliencePipelineFaultHandling());
                return services;
            case FaultHandler.PollyRetries:
            default:
                services.AddSingleton<IFaultHandling>(provider => new PollyFaultHandling());
                return services;
        }
    }
}