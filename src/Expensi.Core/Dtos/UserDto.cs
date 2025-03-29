namespace Expensi.Core.Dtos;

public record UserDto(Guid Id, string Username, string Email);
public record CreateUserDto(string Username, string Email, string Password);
public record UpdateUserDto(string Username, string Email, string? Password = null);
