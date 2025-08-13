using dotnetCoreInterviewPrepDemo.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using dotnetCoreInterviewPrepDemo.Model;
using dotnetCoreInterviewPrepDemo.DAL;
using dotnetCoreInterviewPrepDemo.Extensions;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Register the middleware
builder.Services.AddTransient<MyCustomMiddleware>();
//Database add with code first 
builder.Services.AddScoped<PatientDbContext>();


// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") 
            ?? builder.Configuration["Jwt:SecretKey"]
            ?? throw new InvalidOperationException("JWT secret key not configured");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

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


// Middleware A
app.Use(async (context, next) =>
{
    Console.WriteLine("A: Before");
    await next();
    Console.WriteLine("A: After");
});

// Middleware B
app.Use(async (context, next) =>
{
    Console.WriteLine("B: Before");
    await next();
    Console.WriteLine("B: After");
});

// // Terminal Middleware
// app.Run(async context =>
// {
//     Console.WriteLine("==> Handling Request");
//     await context.Response.WriteAsync("Hello from terminal middleware!");
// });

// // No need to call app.Run() again; it's already terminal middleware above
// app.Run(); // ✅ This is still required to start the app

// middleware pipeline
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<PerformanceMiddleware>();
app.UseMiddleware<MaintenanceMiddleware>();
// Use the middleware
app.UseMiddleware<MyCustomMiddleware>();



app.UseHttpsRedirection();

// Use CORS
app.UseCors("AllowMyOrigin");

// Add authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/test", async context =>
{
    int number =45;
    bool isEven = number.IsEven();
    MyLogger logger = new MyLogger();
    await context.Response.WriteAsync("Is the number " + number + " even? " + isEven + "\n");
    await context.Response.WriteAsync("test endpoint!"+ "\n");
    await context.Response.WriteAsync(logger.Log("test log message!"));
});


// Terminal Middleware
app.Run(async context =>
{
    Console.WriteLine("==> Handling Request");
    await context.Response.WriteAsync("Hello from terminal middleware!");
});

app.Run();
