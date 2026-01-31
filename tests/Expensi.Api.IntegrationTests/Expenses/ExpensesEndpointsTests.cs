using System.Net;
using System.Net.Http.Json;
using Expensi.Api.Expenses.Dtos;
using Microsoft.AspNetCore.Mvc;
using Shouldly;

namespace Expensi.Api.IntegrationTests.Expenses;

public class ExpensesEndpointsTests(ExpensesApiFactory factory) : IntegrationTestBase(factory)
{
    [Fact]
    public async Task CreateExpense_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var dto = new CreateExpenseDto(
            "Test Grocery Shopping",
            55.75m,
            "EUR",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Test Category",
            "Test Category SubType",
            "Family"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/expenses", dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();

        var expenseDto = await response.Content.ReadFromJsonAsync<ExpenseDto>();
        expenseDto.ShouldNotBeNull();
        expenseDto.Id.ShouldNotBe(Guid.Empty);
    }

    [Fact]
    public async Task CreateExpense_WithInvalidData_ShouldReturnBadRequest()
    {
        // Arrange
        var dto = new CreateExpenseDto(
            "", // Invalid: Empty description
            -10m, // Invalid: Negative amount
            "", // Invalid: Empty currency
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Test Category",
            "Test Category SubType",
            "Family"
        );

        // Act
        var response = await Client.PostAsJsonAsync("/api/expenses", dto);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        // Optional: Check for validation error details in the response body
        var validationProblem = await response.Content.ReadFromJsonAsync<ValidationProblemDetails>(); // Using standard ProblemDetails
        validationProblem.ShouldNotBeNull();
        validationProblem?.Errors.ShouldContainKey("Title"); // Check specific validation errors
        validationProblem?.Errors.ShouldContainKey("Amount");
        validationProblem?.Errors.ShouldContainKey("Currency");
    }

     [Fact]
    public async Task GetExpenseById_WhenExists_ShouldReturnOkAndExpense()
    {
        // Arrange: Need to create an expense first
        var dto = new CreateExpenseDto(
            "Test Grocery Shopping",
            55.75m,
            "EUR",
            DateOnly.FromDateTime(DateTime.UtcNow),
            "Test Category",
            "Test Category SubType",
            "Family"
        );
        var createResponse = await Client.PostAsJsonAsync("/api/expenses", dto);
        createResponse.EnsureSuccessStatusCode();
        var createdLocation = createResponse.Headers.Location ?? throw new Exception("Location header not found");
        var expenseId = Guid.Parse(createdLocation.Segments.Last());

        // Act
        var response = await Client.GetAsync($"/api/expenses/{expenseId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var expenseDto = await response.Content.ReadFromJsonAsync<ExpenseDto>(); // Use your response DTO
        expenseDto.ShouldNotBeNull();
        expenseDto.Id.ShouldBe(expenseId);
        expenseDto.Title.ShouldBe(dto.Title);
    }

    [Fact]
    public async Task GetExpenseById_WhenNotExists_ShouldReturnNotFound()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();

        // Act
        var response = await Client.GetAsync($"/api/expenses/{nonExistentId}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }
}
