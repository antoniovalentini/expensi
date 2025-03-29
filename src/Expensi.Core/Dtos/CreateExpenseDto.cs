namespace Expensi.Core.Dtos;

public record CreateExpenseDto(
    string Title,
    decimal Amount,
    string Currency,
    DateTime Date,
    string Category,
    string Remitter);
