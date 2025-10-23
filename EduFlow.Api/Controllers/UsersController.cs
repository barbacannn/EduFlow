using EduFlow.Api.Contracts.Users;
using EduFlow.DataAccess.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public UsersController(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct = default)
    {
        var users = await _userManager.Users
            .Select(u => new { u.Id, u.Email, u.DisplayName, u.RoleName })
            .ToListAsync(ct);

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct = default)
    {
        var user = await _userManager.Users
            .Where(u => u.Id == id)
            .Select(u => new UserDto(u.Id, u.UserName ?? "", u.Email ?? "", ""))
            .FirstOrDefaultAsync(ct);

        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest req)
    {
        var user = new ApplicationUser
        {
            UserName = req.Email,
            Email = req.Email,
            EmailConfirmed = true
        };

        var res = await _userManager.CreateAsync(user, req.Password);

        if (!res.Succeeded)
            return BadRequest(res.Errors.Select(e => $"{e.Code}: {e.Description}"));

        return Ok(new { user.Id, user.Email });
    }

    [HttpPut("{id:guid}/role")]
    public async Task<IActionResult> ChangeUserRole(Guid id, [FromBody] string roleName)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound("User not found");

        if (!await _roleManager.RoleExistsAsync(roleName))
            return BadRequest($"Role {roleName} does not exist");

        var res = await _userManager.AddToRoleAsync(user, roleName);
        if (!res.Succeeded)
            return BadRequest(res.Errors.Select(e => $"{e.Code}: {e.Description}"));

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserRequest req)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound();

        if (!string.IsNullOrWhiteSpace(req.DisplayName))
            user.DisplayName = req.DisplayName;

        if (!string.IsNullOrWhiteSpace(req.Email))
        {
            user.Email = req.Email;
            user.UserName = req.Email;
        }

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
            return BadRequest(result.Errors.Select(e => $"{e.Code}: {e.Description}"));

        if (!string.IsNullOrWhiteSpace(req.Password))
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var passResult = await _userManager.ResetPasswordAsync(user, token, req.Password);

            if (!passResult.Succeeded)
                return BadRequest(passResult.Errors.Select(e => $"{e.Code}: {e.Description}"));
        }

        return Ok(new { user.Id, user.DisplayName, user.Email });
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null) return NotFound("User not found");

        var res = await _userManager.DeleteAsync(user);
        if (!res.Succeeded)
            return BadRequest(res.Errors.Select(e => $"{e.Code}: {e.Description}"));

        return NoContent();
    }
}
