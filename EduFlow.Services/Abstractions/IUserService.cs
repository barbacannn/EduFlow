namespace EduFlow.Services.Abstractions;

using EduFlow.DataAccess.Entities;

public interface IUserService
{
    Task<ApplicationUser?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<ApplicationUser>> GetAllUsersAsync(CancellationToken ct = default);
    Task<ApplicationUser> CreateUserAsync(string email, string password, CancellationToken ct = default);
    Task<bool> DeleteUserAsync(Guid id, CancellationToken ct = default);
    Task<bool> ChangeUserRoleAsync(Guid id, string role, CancellationToken ct = default);
}