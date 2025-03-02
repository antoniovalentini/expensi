namespace Expensi.Core.Dtos;

public record ExpenseDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId,
    string CategoryName);

public record CreateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId);

public record UpdateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId);

public record CategoryDto(
    Guid Id,
    string Name,
    string? Description);

public record CreateCategoryDto(
    string Name,
    string? Description);
