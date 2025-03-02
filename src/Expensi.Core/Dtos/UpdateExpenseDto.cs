namespace Expensi.Core.Dtos;

public record UpdateExpenseDto(
    string Title,
    string? Description,
    decimal Amount,
    string Currency,
    DateTime Date,
    Guid CategoryId);
