using System.Reflection;
using Expensi.Infrastructure;
using FluentValidation;
using FluentValidation.AspNetCore;

const string defaultCorsPolicy = "AllowSpecificOrigin";

var builder = WebApplication.CreateBuilder(args);

// Add environment variables to configuration
builder.Configuration.AddEnvironmentVariables();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddSchemaTransformer((schema, context, _) =>
{
    if (context.JsonTypeInfo.Type == typeof(decimal))
    {
        schema.Format = "decimal";
    }
    return Task.CompletedTask;
}));

builder.Services.AddPersistence();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(defaultCorsPolicy,
        corsBuilder => corsBuilder
            .WithOrigins("http://localhost:5173", "http://localhost:3000", "https://v0-simple-budgeting-app-brown.vercel.app/") // Replace with your frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapGet("/api/version", () => Assembly.GetExecutingAssembly().GetName().Version?.ToString());

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors(defaultCorsPolicy);

// Simple "authentication" middleware - in a real app, use proper auth
app.Use(async (context, next) =>
{
    // For demo purposes, we're setting a fixed user ID
    // In a real app, this would come from authentication
    context.Items["UserId"] = Guid.Parse("11111111-1111-1111-1111-111111111111");
    await next();
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// Necessary for internal visibility in integration tests
namespace Expensi.Api
{
    public partial class Program { }
}
