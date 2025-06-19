using System.Text.Json;

public class CustomExceptionHandleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionHandleMiddleware> _logger;

    public CustomExceptionHandleMiddleware(RequestDelegate next, ILogger<CustomExceptionHandleMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An error occurred. Please try again later.");
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        //set status code based on exception type
        var statusCode= exception switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;   

        //create error response
        var errorResponse = new
        {
            StatusCode = statusCode,
            Message = exception.Message,
            StackTrace = exception.StackTrace
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}