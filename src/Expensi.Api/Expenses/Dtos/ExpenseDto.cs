namespace Expensi.Api.Expenses.Dtos;

public record ExpenseDto(
    Guid Id,
    string Title,
    decimal Amount,
    string Currency,
    DateTime Date,
    string Category,
    string Remitter,
    Guid CreatedById);
