namespace Expensi.UIClient.Dtos;

public record CreateExpenseDto(
    string Title,
    decimal Amount,
    string Currency,
    DateOnly ReferenceDate,
    string Category,
    string Remitter);
