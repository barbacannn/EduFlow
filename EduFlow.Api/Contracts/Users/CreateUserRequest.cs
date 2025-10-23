using System.ComponentModel.DataAnnotations;

namespace EduFlow.Api.Contracts.Users;

public class CreateUserRequest
{
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required, MinLength(8)] public string Password { get; set; } = "";
}
