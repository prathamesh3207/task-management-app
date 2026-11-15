using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;
using System.Security.Claims;
using Task_API.Data;
using Task_API.Services;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IAuth, AuthService>();
builder.Services.AddScoped<ITaskService_cs, TaskService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthorization(options =>
{
    // Create a dynamic policy named "OwnerOrAdmin"
    options.AddPolicy("OwnerOrAdmin", policy =>
        policy.RequireAssertion(context =>
        {
            // 1. Extract the HttpContext to look at the incoming URL parameters
            var httpContext = context.Resource as HttpContext;
            var targetUserId = httpContext?.Request.RouteValues["userId"]?.ToString();

            // 2. Extract identity values out of the caller's JWT token
            var currentUserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = context.User.IsInRole("Admin");

            // 3. THE LOGIC RULE: Grant access if they are an admin OR if their ID matches the target URL
            return isAdmin || currentUserId == targetUserId;
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

try
{
    Log.Information("Starting web host");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message, "An error occurred while starting the web host");

}
finally
{
    Log.Information("Shutting down web host");
    Log.CloseAndFlush();
}
