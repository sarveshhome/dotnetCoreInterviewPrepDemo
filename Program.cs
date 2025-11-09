using System.Text;
using System.Threading.Tasks;
using dotnetCoreInterviewPrepDemo.DAL;
using dotnetCoreInterviewPrepDemo.Extensions;
using dotnetCoreInterviewPrepDemo.Middleware;
using dotnetCoreInterviewPrepDemo.Model;
using dotnetCoreInterviewPrepDemo.Question.IntermediateQ;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder
    .Services.AddControllers()
    .AddXmlSerializerFormatters() // Enable XML format support
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.WriteIndented = true; // Pretty print JSON
    });

// Add API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;

    options.ApiVersionReader = ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new MediaTypeApiVersionReader("ver"),
        new UrlSegmentApiVersionReader()
    );

    // 👇 Controller-specific versioning conventions

    var patientConventions =
        options.Conventions.Controller<dotnetCoreInterviewPrepDemo.Controllers.PatientController>();

    patientConventions.HasApiVersion(new ApiVersion(1, 0));
    patientConventions.HasApiVersion(new ApiVersion(2, 0));
    patientConventions.HasApiVersion(new ApiVersion(3, 0));
});

// Add API Version Explorer
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// Register the middleware
builder.Services.AddTransient<MyCustomMiddleware>();

//Database add with code first
builder.Services.AddScoped<PatientDbContext>();

// Register ParrallelTestService
builder.Services.AddSingleton<ParallelTestService>();

// Register MyLogger as a singleton
builder.Services.AddSingleton<MyLogger>();

//Register GuidService as a singleton
//builder.Services.AddSingleton<GuidService>();

// Register GuidService as a scoped service
//builder.Services.AddScoped<GuidService>();

// Register GuidService as a transient service
builder.Services.AddTransient<GuidService>();

builder.Services.AddScoped<ParentService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowMyOrigin",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

// Configure JWT Authentication
builder
    .Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSecretKey =
            Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero,
        };
    });

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc(
        "v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Dotnet core API",
            Version = "v1",
            Description = "Description of Dotnet core API",
        }
    );
    c.SwaggerDoc(
        "v2",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Dotnet core API",
            Version = "v2",
            Description = "Description of Dotnet core API",
        }
    );
    c.SwaggerDoc(
        "v3",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "Dotnet core API",
            Version = "v3",
            Description = "Description of Dotnet core API",
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DotNET Core API V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "DotNET Core API V2");
        c.SwaggerEndpoint("/swagger/v3/swagger.json", "DotNET Core API V3");
    });
}

#region "Middleware"
// // Middleware A
// app.Use(async (context, next) =>
// {
//     Console.WriteLine("A: Before");
//     await next();
//     Console.WriteLine("A: After");
// });

// // // Middleware B
// app.Use(async (context, next) =>
// {
//     Console.WriteLine("B: Before");
//     await next();
//     Console.WriteLine("B: After");
// });

// // Terminal Middleware
// app.Run(async context =>
// {
//     Console.WriteLine("==> Handling Request");
//     await context.Response.WriteAsync("Hello from terminal middleware!");
// });

// // No need to call app.Run() again; it's already terminal middleware above
// app.Run(); // ✅ This is still required to start the app
#endregion

app.Use(
    async (httpContext, next) =>
    {
        Console.WriteLine($"Request Path: {httpContext.Request.Path}");
        if (httpContext.Request.Query.TryGetValue("a", out var value))
        {
            var guid = httpContext.RequestServices.GetRequiredService<GuidService>();
            Console.WriteLine($"Query parameter 'a': {value}");
            Console.WriteLine($"Guid from service: {guid.Id}");
        }
        await next();
    }
);

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

app.MapGet("/", () => "Hello World!");
app.MapGet("/html", () => "<h1>Hello World!</h1>");
app.MapGet(
    "/one/{myparam}",
    (string myparam, string a, GuidService guidService) =>
        $"Hello {myparam} , {a} and {guidService.Id}!"
);
#region "Endpoint of ParentService"
// app.MapGet("/one/{myparam}",(string myparam, string a,ParentService guidService)=>
//         $"Hello {myparam} , {a} and {guidService.Id}!");
#endregion
app.MapGet(
    "/test",
    async context =>
    {
        int number = 45;
        bool isEven = number.IsEven();
        var logger = context.RequestServices.GetRequiredService<MyLogger>();
        await context.Response.WriteAsync("Is the number " + number + " even? " + isEven + "\n");
        await context.Response.WriteAsync("test endpoint!" + "\n");
        await context.Response.WriteAsync(logger.Log("test log message!"));

        Thread.Sleep(7000);
    }
);

app.MapGet(
    "/Parallel",
    async context =>
    {
        var parallelTestService = context.RequestServices.GetRequiredService<ParallelTestService>();
        await parallelTestService.ParallelTestMethod();
        await context.Response.WriteAsync("Parallel endpoint!");
    }
);

app.MapGet(
    "/question",
    async context =>
    {
        //var result = LargestElement12.FindLargestElement();
        //DuplicatesInArray33.FindDuplicates();
        //order_of_non_zero_elements.ordernonzero();
        Count_Each_Character_in_a_String.CountEachCharacterInAStrings();
        //JoinTwoTableInLinq.JoinTowTablesInLinq();
        //await context.Response.WriteAsync($"The largest element is: {result}");
    }
);

// Terminal Middleware
// app.Run(async context =>
// {
//     Console.WriteLine("==> Handling Request");
//     await context.Response.WriteAsync("Hello from terminal middleware!");
// });

app.Run();
