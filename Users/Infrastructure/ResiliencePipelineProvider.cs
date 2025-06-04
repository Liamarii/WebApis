using Polly;
using Users.Exceptions;
using Users.Services.Vehicles;

namespace Users.Infrastructure
{
    public static class ResiliencePipelineProvider
    {
        private static readonly ResiliencePipeline<HttpResponseMessage> _pipeline;
        
        static ResiliencePipelineProvider()
        {
            _pipeline = new ResiliencePipelineBuilder<HttpResponseMessage>()
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
                        Logs.Add.WarningLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} closed. Reason: {args.Outcome}");
                        return ValueTask.CompletedTask;
                    },
                    OnHalfOpened = args =>
                    {
                        Logs.Add.InfoLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} half-open test triggered");
                        return ValueTask.CompletedTask;
                    },
                    OnOpened = args =>
                    {
                        Logs.Add.InfoLog($"{args.Context.OperationKey} : {nameof(ResiliencePipeline)} opened. Exception: {args.Outcome.Exception?.Message}");
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
                        Logs.Add.InfoLog($"Retry #{args.AttemptNumber} for {nameof(ResiliencePipeline)}");
                        return ValueTask.CompletedTask;
                    }
                })
                .Build();
        }

        public static ResiliencePipeline<HttpResponseMessage> GetDefaultPipeline() => _pipeline;
    }
}