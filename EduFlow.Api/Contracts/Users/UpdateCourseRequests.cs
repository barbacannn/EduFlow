namespace EduFlow.Api.Contracts.Users;

public class UpdateUserRequest
{
    public string? DisplayName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}