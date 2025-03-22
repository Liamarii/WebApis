using Polly;
using Polly.Retry;

namespace Users.Infrastructure.FaultHandlers
{
    public class ResiliencePipelineFaultHandling : IFaultHandling
    {
        private static readonly int _retryCount = 3;

        private readonly ResiliencePipeline _resiliencePipeline = new ResiliencePipelineBuilder()
            .AddRetry(new RetryStrategyOptions
            {
                MaxRetryAttempts = _retryCount,
                Delay = TimeSpan.FromSeconds(2),
                UseJitter = true,
                BackoffType = DelayBackoffType.Exponential,
                OnRetry = args =>
                {
                    Logs.Add.WarningLog($"Retry {args.AttemptNumber + 1} of {_retryCount + 1} failed because: {args.Outcome.Exception?.Message}");
                    return ValueTask.CompletedTask;
                }
            })
            .AddTimeout(TimeSpan.FromSeconds(30))
            .Build();

        public async Task<HttpResponseMessage> ExponentialBackoffAsync(Func<CancellationToken, Task<HttpResponseMessage>> input, CancellationToken cancellationToken)
        {
            try
            {
                return await _resiliencePipeline.ExecuteAsync(async token =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return await input(token);
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Logs.Add.WarningLog("Resilience pipeline was canceled");
                throw;
            }
            catch (Exception ex)
            {
                Logs.Add.ErrorLog($"Resilience pipeline error: {ex.Message}");
                throw;
            }
        }
    }
}