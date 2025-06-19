//Measure endpoint performance for optimization
using System.Diagnostics;

public class PerformanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<PerformanceMiddleware> _logger;    
    public PerformanceMiddleware(RequestDelegate next, ILogger<PerformanceMiddleware> logger){
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext context)
    {
        var sw = Stopwatch.StartNew();
        await _next(context);
        sw.Stop();
        if(sw.ElapsedMilliseconds > 500)
        {
            Console.WriteLine($"Request {context.Request.Path} took {sw.ElapsedMilliseconds}ms");
        }
    }

}