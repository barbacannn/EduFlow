using EduFlow.DataAccess.Entities;
using EduFlow.Services.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Services.Modules.Users;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken ct = default)
        => await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken ct = default)
        => await _userManager.Users.ToListAsync(ct);

    public async Task<ApplicationUser> CreateUserAsync(string email, string password, CancellationToken ct = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

        return user;
    }

    public async Task<bool> DeleteUserAsync(Guid id, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return false;

        var result = await _userManager.DeleteAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangeUserRoleAsync(Guid id, string role, CancellationToken ct = default)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return false;

        if (!await _roleManager.RoleExistsAsync(role))
        {
            var newRole = new IdentityRole<Guid>(role);
            await _roleManager.CreateAsync(newRole);
        }

        var result = await _userManager.AddToRoleAsync(user, role);
        return result.Succeeded;
    }
}
