namespace Expensi.Core.Dtos;

public record ExpenseDto(
    Guid Id,
    string Title,
    string? Description,
    decimal Amount,
    string Currency,
    DateTime Date,
    Guid CategoryId,
    string CategoryName,
    Guid? RemitterId,
    string RemitterName);
