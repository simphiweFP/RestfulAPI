using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using ShippingApi.Core;
using ShippingApi.Data;
using ShippingApi.Services;
using ShippingApi.UseCase;
using ShippingApi.UseCase.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!builder.Environment.IsEnvironment("Testing") && string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "The required connection string 'DefaultConnection' is not configured. " +
        "Provide it through user secrets or an environment-specific configuration source.");
}

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var problemDetails = new Microsoft.AspNetCore.Mvc.ValidationProblemDetails(context.ModelState)
            {
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest,
                Instance = context.HttpContext.Request.Path,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };
            problemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;

            return new BadRequestObjectResult(problemDetails)
            {
                ContentTypes = { "application/problem+json" }
            };
        };
    });

// Add services to the container.

// Add DbContext registration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddHealthChecks();
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = HttpLoggingFields.RequestMethod |
                            HttpLoggingFields.RequestPath |
                            HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Order dependencies
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IOrderService>(provider =>
{
    var orderService = provider.GetRequiredService<OrderService>();
    var logger = provider.GetRequiredService<ILogger<LoggingServiceDecorator>>();
    return new LoggingServiceDecorator(orderService, logger);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ShippingApi",
        Version = "v1",
        Description = "A portfolio-ready shipping API with addresses, drivers, and orders."
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    }

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter a valid JWT token using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var unitOfWork = services.GetRequiredService<IUnitOfWork>();

    await unitOfWork.RunMigrationsAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "ShippingApi v1");
    });
}

if (!app.Environment.IsEnvironment("Testing"))
{
    app.UseHttpsRedirection();
}

app.UseHttpLogging();

app.UseMiddleware<ShippingApi.Middleware.GlobalExceptionHandler>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health").AllowAnonymous();

app.Run();

public partial class Program
{
}

