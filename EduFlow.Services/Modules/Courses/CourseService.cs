using EduFlow.Services.Abstractions;

namespace EduFlow.Services.Modules.Courses;

public class CourseService : ICourseService
{
    public Task<bool> PingAsync(CancellationToken ct = default) => Task.FromResult(true);
}