using Expensi.Core.Dtos;
using Expensi.Core.Models;
using Expensi.Infrastructure.Data;
using Expensi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ExpensiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ExpenseRepository>();
builder.Services.AddScoped<CategoryRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        corsBuilder => corsBuilder
            .WithOrigins("http://localhost:5173") // Replace with your frontend URL
            .AllowAnyMethod()
            .AllowAnyHeader());
});

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

// Helper function to get current user ID
Guid GetUserId(HttpContext context)
{
    return (Guid)context.Items["UserId"]!;
}

// Category endpoints
var categoryGroup = app.MapGroup("/api/categories");

// https://learn.microsoft.com/en-us/aspnet/core/fundamentals/openapi/include-metadata?view=aspnetcore-9.0&tabs=minimal-apis
categoryGroup.MapGet("/", async (CategoryRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var categories = await repository.GetAllAsync(userId);
    var categoryDtos = categories.Select(c => new CategoryDto(c.Id, c.Name, c.Description));
    return Results.Ok(categoryDtos);
}).Produces<IEnumerable<CategoryDto>>();

categoryGroup.MapGet("/{id}", async (Guid id, CategoryRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var category = await repository.GetByIdAsync(id, userId);

    if (category == null)
        return Results.NotFound();

    return Results.Ok(new CategoryDto(category.Id, category.Name, category.Description));
}).Produces<CategoryDto>();

categoryGroup.MapPost("/", async (CreateCategoryDto dto, CategoryRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var category = new Category
    {
        Id = Guid.NewGuid(),
        Name = dto.Name,
        Description = dto.Description,
        UserId = userId
    };

    await repository.CreateAsync(category);
    return Results.Created($"/api/categories/{category.Id}",
        new CategoryDto(category.Id, category.Name, category.Description));
}).Produces<CategoryDto>();

categoryGroup.MapPut("/{id}", async (Guid id, CreateCategoryDto dto, CategoryRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var category = new Category
    {
        Id = id,
        Name = dto.Name,
        Description = dto.Description,
        UserId = userId
    };

    var updatedCategory = await repository.UpdateAsync(category);

    if (updatedCategory == null)
        return Results.NotFound();

    return Results.Ok(new CategoryDto(updatedCategory.Id, updatedCategory.Name, updatedCategory.Description));
}).Produces<CategoryDto>();

categoryGroup.MapDelete("/{id}", async (Guid id, CategoryRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var success = await repository.DeleteAsync(id, userId);

    if (!success)
        return Results.NotFound();

    return Results.NoContent();
});

// Expense endpoints
var expenseGroup = app.MapGroup("/api/expenses");

expenseGroup.MapGet("/", async (ExpenseRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var expenses = await repository.GetAllAsync(userId);
    var expenseDtos = expenses.Select(e => new ExpenseDto(
        e.Id,
        e.Title,
        e.Description,
        e.Amount,
        e.Currency,
        e.Date,
        e.CategoryId,
        e.Category?.Name ?? "Unknown",
        e.FamilyMemberId,
        e.FamilyMember?.Name ?? "Family"));
    return Results.Ok(expenseDtos);
}).Produces<IEnumerable<ExpenseDto>>();

expenseGroup.MapGet("/{id}", async (Guid id, ExpenseRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var expense = await repository.GetByIdAsync(id, userId);

    if (expense == null)
        return Results.NotFound();

    return Results.Ok(new ExpenseDto(
        expense.Id,
        expense.Title,
        expense.Description,
        expense.Amount,
        expense.Currency,
        expense.Date,
        expense.CategoryId,
        expense.Category?.Name ?? "Unknown",
        expense.FamilyMemberId,
        expense.FamilyMember?.Name ?? "Family"));
}).Produces<ExpenseDto>();

expenseGroup.MapPost("/", async (CreateExpenseDto dto, ExpenseRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var expense = new Expense
    {
        Id = Guid.NewGuid(),
        Title = dto.Title,
        Description = dto.Description,
        Amount = dto.Amount,
        Currency = dto.Currency,
        Date = dto.Date,
        CategoryId = dto.CategoryId,
        UserId = userId,
        FamilyMemberId = dto.FamilyMemberId
    };

    var createdExpense = await repository.CreateAsync(expense);
    return Results.Created($"/api/expenses/{expense.Id}",
        new ExpenseDto(createdExpense.Id, createdExpense.Title, createdExpense.Description, createdExpense.Amount, createdExpense.Currency,
            createdExpense.Date,
            createdExpense.CategoryId, createdExpense.Category?.Name ?? "Unknown",
            createdExpense.FamilyMemberId, createdExpense.FamilyMember?.Name ?? "Family"));
}).Produces<ExpenseDto>();

expenseGroup.MapPut("/{id}", async (Guid id, UpdateExpenseDto dto, ExpenseRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var expense = new Expense
    {
        Id = id,
        Title = dto.Title,
        Description = dto.Description,
        Amount = dto.Amount,
        Currency = dto.Currency,
        Date = dto.Date,
        CategoryId = dto.CategoryId,
        UserId = userId,
        FamilyMemberId = dto.FamilyMemberId
    };

    var updatedExpense = await repository.UpdateAsync(expense);

    if (updatedExpense == null)
        return Results.NotFound();

    return Results.Ok(new ExpenseDto(
        updatedExpense.Id,
        updatedExpense.Title,
        updatedExpense.Description,
        updatedExpense.Amount,
        updatedExpense.Currency,
        updatedExpense.Date,
        updatedExpense.CategoryId,
        updatedExpense.Category?.Name ?? "Unknown",
        updatedExpense.FamilyMemberId,
        updatedExpense.FamilyMember?.Name ?? "Family"));
}).Produces<ExpenseDto>();

expenseGroup.MapDelete("/{id}", async (Guid id, ExpenseRepository repository, HttpContext context) =>
{
    var userId = GetUserId(context);
    var success = await repository.DeleteAsync(id, userId);

    if (!success)
        return Results.NotFound();

    return Results.NoContent();
});

app.Run();
