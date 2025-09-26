namespace EduFlow.Services.Abstractions;

public interface ICourseService
{
    Task<bool> PingAsync(CancellationToken ct = default);
}