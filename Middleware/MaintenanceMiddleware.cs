public class MaintenanceMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MaintenanceMiddleware> _logger;

    public MaintenanceMiddleware(RequestDelegate next, ILogger<MaintenanceMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the application is in maintenance mode
        if (IsInMaintenanceMode())
        {
            // Log the maintenance mode status
            _logger.LogWarning("Application is in maintenance mode. Returning 503 Service Unavailable.");

            // Return a 503 Service Unavailable response
            context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            await context.Response.WriteAsync("The application is currently undergoing maintenance. Please try again later.");
            return;
        }

        // Call the next middleware in the pipeline
        await _next(context);
    }

    private bool IsInMaintenanceMode()
    {
        // Logic to determine if the application is in maintenance mode
        // This could be a configuration setting, environment variable, etc.
        return false; // Replace with actual logic
    }
}