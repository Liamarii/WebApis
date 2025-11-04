using Polly;
using Users.Exceptions;
using Users.Services.Vehicles;

namespace Users.Infrastructure;

public static class ResiliencePipelineProvider
{
    private static readonly ResiliencePipeline<HttpResponseMessage> _pipeline = BuildDefaultPipeline();

    private static ResiliencePipeline<HttpResponseMessage> BuildDefaultPipeline()
    {
        var builder = new ResiliencePipelineBuilder<HttpResponseMessage>();
        ConfigureDefaultPipeline(builder);
        return builder.Build();
    }

    public static void ConfigureDefaultPipeline(ResiliencePipelineBuilder<HttpResponseMessage> builder)
    {
        builder
            .AddFallback(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(r => !r.IsSuccessStatusCode),
                FallbackAction = _ => throw new ServiceUnavailableException($"Unable to call the {nameof(VehiclesService)}"),
                OnFallback = _ =>
                {
                    Logs.Add.ErrorLog("Fallback response: operation failed after all retries");
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(r => !r.IsSuccessStatusCode),
                BreakDuration = TimeSpan.FromSeconds(30),
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 10,
                FailureRatio = 0.5,
                OnClosed = args =>
                {
                    Logs.Add.WarningLog($"{args.Context.OperationKey} : circuit closed. Reason: {args.Outcome}");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = args =>
                {
                    Logs.Add.InfoLog($"{args.Context.OperationKey} : circuit half-open");
                    return ValueTask.CompletedTask;
                },
                OnOpened = args =>
                {
                    Logs.Add.InfoLog($"{args.Context.OperationKey} : circuit opened. Exception: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .AddRetry(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult(r => !r.IsSuccessStatusCode),
                Delay = TimeSpan.FromSeconds(2),
                UseJitter = true,
                BackoffType = DelayBackoffType.Constant,
                MaxRetryAttempts = 2,
                OnRetry = args =>
                {
                    Logs.Add.InfoLog($"Retry #{args.AttemptNumber}");
                    return ValueTask.CompletedTask;
                }
            });
    }

    public static ResiliencePipeline<HttpResponseMessage> GetDefaultPipeline() => _pipeline;
}