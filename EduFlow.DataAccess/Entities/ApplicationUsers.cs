using Microsoft.AspNetCore.Identity;

namespace EduFlow.DataAccess.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string DisplayName { get; set; } = "";
    public string RoleName { get; set; } = "Student";
}