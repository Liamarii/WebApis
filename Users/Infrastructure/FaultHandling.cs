using Users.Infrastructure.FaultHandlers;

namespace Users.Infrastructure;

public interface IFaultHandling
{
    Task<HttpResponseMessage> ExponentialBackoffAsync(Func<CancellationToken, Task<HttpResponseMessage>> input, CancellationToken cancellationToken);
}

public enum FaultHandler
{
    ResiliencePipelines,
    PollyRetries
}

public static class FaultHandling
{
    public static IServiceCollection AddFaultHandling(this IServiceCollection services, FaultHandler faultHandler)
    {
        switch (faultHandler)
        {
            case FaultHandler.ResiliencePipelines:
                services.AddSingleton<IFaultHandling>(_ => new ResiliencePipelineFaultHandling());
                return services;
            default:
                services.AddSingleton<IFaultHandling>(_ => new PollyFaultHandling());
                return services;
        }
    }
}