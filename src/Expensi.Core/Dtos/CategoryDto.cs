namespace Expensi.Core.Dtos;

public record CategoryDto(
    Guid Id,
    string Name,
    string? Description);