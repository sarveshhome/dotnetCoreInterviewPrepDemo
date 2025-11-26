using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace dotnetCoreInterviewPrepDemo.Middleware;
public class RateLimitingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IDatabase _redis;
    private readonly int _maxRequests;
    private readonly TimeSpan _window;

    public RateLimitingMiddleware(RequestDelegate next, IConnectionMultiplexer redis)
    {
        _next = next;
        _redis = redis.GetDatabase();
        _maxRequests = 100; // requests per window
        _window = TimeSpan.FromMinutes(1);
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        var key = $"rate_limit:{clientIp}";

        var current = await _redis.StringIncrementAsync(key);
        if (current == 1)
            await _redis.KeyExpireAsync(key, _window);

        if (current > _maxRequests)
        {
            context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            await context.Response.WriteAsync("Rate limit exceeded");
            return;
        }

        await _next(context);
    }
}