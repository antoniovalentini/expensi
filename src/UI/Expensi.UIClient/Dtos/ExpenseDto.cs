namespace Expensi.UIClient.Dtos;

public record ExpenseDto(
    Guid Id,
    string Title,
    decimal Amount,
    string Currency,
    DateOnly ReferenceDate,
    string Category,
    string Remitter,
    Guid CreatedById);
