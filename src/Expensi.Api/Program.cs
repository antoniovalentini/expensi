using Expensi.Api;
using Expensi.Api.Expenses;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options => options.AddSchemaTransformer((schema, context, _) =>
{
    if (context.JsonTypeInfo.Type == typeof(decimal))
    {
        schema.Format = "decimal";
    }
    return Task.CompletedTask;
}));

builder.Services.AddDbContext<ExpensiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ExpenseRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder => corsBuilder
            .WithOrigins("http://localhost:5173","http://localhost:3000", "https://v0-simple-budgeting-app-brown.vercel.app/") // Replace with your frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

// Simple "authentication" middleware - in a real app, use proper auth
app.Use(async (context, next) =>
{
    // For demo purposes, we're setting a fixed user ID
    // In a real app, this would come from authentication
    context.Items["UserId"] = Guid.Parse("11111111-1111-1111-1111-111111111111");
    await next();
});

app.MapControllers();

app.Run();
