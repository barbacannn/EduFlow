namespace EduFlow.Api.Contracts.Users;

public record UserDto(
    Guid Id,
    string DisplayName,
    string Email,
    string Role
);