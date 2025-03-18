using Polly;
using Polly.Retry;

namespace Users.Infrastructure
{
    public interface IFaultHandlingPolicies
    {
        Task<HttpResponseMessage> ExponentialBackoffRetryPolicyAsync(Func<Task<HttpResponseMessage>> input);
    }

    public class FaultHandlingPolicies : IFaultHandlingPolicies
    {
        public FaultHandlingPolicies(int retryCount) => _retryCount = retryCount;

        private static int _retryCount = 0;

        private readonly AsyncRetryPolicy<HttpResponseMessage> _exponentialBackoffRetryPolicy = Policy
            .Handle<HttpRequestException>() //This is just to test it with the service it's calling not turned on.
            .OrResult<HttpResponseMessage>(static x => !x.IsSuccessStatusCode)
            .WaitAndRetryAsync(
                retryCount: _retryCount,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryNumber, context) =>
                {
                    Logs.Add.WarningLog($"Retry {retryNumber} of {_retryCount} after {timespan.TotalSeconds} seconds due to {outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
                });

        public async Task<HttpResponseMessage> ExponentialBackoffRetryPolicyAsync(Func<Task<HttpResponseMessage>> input)
        {
            return await _exponentialBackoffRetryPolicy.ExecuteAsync(input);
        }
    }
}
