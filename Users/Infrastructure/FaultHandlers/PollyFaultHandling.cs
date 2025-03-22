using Polly;
using Polly.Retry;

namespace Users.Infrastructure.FaultHandlers
{
    public class PollyFaultHandling : IFaultHandling
    {
        private static readonly int _retryCount = 3;

        private readonly AsyncRetryPolicy<HttpResponseMessage> _exponentialBackoffRetryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(static x => !x.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: _retryCount,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetryAsync: (outcome, timespan, retryNumber, context) =>
                {
                    Logs.Add.WarningLog($"Retry {retryNumber} of {_retryCount + 1} after failed because: {outcome.Exception?.Message}");
                    return Task.CompletedTask;
                });

        public async Task<HttpResponseMessage> ExponentialBackoffAsync(Func<CancellationToken, Task<HttpResponseMessage>> input, CancellationToken cancellationToken)
        {
            try
            {
                return await _exponentialBackoffRetryPolicy.ExecuteAsync(async (ct) =>
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    return await input(ct);
                }, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                Logs.Add.WarningLog("Polly retry was canceled");
                throw;
            }
            catch (Exception ex)
            {
                Logs.Add.ErrorLog($"Polly retry error: {ex.Message}");
                throw;
            }
        }
    }
}