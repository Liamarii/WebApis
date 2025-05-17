using Microsoft.AspNetCore.Mvc;
using System.Threading.RateLimiting;

namespace Users.Infrastructure;

public static class RateLimiting
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services, TimeSpan window, int permitLimit)
    {
        services.AddRateLimiter(options =>
        {
            options.AddPolicy("SlidingWindowPolicy", context =>
                RateLimitPartition.GetSlidingWindowLimiter(
                    partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
                    factory: _ => new SlidingWindowRateLimiterOptions
                    {
                        PermitLimit = permitLimit,
                        Window = window,
                        SegmentsPerWindow = 4
                    }));

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status429TooManyRequests,
                    Title = "Too Many Requests",
                    Detail = "stop it."
                };

                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsJsonAsync(problemDetails, token);
            };
        });

        return services;
    }
}
