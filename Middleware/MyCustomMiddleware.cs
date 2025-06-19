using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace dotnetCoreInterviewPrepDemo.Middleware
{
    public class MyCustomMiddleware : IMiddleware
    {
        private readonly ILogger<MyCustomMiddleware> _logger;

        public MyCustomMiddleware(ILogger<MyCustomMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                _logger.LogInformation($"Request started: {context.Request.Path}");
                
                // Execute the next middleware in the pipeline
                await next(context);
                
                _logger.LogInformation($"Request completed: {context.Request.Path}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request");
                throw;
            }
        }
    }
}
