using EduFlow.DataAccess.Entities;

namespace EduFlow.Services.Abstractions;

public interface ICourseService
{
    Task<(IReadOnlyList<Course> Items, int Total)> GetPagedAsync(
        int page, int pageSize, CancellationToken ct = default);

    Task<Course?> GetCourseById(Guid id, CancellationToken ct = default);

    Task<Course> CreateCourse(string code, string name, Guid teacherId, CancellationToken ct = default);

    Task<bool> UpdateCourse(Guid id, string code, string name, CancellationToken ct = default);

    Task<bool> SoftDeleteCourse(Guid id, CancellationToken ct = default);
}