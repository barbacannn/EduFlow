namespace EduFlow.Api.Contracts.Courses;

public record CourseDto(
    Guid Id,
    string Code,
    string Name,
    Guid TeacherId,
    DateTimeOffset CreatedAtUtc,
    DateTimeOffset? ModifiedAtUtc
);