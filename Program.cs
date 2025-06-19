using dotnetCoreInterviewPrepDemo.Middleware;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the middleware
builder.Services.AddTransient<MyCustomMiddleware>();


// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Dotnet core API",
        Version = "v1",
        Description = "Description of Dotnet core API"
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNET Core API V1");
    });
}
// middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<MaintenanceMiddleware>();
// Use the middleware
app.UseMiddleware<MyCustomMiddleware>();

app.UseHttpsRedirection();

app.MapGet("/test", async context =>
{
    await context.Response.WriteAsync("test endpoint!");
}); // Added semicolon here

app.Run();
