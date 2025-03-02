namespace Expensi.Core.Dtos;

public record CreateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    string Currency,
    DateTime Date,
    Guid CategoryId);
