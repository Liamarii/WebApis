using Polly;
using Users.Exceptions;
using Users.Services.Vehicles;

namespace Users.Infrastructure.FaultHandlers
{
    public static class ResiliencePipelines
    {
        public static readonly ResiliencePipeline<HttpResponseMessage> DefaultResiliencePipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
            .AddFallback(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult((args) => !args.IsSuccessStatusCode),
                FallbackAction = (_) => throw new ServiceUnavailableException($"Unable to call the {nameof(VehiclesService)}"),
                OnFallback = (_) =>
                {
                    Logs.Add.ErrorLog("Fallback response: operation failed after all retries");
                    return ValueTask.CompletedTask;
                }
            })
            .AddCircuitBreaker(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult((args) => !args.IsSuccessStatusCode),
                BreakDuration = TimeSpan.FromSeconds(30),
                SamplingDuration = TimeSpan.FromSeconds(30),
                MinimumThroughput = 10,
                FailureRatio = 0.5,
                OnClosed = (args) =>
                {
                    Logs.Add.WarningLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} Pipeline has been closed because: {args.Outcome}");
                    return ValueTask.CompletedTask;
                },
                OnHalfOpened = (args) =>
                {
                    Logs.Add.InfoLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} Pipeline is attempting a single test request following a pipeline closure");
                    return ValueTask.CompletedTask;
                },
                OnOpened = (args) =>
                {
                    Logs.Add.InfoLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} Pipeline is opening. Reason: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .AddRetry(new()
            {
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<Exception>()
                    .HandleResult((args) => !args.IsSuccessStatusCode),
                Delay = TimeSpan.FromSeconds(0),
                UseJitter = true,
                BackoffType = DelayBackoffType.Constant,
                OnRetry = (args) =>
                {
                    Logs.Add.InfoLog($"Retry attempt #{args.AttemptNumber} for {nameof(ResiliencePipeline)}");
                    return ValueTask.CompletedTask;
                },
                MaxRetryAttempts = 2
            })
            .Build();
    }
}