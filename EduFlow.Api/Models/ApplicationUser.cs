using Microsoft.AspNetCore.Identity;

namespace EduFlow.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = "";
    public string RoleName { get; set; } = "Student";
}